using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using NemoLib.AssetManagement;
using NemoLib.Popup;
using UniRx;
using UnityEngine;
using VContainer;

public class GameCutsceneManager : MonoBase
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003COnSceneLoadedAsync_003Ed__28 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public GameCutsceneManager _003C_003E4__this;

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

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private GameHost _gameHost;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private GameContext _gameContext;

	[Inject]
	private GameSoundManager _gameSoundManager;

	[Inject]
	private GameInputManager _inputManager;

	[Inject]
	private PopupManager _popupManager;

	private bool _isCurrentCutscenePreloaded;

	private CutsceneTimelineObject _currentCutscene;

	private IDisposable _skipHoldDisposable;

	private const float SkipHoldDuration = 1f;

	private readonly Dictionary<string, CutsceneTimelineObject> _preloadedCutscenes;

	private readonly Dictionary<int, Subject<Unit>> _pendingLuaSignals;

	private readonly HashSet<int> _firedLuaSignals;

	public static GameCutsceneManager Instance { get; private set; }

	public ReactiveProperty<bool> RxIsPlaying { get; }

	public ReactiveProperty<float> RxSkipProgress { get; }

	protected override void Awake()
	{
	}

	private void Start()
	{
	}

	public void RequestPlayCutscene(string cutscenePrefab, Vector3 position)
	{
	}

	[AsyncStateMachine(typeof(_003COnSceneLoadedAsync_003Ed__28))]
	private UniTask OnSceneLoadedAsync()
	{
		return default;
	}

	private UniTask PreloadCutsceneAsync()
	{
		return default;
	}

	public void ClearPreloadedCutscenes()
	{
	}

	private void OnSkipStarted()
	{
	}

	private void OnSkipCanceled()
	{
	}

	private void OnPlayCutscene(ResPlayCutscene message)
	{
	}

	public bool IsLuaSignalFired(int signalId)
	{
		return false;
	}

	public Subject<Unit> GetLuaSignalSubject(int signalId)
	{
		return null;
	}

	public void EmitLuaSignal(int signalId)
	{
	}

	private void FlushAllLuaSignals()
	{
	}

	public void FinishCutscene(CutsceneTimelineObject cutsceneTimelineObject)
	{
	}
}
