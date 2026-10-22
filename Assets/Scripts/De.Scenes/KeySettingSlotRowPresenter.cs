using NemoLib.Mvp;
using UnityEngine;

public class KeySettingSlotRowPresenter : BaseRowSlotPresenter<KeySettingSlotRowModel, KeySettingSlotModel>
{
	[InjectModel]
	private KeySettingSlotRowModel _model;

	[View]
	private KeySettingSlotRowView _view;

	protected override GameObject[] Slots => null;

	protected void OnInject(KeySettingSlotRowModel model)
	{
	}
}
