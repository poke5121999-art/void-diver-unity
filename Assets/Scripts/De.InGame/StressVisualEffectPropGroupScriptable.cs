using GameCommon.Enum;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "Stress Visual Effect Prop Group Scriptable Data", menuName = "Project DE/Stress Visual Effect Prop Group Scriptable Data")]
public class StressVisualEffectPropGroupScriptable : ScriptableObject
{
	[Header("Random Setting")]
	[Tooltip("Stress 비주얼 이펙트를 수행할 확률입니다.")]
	[Range(0f, 1f)]
	public float ChangeRatio = 0.5f;

	[Header("Escape Setting")]
	[Tooltip("Stress 상태에서 벗어날 때의 딜레이입니다.")]
	public float EscapeDelay = 1f;

	[Tooltip("Stress 상태에서 벗어날 때의 페이드 지속 시간입니다.")]
	public float EscapeDuration = 2f;

	[Tooltip("Stress 상태에서 벗어날 때 사용할 이징 타입입니다.")]
	public Ease EscapeEase = Ease.InOutSine;

	[Header("Stress Setting")]
	[Tooltip("Stress 상태별 발생 수치 및 커브 설정입니다.")]
	[SerializeField]
	private List<StressVisualEffectSequence> _sequences = new List<StressVisualEffectSequence>
	{
		new StressVisualEffectSequence(EStressState.Alert, 0f, 0f),
		new StressVisualEffectSequence(EStressState.Fear, 0f, 1f),
		new StressVisualEffectSequence(EStressState.Despair, 0f, 1f)
	};

	[Header("Floating Setting")]
	[Range(0f, 1f)]
	public float FloatingEnableThreshold = 0.5f;

	[Space(10f)]
	public Vector3 FloatingPosition = new Vector3(0f, 0.3f, 0f);

	public Vector3 FloatingAdditionalPosition = new Vector3(0.025f, 0.025f, 0.025f);

	[Space(10f)]
	public float FloatingPositionPeriodMin = 5f;

	public float FloatingPositionPeriodMax = 10f;

	[Space(10f)]
	public float FloatingRotation = 10f;

	[Space(10f)]
	public float FloatingRotationPeriodMin = 5f;

	public float FloatingRotationPeriodMax = 10f;

	private readonly int _stressFloatingEnableThresholdID = Shader.PropertyToID("_GlobalStressFloatingEnableThreshold");

	private readonly int _stressFloatingPositionID = Shader.PropertyToID("_GlobalStressFloatingPosition");

	private readonly int _stressFloatingAdditionalPositionID = Shader.PropertyToID("_GlobalStressFloatingAdditionalPosition");

	private readonly int _stressFloatingRotationID = Shader.PropertyToID("_GlobalStressFloatingRotation");

	private readonly int _stressFloatingPositionPeriodMinID = Shader.PropertyToID("_GlobalStressFloatingPositionPeriodMin");

	private readonly int _stressFloatingPositionPeriodMaxID = Shader.PropertyToID("_GlobalStressFloatingPositionPeriodMax");

	private readonly int _stressFloatingRotationPeriodMinID = Shader.PropertyToID("_GlobalStressFloatingRotationPeriodMin");

	private readonly int _stressFloatingRotationPeriodMaxID = Shader.PropertyToID("_GlobalStressFloatingRotationPeriodMax");

	public List<StressVisualEffectSequence> Sequences => _sequences;

	public void ApplyGlobalStressProperties()
	{
		Shader.SetGlobalFloat(_stressFloatingEnableThresholdID, FloatingEnableThreshold);
		Shader.SetGlobalVector(_stressFloatingPositionID, FloatingPosition);
		Shader.SetGlobalVector(_stressFloatingAdditionalPositionID, FloatingAdditionalPosition);
		Shader.SetGlobalFloat(_stressFloatingRotationID, FloatingRotation);
		Shader.SetGlobalFloat(_stressFloatingPositionPeriodMinID, FloatingPositionPeriodMin);
		Shader.SetGlobalFloat(_stressFloatingPositionPeriodMaxID, FloatingPositionPeriodMax);
		Shader.SetGlobalFloat(_stressFloatingRotationPeriodMinID, FloatingRotationPeriodMin);
		Shader.SetGlobalFloat(_stressFloatingRotationPeriodMaxID, FloatingRotationPeriodMax);
	}
}
