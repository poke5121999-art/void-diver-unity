using UnityEngine;

[CreateAssetMenu(fileName = "Plane Sight Design Scriptable Data", menuName = "NEMO Art/Plane Sight Design Scriptable Data")]
public class PlaneSightDesignScriptableData : ScriptableObject
{
	[Header("Sight On Setting")]
	[Tooltip("On 상태에서 시야 밝기 계수")]
	[SerializeField]
	private float _sightOnAlphaFactor;

	[Tooltip("On 상태에서 시야 각도 계수")]
	[SerializeField]
	private float _sightOnAngleFactor;

	[Tooltip("On 상태에서 시야 전방 거리 계수")]
	[SerializeField]
	private float _sightOnFrontRangeFactor;

	[Tooltip("On 상태에서 시야 후방 거리 계수")]
	[SerializeField]
	private float _sightOnBackRangeFactor;

	[Header("Sight Off Setting")]
	[Tooltip("Off 상태에서 시야 밝기 계수")]
	[SerializeField]
	private float _sightOffAlphaFactor;

	[Tooltip("Off 상태에서 시야 각도 계수")]
	[SerializeField]
	private float _sightOffAngleFactor;

	[Tooltip("Off 상태에서 시야 전방 거리 계수")]
	[SerializeField]
	private float _sightOffFrontRangeFactor;

	[Tooltip("Off 상태에서 시야 후방 거리 계수")]
	[SerializeField]
	private float _sightOffBackRangeFactor;

	[Header("Blinded Setting")]
	[Tooltip("Blinded 상태에서 시야 밝기")]
	[SerializeField]
	private float _blindedSightAlpha;

	[Tooltip("Blinded 상태에서 시야 각도")]
	[SerializeField]
	private float _blindedSightAngle;

	[Tooltip("Blinded 상태에서 시야 전방 거리")]
	[SerializeField]
	private float _blindedSightRange;

	[Tooltip("Blinded 상태에서 시야 후방 거리")]
	[SerializeField]
	private float _blindedSightBackRange;

	[Header("Delta Setting - On/Off")]
	[Tooltip("On/Off 토글 시 시야 변화량")]
	[SerializeField]
	private float _sightMorphDelta;

	[Tooltip("On/Off 토글 시 감소 방향 변화량")]
	[SerializeField]
	private float _sightMorphNegativeDelta;

	[Header("Delta Setting - Blinded")]
	[Tooltip("Blinded 상태의 시야 변화량")]
	[SerializeField]
	private float _blindedSightDelta;

	[Tooltip("Blinded 상태의 시야 감소 방향 변화량")]
	[SerializeField]
	private float _blindedSightNegativeDelta;

	[Header("Delta Setting - Sharing")]
	[Tooltip("공유 시야의 시야 변화량")]
	[SerializeField]
	private float _sharingSightDelta;

	[Tooltip("공유 시야의 감소 방향 변화량")]
	[SerializeField]
	private float _sharingSightNegativeDelta;

	[Header("Ease Setting")]
	[Tooltip("캐릭터 시야 방향 변경 강도")]
	[SerializeField]
	private float _sightDirectionStiffness;

	[Header("Shaper Setting")]
	[Tooltip("Plane Sight 효과 머티리얼")]
	[SerializeField]
	private Material _planeSightShaperMaterial;

	[Tooltip("Plane Sight 피벗 머티리얼")]
	[SerializeField]
	private Material _planeSightShaperPivotMaterial;

	[Tooltip("Shaper 효과 색상")]
	[SerializeField]
	private Color _planeSightShaperColor;

	[Tooltip("Shaper 효과 강도")]
	[SerializeField]
	private float _planeSightShaperIntensity;

	[Header("Vignette Effect Setting")]
	[SerializeField]
	private float _sightVignetteDelta;

	[SerializeField]
	private float _sightVignetteNegativeDelta;

	[Header("Fuel Blink Setting")]
	[SerializeField]
	private bool _fuelBlinkEnable;

	[SerializeField]
	private float _fuelBlinkDurationMax;

	[SerializeField]
	private float _fuelBlinkDurationMin;

	[SerializeField]
	private int _fuelBlinkStartThreshold;

	[SerializeField]
	private float _fuelBlinkMaxDelayAtThreshold;

	[SerializeField]
	private float _fuelBlinkMaxDelayAtZero;

	public float SightOnAlphaFactor => 0f;

	public float SightOnAngleFactor => 0f;

	public float SightOnFrontRangeFactor => 0f;

	public float SightOnBackRangeFactor => 0f;

	public float SightOffAlphaFactor => 0f;

	public float SightOffAngleFactor => 0f;

	public float SightOffFrontRangeFactor => 0f;

	public float SightOffBackRangeFactor => 0f;

	public float BlindedSightAlpha => 0f;

	public float BlindedSightAngle => 0f;

	public float BlindedSightRange => 0f;

	public float BlindedSightBackRange => 0f;

	public float SightMorphDelta => 0f;

	public float SightMorphNegativeDelta => 0f;

	public float BlindedSightDelta => 0f;

	public float BlindedSightNegativeDelta => 0f;

	public float SharingSightDelta => 0f;

	public float SharingSightNegativeDelta => 0f;

	public float SightDirectionStiffness => 0f;

	public Color PlaneSightShaperColor => default;

	public float PlaneSightShaperIntensity => 0f;

	public float SightVignetteDelta => 0f;

	public float SightVignetteNegativeDelta => 0f;

	public bool FuelBlinkEnable => false;

	public float FuelBlinkDurationMax => 0f;

	public float FuelBlinkDurationMin => 0f;

	public int FuelBlinkStartThreshold => 0;

	public float FuelBlinkMaxDelayAtThreshold => 0f;

	public float FuelBlinkMaxDelayAtZero => 0f;
}
