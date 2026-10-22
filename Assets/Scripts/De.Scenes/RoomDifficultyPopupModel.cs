using GameCommon.Enum;
using NemoLib.Mvp;

public class RoomDifficultyPopupModel : Model
{
	public ERoomDifficulty Difficulty { get; set; }

	public bool PreventMaxDurabilityLoss { get; set; }
}
