using GameCommon.Data;
using GameCommon.Enum;
using NemoLib.AssetManagement;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class DropGoods : NetworkBase, IInteractiveObject
{
	[SerializeField]
	[Range(0f, 5f)]
	private float _holdingTime;

	[SerializeField]
	private Image _icon;

	[SerializeField]
	private GameObject _goQuestVfx;

	[SerializeField]
	private string _holdingSfx;

	[SerializeField]
	private string _interactionSfx;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private GameObjectManager _objectManager;

	private readonly NetworkVariable<EGoodsType> _syncGoodsType;

	private readonly NetworkVariable<int> _syncGoodsId;

	private readonly NetworkVariable<long> _syncBaseGoodsCount;

	private readonly NetworkVariable<long> _syncGainGoodsCount;

	public NetworkBase NetworkBase => null;

	public Vector3 Position => default;

	public float HoldingTime => 0f;

	public EOutlineType OutlineType => EOutlineType.None;

	public string HoldingSfx => null;

	public string InteractionSfx => null;

	public EGoodsType GoodsType => EGoodsType.None;

	public int GoodsId => 0;

	public long GoodsCount => 0L;

	public Goods OriginalGoods { get; private set; }

	public Goods DisplayGoods => null;

	ulong IInteractiveObject.NetworkObjectId => 0uL;

	public void Init(Goods goods)
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

	public void SetGoodsCount(long baseCount, long gainCount)
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
