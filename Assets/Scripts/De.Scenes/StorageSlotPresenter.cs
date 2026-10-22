using NemoLib.Mvp;
using UnityEngine.UI;
using VContainer;

public class StorageSlotPresenter : BaseSlotPresenter<StorageSlotModel>
{
	[InjectModel]
	private StorageSlotModel _model;

	[View]
	private StorageSlotView _view;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private GameManager _gameManager;

	protected override StorageSlotModel SlotModel => null;

	protected override Button Button => null;

	private void OnInject(StorageSlotModel model)
	{
	}
}
