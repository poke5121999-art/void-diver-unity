using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameCommon.Enum;
using SomeNet.Collections;
using NemoLib.Log;
using System.Linq;
using UniRx;
using UnityEngine;
using VContainer;

public class GameWaveManager : MonoBase
{
	[Inject]
	private GameManager _gameManager;

	[Inject]
	private GameObjectManager _objectManager;

	[Inject]
	private GameEventSystem _eventSystem;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private GameHost _gameHost;

	[Inject]
	private SeriLogger _logger;

	[Inject]
	private SoundManager _soundManager;

	private readonly Dictionary<ulong, GameWave> _gameWaves = new Dictionary<ulong, GameWave>();

	private CompositeDisposable _disposable;

	public ReactiveProperty<GameWave> RxCurrentWave { get; } = new ReactiveProperty<GameWave>();

	public GameWave CurrentWave => RxCurrentWave.Value;

	protected override void Awake()
	{
		base.Awake();
	}

	private void Start()
	{
		_gameChannel.Receive<NtfWaveStarted>().Subscribe(OnWaveStarted).AddTo(this);
		_gameChannel.Receive<NtfWaveFinished>().Subscribe(OnWaveFinished).AddTo(this);
		_gameManager.RxStageState.Where((EStageState state) => state == EStageState.Ended).Subscribe(delegate
		{
			OnStageFinished();
		}).AddTo(this);
		_gameManager.RxStageState.Where((EStageState state) => state == EStageState.Closed).Subscribe(delegate
		{
			OnStageClosed();
		}).AddTo(this);
		_gameManager.OnPlayerLeft.Where((GamePlayer player) => player.ClientId == _gameManager.LocalClientId).Subscribe(delegate
		{
			OnPlayerLeft();
		}).AddTo(this);
		_gameManager.RxViewTargetClientId.Subscribe(OnViewTargetChanged).AddTo(this);
	}

	private void OnViewTargetChanged(ulong clientId)
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		CharacterModel characterModel = _gameManager.FindCharacterModel(clientId);
		if (characterModel == null)
		{
			RxCurrentWave.Value = null;
			return;
		}
		RxCurrentWave.Value = GetNearestWaveInProgress(characterModel.Position);
		characterModel.RxPosition.Subscribe(OnCharacterPositionChanged).AddTo(_disposable);
	}

	private void OnCharacterPositionChanged(Vector3 position)
	{
		FindCurrentWave(position);
	}

	private void FindCurrentWave(Vector3 position)
	{
		RxCurrentWave.Value = GetNearestWaveInProgress(position);
	}

	public GameWave StartWave(ulong networkObjectId, int waveId, Vector3 startPosition)
	{
		_gameManager.AssertIfNotHost();
		GameWave orAddWave = GetOrAddWave(networkObjectId, waveId);
		orAddWave.StartWave(startPosition);
		return orAddWave;
	}

	public void FinishWave(ulong networkObjectId, bool isClear)
	{
		_gameManager.AssertIfNotHost();
		if (_gameWaves.TryGetValue(networkObjectId, out var value))
		{
			value.FinishWave(isClear);
		}
	}

	public bool IsWaveInProgress(ulong networkObjectId)
	{
		if (_gameWaves.TryGetValue(networkObjectId, out var value))
		{
			return value.IsInProgress;
		}
		return false;
	}

	private GameWave GetNearestWaveInProgress(Vector3 position)
	{
		if (!_gameWaves.Values.Any((GameWave wave) => wave.IsInProgress))
		{
			return null;
		}
		GameWave gameWave = _gameWaves.Values.Where((GameWave wave) => wave.IsInProgress).MinBy((GameWave wave) => Vector3.Distance(position, wave.StartPosition));
		if (gameWave != null && Vector3.Distance(position, gameWave.StartPosition) <= 12f)
		{
			return gameWave;
		}
		return null;
	}

	private GameWave GetOrAddWave(ulong networkObjectId, int waveId)
	{
		if (!_gameWaves.TryGetValue(networkObjectId, out var value))
		{
			value = new GameWave(networkObjectId, waveId);
			_gameWaves[networkObjectId] = value;
		}
		return value;
	}

	private void OnWaveStarted(NtfWaveStarted message)
	{
		_soundManager.PlaySfx("WaveStart");
		if (_objectManager.InteractiveObjects.ContainsKey(message.NetworkObjectId))
		{
			ProcessWaveStarted(message);
			return;
		}
		_objectManager.OnInteractiveObjectSpawned.Where((IInteractiveObject interactiveObject) => interactiveObject.NetworkObjectId == message.NetworkObjectId).Take(1).Subscribe(delegate
		{
			ProcessWaveStarted(message);
		})
			.AddTo(this);
	}

	private void ProcessWaveStarted(NtfWaveStarted message)
	{
		GetOrAddWave(message.NetworkObjectId, message.WaveId).StartProgress(message.StartTime, message.StartPosition.ToVector3());
		CharacterModel characterModel = _gameManager.FindCharacterModel(_gameManager.RxViewTargetClientId.Value);
		if (characterModel != null)
		{
			RxCurrentWave.Value = GetNearestWaveInProgress(characterModel.Position);
			UniTask.DelayFrame(5).ContinueWith(delegate
			{
				RxCurrentWave.Value = GetNearestWaveInProgress(characterModel.Position);
			});
		}
	}

	private void OnWaveFinished(NtfWaveFinished message)
	{
		if (_gameWaves.TryGetValue(message.NetworkObjectId, out var value))
		{
			_eventSystem.Trigger(new WaveFinishedEvent(value.WaveId, message.NetworkObjectId, message.IsClear));
			value.Clear();
			if (RxCurrentWave.Value == value)
			{
				RxCurrentWave.Value = null;
			}
		}
	}

	private void OnStageFinished()
	{
		if (_gameManager.IsHost)
		{
			_gameWaves.Values.ForEach(delegate(GameWave wave)
			{
				wave.FinishWave(isClear: false);
			});
		}
	}

	private void OnStageClosed()
	{
		Clear();
	}

	private void OnPlayerLeft()
	{
		Clear();
	}

	private void Clear()
	{
		_gameWaves.Clear();
		RxCurrentWave.Value = null;
	}
}
