using NemoLib.AssetManagement;
using NemoLib.Mvp;
using UnityEngine.UI;
using VContainer;

public class MenuBuffSlotPresenter : BaseSlotPresenter<MenuBuffSlotModel>
{
	[InjectModel]
	private MenuBuffSlotModel _model;

	[View]
	private MenuBuffSlotView _view;

	[Inject]
	private AssetManager _assetManager;

	protected override MenuBuffSlotModel SlotModel => null;

	protected override Button Button => null;

	private UnitBuff UnitBuff => null;

	private void OnInject(MenuBuffSlotModel model)
	{
	}
}
