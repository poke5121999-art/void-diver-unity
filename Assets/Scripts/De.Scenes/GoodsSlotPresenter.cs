using GameCommon.Data;
using GameCommon.Enum;
using GameCommon.Table;
using UniRx;
using UnityEngine;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using NemoLib.Popup;
using UnityEngine.UI;
using VContainer;

public class GoodsSlotPresenter : BaseSlotPresenter<GoodsSlotModel>
{
	[InjectModel]
	private GoodsSlotModel _model;

	[View]
	private GoodsSlotView _view;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private PopupManager _popupManager;

	protected override GoodsSlotModel SlotModel => _model;

	protected override Button Button => _view.Slot;

	private void OnInject(GoodsSlotModel model)
	{
		InitSlot(model);
		ButtonController.enabled = model.EnableButtonAnimator;
		model.RxGoodsSlot.Subscribe(OnGoodsSlotChanged).AddTo(Disposable);
		_playerManager.OnGoodsMarkChanged.Subscribe(OnGoodsMarkChanged).AddTo(Disposable);
	}

	protected override void OnClick()
	{
		TryShowGoodsTooltipPopup();
		base.OnClick();
	}

	private bool TryShowGoodsTooltipPopup()
	{
		if (!_model.UseTooltip)
		{
			return false;
		}
		if (_model.RxGoodsSlot.Value == null)
		{
			return false;
		}
		if (_model.RxGoodsSlot.Value.IsEmpty)
		{
			return false;
		}
		_popupManager.Open<GoodsTooltipPopupPresenter>(new GoodsTooltipPopupModel
		{
			Goods = _model.RxGoodsSlot.Value.Goods,
			IsCrafting = _model.IsCrafting,
			OnCraftingNavigate = _model.OnCraftingNavigate
		});
		return true;
	}

	private void UpdateLayout()
	{
		if (_model.RxGoodsSlot.Value.IsEmpty)
		{
			_view.Normal.gameObject.SetActive(false);
			return;
		}
		_view.LevelBg.gameObject.SetActive(false);
		_view.Artifact.gameObject.SetActive(false);
		_view.Icon.gameObject.SetActive(false);
		_view.SubIcon.gameObject.SetActive(false);
		_view.Count.gameObject.SetActive(false);
		_view.ItemMark.gameObject.SetActive(false);
		_view.QuestMark.SetActive(false);
		_view.MissionMark.SetActive(false);
		SetNormalLayout();
	}

	private void SetNormalLayout()
	{
		EGoodsType type = _model.GoodsType;
		Color color = ((!_model.ShowUnknownGoods && !_playerManager.IsKnownGoods(_model.RxGoodsSlot.Value.Goods)) ? GameCommon.Utility.ColorUtility.ParseOrWhite("#1B1B1B") : Color.white);
		_view.Normal.gameObject.SetActive(true);
		_view.Icon.gameObject.SetActive(true);
		_view.Icon.sprite = _assetManager.GetIconGoods(_model.RxGoodsSlot.Value.Type, _model.RxGoodsSlot.Value.Id);
		_view.SubIcon.gameObject.SetActive(type == EGoodsType.Blueprint);
		_view.ItemMark.gameObject.SetActive(_playerManager.IsMarked(_model.RxGoodsSlot.Value.Goods));
		_view.Icon.color = color;
		_view.SubIcon.color = color;
		if (type == EGoodsType.Blueprint)
		{
			GoodsData resultGoodsDataByUsedItem = TCrafting.GetResultGoodsDataByUsedItem(_model.GoodsId);
			_view.SubIcon.sprite = _assetManager.GetIconGoods(resultGoodsDataByUsedItem.Type, resultGoodsDataByUsedItem.Id);
		}
		switch (_model.RxGoodsSlot.Value.Category)
		{
		case EGoodsCategory.Currency:
			_view.Count.gameObject.SetActive(_model.ShowItemCount);
			SetItemCountText();
			_view.LevelBg.gameObject.SetActive(false);
			break;
		case EGoodsCategory.Item:
			_view.Count.gameObject.SetActive(_model.ShowItemCount);
			SetItemCountText();
			SetItemFrameColor();
			SetItemTaskIcons();
			break;
		case EGoodsCategory.Equipment:
			_view.Count.gameObject.SetActive(false);
			SetEquipmentFrameColor();
			break;
		case EGoodsCategory.Bag:
			_view.Count.gameObject.SetActive(false);
			SetBagFrameColor();
			break;
		}
		void SetBagFrameColor()
		{
			TBag tBag = TBag.Table[_model.RxGoodsSlot.Value.Id];
			if (tBag != null)
			{
				EGoodsGradeType grade = tBag.Grade;
				Color color2 = grade.ToColor(_view.LevelBg.color.a);
				_view.LevelBg.gameObject.SetActive((int)grade > 1);
				_view.LevelBg.color = color2;
			}
		}
		void SetEquipmentFrameColor()
		{
			TEquipment tEquipment = TEquipment.Table[_model.RxGoodsSlot.Value.Id];
			EGoodsGradeType grade2 = tEquipment.Grade;
			Color color3 = grade2.ToColor(_view.LevelBg.color.a);
			_view.LevelBg.gameObject.SetActive((int)grade2 > 1);
			_view.LevelBg.color = color3;
			_view.Artifact.gameObject.SetActive(tEquipment.IsArtifact());
			if (tEquipment.IsArtifact())
			{
				GameObject[] artifact_Group = _view.Artifact_Group;
				foreach (GameObject gameObject in artifact_Group)
				{
					Image component = gameObject.GetComponent<Image>();
					component.color = new Color(color3.r, color3.g, color3.b, component.color.a);
				}
			}
		}
		void SetItemCountText()
		{
			_view.Count_Text.text = string.Format("{0}", _model.RxGoodsSlot.Value.Count);
		}
		void SetItemFrameColor()
		{
			TItem tItem = TItem.Table[_model.RxGoodsSlot.Value.Id];
			if (tItem != null)
			{
				EGoodsGradeType grade3 = tItem.Grade;
				Color color4 = grade3.ToColor(_view.LevelBg.color.a);
				_view.LevelBg.gameObject.SetActive((int)grade3 > 1);
				_view.LevelBg.color = color4;
			}
		}
		void SetItemTaskIcons()
		{
			TItem tItem2 = TItem.Table[_model.RxGoodsSlot.Value.Id];
			if (tItem2 != null)
			{
				_view.QuestMark.SetActive(tItem2.Category == EItemCategory.Quest);
				_view.MissionMark.SetActive(tItem2.Category == EItemCategory.Mission);
			}
		}
	}

	private void OnGoodsSlotChanged(GoodsSlot goodsSlot)
	{
		UpdateLayout();
	}

	private void OnGoodsMarkChanged((EGoodsType goodsType, int id, bool isMarked) data)
	{
		if (_model.GoodsType == data.goodsType && _model.GoodsId == data.id)
		{
			_view.ItemMark.gameObject.SetActive(_playerManager.IsMarked(_model.RxGoodsSlot.Value.Goods));
		}
	}
}
