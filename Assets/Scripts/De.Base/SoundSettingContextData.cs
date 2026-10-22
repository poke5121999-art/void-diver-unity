using ProtoBuf;

[ProtoContract]
public class SoundSettingContextData : ISaveData
{
	[ProtoMember(1)]
	public string AccountId { get; set; }

	[ProtoMember(2)]
	public long SaveTimeTicks { get; set; }

	[ProtoMember(3)]
	public long CreateTimeTicks { get; set; }

	[ProtoMember(4)]
	public bool MasterVolumeOn { get; set; }

	[ProtoMember(5)]
	public int MasterVolumeLevel { get; set; }

	[ProtoMember(6)]
	public bool BgmVolumeOn { get; set; }

	[ProtoMember(7)]
	public int BgmVolumeLevel { get; set; }

	[ProtoMember(8)]
	public bool SfxVolumeOn { get; set; }

	[ProtoMember(9)]
	public int SfxVolumeLevel { get; set; }

	public ISaveData Clone()
	{
		return null;
	}

	public void Normalize()
	{
	}
}
