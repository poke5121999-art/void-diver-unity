using GameCommon.Data;
using ProtoBuf;

[ProtoContract]
[Response]
public class ResPlayCutscene : BaseMessage
{
	[ProtoMember(1)]
	public string CutscenePrefab { get; set; }

	[ProtoMember(2)]
	public ProtoVector3 Position { get; set; }
}
