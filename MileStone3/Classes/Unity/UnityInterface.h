
#ifndef _TRAMPOLINE_UNITY_UNITYINTERFACE_H_
#define _TRAMPOLINE_UNITY_UNITYINTERFACE_H_

// unity interface

// player life time
void UnityInitApplication(const char* appPathName);
void UnityLoadApplication();
void UnityPlayerLoop();
void UnityFinishRendering();
void UnityPause(bool pause);

// resolution
extern void	UnityRequestRenderingResolution(unsigned w, unsigned h);
extern void	UnityGetRenderingResolution(unsigned* w, unsigned* h);

// orientation
bool				UnityIsOrientationEnabled(EnabledOrientation orientation);
ScreenOrientation	UnityRequestedScreenOrientation();
void				UnitySetScreenOrientation(ScreenOrientation orientation);
bool				UnityUseAnimatedAutorotation();


// player settings
extern bool UnityUse32bitDisplayBuffer();
extern bool UnityUse24bitDepthBuffer();
extern int	UnityGetDesiredMSAASampleCount(int defaultSampleCount);



#endif // _TRAMPOLINE_UNITY_UNITYINTERFACE_H_
