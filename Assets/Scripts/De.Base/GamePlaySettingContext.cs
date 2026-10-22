using GameCommon.Enum;
using UniRx;

public class GamePlaySettingContext
{
	public readonly ReactiveProperty<ELanguage> RxLanguage;

	public readonly ReactiveProperty<bool> RxAimSupportOn;

	public readonly ReactiveProperty<bool> RxAutoAttackOn;

	public readonly ReactiveProperty<bool> RxLockMouseOn;

	public readonly ReactiveProperty<bool> RxRstickAttackOn;

	public readonly ReactiveProperty<bool> RxRunToggleOn;

	public readonly ReactiveProperty<bool> RxAimLineOn;

	public readonly ReactiveProperty<int> RxPadSensitivity;

	public string AccountId { get; set; }

	public long SaveTimeTicks { get; set; }

	public long CreateTimeTicks { get; private set; }

	public GamePlaySettingContext(GamePlaySettingContextData data)
	{
	}

	public GamePlaySettingContextData ToData()
	{
		return null;
	}
}
