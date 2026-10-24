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

	// Generic bodies are the shared <object> instantiations (found via methodSpec -> genericMethodPointers): TriggerClick @0x1823f8850, TriggerRightClick @0x1823f8970, TriggerSlotEnter @0x1823f8a00 ...; each is Subject.OnNext((T)this) on the subject field at +0x28/+0x38/+0x40 (machine code checked)
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

	// @0x1823f8770: Selectable != null && (owner == null || Selectable.transform.IsChildOf(owner)), then Selectable = null
	public void ClearSelectable(Transform owner)
	{
		if (Selectable != null && (owner == null || Selectable.transform.IsChildOf(owner)))
		{
			Selectable = null;
		}
	}
}
