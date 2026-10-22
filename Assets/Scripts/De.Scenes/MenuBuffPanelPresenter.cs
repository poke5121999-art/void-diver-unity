using NemoLib.Mvp;
using UniRx;
using VContainer;

public class MenuBuffPanelPresenter : BasePanelPresenter
{
	[View]
	private MenuBuffPanelView _view;

	[InjectModel]
	private MenuBuffPanelModel _model;

	[Inject]
	private GameManager _gameManager;

	private MenuBuffScrollerModel _scrollerModel;

	private EmptyPanelModel _emptyPanelModel;

	private CompositeDisposable _disposable;

	protected override BasePanelModel PanelModel => null;

	protected override void OnDestroy()
	{
	}

	private void OnInject(MenuBuffPanelModel model)
	{
	}

	protected override void OnInputEnabled()
	{
	}

	protected override void OnInputDisabled()
	{
	}

	private void OnVisibleCountChanged(int count)
	{
	}
}
