using NemoLib.Mvp;
using UnityEngine;

public abstract class BaseRowSlotPresenter<TModel, TSlotModel> : Presenter where TModel : BaseRowSlotModel<TSlotModel> where TSlotModel : BaseSlotModel<TSlotModel>
{
	protected abstract GameObject[] Slots { get; }

	protected void InitRow(TModel model)
	{
	}
}
