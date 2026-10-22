using GameCommon.Data;
using ProtoBuf;

[ProtoContract]
public class ConnectionData
{
	[ProtoMember(1)]
	public AppVersion Version { get; set; }
}
