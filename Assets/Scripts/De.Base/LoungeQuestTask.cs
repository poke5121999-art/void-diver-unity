using System;
using GameCommon.Table;
using GameCommon.Enum;
using ProtoBuf;

[ProtoContract]
public class LoungeQuestTask
{
	private const string COLOR_DONE = "#eeeeee";

	private const string COLOR_NOT_DONE = "#b5b5b5";

	[ProtoMember(1)]
	public int Id { get; set; }

	[ProtoMember(2)]
	public long Progress { get; set; }

	[ProtoMember(3)]
	public long Goal { get; set; }

	public bool IsCompleted => Goal <= Progress;

	public string ToLocalString()
	{
		TLoungeQuestTask tLoungeQuestTask = TLoungeQuestTask.Table[Id];
		if (tLoungeQuestTask.EventCondition.Type != EEventConditionType.LuaProgress)
		{
			string text = string.Format("{0} {1}/{2}", tLoungeQuestTask.Desc, Progress, Goal);
			return text.WithColor((Progress < Goal) ? "#b5b5b5" : "#eeeeee");
		}
		return tLoungeQuestTask.Desc.WithColor((Progress < Goal) ? "#b5b5b5" : "#eeeeee");
	}
}
