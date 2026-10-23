using GameCommon.Enum;
using ProtoBuf;

[ProtoContract]
public class GraphicSettingContextData : ISaveData
{
	[ProtoMember(1)]
	public string AccountId { get; set; }

	[ProtoMember(2)]
	public EScreenMode ScreenMode { get; set; }

	[ProtoMember(3)]
	public EResolution Resolution { get; set; }

	[ProtoMember(4)]
	public EQualityLevel QualityLevel { get; set; }

	[ProtoMember(5)]
	public EFrameRate FrameRate { get; set; }

	[ProtoMember(6)]
	public bool VSyncOn { get; set; }

	[ProtoMember(7)]
	public bool CameraShakingOn { get; set; }

	[ProtoMember(8)]
	public int GammaPercent { get; set; }

	public ISaveData Clone()
	{
		return new GraphicSettingContextData
		{
			AccountId = AccountId,
			ScreenMode = ScreenMode,
			Resolution = Resolution,
			QualityLevel = QualityLevel,
			FrameRate = FrameRate,
			VSyncOn = VSyncOn,
			CameraShakingOn = CameraShakingOn,
			GammaPercent = GammaPercent
		};
	}

	public void Normalize()
	{
	}
}
