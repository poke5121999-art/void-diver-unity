using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GameCommon.Enum;
using System.Text;
using Newtonsoft.Json;
using SomeNet.Utility;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameCommon.Data
{
	[Serializable]
	public class SkillAction
	{
		[NonSerialized]
		private long _uniqueId = UniqueIdFactory.CreateId();

		[HideInInspector]
		public int SkillId;

		[HideInInspector]
		public ESkillType SkillType;

		public string text = "설명 없음";

		public SkillTrigger SkillTrigger;

		public List<BuffCondition> BuffConditionList = new List<BuffCondition>();

		public List<TalentCondition> TalentConditionList = new List<TalentCondition>();

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

		public ESkillActionExecuteType ExecuteType = ESkillActionExecuteType.Immediate;

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
			MoveSpeedCurveData = ((MoveSpeedCurve != null) ? new AnimationCurveData(MoveSpeedCurve) : null);
		}

		[OnDeserialized]
		private void OnDeserializedMoveSpeedCurveData(StreamingContext context)
		{
			MoveSpeedCurve = ((MoveSpeedCurveData != null) ? MoveSpeedCurveData.ToAnimationCurve() : null);
		}

		public bool TryResolveExpression(string expression, out string result)
		{
			result = null;
			string text = expression.ToLower();
			if (text.StartsWith("actionevents["))
			{
				const string prefix = "actionevents[";
				int close = expression.IndexOf(']', prefix.Length);
				if (close > prefix.Length)
				{
					string indexText = expression.Substring(prefix.Length, close - prefix.Length);
					if (int.TryParse(indexText, out int index) && actionEvents != null && index < actionEvents.Length && close + 2 < expression.Length && expression[close + 1] == '.')
					{
						string rest = expression.Substring(close + 2, expression.Length - (close + 2));
						return actionEvents[index].TryResolveExpression(rest, out result);
					}
				}
				result = null;
				return false;
			}
			switch (text)
			{
				case "actionduration":
					result = actionDuration.ToFloatString();
					return true;
				case "executetime":
					result = executeTime.ToFloatString();
					return true;
				case "timefrom":
					result = timeFrom.ToFloatString();
					return true;
				case "timeto":
					result = timeTo.ToFloatString();
					return true;
				case "movespeed":
					result = moveSpeed.ToFloatString();
					return true;
				case "movestarttime":
					result = moveStartTime.ToFloatString();
					return true;
				case "moveendtime":
					result = moveEndTime.ToFloatString();
					return true;
				case "moveoffset":
					result = moveOffset.ToFloatString();
					return true;
				case "maxaimanglepersecond":
					result = maxAimAnglePerSecond.ToFloatString();
					return true;
				case "castrangeradius":
					result = castRangeRadius.ToFloatString();
					return true;
				case "effectarearadius":
					result = effectAreaRadius.ToFloatString();
					return true;
				case "effectareaangle":
					result = effectAreaAngle.ToFloatString();
					return true;
				case "linelength":
					result = LineLength.ToFloatString();
					return true;
				case "groundsfxinterval":
					result = groundSfxInterval.ToFloatString();
					return true;
				case "skillid":
					result = string.Format("{0}", SkillId);
					return true;
			}
			result = null;
			return false;
		}

		public string[] ExtractExpressions()
		{
			return new string[15] { "actionduration", "executetime", "timefrom", "timeto", "movespeed", "movestarttime", "moveendtime", "moveoffset", "maxaimanglepersecond", "castrangeradius", "effectarearadius", "effectareaangle", "linelength", "groundsfxinterval", "skillid" };
		}

		public void RefreshUniqueId()
		{
			_uniqueId = UniqueIdFactory.CreateId();
		}

		public bool Validate(out string errorMessage)
		{
			bool isValid = true;
			StringBuilder stringBuilder = new StringBuilder();
			if (SkillTrigger != null && !SkillTrigger.Validate(out string triggerMessage))
			{
				isValid = false;
				stringBuilder.AppendLine(triggerMessage);
			}
			if (actionEvents != null)
			{
				actionEvents.ForEach(delegate(ActionEvent _)
				{
					if (!_.Validate(out string eventMessage))
					{
						isValid = false;
						stringBuilder.AppendLine(eventMessage);
					}
				});
			}
			errorMessage = stringBuilder.ToString();
			return isValid;
		}
	}
}
