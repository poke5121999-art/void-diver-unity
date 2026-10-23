using System;
using NemoLib.Mvp;

public class PlayerRestoreSlotPresenter : BaseSlotPresenter<PlayerRestoreSlotModel>
{
	[InjectModel]
	private PlayerRestoreSlotModel _model;

	[View]
	private PlayerRestoreSlotView _view;

	protected override PlayerRestoreSlotModel SlotModel => _model;

	private void OnInject(PlayerRestoreSlotModel model)
	{
		InitSlot(model);
		_view.TimeText.text = new DateTime(model.SaveTimeTicks, DateTimeKind.Utc).ToLocalTime().ToString("yyyy / MM / dd / HH:mm");
	}
}
