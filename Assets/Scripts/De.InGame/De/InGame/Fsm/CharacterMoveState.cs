using GameCommon.Enum;
using UniRx;
using UnityEngine;

namespace De.InGame.Fsm
{
	public class CharacterMoveState : UnitStateBase
	{
		private CompositeDisposable _disposable;

		public override EActionState ActionState => EActionState.None;

		public override bool IsAimEnable => false;

		private CharacterView CharacterView => null;

		private CharacterModel CharacterModel => null;

		private CharacterController CharacterController => null;

		public CharacterMoveState(UnitStateMachine stateMachine)
			: base(null)
		{
		}

		public override void OnEnter(IUnitEvent unitEvent)
		{
		}

		public override void OnUpdate()
		{
		}

		public override void OnExit()
		{
		}

		private static bool IsBackward(Vector3 moveDir, Vector3 aimDir)
		{
			return false;
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
