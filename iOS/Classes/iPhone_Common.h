
#ifndef _TRAMPOLINE_IPHONE_COMMON_H_
#define _TRAMPOLINE_IPHONE_COMMON_H_

#include <stdarg.h>

//------------------------------------------------------------------------------

// magic: ensuring proper compiler/xcode/whatever selection
#ifndef __clang__
#error please use clang compiler.
#endif

// NOT the best way but apple do not care about adding extensions properly
#if __clang_major__ < 3
#error please use xcode 4.2 or newer
#endif

//------------------------------------------------------------------------------

// ios/sdk version

extern	bool	_ios43orNewer;
extern	bool	_ios50orNewer;
extern	bool	_ios60orNewer;
extern	bool	_ios70orNewer;

#ifdef __IPHONE_6_0
	#define UNITY_PRE_IOS6_SDK (__IPHONE_OS_VERSION_MAX_ALLOWED < __IPHONE_6_0)
#else
	#define UNITY_PRE_IOS6_SDK 1
#endif

#ifdef __IPHONE_6_0
	#define UNITY_PRE_IOS6_TARGET (__IPHONE_OS_VERSION_MIN_REQUIRED < __IPHONE_6_0)
#else
	#define UNITY_PRE_IOS6_TARGET 1
#endif


#ifdef __IPHONE_7_0
	#define UNITY_PRE_IOS7_SDK (__IPHONE_OS_VERSION_MAX_ALLOWED < __IPHONE_7_0)
#else
	#define UNITY_PRE_IOS7_SDK 1
#endif

#ifdef __IPHONE_7_0
	#define UNITY_PRE_IOS7_TARGET (__IPHONE_OS_VERSION_MIN_REQUIRED < __IPHONE_7_0)
#else
	#define UNITY_PRE_IOS7_TARGET 1
#endif


//------------------------------------------------------------------------------

typedef enum
{
	deviceUnknown = 0,
	deviceiPhone = 1,
	deviceiPhone3G = 2,
	deviceiPhone3GS = 3,
	deviceiPodTouch1Gen = 4,
	deviceiPodTouch2Gen = 5,
	deviceiPodTouch3Gen = 6,
	deviceiPad1Gen = 7,
	deviceiPhone4 = 8,
	deviceiPodTouch4Gen = 9,
	deviceiPad2Gen = 10,
	deviceiPhone4S = 11,
	deviceiPad3Gen = 12,
	deviceiPhone5 = 13,
	deviceiPodTouch5Gen = 14,
	deviceiPadMini1Gen = 15,
	deviceiPad4Gen = 16,
	deviceiPhoneUnknown = 10001,
	deviceiPadUnknown = 10002,
	deviceiPodTouchUnknown = 10003,
}
DeviceGeneration;

typedef enum
{
    orientationUnknown,
    portrait,
    portraitUpsideDown,
    landscapeLeft,
    landscapeRight,
    autorotation,
    orientationCount
}
ScreenOrientation;

struct UnityFrameStats;


typedef enum
{
	/// LogType used for Errors.
	LogType_Error = 0,
    /// LogType used for Asserts. (These indicate an error inside Unity itself.)
	LogType_Assert = 1,
    /// LogType used for Warnings.
	LogType_Warning = 2,
    /// LogType used for regular log messages.
	LogType_Log = 3,
    /// LogType used for Exceptions.
	LogType_Exception = 4,
    /// LogType used for Debug.
	LogType_Debug = 5,
	///
	LogType_NumLevels
}
LogType;

typedef void (*LogEntryHandler) (LogType logType, const char* log, va_list list);
void SetLogEntryHandler(LogEntryHandler newHandler);

//------------------------------------------------------------------------------

#define ENABLE_UNITY_DEBUG_LOG 0

#if ENABLE_UNITY_DEBUG_LOG
	#define UNITY_DBG_LOG(...)				\
		do 									\
		{									\
			printf_console(__VA_ARGS__);	\
		}									\
		while(0)
#else
	#define UNITY_DBG_LOG(...)				\
		do 									\
		{									\
		}									\
		while(0)
#endif // ENABLE_UNITY_DEBUG_LOG


#endif // _TRAMPOLINE_IPHONE_COMMON_H_
