using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GameCommon.Data;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.AssetManagement;
using NemoLib.Log;
using SomeNet.Utility;
using Sirenix.Utilities;
using UniRx;
using Unity.Netcode;
using UnityEngine;
using VContainer;

public class RewardBox : NetworkBase, IInteractiveObject, IActionStats
{
	private enum AnimationState
	{
		None = 0,
		Locked = 1,
		Unlocked = 2,
		Opened = 3
	}

	public class Args
	{
		public int RewardBoxId;

		public Vector3 Forward;

		public int SlotCount;

		public IEnumerable<GoodsSlot> RespawnItemSlots;

		public ELockState LockState;

		public bool IsReplace;
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
	private string _holdingSfx;

	[SerializeField]
	private string _lockedHoldingSfx;

	[SerializeField]
	private string _interactionSfx;

	[Inject]
	private GameObjectManager _objectManager;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private SeriLogger _logger;

	[Inject]
	private GameContext _gameContext;

	[Inject]
	private GameMissionManager _missionManager;

	[Inject]
	private GameEventSystem _eventSystem;

	[Inject]
	private AssetManager _assetManager;

	private Args _args;

	private VisualProp _failVisual;

	private bool _isShowingFailVisual;

	private CompositeDisposable _viewTargetStressDisposable;

	private OutlineObject _outlineObject;

	private readonly NetworkVariable<Vector3> _syncForward = new NetworkVariable<Vector3>();

	private readonly NetworkVariable<int> _syncRewardBoxId = new NetworkVariable<int>();

	private readonly NetworkVariable<bool> _syncIsInteracting = new NetworkVariable<bool>();

	private readonly NetworkVariable<bool> _syncCanInteract = new NetworkVariable<bool>(true);

	private readonly NetworkVariable<bool> _syncIsReplace = new NetworkVariable<bool>();

	private readonly NetworkVariable<ELockState> _syncLockState = new NetworkVariable<ELockState>();

	private readonly NetworkVariable<bool> _syncHitBoxTriggerOnOpen = new NetworkVariable<bool>();

	private bool _isReplaced;

	public ReactiveProperty<bool> RxCanInteract = new ReactiveProperty<bool>(true);

	private EOutlineType _outlineType;

	public NetworkBase NetworkBase => this;

	public Vector3 Position => base.transform.position;

	public float HoldingTime => TableData.HoldingTime;

	public EOutlineType OutlineType
	{
		get
		{
			if (!_syncCanInteract.Value)
			{
				return EOutlineType.None;
			}
			if (_syncIsInteracting.Value)
			{
				return EOutlineType.None;
			}
			if (_isShowingFailVisual)
			{
				return EOutlineType.None;
			}
			return TableData.OutlineType;
		}
	}

	public string HoldingSfx
	{
		get
		{
			if (_syncLockState.Value != ELockState.Locked)
			{
				return string.IsNullOrEmpty(_holdingSfx) ? "DoorInteractied2" : _holdingSfx;
			}
			return string.IsNullOrEmpty(_lockedHoldingSfx) ? "DoorKey" : _lockedHoldingSfx;
		}
	}

	public string InteractionSfx => _interactionSfx;

	public int RewardBoxId => _syncRewardBoxId.Value;

	public TRewardBox TableData => TRewardBox.Table[_syncRewardBoxId.Value];

	public Vector3 Forward => _syncForward.Value;

	public LootingInventory LootingInventory { get; private set; }

	private int VariantDifficulty => _gameContext.VariantDifficulty;

	public ELockState LockState => _syncLockState.Value;

	public bool HitBoxTriggerOnOpen => _syncHitBoxTriggerOnOpen.Value;

	public EElementalType ElementalType => EElementalType.None;

	public float WeaponAtkMultiplier => 1f;

	ulong IInteractiveObject.NetworkObjectId => base.NetworkObjectId;

