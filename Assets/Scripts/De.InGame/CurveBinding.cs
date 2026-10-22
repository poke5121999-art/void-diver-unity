using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public struct CurveBinding
{
	[Tooltip("바인딩 이름 (다른 스크립트에서 이 이름으로 값을 조회)")]
	[FormerlySerializedAs("name")]
	public string Name;

	[Tooltip("normalizedTime(0~1)에 따른 값 커브")]
	[FormerlySerializedAs("curve")]
	public AnimationCurve Curve;
}
