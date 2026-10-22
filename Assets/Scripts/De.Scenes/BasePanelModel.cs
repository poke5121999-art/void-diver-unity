using NemoLib.Mvp;
using UniRx;

public abstract class BasePanelModel : Model
{
	public readonly ReactiveProperty<bool> RxIsInputEnabled;

	public readonly Subject<Unit> OnEscape;

	public readonly Subject<Unit> OutOfTop;

	public readonly Subject<Unit> OutOfBottom;

	public readonly Subject<Unit> OutOfLeft;

	public readonly Subject<Unit> OutOfRight;

	public ENavDir EntryDirection;

	public bool IsInputEnabled => false;
}
