using NemoLib.Mvp;
using UnityEngine;

public abstract class BaseRowSlotPresenter<TModel, TSlotModel> : Presenter where TModel : BaseRowSlotModel<TSlotModel> where TSlotModel : BaseSlotModel<TSlotModel>
{
	protected abstract GameObject[] Slots { get; }

	// TODO(decomp): generic body has no address in the binary; the loop is the one MonsterSlotRowPresenter.OnInject inlines.
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
