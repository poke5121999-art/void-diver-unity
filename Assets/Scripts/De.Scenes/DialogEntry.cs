using UniRx;

public class DialogEntry
{
	public DialogSpeakerEntry[] Speakers;

	public int SpeakerIndex;

	public string Message;

	public SelectableMessageData[] SelectableMessageDatas;

	public bool AllowEscapeClose;

	public readonly Subject<int> OnCompleted;

	public DialogSpeakerEntry FocusedSpeaker => null;

	public bool HasSelection => false;
}
