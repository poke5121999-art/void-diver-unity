using GameCommon.Table;
using System.Linq;
using GameCommon.Enum;
using ProtoBuf;
using Tracker;

[ProtoContract]
public class LoungeQuest : ITrackableDictionaryData<int, LoungeQuest>, ITrackableData<LoungeQuest>
{
	public int Key => Id;

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

	public bool IsAllTaskCompleted => Tasks != null && Tasks.Length != 0 && Tasks.All((LoungeQuestTask t) => t.IsCompleted);

	public ELoungeQuestCategory Category => TLoungeQuest.Table[Id].Category;

	public bool IsStarted
	{
		get
		{
			if (State == ELoungeQuestState.None)
			{
				return false;
			}
			return State != ELoungeQuestState.NotStarted;
		}
	}

	public bool IsActive => State == ELoungeQuestState.InProgress || State == ELoungeQuestState.NotCompleted;

	public LoungeQuest Clone()
	{
		return new LoungeQuest
		{
			Id = Id,
			State = State,
			Tasks = Tasks?.Select((LoungeQuestTask t) => new LoungeQuestTask
			{
				Id = t.Id,
				Progress = t.Progress,
				Goal = t.Goal
			}).ToArray(),
			DisplayInHud = DisplayInHud,
			CurrentDialogReplayId = CurrentDialogReplayId
		};
	}
}
