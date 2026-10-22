using GameCommon.Enum;
using UnityEngine;

namespace De.InGame.Fsm
{
	public class CharacterInteractState : UnitStateBase
	{
		private EInteractionState _interactionState;

		private float _holdingRemainTime;

		private bool _reservedLightOn;

		private IInteractiveObject _target;

		public override EActionState ActionState => EActionState.None;

		public override bool IsAimEnable => false;

		private CharacterView CharacterView => null;

		private CharacterModel CharacterModel => null;

		public CharacterInteractState(UnitStateMachine stateMachine)
			: base(null)
		{
		}

		public override void OnEnter(IUnitEvent unitEvent)
		{
		}

		public override void OnExit()
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
