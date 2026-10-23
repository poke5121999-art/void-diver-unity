using NemoLib.Mvp;
using UnityEngine;

public class KeySettingSlotRowPresenter : BaseRowSlotPresenter<KeySettingSlotRowModel, KeySettingSlotModel>
{
	[InjectModel]
	private KeySettingSlotRowModel _model;

	[View]
	private KeySettingSlotRowView _view;

	protected override GameObject[] Slots => _view.Slots;

	protected void OnInject(KeySettingSlotRowModel model)
	{
		InitRow(model);
	}
}
