using GameCommon.Data;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class DraggingGoodsSlotPresenter : BaseSlotPresenter<DraggingGoodsSlotModel>
{
	[InjectModel]
	private DraggingGoodsSlotModel _model;

	[View]
	private DraggingGoodsSlotView _view;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private PopupManager _popupManager;

	protected override DraggingGoodsSlotModel SlotModel => _model;

	protected override Button Button => null;

	private void OnInject(DraggingGoodsSlotModel model)
	{
		InitSlot(model);
		if (_model.GoodsSlot.Goods != null)
		{
			_view.ItemMark.gameObject.SetActive(_playerManager.IsMarked(_model.GoodsSlot.Goods));
		}
		model.RxGoodsSlot.Subscribe(OnGoodsSlotChanged).AddTo(Disposable);
	}

	private void UpdateLayout()
	{
		if (_model.GoodsSlot.IsEmpty)
		{
			_view.Normal.gameObject.SetActive(value: false);
			return;
		}
		_view.LevelBg.gameObject.SetActive(value: false);
		_view.Artifact.gameObject.SetActive(value: false);
		_view.Icon.gameObject.SetActive(value: false);
		_view.Count.gameObject.SetActive(value: false);
		_view.ItemMark.gameObject.SetActive(value: false);
		_view.QuestMark.SetActive(value: false);
		_view.MissionMark.SetActive(value: false);
		SetNormalLayout();
	}

	private void SetNormalLayout()
	{
		EGoodsType type = _model.GoodsSlot.Type;
		_view.Normal.gameObject.SetActive(value: true);
		_view.Icon.gameObject.SetActive(value: true);
		_view.Icon.sprite = _assetManager.GetIconGoods(_model.GoodsSlot.Type, _model.GoodsSlot.Id);
		_view.SubIcon.gameObject.SetActive(type == EGoodsType.Blueprint);
		_view.ItemMark.gameObject.SetActive(_playerManager.IsMarked(_model.GoodsSlot.Goods));
		if (type == EGoodsType.Blueprint)
		{
			GoodsData resultGoodsDataByUsedItem = TCrafting.GetResultGoodsDataByUsedItem(_model.GoodsSlot.Id);
			_view.SubIcon.sprite = _assetManager.GetIconGoods(resultGoodsDataByUsedItem.Type, resultGoodsDataByUsedItem.Id);
		}
		switch (_model.GoodsSlot.Category)
		{
		case EGoodsCategory.Currency:
			_view.Count.gameObject.SetActive(value: true);
			_view.Count_Text.text = $"{_model.GoodsSlot.Count}";
			_view.LevelBg.gameObject.SetActive(value: false);
			break;
		case EGoodsCategory.Item:
			_view.Count.gameObject.SetActive(value: true);
			SetItemCountText();
			SetItemFrameColor();
			SetItemTaskIcons();
			break;
		case EGoodsCategory.Equipment:
			_view.Count.gameObject.SetActive(value: false);
			SetEquipmentFrameColor();
			break;
		case EGoodsCategory.Bag:
			_view.Count.gameObject.SetActive(value: false);
			SetBagFrameColor();
			break;
		}

		void SetBagFrameColor()
		{
			EGoodsGradeType grade = TBag.Table[_model.GoodsSlot.Id].Grade;
			Color color = grade.ToColor(_view.LevelBg.color.a);
			_view.LevelBg.gameObject.SetActive(grade > EGoodsGradeType.Normal);
			_view.LevelBg.color = color;
		}
		void SetEquipmentFrameColor()
		{
			TEquipment tEquipment = TEquipment.Table[_model.GoodsSlot.Id];
			EGoodsGradeType grade2 = tEquipment.Grade;
			Color color2 = grade2.ToColor(_view.LevelBg.color.a);
			_view.LevelBg.gameObject.SetActive(grade2 > EGoodsGradeType.Normal);
			_view.LevelBg.color = color2;
			_view.Artifact.gameObject.SetActive(tEquipment.IsArtifact());
			if (tEquipment.IsArtifact())
			{
				GameObject[] artifact_Group = _view.Artifact_Group;
				for (int i = 0; i < artifact_Group.Length; i++)
				{
					Image component = artifact_Group[i].GetComponent<Image>();
					component.color = new Color(color2.r, color2.g, color2.b, component.color.a);
				}
			}
		}
		void SetItemCountText()
		{
			_view.Count_Text.text = $"{_model.GoodsSlot.Count}";
		}
		void SetItemFrameColor()
		{
			EGoodsGradeType grade3 = TItem.Table[_model.GoodsSlot.Id].Grade;
			Color color3 = grade3.ToColor(_view.LevelBg.color.a);
			_view.LevelBg.gameObject.SetActive(grade3 > EGoodsGradeType.Normal);
			_view.LevelBg.color = color3;
		}
		void SetItemTaskIcons()
		{
			TItem tItem = TItem.Table[_model.GoodsSlot.Id];
			_view.QuestMark.SetActive(tItem.Category == EItemCategory.Quest);
			_view.MissionMark.SetActive(tItem.Category == EItemCategory.Mission);
		}
	}

	private void OnGoodsSlotChanged(GoodsSlot goodsSlot)
	{
		UpdateLayout();
	}
}
