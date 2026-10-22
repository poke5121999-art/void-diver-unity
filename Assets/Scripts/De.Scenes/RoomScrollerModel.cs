using UniRx;

public class RoomScrollerModel : BaseScrollerModel<RoomSlotModel>
{
	public Subject<MatchingLobby> OnGameRoomClick = new Subject<MatchingLobby>();
}
