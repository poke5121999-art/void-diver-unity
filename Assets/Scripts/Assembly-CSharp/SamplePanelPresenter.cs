using NemoLib.Mvp;
using UniRx;

public class SamplePanelPresenter : BasePanelPresenter
{
	[InjectModel]
	private SamplePanelModel _model;

	[View]
	private SamplePanelView _view;

	private CompositeDisposable _disposable;

	protected override BasePanelModel PanelModel => null;

	protected override void OnDestroy()
	{
	}

	private void OnInject(SamplePanelModel model)
	{
	}

	private void InitGamePadNav()
	{
	}

	private void OnBack()
	{
	}
}
