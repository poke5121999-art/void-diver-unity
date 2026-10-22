using NemoLib.Mvp;
using UniRx;

public class ArtifactDealPanelPresenter : BasePanelPresenter
{
	[InjectModel]
	private ArtifactDealPanelModel _model;

	[View]
	private ArtifactDealPanelView _view;

	private ArtifactDealScrollerModel _scrollerModel;

	private CompositeDisposable _disposable;

	protected override BasePanelModel PanelModel => null;

	protected void OnInject(ArtifactDealPanelModel model)
	{
	}

	protected override void OnDestroy()
	{
	}
}
