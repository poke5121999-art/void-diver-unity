using NemoLib.Mvp;
using UniRx;

public class BaseScrollerModel<TSlot> : Model where TSlot : BaseSlotModel<TSlot>
{
	public readonly ReactiveProperty<bool> RxIsInputEnabled = new ReactiveProperty<bool>();

	public readonly Subject<Unit> OutOfTop = new Subject<Unit>();

	public readonly Subject<Unit> OutOfBottom = new Subject<Unit>();

	public readonly Subject<Unit> OutOfLeft = new Subject<Unit>();

	public readonly Subject<Unit> OutOfRight = new Subject<Unit>();

	public readonly Subject<TSlot> OnSlotEnter = new Subject<TSlot>();

	public readonly Subject<TSlot> OnSlotExit = new Subject<TSlot>();

	public readonly Subject<TSlot> OnSlotClick = new Subject<TSlot>();

	public readonly Subject<TSlot> OnSlotLeftClick = new Subject<TSlot>();

	public readonly Subject<TSlot> OnSlotRightClick = new Subject<TSlot>();

	public ENavDir EntryDirection;
}
