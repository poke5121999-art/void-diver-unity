using GameCommon.Enum;
using NemoLib.Mvp;
using UniRx;

public class VoteStateModel : Model
{
	public readonly ReactiveProperty<EVoteStateLayout> RxELayout;

	public EVoteStateLayout ELayout => EVoteStateLayout.None;
}
