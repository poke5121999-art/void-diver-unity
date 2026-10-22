using NemoLib.Mvp;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseSlotModel<T> : Model, IClearSelectable where T : BaseSlotModel<T>
{
	public Selectable Selectable { get; private set; }

	public ReactiveProperty<bool> RxIsSelected { get; } = new ReactiveProperty<bool>();

	public ReactiveProperty<bool> RxIsLocked { get; } = new ReactiveProperty<bool>();

	public Subject<T> OnClick { get; } = new Subject<T>();

	public Subject<T> OnLeftClick { get; } = new Subject<T>();

	public Subject<T> OnRightClick { get; } = new Subject<T>();

	public Subject<T> OnEnter { get; } = new Subject<T>();

	public Subject<T> OnExit { get; } = new Subject<T>();

	public bool IsSelected => RxIsSelected.Value;

	public bool IsLocked => RxIsLocked.Value;

	// TODO(decomp): generic method bodies have no address in the binary (0x0 in methods.tsv); the Trigger* bodies are inferred from signatures and call sites.
	public void TriggerClick()
	{
		OnClick.OnNext((T)this);
	}

	public void TriggerLeftClick()
	{
		OnLeftClick.OnNext((T)this);
	}

	public void TriggerRightClick()
	{
		OnRightClick.OnNext((T)this);
	}

	public void TriggerSlotEnter()
	{
		OnEnter.OnNext((T)this);
	}

	public void TriggerSlotExit()
	{
		OnExit.OnNext((T)this);
	}

	public void SetSelectable(Selectable selectable)
	{
		Selectable = selectable;
	}

	// TODO(decomp): drops the selectable only when it belongs to the cell `owner`; condition inferred from BaseCell.SetData, which calls this on the model it is about to replace.
	public void ClearSelectable(Transform owner)
	{
		if (Selectable != null && Selectable.transform.IsChildOf(owner))
		{
			Selectable = null;
		}
	}
}
