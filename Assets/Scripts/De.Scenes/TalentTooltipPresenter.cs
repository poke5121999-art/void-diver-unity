using NemoLib.AssetManagement;
using NemoLib.Mvp;
using UniRx;
using VContainer;

public class TalentTooltipPresenter : BasePanelPresenter
{
	[InjectModel]
	private TalentTooltipModel _model;

	[View]
	private TalentTooltipView _view;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private GameEventSystem _eventSystem;

	[Inject]
	private PlayerManager _playerManager;

	private CompositeDisposable _disposable;

	protected override BasePanelModel PanelModel => null;

	protected void OnInject(TalentTooltipModel model)
	{
	}

	protected override void OnDestroy()
	{
	}

	private void OnTalentUpdated(Talent talent)
	{
	}
}
