using System;
using System.Collections.Generic;
using System.Linq;
using GameCommon.Data;
using GameCommon.Enum;
using GameCommon.Table;
using SomeNet.Utility;
using UniRx;
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

		public override EActionState ActionState => EActionState.Dying;

		public override bool IsAimEnable => false;

		public UnitDyingState(UnitStateMachine stateMachine)
			: base(stateMachine)
		{
		}

		public override void OnEnter(IUnitEvent unitEvent)
		{
			base.OnEnter(unitEvent);
			_isSelfRevive = false;
			_isEquipmentRevive = false;
			_equipmentReviveHpPercent = 0f;
			DieEvent dieEvent = unitEvent as DieEvent;
			float duration = StateMachine.UnitView.GetAnimationDuration(StateMachine.UnitView.GetDeathAnimation());
			if (StateMachine.UnitModel is CharacterModel characterModel)
			{
				characterModel.IsLightOn = false;
				EvaluateSelfRevive(dieEvent, characterModel, ref duration);
				if (!_isSelfRevive && !_isEquipmentRevive)
				{
					Inventory inventory = characterModel.GetComponent<Inventory>();
					inventory.Slots.Values.Where((GoodsSlot slot) => slot.IsEventItem).ToList().ForEach(delegate(GoodsSlot slot)
					{
						if (!slot.IsEmpty && slot.Count > 0)
						{
							inventory.DropGoods(slot.Goods);
						}
					});
				}
			}
			Observable.Timer(TimeSpan.FromSeconds(duration)).Subscribe(delegate
			{
				_isCompleted = true;
			}).AddTo(StateMachine.UnitModel);
		}

		private void EvaluateSelfRevive(DieEvent dieEvent, CharacterModel characterModel, ref float duration)
		{
			if (dieEvent != null && dieEvent.IsForced)
			{
				duration = 5f;
				_isSelfRevive = false;
				SendDeathThresholdFx(characterModel.ClientId, isRestore: false, duration);
				return;
			}
			if (GameContext.CampaignId == 1101 && characterModel.SelfReviveCount == 0)
			{
				_isSelfRevive = true;
				duration = 3.7f;
				SendDeathThresholdFx(characterModel.ClientId, isRestore: true, duration);
				SendSelfReviveTextWithDelay(characterModel.ClientId, 2.5f);
				characterModel.SelfReviveCount++;
				return;
			}
			if (TryConsumeForcedSelfReviveEffect(onlyConditional: false))
			{
				_isSelfRevive = true;
				duration = 3.7f;
				SendDeathThresholdFx(characterModel.ClientId, isRestore: true, duration);
				SendSelfReviveTextWithDelay(characterModel.ClientId, 2.5f);
				return;
			}
			_isSelfRevive = RandomUtility.GetRandomBool(GetSelfReviveRateWithAmplifier(characterModel));
			characterModel.SelfReviveCount++;
			if (!_isSelfRevive && TryConsumeForcedSelfReviveEffect(onlyConditional: true))
			{
				_isSelfRevive = true;
				characterModel.SelfReviveCount--;
			}
			int equipmentId = 0;
			if (!_isSelfRevive && TryConsumeEquipmentReviveEffect(out equipmentId, out var hpPercent))
			{
				_equipmentReviveHpPercent = hpPercent;
				_isEquipmentRevive = true;
				characterModel.SelfReviveCount--;
			}
			bool isRestore;
			if (_isSelfRevive)
			{
				isRestore = true;
				duration = 3.7f;
			}
			else
			{
				isRestore = _isEquipmentRevive;
				duration = (_isEquipmentRevive ? 3.7f : 5f);
			}
			SendDeathThresholdFx(characterModel.ClientId, isRestore, duration);
			if (_isSelfRevive || _isEquipmentRevive)
			{
				SendSelfReviveTextWithDelay(characterModel.ClientId, 2.5f, equipmentId);
			}
		}

		public override void OnUpdate()
		{
		}

		public override void OnExit()
		{
			if (IsActivated)
			{
				base.OnExit();
				_isCompleted = false;
				_isSelfRevive = false;
				_isEquipmentRevive = false;
				_equipmentReviveHpPercent = 0f;
			}
		}

		public override EActionState GetNextStateEvent(IUnitEvent unitEvent)
		{
			if (unitEvent is ReviveEvent reviveEvent && (reviveEvent.IsSelfRevive || reviveEvent.IsEquipmentRevive))
			{
				return EActionState.Revive;
			}
			if (!_isCompleted)
			{
				return EActionState.None;
			}
			if (!_isSelfRevive)
			{
				if (!_isEquipmentRevive)
				{
					return EActionState.Dead;
				}
				ReviveEvent reviveEvent2 = new ReviveEvent();
				reviveEvent2.IsEquipmentRevive = true;
				reviveEvent2.EquipmentReviveHpPercent = _equipmentReviveHpPercent;
				StateMachine.UnitController.HandleUnitEvent(reviveEvent2);
			}
			else
			{
				ReviveEvent reviveEvent3 = new ReviveEvent();
				reviveEvent3.IsSelfRevive = true;
				StateMachine.UnitController.HandleUnitEvent(reviveEvent3);
			}
			return EActionState.None;
		}

		public override Vector3 GetForward()
		{
			return Vector3.zero;
		}

		private bool TryConsumeForcedSelfReviveEffect(bool onlyConditional)
		{
			IList<BuffEffectInfo<ForcedSelfReviveEffect>> buffEffects = StateMachine.UnitModel.GetBuffEffects<ForcedSelfReviveEffect>();
			if (buffEffects == null || buffEffects.Count == 0)
			{
				return false;
			}
			List<BuffEffectInfo<ForcedSelfReviveEffect>> list = buffEffects.Where((BuffEffectInfo<ForcedSelfReviveEffect> b) => b.BuffEffect.TriggerOnReviveFail == onlyConditional).ToList();
			if (list.Count == 0)
			{
				return false;
			}
			BuffEffectInfo<ForcedSelfReviveEffect> buffEffectInfo = list.First();
			StateMachine.UnitModel.RemoveBuff(buffEffectInfo.BuffId);
			return true;
		}

		private bool TryConsumeEquipmentReviveEffect(out int equipmentId, out float hpPercent)
		{
			equipmentId = 0;
			hpPercent = 0f;
			IList<BuffEffectInfo<EquipmentReviveEffect>> buffEffects = StateMachine.UnitModel.GetBuffEffects<EquipmentReviveEffect>();
			if (buffEffects == null || buffEffects.Count == 0)
			{
				return false;
			}
			BuffEffectInfo<EquipmentReviveEffect> buffEffectInfo = buffEffects.First();
			equipmentId = buffEffectInfo.BuffEffect.EquipmentId;
			hpPercent = buffEffectInfo.BuffEffect.HpPercent;
			StateMachine.UnitModel.RemoveBuff(buffEffectInfo.BuffId);
			(StateMachine.UnitController as CharacterController)?.UnequipByEquipmentId(equipmentId);
			return true;
		}

		private float GetSelfReviveRateWithAmplifier(CharacterModel characterModel)
		{
			int num = characterModel.SelfReviveCount + 1;
			float num2 = StateMachine.UnitModel.Stress / (float)TConst.StressMax.IntValue;
			float num3 = Mathf.Pow(TConst.SelfReviveStressFactor.FloatValue * num2, TConst.SelfReviveStressExponent.FloatValue);
			float num4 = Mathf.Pow(TConst.SelfReviveProbabilityReductionRate.FloatValue, num);
			IList<BuffEffectInfo<SelfReviveChanceAmplifierBuffEffect>> buffEffects = StateMachine.UnitModel.GetBuffEffects<SelfReviveChanceAmplifierBuffEffect>();
			float num5 = 0f;
			if (buffEffects != null && buffEffects.Count > 0)
			{
				num5 = buffEffects.Sum((BuffEffectInfo<SelfReviveChanceAmplifierBuffEffect> b) => (float)b.Stack * b.BuffEffect.Percent);
			}
			float num6 = ((GameContext.Players.Count > 1) ? TConst.SelfReviveMultiplayFactor.FloatValue : 1f);
			float num7 = (num5 / 100f + 1f) * (1f - num3) * num4 * num6;
			if (num7 < 0f)
			{
				num7 = 0f;
			}
			else if (num7 > 1f)
			{
				num7 = 1f;
			}
			return num7;
		}

		private void SendDeathThresholdFx(ulong clientId, bool isRestore, float duration)
		{
			string sfx = (isRestore ? "ThresholdOfDeathRestore" : "ThresholdOfDeath");
			string text = (isRestore ? "Common/ThresholdOfDeathRestore" : "Common/ThresholdOfDeath");
			GameHost.SendToClient(clientId, new NtfDisplayFx
			{
				Vfx = text + "_Local",
				VfxDuration = duration,
				Sfx = sfx,
				Position = StateMachine.UnitView.transform.position.ToProtoVector3()
			});
			GameHost.SendToAll(new NtfDisplayFx
			{
				Vfx = text,
				VfxDuration = duration,
				Sfx = string.Empty,
				Position = StateMachine.UnitView.transform.position.ToProtoVector3()
			}, new HashSet<ulong> { clientId });
		}

		private void SendSelfReviveTextWithDelay(ulong clientId, float delaySeconds, int equipmentId = 0)
		{
			Observable.Timer(TimeSpan.FromSeconds(delaySeconds)).Subscribe(delegate
			{
				GameHost.SendToAll(new NtfSelfReviveText
				{
					ClientId = clientId,
					EquipmentId = equipmentId
				});
			}).AddTo(StateMachine.UnitModel);
		}
	}
}
