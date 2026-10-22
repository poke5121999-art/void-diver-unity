using System.Collections.Generic;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.Log;
using Tracker;
using UniRx;
using Unity.Netcode;
using VContainer;

public class GameShopManager : MonoBase
{
	[Inject]
	private GameManager _gameManager;

	[Inject]
	private GameObjectManager _objectManager;

	[Inject]
	private SeriLogger _logger;

	[Inject]
	private NetworkManager _networkManager;

	[Inject]
	private GameHost _gameHost;

	[Inject]
	private GameChannel _gameChannel;

	private readonly HashSet<int> _existingShopGroupIds;

	private readonly TrackableDictionary<int, ShopProduct> _trackableProducts;

	private readonly Dictionary<int, ShopProduct> _products;

	public readonly Subject<IList<ShopProductChangeData>> OnShopProductChanged;

	private int _syncTick;

	private bool HasChange => false;

	private IDictionary<int, ShopProduct> Products => null;

	protected override void Awake()
	{
	}

	private void Start()
	{
	}

	private void FixedUpdate()
	{
	}

	private void OnNpcSpawned(NpcObject npc)
	{
	}

	private void OnJoined()
	{
	}

	private void OnStageClosed(EStageState stageState)
	{
	}

	private void Reset()
	{
	}

	private void Synchronize()
	{
	}

	public int[] GetShopProductIds(int groupId)
	{
		return null;
	}

	public ShopProduct GetShopProduct(int shopProductId)
	{
		return null;
	}

	public int GetShopProductStock(int shopProductId)
	{
		return 0;
	}

	public bool TryConsumeShopProductStock(int shopProductId, int count)
	{
		return false;
	}

	public bool HasEnoughShopProductStock(int shopProductId, int count)
	{
		return false;
	}

	private void ApplyProducts(ShopProduct[] products)
	{
	}

	private void OnSyncShop(ReqSyncShop message)
	{
	}

	private void OnUpdateShop(NtfUpdateShop message)
	{
	}

	private void OnSyncShop(ResSyncShop message)
	{
	}

	private int GetGradeAppearBonus(TShopProduct productData)
	{
		return 0;
	}
}
