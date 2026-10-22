using NemoLib.Mvp;
using UniRx;

public class BaseToastPresenter : Presenter
{
	public Subject<BaseToastPresenter> OnClose { get; }

	public virtual string OpenSfx => null;

	private void OnInject(Model model)
	{
	}

	public void Show()
	{
	}

	protected virtual void OnShow()
	{
	}

	public virtual void Close(object result = null)
	{
	}

	public virtual void OnCloseDuplicateToast(string message)
	{
	}
}
