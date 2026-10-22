using ProtoBuf;
using Tracker;

[ProtoContract]
public class DialogReplayRecord : ITrackableDictionaryData<long, DialogReplayRecord>, ITrackableData<DialogReplayRecord>
{
	public long Key => 0L;

	[ProtoMember(1)]
	public EDataOperation Operation { get; set; }

	[ProtoMember(2)]
	public long Id { get; set; }

	[ProtoMember(3)]
	public byte SelectedIndex { get; set; }

	public DialogReplayRecord Clone()
	{
		return null;
	}
}
