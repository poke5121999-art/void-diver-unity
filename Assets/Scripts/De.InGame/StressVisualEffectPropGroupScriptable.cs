using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "Stress Visual Effect Prop Group Scriptable Data", menuName = "Project DE/Stress Visual Effect Prop Group Scriptable Data")]
public class StressVisualEffectPropGroupScriptable : ScriptableObject
{
	[Header("Random Setting")]
	[Tooltip("Stress 비주얼 이펙트를 수행할 확률입니다.")]
	[Range(0f, 1f)]
	public float ChangeRatio;

	[Header("Escape Setting")]
	[Tooltip("Stress 상태에서 벗어날 때의 딜레이입니다.")]
	public float EscapeDelay;

	[Tooltip("Stress 상태에서 벗어날 때의 페이드 지속 시간입니다.")]
	public float EscapeDuration;

	[Tooltip("Stress 상태에서 벗어날 때 사용할 이징 타입입니다.")]
	public Ease EscapeEase;

	[Header("Stress Setting")]
	[Tooltip("Stress 상태별 발생 수치 및 커브 설정입니다.")]
	[SerializeField]
	private List<StressVisualEffectSequence> _sequences;

	[Header("Floating Setting")]
	[Range(0f, 1f)]
	public float FloatingEnableThreshold;

	[Space(10f)]
	public Vector3 FloatingPosition;

	public Vector3 FloatingAdditionalPosition;

	[Space(10f)]
	public float FloatingPositionPeriodMin;

	public float FloatingPositionPeriodMax;

	[Space(10f)]
	public float FloatingRotation;

	[Space(10f)]
	public float FloatingRotationPeriodMin;

	public float FloatingRotationPeriodMax;

	private readonly int _stressFloatingEnableThresholdID;

	private readonly int _stressFloatingPositionID;

	private readonly int _stressFloatingAdditionalPositionID;

	private readonly int _stressFloatingRotationID;

	private readonly int _stressFloatingPositionPeriodMinID;

	private readonly int _stressFloatingPositionPeriodMaxID;

	private readonly int _stressFloatingRotationPeriodMinID;

	private readonly int _stressFloatingRotationPeriodMaxID;

	public List<StressVisualEffectSequence> Sequences => null;

	public void ApplyGlobalStressProperties()
	{
	}
}
