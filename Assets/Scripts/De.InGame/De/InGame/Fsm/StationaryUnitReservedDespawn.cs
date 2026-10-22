using GameCommon.Enum;
using UnityEngine;

namespace De.InGame.Fsm
{
	public class StationaryUnitReservedDespawn : ExtraUnitStateBase
	{
		private float _despawnTime;

		private bool _isDespawned;

		public override EActionState ActionState => EActionState.ReservedDespawn;

		public override bool IsAimEnable => false;

		public StationaryUnitReservedDespawn(UnitStateMachine stateMachine)
			: base(stateMachine)
		{
		}

		public override void OnEnter(IUnitEvent unitEvent)
		{
			base.OnEnter(unitEvent);
			_isDespawned = false;
			_despawnTime = GameManager.TimeNow + 2f;
		}

		public override void OnUpdate()
		{
			if (!_isDespawned && !(GameManager.TimeNow <= _despawnTime))
			{
				_isDespawned = true;
				StateMachine.UnitModel.NetworkObject.Despawn();
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
}
