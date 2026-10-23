using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.Log;
using SomeNet.Collections;
using System;
using UniRx;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;
using VContainer;

public class MimicObject : NetworkBase, IInteractiveObject, IActionStats
{
	private enum EQteZone
	{
		Cancel = 0,
		Success = 1,
		Fail = 2
	}

	private enum AnimationState
	{
		None = 0,
		Idle = 1,
		Triggered = 2
	}

	public class Args
	{
		public int MimicId;

		public Vector3 Forward;
	}

	private class QteHoldingState
	{
		public float StartTime;

		public CompositeDisposable Disposable;
	}

	[SerializeField]
	private EMeshType _meshType;

	[SerializeField]
	private GameObject _frontObject;

	[SerializeField]
	private GameObject _backObject;

	[SerializeField]
	private Animator _frontAnimator;

	[SerializeField]
	private Animator _backAnimator;

	[SerializeField]
	private string _interactionSfx;

	[Inject]
	private GameObjectManager _objectManager;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private SoundManager _soundManager;

	[Inject]
	private SeriLogger _logger;

	private readonly NetworkVariable<int> _syncMimicId = new NetworkVariable<int>();

	private readonly NetworkVariable<Vector3> _syncForward = new NetworkVariable<Vector3>();

	private readonly NetworkVariable<bool> _syncMonsterSpawnTriggered = new NetworkVariable<bool>();

	private Args _args;

	private readonly Dictionary<ulong, QteHoldingState> _qteStates = new Dictionary<ulong, QteHoldingState>();

	private CompositeDisposable _zoneSfxDisposable;

	private EQteZone _currentZone;

	private OutlineObject _outlineObject;

	public NetworkBase NetworkBase => this;

	public Vector3 Position => base.transform.position;

	public float HoldingTime => TableData.HoldingTime;

	public EOutlineType OutlineType
	{
		get
		{
			if (!_syncMonsterSpawnTriggered.Value)
			{
				return EOutlineType.Default;
			}
			return EOutlineType.None;
		}
	}

	public string HoldingSfx => TableData.QteCancelZoneSfx;

	public string InteractionSfx => _interactionSfx;

	public int MimicId => _syncMimicId.Value;

	public Vector3 Forward => _syncForward.Value;

	public TMimic TableData => TMimic.Table[_syncMimicId.Value];

	public EElementalType ElementalType => EElementalType.None;

	public float WeaponAtkMultiplier => 1f;

	ulong IInteractiveObject.NetworkObjectId => base.NetworkObjectId;

	protected override void Awake()
	{
		base.Awake();
		_outlineObject = GetComponent<OutlineObject>();
	}

	public void Init(Args args)
	{
		if (base.IsSpawned)
		{
			_logger.Error("Mimic.Init() should be called before spawning the object.");
		}
		else
		{
			_args = args;
		}
	}

	public override void OnHostSpawn()
	{
		base.OnHostSpawn();
		_syncMimicId.Value = _args.MimicId;
		_syncForward.Value = _args.Forward;
	}

	public override void OnNetworkSpawn()
	{
		base.OnNetworkSpawn();
		_syncMonsterSpawnTriggered.OnValueChanged += OnMonsterSpawnTriggeredChanged;
		_objectManager.OnInteractiveObjectSpawned.OnNext(this);
		base.gameObject.name = string.Format("[Mimic] {0}", _syncMimicId.Value);
		UpdateOutline();
		InGameUtility.LookAtForward(_meshType, _syncForward.Value, _frontObject, _backObject);
		SetAnimation(_syncMonsterSpawnTriggered.Value ? AnimationState.Triggered : AnimationState.Idle);
	}

	public override void OnNetworkDespawn()
	{
		_objectManager.OnInteractiveObjectDespawned.OnNext(this);
		_syncMonsterSpawnTriggered.OnValueChanged -= OnMonsterSpawnTriggeredChanged;
		foreach (QteHoldingState value in _qteStates.Values)
		{
			if (value.Disposable != null)
			{
				value.Disposable.Dispose();
			}
		}
		_qteStates.Clear();
		StopZoneSfxTracking();
		base.OnNetworkDespawn();
	}

	private void OnMonsterSpawnTriggeredChanged(bool oldValue, bool newValue)
	{
		UpdateOutline();
		SetAnimation((AnimationState)(1 + (newValue ? 1 : 0)));
	}

	private void UpdateOutline()
	{
		_outlineObject.SetColor(OutlineType.ToColor());
		_outlineObject.SetStroke(OutlineType.ToStroke());
	}

	private void LookAtForward()
	{
		InGameUtility.LookAtForward(_meshType, _syncForward.Value, _frontObject, _backObject);
	}

	private void SetAnimation(AnimationState state)
	{
		Animator animator;
		switch (_meshType)
		{
		case EMeshType.D25:
		{
			Vector3 vector = MathUtility.SnapToCardinalDirection(_syncForward.Value);
			animator = ((vector == Vector3.forward || vector == Vector3.left) ? _frontAnimator : _backAnimator);
			if (!(animator != null))
			{
				return;
			}
			break;
		}
		case EMeshType.D3:
			animator = _frontAnimator;
			if (!(animator != null))
			{
				return;
			}
			break;
		default:
			return;
		}
		animator.SetInteger("State", (int)state);
	}

	bool IInteractiveObject.CanInteract(UnitModel sourceModel)
	{
		return !_syncMonsterSpawnTriggered.Value;
	}

