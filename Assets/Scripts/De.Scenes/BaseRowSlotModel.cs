using NemoLib.Mvp;
using UnityEngine;

public abstract class BaseRowSlotModel<TSlotModel> : Model, IClearSelectable where TSlotModel : BaseSlotModel<TSlotModel>
{
	public TSlotModel[] SlotModels;

	public void ClearSelectable(Transform owner)
	{
	}
}
