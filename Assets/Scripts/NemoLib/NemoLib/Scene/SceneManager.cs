using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UniRx;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace NemoLib.Scene
{
	public class SceneManager : NativeBase
	{
		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CLoadSceneAsync_003Ed__11 : IAsyncStateMachine
		{
			public int _003C_003E1__state;

			public AsyncUniTaskMethodBuilder<bool> _003C_003Et__builder;

			public SceneManager _003C_003E4__this;

			public string sceneName;

			public LoadSceneMode mode;

			private AsyncOperationHandle<SceneInstance> _003Chandle_003E5__2;

			private UniTask.Awaiter _003C_003Eu__1;

			private void MoveNext()
			{
			}

			void IAsyncStateMachine.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				this.MoveNext();
			}

			[DebuggerHidden]
			private void SetStateMachine(IAsyncStateMachine stateMachine)
			{
			}

			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
				this.SetStateMachine(stateMachine);
			}
		}

		public readonly ReactiveProperty<float> Progress;

		public readonly Subject<string> RxSceneLoad;

		public readonly Subject<string> RxSceneLoaded;

		public readonly Subject<string> RxSceneUnloaded;

		public readonly Subject<string> RxSceneLoadFailed;

		public readonly ReactiveProperty<float> RxProgress;

		private string _loadedScene;

		private BaseScenePresenter _currentPresenter;

		public bool IsInputEnable => false;

		public void LoadScene(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
		{
		}

		[AsyncStateMachine(typeof(_003CLoadSceneAsync_003Ed__11))]
		public UniTask<bool> LoadSceneAsync(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
		{
			return default;
		}

		public void OnSceneStarted(BaseScenePresenter presenter)
		{
		}
	}
}
