using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.AssetManagement;
using NemoLib.Log;
using SomeNet.Utility;
using UniRx;
using UnityEngine;
using VContainer;

namespace De.InGame.Unit
{
	public class MonsterView : UnitView
	{
		[Inject]
		private GameObjectManager _gameObjectManager;

		[Inject]
		private SeriLogger _logger;

		[SerializeField]
		private ShadowMonsterVisualEffect _shadowMonsterVisualEffect;

		private OutlineObject _outlineObject;

		private StatusVfx _groggyVfx;

		protected MonsterModel MonsterModel => UnitModel as MonsterModel;

		private int MonsterId => MonsterModel.RxMonsterId.Value;

		private TMonster MonsterData => TMonster.Table[MonsterModel.RxMonsterId.Value];

		private OutlineObject OutlineObject
		{
			get
			{
				if (_outlineObject == null)
				{
					_outlineObject = GetComponentInChildren<OutlineObject>();
				}
				return _outlineObject;
			}
		}

		public Vector3 SpawnPosition { get; private set; }

		protected override string ResolveFootstepSound(bool isLeft)
		{
			switch (MonsterData.FootstepSfxMode)
			{
			case EMonsterFootstepSfxMode.Always:
				break;
			case EMonsterFootstepSfxMode.InCombat:
				if (!MonsterModel.RxIsInCombat.Value)
				{
					return string.Empty;
				}
				break;
			case EMonsterFootstepSfxMode.NoneCombat:
				if (MonsterModel.RxIsInCombat.Value)
				{
					return string.Empty;
				}
				break;
			default:
				return string.Empty;
			}
			return GetFootstepSfx(EGroundType.None, isLeft) ?? string.Empty;
		}

		protected override string GetFootstepSfx(EGroundType groundType, bool isLeft)
		{
			string text = ((UnitModel.MoveSpeed < TConst.MoveSpeedThresholdForRun.FloatValue) ? (isLeft ? MonsterData.WalkLeftSfx : MonsterData.WalkRightSfx) : (isLeft ? MonsterData.RunLeftSfx : MonsterData.RunRightSfx));
			return text.HasValue() ? text : null;
		}

		private bool ShouldPlayFootstep()
		{
			switch (MonsterData.FootstepSfxMode)
			{
			case EMonsterFootstepSfxMode.Always:
				return true;
			case EMonsterFootstepSfxMode.InCombat:
				return MonsterModel.RxIsInCombat.Value;
			case EMonsterFootstepSfxMode.NoneCombat:
				return !MonsterModel.RxIsInCombat.Value;
			default:
				return false;
			}
		}

		public override void Init()
		{
			base.Init();
			SpawnPosition = base.gameObject.transform.position;
			if (OutlineObject != null && MonsterData != null)
			{
				OutlineObject.SetColor(MonsterData.OutlineType.ToColor());
				OutlineObject.SetStroke(MonsterData.OutlineType.ToStroke());
				OutlineObject.SetEnable(MonsterData.OutlineType.ToStartActive());
			}
		}

		protected override void Start()
		{
			base.Start();
			GameChannel.Receive<NtfHpDamage>().Where((NtfHpDamage message) => message.NetworkObjectId == UnitModel.NetworkObjectId).Subscribe(OnHpDamage)
				.AddTo(this);
		}

		private void OnHpDamage(NtfHpDamage message)
		{
			if (message.ReasonType == EHpDamageReasonType.Hit && MonsterData.HitSfx.HasValue() && RandomUtility.GetRandomBoolFromPercentage(MonsterData.HitSfxPercent))
			{
				SoundManager.PlaySfx3D(MonsterData.HitSfx, base.transform);
			}
		}

		protected override void OnStateChanged(EActionState state)
		{
			base.OnStateChanged(state);
			PlayAnimationByActionState(state);
			switch (state)
			{
			case EActionState.Dying:
				SoundManager.PlaySfx3D(TMonster.Table[MonsterModel.RxMonsterId.Value].DyingSfx, base.transform.position);
				break;
			case EActionState.Groggy:
				SpawnGroggyVfx();
				return;
			}
			DespawnGroggyVfx();
		}

		private void SpawnGroggyVfx()
		{
			if (_groggyVfx != null)
			{
				return;
			}
			SoundManager.PlaySfx3D("StatusEffect_Suppression", base.transform);
			StatusVfx statusVfx = AssetManager.SpawnStatusVfx("Groggy_Start");
			if (statusVfx != null)
			{
				statusVfx.gameObject.GetOrAddComponent<UnitFollower>().Init(UnitModel, EUnitBoneType.Head, Vector3.zero, inheritMoveDir: false, destroyOnUnitDeath: true);
				statusVfx.Despawn(1.5f);
			}
			TStatusEffectTag tStatusEffectTag = TStatusEffectTag.Table[EStatusEffectTag.Stun];
			_groggyVfx = AssetManager.SpawnStatusVfx(tStatusEffectTag.Vfx);
			if (!(_groggyVfx == null))
			{
				float num = ((tStatusEffectTag.VfxBoneType == EUnitBoneType.Symbol) ? 1f : StatusVfxScale);
				_groggyVfx.transform.localScale = Vector3.one * num;
				_groggyVfx.gameObject.GetOrAddComponent<UnitFollower>().Init(UnitModel, tStatusEffectTag.VfxBoneType, tStatusEffectTag.VfxOffset, inheritMoveDir: false, destroyOnUnitDeath: true);
			}
		}

