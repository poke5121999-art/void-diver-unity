using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine.AddressableAssets;
using VContainer.Unity;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace NemoLib.Scene
{
	public class SceneManager : NativeBase
	{
		public readonly ReactiveProperty<float> Progress = new ReactiveProperty<float>();

		public readonly Subject<string> RxSceneLoad = new Subject<string>();

		public readonly Subject<string> RxSceneLoaded = new Subject<string>();

		public readonly Subject<string> RxSceneUnloaded = new Subject<string>();

		public readonly Subject<string> RxSceneLoadFailed = new Subject<string>();

		public readonly ReactiveProperty<float> RxProgress = new ReactiveProperty<float>();

		private string _loadedScene;

		private BaseScenePresenter _currentPresenter;

		public SceneManager()
		{
			this.InjectFromRootContainer();
		}

		public bool IsInputEnable => _currentPresenter != null && _currentPresenter.IsInputEnable;

		public void LoadScene(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
		{
			LoadSceneAsync(sceneName, mode).Forget();
		}

		public async UniTask<bool> LoadSceneAsync(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
		{
			RxProgress.Value = 0f;
			RxSceneLoad.OnNext(sceneName);
			AsyncOperationHandle<SceneInstance> handle = Addressables.LoadSceneAsync(sceneName, mode, activateOnLoad: true, 100);
			while (!handle.IsDone)
			{
				RxProgress.Value = handle.PercentComplete;
				await UniTask.DelayFrame(1);
			}
			if (handle.Status == AsyncOperationStatus.Succeeded)
			{
				if (!string.IsNullOrEmpty(_loadedScene))
				{
					RxSceneUnloaded.OnNext(_loadedScene);
					await UniTask.DelayFrame(1);
				}
				_loadedScene = sceneName;
				RxSceneLoaded.OnNext(sceneName);
				return true;
			}
			RxSceneLoadFailed.OnNext(sceneName);
			return false;
		}

		public void OnSceneStarted(BaseScenePresenter presenter)
		{
			_currentPresenter = presenter;
		}
	}
}
