using GameCommon.Enum;
using NemoLib.Mvp;
using UniRx;

public class VoteStatePresenter : Presenter
{
	[View]
	private VoteStateView _view;

	[InjectModel]
	private VoteStateModel _model;

	private CompositeDisposable _disposable;

	private void OnInject(VoteStateModel model)
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		model.RxELayout.Subscribe(OnLayoutChanged).AddTo(_disposable);
	}

	private void OnDestroy()
	{
		_disposable?.Dispose();
	}

	private void OnLayoutChanged(EVoteStateLayout layout)
	{
		_view.Yes.SetActive(layout == EVoteStateLayout.Yes);
		_view.No.SetActive(layout == EVoteStateLayout.No);
		_view.Waiting.SetActive(layout == EVoteStateLayout.Waiting);
	}
}
