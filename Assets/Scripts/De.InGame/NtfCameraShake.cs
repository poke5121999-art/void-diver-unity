using ProtoBuf;

[ProtoContract]
[Notify]
public class NtfCameraShake : BaseMessage
{
	[ProtoMember(1)]
	public ulong TargetClientId { get; set; }

	[ProtoMember(2)]
	public float ShakeAmplitude { get; set; }

	[ProtoMember(3)]
	public bool IsGlobal { get; set; }

	[ProtoMember(4)]
	public float ShakeFrequency { get; set; }

	[ProtoMember(5)]
	public float ShakeDuration { get; set; }
}
