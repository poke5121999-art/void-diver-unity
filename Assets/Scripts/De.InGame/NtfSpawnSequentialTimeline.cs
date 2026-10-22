using GameCommon.Data;
using ProtoBuf;

[ProtoContract]
[Notify]
public class NtfSpawnSequentialTimeline : BaseMessage
{
	[ProtoMember(1)]
	public string PrefabName { get; set; }

	[ProtoMember(2)]
	public ProtoVector3 Position { get; set; }
}
