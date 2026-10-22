using NemoLib.AssetManagement;
using NemoLib.Mvp;
using VContainer;

public class ParadoxSlotPresenter : BaseSlotPresenter<ParadoxSlotModel>
{
	[View]
	private ParadoxSlotView _view;

	[InjectModel]
	private ParadoxSlotModel _model;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private GameManager _gameManager;

	private GamePlayer MyPlayer => null;

	protected override ParadoxSlotModel SlotModel => null;

	private void OnInject(ParadoxSlotModel model)
	{
	}
}
