using De.InGame.Fsm;
public class StationaryUnitStateMachine : ExtraUnitStateMachine
{
	public StationaryUnitStateMachine(UnitController controller)
		: base(controller)
	{
		AddState(new StationaryUnitIdleState(this));
		AddState(new UnitSkillState(this));
		AddState(new UnitDyingState(this));
		AddState(new StationaryUnitDeadState(this));
		AddState(new StationaryUnitReservedDespawn(this));
	}
}
