using NemoLib.Mvp;
using UniRx;
using UnityEngine.UI;
using VContainer;

public abstract class BaseSlotPresenter<TModel> : Presenter where TModel : BaseSlotModel<TModel>
{
	[Inject]
	protected GameInputManager InputManager;

	private Button _button;

	private ButtonController _buttonController;

	protected CompositeDisposable Disposable;

	protected abstract TModel SlotModel { get; }

	protected virtual Button Button => null;

	protected ButtonController ButtonController => null;

	protected virtual void Start()
	{
	}

	protected virtual void OnDestroy()
	{
	}

	protected void InitSlot(TModel model)
	{
	}

	protected virtual void OnClick()
	{
	}

	protected virtual void OnLeftClick()
	{
	}

	protected virtual void OnRightClick()
	{
	}

	protected virtual void OnSlotEnter()
	{
	}

	protected virtual void OnSlotExit()
	{
	}

	protected virtual void OnSelected(bool isSelected)
	{
	}

	protected virtual void OnLocked(bool isLocked)
	{
	}
}
