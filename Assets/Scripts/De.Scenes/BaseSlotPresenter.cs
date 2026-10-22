using NemoLib.Mvp;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VContainer;

// TODO(decomp): the generic bodies of BaseSlotPresenter<T> have no address in the binary (0x0 in methods.tsv).
// Everything below is inferred from the member signatures, ButtonController and the subclasses that override or call into it.
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
			if (_button == null)
			{
				_button = GetComponent<Button>();
			}
			return _button;
		}
	}

	protected ButtonController ButtonController
	{
		get
		{
			if (_buttonController == null)
			{
				_buttonController = GetComponent<ButtonController>();
			}
			return _buttonController;
		}
	}

	protected virtual void Start()
	{
		Button.OnClickAsObservable().Subscribe(delegate
		{
			OnClick();
		}).AddTo(this);
		Button.OnPointerClickAsObservable().Subscribe(delegate(PointerEventData eventData)
		{
			if (eventData.button == PointerEventData.InputButton.Left)
			{
				OnLeftClick();
			}
			else if (eventData.button == PointerEventData.InputButton.Right)
			{
				OnRightClick();
			}
		}).AddTo(this);
		if (ButtonController != null)
		{
			ButtonController.OnEnter.Subscribe(delegate
			{
				OnSlotEnter();
			}).AddTo(this);
			ButtonController.OnExit.Subscribe(delegate
			{
				OnSlotExit();
			}).AddTo(this);
		}
	}

	protected virtual void OnDestroy()
	{
		Disposable?.Dispose();
	}

	protected void InitSlot(TModel model)
	{
		Disposable?.Dispose();
		Disposable = new CompositeDisposable();
		model.SetSelectable(Button);
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
		SlotModel.TriggerSlotEnter();
	}

	protected virtual void OnSlotExit()
	{
		SlotModel.TriggerSlotExit();
	}

	protected virtual void OnSelected(bool isSelected)
	{
		if (ButtonController != null)
		{
			ButtonController.SetSelected(isSelected);
		}
	}

	protected virtual void OnLocked(bool isLocked)
	{
		if (ButtonController != null)
		{
			ButtonController.SetLocked(isLocked);
		}
	}
}
