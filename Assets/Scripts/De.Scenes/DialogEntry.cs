using UniRx;

public class DialogEntry
{
	public DialogSpeakerEntry[] Speakers = new DialogSpeakerEntry[3];

	public int SpeakerIndex;

	public string Message;

	public SelectableMessageData[] SelectableMessageDatas;

	public bool AllowEscapeClose;

	public readonly Subject<int> OnCompleted = new Subject<int>();

	public DialogSpeakerEntry FocusedSpeaker => (SpeakerIndex < 0) ? null : Speakers[SpeakerIndex];

	public bool HasSelection => SelectableMessageDatas != null && SelectableMessageDatas.Length > 0;
}
