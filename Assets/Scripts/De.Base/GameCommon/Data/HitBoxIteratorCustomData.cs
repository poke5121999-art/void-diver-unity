using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameCommon.Data
{
	[Serializable]
	public class HitBoxIteratorCustomData
	{
		public float CustomAdditionalDelay;

		[FormerlySerializedAs("CustomOffset")]
		public Vector3 CustomPositionOffset;

		public float CustomAngle;
	}
}
