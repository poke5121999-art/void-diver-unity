using System;
using System.Collections.Generic;
using System.Linq;
using GameCommon.Data;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.Log;
using SomeNet.Utility;
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

	private readonly HashSet<int> _existingShopGroupIds = new HashSet<int>();

	private readonly TrackableDictionary<int, ShopProduct> _trackableProducts = new TrackableDictionary<int, ShopProduct>();

	private readonly Dictionary<int, ShopProduct> _products = new Dictionary<int, ShopProduct>();

	public readonly Subject<IList<ShopProductChangeData>> OnShopProductChanged = new Subject<IList<ShopProductChangeData>>();

	private int _syncTick;

	private bool HasChange => _trackableProducts.HasChange;

	private IDictionary<int, ShopProduct> Products
	{
		get
		{
			if (!_gameManager.IsHost)
			{
				return _products;
			}
			return _trackableProducts;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	private void Start()
	{
		_objectManager.OnNpcSpawned.Subscribe(OnNpcSpawned).AddTo(this);
		_gameManager.RxStageState.Where((EStageState _) => _ == EStageState.Closed).Subscribe(OnStageClosed).AddTo(this);
		_gameHost.Receive<ReqSyncShop>().Subscribe(OnSyncShop).AddTo(this);
		_gameChannel.OnJoined.Subscribe(delegate
		{
			OnJoined();
		}).AddTo(this);
		_gameChannel.Receive<NtfUpdateShop>().Subscribe(OnUpdateShop).AddTo(this);
		_gameChannel.Receive<ResSyncShop>().Subscribe(OnSyncShop).AddTo(this);
	}

	private void FixedUpdate()
	{
		if (_gameManager.IsHost)
		{
			Synchronize();
		}
	}

	private void OnNpcSpawned(NpcObject npc)
	{
		if (!_gameManager.IsHost || npc.NpcFunctionIds == null || npc.NpcFunctionIds.Length == 0)
		{
			return;
		}
		npc.NpcFunctionIds.Select((int id) => TNpcFunction.Table[id]).Where((TNpcFunction f) => f.Type == ENpcFunctionType.Shop).ToList()
			.ForEach(delegate(TNpcFunction func)
			{
				if (_existingShopGroupIds.Add(func.Value1))
				{
					TShopProduct.GroupTable[func.Value1].Where((TShopProduct p) => p.IsEnabled && (p.AppearProbability >= 0 || p.StockCount >= 0)).Where(delegate(TShopProduct p)
					{
						if (p.AppearProbability < 0)
						{
							return true;
						}
						int gradeAppearBonus = GetGradeAppearBonus(p);
						return RandomUtility.GetRandomFloat() <= (float)(gradeAppearBonus + p.AppearProbability) / 10000f;
					}).Select((TShopProduct p) => new ShopProduct
					{
						Id = p.Id,
						Stock = p.StockCount,
						StockMax = p.StockCount
					}).ToList()
						.ForEach(delegate(ShopProduct p)
						{
							_trackableProducts.Add(p.Id, p);
						});
				}
			});
	}

	private void OnJoined()
	{
		_syncTick = 0;
		_gameChannel.SendToHost(new ReqSyncShop());
	}

	private void OnStageClosed(EStageState stageState)
	{
		Reset();
	}

	private void Reset()
	{
		if (_gameManager.IsHost)
		{
			_trackableProducts.ClearWithDirty();
		}
		_products.Clear();
		_existingShopGroupIds.Clear();
	}

	private void Synchronize()
	{
		_gameManager.AssertIfNotHost();
		if (HasChange)
		{
			ShopProduct[] array = _trackableProducts.Flush();
			ApplyProducts(array);
			_gameHost.SendToAllExceptHost(new NtfUpdateShop
			{
				Tick = (int)_networkManager.ServerTime.Tick,
				ChangeProducts = array
			});
		}
	}

	public int[] GetShopProductIds(int groupId)
	{
		return (from p in TShopProduct.GroupTable[groupId]
			where p.HasNoPurchaseLimit || Products.ContainsKey(p.Id)
			select p.Id).OrderBy((int id) => id).ToArray();
	}

	public ShopProduct GetShopProduct(int shopProductId)
	{
		TShopProduct tShopProduct = TShopProduct.Table[shopProductId];
		if (tShopProduct == null)
		{
			return null;
		}
		if (tShopProduct.HasNoPurchaseLimit)
		{
			return new ShopProduct
			{
				Id = tShopProduct.Id,
				Stock = tShopProduct.StockCount,
				StockMax = tShopProduct.StockCount
			};
		}
		if (!Products.TryGetValue(shopProductId, out var value))
		{
			return null;
		}
		return value;
	}

	public int GetShopProductStock(int shopProductId)
	{
		ShopProduct shopProduct = GetShopProduct(shopProductId);
		if (shopProduct == null)
		{
			return 0;
		}
		if (shopProduct.StockMax < 0)
		{
			return -1;
		}
		return shopProduct.Stock;
	}

	public bool TryConsumeShopProductStock(int shopProductId, int count)
	{
		_gameManager.AssertIfNotHost();
		TShopProduct tShopProduct = TShopProduct.Table[shopProductId];
		if (tShopProduct == null)
		{
			return false;
		}
		if (tShopProduct.HasNoPurchaseLimit)
		{
			return true;
		}
		ShopProduct shopProduct = GetShopProduct(shopProductId);
		if (shopProduct != null)
		{
			if (shopProduct.StockMax < 0)
			{
				return true;
			}
			if (count <= shopProduct.Stock)
			{
				shopProduct = shopProduct.Clone();
				shopProduct.Stock -= count;
				_trackableProducts[shopProductId] = shopProduct;
				return true;
			}
		}
		return false;
	}

	public bool HasEnoughShopProductStock(int shopProductId, int count)
	{
		ShopProduct shopProduct = GetShopProduct(shopProductId);
		if (shopProduct == null)
		{
			return false;
		}
		if (shopProduct.StockMax < 0)
		{
			return true;
		}
		return count <= shopProduct.Stock;
	}

	private void ApplyProducts(ShopProduct[] products)
	{
		if (products != null)
		{
			List<ShopProductChangeData> changeDatas = new List<ShopProductChangeData>();
			products.ForEach(delegate(ShopProduct p)
			{
				ShopProduct value = null;
				if (p.Operation == EDataOperation.Remove)
				{
					_products.TryGetValue(p.Id, out value);
					_products.Remove(p.Id);
				}
				ShopProduct newValue = null;
				if (p.Operation == EDataOperation.Add)
				{
					_products.Add(p.Id, p);
					newValue = p;
				}
				if (p.Operation == EDataOperation.Replace)
				{
					_products.TryGetValue(p.Id, out value);
					_products[p.Id] = p;
					newValue = p;
				}
				changeDatas.Add(new ShopProductChangeData
				{
					Operation = p.Operation,
					Key = p.Id,
					OldValue = value,
					NewValue = newValue
				});
			});
			OnShopProductChanged.OnNext(changeDatas);
		}
	}

	private void OnSyncShop(ReqSyncShop message)
	{
		_gameHost.SendToClient(message.SenderId, new ResSyncShop
		{
			Tick = (int)_networkManager.ServerTime.Tick,
			Products = Products.Values.ToArray()
		});
	}

	private void OnUpdateShop(NtfUpdateShop message)
	{
		if (_syncTick > 0 && _syncTick < message.Tick)
		{
			ApplyProducts(message.ChangeProducts);
		}
	}

	private void OnSyncShop(ResSyncShop message)
	{
		Products.Clear();
		if (message.Products != null)
		{
			message.Products.ForEach(delegate(ShopProduct p)
			{
				Products[p.Id] = p;
			});
		}
		_syncTick = message.Tick;
	}

	private int GetGradeAppearBonus(TShopProduct productData)
	{
		GamePlayer myPlayer = _gameManager.MyPlayer;
		if (myPlayer == null)
		{
			return 0;
		}
		EGoodsGradeType grade = productData.GainGoodsData.GetGrade();
		if (grade == EGoodsGradeType.None)
		{
			return 0;
		}
		return myPlayer.GetTalentEffects(ETalentEffectType.ShopProductGradeAppearBonus).Where((TalentEffect e) => e.Value1 == (int)grade).Sum((TalentEffect e) => e.Value2);
	}
}
