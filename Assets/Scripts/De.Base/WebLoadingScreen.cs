#if UNITY_WEBGL && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif

// The web build's index.html shows a loading screen over the canvas from the first byte (Assets/_VD/Runtime/WebGL/
// VdLoading.jslib, WebGLBuild.AddLoadingScreen); the title calls Done to lift it. Elsewhere Done does nothing.
public static class WebLoadingScreen
{
	// Loading from bundles in the web player takes several frames per asset whatever the frame's spare time (342 'Login'
	// prefabs: about 1900 frames, 36 s at 56 fps and 26 s at 90 fps, 2026-10-30). Any targetFrameRate other than -1
	// moves Unity's main loop from requestAnimationFrame to setTimeout, so behind the loading screen it runs as fast as
	// the browser lets it; SplashScene gives the graphic settings' rate back once the title's assets are in.
	public const int PreloadFrameRate = 1000;

#if UNITY_WEBGL && !UNITY_EDITOR
	[DllImport("__Internal")]
	private static extern void VdLoadingDone();
#endif

	public static void Done()
	{
#if UNITY_WEBGL && !UNITY_EDITOR
		VdLoadingDone();
#endif
	}
}
