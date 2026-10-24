using NemoLib.Mvp;
using UnityEngine;

public abstract class BaseRowSlotModel<TSlotModel> : Model, IClearSelectable where TSlotModel : BaseSlotModel<TSlotModel>
{
	public TSlotModel[] SlotModels;

	// shared body @0x1823df800: null-check SlotModels, then SlotModels[i].ClearSelectable(owner) for each i (a null element throws NullReferenceException, no ?.)
	public void ClearSelectable(Transform owner)
	{
		if (SlotModels == null)
		{
			return;
		}
		for (int i = 0; i < SlotModels.Length; i++)
		{
			SlotModels[i].ClearSelectable(owner);
		}
	}
}
