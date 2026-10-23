using GameCommon.Enum;
using NemoLib.Mvp;
using UniRx;

public class RoomDifficultySlotModel : Model
{
	public ERoomDifficulty RoomDifficulty;

	public readonly ReactiveProperty<bool> RxIsSelected = new ReactiveProperty<bool>();

	public readonly Subject<RoomDifficultySlotModel> OnSlotClick = new Subject<RoomDifficultySlotModel>();
}
