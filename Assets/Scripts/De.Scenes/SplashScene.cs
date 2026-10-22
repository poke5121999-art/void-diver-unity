using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using NemoLib.AssetManagement;
using NemoLib.Log;
using NemoLib.Mvp;
using NemoLib.Scene;
using UnityEngine.UI;
using UnityEngine.Video;
using VContainer;

public class SplashScene : BaseScenePresenter
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CAuthAsync_003Ed__14 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public SplashScene _003C_003E4__this;

		private UniTask<bool>.Awaiter _003C_003Eu__1;

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

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CStartAsync_003Ed__13 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public SplashScene _003C_003E4__this;

		private UniTask _003CpreloadTask_003E5__2;

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

	[View]
	private SplashSceneView _view;

	[Inject]
	private SeriLogger _logger;

	[Inject]
	private GraphicSettingService _graphicSettings;

	[Inject]
	private SoundSettingService _soundSettings;

	[Inject]
	private GamePlaySettingService _gamePlaySettings;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private EnvironmentSettings _environmentSettings;

	[Inject]
	private AppManager _appManager;

	[Inject]
	private GameEventSystem _eventSystem;

	private bool _loadingEnded;

	protected override Selectable SelectedObject => null;

	protected override void Start()
	{
	}

	[AsyncStateMachine(typeof(_003CStartAsync_003Ed__13))]
	private UniTask StartAsync()
	{
		return default;
	}

	[AsyncStateMachine(typeof(_003CAuthAsync_003Ed__14))]
	private UniTask AuthAsync()
	{
		return default;
	}

	private void OnSplashVideoEnd(VideoPlayer videoPlayer)
	{
	}
}
