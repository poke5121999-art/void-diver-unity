using ProtoBuf;

[ProtoContract]
[Notify]
public class NtfStartMission : BaseMessage
{
	[ProtoMember(1)]
	public int[] MissionIds { get; set; }
}
