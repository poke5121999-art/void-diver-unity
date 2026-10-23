using GameCommon.Table;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using GameCommon.Enum;
using Newtonsoft.Json;
using UnityEngine;

namespace GameCommon.Data
{
	[Serializable]
	public class HitBoxInfo
	{
		public float duration;

		public float multiHitInterval = -1f;

		public float angleOffset;

		public Vector3 startOffset;

		public float FixedYPos = -1f;

		[Tooltip("스폰 시 위치 XZ를 NavMesh 지면에 투영해 Y를 지면에 맞춘다. 공중/벽 충돌시 바닥에 스폰되는 용도")]
		public bool SnapToGroundOnSpawn;

		public bool InheritOwnerScaleX;

		public bool IsCancleOnSkillChange;

		public bool IsCancelOnOwnerDie;

		[Tooltip("스폰 시 Owner → 스폰 위치 사이에 벽이 있으면 벽 충돌 지점으로 위치를 보정한다")]
		public bool ClampSpawnPositionToWall;

		[Tooltip("스폰 시 원래 AimDir ±10도 내 시야 안 가장 가까운 적 방향으로 AimDir을 보정한다")]
		public bool UseAimCorrection;

		[Tooltip("방향 없는 제자리/자기중심 히트박스(자폭·하울링 등)에 켠다. AimDir 회전 무시하고 identity로 스폰")]
		public bool SpawnWithIdentityRotation;

		public string vfx;

		public float VfxZOffset;

		public AnimationSpeed[] vfxSpeeds;

		public bool IsLoopVfx;

		public float VfxLoopDuration;

		public EHitPointType hitPointType;

		public string hitVfx;

		public bool UseBoneAttachHitVfx;

		public bool UseHitVfxRotate;

		public EUnitBoneType HitVfxBoneType;

		public Vector3 HitVfxBoneOffset;

		public bool UseElementalHitVfx;

		public float hitVfxDuration;

		public AnimationSpeed[] hitVfxSpeeds;

		public bool IsLoopHitVfx;

		public float HitVfxLoopDuration;

		public bool IgnoreHitVfxWall;

		public string FireVfx;

		public float FireVfxDuration;

		public float SpawnDelayAfterFireVfx;

		public string SpawnSfx;

		public GroundSfxVariant[] GroundSpawnSfxVariants = Array.Empty<GroundSfxVariant>();

		public string hitSfx;

		public bool SfxFollowOwner;

		public bool UseElementalHitSfx;

		public string criticalHitSfx;

		public bool UseElementalCritSfx;

		public string DestroyByHitSfx;

		public string DestroyByTimeoutSfx;

		public EHitBoxMoveType MoveType;

		public float moveSpeed;

		[JsonIgnore]
		public AnimationCurve MoveSpeedCurve;

		public AnimationCurveData MoveSpeedCurveData;

		public float MinMoveDistance;

		public float MaxMoveDistance;

		public float ParabolaMoveHeight;

		[Tooltip("tween 이동 중 매 프레임 진행 방향으로 회전시키는 동작을 막는다")]
		public bool LockRotationOnMove;

		public EHitTarget tracingTargetType;

		public float tracingDetectingRange;

		public float tracingAngle;

		public ECollisionType collisionType;

		public float CollisionTriggerDelay = -1f;

		public bool ignoreObstaclesForCollision;

		public Vector3 collisionScale;

		public CollisionScaleKey[] CollisionScaleKeys = Array.Empty<CollisionScaleKey>();

		public float collisionDelay;

		public float collisionEndTime;

		public ECollisionDestroyType collisionDestroy;

		public int pierceCount = 1;

		[Tooltip("관통 횟수별 확률(%). 크기는 pierceCount-1. [0]=1회 관통 확률, [1]=2회 관통 확률...  비어있으면 100%")]
		public int[] pierceChances = Array.Empty<int>();

		[Tooltip("관통 성공 시 재생되는 SFX")]
		public string pierceSfx;

		[SerializeReference]
		public CollisionEvent[] collisionEvents = Array.Empty<CollisionEvent>();

		public float OwnerHitShakeAmplitude;

		public float OwnerHitShakeFrequency;

		public float OwnerHitShakeDuration;

		public bool CanParry = true;

		[Tooltip("백어택 판정을 방향이 아닌 위치 기반으로 판단한다 (트랩 폭발 등 정지형 HitBox에 사용)")]
		public bool UsePositionBasedBackAttack;

		public ESkillTagType[] SkillTags;

		public int[] destroyHitBoxId = Array.Empty<int>();

		public int ExtraUnitIdOnDestroy;

		[SerializeReference]
		public ActionEvent[] ActionEventsOnDestroy;

		private static readonly EElementalType[] ElementalSfxSuffixTypes = new EElementalType[3]
		{
			EElementalType.Fire,
			EElementalType.Water,
			EElementalType.Wind
		};

