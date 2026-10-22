using NemoLib.Mvp;
using UniRx;
using VContainer;

public class HudMissionQuestSlotPresenter : Presenter
{
	[View]
	private HudMissionQuestSlotView _view;

	[InjectModel]
	private HudMissionQuestSlotModel _model;

	[Inject]
	private GameMissionManager _missionManager;

	private CompositeDisposable _disposable;

	private void OnInject(HudMissionQuestSlotModel model)
	{
	}

	private void OnDestroy()
	{
	}

	private void UpdateUI()
	{
	}
}
