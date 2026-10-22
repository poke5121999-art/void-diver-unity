using GameCommon.Enum;
using ProtoBuf;
using Tracker;

[ProtoContract]
public class LoungeQuest : ITrackableDictionaryData<int, LoungeQuest>, ITrackableData<LoungeQuest>
{
	public int Key => 0;

	[ProtoMember(1)]
	public EDataOperation Operation { get; set; }

	[ProtoMember(2)]
	public int Id { get; set; }

	[ProtoMember(3)]
	public ELoungeQuestState State { get; set; }

	[ProtoMember(4)]
	public LoungeQuestTask[] Tasks { get; set; }

	[ProtoMember(5)]
	public bool DisplayInHud { get; set; }

	[ProtoMember(6)]
	public long CurrentDialogReplayId { get; set; }

	public bool IsAllTaskCompleted => false;

	public ELoungeQuestCategory Category => ELoungeQuestCategory.None;

	public bool IsStarted => false;

	public bool IsActive => false;

	public LoungeQuest Clone()
	{
		return null;
	}
}
