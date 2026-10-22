using System.Linq;
using Cysharp.Threading.Tasks;
using GameCommon.Data;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using UniRx;
using UnityEngine;
using VContainer;

public class TycoonResultPopupPresenter : GamePopupPresenter
{
	[View]
	private TycoonResultPopupView _view;

	[InjectModel]
	private TycoonResultPopupModel _model;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private SoundManager _soundManager;

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	private InputActions.UiActions UiActions => GameInputManager.UiActions;

	public override bool IsInputActionPopup => true;

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		if (!_model.IsPanic)
		{
			_soundManager.PlaySfx("Employee_Purchase_Success");
		}
		else
		{
			_soundManager.PlaySfx("Employee_Panic");
		}
		InitTitle();
		InitEmployeeResult();
		InitArtifact();
		InitSellResult();
		_view.Confirm_Button.OnClickWithSfx().Subscribe(delegate
		{
			OnConfirm();
		}).AddTo(this);
		await UniTask.Yield();
		InitGamePadNav();
	}

	private void InitTitle()
	{
		string name = TTycoonSalesSlot.Table[_model.SlotIndex].Name;
		_view.PopupTitle_TextTitle.text = string.Format(TLocalizedText.TycoonResultTitleFormat.LocalText, name);
	}

	private void InitEmployeeResult()
	{
		_view.Employee_Success.SetActive(!_model.IsPanic);
		_view.Employee_Fail.SetActive(_model.IsPanic);
		TEmployee tEmployee = TEmployee.Table[_model.EmployeeId];
		_view.Employee_EmployeeInfo_TextName.text = tEmployee.Name;
		_view.Employee_EmployeeInfo_TextLevel.text = TLocalizedText.LevelFormat.Format(_model.Level);
		_view.Employee_EmployeeSimpleSlot.Inject(new EmployeeSimpleSlotModel
		{
			EmployeeId = _model.EmployeeId,
			Level = _model.Level,
			Exp = _model.EmployeeExp,
			Stress = _model.Stress,
			IsPanic = _model.IsPanic
		});
		if (!_model.IsPanic)
		{
			PanicGaugeUtility.Apply(_view.Employee_Success_ResultEmployeeStress_PanicSlider, _view.Employee_Success_ResultEmployeeStress_PanicSlider_Gauge_img_back, _view.Employee_Success_ResultEmployeeStress_PanicSlider_Gauge_img_front, _model.Stress, isPanic: false);
			_view.Employee_Success_ResultEmployeeStress_PanicSlider_ConditionFace.sprite = _assetManager.GetIconEmployeeCondition(_model.Stress, _model.IsPanic);
			bool flag = _model.StressSkillIds != null && _model.StressSkillIds.Length > 0;
			_view.Employee_Success_ResultEmployeeStress_EmployeeSkillBubble.SetActive(flag);
			if (flag)
			{
				_view.Employee_Success_ResultEmployeeStress_EmployeeSkillBubble_TextTitle.text = string.Join("\n", _model.StressSkillIds.Select((int id) => TEmployeeSkill.Table[id].Name));
			}
			_view.Employee_Success_ResultEmployeeExp.SetActive(_model.EmployeeExpGain > 0);
			if (_model.EmployeeExpGain > 0)
			{
				_view.Employee_Success_ResultEmployeeExp_Text.text = _model.EmployeeExpGain.ToString("N0");
				EmployeeExpUtility.Apply(_view.Employee_Success_ResultEmployeeExp_ExpSlider, _model.Level, _model.EmployeeExp);
			}
		}
		else
		{
			_view.Employee_Fail_TextTitle.text = _model.FailType.GetFailTypeName();
			_view.Employee_Fail_TextDesc.text = _model.FailType.GetFailTypeDesc();
		}
	}

	private void InitArtifact()
	{
		_view.GoodsSlot.Inject(new GoodsSlotModel(_model.Artifact));
	}

	private void InitSellResult()
	{
		bool flag = _model.IsSold || _model.FailType == ETycoonFailType.ArtifactDamage || _model.FailType == ETycoonFailType.CheapSale;
		bool flag2 = false;
		_view.SellResult_TextTitle.text = _model.Artifact.GetName();
		_view.SellResult_TextSuccess.gameObject.SetActive(flag);
		_view.ResultCost.SetActive(flag);
		_view.SellResult_TextFail.gameObject.SetActive(!flag);
		_view.FailDesc.gameObject.SetActive(!flag);
		_view.ResultExp.SetActive(flag);
		if (!flag)
		{
			SetupPriceDelta(hasSold: false);
		}
		else
		{
			_view.ResultCost_Gold_TextCost.text = _model.ResultCoin.ToString("N0");
			_view.ResultExp_TextCost.text = _model.ResultExp.ToString("N0");
			SetupPriceDelta(flag);
			flag2 = _model.PriceSkillIds != null && _model.PriceSkillIds.Length > 0;
		}
		_view.ResultCost_EmployeeSkillBubble.SetActive(flag2);
		if (flag2)
		{
			_view.ResultCost_EmployeeSkillBubble_TextTitle.text = string.Join("\n", _model.PriceSkillIds.Select((int id) => TEmployeeSkill.Table[id].Name));
			_view.ResultCost_EmployeeSkillBubble_TextPrice.text = _model.SkillCoinBonus.ToString("N0");
			_view.ResultCost_EmployeeSkillBubble_TextPercent.text = string.Format("(+{0}%)", _model.SkillPriceBonusPercent);
		}
	}

	private void SetupPriceDelta(bool hasSold)
	{
		int intValue = TConst.CoinToGoldRate.IntValue;
		int num = ((intValue >= 1) ? ((_model.Artifact == null) ? 0 : (_model.Artifact.SellPriceTotal / intValue)) : 0);
		int num2 = ((hasSold && num > 0) ? (int)((_model.ResultCoin - num) * 100 / num) : 0);
		_view.ResultCost_Gold_LayoutUp.SetActive(num2 > 0);
		_view.ResultCost_Gold_LayoutUp_Up.SetActive(num2 > 0);
		_view.ResultCost_Gold_LayoutDown.SetActive(num2 < 0);
		_view.ResultCost_Gold_LayoutDown_Down.SetActive(num2 < 0);
		if (num2 > 0)
		{
			_view.ResultCost_Gold_LayoutUp_TextPercent.text = string.Format("{0}%", num2);
		}
		else if (num2 < 0)
		{
			_view.ResultCost_Gold_LayoutDown_TextPercent.text = string.Format("{0}%", -num2);
		}
	}

	private void OnConfirm()
	{
		if (_model.ResultCoin > 0)
		{
			_soundManager.PlaySfx("Casher_Coin_Online");
		}
		if (_model.IsPanic)
		{
			if (_model.FailType == ETycoonFailType.ArtifactDamage)
			{
				_soundManager.PlaySfx("Employee_Panic_Destroy");
			}
			else if (_model.FailType == ETycoonFailType.SlotDamage)
			{
				_soundManager.PlaySfx("Employee_Panic_Destroy2");
			}
		}
		Close(true);
	}

	private void InitGamePadNav()
	{
		SetDefaultNav(_view.Confirm_Button);
	}
}
