using System;
using De.InGame.Fsm;
using GameCommon.Enum;
using GameCommon.Table;
using UnityEngine;

public class MonsterPhaseUpState : MonsterStateBase
{
	private float _phaseUpTime;

	public override EActionState ActionState => EActionState.PhaseUp;

	public override bool IsAimEnable => false;

	public MonsterPhaseUpState(UnitStateMachine stateMachine)
		: base(stateMachine)
	{
	}

	public override void OnEnter(IUnitEvent unitEvent)
	{
		base.OnEnter(unitEvent);
		string reviveAnimation = UnitView.GetReviveAnimation();
		float animationDuration = UnitView.GetAnimationDuration(reviveAnimation);
		_phaseUpTime = TimeNow + animationDuration;
		TMonster tMonster = TMonster.Table[MonsterModel.MonsterId];
		string spawnVfx = tMonster.SpawnVfx;
		Vector3 position = MonsterModel.Position;
		GameHost.SendToAll(new NtfDisplayFx
		{
			Vfx = (spawnVfx.HasValue() ? ("Spawn/" + tMonster.SpawnVfx) : null),
			VfxDuration = animationDuration,
			Sfx = tMonster.SpawnSfx,
			Position = new Vector3(position.x + 0f, position.y + tMonster.SpawnVfxOffsetY, position.z + 0f).ToProtoVector3()
		});
	}

	public override void OnUpdate()
	{
	}

	public override EActionState GetNextStateEvent(IUnitEvent unitEvent)
	{
		if (TimeNow > _phaseUpTime)
		{
			return EActionState.Idle;
		}
		return EActionState.None;
	}

	public override Vector3 GetForward()
	{
		return UnitModel.RxForward.Value;
	}
}
