using UniRx;

public class SoundSettingContext
{
	public readonly ReactiveProperty<bool> RxMasterVolumeOn = new ReactiveProperty<bool>();

	public readonly ReactiveProperty<int> RxMasterVolumeLevel = new ReactiveProperty<int>();

	public readonly ReactiveProperty<bool> RxBgmVolumeOn = new ReactiveProperty<bool>();

	public readonly ReactiveProperty<int> RxBgmVolumeLevel = new ReactiveProperty<int>();

	public readonly ReactiveProperty<bool> RxSfxVolumeOn = new ReactiveProperty<bool>();

	public readonly ReactiveProperty<int> RxSfxVolumeLevel = new ReactiveProperty<int>();

	public string AccountId { get; set; }

	public long SaveTimeTicks { get; set; }

	public long CreateTimeTicks { get; private set; }

	public SoundSettingContext(SoundSettingContextData data)
	{
		AccountId = data.AccountId;
		SaveTimeTicks = data.SaveTimeTicks;
		CreateTimeTicks = data.CreateTimeTicks;
		RxMasterVolumeOn.Value = data.MasterVolumeOn;
		RxMasterVolumeLevel.Value = data.MasterVolumeLevel;
		RxBgmVolumeOn.Value = data.BgmVolumeOn;
		RxBgmVolumeLevel.Value = data.BgmVolumeLevel;
		RxSfxVolumeOn.Value = data.SfxVolumeOn;
		RxSfxVolumeLevel.Value = data.SfxVolumeLevel;
	}

	public SoundSettingContextData ToData()
	{
		return new SoundSettingContextData
		{
			AccountId = AccountId,
			SaveTimeTicks = SaveTimeTicks,
			CreateTimeTicks = CreateTimeTicks,
			MasterVolumeOn = RxMasterVolumeOn.Value,
			MasterVolumeLevel = RxMasterVolumeLevel.Value,
			BgmVolumeOn = RxBgmVolumeOn.Value,
			BgmVolumeLevel = RxBgmVolumeLevel.Value,
			SfxVolumeOn = RxSfxVolumeOn.Value,
			SfxVolumeLevel = RxSfxVolumeLevel.Value
		};
	}
}
