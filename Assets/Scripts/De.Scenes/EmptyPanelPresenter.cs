using NemoLib.Mvp;
using UniRx;
using UnityEngine.UI;

public class EmptyPanelPresenter : BasePanelPresenter
{
	[InjectModel]
	private EmptyPanelModel _model;

	[View]
	private EmptyPanelView _view;

	private CompositeDisposable _disposable;

	protected override BasePanelModel PanelModel => _model;

	protected override void OnDestroy()
	{
		base.OnDestroy();
		_disposable?.Dispose();
	}

	protected void OnInject(EmptyPanelModel model)
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		Selectable button = GetComponent<Button>();
		SetNavVertical(new Selectable[] { button });
		SetDefaultNav(button);
		_view.Text.text = model.Text;
		model.RxIsInputEnabled.Where((bool x) => x).Subscribe(delegate
		{
			OnInputEnabled();
		}).AddTo(_disposable);
		model.RxIsInputEnabled.Where((bool x) => !x).Subscribe(delegate
		{
			OnInputDisabled();
		}).AddTo(_disposable);
	}
}
