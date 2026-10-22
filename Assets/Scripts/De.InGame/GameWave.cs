using GameCommon.Table;
using UniRx;
using UnityEngine;
using VContainer;

public class GameWave : NativeBase
{
	[Inject]
	private GameManager _gameManager;

	[Inject]
	private GameHost _gameHost;

	[Inject]
	private GameObjectManager _objectManager;

	private UnitController _targetUnit;

	private CompositeDisposable _disposable;

	public ulong SourceNetworkObjectId => 0uL;

	public IInteractiveObject SourceObject { get; private set; }

	public int WaveId { get; private set; }

	public Vector3 StartPosition { get; private set; }

	public float StartTime { get; private set; }

	public float FinishTime { get; private set; }

	public bool IsInProgress { get; private set; }

	private TWave WaveData => null;

	public float Duration => 0f;

	public Subject<bool> OnWaveFinished { get; }

	public GameWave(ulong sourceNetworkObjectId, int waveId)
	{
	}

	public void StartWave(Vector3 startPosition)
	{
	}

	private void StartWavePhase(WavePhase wavePhase)
	{
	}

	public void FinishWave(bool isClear)
	{
	}

	public void StartProgress(float startTime, Vector3 startPosition)
	{
	}

	public void Clear()
	{
	}
}
