using GameCommon.Enum;
using NemoLib.Mvp;

public class RoomSettingPopupModel : Model
{
	public string RoomTitle;

	public ERoomDifficulty RoomDifficulty;

	public ERoomPublishType PublishType;

	public bool IsCreate;
}
