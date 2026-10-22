using NemoLib.Mvp;
using UnityEngine.UI;

public class MenuTabPresenter : BaseSlotPresenter<MenuTabModel>
{
	[View]
	private MenuTabView _view;

	[InjectModel]
	private MenuTabModel _model;

	protected override MenuTabModel SlotModel => _model;

	protected override Button Button => _view.Button;

	private void OnInject(MenuTabModel model)
	{
		InitSlot(model);
	}
}