	void IInteractiveObject.OnStartHolding(ulong sourceClientId)
	{
		StartZoneSfxTracking(sourceClientId);
		if (!base.IsHost)
		{
			return;
		}
		if (_qteStates.TryGetValue(sourceClientId, out var value) && value.Disposable != null)
		{
			value.Disposable.Dispose();
		}
		QteHoldingState qteHoldingState = new QteHoldingState
		{
			StartTime = _gameManager.TimeNow,
			Disposable = new CompositeDisposable()
		};
		_qteStates[sourceClientId] = qteHoldingState;
		CharacterModel characterModel = _gameManager.FindCharacterModel(sourceClientId);
		if (!(characterModel == null))
		{
			characterModel.RxState.Pairwise().Where((Pair<EActionState> p) => p.Previous == EActionState.Interact && p.Current != EActionState.Interact).Take(1)
				.Subscribe(delegate
				{
					OnInteractStateChanged(sourceClientId);
				})
				.AddTo(qteHoldingState.Disposable);
		}
	}

	void IInteractiveObject.OnStartInteraction(ulong sourceClientId)
	{
	}

	void IInteractiveObject.OnEndInteraction(ulong sourceClientId)
	{
		if (sourceClientId == _gameManager.RxViewTargetClientId.Value)
		{
			StopZoneSfxTracking();
		}
	}

	private void OnInteractStateChanged(ulong sourceClientId)
	{
		if (!_qteStates.TryGetValue(sourceClientId, out var value))
		{
			return;
		}
		float ratio = Mathf.Clamp01((_gameManager.TimeNow - value.StartTime) / TableData.HoldingTime);
		EQteZone eQteZone = ResolveResult();
		if (value.Disposable != null)
		{
			value.Disposable.Dispose();
		}
		_qteStates.Remove(sourceClientId);
		if (eQteZone == EQteZone.Cancel || _syncMonsterSpawnTriggered.Value)
		{
			return;
		}
		_syncMonsterSpawnTriggered.Value = true;
		switch (eQteZone)
		{
		case EQteZone.Success:
			SpawnMonster();
			break;
		case EQteZone.Fail:
			SpawnMonster();
			SpawnHitBoxes();
			break;
		}
		DespawnMimicDelayedAsync().Forget();
		EQteZone ResolveResult()
		{
			if (ratio < TableData.QteCancelZoneEndRatio)
			{
				return EQteZone.Cancel;
			}
			if (!(TableData.QteSuccessZoneEndRatio <= ratio))
			{
				return EQteZone.Success;
			}
			return EQteZone.Fail;
		}
	}

	private void StartZoneSfxTracking(ulong sourceClientId)
	{
		if (sourceClientId == _gameManager.RxViewTargetClientId.Value)
		{
			StopZoneSfxTracking();
			_zoneSfxDisposable = new CompositeDisposable();
			_currentZone = EQteZone.Cancel;
			float startTime = _gameManager.TimeNow;
			Observable.EveryUpdate().Subscribe(delegate
			{
				UpdateZoneSfx(startTime);
			}).AddTo(_zoneSfxDisposable);
		}
	}

	private void UpdateZoneSfx(float startTime)
	{
		float num = Mathf.Clamp01((_gameManager.TimeNow - startTime) / TableData.HoldingTime);
		EQteZone eQteZone = ((num < TableData.QteCancelZoneEndRatio) ? EQteZone.Cancel : ((!(TableData.QteSuccessZoneEndRatio <= num)) ? EQteZone.Success : EQteZone.Fail));
		string text = null;
		if (eQteZone != _currentZone)
		{
			_currentZone = eQteZone;
			if (eQteZone == EQteZone.Success)
			{
				text = TableData.QteSuccessZoneSfx;
			}
			else if (eQteZone == EQteZone.Fail)
			{
				text = TableData.QteFailZoneSfx;
			}
			if (!string.IsNullOrEmpty(text))
			{
				_soundManager.PlaySfx(text);
			}
		}
	}

	private void StopZoneSfxTracking()
	{
		if (_zoneSfxDisposable != null)
		{
			_zoneSfxDisposable.Dispose();
		}
		_zoneSfxDisposable = null;
		_currentZone = EQteZone.Cancel;
	}

	private void SpawnMonster()
	{
		if (TableData.MonsterId >= 1)
		{
			DisableNavMeshObstacle();
			SpawnMonsterDelayedAsync(TableData.MonsterId, base.transform.position, base.NetworkObjectId).Forget();
		}
	}

	private async UniTask SpawnMonsterDelayedAsync(int monsterId, Vector3 spawnPosition, ulong summonerNetworkObjectId)
	{
		await UniTask.Delay(TimeSpan.FromSeconds(0.25));
		_objectManager.TrySpawnMonster(new MonsterSpawnData
		{
			MonsterId = monsterId,
			SpawnPosition = spawnPosition,
			IsRespawn = false,
			SpawnType = ESpawnType.SummonActionEvent,
			SummonerNetworkObjectId = summonerNetworkObjectId
		});
	}

	private void DisableNavMeshObstacle()
	{
		if (TryGetComponent<NavMeshObstacle>(out var component))
		{
			component.enabled = false;
		}
	}

	private async UniTask DespawnMimicDelayedAsync()
	{
		await UniTask.Delay(TimeSpan.FromSeconds(0.1), ignoreTimeScale: false, PlayerLoopTiming.Update, base.destroyCancellationToken);
		if (base.NetworkObject != null && base.NetworkObject.IsSpawned)
		{
			base.NetworkObject.Despawn();
		}
	}

	private void SpawnHitBoxes()
	{
		TableData.HitBoxIds?.Where((int id) => id > 0).ForEach(delegate(int id)
		{
			_objectManager.SpawnHitBoxAsync(id, null, this.ToStatsData(), new HitLayerInfo
			{
				HitLayer = 4000
			}, _syncForward.Value, base.transform.position).Forget();
		});
	}

	public float GetStat(EStatType statType)
	{
		if (statType != EStatType.Atk)
		{
			return 0f;
		}
		return TableData.HitBoxAtk;
	}
}
