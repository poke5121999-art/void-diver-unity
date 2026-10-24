using NemoLib.Mvp;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VContainer;

// The generic bodies of BaseSlotPresenter<T> are read from the shared <object> instantiations (methodSpec -> genericMethodPointers):
// get_ButtonController @0x1823f9740, get_Button @0x1823f97d0, Start @0x1823f9410, InitSlot @0x1823f8df0, OnClick @0x1823f8fd0, OnSelected @0x1823f9250 (machine code checked).
public abstract class BaseSlotPresenter<TModel> : Presenter where TModel : BaseSlotModel<TModel>
{
	[Inject]
	protected GameInputManager InputManager;

	private Button _button;

	private ButtonController _buttonController;

	protected CompositeDisposable Disposable;

	protected abstract TModel SlotModel { get; }

	protected virtual Button Button
	{
		get
		{
			// plain reference test of the field (no UnityEngine.Object comparison call)
			return _button ?? (_button = GetComponent<Button>());
		}
	}

	protected ButtonController ButtonController
	{
		get
		{
			// plain reference test of the field; the virtual Button getter is null-checked before GetComponent<ButtonController>
			return _buttonController ?? (_buttonController = Button?.GetComponent<ButtonController>());
		}
	}

	protected virtual void Start()
	{
		// each of the five subscriptions re-reads the Button / ButtonController getter and skips when it is null (plain test)
		Button?.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnClick();
		}).AddTo(this);
		Button?.OnLeftClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnLeftClick();
		}).AddTo(this);
		Button?.OnRightClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnRightClick();
		}).AddTo(this);
		ButtonController?.OnEnter.Subscribe(delegate
		{
			OnSlotEnter();
		}).AddTo(this);
		ButtonController?.OnExit.Subscribe(delegate
		{
			OnSlotExit();
		}).AddTo(this);
	}

	protected virtual void OnDestroy()
	{
		Disposable?.Dispose();
	}

	protected void InitSlot(TModel model)
	{
		Disposable?.Dispose();
		Disposable = new CompositeDisposable();
		if (Button != null)
		{
			model.SetSelectable(Button);
		}
		model.RxIsSelected.Subscribe(OnSelected).AddTo(Disposable);
		model.RxIsLocked.Subscribe(OnLocked).AddTo(Disposable);
	}

	protected virtual void OnClick()
	{
		SlotModel.TriggerClick();
	}

	protected virtual void OnLeftClick()
	{
		SlotModel.TriggerLeftClick();
	}

	protected virtual void OnRightClick()
	{
		SlotModel.TriggerRightClick();
	}

	protected virtual void OnSlotEnter()
	{
		SlotModel?.TriggerSlotEnter();
	}

	protected virtual void OnSlotExit()
	{
		SlotModel?.TriggerSlotExit();
	}

	protected virtual void OnSelected(bool isSelected)
	{
		ButtonController?.SetSelected(isSelected);
	}

	protected virtual void OnLocked(bool isLocked)
	{
		ButtonController?.SetLocked(isLocked);
	}
}
