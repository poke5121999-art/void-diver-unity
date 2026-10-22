using UnityEngine;

[CreateAssetMenu(fileName = "Plane Reflection Setting", menuName = "LCArt/Plane Reflection Setting")]
public class PlaneReflectionSettings : ScriptableObject
{
	[Header("Reflection Setting: Common")]
	[Tooltip("렌더링 화질 설정입니다.")]
	[SerializeField]
	private CustomCameraUtils.ECustomCameraResolutionType _resolutionMultiplier;

	[Tooltip("반사 카메라의 렌더 설정입니다. 인덱스를 통해 렌더 데이터를 설정합니다.")]
	[SerializeField]
	private int _renderDataIndex;

	[Tooltip("반사 Offset 정도입니다. 공중에 떠 보이는 현상을 완화합니다.")]
	[SerializeField]
	private float _reflectionBias;

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

	public CustomCameraUtils.ECustomCameraResolutionType ResolutionMultiplier => (CustomCameraUtils.ECustomCameraResolutionType)0;

	public int RenderDataIndex => 0;

	public float ReflectionBias => 0f;

	public LayerMask ReflectionMask => default;

	public bool IsReflectionSkyBox => false;

	public bool IsDrawingShadow => false;
}
