using De.InGame.Fsm;

public abstract class ExtraUnitStateMachine : UnitStateMachine
{
	public ExtraUnitController ExtraUnitController => base.UnitController as ExtraUnitController;

	public ExtraUnitView ExtraUnitView => base.UnitView as ExtraUnitView;

	public ExtraUnitModel ExtraUnitModel => base.UnitModel as ExtraUnitModel;

	protected ExtraUnitStateMachine(UnitController controller)
		: base(controller)
	{
	}
}
