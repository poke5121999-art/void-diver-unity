using System.Linq;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class EmployeeCardPresenter : BaseSlotPresenter<EmployeeCardModel>
{
	[InjectModel]
	private EmployeeCardModel _model;

	[View]
	private EmployeeCardView _view;

	[Inject]
	private TycoonManager _tycoonManager;

	[Inject]
	private AssetManager _assetManager;

	private CompositeDisposable _disposable;

	protected override EmployeeCardModel SlotModel => _model;

	protected override Button Button => _view.Contents;

	protected override void Start()
	{
		base.Start();
		_view.Contents.OnPointerEnterAsObservable().Subscribe(delegate
		{
			_model.OnPointerEnter.OnNext(_model);
		});
		_view.Btns_BtnCleansing.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			_model.OnCleansingClick.OnNext(_model);
		});
		_view.Btns_BtnFire.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			_model.OnFireClick.OnNext(_model);
		});
	}

	protected override void OnDestroy()
	{
		_disposable?.Dispose();
		base.OnDestroy();
	}

	private void OnInject(EmployeeCardModel model)
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		TEmployee tEmployee = TEmployee.Table[model.EmployeeId];
		_view.EmployeeInfo_EmployeeFace.sprite = _assetManager.GetIconEmployeeFace(model.EmployeeId);
		SetupName(model, tEmployee);
		// Trap 59 (docs/DECODING.md): float32 product before the integer conversion, as IL2CPP; Mono needs the (float).
		int num = (int)(float)((float)(model.Level - 1) * tEmployee.CharmGrowth + (float)tEmployee.Charm);
		int num2 = (int)(float)((float)(model.Level - 1) * tEmployee.PersuasionGrowth + (float)tEmployee.Persuasion);
		int num3 = (int)(float)((float)(model.Level - 1) * tEmployee.ResistanceGrowth + (float)tEmployee.Resistance);
		_view.Charm_Num.text = num.ToString();
		_view.Persuasion_Num.text = num2.ToString();
		_view.Resistance_Num.text = num3.ToString();
		SetupStatDelta(model, tEmployee);
		SetupSkill(model, tEmployee, 0, _view.SpecialSkillFirst, _view.SpecialSkillFirst_Explain, _view.SpecialSkillFirst_Get, _view.SpecialSkillFirst_TextSkill, _view.SpecialSkillFirst_TextSkillDesc, _view.SpecialSkillFirst_Get_TextSkill, _view.SpecialSkillFirst_Get_TextSkillDesc, _view.SpecialSkillFirst_Lock);
		SetupSkill(model, tEmployee, 1, _view.SpecialSkillSecond, _view.SpecialSkillSecond_Explain, _view.SpecialSkillSecond_Get, _view.SpecialSkillSecond_TextSkill, _view.SpecialSkillSecond_TextSkillDesc, _view.SpecialSkillSecond_Get_TextSkill, _view.SpecialSkillSecond_Get_TextSkillDesc, _view.SpecialSkillSecond_Lock);
		SetupPanicGauge(model);
		SetupExp(model);
		SetupHireInfo(model);
		SetupCleansingCost(model);
		SetupCondition(model);
		_view.EmployeeInfo_Line.gameObject.SetActive(!model.IsPanic);
		_view.EmployeeInfo_LinePanic.gameObject.SetActive(model.IsPanic);
		bool flag = model.SlotType == EEmployeeSlotType.Viewing || model.SlotType == EEmployeeSlotType.Setting;
		_view.Contents.interactable = model.SlotType != EEmployeeSlotType.Viewing;
		_view.Btns.SetActive(value: false);
		if (!flag)
		{
			model.RxIsSelected.Subscribe(OnSelectedChanged).AddTo(_disposable);
		}
	}

	private void SetupName(EmployeeCardModel model, TEmployee employeeData)
	{
		string name = employeeData.Name;
		_view.EmployeeInfo_TextName.gameObject.SetActive(!model.IsPanic);
		_view.EmployeeInfo_TextNamePanic.gameObject.SetActive(model.IsPanic);
		(model.IsPanic ? _view.EmployeeInfo_TextNamePanic : _view.EmployeeInfo_TextName).text = name;
		_view.EmployeeInfo_TextLv.gameObject.SetActive(!model.IsLevelUp);
		_view.EmployeeInfo_TextLvUp.gameObject.SetActive(model.IsLevelUp);
		(model.IsLevelUp ? _view.EmployeeInfo_TextLvUp : _view.EmployeeInfo_TextLv).text = TLocalizedText.LevelFormat.Format(model.Level);
	}

	private void SetupPanicGauge(EmployeeCardModel model)
	{
		_view.EmployeeInfo_PanicSlider.gameObject.SetActive(model.SlotType != EEmployeeSlotType.Hire);
		if (model.SlotType != EEmployeeSlotType.Hire)
		{
			PanicGaugeUtility.Apply(_view.EmployeeInfo_PanicSlider, _view.EmployeeInfo_PanicSlider_Gauge_img_back, _view.EmployeeInfo_PanicSlider_Gauge_img_front, model.Stress, model.IsPanic);
			_view.EmployeeInfo_PanicSlider_Condition.sprite = _assetManager.GetIconEmployeeCondition(model.Stress, model.IsPanic);
		}
	}

	private void SetupExp(EmployeeCardModel model)
	{
		_view.EmployeeInfo_ExpSlider.gameObject.SetActive(model.SlotType != EEmployeeSlotType.Hire);
		if (model.SlotType != EEmployeeSlotType.Hire)
		{
			EmployeeExpUtility.Apply(_view.EmployeeInfo_ExpSlider, model.Level, model.Exp);
		}
	}

	private void SetupHireInfo(EmployeeCardModel model)
	{
		_view.EmployeeInfo_GoldText.SetActive(model.SlotType == EEmployeeSlotType.Hire);
		if (model.SlotType == EEmployeeSlotType.Hire)
		{
			_view.EmployeeInfo_GoldText_Value.text = model.HireCost.ToString("N0");
		}
	}

	private void SetupCondition(EmployeeCardModel model)
	{
		_view.EmployeeInfo_EmployeeCondition.SetActive(model.SlotType != EEmployeeSlotType.Hire);
		if (model.SlotType != EEmployeeSlotType.Hire)
		{
			_view.EmployeeInfo_EmployeeCondition_TextPanic.gameObject.SetActive(model.IsPanic);
			_view.EmployeeInfo_EmployeeCondition_TextPlaced.gameObject.SetActive(model.IsPlaced);
		}
	}

	private void OnSelectedChanged(bool isSelected)
	{
		ButtonController.SetSelected(isSelected);
		_view.Btns.SetActive(_model.SlotType == EEmployeeSlotType.Info && isSelected);
	}

	private void SetupCleansingCost(EmployeeCardModel model)
	{
		long num = _tycoonManager.CalculatePurifyCost(model.Level);
		_view.Btns_BtnCleansing_Gold_Value.text = num.ToString("N0");
	}

	private void SetupSkill(EmployeeCardModel model, TEmployee employeeData, int index, GameObject root, GameObject explain, GameObject get, TextMeshProUGUI nameText, TextMeshProUGUI descText, TextMeshProUGUI getNameText, TextMeshProUGUI getDescText, Image lockIcon)
	{
		bool flag = employeeData.SkillIds != null && index < employeeData.SkillIds.Length;
		root.SetActive(flag);
		if (flag)
		{
			int num = employeeData.SkillIds[index];
			TEmployeeSkill tEmployeeSkill = TEmployeeSkill.Table[num];
			bool flag2 = model.LearnedSkillIds != null && model.LearnedSkillIds.Contains(num);
			bool flag3 = model.NewlyLearnedSkillIds != null && model.NewlyLearnedSkillIds.Contains(num);
			explain.SetActive(flag2 && !flag3);
			get.SetActive(flag3);
			lockIcon.gameObject.SetActive(!flag2);
			if (flag3)
			{
				getNameText.text = tEmployeeSkill.Name;
				getDescText.text = tEmployeeSkill.Desc;
			}
			else
			{
				nameText.text = tEmployeeSkill.Name;
				descText.text = tEmployeeSkill.Desc;
			}
		}
	}

	private void SetupStatDelta(EmployeeCardModel model, TEmployee employeeData)
	{
		bool flag = model.IsLevelUp && model.PrevLevel > 0;
		_view.Charm_NumLvUp.gameObject.SetActive(flag);
		_view.Persuasion_NumLvUp.gameObject.SetActive(flag);
		_view.Resistance_NumLvUp.gameObject.SetActive(flag);
		if (flag)
		{
			int num = (int)(float)((float)(model.Level - 1) * employeeData.CharmGrowth + (float)employeeData.Charm) - (int)(float)((float)(model.PrevLevel - 1) * employeeData.CharmGrowth + (float)employeeData.Charm);
			int num2 = (int)(float)((float)(model.Level - 1) * employeeData.PersuasionGrowth + (float)employeeData.Persuasion) - (int)(float)((float)(model.PrevLevel - 1) * employeeData.PersuasionGrowth + (float)employeeData.Persuasion);
			int num3 = (int)(float)((float)(model.Level - 1) * employeeData.ResistanceGrowth + (float)employeeData.Resistance) - (int)(float)((float)(model.PrevLevel - 1) * employeeData.ResistanceGrowth + (float)employeeData.Resistance);
			_view.Charm_NumLvUp.gameObject.SetActive(num != 0);
			_view.Persuasion_NumLvUp.gameObject.SetActive(num2 != 0);
			_view.Resistance_NumLvUp.gameObject.SetActive(num3 != 0);
			if (num != 0)
			{
				_view.Charm_NumLvUp.text = string.Format("(+{0})", num);
			}
			if (num2 != 0)
			{
				_view.Persuasion_NumLvUp.text = string.Format("(+{0})", num2);
			}
			if (num3 != 0)
			{
				_view.Resistance_NumLvUp.text = string.Format("(+{0})", num3);
			}
		}
	}
}