		private void DespawnGroggyVfx()
		{
			if (!(_groggyVfx == null))
			{
				_groggyVfx.Despawn();
				_groggyVfx = null;
			}
		}

		protected override void PlayAnimationByActionState(EActionState state)
		{
			string text;
			switch (state)
			{
			case EActionState.Idle:
			case EActionState.CrowdControlled:
			case EActionState.Spawn:
				text = GetIdleAnimation();
				break;
			case EActionState.Move:
				text = GetMoveAnimation();
				break;
			case EActionState.Skill:
			case EActionState.Dead:
			case EActionState.Revive:
			case EActionState.Escaped:
			case EActionState.Interact:
			case EActionState.ReservedDespawn:
				text = string.Empty;
				break;
			case EActionState.Dying:
				text = GetDeathAnimation();
				break;
			default:
				switch (state)
				{
				case EActionState.MoveToTarget:
				case EActionState.Wander:
				case EActionState.Flee:
				case EActionState.Avoid:
				case EActionState.KiteFromTarget:
					text = GetMoveAnimation();
					break;
				case EActionState.Pause:
				case EActionState.Alert:
					text = GetIdleAnimation();
					break;
				case EActionState.Aggro:
					text = GetAggroAnimation();
					break;
				case EActionState.LostTarget:
					text = GetLostTargetAnimation();
					break;
				case EActionState.ReceiveAggroSpread:
					text = GetReceiveAggroSpreadAnimation();
					break;
				case EActionState.PhaseUp:
					text = GetReviveAnimation();
					break;
				default:
					text = string.Empty;
					break;
				}
				break;
			}
			bool isLoop;
			switch (state)
			{
			case EActionState.Idle:
			case EActionState.Move:
			case EActionState.CrowdControlled:
			case EActionState.MoveToTarget:
			case EActionState.Wander:
			case EActionState.Flee:
			case EActionState.Avoid:
			case EActionState.KiteFromTarget:
			case EActionState.Alert:
				isLoop = true;
				break;
			default:
				isLoop = false;
				break;
			}
			float defaultSpeed;
			switch (state)
			{
			case EActionState.Move:
			case EActionState.MoveToTarget:
			case EActionState.Wander:
			case EActionState.Flee:
			case EActionState.Avoid:
			case EActionState.KiteFromTarget:
				defaultSpeed = GetMoveAnimationSpeed();
				break;
			case EActionState.CrowdControlled:
				defaultSpeed = (UnitModel.IsCrowdControlled(ECrowdControlType.Freeze) ? 0f : 1f);
				break;
			default:
				defaultSpeed = 1f;
				break;
			}
			if (!string.IsNullOrEmpty(text))
			{
				PlayAnimationAsync(text, isLoop, 0f, null, defaultSpeed).Forget();
			}
		}

		public virtual string GetAggroAnimation()
		{
			return string.Empty;
		}

		public virtual string GetReceiveAggroSpreadAnimation()
		{
			return string.Empty;
		}

		public virtual string GetLostTargetAnimation()
		{
			return string.Empty;
		}

		protected override void OnUsingSkillIdChanged(int skillId)
		{
			if (skillId != 0)
			{
				TSkill tSkill = TSkill.Table[skillId];
				if (tSkill != null && !string.IsNullOrEmpty(tSkill.ActionSfx))
				{
					SoundManager.PlaySfx3D(tSkill.ActionSfx, base.transform);
				}
			}
		}

		private string WithPhase(string animationName)
		{
			if (MonsterModel != null && MonsterModel.IsPhaseMonster)
			{
				return string.Format("{0}/{1}", MonsterModel.Phase, animationName);
			}
			return animationName;
		}

		public override string GetIdleAnimation()
		{
			return WithPhase("idle");
		}

		public override string GetMoveAnimation()
		{
			if (UnitModel.MoveSpeed <= 0f)
			{
				return GetIdleAnimation();
			}
			var animations = CurrentSkeletonAnimation.Skeleton.Data.Animations;
			string animationName;
			if (UnitModel.MoveSpeed < TConst.MoveSpeedThresholdForRun.FloatValue)
			{
				animationName = "walk";
			}
			else
			{
				animationName = (animations.Any((Spine.Animation a) => a.Name == "run") ? "run" : "walk");
			}
			return WithPhase(animationName);
		}

		public override string GetDeathAnimation()
		{
			return WithPhase("death");
		}

		public override string GetReviveAnimation()
		{
			return WithPhase("revive");
		}

		public override float GetMoveAnimationSpeed()
		{
			if (UnitModel.MoveSpeed <= 0f)
			{
				return 1f;
			}
			if (UnitModel.MoveSpeed >= TConst.MoveSpeedThresholdForRun.FloatValue)
			{
				return UnitModel.MoveSpeed / TConst.MoveSpeedThresholdForRun.FloatValue;
			}
			float value = UnitModel.MoveSpeed / MonsterData.WalkAnimation1xSpeed;
			return Mathf.Clamp(value, TConst.MinWalkAnimationSpeed.FloatValue, TConst.MaxWalkAnimationSpeed.FloatValue);
		}

		public void SetShadowMonsterEffect(bool isLocalShadowMonster)
		{
			if (_shadowMonsterVisualEffect != null)
			{
				_shadowMonsterVisualEffect.SetIsLocalGhost(isLocalShadowMonster);
			}
		}
	}
}
