using GameCommon.Enum;
using ProtoBuf;

[ProtoContract]
public class GamePlaySettingContextData : ISaveData
{
	public const int DefaultPadSensitivity = 40;

	[ProtoMember(1)]
	public string AccountId { get; set; }

	[ProtoMember(2)]
	public long SaveTimeTicks { get; set; }

	[ProtoMember(3)]
	public long CreateTimeTicks { get; set; }

	[ProtoMember(4)]
	public ELanguage Language { get; set; }

	[ProtoMember(5)]
	public bool AimSupportOn { get; set; }

	[ProtoMember(6)]
	public bool AutoAttackOn { get; set; }

	[ProtoMember(7)]
	public bool LockMouseOn { get; set; }

	[ProtoMember(8)]
	public bool RstickAttackOn { get; set; }

	[ProtoMember(9)]
	public bool RunToggleOn { get; set; }

	[ProtoMember(10)]
	public bool AimLineOn { get; set; }

	[ProtoMember(11)]
	public int PadSensitivity { get; set; }

	public ISaveData Clone()
	{
		return null;
	}

	public void Normalize()
	{
	}
}
