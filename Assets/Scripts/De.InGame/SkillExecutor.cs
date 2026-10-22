using GameCommon.Data;
using GameCommon.Enum;
using NemoLib.Log;
using UniRx;
using Unity.Netcode;
using UnityEngine;
using VContainer;

public class SkillExecutor : NetworkBase, IInteractiveObject, IActionExecutable, IActionStats
{
	public class Args
	{
		public int SkillExecutorId;
	}

	private Args _args;

	[SerializeField]
	[Range(0f, 5f)]
	private float _holdingTime;

	[SerializeField]
	private string _holdingSfx;

	[SerializeField]
	private string _interactionSfx;

	[Inject]
	private GameObjectManager _objectManager;

	[Inject]
	private SeriLogger _logger;

	private OutlineObject _outlineObject;

	private readonly NetworkVariable<int> _syncSkillExecutorId;

	private readonly NetworkVariable<int> _syncRemainCount;

	private CompositeDisposable _disposable;

	public NetworkBase NetworkBase => null;

	public Vector3 Position => default;

	public float HoldingTime => 0f;

	public EOutlineType OutlineType => EOutlineType.None;

	public string HoldingSfx => null;

	public string InteractionSfx => null;

	public GameObjectManager ObjectManager => null;

	public GameObject GameObject => null;

	public int SkillExecutorId => 0;

	public int RemainCount => 0;

	public EElementalType ElementalType => EElementalType.None;

	public float WeaponAtkMultiplier => 0f;

	ulong IInteractiveObject.NetworkObjectId => 0uL;

	ulong IActionExecutable.NetworkObjectId => 0uL;

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

	private void OnRemainCountChanged(int oldValue, int newValue)
	{
	}

	public void Execute(CharacterModel casterModel)
	{
	}

	private void ExecuteActionEvent(ActionEvent actionEvent, HitLayerInfo hitLayerInfo)
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

	public float GetStat(EStatType statType)
	{
		return 0f;
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
