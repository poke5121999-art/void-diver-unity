using System;
using System.Runtime.Serialization;
using GameCommon.Enum;
using Newtonsoft.Json;
using UnityEngine;

namespace GameCommon.Data
{
	[Serializable]
	public class TrailBuffEffect : BuffEffect
	{
		public int TrailCount;

		public float TrailPeriod;

		[JsonIgnore]
		public Color StartColor;

		[JsonIgnore]
		public Color EndColor;

		[HideInInspector]
		public ColorData StartColorData;

		[HideInInspector]
		public ColorData EndColorData;

		public override EBuffEffectType EffectType => EBuffEffectType.Trail;

		public override bool TryResolveExpression(string expression, out string result)
		{
			result = null;
			return false;
		}

		public override string[] ExtractExpressions()
		{
			return null;
		}

		public override bool Validate(out string errorMessage)
		{
			errorMessage = null;
			return false;
		}

		private void OnColorChanged()
		{
		}

		[OnDeserialized]
		private void OnDeserialized(StreamingContext context)
		{
		}
	}
}
