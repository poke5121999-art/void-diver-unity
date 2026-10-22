using System;
using GameCommon.Enum;
using UnityEngine;

[Serializable]
public class StressCameraEffectSequence
{
	public EStressState State;

	[Tooltip("Stress 단발성 효과의 시간과 세기를 커브로 설정합니다.")]
	public AnimationCurve OneShotCurve;

	public CameraEffectSequence Sequence;
}
