using ProtoBuf;

[ProtoContract]
[Notify]
public class NtfDisplayNoticeToast : BaseMessage
{
	[ProtoMember(1)]
	public string TextKey { get; set; }

	[ProtoMember(2)]
	public long FontSize { get; set; }
}
