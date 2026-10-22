using NemoLib.Mvp;
using UniRx;

public class SamplePanelPresenter : BasePanelPresenter
{
	[InjectModel]
	private SamplePanelModel _model;

	[View]
	private SamplePanelView _view;

	private CompositeDisposable _disposable;

	protected override BasePanelModel PanelModel => _model;

	protected override void OnDestroy()
	{
		base.OnDestroy();
		_disposable?.Dispose();
	}

	private void OnInject(SamplePanelModel model)
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		ClearNav();
		model.RxIsInputEnabled.Where((bool x) => x).Subscribe(delegate
		{
			OnInputEnabled();
		}).AddTo(_disposable);
		model.RxIsInputEnabled.Where((bool x) => !x).Subscribe(delegate
		{
			OnInputDisabled();
		}).AddTo(_disposable);
	}

	private void InitGamePadNav()
	{
		ClearNav();
	}

	private void OnBack()
	{
		_model.OnBack.OnNext(Unit.Default);
	}
}