	public bool CheckStressCondition(EStressState stressState)
	{
		return TableData.InteractStressConditions.Contains(stressState);
	}

	protected override void Awake()
	{
		base.Awake();
		LootingInventory = GetComponent<LootingInventory>();
		_outlineObject = GetComponent<OutlineObject>();
	}

	public void Init(Args args)
	{
		if (base.IsSpawned)
		{
			_logger.Error("RewardBox.Init() should be called before spawning the object.");
		}
		_args = args;
	}

	private void Start()
	{
		_gameManager.RxStageState.Where((EStageState _) => _ == EStageState.SceneLoaded).Subscribe(delegate
		{
			TryRemoveReplacedTarget();
		}).AddTo(this);
	}

	public override void OnHostSpawn()
	{
		_syncForward.Value = _args.Forward;
		_syncRewardBoxId.Value = _args.RewardBoxId;
		_syncIsReplace.Value = _args.IsReplace;
	}

	public override void OnNetworkSpawn()
	{
		base.OnNetworkSpawn();
		TryRemoveReplacedTarget();
		_syncCanInteract.OnValueChanged += OnCanInteractChanged;
		_syncLockState.OnValueChanged += OnLockStateChanged;
		_syncIsInteracting.OnValueChanged += OnIsInteractingChanged;
		if (base.IsHost)
		{
			ELockState eLockState = ((_args.LockState != ELockState.None) ? _args.LockState : (TableData.HasKeyInteraction ? ELockState.Locked : ELockState.Closed));
			SetLockState(eLockState);
			List<GoodsSlot> list = _args.RespawnItemSlots?.ToList();
			if (list == null)
			{
				int dropRewardGroupId = TableData.DropRewardGroupId;
				List<Goods> source = ((dropRewardGroupId >= 1) ? TDropReward.Table.PickRewards(dropRewardGroupId, _gameContext.CampaignId, _missionManager.MissionIds.ToArray(), _gameContext.Difficulty, _gameContext.GetPartyWeaponTypes()) : new List<Goods>());
				list = source.Select((Goods goods, int index) => new GoodsSlot
				{
					SlotIndex = index,
					Goods = goods,
					IsUnrevealed = true
				}).ToList();
			}
			int slotCount = ((_args.SlotCount < 0) ? list.Count : _args.SlotCount);
			LootingInventory.Init(list, Math.Max(slotCount, 1));
			if (_syncLockState.Value == ELockState.Closed && RandomUtility.GetRandomBoolFromPercentage(TableData.HitboxTriggerPercent))
			{
				_syncHitBoxTriggerOnOpen.Value = true;
			}
			if (_syncLockState.Value == ELockState.Opened && LootingInventory.IsEmpty)
			{
				_syncCanInteract.Value = false;
			}
		}
		ForceApplyOutline();
		_objectManager.OnInteractiveObjectSpawned.OnNext(this);
		LookAtForward();
		base.gameObject.name = string.Format("[RewardBox] {0}", _syncRewardBoxId.Value);
		InitFailVisual();
	}

	public override void OnNetworkDespawn()
	{
		_objectManager.OnInteractiveObjectDespawned.OnNext(this);
		_syncCanInteract.OnValueChanged -= OnCanInteractChanged;
		_syncLockState.OnValueChanged -= OnLockStateChanged;
		_syncIsInteracting.OnValueChanged -= OnIsInteractingChanged;
		_viewTargetStressDisposable?.Dispose();
		_viewTargetStressDisposable = null;
		base.OnNetworkDespawn();
	}

	private void OnStageSceneLoaded()
	{
		TryRemoveReplacedTarget();
	}

	private void OnCanInteractChanged(bool oldValue, bool newValue)
	{
		RxCanInteract.Value = newValue;
		UpdateOutline();
	}

	private void OnIsInteractingChanged(bool oldValue, bool newValue)
	{
		UpdateOutline();
	}

