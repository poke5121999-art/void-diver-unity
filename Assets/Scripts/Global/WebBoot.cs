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

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void ServeBundlesAsText()
	{
		Addressables.InternalIdTransformFunc = location =>
			location.InternalId.EndsWith(".bundle") ? location.InternalId + CompressibleSuffix : location.InternalId;
	}

	private void Start()
	{
		BootAsync().Forget();
	}

	private static async UniTask BootAsync()
	{
		float start = Time.realtimeSinceStartup;
		Application.targetFrameRate = WebLoadingScreen.PreloadFrameRate;
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
