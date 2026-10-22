using GameCommon.Enum;
using NemoLib.Mvp;
using UniRx;

public class VoteStateModel : Model
{
	public readonly ReactiveProperty<EVoteStateLayout> RxELayout = new ReactiveProperty<EVoteStateLayout>();

	public EVoteStateLayout ELayout => RxELayout.Value;
}
