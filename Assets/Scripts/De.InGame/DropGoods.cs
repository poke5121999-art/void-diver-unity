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

	private readonly NetworkVariable<EGoodsType> _syncGoodsType = new NetworkVariable<EGoodsType>();

	private readonly NetworkVariable<int> _syncGoodsId = new NetworkVariable<int>();

	private readonly NetworkVariable<long> _syncBaseGoodsCount = new NetworkVariable<long>();

	private readonly NetworkVariable<long> _syncGainGoodsCount = new NetworkVariable<long>();

	public NetworkBase NetworkBase => this;

	public Vector3 Position => base.transform.position;

	public float HoldingTime => _holdingTime;

	public EOutlineType OutlineType => EOutlineType.Default;

	public string HoldingSfx => _holdingSfx;

	public string InteractionSfx => _interactionSfx;

	public EGoodsType GoodsType => _syncGoodsType.Value;

	public int GoodsId => _syncGoodsId.Value;

	public long GoodsCount => _syncBaseGoodsCount.Value + _syncGainGoodsCount.Value;

	public Goods OriginalGoods { get; private set; }

	public Goods DisplayGoods
	{
		get
		{
			if (!base.IsSpawned)
			{
				return null;
			}
			return GoodsCreator.MakeGoods(_syncGoodsType.Value, _syncGoodsId.Value, _syncBaseGoodsCount.Value, _syncGainGoodsCount.Value);
		}
	}

	ulong IInteractiveObject.NetworkObjectId => base.NetworkObjectId;

	public void Init(Goods goods)
	{
		if (goods != null)
		{
			OriginalGoods = goods.Clone();
		}
	}

	public override void OnHostSpawn()
	{
		_syncGoodsType.Value = OriginalGoods.Type;
		_syncGoodsId.Value = OriginalGoods.Id;
		_syncBaseGoodsCount.Value = OriginalGoods.BaseCount;
		_syncGainGoodsCount.Value = OriginalGoods.GainCount;
	}

	public override void OnNetworkSpawn()
	{
		base.OnNetworkSpawn();
		_icon.sprite = _assetManager.GetIconGoods(_syncGoodsType.Value, _syncGoodsId.Value);
		_goQuestVfx.SetActive(DisplayGoods != null && DisplayGoods.IsEventItem());
		_objectManager.OnInteractiveObjectSpawned.OnNext(this);
	}

	public override void OnNetworkDespawn()
	{
		_objectManager.OnInteractiveObjectDespawned.OnNext(this);
		base.OnNetworkDespawn();
	}

	public void SetGoodsCount(long baseCount, long gainCount)
	{
		_syncBaseGoodsCount.Value = baseCount;
		_syncGainGoodsCount.Value = gainCount;
		if (OriginalGoods.Category == EGoodsCategory.Item)
		{
			OriginalGoods = GoodsCreator.MakeItemGoods(OriginalGoods.Id, baseCount, gainCount);
		}
	}

	bool IInteractiveObject.CanInteract(UnitModel sourceModel)
	{
		return true;
	}

	void IInteractiveObject.OnStartHolding(ulong sourceClientId)
	{
	}

	void IInteractiveObject.OnStartInteraction(ulong sourceClientId)
	{
	}

	void IInteractiveObject.OnEndInteraction(ulong sourceClientId)
	{
		if (base.IsHost && GoodsCount <= 0)
		{
			Despawn(0f, destroy: true);
		}
	}



}
