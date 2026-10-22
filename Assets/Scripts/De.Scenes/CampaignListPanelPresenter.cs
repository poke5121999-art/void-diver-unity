using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using VContainer;

public class CampaignListPanelPresenter : Presenter
{
	[View]
	private CampaignListPanelView _view;

	[InjectModel]
	private CampaignListPanelModel _model;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private GameInputManager _inputManager;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private GameCampaignManager _campaignManager;

	[Inject]
	private GameContext _gameContext;

	private CampaignScrollerModel _scrollerModel;

	private CompositeDisposable _disposable;

	private void OnInject(CampaignListPanelModel model)
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		_scrollerModel = new CampaignScrollerModel();
		_scrollerModel.Category = model.Category;
		_view.Scroller.GetComponent<CampaignScrollerPresenter>().Inject(_scrollerModel);
		_scrollerModel.OnSlotClick.Subscribe(OnCampaignClick).AddTo(_disposable);
		model.RxIsInputEnabled.Where((bool _) => _).Subscribe(delegate
		{
			OnInputEnabled();
		}).AddTo(_disposable);
		model.RxIsInputEnabled.Where((bool _) => !_).Subscribe(delegate
		{
			OnInputDisabled();
		}).AddTo(_disposable);
	}

	private void OnDestroy()
	{
		_disposable?.Dispose();
	}

	private void OnCampaignClick(CampaignSlotModel slotModel)
	{
		_model.OnSlotClick.OnNext(slotModel.CampaignId);
	}

	private void OnInputEnabled()
	{
		_scrollerModel.RxIsInputEnabled.Value = true;
	}

	private void OnInputDisabled()
	{
		_scrollerModel.RxIsInputEnabled.Value = false;
	}
}
