
#include "UnityView.h"
#include "iPhone_View.h"
#include "iPhone_OrientationSupport.h"
#include "Unity/UnityInterface.h"
#include "Unity/GlesHelper.h"
#include "Unity/DisplayManager.h"

extern void UnitySendTouchesBegin(NSSet* touches, UIEvent* event);
extern void UnitySendTouchesEnded(NSSet* touches, UIEvent* event);
extern void UnitySendTouchesCancelled(NSSet* touches, UIEvent* event);
extern void UnitySendTouchesMoved(NSSet* touches, UIEvent* event);


@implementation UnityView
{
	CGSize 				_surfaceSize;
	ScreenOrientation 	_curOrientation;

	BOOL				_recreateView;
}
- (id)initWithFrame:(CGRect)frame
{
	if( (self = [super initWithFrame:frame]) )
	{
		[self setMultipleTouchEnabled:YES];
		[self setExclusiveTouch:YES];

		_surfaceSize = frame.size;
	}
	return self;
}
- (void)layoutSubviews
{
	if (_surfaceSize.width != self.bounds.size.width || _surfaceSize.height != self.bounds.size.height)
	{
		_surfaceSize  = self.bounds.size;
		_recreateView = YES;
	}
}

- (void)willRotateTo:(ScreenOrientation)orientation
{
	_curOrientation = orientation;
	UnitySetScreenOrientation(_curOrientation);

	// please see comments in iPhone_View.mm about this function
	extern void UnityUpdateCurrentOrientationValue(ScreenOrientation);
	UnityUpdateCurrentOrientationValue(_curOrientation);
}
- (void)didRotate
{
	if(_recreateView)
	{
		// we are not inside repaint so we need to draw second time ourselves
		[self recreateGLESSurface];
		UnityPlayerLoop();
	}
}


- (ScreenOrientation)contentOrientation
{
	return _curOrientation;
}

- (void)recreateGLESSurfaceIfNeeded
{
	unsigned requestedW, requestedH;	UnityGetRenderingResolution(&requestedW, &requestedH);
	int requestedMSAA = UnityGetDesiredMSAASampleCount(MSAA_DEFAULT_SAMPLE_COUNT);

	if(		GetMainDisplay()->surface.use32bitColor != UnityUse32bitDisplayBuffer()
		||	GetMainDisplay()->surface.use24bitDepth != UnityUse24bitDepthBuffer()
		||	requestedW != GetMainDisplay()->surface.targetW || requestedH != GetMainDisplay()->surface.targetH
		||	(_supportsMSAA && requestedMSAA != GetMainDisplay()->surface.msaaSamples)
		||	_recreateView == YES
	  )
	{
		[self recreateGLESSurface];
	}
}

- (void)recreateGLESSurface
{
	extern bool _glesContextCreated;
	extern bool _unityLevelReady;
	extern bool _skipPresent;

	if(_glesContextCreated)
	{
		unsigned requestedW, requestedH;
		UnityGetRenderingResolution(&requestedW, &requestedH);

		[GetMainDisplay()	recreateSurface:UnityUse32bitDisplayBuffer() use24bitDepth:UnityUse24bitDepthBuffer()
							msaaSampleCount:UnityGetDesiredMSAASampleCount(MSAA_DEFAULT_SAMPLE_COUNT)
							renderW:requestedW renderH:requestedH
		];

		if(_unityLevelReady)
		{
			// seems like ios sometimes got confused about abrupt swap chain destroy
			// draw 2 times to fill both buffers
			// present only once to make sure correct image goes to CA
			// if we are calling this from inside repaint, second draw and present will be done automatically
			_skipPresent = true;
			{
				SetupUnityDefaultFBO(&GetMainDisplay()->surface);
				UnityPlayerLoop();
				UnityFinishRendering();
			}
			_skipPresent = false;
		}
	}

	_recreateView = NO;
}

- (void)touchesBegan:(NSSet*)touches withEvent:(UIEvent*)event
{
	UnitySendTouchesBegin(touches, event);
}
- (void)touchesEnded:(NSSet*)touches withEvent:(UIEvent*)event
{
	UnitySendTouchesEnded(touches, event);
}
- (void)touchesCancelled:(NSSet*)touches withEvent:(UIEvent*)event
{
	UnitySendTouchesCancelled(touches, event);
}
- (void)touchesMoved:(NSSet*)touches withEvent:(UIEvent*)event
{
	UnitySendTouchesMoved(touches, event);
}

@end
