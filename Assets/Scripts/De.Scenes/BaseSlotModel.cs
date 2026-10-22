using NemoLib.Mvp;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseSlotModel<T> : Model, IClearSelectable where T : BaseSlotModel<T>
{
	public Selectable Selectable { get; private set; }

	public ReactiveProperty<bool> RxIsSelected { get; }

	public ReactiveProperty<bool> RxIsLocked { get; }

	public Subject<T> OnClick { get; }

	public Subject<T> OnLeftClick { get; }

	public Subject<T> OnRightClick { get; }

	public Subject<T> OnEnter { get; }

	public Subject<T> OnExit { get; }

	public bool IsSelected => false;

	public bool IsLocked => false;

	public void TriggerClick()
	{
	}

	public void TriggerLeftClick()
	{
	}

	public void TriggerRightClick()
	{
	}

	public void TriggerSlotEnter()
	{
	}

	public void TriggerSlotExit()
	{
	}

	public void SetSelectable(Selectable selectable)
	{
	}

	public void ClearSelectable(Transform owner)
	{
	}
}
