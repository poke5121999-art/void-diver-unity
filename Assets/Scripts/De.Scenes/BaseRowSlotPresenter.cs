using NemoLib.Mvp;
using UnityEngine;

public abstract class BaseRowSlotPresenter<TModel, TSlotModel> : Presenter where TModel : BaseRowSlotModel<TSlotModel> where TSlotModel : BaseSlotModel<TSlotModel>
{
	protected abstract GameObject[] Slots { get; }

	// shared body @0x1823df890 checked: loop over virtual Slots, SetActive(true) + GetComponent<Presenter>().Inject(model.SlotModels[i]) while i < SlotModels.Length, else SetActive(false)
	protected void InitRow(TModel model)
	{
		for (int i = 0; i < Slots.Length; i++)
		{
			if (i < model.SlotModels.Length)
			{
				Slots[i].SetActive(value: true);
				Slots[i].GetComponent<Presenter>().Inject(model.SlotModels[i]);
			}
			else
			{
				Slots[i].SetActive(value: false);
			}
		}
	}
}
