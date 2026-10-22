using System;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using UnityEngine.UI;
using VContainer;

public class ItemInfoPanelPresenter : BasePanelPresenter
{
	[View]
	private ItemInfoPanelView _view;

	[InjectModel]
	private ItemInfoPanelModel _model;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private PopupManager _popupManager;

	private CompositeDisposable _disposable;

	private const string ITEM_MAX_COLOR = "#a74200";

	protected override BasePanelModel PanelModel => _model;

	private GamePlayer Player => _gameManager.MyPlayer;

	private bool ShowNoteButtonActive
	{
		get
		{
			if (!_playerManager.IsKnownGoods(_model.Goods))
			{
				return false;
			}
			return TItem.Table[_model.Goods.Id].GoodsType == EGoodsType.Note;
		}
	}

	private void Start()
	{
		_view.ShowNoteButton.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			ShowNoteButtonClick();
		}).AddTo(this);
		_view.BackButton.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnBackButtonClick();
		}).AddTo(this);
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		_disposable?.Dispose();
	}

	private void OnInject(ItemInfoPanelModel model)
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		UpdateItemUis();
		_playerManager.OnOwnedGoodsChanged.Subscribe(delegate
		{
			UpdateItemUis();
		}).AddTo(_disposable);
		_view.Title_TypeIcon.sprite = _assetManager.GetIconGoodsSubType(_model.Goods.Type);
		InitGamePadNav();
		model.RxIsInputEnabled.Where((bool _) => _).Subscribe(delegate
		{
			OnInputEnabled();
		}).AddTo(_disposable);
		model.RxIsInputEnabled.Where((bool _) => !_).Subscribe(delegate
		{
			OnInputDisabled();
		}).AddTo(_disposable);
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
		int inventoryItemCountMax = _gameManager.MyPlayer.GetInventoryItemCountMax(tItem.Id);
		string text = string.Format("{0} / {1}", inventoryGoodsCount, inventoryItemCountMax);
		if (inventoryGoodsCount == inventoryItemCountMax)
		{
			text = text.WithColor("#a74200");
		}
		_view.InventoryAmount_Value.text = text;
		_view.StashAmount_Value.text = string.Format("{0}", stashGoodsCount);
		bool active = _playerManager.IsKnownGoods(_model.Goods) && tItem.GoodsType == EGoodsType.Note;
		_view.ShowNoteButton.gameObject.SetActive(active);
	}

	private void OnBackButtonClick()
	{
		_model.OnBack.OnNext(Unit.Default);
	}

	private void InitGamePadNav()
	{
		ClearNav();
		if (ShowNoteButtonActive)
		{
			ConnectNav(_view.ShowNoteButton, ENavDir.Right, _view.BackButton);
			ConnectNav(_view.BackButton, ENavDir.Left, _view.ShowNoteButton);
		}
		SetNavHorizontal(new Selectable[1] { _view.BackButton });
	}

	private void ShowNoteButtonClick()
	{
		TItem tItem = TItem.Table[_model.Goods.Id];
		_popupManager.Open<NoteSystemPopupPresenter>(new NoteSystemPopupModel
		{
			Texts = new string[1] { tItem.Note },
			NoteType = tItem.NoteType
		});
	}
}
