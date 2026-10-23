using GameCommon.Enum;
using UniRx;

public class GamePlaySettingContext
{
	public readonly ReactiveProperty<ELanguage> RxLanguage = new ReactiveProperty<ELanguage>();

	public readonly ReactiveProperty<bool> RxAimSupportOn = new ReactiveProperty<bool>();

	public readonly ReactiveProperty<bool> RxAutoAttackOn = new ReactiveProperty<bool>();

	public readonly ReactiveProperty<bool> RxLockMouseOn = new ReactiveProperty<bool>();

	public readonly ReactiveProperty<bool> RxRstickAttackOn = new ReactiveProperty<bool>();

	public readonly ReactiveProperty<bool> RxRunToggleOn = new ReactiveProperty<bool>();

	public readonly ReactiveProperty<bool> RxAimLineOn = new ReactiveProperty<bool>();

	public readonly ReactiveProperty<int> RxPadSensitivity = new ReactiveProperty<int>();

	public string AccountId { get; set; }

	public long SaveTimeTicks { get; set; }

	public long CreateTimeTicks { get; private set; }

	public GamePlaySettingContext(GamePlaySettingContextData data)
	{
		AccountId = data.AccountId;
		SaveTimeTicks = data.SaveTimeTicks;
		CreateTimeTicks = data.CreateTimeTicks;
		RxLanguage.Value = data.Language;
		RxAimSupportOn.Value = data.AimSupportOn;
		RxAutoAttackOn.Value = data.AutoAttackOn;
		RxLockMouseOn.Value = data.LockMouseOn;
		RxRstickAttackOn.Value = data.RstickAttackOn;
		RxRunToggleOn.Value = data.RunToggleOn;
		RxAimLineOn.Value = data.AimLineOn;
		RxPadSensitivity.Value = data.PadSensitivity;
	}

	public GamePlaySettingContextData ToData()
	{
		return new GamePlaySettingContextData
		{
			AccountId = AccountId,
			SaveTimeTicks = SaveTimeTicks,
			CreateTimeTicks = CreateTimeTicks,
			Language = RxLanguage.Value,
			AimSupportOn = RxAimSupportOn.Value,
			AutoAttackOn = RxAutoAttackOn.Value,
			LockMouseOn = RxLockMouseOn.Value,
			RstickAttackOn = RxRstickAttackOn.Value,
			RunToggleOn = RxRunToggleOn.Value,
			AimLineOn = RxAimLineOn.Value,
			PadSensitivity = RxPadSensitivity.Value
		};
	}
}
