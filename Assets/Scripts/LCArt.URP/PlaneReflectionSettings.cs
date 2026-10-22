using UnityEngine;

[CreateAssetMenu(fileName = "Plane Reflection Setting", menuName = "LCArt/Plane Reflection Setting")]
public class PlaneReflectionSettings : ScriptableObject
{
	[Header("Reflection Setting: Common")]
	[Tooltip("렌더링 화질 설정입니다.")]
	[SerializeField]
	private CustomCameraUtils.ECustomCameraResolutionType _resolutionMultiplier = CustomCameraUtils.ECustomCameraResolutionType.Half;

	[Tooltip("반사 카메라의 렌더 설정입니다. 인덱스를 통해 렌더 데이터를 설정합니다.")]
	[SerializeField]
	private int _renderDataIndex = 3;

	[Tooltip("반사 Offset 정도입니다. 공중에 떠 보이는 현상을 완화합니다.")]
	[SerializeField]
	private float _reflectionBias = 0.3f;

	[Header("Reflection Setting: Toggles")]
	[Tooltip("반사 오브젝트에 렌더링될 레이어를 설정합니다.")]
	[SerializeField]
	private LayerMask _reflectionMask;

	[Tooltip("스카이 박스 반사 옵션입니다.")]
	[SerializeField]
	private bool _isReflectingSkybox;

	[Tooltip("그림자 출력 여부입니다.")]
	[SerializeField]
	private bool _isDrawingShadow;

	public CustomCameraUtils.ECustomCameraResolutionType ResolutionMultiplier => _resolutionMultiplier;

	public int RenderDataIndex => _renderDataIndex;

	public float ReflectionBias => _reflectionBias;

	public LayerMask ReflectionMask => _reflectionMask;

	public bool IsReflectionSkyBox => _isReflectingSkybox;

	public bool IsDrawingShadow => _isDrawingShadow;
}
