using NemoLib.Mvp;
using UnityEngine;

public abstract class BaseRowSlotModel<TSlotModel> : Model, IClearSelectable where TSlotModel : BaseSlotModel<TSlotModel>
{
	public TSlotModel[] SlotModels;

	// TODO(decomp): generic body has no address in the binary; forwards to every slot model.
	public void ClearSelectable(Transform owner)
	{
		if (SlotModels == null)
		{
			return;
		}
		for (int i = 0; i < SlotModels.Length; i++)
		{
			SlotModels[i]?.ClearSelectable(owner);
		}
	}
}