	private void OnLockStateChanged(ELockState oldValue, ELockState newValue)
	{
		if (newValue == ELockState.None)
		{
			_logger.Error(string.Format("LockState must not be None. RewardBoxId: {0}", _syncRewardBoxId.Value));
		}
		if (!_isShowingFailVisual)
		{
			SetAnimation(ToAnimationState(newValue));
		}
		if (base.IsHost && oldValue == ELockState.Closed && newValue == ELockState.Opened)
		{
			TryActivateTrap();
		}
	}

	public bool IsAllRevealed()
	{
		return LootingInventory.IsAllRevealed();
	}

	private void LookAtForward()
	{
		InGameUtility.LookAtForward(_meshType, _syncForward.Value, _frontObject, _backObject);
	}

	private AnimationState ToAnimationState(ELockState lockState)
	{
		switch (lockState)
		{
		case ELockState.Locked:
		case ELockState.PermanentlyLocked:
			return AnimationState.Locked;
		case ELockState.Closed:
			return AnimationState.Unlocked;
		case ELockState.Opened:
			return AnimationState.Opened;
		default:
			return AnimationState.None;
		}
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
			if (!(_frontAnimator != null))
			{
				return;
			}
			animator = _frontAnimator;
			break;
		default:
			return;
		}
		animator.SetInteger("State", (int)state);
	}

	public bool CanInteract(UnitModel sourceModel)
	{
		return _syncCanInteract.Value && !_syncIsInteracting.Value && !_isShowingFailVisual;
	}

	void IInteractiveObject.OnStartHolding(ulong sourceClientId)
	{

	}

	void IInteractiveObject.OnStartInteraction(ulong sourceClientId)
	{
		if (base.IsHost)
		{
			_syncIsInteracting.Value = true;
		}
		if (_syncLockState.Value == ELockState.Closed)
		{
			if (_gameManager.IsInStage)
			{
				_eventSystem.Trigger(new OpenRewardBoxEvent(sourceClientId, _syncRewardBoxId.Value));
			}
			if (base.IsHost)
			{
				if (!_syncHitBoxTriggerOnOpen.Value)
				{
					CharacterModel sourceModel = _gameManager.FindCharacterModel(sourceClientId);
					LootingInventory.StartLooting(sourceModel);
				}
				else
				{
					_syncIsInteracting.Value = false;
					CharacterController characterController = _gameManager.FindCharacterController(sourceClientId);
					if (characterController != null)
					{
						characterController.HandleUnitEvent(new CancelInteractEvent
						{
							NetworkObjectId = base.NetworkObjectId
						});
					}
				}
				UniTask.DelayFrame(1).ContinueWith(delegate
				{
					AssertIfNotHost();
					_syncLockState.Value = ELockState.Opened;
				}).Forget();
			}
		}
		if (_syncLockState.Value == ELockState.Opened && base.IsHost)
		{
			CharacterModel sourceModel2 = _gameManager.FindCharacterModel(sourceClientId);
			LootingInventory.StartLooting(sourceModel2);
		}
	}

	void IInteractiveObject.OnEndInteraction(ulong sourceClientId)
	{
		if (base.IsHost)
		{
			_syncIsInteracting.Value = false;
			if (_syncLockState.Value == ELockState.Opened && LootingInventory.IsEmpty)
			{
				_syncCanInteract.Value = false;
			}
			if (_syncLockState.Value == ELockState.Opened)
			{
				LootingInventory.EndLooting(sourceClientId);
			}
		}
	}

	private bool TryRemoveReplacedTarget()
	{
		if (!_syncIsReplace.Value || _isReplaced)
		{
			return false;
		}
		if (!PhysicsUtility.Raycast(base.transform.position, "InteractableObject", out var hitObject))
		{
			return false;
		}
		Transform transform = ((hitObject != null) ? hitObject.transform.parent : null);
		if (transform == null)
		{
			return false;
		}
		UnityEngine.Object.Destroy(transform.gameObject);
		_isReplaced = true;
		return true;
	}

