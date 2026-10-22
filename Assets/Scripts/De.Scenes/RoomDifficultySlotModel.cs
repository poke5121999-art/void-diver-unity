using GameCommon.Enum;
using NemoLib.Mvp;
using UniRx;

public class RoomDifficultySlotModel : Model
{
	public ERoomDifficulty RoomDifficulty;

	public readonly ReactiveProperty<bool> RxIsSelected;

	public readonly Subject<RoomDifficultySlotModel> OnSlotClick;
}
