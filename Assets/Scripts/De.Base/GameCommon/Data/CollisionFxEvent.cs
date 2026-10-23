using GameCommon.Table;
using System.Text;
using System;
using GameCommon.Enum;
using UnityEngine;

namespace GameCommon.Data
{
	[Serializable]
	public class CollisionFxEvent : CollisionEvent
	{
		public string Vfx;

		public float VfxDuration;

		public bool IsLoopVfx;

		public float VfxLoopDuration;

		public EUnitBoneType BoneType;

		public Vector3 Offset;

		public AnimationSpeed[] VfxSpeeds;

		public bool IsIndependent;

		public string Sfx;

		public bool UseRotation;

		public override bool Validate(out string errorMessage)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			if (!base.Validate(out var errorMessage2))
			{
				stringBuilder.AppendLine(errorMessage2);
				flag = false;
			}
			if (!ValidationExtensions.ContainsSfx(Sfx))
			{
				flag = false;
				stringBuilder.AppendLine("MasterAudio not contains \"[CollisionFxEvent.Sfx] " + Sfx + "\"");
			}
			errorMessage = stringBuilder.ToString();
			return flag;
		}
	}
}
