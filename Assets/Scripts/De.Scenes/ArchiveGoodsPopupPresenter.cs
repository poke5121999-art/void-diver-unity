using System.Linq;
using Cysharp.Threading.Tasks;
using GameCommon.Data;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using UnityEngine;
using VContainer;

public class ArchiveGoodsPopupPresenter : GamePopupPresenter
{
	[InjectModel]
	private ArchiveGoodsPopupModel _model;

	[View]
	private ArchiveGoodsPopupView _view;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private ToastManager _toastManager;

	private GoodsSlotRowScrollerModel _scrollerModel;

	private EquipmentInfoPanelModel _equipmentPanelModel;

	private ItemInfoPanelModel _itemPanelModel;

	private CompositeDisposable _panelDisposable;

	private GoodsSlotModel _selectedSlot;

	private GoodsSlotModel _focusedSlot;

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	public override bool IsInputActionPopup => true;

	private InputActions.UiActions UiActions => GameInputManager.UiActions;

	public override string OpenSfx => "PopUpOpen";

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		_view.Unknown.SetActive(false);
		_view.Empty.SetActive(true);
		_view.EquipmentInfoPanel.gameObject.SetActive(false);
		_view.ItemInfoPanel.gameObject.SetActive(false);
		_view.KeyGuide_Note.SetActive(_model.FilterGoodsType == EGoodsType.Note);
		_view.Title.text = GetTitle();
		_view.TitleSub.text = GetSubTitle();
		Goods[] array = BuildGoodsArray();
		_view.Left.SetActive(array.Length != 0);
		_scrollerModel = new GoodsSlotRowScrollerModel
		{
			GoodsArray = array,
			ShowUnknownGoods = false
		};
		_scrollerModel.OnSlotClick.Subscribe(OnSlotClick).AddTo(this);
		_scrollerModel.OnSlotEnter.Subscribe(OnSlotEnter).AddTo(this);
		_scrollerModel.OnSlotExit.Subscribe(OnSlotExit).AddTo(this);
		_view.Scroller.GetComponent<GoodsSlotRowScrollerPresenter>().Inject(_scrollerModel);
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		UiActions.Escape.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnBack();
		}).AddTo(InputDisposable);
		UiActions.ShowNote.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnShowNote();
		}).AddTo(InputDisposable);
		_scrollerModel.RxIsInputEnabled.Value = _selectedSlot == null;
		SetActivePanelInputEnabled(_selectedSlot != null);
	}

	protected override void OnInputDisabled()
	{
		base.OnInputDisabled();
		_scrollerModel.RxIsInputEnabled.Value = false;
		SetActivePanelInputEnabled(false);
	}

	private void OnBack()
	{
		if (_selectedSlot == null)
		{
			Close();
			return;
		}
		UpdateSelectedObject(null);
		UpdateInfo(_focusedSlot);
	}

	private void OnShowNote()
	{
		GoodsSlotModel goodsSlotModel = _selectedSlot ?? _focusedSlot;
		if (goodsSlotModel == null || goodsSlotModel.IsSlotEmpty)
		{
			return;
		}
		Goods goods = goodsSlotModel.GoodsSlot.Goods;
		if (goods.Type != EGoodsType.Note)
		{
			return;
		}
		if (_playerManager.IsUnknownGoods(goods))
		{
			_toastManager.NoticeFail(TLocalizedText.NoteNotAcquired.LocalText);
			return;
		}
		TItem tItem = TItem.Table[goods.Id];
		_popupManager.Open<NoteSystemPopupPresenter>(new NoteSystemPopupModel
		{
			Texts = new string[1] { tItem.Note },
			NoteType = tItem.NoteType
		});
	}

	private string GetTitle()
	{
		EArchiveCategory eArchiveCategory;
		switch (_model.FilterGoodsType)
		{
		case EGoodsType.Note:
			eArchiveCategory = EArchiveCategory.Record;
			break;
		case EGoodsType.Weapon:
			eArchiveCategory = EArchiveCategory.Weapon;
			break;
		case EGoodsType.Accessory:
			eArchiveCategory = EArchiveCategory.Accessory;
			break;
		case EGoodsType.Artifact:
			eArchiveCategory = EArchiveCategory.Artifact;
			break;
		default:
			eArchiveCategory = EArchiveCategory.Item;
			break;
		}
		return eArchiveCategory.ToLocalText();
	}

	private string GetSubTitle()
	{
		switch (_model.FilterGoodsType)
		{
		case EGoodsType.Note:
			return EArchiveRecordCategory.Note.ToLocalText();
		case EGoodsType.Weapon:
			return _model.FilterWeaponType.ToLocalText();
		case EGoodsType.Accessory:
		case EGoodsType.Artifact:
			return _model.FilterGradeType.ToLocalText();
		default:
			return _model.FilterGoodsType.ToLocalText();
		}
	}

	private Goods[] BuildGoodsArray()
	{
		EGoodsType filterGoodsType = _model.FilterGoodsType;
		if (filterGoodsType == EGoodsType.Weapon)
		{
			return TEquipment.Table.Values.Where((TEquipment x) => x.GoodsType == EGoodsType.Weapon && x.WeaponType == _model.FilterWeaponType).OrderBy((TEquipment x) => x.Grade).ThenBy((TEquipment x) => x.Id)
				.Select((TEquipment x) => GoodsCreator.MakeEquipmentGoods(x.Id))
				.ToArray();
		}
		if (filterGoodsType != EGoodsType.Accessory && filterGoodsType != EGoodsType.Artifact)
		{
			return TItem.Table.Values.Where((TItem x) => x.IsVisibleInArchive).Where((TItem x) => x.GoodsType == _model.FilterGoodsType).OrderBy((TItem x) => x.Grade)
				.ThenBy((TItem x) => x.Id)
				.Select((TItem x) => GoodsCreator.MakeItemGoods(x.Id))
				.ToArray();
		}
		return TEquipment.Table.Values.Where((TEquipment x) => x.GoodsType == _model.FilterGoodsType && x.Grade == _model.FilterGradeType).OrderBy((TEquipment x) => x.Grade).ThenBy((TEquipment x) => x.Id)
			.Select((TEquipment x) => GoodsCreator.MakeEquipmentGoods(x.Id))
			.ToArray();
	}

	private void OnSlotClick(GoodsSlotModel slotModel)
	{
		if (slotModel == null || slotModel.GoodsSlot == null)
		{
			return;
		}
		Goods goods = slotModel.GoodsSlot.Goods;
		if (_playerManager.IsUnknownGoods(goods))
		{
			_toastManager.NoticeFail(TLocalizedText.UnknownGoodsFormat.Format(goods.Type.ToLocalText()));
			return;
		}
		UpdateSelectedObject(slotModel);
		UpdateInfo(slotModel);
	}

	private void OnSlotEnter(GoodsSlotModel slotModel)
	{
		_focusedSlot = slotModel;
		if (_selectedSlot == null)
		{
			UpdateInfo(_focusedSlot);
		}
	}

	private void OnSlotExit(GoodsSlotModel slotModel)
	{
		_focusedSlot = null;
		if (_selectedSlot == null)
		{
			UpdateInfo(null);
		}
	}

	private void UpdateInfo(GoodsSlotModel slotModel)
	{
		_panelDisposable?.Dispose();
		_panelDisposable = new CompositeDisposable();
		if (slotModel == null || slotModel.IsSlotEmpty)
		{
			_view.Empty.SetActive(false);
			_view.Unknown.SetActive(true);
			_view.EquipmentInfoPanel.gameObject.SetActive(false);
			_view.ItemInfoPanel.gameObject.SetActive(false);
			return;
		}
		Goods goods = slotModel.GoodsSlot.Goods;
		bool flag = _playerManager.IsUnknownGoods(goods);
		bool flag2 = goods.IsEquipment();
		_view.Empty.SetActive(false);
		_view.Unknown.SetActive(flag);
		_view.EquipmentInfoPanel.gameObject.SetActive(!flag && flag2);
		_view.ItemInfoPanel.gameObject.SetActive(!flag && !flag2);
		if (flag2)
		{
			_equipmentPanelModel = new EquipmentInfoPanelModel
			{
				Goods = goods
			};
			if (IsInputEnable)
			{
				_equipmentPanelModel.RxIsInputEnabled.Value = _selectedSlot != null;
			}
			_equipmentPanelModel.OnBack.Subscribe(delegate
			{
				OnBack();
			}).AddTo(_panelDisposable);
			_view.EquipmentInfoPanel.Inject(_equipmentPanelModel);
		}
		else
		{
			_itemPanelModel = new ItemInfoPanelModel
			{
				Goods = goods
			};
			if (IsInputEnable)
			{
				_itemPanelModel.RxIsInputEnabled.Value = _selectedSlot != null;
			}
			_itemPanelModel.OnBack.Subscribe(delegate
			{
				OnBack();
			}).AddTo(_panelDisposable);
			_view.ItemInfoPanel.Inject(_itemPanelModel);
		}
	}

	private void UpdateSelectedObject(GoodsSlotModel slotModel)
	{
		if (_selectedSlot != null)
		{
			_selectedSlot.RxIsSelected.Value = false;
		}
		if (slotModel != null)
		{
			slotModel.RxIsSelected.Value = true;
		}
		_selectedSlot = slotModel;
		if (IsInputEnable)
		{
			_scrollerModel.RxIsInputEnabled.Value = _selectedSlot == null;
			SetActivePanelInputEnabled(_selectedSlot != null);
		}
	}

	private void SetActivePanelInputEnabled(bool value)
	{
		if (_view.EquipmentInfoPanel.gameObject.activeSelf && _equipmentPanelModel != null)
		{
			_equipmentPanelModel.RxIsInputEnabled.Value = value;
		}
		else if (_view.ItemInfoPanel.gameObject.activeSelf && _itemPanelModel != null)
		{
			_itemPanelModel.RxIsInputEnabled.Value = value;
		}
	}
}
