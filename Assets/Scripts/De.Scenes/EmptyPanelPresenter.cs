using NemoLib.Mvp;
using UniRx;

public class EmptyPanelPresenter : BasePanelPresenter
{
	[InjectModel]
	private EmptyPanelModel _model;

	[View]
	private EmptyPanelView _view;

	private CompositeDisposable _disposable;

	protected override BasePanelModel PanelModel => null;

	protected override void OnDestroy()
	{
	}

	protected void OnInject(EmptyPanelModel model)
	{
	}
}
