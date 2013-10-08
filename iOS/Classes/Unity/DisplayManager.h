#ifndef _TRAMPOLINE_UNITY_DISPLAYMANAGER_H_
#define _TRAMPOLINE_UNITY_DISPLAYMANAGER_H_

#include "iPhone_Common.h"
#include "GlesHelper.h"

@class EAGLContext;
@class UnityView;

@interface DisplayConnection : NSObject
{
@public
    UIScreen*       screen;
    UIWindow*       window;
    UIView*         view;

    CGSize          screenSize;

    UnityRenderingSurface   surface;
}
- (id)init:(UIScreen*)targetScreen;
- (id)createView:(BOOL)useWithGles showRightAway:(BOOL)showRightAway;
- (id)createView:(BOOL)useWithGles;

- (void)dealloc;

- (void)createContext:(EAGLContext*)parent;
- (void)recreateSurface:(BOOL)use32bitColor use24bitDepth:(BOOL)use24bitDepth msaaSampleCount:(int)msaaSampleCount renderW:(int)renderW renderH:(int)renderH;
- (void)recreateSurface:(BOOL)use32bitColor use24bitDepth:(BOOL)use24bitDepth msaaSampleCount:(int)msaaSampleCount;
- (void)recreateSurface:(BOOL)use32bitColor use24bitDepth:(BOOL)use24bitDepth;
- (void)recreateSurface:(BOOL)use32bitColor;

- (void)requestRenderingResolution:(CGSize)res;

- (void)present;
@end


@interface DisplayManager : NSObject
{
    NSMutableDictionary*    displayConnection;
    DisplayConnection*      mainDisplay;
}
- (int)displayCount;
- (BOOL)displayAvailable:(UIScreen*)targetScreen;
- (DisplayConnection*)display:(UIScreen*)targetScreen;
- (DisplayConnection*)mainDisplay;

- (void)updateDisplayListInUnity;

- (void)presentAll;
- (void)presentAllButMain;

+ (void)Initialize;
+ (DisplayManager*)Instance;
@end

inline DisplayConnection* GetMainDisplay()
{
	return [[DisplayManager Instance] mainDisplay];
}

#endif // _TRAMPOLINE_UNITY_DISPLAYMANAGER_H_
