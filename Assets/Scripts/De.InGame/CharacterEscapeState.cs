using De.InGame.Fsm;
using GameCommon.Enum;
using UnityEngine;

public class CharacterEscapeState : UnitStateBase
{
	public override EActionState ActionState => EActionState.Escaped;

	public override bool IsAimEnable => false;

	public CharacterEscapeState(UnitStateMachine stateMachine)
		: base(stateMachine)
	{
	}

	public override void OnUpdate()
	{
	}

	public override void OnEnter(IUnitEvent unitEvent)
	{
		base.OnEnter(unitEvent);
		if (base.UnitModel is CharacterModel characterModel)
		{
			characterModel.IsLightOn = false;
		}
	}

	public override EActionState GetNextStateEvent(IUnitEvent unitEvent)
	{
		return EActionState.None;
	}

	public override Vector3 GetForward()
	{
		return Vector3.zero;
	}
}
