using UniRx;
using System;
using GameCommon.Table;
using UnityEngine;

public class InGamePassiveSkillSlotPresenter : InGameSkillSlotPresenter
{
	private static readonly Color _redColor = new Color(0.77f, 0.43f, 0.37f, 1f);

	protected override void OnInject(InGameSkillSlotModel model)
	{
		base.OnInject(model);
		if (model.UnitModel is CharacterModel characterModel)
		{
			_view.Stack.SetActive(false);
			_view.Stack_Gauge.gameObject.SetActive(false);
			_view.PassiveGauge.SetActive(true);
			switch (characterModel.CharacterId)
			{
			case 100001:
				Set100001();
				break;
			case 100003:
				Set100003();
				break;
			case 100004:
				Set100004();
				break;
			case 100005:
				Set100005();
				break;
			}
		}
	}

	protected override void FixedUpdate()
	{
		if (_model != null)
		{
			UpdateCoolTime();
			UpdateChargeGauge();
			UpdateSkillUsable();
		}
	}

	private void Set100001()
	{
		int maxValue = TBuff.Table[1011].MaxStackCount;
		ImageTweenGauge passiveGauge = _view.PassiveGauge.GetComponent<ImageTweenGauge>();
		_model.UnitModel.RxBuffs.ObserveChange().Where((int _) => _ == 1011).Subscribe(delegate
		{
			int num = (_model.UnitModel.Buffs.TryGetValue(1011, out var value) ? value.StackCount : 0);
			passiveGauge.SetRatio((maxValue > 0) ? ((float)num / (float)maxValue) : 0f);
		}).AddTo(Disposable);
	}

	private void Set100003()
	{
		TSkill skillData = TSkill.Table[10000000];
		_view.PassiveGauge.SetActive(false);
		_view.Stack.SetActive(true);
		_view.Stack_Text.text = skillData.StackCount.ToString();
		Observable.EveryFixedUpdate().Subscribe(delegate
		{
			UpdateBasicAttackStack(10000000, skillData);
		}).AddTo(Disposable);
	}

	private void UpdateBasicAttackStack(int skillId, TSkill skillData)
	{
		if (!_model.UnitModel.Skills.TryGetValue(skillId, out var value))
		{
			return;
		}
		_view.Stack_Text.text = value.Stack.ToString();
		if (value.StackChargeTime >= 0f)
		{
			bool flag = value.Stack < skillData.StackCount;
			_view.Stack_Gauge.gameObject.SetActive(flag);
			if (flag)
			{
				float num = value.StackChargeTime - GameManager.TimeNow;
				float num2 = ((skillData.StackChargeInterval - num > 0f) ? ((skillData.StackChargeInterval - num) / skillData.StackChargeInterval) : 0f);
				_view.Stack_Gauge.value = Mathf.Clamp01(num2);
			}
		}
	}

	private void Set100004()
	{
		int maxValue = TConst.SoulMax.IntValue;
		ImageTweenGauge passiveGauge = _view.PassiveGauge.GetComponent<ImageTweenGauge>();
		_model.UnitModel.RxSoul.Subscribe(delegate(int soul)
		{
			passiveGauge.SetRatio((maxValue > 0) ? ((float)soul / (float)maxValue) : 0f);
		}).AddTo(Disposable);
	}

	private void Set100005()
	{
		ImageTweenGauge passiveGauge = _view.PassiveGauge.GetComponent<ImageTweenGauge>();
		IDisposable reloadGaugeDisposable = null;
		passiveGauge.SetRatio(0f);
		_view.Stack.SetActive(true);
		_view.Stack_Text.text = (_model.UnitModel.Buffs.TryGetValue(1303, out var value) ? value.StackCount : 0).ToString();
		_model.UnitModel.RxBuffs.ObserveChange().Where((int _) => _ == 1303).Subscribe(delegate
		{
			int num = (_model.UnitModel.Buffs.TryGetValue(1303, out var value2) ? value2.StackCount : 0);
			_view.Stack_Text.text = num.ToString();
		}).AddTo(Disposable);
		reloadGaugeDisposable = null;
		_model.UnitModel.RxBuffs.ObserveChange().Where((int _) => _ == 1302).Subscribe(delegate
		{
			reloadGaugeDisposable?.Dispose();
			if (_model.UnitModel.Buffs.TryGetValue(1302, out var buff))
			{
				float firstRemainTime = buff.EndTime - GameManager.TimeNow;
				reloadGaugeDisposable = Observable.EveryFixedUpdate().TakeWhile((long __) => _model.UnitModel.Buffs.ContainsKey(1302)).DoOnCompleted(delegate
				{
					passiveGauge.SetRatio(0f);
				})
					.Subscribe(delegate
					{
						float ratio = 1f;
						if (firstRemainTime > 0f)
						{
							ratio = 1f - (buff.EndTime - GameManager.TimeNow) / firstRemainTime;
						}
						passiveGauge.SetRatio(ratio);
					});
			}
			else
			{
				passiveGauge.SetRatio(0f, immediate: true);
				PlayClearEffect();
			}
		}).AddTo(Disposable);
	}
}
