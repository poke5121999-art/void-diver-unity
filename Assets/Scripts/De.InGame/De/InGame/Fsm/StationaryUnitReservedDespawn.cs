using GameCommon.Enum;
using UnityEngine;

namespace De.InGame.Fsm
{
	public class StationaryUnitReservedDespawn : ExtraUnitStateBase
	{
		private float _despawnTime;

		private bool _isDespawned;

		public override EActionState ActionState => EActionState.None;

		public override bool IsAimEnable => false;

		public StationaryUnitReservedDespawn(UnitStateMachine stateMachine)
			: base(null)
		{
		}

		public override void OnEnter(IUnitEvent unitEvent)
		{
		}

		public override void OnUpdate()
		{
		}

		public override EActionState GetNextStateEvent(IUnitEvent unitEvent)
		{
			return EActionState.None;
		}

		public override Vector3 GetForward()
		{
			return default;
		}
	}
}
