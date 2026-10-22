using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[DefaultExecutionOrder(-100)]
[ExecuteAlways]
public class PlaneReflectionManager : MonoBehaviour
{
	[Header("Reflection Active")]
	[SerializeField]
	private bool _isUsingReflection;

	private bool _isUsingReflection_prev;

	[Header("Reflection Debug")]
	[SerializeField]
	private Camera _mainCamera;

	[SerializeField]
	private Camera _reflectionCamera;

	[SerializeField]
	private RenderTexture _reflectionTexture;

	[Header("Reflection Bound Check")]
	[SerializeField]
	private float _boundMinSize;

	[SerializeField]
	private float _boundBumpSize;

	public static PlaneReflectionManager Instance;

	public static List<PlaneReflectionObject> WaitingPlaneReflectionObjects;

	private readonly List<PlaneReflectionObject> _planeReflectionObjects;

	private PlaneReflectionSettings _planeReflectionSettings;

	private UniversalAdditionalCameraData _planeReflectionCameradata;

	private PlaneReflectionVolumeComponent _volumeComponent;

	private PlanarReflectionSettingData _settingData;

	private Plane[] _cachedFrustumPlanes;

	private int _lastFrameCheck;

	private bool _pendingActivation;

	public float BoundMinSize => 0f;

	public float BoundBumpSize => 0f;

	private void Awake()
	{
	}

	private void OnEnable()
	{
	}

	private void Start()
	{
	}

	private void OnDisable()
	{
	}

	private void OnDestroy()
	{
	}

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	private static void ResetStaticFields()
	{
	}

	private bool IsSettingValid()
	{
		return false;
	}

	private static bool IsReflectionOptionEnabled()
	{
		return false;
	}

	private static bool IsFinalPresentCandidate(Camera camera)
	{
		return false;
	}

	private bool IsValidCamera(Camera camera, out PlaneReflectionCameraTarget planeReflectionTarget, out PlaneReflectionSettings planeReflectionSettings)
	{
		planeReflectionTarget = null;
		planeReflectionSettings = null;
		return false;
	}

	private void OnBeginCameraRendering(ScriptableRenderContext context, Camera camera)
	{
	}

	private void RenderReflectionCamera(ScriptableRenderContext context, Camera reflectionCamera)
	{
	}

	private void PlanarReflectionTexture(Camera camera)
	{
	}

	private void ClearNewReflectionTexture(RenderTexture rt)
	{
	}

	private CustomCameraUtils.ECustomCameraResolutionType GetResolution()
	{
		return (CustomCameraUtils.ECustomCameraResolutionType)0;
	}

	private Vector2 ReflectionResolution(Camera camera, float scale)
	{
		return default;
	}

	private void UpdateReflectionCamera(Camera mainCamera)
	{
	}

	private bool CreateMirrorObjects()
	{
		return false;
	}

	private void UpdateReflectionCameraUsingMain(Camera mainCamera)
	{
	}

	private void UpdateReflectionBasicSettings(Camera mainCamera)
	{
	}

	private void UpdateReflectionTransform(Camera mainCamera)
	{
	}

	private Matrix4x4 CalculateReflectionMatrix()
	{
		return default;
	}

	private void UpdateReflectionRenderSettings()
	{
	}

	private void UpdateShaderProperties()
	{
	}

	private static Vector3 ReflectPosition(Vector3 position)
	{
		return default;
	}

	public void AddPlaneReflectionObjectToList(PlaneReflectionObject planeReflectionObject)
	{
	}

	public void RemovePlaneReflectionObjectFromList(PlaneReflectionObject planeReflectionObject)
	{
	}

	private void PerformBoundsCheck()
	{
	}

	private bool CheckCameraBoundsOverlap(Camera camera)
	{
		return false;
	}
}
