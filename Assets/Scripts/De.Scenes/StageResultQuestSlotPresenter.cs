using NemoLib.Mvp;
using VContainer;

public class StageResultQuestSlotPresenter : Presenter
{
	[View]
	private StageResultQuestSlotView _view;

	[InjectModel]
	private StageResultQuestSlotModel _model;

	[Inject]
	private GameManager _gameManager;

	private void OnInject(StageResultQuestSlotModel model)
	{
	}
}
