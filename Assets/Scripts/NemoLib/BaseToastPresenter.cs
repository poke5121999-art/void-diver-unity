using NemoLib.Mvp;
using UniRx;

public class BaseToastPresenter : Presenter
{
	public Subject<BaseToastPresenter> OnClose { get; } = new Subject<BaseToastPresenter>();

	public virtual string OpenSfx => null;

	private void OnInject(Model model)
	{
	}

	public void Show()
	{
		OnShow();
	}

	protected virtual void OnShow()
	{
	}

	public virtual void Close(object result = null)
	{
		OnClose.OnNext(this);
	}

	public virtual void OnCloseDuplicateToast(string message)
	{
	}
}
