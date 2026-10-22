using GameCommon.Table;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using VContainer;

public class SkillInfoDisplaySlotPresenter : Presenter
{
	[InjectModel]
	private SkillInfoDisplaySlotModel _model;

	[View]
	private SkillInfoDisplaySlotView _view;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private GameManager _gameManager;

	private void OnInject(SkillInfoDisplaySlotModel model)
	{
		Refresh();
	}

	private void Refresh()
	{
		TSkill tSkill = TSkill.Table[_model.SkillId];
		bool flag = _gameManager.MyPlayer.IsModeSkill(_model.SkillId);
		_view.Icon.sprite = _assetManager.GetIconSkill(_model.SkillId);
		_view.Arrow.gameObject.SetActive(_model.ShowArrow);
		_view.TitleGroup_TitleText.text = tSkill.Name;
		_view.TitleGroup_ModText.gameObject.SetActive(flag);
		if (flag)
		{
			_view.TitleGroup_ModText.text = TLocalizedText.SkillModeBaseFormat.Format(tSkill.Name);
		}
		float coolTime = tSkill.CoolTime;
		int hpCost = tSkill.HpCost;
		int stressCost = tSkill.StressCost;
		_view.Condition.SetActive(stressCost > 0 || hpCost > 0 || coolTime > 0f);
		_view.Cooltime.SetActive(coolTime > 0f);
		if (coolTime > 0f)
		{
			_view.Cooltime_Text.text = TLocalizedText.SecondsFormat.Format(coolTime);
		}
		_view.Hp.SetActive(hpCost > 0);
		if (hpCost > 0)
		{
			_view.Hp_Text.text = TLocalizedText.SkillHpCostFormat.Format(hpCost);
		}
		_view.Stress.SetActive(stressCost > 0);
		if (stressCost > 0)
		{
			_view.Stress_Text.text = TLocalizedText.SkillStressCostFormat.Format(stressCost);
		}
		_view.DescText.text = tSkill.Desc;
	}
}
