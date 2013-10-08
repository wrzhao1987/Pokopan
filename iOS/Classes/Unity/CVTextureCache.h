#ifndef _TRAMPOLINE_UNITY_CVTEXTURECACHE_H_
#define _TRAMPOLINE_UNITY_CVTEXTURECACHE_H_

bool		CanUseCVTextureCache();
// returns CVOpenGLESTextureCacheRef
void*		CreateCVTextureCache();
// cache = CVOpenGLESTextureCacheRef
void		FlushCVTextureCache(void* cache);
// returns CVOpenGLESTextureRef
// cache = CVOpenGLESTextureCacheRef
// image = CVImageBufferRef
void*		CreateTextureFromCVTextureCache(void* cache, void* image, unsigned w, unsigned h, int format, int internalFormat, int type);
// texture = CVOpenGLESTextureRef
unsigned	GetGLTextureFromTextureCache(void* texture);

#endif // _TRAMPOLINE_UNITY_CVTEXTURECACHE_H_
