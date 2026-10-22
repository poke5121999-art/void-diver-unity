using GameCommon.Enum;
using Unity.Netcode;
using VContainer;

namespace De.InGame.Fsm
{
	public class MonsterStateMachine : UnitStateMachine
	{
		[Inject]
		protected NetworkManager NetworkManager;

		public MonsterController MonsterController { get; }

		public MonsterStateMachine(UnitController controller, bool isPatrolMonster)
			: base(null)
		{
		}

		public override void TriggerEvent(IUnitEvent stateEvent)
		{
		}

		protected override void ChangeState(EActionState actionState, IUnitEvent unitEvent)
		{
		}
	}
}
