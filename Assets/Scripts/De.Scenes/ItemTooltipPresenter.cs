using System;
using GameCommon.Data;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using UniRx;
using UnityEngine;
using VContainer;

public class ItemTooltipPresenter : BasePanelPresenter
{
	[View]
	private ItemTooltipView _view;

	[InjectModel]
	private ItemTooltipModel _model;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private EnvironmentSettings _environmentSettings;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private GameManager _gameManager;

	private CompositeDisposable _disposable;

	private const string ITEM_MAX_COLOR = "#a74200";

	protected override BasePanelModel PanelModel => _model;

	private GamePlayer Player => _gameManager.MyPlayer;

	protected override void OnDestroy()
	{
		base.OnDestroy();
		_disposable?.Dispose();
	}

	private void OnInject(ItemTooltipModel model)
	{
		if (_gameManager.IsClientJoined(_gameManager.LocalClientId))
		{
			_disposable?.Dispose();
			_disposable = new CompositeDisposable();
			UpdateItemUis();
			_playerManager.OnOwnedGoodsChanged.Subscribe(delegate
			{
				UpdateItemUis();
			}).AddTo(_disposable);
			_view.Title_Mark.gameObject.SetActive(_playerManager.IsMarked(model.Goods));
			_view.Title_TypeIcon.sprite = _assetManager.GetIconGoodsSubType(_model.Goods.Type);
			_playerManager.OnGoodsMarkChanged.Subscribe(OnGoodsMarkChanged).AddTo(_disposable);
		}
		else
		{
			base.gameObject.SetActive(false);
		}
	}

	private void UpdateItemUis()
	{
		TItem tItem = TItem.Table[_model.Goods.Id];
		_view.Title_Name.text = tItem.Name;
		_view.Title_GradeText.gameObject.SetActive(tItem.Grade != EGoodsGradeType.None);
		_view.Title_GradeText.text = tItem.Grade.ToLocalText();
		_view.Title_GradeText.color = tItem.Grade.ToColor(_view.Title_GradeText.color.a);
		_view.Description_Text.text = tItem.Desc;
		long inventoryGoodsCount = _playerManager.GetInventoryGoodsCount(_model.Goods);
		long stashGoodsCount = _playerManager.GetStashGoodsCount(_model.Goods);
		_view.Price_GoldText_Value.text = string.Format("{0}", tItem.SellPrice);
		int inventoryItemCountMax = Player.GetInventoryItemCountMax(tItem.Id);
		string text = string.Format("{0} / {1}", inventoryGoodsCount, inventoryItemCountMax);
		if (inventoryGoodsCount == inventoryItemCountMax)
		{
			text = text.WithColor("#a74200");
		}
		_view.InventoryAmount_Value.text = text;
		_view.StashAmount_Value.text = string.Format("{0}", stashGoodsCount);
		EGoodsType type = _model.Goods.Type;
		bool active = (type == EGoodsType.Note || type == EGoodsType.Blueprint || type == EGoodsType.MusicDisc) && _playerManager.IsItemUsed(_model.Goods.Id);
		_view.RecordComplete.SetActive(active);
		if (_environmentSettings.Environment != EEnvironment.Live)
		{
			_view.Worth.SetActive(true);
			_view.Worth_Text_Value.text = string.Format("{0}", _model.Goods.GetWorth());
		}
	}

	private void OnGoodsMarkChanged((EGoodsType goodsType, int id, bool isMarked) data)
	{
		if (_model.Goods.Type == data.goodsType && _model.Goods.Id == data.id)
		{
			_view.Title_Mark.gameObject.SetActive(data.isMarked);
		}
	}
}
