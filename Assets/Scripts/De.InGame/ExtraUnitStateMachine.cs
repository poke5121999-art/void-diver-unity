using De.InGame.Fsm;

public abstract class ExtraUnitStateMachine : UnitStateMachine
{
	public ExtraUnitController ExtraUnitController => null;

	public ExtraUnitView ExtraUnitView => null;

	public ExtraUnitModel ExtraUnitModel => null;

	protected ExtraUnitStateMachine(UnitController controller)
		: base(null)
	{
	}
}
