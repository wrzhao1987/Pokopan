
#include "iPhone_Common.h"


#include <sys/types.h>
#include <sys/sysctl.h>

#if UNITY_PRE_IOS7_TARGET
	#include <sys/socket.h>
	#include <net/if.h>
	#include <net/if_dl.h>
	#include <CommonCrypto/CommonDigest.h>

	static void _InitDeviceIDPreIOS7();
#endif

#include "DisplayManager.h"


static NSString*	_DeviceID			= nil;
static NSString*	_ADID				= nil;
static bool			_AdTrackingEnabled	= false;
static NSString*	_VendorID			= nil;

static void QueryDeviceID();
static void QueryAdID();
static void QueryAdTracking();
static void QueryVendorID();

//
// unity interface
//

extern "C" const char*	UnityDeviceUniqueIdentifier()
{
	QueryDeviceID();
	return [_DeviceID UTF8String];
}
extern "C" const char*	UnityVendorIdentifier()
{
	QueryVendorID();
	return [_VendorID UTF8String];
}
extern "C" const char*	UnityAdvertisingIdentifier()
{
	QueryAdID();
	return [_ADID UTF8String];
}
extern "C" bool 		UnityAdvertisingTrackingEnabled()
{
	QueryAdTracking();
	return _AdTrackingEnabled;
}


//------------------------------------------------------------------------------
//
//

static void QueryDeviceID()
{
	if(_DeviceID == nil)
	{
	#if UNITY_PRE_IOS7_TARGET
		if(!_ios70orNewer)
			_InitDeviceIDPreIOS7();
	#endif

		// first check vendor id
		if(_DeviceID == nil)
		{
			QueryVendorID();
			_DeviceID = _VendorID;
		}

		// then ad id if smth went wrong
		if(_DeviceID == nil)
		{
			QueryAdID();
			_DeviceID = _ADID;
		}
	}
}

static id QueryASIdentifierManager()
{
	NSBundle* bundle = [NSBundle bundleWithPath:@"/System/Library/Frameworks/AdSupport.framework"];
	if(bundle)
	{
		[bundle load];
		Class retClass = [bundle classNamed:@"ASIdentifierManager"];
		if(		retClass
			&&	[retClass respondsToSelector:@selector(sharedManager)]
			&&	[retClass instancesRespondToSelector:@selector(advertisingIdentifier)]
			&&	[retClass instancesRespondToSelector:@selector(isAdvertisingTrackingEnabled)]
		  )
		{
			return [retClass performSelector:@selector(sharedManager)];
		}
	}

	return nil;
}

static void QueryAdID()
{
	// ad id can be reset during app lifetime
	id manager = QueryASIdentifierManager();
	if(manager)
		_ADID = (NSString*)[[[manager performSelector:@selector(advertisingIdentifier)] UUIDString] retain];
}

static void QueryAdTracking()
{
	// ad tracking can be changed during app lifetime
	id manager = QueryASIdentifierManager();
	if(manager)
		_AdTrackingEnabled = [manager performSelector:@selector(isAdvertisingTrackingEnabled)];
}

static void QueryVendorID()
{
	if(_VendorID == nil)
	{
		if([UIDevice instancesRespondToSelector:@selector(identifierForVendor)])
			_VendorID = (NSString*)[[[[UIDevice currentDevice] performSelector:@selector(identifierForVendor)] UUIDString] retain];
	}
}


//
// gritty stuff
//

#if UNITY_PRE_IOS7_TARGET
	static void _InitDeviceIDPreIOS7()
	{
		static const int MD5_DIGEST_LENGTH = 16;

		// macaddr: courtesy of FreeBSD hackers email list
		int mib[6] = { CTL_NET, AF_ROUTE, 0, AF_LINK, NET_RT_IFLIST, 0 };
		mib[5] = ::if_nametoindex("en0");

		size_t len = 0;
		::sysctl(mib, 6, NULL, &len, NULL, 0);

		char* buf = (char*)::malloc(len);
		::sysctl(mib, 6, buf, &len, NULL, 0);

		sockaddr_dl*   sdl = (sockaddr_dl*)((if_msghdr*)buf + 1);
		unsigned char* mac = (unsigned char*)LLADDR(sdl);

		char macaddr_str[18]={0};
		::sprintf(macaddr_str, "%02X:%02X:%02X:%02X:%02X:%02X", *mac, *(mac+1), *(mac+2), *(mac+3), *(mac+4), *(mac+5));
		::free(buf);

		unsigned char hash_buf[MD5_DIGEST_LENGTH];
		CC_MD5(macaddr_str, sizeof(macaddr_str)-1, hash_buf);

		char uid_str[MD5_DIGEST_LENGTH*2 + 1] = {0};
		for(int i = 0 ; i < MD5_DIGEST_LENGTH ; ++i)
			::sprintf(uid_str + 2*i, "%02x", hash_buf[i]);

		_DeviceID = [[NSString stringWithUTF8String:uid_str] retain];
	}
#endif
