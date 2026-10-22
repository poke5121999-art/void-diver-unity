using NemoLib.Mvp;
using UniRx;

public class BaseScrollerModel<TSlot> : Model where TSlot : BaseSlotModel<TSlot>
{
	public readonly ReactiveProperty<bool> RxIsInputEnabled;

	public readonly Subject<Unit> OutOfTop;

	public readonly Subject<Unit> OutOfBottom;

	public readonly Subject<Unit> OutOfLeft;

	public readonly Subject<Unit> OutOfRight;

	public readonly Subject<TSlot> OnSlotEnter;

	public readonly Subject<TSlot> OnSlotExit;

	public readonly Subject<TSlot> OnSlotClick;

	public readonly Subject<TSlot> OnSlotLeftClick;

	public readonly Subject<TSlot> OnSlotRightClick;

	public ENavDir EntryDirection;
}
