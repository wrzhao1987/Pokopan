
#include "CVTextureCache.h"

#ifdef __IPHONE_5_0

#include "iPhone_Common.h"
#include "GlesHelper.h"

#include <OpenGLES/ES2/gl.h>
#include <OpenGLES/ES2/glext.h>
#include <CoreVideo/CVOpenGLESTextureCache.h>

extern "C" const UnityRenderingSurface* UnityDisplayManager_MainDisplayRenderingSurface();


bool CanUseCVTextureCache()
{
	static bool _CanUseCVTextureCache = true;
	static bool _CanUseInited = false;

	if(!_CanUseInited)
	{
		if(!_ios50orNewer)
			_CanUseCVTextureCache = false;

		if(!IsRunningWithGLES2())
			_CanUseCVTextureCache = false;

		_CanUseInited = true;
	}

	return _CanUseCVTextureCache;
}

void* CreateCVTextureCache()
{
	if(!CanUseCVTextureCache())
		return 0;

	EAGLContext* context = UnityDisplayManager_MainDisplayRenderingSurface()->context;

	CVOpenGLESTextureCacheRef cache = 0;
	CVReturn err = CVOpenGLESTextureCacheCreate(kCFAllocatorDefault, 0, context, 0, &cache);
	if (err)
	{
		::printf_console("Error at CVOpenGLESTextureCacheCreate: %d", err);
		cache = 0;
	}

	return cache;
}

void FlushCVTextureCache(void* cache_)
{
	if(!CanUseCVTextureCache())
		return;

	CVOpenGLESTextureCacheRef	cache = (CVOpenGLESTextureCacheRef)cache_;
	if(cache == 0)
		return;

	CVOpenGLESTextureCacheFlush(cache, 0);
}

void* CreateTextureFromCVTextureCache(void* cache_, void* image_, unsigned w, unsigned h, int format, int internalFormat, int type)
{
	if(!CanUseCVTextureCache())
		return 0;

	CVOpenGLESTextureCacheRef	cache = (CVOpenGLESTextureCacheRef)cache_;
	CVImageBufferRef			image = (CVImageBufferRef)image_;
	if(!cache || !image)
		return 0;

	CVOpenGLESTextureRef texture = 0;
	CVReturn err = CVOpenGLESTextureCacheCreateTextureFromImage( kCFAllocatorDefault, cache, image, 0,
																 GL_TEXTURE_2D, (GLint)internalFormat,
																 w, h, (GLenum)format, (GLenum)type,
																 0, &texture
																);
	if (err)
	{
		::printf_console("Error at CVOpenGLESTextureCacheCreateTextureFromImage: %d", err);
		texture = 0;
	}

	return texture;
}

unsigned GetGLTextureFromTextureCache(void* texture_)
{
	if(!CanUseCVTextureCache())
		return 0;

	CVOpenGLESTextureRef texture = (CVOpenGLESTextureRef)texture_;
	if(texture == 0)
		return 0;

	return CVOpenGLESTextureGetName(texture);
}

#else

bool		CanUseCVTextureCache()																{ return false; }
void*       CreateCVTextureCache()																{ return 0; }
void		FlushCVTextureCache(void*)															{}
void*       CreateTextureFromCVTextureCache(void*, void*, unsigned, unsigned, int, int, int)	{ return 0; }
unsigned    GetGLTextureFromTextureCache(void*)													{ return 0; }

#endif // __IPHONE_5_0
