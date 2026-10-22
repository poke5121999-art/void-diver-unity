using System.Collections.Generic;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.AssetManagement;
using NemoLib.Log;
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

	private readonly NetworkVariable<Vector3> _syncForward;

	private readonly NetworkVariable<int> _syncRewardBoxId;

	private readonly NetworkVariable<bool> _syncIsInteracting;

	private readonly NetworkVariable<bool> _syncCanInteract;

	private readonly NetworkVariable<bool> _syncIsReplace;

	private readonly NetworkVariable<ELockState> _syncLockState;

	private readonly NetworkVariable<bool> _syncHitBoxTriggerOnOpen;

	private bool _isReplaced;

	public ReactiveProperty<bool> RxCanInteract;

	private EOutlineType _outlineType;

	public NetworkBase NetworkBase => null;

	public Vector3 Position => default;

	public float HoldingTime => 0f;

	public EOutlineType OutlineType => EOutlineType.None;

	public string HoldingSfx => null;

	public string InteractionSfx => null;

	public int RewardBoxId => 0;

	public TRewardBox TableData => null;

	public Vector3 Forward => default;

	public LootingInventory LootingInventory { get; private set; }

	private int VariantDifficulty => 0;

	public ELockState LockState => ELockState.None;

	public bool HitBoxTriggerOnOpen => false;

	public EElementalType ElementalType => EElementalType.None;

	public float WeaponAtkMultiplier => 0f;

	ulong IInteractiveObject.NetworkObjectId => 0uL;

	public bool CheckStressCondition(EStressState stressState)
	{
		return false;
	}

	protected override void Awake()
	{
	}

	public void Init(Args args)
	{
	}

	private void Start()
	{
	}

	public override void OnHostSpawn()
	{
	}

	public override void OnNetworkSpawn()
	{
	}

	public override void OnNetworkDespawn()
	{
	}

	private void OnStageSceneLoaded()
	{
	}

	private void OnCanInteractChanged(bool oldValue, bool newValue)
	{
	}

	private void OnIsInteractingChanged(bool oldValue, bool newValue)
	{
	}

	private void OnLockStateChanged(ELockState oldValue, ELockState newValue)
	{
	}

	public bool IsAllRevealed()
	{
		return false;
	}

	private void LookAtForward()
	{
	}

	private AnimationState ToAnimationState(ELockState lockState)
	{
		return AnimationState.None;
	}

	private void SetAnimation(AnimationState state)
	{
	}

	public bool CanInteract(UnitModel sourceModel)
	{
		return false;
	}

	void IInteractiveObject.OnStartHolding(ulong sourceClientId)
	{
	}

	void IInteractiveObject.OnStartInteraction(ulong sourceClientId)
	{
	}

	void IInteractiveObject.OnEndInteraction(ulong sourceClientId)
	{
	}

	private bool TryRemoveReplacedTarget()
	{
		return false;
	}

	private void TryActivateTrap()
	{
	}

	public IEnumerable<GoodsSlot> GetSlots()
	{
		return null;
	}

	public int GetSlotCount()
	{
		return 0;
	}

	public void SetLockState(ELockState lockState)
	{
	}

	private void InitFailVisual()
	{
	}

	private void OnViewTargetChangedForVisual(ulong clientId)
	{
	}

	private void OnStressChangedForVisual(EStressState stressState)
	{
	}

	private void SwapToDefaultVisual()
	{
	}

	private void SwapToFailVisual()
	{
	}

	private void UpdateOutline()
	{
	}

	private void ForceApplyOutline()
	{
	}

	public float GetStat(EStatType statType)
	{
		return 0f;
	}



}
