using GameCommon.Enum;
using UniRx;

public class GraphicSettingContext
{
	public readonly ReactiveProperty<EScreenMode> RxScreenMode = new ReactiveProperty<EScreenMode>();

	public readonly ReactiveProperty<EResolution> RxResolution = new ReactiveProperty<EResolution>();

	public readonly ReactiveProperty<EQualityLevel> RxQualityLevel = new ReactiveProperty<EQualityLevel>();

	public readonly ReactiveProperty<EFrameRate> RxFrameRate = new ReactiveProperty<EFrameRate>();

	public readonly ReactiveProperty<bool> RxVSyncOn = new ReactiveProperty<bool>();

	public readonly ReactiveProperty<bool> RxCameraShakingOn = new ReactiveProperty<bool>();

	public readonly ReactiveProperty<int> RxGammaPercent = new ReactiveProperty<int>();

	public string AccountId { get; set; }

	public GraphicSettingContext(GraphicSettingContextData data)
	{
		AccountId = data.AccountId;
		RxScreenMode.Value = data.ScreenMode;
		RxResolution.Value = data.Resolution;
		RxQualityLevel.Value = data.QualityLevel;
		RxFrameRate.Value = data.FrameRate;
		RxVSyncOn.Value = data.VSyncOn;
		RxCameraShakingOn.Value = data.CameraShakingOn;
		RxGammaPercent.Value = data.GammaPercent;
	}

	public GraphicSettingContextData ToData()
	{
		return new GraphicSettingContextData
		{
			AccountId = AccountId,
			ScreenMode = RxScreenMode.Value,
			Resolution = RxResolution.Value,
			QualityLevel = RxQualityLevel.Value,
			FrameRate = RxFrameRate.Value,
			VSyncOn = RxVSyncOn.Value,
			CameraShakingOn = RxCameraShakingOn.Value,
			GammaPercent = RxGammaPercent.Value
		};
	}
}
