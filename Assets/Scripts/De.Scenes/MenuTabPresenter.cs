using NemoLib.Mvp;
using UnityEngine.UI;

public class MenuTabPresenter : BaseSlotPresenter<MenuTabModel>
{
	[View]
	private MenuTabView _view;

	[InjectModel]
	private MenuTabModel _model;

	protected override MenuTabModel SlotModel => null;

	protected override Button Button => null;

	private void OnInject(MenuTabModel model)
	{
	}
}
