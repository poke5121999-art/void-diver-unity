// The web build's loading screen (WebGLBuild adds it to index.html) covers the page from the first byte to the title
// screen. WebLoadingScreen.Done() calls this when the title shows.
mergeInto(LibraryManager.library, {
  VdLoadingDone: function () {
    if (typeof window.vdLoadingDone === 'function') window.vdLoadingDone();
  },
});
