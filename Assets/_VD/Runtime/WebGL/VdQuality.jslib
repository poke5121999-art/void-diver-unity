// Web quality tier (WebQualityTier.cs): the GPU renderer string, to tell a software renderer (no graphics card:
// SwiftShader, llvmpipe, Microsoft Basic Render Driver) from a real GPU.
mergeInto(LibraryManager.library, {
  VdGpuRenderer: function () {
    var s = '';
    try {
      var gl = GLctx;
      var ext = gl.getExtension('WEBGL_debug_renderer_info');
      s = ext ? gl.getParameter(ext.UNMASKED_RENDERER_WEBGL) : gl.getParameter(gl.RENDERER);
    } catch (e) { s = 'unknown (' + e + ')'; }
    s = String(s || '');
    var size = lengthBytesUTF8(s) + 1, buf = _malloc(size);
    stringToUTF8(s, buf, size);
    return buf;
  }
});
