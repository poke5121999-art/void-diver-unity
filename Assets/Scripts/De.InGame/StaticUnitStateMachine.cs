using De.InGame.Fsm;
public class StaticUnitStateMachine : ExtraUnitStateMachine
{
	public StaticUnitStateMachine(UnitController controller)
		: base(controller)
	{
		AddState(new StaticUnitIdleState(this));
		AddState(new UnitDyingState(this));
		AddState(new UnitDeadState(this));
	}
}
