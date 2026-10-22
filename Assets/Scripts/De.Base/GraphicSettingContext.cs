using GameCommon.Enum;
using UniRx;

public class GraphicSettingContext
{
	public readonly ReactiveProperty<EScreenMode> RxScreenMode;

	public readonly ReactiveProperty<EResolution> RxResolution;

	public readonly ReactiveProperty<EQualityLevel> RxQualityLevel;

	public readonly ReactiveProperty<EFrameRate> RxFrameRate;

	public readonly ReactiveProperty<bool> RxVSyncOn;

	public readonly ReactiveProperty<bool> RxCameraShakingOn;

	public readonly ReactiveProperty<int> RxGammaPercent;

	public string AccountId { get; set; }

	public GraphicSettingContext(GraphicSettingContextData data)
	{
	}

	public GraphicSettingContextData ToData()
	{
		return null;
	}
}
