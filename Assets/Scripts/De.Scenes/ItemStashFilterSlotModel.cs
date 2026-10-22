using GameCommon.Enum;
using UniRx;

public class ItemStashFilterSlotModel : BaseSlotModel<ItemStashFilterSlotModel>
{
	public string InteractionName;

	public int Index;

	public readonly ReactiveProperty<EButtonState> RxButtonState;

	public EButtonState State
	{
		get
		{
			return EButtonState.Normal;
		}
		set
		{
		}
	}
}
