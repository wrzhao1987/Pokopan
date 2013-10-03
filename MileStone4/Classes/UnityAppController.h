#import <UIKit/UIKit.h>
#include "iPhone_Common.h"

// it is the unity rendering view class
// if you want custom view logic, you should subclass UnityView
@class UnityView;

@interface UnityAppController : NSObject<UIAccelerometerDelegate, UIApplicationDelegate>
{
	UnityView*			_unityView;

	UIView*				_rootView;
	UIViewController*	_rootController;
}
// this one is called at the very end of didFinishLaunchingWithOptions:, after view hierarchy been created
- (void)startUnity:(UIApplication*)application;
// this is one is passed to CADisplayLink: unity frame processing
- (void)repaintDisplayLink;

// override this only if you need customized unityview
- (UnityView*)initUnityViewImpl;

// override this to tweak unity view hierarchy
// _unityView will be inited
// you need to init _rootView and _rootController
- (void)createViewHierarchyImpl;

// you should not override these methods in usual case
- (UnityView*)initUnityView;
- (void)createViewHierarchy;
- (void)showGameUI:(UIWindow*)window;

// in general this method just works, so override it only if you have very special reorientation logic
- (void)onForcedOrientation:(ScreenOrientation)orient;

@property (readonly, copy, nonatomic) UnityView*		unityView;
@property (readonly, copy, nonatomic) UIView*			rootView;
@property (readonly, copy, nonatomic) UIViewController*	rootViewController;

@end

// Put this into mm file with your subclass implementation
// pass subclass name to define

#define IMPL_APP_CONTROLLER_SUBCLASS(ClassName)	\
@interface ClassName(OverrideAppDelegate)		\
{												\
}												\
+(void)load;									\
@end											\
@implementation ClassName(OverrideAppDelegate)	\
+(void)load										\
{												\
	extern const char* AppControllerClassName;	\
	AppControllerClassName = #ClassName;		\
}												\
@end											\

inline UnityAppController*	GetAppController()
{
	return (UnityAppController*)[UIApplication sharedApplication].delegate;
}
