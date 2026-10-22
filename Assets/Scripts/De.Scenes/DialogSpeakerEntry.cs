using GameCommon.Enum;

public class DialogSpeakerEntry
{
	public ESpeaker Speaker;

	public int TableId;

	public string ImageName;

	public bool HasPortrait => false;

	public DialogSpeakerEntry(ESpeaker speaker, int tableId, string imageName)
	{
	}
}
