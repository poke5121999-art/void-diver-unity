using NemoLib.Mvp;
using UniRx;
using VContainer;

public class HudLoungeQuestSlotPresenter : Presenter
{
	[View]
	private HudLoungeQuestSlotView _view;

	[InjectModel]
	private HudLoungeQuestSlotModel _model;

	[Inject]
	private GameLoungeQuestManager _loungeQuestManager;

	[Inject]
	private PlayerManager _playerManager;

	private CompositeDisposable _disposable;

	private void OnInject(HudLoungeQuestSlotModel model)
	{
	}

	private void OnDestroy()
	{
	}

	private void UpdateUI()
	{
	}
}
