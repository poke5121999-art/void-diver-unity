using System.Collections.Generic;
using NemoLib.Log;
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

	private readonly Dictionary<ulong, GameWave> _gameWaves;

	private CompositeDisposable _disposable;

	public ReactiveProperty<GameWave> RxCurrentWave { get; }

	public GameWave CurrentWave => null;

	protected override void Awake()
	{
	}

	private void Start()
	{
	}

	private void OnViewTargetChanged(ulong clientId)
	{
	}

	private void OnCharacterPositionChanged(Vector3 position)
	{
	}

	private void FindCurrentWave(Vector3 position)
	{
	}

	public GameWave StartWave(ulong networkObjectId, int waveId, Vector3 startPosition)
	{
		return null;
	}

	public void FinishWave(ulong networkObjectId, bool isClear)
	{
	}

	public bool IsWaveInProgress(ulong networkObjectId)
	{
		return false;
	}

	private GameWave GetNearestWaveInProgress(Vector3 position)
	{
		return null;
	}

	private GameWave GetOrAddWave(ulong networkObjectId, int waveId)
	{
		return null;
	}

	private void OnWaveStarted(NtfWaveStarted message)
	{
	}

	private void ProcessWaveStarted(NtfWaveStarted message)
	{
	}

	private void OnWaveFinished(NtfWaveFinished message)
	{
	}

	private void OnStageFinished()
	{
	}

	private void OnStageClosed()
	{
	}

	private void OnPlayerLeft()
	{
	}

	private void Clear()
	{
	}
}
