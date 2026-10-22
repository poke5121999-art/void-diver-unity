using NemoLib.Log;
using NemoLib.Mvp;
using VContainer;

public class RoomSlotPresenter : BaseSlotPresenter<RoomSlotModel>
{
	[InjectModel]
	private RoomSlotModel _model;

	[View]
	private RoomSlotView _view;

	[Inject]
	private SeriLogger _logger;

	[Inject]
	private IRoomMatchmaker _roomMatchmaker;

	protected override RoomSlotModel SlotModel => null;

	private void OnInject(RoomSlotModel model)
	{
	}

	private void SetPlayingState()
	{
	}

	private void SetDifficulty()
	{
	}

	private void SetMemberCount()
	{
	}

	private void SetLevel()
	{
	}

	private void SetPingBars()
	{
	}
}
