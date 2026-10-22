using UniRx;

public class SoundSettingContext
{
	public readonly ReactiveProperty<bool> RxMasterVolumeOn;

	public readonly ReactiveProperty<int> RxMasterVolumeLevel;

	public readonly ReactiveProperty<bool> RxBgmVolumeOn;

	public readonly ReactiveProperty<int> RxBgmVolumeLevel;

	public readonly ReactiveProperty<bool> RxSfxVolumeOn;

	public readonly ReactiveProperty<int> RxSfxVolumeLevel;

	public string AccountId { get; set; }

	public long SaveTimeTicks { get; set; }

	public long CreateTimeTicks { get; private set; }

	public SoundSettingContext(SoundSettingContextData data)
	{
	}

	public SoundSettingContextData ToData()
	{
		return null;
	}
}
