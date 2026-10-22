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
			errorMessage = null;
			return false;
		}
	}
}
