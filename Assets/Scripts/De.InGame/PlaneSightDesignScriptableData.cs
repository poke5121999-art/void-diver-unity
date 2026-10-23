using UnityEngine;

[CreateAssetMenu(fileName = "Plane Sight Design Scriptable Data", menuName = "NEMO Art/Plane Sight Design Scriptable Data")]
public class PlaneSightDesignScriptableData : ScriptableObject
{
	[Header("Sight On Setting")]
	[Tooltip("On 상태에서 시야 밝기 계수")]
	[SerializeField]
	private float _sightOnAlphaFactor = 1f;

	[Tooltip("On 상태에서 시야 각도 계수")]
	[SerializeField]
	private float _sightOnAngleFactor = 1f;

	[Tooltip("On 상태에서 시야 전방 거리 계수")]
	[SerializeField]
	private float _sightOnFrontRangeFactor = 1f;

	[Tooltip("On 상태에서 시야 후방 거리 계수")]
	[SerializeField]
	private float _sightOnBackRangeFactor = 1f;

	[Header("Sight Off Setting")]
	[Tooltip("Off 상태에서 시야 밝기 계수")]
	[SerializeField]
	private float _sightOffAlphaFactor = 0.25f;

	[Tooltip("Off 상태에서 시야 각도 계수")]
	[SerializeField]
	private float _sightOffAngleFactor = 0.25f;

	[Tooltip("Off 상태에서 시야 전방 거리 계수")]
	[SerializeField]
	private float _sightOffFrontRangeFactor = 0.85f;

	[Tooltip("Off 상태에서 시야 후방 거리 계수")]
	[SerializeField]
	private float _sightOffBackRangeFactor = 1f;

	[Header("Blinded Setting")]
	[Tooltip("Blinded 상태에서 시야 밝기")]
	[SerializeField]
	private float _blindedSightAlpha = 0.5f;

	[Tooltip("Blinded 상태에서 시야 각도")]
	[SerializeField]
	private float _blindedSightAngle;

	[Tooltip("Blinded 상태에서 시야 전방 거리")]
	[SerializeField]
	private float _blindedSightRange = 1f;

	[Tooltip("Blinded 상태에서 시야 후방 거리")]
	[SerializeField]
	private float _blindedSightBackRange = 1f;

	[Header("Delta Setting - On/Off")]
	[Tooltip("On/Off 토글 시 시야 변화량")]
	[SerializeField]
	private float _sightMorphDelta = 10f;

	[Tooltip("On/Off 토글 시 감소 방향 변화량")]
	[SerializeField]
	private float _sightMorphNegativeDelta = 10f;

	[Header("Delta Setting - Blinded")]
	[Tooltip("Blinded 상태의 시야 변화량")]
	[SerializeField]
	private float _blindedSightDelta = 10f;

	[Tooltip("Blinded 상태의 시야 감소 방향 변화량")]
	[SerializeField]
	private float _blindedSightNegativeDelta = 5f;

	[Header("Delta Setting - Sharing")]
	[Tooltip("공유 시야의 시야 변화량")]
	[SerializeField]
	private float _sharingSightDelta = 10f;

	[Tooltip("공유 시야의 감소 방향 변화량")]
	[SerializeField]
	private float _sharingSightNegativeDelta = 10f;

	[Header("Ease Setting")]
	[Tooltip("캐릭터 시야 방향 변경 강도")]
	[SerializeField]
	private float _sightDirectionStiffness = 20f;

	[Header("Shaper Setting")]
	[Tooltip("Plane Sight 효과 머티리얼")]
	[SerializeField]
	private Material _planeSightShaperMaterial;

	[Tooltip("Plane Sight 피벗 머티리얼")]
	[SerializeField]
	private Material _planeSightShaperPivotMaterial;

	[Tooltip("Shaper 효과 색상")]
	[SerializeField]
	private Color _planeSightShaperColor = Color.white;

	[Tooltip("Shaper 효과 강도")]
	[SerializeField]
	private float _planeSightShaperIntensity = 1f;

	[Header("Vignette Effect Setting")]
	[SerializeField]
	private float _sightVignetteDelta = 5f;

	[SerializeField]
	private float _sightVignetteNegativeDelta = 5f;

	[Header("Fuel Blink Setting")]
	[SerializeField]
	private bool _fuelBlinkEnable;

	[SerializeField]
	private float _fuelBlinkDurationMax = 0.1f;

	[SerializeField]
	private float _fuelBlinkDurationMin = 0.05f;

	[SerializeField]
	private int _fuelBlinkStartThreshold = 30;

	[SerializeField]
	private float _fuelBlinkMaxDelayAtThreshold = 5f;

	[SerializeField]
	private float _fuelBlinkMaxDelayAtZero = 1f;

	public float SightOnAlphaFactor => Mathf.Max(0f, _sightOnAlphaFactor);

	public float SightOnAngleFactor => Mathf.Max(0f, _sightOnAngleFactor);

	public float SightOnFrontRangeFactor => Mathf.Max(0f, _sightOnFrontRangeFactor);

	public float SightOnBackRangeFactor => Mathf.Max(0f, _sightOnBackRangeFactor);

	public float SightOffAlphaFactor => Mathf.Max(0f, _sightOffAlphaFactor);

	public float SightOffAngleFactor => Mathf.Max(0f, _sightOffAngleFactor);

	public float SightOffFrontRangeFactor => Mathf.Max(0f, _sightOffFrontRangeFactor);

	public float SightOffBackRangeFactor => Mathf.Max(0f, _sightOffBackRangeFactor);

	public float BlindedSightAlpha => Mathf.Max(0f, _blindedSightAlpha);

	public float BlindedSightAngle => Mathf.Max(0f, _blindedSightAngle);

	public float BlindedSightRange => Mathf.Max(0f, _blindedSightRange);

	public float BlindedSightBackRange => Mathf.Max(0f, _blindedSightBackRange);

	public float SightMorphDelta => Mathf.Max(0f, _sightMorphDelta);

	public float SightMorphNegativeDelta => Mathf.Max(0f, _sightMorphNegativeDelta);

	public float BlindedSightDelta => Mathf.Max(0f, _blindedSightDelta);

	public float BlindedSightNegativeDelta => Mathf.Max(0f, _blindedSightNegativeDelta);

	public float SharingSightDelta => Mathf.Max(0f, _sharingSightDelta);

	public float SharingSightNegativeDelta => Mathf.Max(0f, _sharingSightNegativeDelta);

	public float SightDirectionStiffness => Mathf.Max(0f, _sightDirectionStiffness);

	public Color PlaneSightShaperColor => _planeSightShaperColor;

	public float PlaneSightShaperIntensity => Mathf.Max(0f, _planeSightShaperIntensity);

	public float SightVignetteDelta => _sightVignetteDelta;

	public float SightVignetteNegativeDelta => _sightVignetteNegativeDelta;

	public bool FuelBlinkEnable => _fuelBlinkEnable;

	public float FuelBlinkDurationMax => _fuelBlinkDurationMax;

	public float FuelBlinkDurationMin => _fuelBlinkDurationMin;

	public int FuelBlinkStartThreshold => _fuelBlinkStartThreshold;

	public float FuelBlinkMaxDelayAtThreshold => _fuelBlinkMaxDelayAtThreshold;

	public float FuelBlinkMaxDelayAtZero => _fuelBlinkMaxDelayAtZero;
}
