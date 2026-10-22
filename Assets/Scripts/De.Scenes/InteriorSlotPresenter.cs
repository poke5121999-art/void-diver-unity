using GameCommon.Data;
using GameCommon.Table;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using VContainer;

public class InteriorSlotPresenter : BaseSlotPresenter<InteriorSlotModel>
{
	[InjectModel]
	private InteriorSlotModel _model;

	[View]
	private InteriorSlotView _view;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private GameContext _gameContext;

	[Inject]
	private GameLoungeManager _gameLoungeManager;

	[Inject]
	private AssetManager _assetManager;

	protected override InteriorSlotModel SlotModel => null;

	private void OnInject(InteriorSlotModel model)
	{
	}

	private void SetupArea(TInteriorShop shop)
	{
	}

	private void SetupDeco(TInteriorShop shop)
	{
	}

	private void SetupNpcLevel(TInteriorShop shop)
	{
	}

	private void SetupLocked(EventCondition[] unlockConditions, bool isUnlocked, bool isAreaLocked = false)
	{
	}

	private void SetupCost(int consumeGold, int consumeCoin, bool isUnlocked)
	{
	}

	private bool IsAreaLocked(TInteriorShop shop)
	{
		return false;
	}
}
