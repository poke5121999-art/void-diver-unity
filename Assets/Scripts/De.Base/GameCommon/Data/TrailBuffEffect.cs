using System.Linq;
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
		public int TrailCount = 3;

		public float TrailPeriod = 0.1f;

		[JsonIgnore]
		public Color StartColor = Color.white;

		[JsonIgnore]
		public Color EndColor = Color.white;

		[HideInInspector]
		public ColorData StartColorData = new ColorData(Color.white);

		[HideInInspector]
		public ColorData EndColorData = new ColorData(Color.white);

		public override EBuffEffectType EffectType => EBuffEffectType.Trail;

		public override bool TryResolveExpression(string expression, out string result)
		{
			if (base.TryResolveExpression(expression, out result))
			{
				return true;
			}
			if (expression == "TrailCount")
			{
				result = TrailCount.ToString();
			}
			else if (expression == "TrailPeriod")
			{
				result = TrailPeriod.ToFloatString();
			}
			else
			{
				result = null;
			}
			return result != null;
		}

		public override string[] ExtractExpressions()
		{
			return base.ExtractExpressions().Concat(new string[2] { "TrailCount", "TrailPeriod" }).ToArray();
		}

		public override bool Validate(out string errorMessage)
		{
			errorMessage = string.Empty;
			return true;
		}

		private void OnColorChanged()
		{
			StartColorData = new ColorData(StartColor);
		}

		[OnDeserialized]
		private void OnDeserialized(StreamingContext context)
		{
			if (StartColorData != null)
			{
				StartColor = StartColorData.ToColor();
				if (EndColorData != null)
				{
					EndColor = EndColorData.ToColor();
				}
			}
		}
	}
}
