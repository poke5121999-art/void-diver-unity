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
	}

	private void OnDestroy()
	{
	}

	private void OnLayoutChanged(EVoteStateLayout layout)
	{
	}
}
