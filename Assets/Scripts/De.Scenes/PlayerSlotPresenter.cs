using NemoLib.Mvp;
using UnityEngine.UI;
using VContainer;

public class PlayerSlotPresenter : BaseSlotPresenter<PlayerSlotModel>
{
	[InjectModel]
	private PlayerSlotModel _model;

	[View]
	private PlayerSlotView _view;

	[Inject]
	private PlayerManager _playerManager;

	protected override PlayerSlotModel SlotModel => null;

	protected override Button Button => null;

	protected override void Start()
	{
	}

	private void OnInject(PlayerSlotModel model)
	{
	}

	private void UpdateUis()
	{
	}

	private void OnDeleteClick()
	{
	}

	private void OnPlayerSlotIndexChanged(int index)
	{
	}

	private void OnPlayerSlotUpdated(int slotIndex)
	{
	}
}
