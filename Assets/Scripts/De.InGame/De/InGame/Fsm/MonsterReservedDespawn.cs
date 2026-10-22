using GameCommon.Enum;
using UnityEngine;

namespace De.InGame.Fsm
{
	public class MonsterReservedDespawn : MonsterStateBase
	{
		private const float _timeToDespawn = 3f;

		private float _despawnTime;

		private bool _isDespawned;

		public override EActionState ActionState => EActionState.None;

		public override bool IsAimEnable => false;

		public MonsterReservedDespawn(UnitStateMachine stateMachine)
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
