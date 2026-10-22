using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GameCommon.Enum;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameCommon.Data
{
	[Serializable]
	public class SkillAction
	{
		[NonSerialized]
		private long _uniqueId;

		[HideInInspector]
		public int SkillId;

		[HideInInspector]
		public ESkillType SkillType;

		public string text;

		public SkillTrigger SkillTrigger;

		public List<BuffCondition> BuffConditionList;

		public List<TalentCondition> TalentConditionList;

		public bool IsGivingCoolDown;

		public bool IsUsingStack;

		public bool IsUsingCost;

		public bool AutoAimToTarget;

		[Tooltip("action 시작시 aim 갱신 여부")]
		public bool UpdateAimOnStart;

		[Tooltip("action 실행중 매 프레임마다 aim 갱신 여부")]
		public bool UpdateAimOnFrame;

		[Tooltip("초당 최대 aim 회전 각도 (도/초, 0 = 제한 없음)")]
		public float maxAimAnglePerSecond;

		[Tooltip("이 action으로 진입하기 위한 시간체크를 할지 여부 \n from <= 전단계시간 <= to ")]
		public bool checkTime;

		public float timeFrom;

		public float timeTo;

		public ESkillActionExecuteType ExecuteType;

		public float executeTime;

		public bool isDisabledWhenSnared;

		public float actionDuration;

		public SkillAnimationData[] SkillAnimationDatas;

		public ESkillIndicatorType indicatorType;

		[Tooltip("시전 가능 범위 반지름")]
		public float castRangeRadius;

		[Tooltip("효과 범위 반지름")]
		public float effectAreaRadius;

		[Tooltip("직선 경로 길이")]
		public float LineLength;

		[Tooltip("부채꼴 각도(degree)")]
		public float effectAreaAngle;

		public ESkillMoveType moveType;

		[FormerlySerializedAs("ignoreMoveCollition")]
		[FormerlySerializedAs("ignoreNavMesh")]
		public bool ignoreMoveCollision;

		public EMoveSpeedType moveSpeedType;

		[Range(0f, 20f)]
		public float moveStartTime;

		[Range(0f, 20f)]
		public float moveEndTime;

		public float moveSpeed;

		public AnimationCurve MoveSpeedCurve;

		[HideInInspector]
		public AnimationCurveData MoveSpeedCurveData;

		[Range(-5f, 5f)]
		public float moveOffset;

		public bool IsGroundSfxOnMove;

		public float groundSfxInterval;

		public CancelableTime[] cancelableTimes;

		[SerializeReference]
		public ActionEvent[] actionEvents;

		[JsonProperty]
		public long UniqueId
		{
			get
			{
				return _uniqueId;
			}
			set
			{
				_uniqueId = value;
			}
		}

		private void OnMoveSpeedCurveChanged()
		{
		}

		[OnDeserialized]
		private void OnDeserializedMoveSpeedCurveData(StreamingContext context)
		{
		}

		public bool TryResolveExpression(string expression, out string result)
		{
			result = null;
			return false;
		}

		public string[] ExtractExpressions()
		{
			return null;
		}

		public void RefreshUniqueId()
		{
		}

		public bool Validate(out string errorMessage)
		{
			errorMessage = null;
			return false;
		}
	}
}
