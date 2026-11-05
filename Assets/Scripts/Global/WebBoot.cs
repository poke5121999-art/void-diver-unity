using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using VContainer.Unity;

// First scene of the web build only (Assets/_VD/Editor/WebGLBuild.cs adds it in front of the game's scenes). The web
// player cannot load Addressables synchronously, so this loads everything RootLifetimeScope.Configure needs and the
// StreamingAssets files before the game's first scene, then opens that scene.
public class WebBoot : MonoBehaviour
{
	// Must match WebGLBuild.CompressibleSuffix: the build names every bundle "<name>.bundle.txt". GitHub Pages sends
	// no Content-Encoding for files compressed ahead, but gzips text types on the fly; as .bundle (octet-stream) the
	// LZ4 bundles went over the wire as they are, as .txt about 17% smaller. The browser undoes the gzip before
	// UnityWebRequest sees the bytes.
	public const string CompressibleSuffix = ".txt";

#if UNITY_WEBGL && !UNITY_EDITOR
	// Web player only: this runs before any scene on every platform, and the Standalone and editor builds keep the
	// plain ".bundle" names.
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void ServeBundlesAsText()
	{
		Addressables.InternalIdTransformFunc = location =>
			location.InternalId.EndsWith(".bundle") ? location.InternalId + CompressibleSuffix : location.InternalId;
		// Web diagnostics (NemoLib.Log.WebDiag): every bundle download with its URL, result and time, and every failed
		// Addressables operation with its name, before Addressables' own exception log.
		Addressables.WebRequestOverride = NemoLib.Log.WebDiag.TrackRequest;
		UnityEngine.ResourceManagement.ResourceManager.ExceptionHandler = (handle, exception) =>
		{
			NemoLib.Log.WebDiag.Error("Addressables", string.Format("{0} failed: {1}", handle.IsValid() ? handle.DebugName : "(released handle)", exception));
			Addressables.LogException(handle, exception);
		};
	}
#endif

	private void Start()
	{
		BootAsync().Forget();
	}

#if UNITY_WEBGL && !UNITY_EDITOR
	[System.Runtime.InteropServices.DllImport("__Internal")]
	private static extern int VdRestoreSaves();
#else
	private static int VdRestoreSaves() => 0;
#endif

	private static async UniTask BootAsync()
	{
		float start = Time.realtimeSinceStartup;
		// Before anything reads a save: the last web autosave's copy in localStorage, when IndexedDB missed it because
		// the tab closed during the write (PlayerManager web autosave).
		int restored = VdRestoreSaves();
		if (restored != 0)
		{
			Debug.LogWarning(string.Format("[WebBoot] {0} save files restored from the localStorage copy", restored));
		}
		Application.targetFrameRate = WebLoadingScreen.PreloadFrameRate;
		WebLoadingScreen.Hide();
		var root = VContainerSettings.Instance != null ? VContainerSettings.Instance.RootLifetimeScope as RootLifetimeScope : null;
		if (root == null)
		{
			Debug.LogError("[WebBoot] VContainerSettings has no RootLifetimeScope prefab; the game cannot start");
			return;
		}
		await UniTask.WhenAll(RootLifetimeScope.PreloadAsync(root), StreamingAssetsFiles.PrepareAsync());
		Debug.Log(string.Format("[WebBoot] ready in {0:F1}s, opening scene 1", Time.realtimeSinceStartup - start));
		SceneManager.LoadScene(1);
	}
}
