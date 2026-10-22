namespace De.InGame.Fsm
{
	public class CharacterStateMachine : UnitStateMachine
	{
		public CharacterStateMachine(UnitController controller)
			: base(controller)
		{
			AddState(new CharacterStageReadyState(this));
			AddState(new CharacterIdleState(this));
			AddState(new CharacterMoveState(this));
			AddState(new CharacterInteractState(this));
			AddState(new CharacterEscapeState(this));
			AddState(new UnitSkillState(this));
			AddState(new UnitDyingState(this));
			AddState(new UnitDeadState(this));
			AddState(new UnitReviveState(this));
			AddState(new UnitCrowdControlledState(this));
			AddState(new CharacterCutsceneState(this));
		}
	}
}
