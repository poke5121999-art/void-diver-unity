using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public struct LayerWeightBinding
{
	[Tooltip("제어할 레이어 인덱스")]
	[FormerlySerializedAs("layerIndex")]
	public int LayerIndex;

	[Tooltip("normalizedTime(0~1)에 따른 weight 커브 (0~1)")]
	[FormerlySerializedAs("weightCurve")]
	public AnimationCurve WeightCurve;
}
