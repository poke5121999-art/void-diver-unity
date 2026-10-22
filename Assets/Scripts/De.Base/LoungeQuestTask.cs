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

	public bool IsCompleted => false;

	public string ToLocalString()
	{
		return null;
	}
}
