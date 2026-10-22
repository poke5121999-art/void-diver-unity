using UnityEngine;
using UniRx;
using System;
using GameCommon.Table;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using UnityEngine.UI;
using VContainer;

public class InGameSkillSlotPresenter : BaseSlotPresenter<InGameSkillSlotModel>
{
	[View]
	protected InGameSkillSlotView _view;

	[InjectModel]
	protected InGameSkillSlotModel _model;

	[Inject]
	protected AssetManager AssetManager;

	[Inject]
	protected GameManager GameManager;

	private const string STACK_TEXT_COLOR = "#B5B5B5";

	private const string STACK_TEXT_NOT_RECOVERY_COLOR = "#A45646";

	private const float CLEAR_EFFECT_DURATION = 0.5f;

	private bool _isSkillUsable;

	private float _changeSkillGaugeMax;

	protected override InGameSkillSlotModel SlotModel => _model;

	protected override Button Button => _view.Slot;

	private TSkill SkillData
	{
		get
		{
			if (_model == null || _model.SkillId < 1)
			{
				return null;
			}
			return TSkill.Table[_model.SkillId];
		}
	}

	protected virtual void OnInject(InGameSkillSlotModel model)
	{
		InitSlot(model);
		base.name = string.Format("SkillSlot[{0}]:{1}", model.SlotIndex, model.SkillId);
		_view.Locked.SetActive(model.IsLocked);
		if (model.SkillId < 1)
		{
			_view.Icon.gameObject.SetActive(false);
			_view.Cooltime.SetActive(false);
			_view.Gauge.SetActive(false);
			_view.StressActivate.gameObject.SetActive(false);
			_view.HpActivate.gameObject.SetActive(false);
			_view.HpDeactivate.gameObject.SetActive(false);
			_view.Stack.SetActive(false);
			_view.LimitStack.gameObject.SetActive(false);
			_view.ChargeGauge.SetActive(false);
			_view.PassiveGauge.SetActive(false);
			_view.ClearEffect.SetActive(false);
		}
		else
		{
			_view.Icon.gameObject.SetActive(true);
			_view.Icon.sprite = AssetManager.GetIconSkill(_model.SkillId);
			model.UnitModel.RxHp.Subscribe(OnHpUpdated).AddTo(Disposable);
			model.UnitModel.RxStress.Subscribe(OnStressUpdated).AddTo(Disposable);
			model.UnitModel.RxStamina.Subscribe(OnStaminaUpdated).AddTo(Disposable);
		}
	}

	protected virtual void FixedUpdate()
	{
		if (_model != null)
		{
			UpdateCoolTime();
			UpdateChargeGauge();
			UpdateStack();
			UpdateSkillUsable();
			UpdateChangeSkillGauge();
		}
	}

	private void OnHpUpdated(int hp)
	{
		if (SkillData != null && SkillData.HpCost > 0)
		{
			int hpCost = SkillData.HpCost;
			_view.HpActivate.gameObject.SetActive(hpCost < hp);
			_view.HpDeactivate.gameObject.SetActive(hp <= hpCost);
			string text = SkillData.HpCost.ToString();
			_view.HpDeactivate_Text.text = text;
			_view.HpActivate_Text.text = text;
		}
		else
		{
			_view.HpActivate.gameObject.SetActive(false);
			_view.HpDeactivate.gameObject.SetActive(false);
		}
	}

	private void OnStressUpdated(float stress)
	{
		if (SkillData != null && SkillData.StressCost > 0)
		{
			_view.StressActivate.gameObject.SetActive(true);
			_view.StressActivate_Text.text = SkillData.StressCost.ToString();
		}
		else
		{
			_view.StressActivate.gameObject.SetActive(false);
		}
	}

	private void OnStaminaUpdated(float stamina)
	{

	}

	protected void UpdateCoolTime()
	{
		if (SkillData != null && _model.RemainCoolTime > 0f)
		{
			_view.Cooltime.SetActive(true);
			_view.Cooltime_Image.fillAmount = _model.RemainCoolTime / SkillData.CoolTime;
			_view.Cooltime_Text.text = string.Format("{0:F1}", _model.RemainCoolTime);
		}
		else
		{
			_view.Cooltime.SetActive(false);
		}
	}

	protected void UpdateChargeGauge()
	{
		if (SkillData == null || _model.ChargeAmount < 0f)
		{
			_view.Gauge.SetActive(false);
			_view.ChargeGauge.SetActive(false);
			return;
		}
		_view.Gauge.SetActive(true);
		_view.ChargeGauge.SetActive(true);
		float num = _model.ChargeAmount / (float)SkillData.ChargeCost;
		_view.ChargeGauge_Gauge.fillAmount = num;
		_view.Gauge_Text.text = ((num < 1f) ? string.Format("{0:F1}%", Math.Floor(num * 100f * 10f) / 10.0) : "100%");
	}

	protected void UpdateStack()
	{
		TSkill skillData = SkillData;
		if (skillData != null && (skillData.StackCount >= 2 || skillData.StackChargeInterval != 0f) && _model.UnitModel.Skills.TryGetValue(_model.SkillId, out var value))
		{
			_view.Stack.SetActive(true);
			_view.Stack_Gauge.gameObject.SetActive(false);
			string colorCode = ((SkillData.StackChargeInterval >= 0f) ? "#B5B5B5" : "#A45646");
			_view.Stack_Text.text = value.Stack.ToString().WithColor(colorCode);
			bool flag = false;
			if (value.StackChargeTime >= 0f)
			{
				flag = value.Stack < SkillData.StackCount;
			}
			_view.Stack_Gauge.gameObject.SetActive(flag);
			if (flag)
			{
				float num = value.StackChargeTime - GameManager.TimeNow;
				float value2 = 0f;
				if (SkillData.StackChargeInterval - num > 0f)
				{
					value2 = (SkillData.StackChargeInterval - num) / SkillData.StackChargeInterval;
				}
				_view.Stack_Gauge.value = value2;
			}
		}
		else
		{
			_view.Stack.SetActive(false);
		}
	}

	protected void UpdateSkillUsable()
	{
		bool isSkillUsable = _isSkillUsable;
		_isSkillUsable = _model.SkillId >= 1 && _model.UnitModel.CheckSkill(_model.SkillId);
		if (!isSkillUsable && _isSkillUsable)
		{
			PlayClearEffect();
		}
		_view.Slot.animator.SetInteger("Layout", _isSkillUsable ? 0 : 4);
	}

	protected void PlayClearEffect()
	{
		_view.ClearEffect.SetActive(true);
		Observable.Timer(TimeSpan.FromSeconds(0.5)).Subscribe(delegate
		{
			_view.ClearEffect.SetActive(false);
		}).AddTo(Disposable);
	}

	private void UpdateChangeSkillGauge()
	{
		if (_model != null && _model.SkillId >= 1)
		{
			float changeSkillBuffEndTime = _model.UnitModel.GetChangeSkillBuffEndTime(_model.SkillId);
			if (changeSkillBuffEndTime >= 0f)
			{
				float num = Mathf.Max(changeSkillBuffEndTime - GameManager.TimeNow, 0f);
				if (_changeSkillGaugeMax < 0f)
				{
					_changeSkillGaugeMax = num;
				}
				_view.PassiveGauge.SetActive(true);
				_view.PassiveGauge_Gauge.fillAmount = ((_changeSkillGaugeMax > 0f) ? Mathf.Clamp01(num / _changeSkillGaugeMax) : 0f);
				return;
			}
		}
		_view.PassiveGauge.SetActive(false);
		_changeSkillGaugeMax = -1f;
	}
}
