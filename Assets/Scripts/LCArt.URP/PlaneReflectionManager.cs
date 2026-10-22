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
	private bool _isUsingReflection = true;

	private bool _isUsingReflection_prev = true;

	[Header("Reflection Debug")]
	[SerializeField]
	private Camera _mainCamera;

	[SerializeField]
	private Camera _reflectionCamera;

	[SerializeField]
	private RenderTexture _reflectionTexture;

	[Header("Reflection Bound Check")]
	[SerializeField]
	private float _boundMinSize = 2.5f;

	[SerializeField]
	private float _boundBumpSize = -5f;

	public static PlaneReflectionManager Instance;

	public static List<PlaneReflectionObject> WaitingPlaneReflectionObjects = new List<PlaneReflectionObject>();

	private readonly List<PlaneReflectionObject> _planeReflectionObjects = new List<PlaneReflectionObject>();

	private PlaneReflectionSettings _planeReflectionSettings;

	private UniversalAdditionalCameraData _planeReflectionCameradata;

	private PlaneReflectionVolumeComponent _volumeComponent;

	private PlanarReflectionSettingData _settingData;

	private Plane[] _cachedFrustumPlanes = new Plane[6];

	private int _lastFrameCheck = -1;

	private bool _pendingActivation;

	public float BoundMinSize => _boundMinSize;

	public float BoundBumpSize => _boundBumpSize;

	private void Awake()
	{
		_planeReflectionObjects.Clear();
	}

	private void OnEnable()
	{
		if (Instance != null && Instance != this)
		{
			if (!Application.isPlaying)
			{
				Object.DestroyImmediate(gameObject);
			}
			else
			{
				Object.Destroy(gameObject);
			}
			return;
		}
		Instance = this;
		_planeReflectionObjects.AddRange(WaitingPlaneReflectionObjects);
		WaitingPlaneReflectionObjects.Clear();
		Shader.SetGlobalInt(PlaneReflectionConsts.SHADER_PROPERTY_NAME_ACTIVE, _isUsingReflection ? 1 : 0);
		_isUsingReflection_prev = _isUsingReflection;
		RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
	}

	private void Start()
	{
	}

	private void OnDisable()
	{
		RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
		if (_reflectionCamera != null)
		{
			_reflectionCamera.gameObject.SetActive(false);
		}
		if (_reflectionTexture != null)
		{
			DeferredGpuRelease.ReleaseTemporaryRT(_reflectionTexture);
			_reflectionTexture = null;
		}
	}

	private void OnDestroy()
	{
		if (_reflectionTexture != null)
		{
			DeferredGpuRelease.ReleaseTemporaryRT(_reflectionTexture);
			_reflectionTexture = null;
		}
		if ((bool)_reflectionCamera)
		{
			if (_reflectionCamera.targetTexture != null)
			{
				_reflectionCamera.targetTexture = null;
			}
			if (!Application.isPlaying)
			{
				Object.DestroyImmediate(_reflectionCamera.gameObject);
			}
			else
			{
				Object.Destroy(_reflectionCamera.gameObject);
			}
		}
		if (Instance == this)
		{
			Instance = null;
		}
	}

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	private static void ResetStaticFields()
	{
		WaitingPlaneReflectionObjects.Clear();
		Instance = null;
	}

	private bool IsSettingValid()
	{
		return _planeReflectionSettings != null;
	}

	private static bool IsReflectionOptionEnabled()
	{
		return Shader.GetGlobalFloat(PlaneReflectionConsts.OPTION_DISABLE_REFLECTION) < 0.5f;
	}

	private static bool IsFinalPresentCandidate(Camera camera)
	{
		if (camera == null)
		{
			return false;
		}
		if (!camera.enabled || !camera.gameObject.activeInHierarchy)
		{
			return false;
		}
		if (camera.cameraType != CameraType.Game)
		{
			return false;
		}
		if (camera.targetTexture != null)
		{
			return false;
		}
		if (!camera.TryGetComponent<UniversalAdditionalCameraData>(out var data))
		{
			return false;
		}
		return data.renderType == CameraRenderType.Base;
	}

	private bool IsValidCamera(Camera camera, out PlaneReflectionCameraTarget planeReflectionTarget, out PlaneReflectionSettings planeReflectionSettings)
	{
		planeReflectionTarget = null;
		planeReflectionSettings = null;
		if (camera == null)
		{
			return false;
		}
		if (!camera.enabled || !camera.gameObject.activeInHierarchy)
		{
			return false;
		}
		if (camera.cameraType != CameraType.Game)
		{
			return false;
		}
		if (camera.targetTexture != null)
		{
			return false;
		}
		if (!camera.TryGetComponent<UniversalAdditionalCameraData>(out var data))
		{
			return false;
		}
		if (data.renderType != CameraRenderType.Base)
		{
			return false;
		}
		planeReflectionTarget = camera.gameObject.GetComponent<PlaneReflectionCameraTarget>();
		planeReflectionSettings = (planeReflectionTarget != null) ? planeReflectionTarget.PlaneReflectionSettings : null;
		return planeReflectionTarget != null;
	}

	private void OnBeginCameraRendering(ScriptableRenderContext context, Camera camera)
	{
		PlaneReflectionCameraTarget target = null;
		_lastFrameCheck--;
		if (_lastFrameCheck < 1)
		{
			PerformBoundsCheck();
			_lastFrameCheck = PlaneReflectionConsts.NUMBER_CAMERA_CHECK_COUNT_FRAME;
		}
		if (!IsValidCamera(camera, out target, out _planeReflectionSettings) || !_isUsingReflection)
		{
			return;
		}
		if (!IsReflectionOptionEnabled())
		{
			return;
		}
		if (_reflectionCamera != null && !_reflectionCamera.gameObject.activeInHierarchy)
		{
			return;
		}
		_mainCamera = camera;
		if (_reflectionCamera == null)
		{
			CreateMirrorObjects();
		}
		UpdateReflectionCameraUsingMain(camera);
		PlanarReflectionTexture(camera);
		if (_settingData == null)
		{
			_settingData = new PlanarReflectionSettingData();
		}
		_settingData.Set();
		RenderReflectionCamera(context, _reflectionCamera);
		_settingData.Restore();
		if (_pendingActivation)
		{
			_pendingActivation = false;
			Shader.SetGlobalInt(PlaneReflectionConsts.SHADER_PROPERTY_NAME_ACTIVE, 1);
		}
	}

	private void RenderReflectionCamera(ScriptableRenderContext context, Camera reflectionCamera)
	{
		if (reflectionCamera != null)
		{
			UniversalRenderPipeline.SingleCameraRequest request = new UniversalRenderPipeline.SingleCameraRequest
			{
				destination = _reflectionTexture
			};
			if (RenderPipeline.SupportsRenderRequest(reflectionCamera, request))
			{
				RenderPipeline.SubmitRenderRequest(reflectionCamera, request);
			}
		}
	}

	private void PlanarReflectionTexture(Camera camera)
	{
		Vector2 size = ReflectionResolution(camera, UniversalRenderPipeline.asset.renderScale);
		if (_reflectionTexture == null)
		{
			_reflectionTexture = RenderTexture.GetTemporary((int)size.x, (int)size.y, 16, RenderTextureFormat.DefaultHDR);
			ClearNewReflectionTexture(_reflectionTexture);
		}
		else if (_reflectionTexture.width != (int)size.x || _reflectionTexture.height != (int)size.y)
		{
			RenderTexture oldTexture = _reflectionTexture;
			_reflectionTexture = RenderTexture.GetTemporary((int)size.x, (int)size.y, 16, RenderTextureFormat.DefaultHDR);
			ClearNewReflectionTexture(_reflectionTexture);
			DeferredGpuRelease.ReleaseTemporaryRT(oldTexture);
		}
		_reflectionCamera.targetTexture = _reflectionTexture;
		Shader.SetGlobalTexture(PlaneReflectionConsts.SHADER_PROPERTY_NAME_SAMPLER, _reflectionTexture);
	}

	private void ClearNewReflectionTexture(RenderTexture rt)
	{
		RenderTexture previous = RenderTexture.active;
		RenderTexture.active = rt;
		Color clearColor = (_reflectionCamera != null) ? _reflectionCamera.backgroundColor : new Color(0f, 0f, 0f, 0f);
		GL.Clear(true, true, clearColor);
		RenderTexture.active = previous;
	}

	private CustomCameraUtils.ECustomCameraResolutionType GetResolution()
	{
		if (_planeReflectionSettings != null)
		{
			CustomCameraUtils.ECustomCameraResolutionType resolution = _planeReflectionSettings.ResolutionMultiplier;
			if (Application.isPlaying)
			{
				resolution = (CustomCameraUtils.ECustomCameraResolutionType)Shader.GetGlobalInt(PlaneReflectionConsts.OPTION_RESOLUTION_TYPE);
			}
			return resolution;
		}
		return CustomCameraUtils.ECustomCameraResolutionType.Full;
	}

	private Vector2 ReflectionResolution(Camera camera, float scale)
	{
		if (camera == null)
		{
			return new Vector2(1600f, 900f);
		}
		CustomCameraUtils.GetCappedRTSize((int)((float)camera.pixelWidth * scale), (int)((float)camera.pixelHeight * scale), out var cappedWidth, out var cappedHeight);
		CustomCameraUtils.ECustomCameraResolutionType resolution = GetResolution();
		float downsampling = CustomCameraUtils.GetDownsamplingFloat(in resolution);
		return new Vector2((int)((float)cappedWidth * downsampling), (int)((float)cappedHeight * downsampling));
	}

	private void UpdateReflectionCamera(Camera mainCamera)
	{
		if (_reflectionCamera == null)
		{
			CreateMirrorObjects();
		}
		UpdateReflectionCameraUsingMain(mainCamera);
	}

	private bool CreateMirrorObjects()
	{
		if (_planeReflectionSettings == null)
		{
			return false;
		}
		GameObject mirrorObject = new GameObject("PlaneReflectionCamera", typeof(Camera));
		_reflectionCamera = mirrorObject.GetComponent<Camera>();
		_planeReflectionCameradata = mirrorObject.AddComponent<UniversalAdditionalCameraData>();
		mirrorObject.hideFlags = HideFlags.HideAndDontSave;
		mirrorObject.transform.parent = gameObject.transform;
		_planeReflectionCameradata.requiresColorOption = CameraOverrideOption.Off;
		_planeReflectionCameradata.requiresDepthOption = CameraOverrideOption.Off;
		_reflectionCamera.enabled = false;
		return true;
	}

	private void UpdateReflectionCameraUsingMain(Camera mainCamera)
	{
		if (_planeReflectionSettings == null || _reflectionCamera == null)
		{
			return;
		}
		_volumeComponent = VolumeManager.instance.stack.GetComponent<PlaneReflectionVolumeComponent>();
		UpdateReflectionBasicSettings(mainCamera);
		UpdateReflectionTransform(mainCamera);
		UpdateReflectionRenderSettings();
		UpdateShaderProperties();
	}

	private void UpdateReflectionBasicSettings(Camera mainCamera)
	{
		_reflectionCamera.farClipPlane = mainCamera.farClipPlane;
		_reflectionCamera.nearClipPlane = mainCamera.nearClipPlane;
		_reflectionCamera.fieldOfView = mainCamera.fieldOfView;
		_reflectionCamera.aspect = mainCamera.aspect;
		_reflectionCamera.orthographic = mainCamera.orthographic;
		_reflectionCamera.orthographicSize = mainCamera.orthographicSize;
		_reflectionCamera.cameraType = CameraType.Reflection;
		_reflectionCamera.cullingMask = _planeReflectionSettings.ReflectionMask;
	}

	private void UpdateReflectionTransform(Camera mainCamera)
	{
		Transform reflectionTransform = _reflectionCamera.transform;
		Transform mainTransform = mainCamera.transform;
		reflectionTransform.position = ReflectPosition(mainTransform.position);
		Vector3 forward = mainTransform.forward;
		reflectionTransform.forward = new Vector3(forward.x, forward.y * -1f, forward.z);
		Matrix4x4 reflection = CalculateReflectionMatrix();
		_reflectionCamera.worldToCameraMatrix = _reflectionCamera.worldToCameraMatrix * reflection;
	}

	private Matrix4x4 CalculateReflectionMatrix()
	{
		Matrix4x4 matrix = Matrix4x4.identity * Matrix4x4.Scale(new Vector3(1f, -1f, 1f));
		if ((bool)_volumeComponent)
		{
			float worldY = _volumeComponent.ReflectionWorldY.value;
			Matrix4x4 translation = Matrix4x4.Translate(new Vector3(0f, -_planeReflectionSettings.ReflectionBias + worldY * -2f, 0f));
			matrix *= translation;
		}
		return matrix;
	}

	private void UpdateReflectionRenderSettings()
	{
		if (_reflectionCamera.gameObject.TryGetComponent<UniversalAdditionalCameraData>(out var data))
		{
			data.SetRenderer(_planeReflectionSettings.RenderDataIndex);
			data.requiresColorOption = CameraOverrideOption.Off;
			data.requiresDepthOption = CameraOverrideOption.Off;
			data.renderShadows = _planeReflectionSettings.IsDrawingShadow;
			if (!_planeReflectionSettings.IsReflectionSkyBox)
			{
				_reflectionCamera.clearFlags = CameraClearFlags.SolidColor;
				if ((bool)_volumeComponent)
				{
					_reflectionCamera.backgroundColor = _volumeComponent.RelfectionColor.value;
				}
			}
			else
			{
				_reflectionCamera.clearFlags = CameraClearFlags.Skybox;
			}
		}
	}

	private void UpdateShaderProperties()
	{
		if ((bool)_volumeComponent)
		{
			Shader.SetGlobalFloat(PlaneReflectionConsts.SHADER_PROPERTY_NAME_PIVOT_HEIGHT, _volumeComponent.ReflectionWorldY.value);
			Shader.SetGlobalFloat(PlaneReflectionConsts.SHADER_PROPERTY_NAME_OPACITY, _volumeComponent.ReflectionOpacity.value);
			Shader.SetGlobalColor(PlaneReflectionConsts.SHADER_PROPERTY_NAME_BACKGROUND_COLOR, _volumeComponent.RelfectionColor.value);
		}
		Shader.SetGlobalTexture(PlaneReflectionConsts.SHADER_PROPERTY_NAME_SAMPLER, _reflectionTexture);
		if (_isUsingReflection_prev != _isUsingReflection)
		{
			Shader.SetGlobalInt(PlaneReflectionConsts.SHADER_PROPERTY_NAME_ACTIVE, _isUsingReflection ? 1 : 0);
			_isUsingReflection_prev = _isUsingReflection;
		}
	}

	private static Vector3 ReflectPosition(Vector3 position)
	{
		return new Vector3(position.x, 0f - position.y, position.z);
	}

	public void AddPlaneReflectionObjectToList(PlaneReflectionObject planeReflectionObject)
	{
		if (!_planeReflectionObjects.Contains(planeReflectionObject))
		{
			_planeReflectionObjects.Add(planeReflectionObject);
		}
	}

	public void RemovePlaneReflectionObjectFromList(PlaneReflectionObject planeReflectionObject)
	{
		_planeReflectionObjects.Remove(planeReflectionObject);
	}

	private void PerformBoundsCheck()
	{
		if (!_isUsingReflection || _mainCamera == null || !_mainCamera.gameObject.activeInHierarchy || _reflectionCamera == null)
		{
			return;
		}
		bool visible = IsReflectionOptionEnabled() && CheckCameraBoundsOverlap(_mainCamera);
		bool wasActive = _reflectionCamera.gameObject.activeSelf;
		_reflectionCamera.gameObject.SetActive(visible);
		if (!visible)
		{
			_pendingActivation = false;
			Shader.SetGlobalInt(PlaneReflectionConsts.SHADER_PROPERTY_NAME_ACTIVE, 0);
			if (_reflectionTexture != null)
			{
				DeferredGpuRelease.ReleaseTemporaryRT(_reflectionTexture);
				_reflectionTexture = null;
			}
		}
		else if (!wasActive)
		{
			_pendingActivation = true;
		}
		else if (!_pendingActivation)
		{
			Shader.SetGlobalInt(PlaneReflectionConsts.SHADER_PROPERTY_NAME_ACTIVE, 1);
		}
	}

	private bool CheckCameraBoundsOverlap(Camera camera)
	{
		if (camera == null)
		{
			return false;
		}
		if (_planeReflectionObjects.Count == 0)
		{
			return false;
		}
		GeometryUtility.CalculateFrustumPlanes(camera, _cachedFrustumPlanes);
		for (int i = 0; i < _planeReflectionObjects.Count; i++)
		{
			PlaneReflectionObject obj = _planeReflectionObjects[i];
			if (obj == null)
			{
				continue;
			}
			if (obj.HasOverrideBounds)
			{
				List<Bounds> overrideBounds = obj.GetOverrideBoundsList();
				for (int j = 0; j < overrideBounds.Count; j++)
				{
					if (GeometryUtility.TestPlanesAABB(_cachedFrustumPlanes, overrideBounds[j]))
					{
						return true;
					}
				}
			}
			else if (GeometryUtility.TestPlanesAABB(_cachedFrustumPlanes, obj.GetBounds()))
			{
				return true;
			}
		}
		return false;
	}
}
