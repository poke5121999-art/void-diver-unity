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
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		UpdateUI();
		_missionManager.OnMissionUpdated.Subscribe(delegate
		{
			UpdateUI();
		}).AddTo(_disposable);
	}

	private void OnDestroy()
	{
		_disposable?.Dispose();
	}

	private void UpdateUI()
	{
		if (_missionManager.Missions.TryGetValue(_model.MissionId, out var value))
		{
			_view.Text.text = value.ToLocalString();
			_view.Mark.gameObject.SetActive(value.IsCompleted);
		}
		else
		{
			_view.gameObject.SetActive(false);
		}
	}
}
