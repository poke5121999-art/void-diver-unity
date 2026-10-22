using DG.Tweening;
using GameCommon.Enum;
using NemoLib.Log;
using Spine.Unity;
using UniRx;
using Unity.Netcode;
using UnityEngine;
using VContainer;

public class NpcObject : NetworkBase, IInteractiveObject
{
	public class Args
	{
		public bool ShowSpawnVfx;
	}

	[SerializeField]
	private int _npcId;

	[SerializeField]
	[Range(0f, 5f)]
	private float _holdingTime;

	[SerializeField]
	private string _holdingSfx;

	[SerializeField]
	private string _interactionSfx;

	[SerializeField]
	private UnitDissolveVisualEffect _dissolveEffect;

	[SerializeField]
	private GroundNavigation _navigation;

	private Args _args;

	public readonly ReactiveProperty<int> RxEmployeeId;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private GameObjectManager _objectManager;

	[Inject]
	private GameVfxManager _vfxManager;

	[Inject]
	private ToastManager _toastManager;

	[Inject]
	private SoundManager _soundManager;

	[Inject]
	private GameNpcDialogManager _npcDialogManager;

	[Inject]
	private GameEventSystem _eventSystem;

	[Inject]
	protected PlayerManager PlayerManager;

	[Inject]
	private SeriLogger _logger;

	private NetworkList<int> _syncNpcFunctionIds;

	private bool _isNavigationActive;

	private bool _isInteractableIndividually;

	private readonly NetworkVariable<bool> _syncShowSpawnVfx;

	private Tweener _tweener;

	private SkeletonAnimation _skeletonAnimation;

	public NetworkBase NetworkBase => null;

	public Vector3 Position => default;

	public float HoldingTime => 0f;

	public EOutlineType OutlineType => EOutlineType.None;

	public string HoldingSfx => null;

	public string InteractionSfx => null;

	public int NpcId => 0;

	public bool HasFunction => false;

	public int[] NpcFunctionIds => null;

	public ReactiveCollection<int> RxNpcFunctionIds { get; }

	public ReactiveProperty<ENpcState> RxNpcState { get; }

	public ENpcState NpcState => ENpcState.None;

	public Subject<string> OnShowBubbleText { get; }

	public Subject<string> OnShowMonologueText { get; }

	private bool ShowSpawnVfx => false;

	private bool HasDialog => false;

	ulong IInteractiveObject.NetworkObjectId => 0uL;

	public void Init(Args args)
	{
	}

	protected override void Awake()
	{
	}

	public override void OnDestroy()
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

	private void RegisterListEvents()
	{
	}

	private void UnregisterListEvents()
	{
	}

	private void OnSyncNpcFunctionIdsChanged(NetworkListEvent<int> changeEvent)
	{
	}

	private void OnEventOccured(IBaseEvent baseEvent)
	{
	}

	public void SetNavigationActive(bool isActive)
	{
	}

	private void OnCharacterSpawned(CharacterController characterController)
	{
	}

	public void DisableInteraction()
	{
	}

	public void EnableInteraction()
	{
	}

	public void SetSkin(string skinName)
	{
	}

	public void SetEmployeeId(int employeeId)
	{
	}

	public void PlayAnimation(string animationName, bool isLoop)
	{
	}

	public void SetSkeletonScaleX(float scaleX)
	{
	}

	bool IInteractiveObject.CanInteract(UnitModel sourceModel)
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

	protected override void __initializeVariables()
	{
	}

	protected override void __initializeRpcs()
	{
	}

	protected internal override string __getTypeName()
	{
		return null;
	}
}
