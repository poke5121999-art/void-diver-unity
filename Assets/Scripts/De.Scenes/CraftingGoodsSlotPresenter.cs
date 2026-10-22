using GameCommon.Table;
using NemoLib.Mvp;
using UnityEngine.UI;
using VContainer;

public class CraftingGoodsSlotPresenter : BaseSlotPresenter<CraftingGoodsSlotModel>
{
	[InjectModel]
	private CraftingGoodsSlotModel _model;

	[View]
	private CraftingGoodsSlotView _view;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private GameManager _gameManager;

	protected override CraftingGoodsSlotModel SlotModel => null;

	protected override Button Button => null;

	private TCrafting CraftingData => null;

	private bool HasEnoughMaterials => false;

	private void OnInject(CraftingGoodsSlotModel model)
	{
	}

	private void OnUpdateUis()
	{
	}
}
