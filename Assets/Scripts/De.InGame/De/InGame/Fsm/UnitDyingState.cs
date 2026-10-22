using GameCommon.Enum;
using UnityEngine;
using VContainer;

namespace De.InGame.Fsm
{
	public class UnitDyingState : UnitStateBase
	{
		[Inject]
		private GameChatManager _chatManager;

		private bool _isCompleted;

		private bool _isSelfRevive;

		private bool _isEquipmentRevive;

		private float _equipmentReviveHpPercent;

		public override EActionState ActionState => EActionState.None;

		public override bool IsAimEnable => false;

		public UnitDyingState(UnitStateMachine stateMachine)
			: base(null)
		{
		}

		public override void OnEnter(IUnitEvent unitEvent)
		{
		}

		private void EvaluateSelfRevive(DieEvent dieEvent, CharacterModel characterModel, ref float duration)
		{
		}

		public override void OnUpdate()
		{
		}

		public override void OnExit()
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

		private bool TryConsumeForcedSelfReviveEffect(bool onlyConditional)
		{
			return false;
		}

		private bool TryConsumeEquipmentReviveEffect(out int equipmentId, out float hpPercent)
		{
			equipmentId = default;
			hpPercent = default;
			return false;
		}

		private float GetSelfReviveRateWithAmplifier(CharacterModel characterModel)
		{
			return 0f;
		}

		private void SendDeathThresholdFx(ulong clientId, bool isRestore, float duration)
		{
		}

		private void SendSelfReviveTextWithDelay(ulong clientId, float delaySeconds, int equipmentId = 0)
		{
		}
	}
}
