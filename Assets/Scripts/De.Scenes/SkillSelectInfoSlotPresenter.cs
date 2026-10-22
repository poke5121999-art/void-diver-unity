using GameCommon.Table;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using UnityEngine.UI;
using VContainer;

public class SkillSelectInfoSlotPresenter : BaseSlotPresenter<SkillSelectInfoSlotModel>
{
	[View]
	private SkillSelectInfoSlotView _view;

	[InjectModel]
	private SkillSelectInfoSlotModel _model;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private GameManager _gameManager;

	protected override SkillSelectInfoSlotModel SlotModel => null;

	protected override Button Button => null;

	private void OnInject(SkillSelectInfoSlotModel model)
	{
		InitSlot(model);
		Refresh();
	}

	private void Refresh()
	{
		TSkill tSkill = TSkill.Table[_model.SkillId];
		bool flag = _gameManager.MyPlayer.IsModeSkill(_model.SkillId);
		_view.ModLine.SetActive(flag);
		_view.Icon.sprite = _assetManager.GetIconSkill(_model.SkillId);
		_view.TitleGroup_TitleText.text = tSkill.Name;
		_view.TitleGroup_ModText.gameObject.SetActive(flag);
		if (flag)
		{
			_view.TitleGroup_ModText.text = TLocalizedText.SkillModeBaseFormat.Format(tSkill.Name);
		}
		float coolTime = tSkill.CoolTime;
		float hpCost = tSkill.HpCost;
		float stressCost = tSkill.StressCost;
		_view.Condition.SetActive(stressCost > 0f || hpCost > 0f || coolTime > 0f);
		_view.Cooltime.SetActive(coolTime > 0f);
		if (coolTime > 0f)
		{
			_view.Cooltime_Text.text = TLocalizedText.SecondsFormat.Format(tSkill.CoolTime);
		}
		_view.Hp.SetActive(hpCost > 0f);
		if (hpCost > 0f)
		{
			_view.Hp_Text.text = TLocalizedText.SkillHpCostFormat.Format(hpCost);
		}
		_view.Stress.SetActive(stressCost > 0f);
		if (stressCost > 0f)
		{
			_view.Stress_Text.text = TLocalizedText.SkillStressCostFormat.Format(stressCost);
		}
		_view.DescText.text = tSkill.Desc;
		bool active = _model.EquippedIndex >= 0 && _model.EquippedIndex == _model.TrialIndex;
		_view.ActiveOtherSlot.SetActive(active);
	}
}
