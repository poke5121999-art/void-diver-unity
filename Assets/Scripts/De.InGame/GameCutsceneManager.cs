using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.AssetManagement;
using NemoLib.Popup;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using VContainer;

public class GameCutsceneManager : MonoBase
{
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

	private readonly Dictionary<string, CutsceneTimelineObject> _preloadedCutscenes = new Dictionary<string, CutsceneTimelineObject>();

	private readonly Dictionary<int, Subject<Unit>> _pendingLuaSignals = new Dictionary<int, Subject<Unit>>();

	private readonly HashSet<int> _firedLuaSignals = new HashSet<int>();

	public static GameCutsceneManager Instance { get; private set; }

	public ReactiveProperty<bool> RxIsPlaying { get; } = new ReactiveProperty<bool>(false);

	public ReactiveProperty<float> RxSkipProgress { get; } = new ReactiveProperty<float>(0f);

	protected override void Awake()
	{
		base.Awake();
		Instance = this;
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	private void Start()
	{
		_gameChannel.Receive<ResPlayCutscene>().Subscribe(OnPlayCutscene).AddTo(this);
		_gameManager.RxStageState.Where((EStageState _) => _ == EStageState.SceneLoaded).Subscribe(delegate
		{
			OnSceneLoadedAsync().Forget();
		}).AddTo(this);
		_gameManager.RxStageState.Where((EStageState _) => _ == EStageState.Closed).Subscribe(delegate
		{
			FlushAllLuaSignals();
			ClearPreloadedCutscenes();
		}).AddTo(this);
		InputAction skipAction = _inputManager.CutsceneActions.Skip;
		Observable.FromEvent(delegate(Action<InputAction.CallbackContext> h)
		{
			skipAction.started += h;
		}, delegate(Action<InputAction.CallbackContext> h)
		{
			skipAction.started -= h;
		}).Subscribe(delegate
		{
			OnSkipStarted();
		}).AddTo(this);
		Observable.FromEvent(delegate(Action<InputAction.CallbackContext> h)
		{
			skipAction.canceled += h;
		}, delegate(Action<InputAction.CallbackContext> h)
		{
			skipAction.canceled -= h;
		}).Subscribe(delegate
		{
			OnSkipCanceled();
		}).AddTo(this);
	}

	public void RequestPlayCutscene(string cutscenePrefab, Vector3 position)
	{
		_gameChannel.SendToHost(new ReqPlayCutscene
		{
			CutscenePrefab = cutscenePrefab,
			Position = position.ToProtoVector3()
		});
	}

	private async UniTask OnSceneLoadedAsync()
	{
		_gameContext.RxIsCutscenePreloaded.Value = false;
		await PreloadCutsceneAsync();
		_gameContext.RxIsCutscenePreloaded.Value = true;
	}

	private UniTask PreloadCutsceneAsync()
	{
		ClearPreloadedCutscenes();
		if (_gameContext.CampaignId < 1)
		{
			return UniTask.CompletedTask;
		}
		string[] cutscenes = TCampaign.Table[_gameContext.CampaignId].Cutscenes;
		if (cutscenes != null)
		{
			foreach (string text in cutscenes)
			{
				CutsceneTimelineObject cutsceneTimelineObject = _assetManager.InstantiateCutscene(text);
				if (!(cutsceneTimelineObject == null))
				{
					cutsceneTimelineObject.GetComponent<PlayableDirector>().extrapolationMode = DirectorWrapMode.None;
					cutsceneTimelineObject.gameObject.SetActive(value: false);
					_preloadedCutscenes[text] = cutsceneTimelineObject;
				}
			}
		}
		return UniTask.CompletedTask;
	}

	public void ClearPreloadedCutscenes()
	{
		foreach (CutsceneTimelineObject value in _preloadedCutscenes.Values)
		{
			if (value != null)
			{
				UnityEngine.Object.Destroy(value.gameObject);
			}
		}
		_preloadedCutscenes.Clear();
	}

	private void OnSkipStarted()
	{
		_skipHoldDisposable?.Dispose();
		float startTime = Time.time;
		_skipHoldDisposable = Observable.EveryUpdate().Select((long _) => Mathf.Clamp01((Time.time - startTime) / 1f)).TakeWhile((float p) => p < 1f)
			.DoOnCompleted(delegate
			{
				RxSkipProgress.Value = 1f;
				_currentCutscene.StopTimeline();
			})
			.Subscribe(delegate(float p)
			{
				RxSkipProgress.Value = p;
			});
	}

	private void OnSkipCanceled()
	{
		_skipHoldDisposable?.Dispose();
		_skipHoldDisposable = null;
		RxSkipProgress.Value = 0f;
	}

	private void OnPlayCutscene(ResPlayCutscene message)
	{
		if (RxIsPlaying.Value)
		{
			return;
		}
		RxIsPlaying.Value = true;
		CutsceneTimelineObject value;
		bool isCurrentCutscenePreloaded;
		if (!_preloadedCutscenes.TryGetValue(message.CutscenePrefab, out value))
		{
			value = _assetManager.InstantiateCutscene(message.CutscenePrefab);
			if (message.Position != null)
			{
				value.transform.position = message.Position.ToVector3();
			}
			isCurrentCutscenePreloaded = false;
		}
		else
		{
			if (message.Position != null)
			{
				value.transform.position = message.Position.ToVector3();
			}
			value.gameObject.SetActive(value: true);
			isCurrentCutscenePreloaded = true;
		}
		_isCurrentCutscenePreloaded = isCurrentCutscenePreloaded;
		_inputManager.BlockInput();
		_inputManager.UnblockInput(EInputActionType.Cutscene);
		_currentCutscene = value;
		_popupManager.HideCanvas();
		_gameSoundManager.OnCutsceneStart();
		value.PlayTimeline();
	}

	public bool IsLuaSignalFired(int signalId)
	{
		return _firedLuaSignals.Contains(signalId);
	}

	public Subject<Unit> GetLuaSignalSubject(int signalId)
	{
		if (!_pendingLuaSignals.TryGetValue(signalId, out var value))
		{
			value = new Subject<Unit>();
			_pendingLuaSignals[signalId] = value;
		}
		return value;
	}

	public void EmitLuaSignal(int signalId)
	{
		_firedLuaSignals.Add(signalId);
		if (_pendingLuaSignals.TryGetValue(signalId, out var value))
		{
			_pendingLuaSignals.Remove(signalId);
			value?.OnNext(Unit.Default);
			value?.OnCompleted();
		}
	}

	private void FlushAllLuaSignals()
	{
		foreach (Subject<Unit> value in _pendingLuaSignals.Values)
		{
			value.OnNext(Unit.Default);
			value.OnCompleted();
		}
		_pendingLuaSignals.Clear();
		_firedLuaSignals.Clear();
	}

	public void FinishCutscene(CutsceneTimelineObject cutsceneTimelineObject)
	{
		if (RxIsPlaying.Value)
		{
			FlushAllLuaSignals();
			RxIsPlaying.Value = false;
			RxSkipProgress.Value = 0f;
			_currentCutscene = null;
			_skipHoldDisposable?.Dispose();
			_skipHoldDisposable = null;
			_popupManager.ShowCanvas();
			_inputManager.BlockInput(EInputActionType.Cutscene);
			_inputManager.UnblockInput();
			_gameSoundManager.OnCutsceneEnd();
			_gameChannel.SendToHost(new ReqFinishCutscene());
			if (_isCurrentCutscenePreloaded)
			{
				cutsceneTimelineObject.gameObject.SetActive(value: false);
			}
			else
			{
				UnityEngine.Object.Destroy(cutsceneTimelineObject.gameObject);
			}
			_isCurrentCutscenePreloaded = false;
		}
	}
}
