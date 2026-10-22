using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using NemoLib.AssetManagement;
using UnityEngine;
using VContainer;

public class SceneLoader : MonoBase
{
	[SerializeField]
	private string _sceneName;

	[Inject]
	private AssetManager _assetManager;

	private void Start()
	{
		LoadSceneContentAsync().Forget();
	}

	private async UniTaskVoid LoadSceneContentAsync()
	{
		GameObject original = await _assetManager.GetRecentlyCacheAsync<GameObject>("Scene", "Scene_" + _sceneName + "_prefab");
		if (!(this == null) && !(original == null))
		{
			Object.Instantiate(original, Vector3.zero, Quaternion.identity).transform.localScale = Vector3.one;
		}
	}
}
