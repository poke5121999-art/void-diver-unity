using NemoLib.AssetManagement;
using GameCommon.Table;
using NemoLib.Mvp;
using UnityEngine;
using VContainer;

public class InGameSkillTooltipPresenter : BasePanelPresenter
{
	[View]
	private InGameSkillTooltipView _view;

	[InjectModel]
	private InGameSkillTooltipModel _model;

	[Inject]
	private AssetManager _assetManager;

	protected override BasePanelModel PanelModel => _model;

	protected void OnInject(InGameSkillTooltipModel model)
	{
		TSkill tSkill = TSkill.Table[model.SkillId];
		_view.Title_Name.text = tSkill.Name;
		bool flag = tSkill.HpCost > 0 || tSkill.StressCost > 0 || tSkill.StaminaCost > 0 || tSkill.CoolTime > 0f || tSkill.StackChargeInterval > 0f;
		_view.InfoGroup.SetActive(flag);
		if (flag)
		{
			int hpCost = tSkill.HpCost;
			int stressCost = tSkill.StressCost;
			int staminaCost = tSkill.StaminaCost;
			float num = tSkill.CoolTime;
			float num2 = tSkill.StackChargeInterval;
			if (model.ShowModifiedValues && model.UnitModel != null)
			{
				model.UnitModel.AdjustSkillCosts(tSkill, ref hpCost, ref stressCost, ref staminaCost);
				float num3 = Mathf.Min(model.UnitModel.CooldownReductionPercent, TConst.MaxCooldownReductionPercent.FloatValue);
				float num4 = (100f - num3) / 100f;
				if (num > 0f)
				{
					num = Mathf.Max(tSkill.CoolTime * num4, TConst.MinCoolTime.FloatValue);
				}
				if (num2 > 0f)
				{
					num2 = Mathf.Max(tSkill.StackChargeInterval * num4, TConst.MinCoolTime.FloatValue);
				}
			}
			_view.HP.SetActive(tSkill.HpCost > 0);
			_view.HP_Value.text = $"{hpCost}";
			_view.MP.SetActive(tSkill.StressCost > 0);
			_view.MP_Value.text = $"{stressCost}";
			_view.SP.SetActive(tSkill.StaminaCost > 0);
			_view.SP_Value.text = $"{staminaCost}";
			_view.Cooltime.SetActive(tSkill.CoolTime > 0f);
			_view.Cooltime_Value.text = string.Format(TLocalizedText.SecondsFormat.LocalText, num);
			_view.StackCooltime.SetActive(tSkill.StackChargeInterval > 0f);
			_view.StackCooltime_Value.text = string.Format(TLocalizedText.SecondsFormat.LocalText, num2);
		}
		_view.Desc_Text.text = tSkill.Desc;
	}
}