		[JsonIgnore]
		public bool IsTracing => tracingTargetType != EHitTarget.None;

		public string ResolveSpawnSfx(EGroundType groundType)
		{
			if (groundType == EGroundType.None)
			{
				return SpawnSfx;
			}
			for (int i = 0; i < GroundSpawnSfxVariants.Length; i++)
			{
				if (GroundSpawnSfxVariants[i].GroundType == groundType)
				{
					return GroundSpawnSfxVariants[i].Sfx;
				}
			}
			return SpawnSfx;
		}

		[OnSerialized]
		private void OnSerialized(StreamingContext context)
		{
			MoveSpeedCurveData = new AnimationCurveData(MoveSpeedCurve);
		}

		[OnDeserialized]
		private void OnDeserialized(StreamingContext context)
		{
			MoveSpeedCurve = MoveSpeedCurveData?.ToAnimationCurve();
		}

		private IEnumerable<Type> GetAllowedActionTypes()
		{
			return new Type[3]
			{
				typeof(HitBoxEvent),
				typeof(SummonActionEvent),
				typeof(SpecialFieldActionEvent)
			};
		}

		public bool Validate(out string errorMessage)
		{
			bool isValid = true;
			StringBuilder stringBuilder = new StringBuilder();
			if (collisionEvents != null)
			{
				Array.ForEach(collisionEvents, delegate(CollisionEvent _)
				{
					if (!_.Validate(out var errorMessage2))
					{
						isValid = false;
						stringBuilder.AppendLine(errorMessage2);
					}
				});
			}
			if (destroyHitBoxId != null)
			{
				Array.ForEach(destroyHitBoxId, delegate(int _)
				{
					if (!THitBox.Table.ContainsKey(_))
					{
						isValid = false;
						stringBuilder.AppendLine(string.Format("{0} HitBoxId {1} is not exist in {2}", "destroyHitBoxId", _, "THitBox"));
					}
				});
			}
			if (ExtraUnitIdOnDestroy > 0 && !TExtraUnit.Table.ContainsKey(ExtraUnitIdOnDestroy))
			{
				isValid = false;
				stringBuilder.AppendLine(string.Format("{0} [{1}] is not exist in  {2}", "ExtraUnitIdOnDestroy", ExtraUnitIdOnDestroy, "TExtraUnit"));
			}
			if (collisionScale.x < 0f || collisionScale.y < 0f || collisionScale.z < 0f)
			{
				isValid = false;
				stringBuilder.AppendLine("collisionScale value cannot be negative in HitBoxInfo");
			}
			if (!ValidateSfx(SpawnSfx, false, "SpawnSfx", stringBuilder))
			{
				isValid = false;
			}
			if (!ValidateSfx(hitSfx, UseElementalHitSfx, "hitSfx", stringBuilder))
			{
				isValid = false;
			}
			if (!ValidateSfx(criticalHitSfx, UseElementalCritSfx, "criticalHitSfx", stringBuilder))
			{
				isValid = false;
			}
			if (!ValidateSfx(DestroyByHitSfx, false, "DestroyByHitSfx", stringBuilder))
			{
				isValid = false;
			}
			if (!ValidateSfx(DestroyByTimeoutSfx, false, "DestroyByTimeoutSfx", stringBuilder))
			{
				isValid = false;
			}
			if (!ValidateSfx(pierceSfx, false, "pierceSfx", stringBuilder))
			{
				isValid = false;
			}
			if (GroundSpawnSfxVariants == null)
			{
				isValid = false;
				stringBuilder.AppendLine("GroundSpawnSfxVariants cannot be null in HitBoxInfo");
			}
			else
			{
				for (int i = 0; i < GroundSpawnSfxVariants.Length; i++)
				{
					if (!ValidateSfx(GroundSpawnSfxVariants[i].Sfx, false, "GroundSpawnSfxVariants", stringBuilder))
					{
						isValid = false;
					}
				}
			}
			errorMessage = stringBuilder.ToString();
			return isValid;
		}

		private static bool ValidateSfx(string sfxName, bool useElementalVariant, string fieldName, StringBuilder stringBuilder)
		{
			if (string.IsNullOrEmpty(sfxName))
			{
				return true;
			}
			if (useElementalVariant)
			{
				if (ElementalSfxSuffixTypes.Any((EElementalType t) => ValidationExtensions.ContainsSfx(string.Format("{0}_{1}", sfxName, t))))
				{
					return true;
				}
				stringBuilder.AppendLine(string.Concat("MasterAudio not contains \"[", fieldName, "] ", sfxName, "_(Fire/Water/Wind)\""));
				return false;
			}
			if (ValidationExtensions.ContainsSfx(sfxName))
			{
				return true;
			}
			stringBuilder.AppendLine(string.Concat("MasterAudio not contains \"[", fieldName, "] ", sfxName, "\""));
			return false;
		}
	}
}
