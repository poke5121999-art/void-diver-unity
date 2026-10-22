using NemoLib.AssetManagement;
using NemoLib.Mvp;
using UnityEngine.UI;
using VContainer;

public class InGameWeaponSwapSlotPresenter : BaseSlotPresenter<InGameWeaponSwapSlotModel>
{
	[InjectModel]
	private InGameWeaponSwapSlotModel _model;

	[View]
	private InGameWeaponSwapSlotView _view;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private GameManager _gameManager;

	private int _lastActiveWeaponIndex;

	protected override InGameWeaponSwapSlotModel SlotModel => null;

	protected override Button Button => null;

	private GamePlayer Player => null;

	private void OnInject(InGameWeaponSwapSlotModel model)
	{
	}

	private void RefreshWeaponIcon(Character character)
	{
	}
}
