using System;
using System.Collections.Generic;
using System.Linq;
using GameCommon.Data;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.AssetManagement;
using NemoLib.Log;
using NemoLib.Mvp;
using UniRx;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class EquipmentInfoPanelPresenter : BasePanelPresenter
{
	[View]
	private EquipmentInfoPanelView _view;

	[InjectModel]
	private EquipmentInfoPanelModel _model;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private SeriLogger _logger;

	private CompositeDisposable _disposable;

	protected override BasePanelModel PanelModel => _model;

	private void Start()
	{
		_view.BackButton.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnBackButtonClick();
		}).AddTo(this);
	}

	protected override void OnDestroy()
	{
		if (_disposable != null)
		{
			_disposable.Dispose();
		}
		base.OnDestroy();
	}

	private void OnInject(EquipmentInfoPanelModel model)
	{
		if (_disposable != null)
		{
			_disposable.Dispose();
		}
		_disposable = new CompositeDisposable();
		UpdateEquipmentUis();
		PlayerInventory myInventory = _gameManager.MyInventory;
		if (myInventory != null)
		{
			myInventory.OnInventoryChanged.Subscribe(delegate
			{
				UpdateEquipmentUis();
			}).AddTo(_disposable);
		}
		_playerManager.OnStashChanged.Subscribe(delegate
		{
			UpdateEquipmentUis();
		}).AddTo(_disposable);
		_view.Title_TypeIcon.sprite = _assetManager.GetIconGoodsSubType(_model.Goods.Type);
		InitGamePadNav();
		model.RxIsInputEnabled.Where((bool b) => b).Subscribe(delegate
		{
			OnInputEnabled();
		}).AddTo(_disposable);
		model.RxIsInputEnabled.Where((bool b) => !b).Subscribe(delegate
		{
			OnInputDisabled();
		}).AddTo(_disposable);
	}

	private void UpdateEquipmentUis()
	{
		TEquipment equipmentData = TEquipment.Table[_model.Goods.Id];
		bool flag = _model.Goods.IsArtifact();
		_view.Price.SetActive(!flag);
		_view.StashAmount.SetActive(!flag);
		_view.ArtifactTagGroup.SetActive(flag);
		_view.ArtifactPrice.SetActive(flag);
		_view.ArtifactCorruption.SetActive(flag);
		if (flag)
		{
			UpdateArtifactUi();
			UpdateCorruption();
		}
		EGoodsType type = _model.Goods.Type;
		if (type == EGoodsType.Weapon)
		{
			bool flag2 = TCharacter.Table[_playerManager.SelectedCharacterId].WeaponType == equipmentData.WeaponType;
			string text = TLocalizedText.EquipmentTooltipExclusiveCharacter.Format(TCharacter.Table.Values.First((TCharacter c) => c.WeaponType == equipmentData.WeaponType).Name);
			_view.CharacterSpecificCanEquip.SetActive(flag2);
			_view.CharacterSpecificCanEquip_Text.text = text;
			_view.CharacterSpecificCannotEquip.SetActive(!flag2);
			_view.CharacterSpecificCannotEquip_Text.text = text;
		}
		else
		{
			_view.CharacterSpecificCanEquip.SetActive(false);
			_view.CharacterSpecificCannotEquip.SetActive(false);
		}
		_view.Title_Name.text = equipmentData.Name ?? "";
		_view.Title_LevelText.gameObject.SetActive(equipmentData.Grade != EGoodsGradeType.None);
		_view.Title_LevelText.text = equipmentData.Grade.ToLocalText();
		_view.Title_LevelText.color = equipmentData.Grade.ToColor(_view.Title_LevelText.color.a);
		float statValue = equipmentData.GetStatValue(EStatType.Atk);
		_view.Attack.SetActive(type == EGoodsType.Weapon && statValue != 0f);
		_view.Attack_Value.text = string.Format("+{0}", statValue);
		_view.Elemental.SetActive(equipmentData.ElementalType != EElementalType.None);
		_view.Elemental_Value.text = string.Format("{0}", equipmentData.ElementalType).WithColor(equipmentData.ElementalType.GetColorCode());
		_view.Description_Text.text = equipmentData.Desc ?? "";
		bool flag3 = equipmentData.EquipmentEffectIds != null && equipmentData.EquipmentEffectIds.Length != 0;
		Dictionary<EStatType, float> artifactStats = _model.Goods.GetArtifactStats();
		bool active = true;
		if (!flag3)
		{
			active = artifactStats.Count > 0;
		}
		_view.EffectGroup.SetActive(active);
		_view.EffectGroup_Skills[0].transform.parent.gameObject.SetActive(flag3);
		for (int i = 0; i < _view.EffectGroup_Skills.Length; i++)
		{
			int num = ((equipmentData.EquipmentEffectIds != null && i < equipmentData.EquipmentEffectIds.Length) ? equipmentData.EquipmentEffectIds[i] : 0);
			GameObject gameObject = _view.EffectGroup_Skills[i];
			if (num < 1)
			{
				gameObject.SetActive(false);
				continue;
			}
			gameObject.SetActive(true);
			TEquipmentEffect tEquipmentEffect = TEquipmentEffect.Table[num];
			int num2 = TEquipmentEffect.GroupTable[tEquipmentEffect.GroupId].Max((TEquipmentEffect e) => e.Level);
			TextMeshProUGUI textMeshProUGUI = gameObject.FindChildComponent<TextMeshProUGUI>("@Text");
			textMeshProUGUI.text = string.Format("{0} Lv.{1} (Max {2}) : {3}", tEquipmentEffect.Name, tEquipmentEffect.Level, num2, tEquipmentEffect.Desc);
		}
		List<KeyValuePair<EStatType, float>> list = artifactStats.ToList();
		_view.EffectGroup_ArtifactsStats[0].transform.parent.gameObject.SetActive(list.Count > 0);
		for (int j = 0; j < _view.EffectGroup_ArtifactsStats.Length; j++)
		{
			GameObject gameObject2 = _view.EffectGroup_ArtifactsStats[j];
			if (j < list.Count)
			{
				gameObject2.SetActive(true);
				KeyValuePair<EStatType, float> keyValuePair = list[j];
				gameObject2.FindChildComponent<TextMeshProUGUI>("@Text").text = keyValuePair.Key.ToStatText(keyValuePair.Value);
			}
			else
			{
				gameObject2.SetActive(false);
			}
		}
		_view.Price_GoldText_Value.text = string.Format("{0}", equipmentData.SellPrice);
		_view.StashAmount_Value.text = string.Format("{0}", _playerManager.GetStashGoodsCount(_model.Goods));
	}

	private void UpdateArtifactUi()
	{
		TEquipment tEquipment = TEquipment.Table[_model.Goods.Id];
		GameObject[] artifactTagGroup_Slots = _view.ArtifactTagGroup_Slots;
		for (int i = 0; i < artifactTagGroup_Slots.Length; i++)
		{
			artifactTagGroup_Slots[i].SetActive(false);
		}
		ArtifactCategorySlotPresenter component = _view.ArtifactTagGroup_Slots[0].GetComponent<ArtifactCategorySlotPresenter>();
		component.gameObject.SetActive(true);
		component.Inject(new ArtifactCategorySlotModel
		{
			Category = tEquipment.ArtifactCategory
		});
		if (_view.ArtifactTagGroup_Slots.Length - 1 < _model.Goods.GetPrefixes().Length)
		{
			_logger.Error(string.Format("[ArtifactPrefixOverflow] EquipmentInfoPanel GoodsId={0} Grade={1} ", _model.Goods.Id, tEquipment.Grade) + string.Format("prefixCount={0} slotCapacity={1} prefixIds=[{2}]", _model.Goods.GetPrefixes().Length, _view.ArtifactTagGroup_Slots.Length - 1, string.Join(",", _model.Goods.GetPrefixes())));
		}
		for (int j = 0; j < _model.Goods.GetPrefixes().Length; j++)
		{
			ArtifactPrefixSlotPresenter component2 = _view.ArtifactTagGroup_Slots[j + 1].GetComponent<ArtifactPrefixSlotPresenter>();
			component2.gameObject.SetActive(true);
			component2.Inject(new ArtifactPrefixSlotModel
			{
				PrefixId = _model.Goods.GetPrefixes()[j]
			});
		}
		_view.ArtifactPrice_GoldText_Value.text = string.Format("{0}", _model.Goods.GetArtifactPrice());
		Canvas.ForceUpdateCanvases();
	}

	private void UpdateCorruption()
	{
		_view.ArtifactCorruption_Value.text = string.Format("{0}", TEquipment.Table[_model.Goods.Id].Corruption);
	}

	private void OnBackButtonClick()
	{
		_model.OnBack.OnNext(Unit.Default);
	}

	private void InitGamePadNav()
	{
		ClearNav();
		IReadOnlyList<Selectable> items = new Selectable[1] { _view.BackButton };
		SetNavHorizontal(items);
	}
}
