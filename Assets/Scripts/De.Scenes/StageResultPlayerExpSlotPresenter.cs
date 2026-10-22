using NemoLib.Mvp;

public class StageResultPlayerExpSlotPresenter : Presenter
{
	[View]
	private StageResultPlayerExpSlotView _view;

	[InjectModel]
	private StageResultPlayerExpSlotModel _model;

	private void OnInject(Model model)
	{
		_view.UserName.text = _model.UserName;
		_view.ExpEscape_Value.text = "+" + _model.ExpFromEndStage.ToString();
		_view.ExpMonsterKill_Value.text = "+" + _model.ExpFromMonsterKill.ToString();
		_view.ExpRewardBox_Value.text = "+" + _model.ExpFromRewardBox.ToString();
		_view.ExpDifficulty_Value.text = "+" + _model.ExpFromDifficulty.ToString();
		_view.ExpTalent_Value.text = "+" + _model.ExpFromTalent.ToString();
		_view.ExpDifficulty.gameObject.SetActive(_model.ExpFromDifficulty > 0);
		_view.ExpTalent.gameObject.SetActive(_model.ExpFromTalent > 0);
	}
}
