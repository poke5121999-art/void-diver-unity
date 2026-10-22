using System;
using GameCommon.Enum;
using GameCommon.Table;
using Unity.Netcode;
using SomeNet.Utility;
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

	private CompositeDisposable _disposable = new CompositeDisposable();

	public ulong SourceNetworkObjectId => SourceObject.NetworkObjectId;

	public IInteractiveObject SourceObject { get; private set; }

	public int WaveId { get; private set; }

	public Vector3 StartPosition { get; private set; }

	public float StartTime { get; private set; }

	public float FinishTime { get; private set; }

	public bool IsInProgress { get; private set; }

	private TWave WaveData => TWave.Table[WaveId];

	public float Duration => TWave.Table[WaveId].Duration;

	public Subject<bool> OnWaveFinished { get; } = new Subject<bool>();

	public GameWave(ulong sourceNetworkObjectId, int waveId)
	{
		if (!_objectManager.InteractiveObjects.TryGetValue(sourceNetworkObjectId, out var value))
		{
			throw new ArgumentException(string.Format("Source object with ID {0} not found", sourceNetworkObjectId));
		}
		SourceObject = value;
		WaveId = waveId;
	}

	public void StartWave(Vector3 startPosition)
	{
		_gameManager.AssertIfNotHost();
		if (IsInProgress)
		{
			return;
		}
		StartPosition = startPosition;
		IsInProgress = true;
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		EWaveClearCondition clearCondition = WaveData.ClearCondition;
		if (clearCondition == EWaveClearCondition.ProtectUnitForDuration)
		{
			if (_objectManager.TrySpawnExtraUnit(WaveData.ConditionTargetId, startPosition, out var controller))
			{
				_targetUnit = controller;
				_targetUnit.GetComponent<UnitModel>().RxState.Where((EActionState state) => state == EActionState.Dead).Subscribe(delegate
				{
					FinishWave(isClear: false);
				}).AddTo(_disposable);
				if (Duration > 0f)
				{
					Observable.Timer(TimeSpan.FromSeconds(Duration)).Subscribe(delegate
					{
						FinishWave(isClear: true);
					}).AddTo(_disposable);
				}
			}
			else
			{
				FinishWave(isClear: false);
			}
		}
		else if (clearCondition == EWaveClearCondition.SurviveForDuration && Duration > 0f)
		{
			Observable.Timer(TimeSpan.FromSeconds(Duration)).Subscribe(delegate
			{
				FinishWave(isClear: true);
			}).AddTo(_disposable);
		}
		if (WaveData.SpecialFieldId > 0)
		{
			float num = WaveData.SpecialFieldRange * 2f;
			_objectManager.SpawnSpecialField(SourceNetworkObjectId, WaveData.SpecialFieldId, startPosition, new Vector3(num, 1f, num));
		}
		WavePhase[] wavePhases = WaveData.WavePhases;
		for (int i = 0; i < wavePhases.Length; i++)
		{
			WavePhase wavePhase = wavePhases[i];
			Observable.Timer(TimeSpan.FromSeconds(wavePhase.StartDelay)).Subscribe(delegate
			{
				StartWavePhase(wavePhase);
			}).AddTo(_disposable);
		}
		_gameHost.SendToAll(new NtfWaveStarted
		{
			StartPosition = startPosition.ToProtoVector3(),
			StartTime = _gameManager.TimeNow,
			WaveId = WaveId,
			NetworkObjectId = SourceNetworkObjectId
		});
	}

	private void StartWavePhase(WavePhase wavePhase)
	{
		_gameManager.AssertIfNotHost();
		foreach (WaveSpawnData waveSpawnData in wavePhase.WaveSpawnDatas)
		{
			for (int i = 0; i < waveSpawnData.spawnCount; i++)
			{
				for (int j = 0; j < 50; j++)
				{
					Vector3 insideUnitSphere = UnityEngine.Random.insideUnitSphere;
					Vector3 normalized = new Vector3(insideUnitSphere.x, 0f, insideUnitSphere.z).normalized;
					float randomFloat = RandomUtility.GetRandomFloat(wavePhase.SpawnMinRange, wavePhase.SpawnMaxRange);
					Vector3 sourcePosition = StartPosition + normalized * randomFloat;
					if (UnityEngine.AI.NavMesh.SamplePosition(sourcePosition, out var hit, 1f, 1))
					{
						_objectManager.TrySpawnMonster(new MonsterSpawnData
						{
							IsRespawn = false,
							MonsterId = waveSpawnData.monsterId,
							SpawnPosition = hit.position,
							SpawnType = ESpawnType.Wave,
							WaveStartPosition = StartPosition
						}, isLightOn: false, waveSpawnData.buffIds);
						break;
					}
				}
			}
		}
	}

	public void FinishWave(bool isClear)
	{
		_gameManager.AssertIfNotHost();
		if (IsInProgress)
		{
			_gameHost.SendToAll(new NtfWaveFinished
			{
				NetworkObjectId = SourceNetworkObjectId,
				WaveId = WaveId,
				IsClear = isClear
			});
			IsInProgress = false;
			_disposable?.Dispose();
			_disposable = null;
			if (_targetUnit != null)
			{
				_targetUnit.GetComponent<NetworkObject>().Despawn();
				_targetUnit = null;
			}
			OnWaveFinished.OnNext(isClear);
		}
	}

	public void StartProgress(float startTime, Vector3 startPosition)
	{
		IsInProgress = true;
		StartTime = startTime;
		FinishTime = ((Duration > 0f) ? (Duration + startTime) : (-1f));
		StartPosition = startPosition;
	}

	public void Clear()
	{
		_disposable?.Dispose();
		_disposable = null;
		IsInProgress = false;
		StartTime = -1f;
		FinishTime = -1f;
	}
}
