using NemoLib.Mvp;

public class StageResultPlayerExpSlotPresenter : Presenter
{
	[View]
	private StageResultPlayerExpSlotView _view;

	[InjectModel]
	private StageResultPlayerExpSlotModel _model;

	private void OnInject(Model model)
	{
	}
}
