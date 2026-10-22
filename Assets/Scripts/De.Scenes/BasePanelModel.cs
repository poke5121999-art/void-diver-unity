using NemoLib.Mvp;
using UniRx;

public abstract class BasePanelModel : Model
{
	public readonly ReactiveProperty<bool> RxIsInputEnabled = new ReactiveProperty<bool>();

	public readonly Subject<Unit> OnEscape = new Subject<Unit>();

	public readonly Subject<Unit> OutOfTop = new Subject<Unit>();

	public readonly Subject<Unit> OutOfBottom = new Subject<Unit>();

	public readonly Subject<Unit> OutOfLeft = new Subject<Unit>();

	public readonly Subject<Unit> OutOfRight = new Subject<Unit>();

	public ENavDir EntryDirection;

	public bool IsInputEnabled => RxIsInputEnabled.Value;
}
