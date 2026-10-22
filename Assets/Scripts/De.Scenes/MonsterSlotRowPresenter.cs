using NemoLib.Mvp;
using UnityEngine;

public class MonsterSlotRowPresenter : BaseRowSlotPresenter<MonsterSlotRowModel, MonsterSlotModel>
{
	[InjectModel]
	private MonsterSlotRowModel _model;

	[View]
	private MonsterSlotRowView _view;

	protected override GameObject[] Slots => _view.Slots;

	protected void OnInject(MonsterSlotRowModel model)
	{
		InitRow(model);
	}
}
