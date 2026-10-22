using GameCommon.Enum;
using UniRx;

public class ItemStashFilterSlotModel : BaseSlotModel<ItemStashFilterSlotModel>
{
	public string InteractionName;

	public int Index;

	public readonly ReactiveProperty<EButtonState> RxButtonState = new ReactiveProperty<EButtonState>();

	public EButtonState State
	{
		get
		{
			return RxButtonState.Value;
		}
		set
		{
			RxButtonState.Value = value;
		}
	}
}
