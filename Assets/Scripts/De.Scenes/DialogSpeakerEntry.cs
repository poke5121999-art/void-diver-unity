using GameCommon.Enum;

public class DialogSpeakerEntry
{
	public ESpeaker Speaker;

	public int TableId;

	public string ImageName;

	public bool HasPortrait => Speaker != ESpeaker.None && !string.IsNullOrEmpty(ImageName);

	public DialogSpeakerEntry(ESpeaker speaker, int tableId, string imageName)
	{
		Speaker = speaker;
		TableId = tableId;
		ImageName = imageName;
	}
}