	private void TryActivateTrap()
	{
		AssertIfNotHost();
		if (!_syncHitBoxTriggerOnOpen.Value)
		{
			return;
		}
		Vector3 direction = _syncForward.Value;
		int[] hitBoxIds = TableData.HitBoxIds;
		if (hitBoxIds != null)
		{
			hitBoxIds.Where((int id) => 0 < id).ForEach(delegate(int hitBoxId)
			{
				_objectManager.SpawnHitBoxAsync(hitBoxId, null, this.ToStatsData(), new HitLayerInfo { HitLayer = 4000, FactionId = 0 }, direction, base.transform.position).Forget();
			});
		}
		_syncHitBoxTriggerOnOpen.Value = false;
	}

	public IEnumerable<GoodsSlot> GetSlots()
	{
		return LootingInventory.Slots.Values.Where((GoodsSlot slot) => slot != null).Where((GoodsSlot slot) => !slot.IsEmpty).ToList();
	}

	public int GetSlotCount()
	{
		return LootingInventory.SlotCount;
	}

	public void SetLockState(ELockState lockState)
	{
		AssertIfNotHost();
		_syncLockState.Value = lockState;
	}

	private void InitFailVisual()
	{
		string stressFailPrefabName = TableData.StressFailPrefabName;
		if (!string.IsNullOrEmpty(stressFailPrefabName))
		{
			_failVisual = _assetManager.InstantiateVisualProp(stressFailPrefabName);
			_failVisual.transform.SetParent(base.transform, worldPositionStays: false);
			_failVisual.gameObject.SetActive(false);
			_gameManager.RxViewTargetClientId.Subscribe(OnViewTargetChangedForVisual).AddTo(this);
		}
	}

	private void OnViewTargetChangedForVisual(ulong clientId)
	{
		_viewTargetStressDisposable?.Dispose();
		CharacterModel characterModel = _gameManager.FindCharacterModel(clientId);
		if (!(characterModel == null))
		{
			_viewTargetStressDisposable = new CompositeDisposable();
			characterModel.RxStressState.Subscribe(OnStressChangedForVisual).AddTo(_viewTargetStressDisposable);
		}
	}

	private void OnStressChangedForVisual(EStressState stressState)
	{
		EStressState[] interactStressConditions = TableData.InteractStressConditions;
		if (interactStressConditions == null || TableData.InteractStressConditions.Contains(stressState))
		{
			SwapToDefaultVisual();
		}
		else
		{
			SwapToFailVisual();
		}
	}

	private void SwapToDefaultVisual()
	{
		if (_isShowingFailVisual)
		{
			_failVisual.gameObject.SetActive(false);
			_isShowingFailVisual = false;
			LookAtForward();
			SetAnimation(ToAnimationState(_syncLockState.Value));
			UpdateOutline();
		}
	}

	private void SwapToFailVisual()
	{
		if (!_isShowingFailVisual)
		{
			_frontObject.SetActive(false);
			_backObject.SetActive(false);
			_failVisual.gameObject.SetActive(true);
			_failVisual.LookAtForward(_syncForward.Value);
			_isShowingFailVisual = true;
			UpdateOutline();
		}
	}

	private void UpdateOutline()
	{
		if (_outlineType != OutlineType)
		{
			ForceApplyOutline();
		}
	}

	private void ForceApplyOutline()
	{
		_outlineType = OutlineType;
		_outlineObject.SetColor(OutlineType.ToColor());
		_outlineObject.SetStroke(OutlineType.ToStroke());
	}

	public float GetStat(EStatType statType)
	{
		if (statType != EStatType.Atk)
		{
			return 0f;
		}
		return TableData.GetBaseAtkStat(_gameContext.Difficulty, _gameContext.VariantDifficulty);
	}



}
