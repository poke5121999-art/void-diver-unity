using GameCommon.Table;
using GameCommon.Data;
using System;
using DG.Tweening;
using GameCommon.Enum;
using NemoLib.Log;
using NemoLib.Mvp;
using UniRx;
using UnityEngine.UI;
using VContainer;

public class InGameQuickSlotPresenter : BaseSlotPresenter<InGameQuickSlotModel>
{
	[InjectModel]
	private InGameQuickSlotModel _model;

	[View]
	private InGameQuickSlotView _view;

	[Inject]
	private SeriLogger _logger;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private GameManager _gameManager;

	private Tweener _tweener;

	private GoodsSlotModel _goodsSlotModel;

	private CompositeDisposable _coolTimeDisposable;

	private int _itemId;

	protected override InGameQuickSlotModel SlotModel => _model;

	protected override Button Button => _view.GoodsSlot_Slot;

	private EConsumableType ConsumableType => TItem.Table[_itemId].ConsumableType;

	protected override void OnDestroy()
	{
		base.OnDestroy();
		_tweener?.Kill();
		_coolTimeDisposable?.Dispose();
	}

	private void OnInject(InGameQuickSlotModel model)
	{
		InitSlot(model);
		_tweener?.Kill();
		_tweener = null;
		RefreshGoodsSlot();
		_playerManager.RxQuickSlotItemIds.ObserveReplace().Where((CollectionReplaceEvent<int> e) => e.Index == model.Index).Subscribe(delegate
		{
			RefreshGoodsSlot();
		})
			.AddTo(Disposable);
		_playerManager.OnInventoryChanged.Subscribe(delegate
		{
			UpdateCountText();
		}).AddTo(Disposable);
		_playerManager.OnSafeInventoryChanged.Subscribe(delegate
		{
			UpdateCountText();
		}).AddTo(Disposable);
		_gameManager.OnMyCharacterSpawned.Subscribe(BindCoolTime).AddTo(Disposable);
		BindCoolTime(_gameManager.MyCharacterModel);
	}

	private void BindCoolTime(CharacterModel characterModel)
	{
		_coolTimeDisposable?.Dispose();
		_coolTimeDisposable = new CompositeDisposable();
		if (!(characterModel == null))
		{
			characterModel.RxConsumableTypeCoolTimes.ObserveChange().Where((EConsumableType _) => _itemId >= 1 && _ == ConsumableType).Subscribe(delegate
			{
				UpdateItemTypeCoolTime();
			})
				.AddTo(_coolTimeDisposable);
			UpdateItemTypeCoolTime();
		}
	}

	private void RefreshGoodsSlot()
	{
		if (_playerManager.TryGetQuickSlotItemId(_model.Index, out _itemId))
		{
			_view.GoodsSlot.gameObject.SetActive(true);
			long inventoryGoodsCount = _playerManager.GetInventoryGoodsCount(GoodsCreator.MakeItemGoods(_itemId, 0L, 0L));
			_goodsSlotModel = new GoodsSlotModel(GoodsCreator.MakeItemGoods(_itemId, inventoryGoodsCount, 0L));
			_goodsSlotModel.ShowItemCount = true;
			_view.GoodsSlot.Inject(_goodsSlotModel);
			ApplyMaxStackColor();
		}
		else
		{
			_tweener?.Kill();
			_tweener = null;
			_view.GoodsSlot.gameObject.SetActive(false);
			_view.Cooltime.SetActive(false);
		}
	}

	private void UpdateCountText()
	{
		if (_itemId == 0)
		{
			return;
		}
		long count = _goodsSlotModel.GoodsSlot.Count;
		long inventoryGoodsCount = _playerManager.GetInventoryGoodsCount(GoodsCreator.MakeItemGoods(_itemId, 0L, 0L));
		if (count != inventoryGoodsCount)
		{
			GoodsSlot goodsSlot = _goodsSlotModel.GoodsSlot.Clone();
			goodsSlot.Goods = GoodsCreator.MakeItemGoods(_itemId, inventoryGoodsCount, 0L);
			_goodsSlotModel.GoodsSlot = goodsSlot;
			ApplyMaxStackColor();
		}
	}

	private void ApplyMaxStackColor()
	{
		long count = _goodsSlotModel.GoodsSlot.Count;
		int inventoryItemCountMax = _gameManager.MyPlayer.GetInventoryItemCountMax(_itemId);
		_view.GoodsSlot_Count_Text.text = ((count < inventoryItemCountMax) ? string.Format("{0}", count) : string.Format("{0}", count).WithColor("#a74200"));
	}

	private void UpdateItemTypeCoolTime()
	{
		if (_itemId == 0)
		{
			return;
		}
		_tweener?.Kill();
		_tweener = null;
		float remainCoolTime = GetRemainCoolTime();
		if (remainCoolTime <= 0.01f)
		{
			_view.Cooltime.SetActive(false);
			return;
		}
		_view.Cooltime.SetActive(true);
		_view.Cooltime_Image.fillAmount = 1f;
		_tweener = _view.Cooltime_Image.DOFillAmount(0f, remainCoolTime).SetLink(_view.Cooltime_Image.gameObject).OnUpdate(UpdateCoolTimeText)
			.OnComplete(OnCompleteCoolTimeTween);
	}

	private void UpdateCoolTimeText()
	{
		float remainCoolTime = GetRemainCoolTime();
		_view.Cooltime_Text.text = string.Format((remainCoolTime > 1f) ? "{0:0}" : "{0:0.0}", remainCoolTime);
	}

	private void OnCompleteCoolTimeTween()
	{
		_view.Cooltime.SetActive(false);
		_tweener = null;
	}

	private float GetRemainCoolTime()
	{
		if (_itemId != 0)
		{
			CharacterModel myCharacterModel = _gameManager.MyCharacterModel;
			if (myCharacterModel != null)
			{
				return myCharacterModel.GetRemainConsumableTypeCoolTime(ConsumableType);
			}
		}
		return 0f;
	}
}
