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

	// While the loading screen covers the canvas nothing Unity draws is seen, and loading runs at the pace of frames:
	// draw one frame in HiddenRenderInterval until the title. Without a GPU (software WebGL) a frame of the splash scene
	// took about as long as a whole frame budget, so the 1793 'Login' prefabs of webgl-39 had not loaded after 7 min
	// (2026-11-05). Done draws every frame again.
	public const int HiddenRenderInterval = 60;

	public static void Hide()
	{
#if UNITY_WEBGL && !UNITY_EDITOR
		UnityEngine.Rendering.OnDemandRendering.renderFrameInterval = HiddenRenderInterval;
#endif
	}

	public static void Done()
	{
#if UNITY_WEBGL && !UNITY_EDITOR
		UnityEngine.Rendering.OnDemandRendering.renderFrameInterval = 1;
		VdLoadingDone();
#endif
	}
}
