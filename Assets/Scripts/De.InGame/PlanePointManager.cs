using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;

[DefaultExecutionOrder(100)]
[ExecuteAlways]
public class PlanePointManager : MonoBase
{
	public static PlanePointManager Instance;

	public static Subject<PlanePointManager> OnInstantiated = new Subject<PlanePointManager>();

	protected List<PlanePointHelper> PlanePoints = new List<PlanePointHelper>();

	protected List<PlanePointHelper> CurrentVisiblePoints = new List<PlanePointHelper>();

	protected List<PlanePointHelper> CurrentPlanePointBuffer = new List<PlanePointHelper>();

	protected List<PlanePointHelper> CurrentRimEffect = new List<PlanePointHelper>();

	[Inject]
	private CameraManager _cameraManager;

	private int _checkFrame;

	private Vector4[] _planePointColors = new Vector4[PlanePointConsts.NUMBER_PLANE_POINT_COUNT_MAX];

	private Vector4[] _planePointPositions = new Vector4[PlanePointConsts.NUMBER_PLANE_POINT_COUNT_MAX];

	private float[] _planePointRanges = new float[PlanePointConsts.NUMBER_PLANE_POINT_COUNT_MAX];

	private float[] _planePointIntensities = new float[PlanePointConsts.NUMBER_PLANE_POINT_COUNT_MAX];

	private Vector4[] _planePointBufferColors = new Vector4[PlanePointConsts.NUMBER_PLANE_POINT_COUNT_MAX];

	private Vector4[] _planePointBufferPositions = new Vector4[PlanePointConsts.NUMBER_PLANE_POINT_COUNT_MAX];

	private float[] _planePointBufferRanges = new float[PlanePointConsts.NUMBER_PLANE_POINT_COUNT_MAX];

	private float[] _planePointBufferIntensities = new float[PlanePointConsts.NUMBER_PLANE_POINT_COUNT_MAX];

	private Vector4[] _rimEffectColors = new Vector4[PlanePointConsts.NUMBER_PLANE_POINT_COUNT_MAX];

	private Vector4[] _rimEffectPositions = new Vector4[PlanePointConsts.NUMBER_PLANE_POINT_COUNT_MAX];

	private float[] _rimEffectRanges = new float[PlanePointConsts.NUMBER_PLANE_POINT_COUNT_MAX];

	private float[] _rimEffectIntensities = new float[PlanePointConsts.NUMBER_PLANE_POINT_COUNT_MAX];

	[Header("Plane Point Buffer Enable")]
	[FormerlySerializedAs("_lightBufferEnable")]
	[SerializeField]
	private bool _planePointBufferEnable = true;

	[Header("Plane Point Buffer Settings")]
	[FormerlySerializedAs("_lightComputeShader")]
	[SerializeField]
	private ComputeShader _planePointComputeShader;

	[SerializeField]
	private Vector3Int _renderTextureResolution = new Vector3Int(256, 1, 256);

	[Header("Debug")]
	[FormerlySerializedAs("_lightBuffer")]
	[SerializeField]
	private RenderTexture _planePointBuffer;

	private int _kernelIndex;

	private uint _kernelSizeX;

	private uint _kernelSizeY;

	private uint _kernelSizeZ;

	private int _groupsX;

	private int _groupsY;

	private bool _isBufferInitialized;

	private const float _frustumMargin = 5f;

	private readonly Plane[] _frustumPlanes = new Plane[6];

	private Camera CurrentTargetCamera => (_cameraManager == null) ? null : _cameraManager.MainCamera;

	protected override void Awake()
	{
		if (Application.isPlaying)
		{
			base.Awake();
		}
		PlanePoints.Clear();
		CurrentVisiblePoints.Clear();
		CurrentPlanePointBuffer.Clear();
		CurrentRimEffect.Clear();
	}

	private void OnEnable()
	{
		if (Instance != null && Instance != this)
		{
			if (!Application.isPlaying)
			{
				Object.DestroyImmediate(base.gameObject);
				return;
			}
			Object.Destroy(base.gameObject);
			return;
		}
		Instance = this;
		OnInstantiated?.OnNext(this);
		if (Application.isPlaying && !_isBufferInitialized)
		{
			InitializePlanePointBuffer();
			_isBufferInitialized = true;
		}
		PlaneSightCore.OnRevealingMapChanged += OnRevealingMapChanged;
		Shader.SetGlobalFloat(PlanePointConsts.SHADER_PROPERTY_PLANE_POINT_BUFFER_ENABLE, _planePointBufferEnable ? 1f : 0f);
	}

	private void OnValidate()
	{
		Shader.SetGlobalFloat(PlanePointConsts.SHADER_PROPERTY_PLANE_POINT_BUFFER_ENABLE, _planePointBufferEnable ? 1f : 0f);
	}

	private void OnDisable()
	{
		PlaneSightCore.OnRevealingMapChanged -= OnRevealingMapChanged;
		_isBufferInitialized = false;
	}

	private void OnDestroy()
	{
		PlaneSightCore.OnRevealingMapChanged -= OnRevealingMapChanged;
		CleanUpPlanePointBufferRenderTexture();
		_isBufferInitialized = false;
		if (Instance == this)
		{
			Instance = null;
		}
	}

	private void LateUpdate()
	{
		if (Application.isPlaying && _isBufferInitialized && _planePointBufferEnable && _planePointComputeShader != null && _planePointBuffer != null)
		{
			_planePointComputeShader.Dispatch(_kernelIndex, _groupsX, _groupsY, 1);
		}
		_checkFrame--;
		if (_checkFrame <= 0)
		{
			_checkFrame = PlanePointConsts.NUMBER_CAMERA_CHECK_INTERVAL;
			UpdateVisiblePoints();
		}
	}

	public void RegisterPoint(PlanePointHelper point)
	{
		if (!(point == null) && PlanePoints != null && !PlanePoints.Contains(point))
		{
			PlanePoints.Add(point);
		}
	}

	public void UnRegisterPoint(PlanePointHelper point)
	{
		if (!(point == null) && PlanePoints != null && PlanePoints.Contains(point))
		{
			PlanePoints.Remove(point);
		}
	}

	private void UpdateVisiblePoints()
	{
		if (CurrentTargetCamera == null)
		{
			return;
		}
		CurrentVisiblePoints.Clear();
		CurrentPlanePointBuffer.Clear();
		CurrentRimEffect.Clear();
		GeometryUtility.CalculateFrustumPlanes(CurrentTargetCamera, _frustumPlanes);
		foreach (PlanePointHelper planePoint in PlanePoints)
		{
			if (planePoint == null || !planePoint.gameObject.activeSelf || !planePoint.enabled)
			{
				continue;
			}
			Vector3 position = planePoint.transform.position;
			float num = planePoint.GetLightRange() + 5f;
			Bounds bounds = new Bounds(position, Vector3.one * num * 2f);
			if (GeometryUtility.TestPlanesAABB(_frustumPlanes, bounds))
			{
				CurrentVisiblePoints.Add(planePoint);
				if (planePoint.PlanePointBufferEnable)
				{
					CurrentPlanePointBuffer.Add(planePoint);
				}
				if (planePoint.RimEffectEnable)
				{
					CurrentRimEffect.Add(planePoint);
				}
			}
		}
		int count = Mathf.Min(CurrentVisiblePoints.Count, PlanePointConsts.NUMBER_PLANE_POINT_COUNT_MAX);
		for (int i = 0; i < count; i++)
		{
			PlanePointHelper pointi = CurrentVisiblePoints[i];
			if (!(pointi == null))
			{
				_planePointColors[i] = pointi.GetLightColor();
				Vector3 positioni = pointi.GetLightPosition();
				_planePointPositions[i] = new Vector4(positioni.x, positioni.y, positioni.z, 0f);
				_planePointRanges[i] = pointi.GetLightRange();
				_planePointIntensities[i] = pointi.GetLightInetnsity();
			}
		}
		Shader.SetGlobalInt(PlanePointConsts.SHADER_PROPERTY_PLANE_POINT_COUNT, count);
		Shader.SetGlobalVectorArray(PlanePointConsts.SHADER_PROPERTY_PLANE_POINT_COLORS, _planePointColors);
		Shader.SetGlobalVectorArray(PlanePointConsts.SHADER_PROPERTY_PLANE_POINT_POSITIONS, _planePointPositions);
		Shader.SetGlobalFloatArray(PlanePointConsts.SHADER_PROPERTY_PLANE_POINT_RANGES, _planePointRanges);
		Shader.SetGlobalFloatArray(PlanePointConsts.SHADER_PROPERTY_PLANE_POINT_INTENSITIES, _planePointIntensities);
		int bufferCount = Mathf.Min(CurrentPlanePointBuffer.Count, PlanePointConsts.NUMBER_PLANE_POINT_COUNT_MAX);
		for (int j = 0; j < bufferCount; j++)
		{
			PlanePointHelper pointj = CurrentPlanePointBuffer[j];
			if (!(pointj == null))
			{
				_planePointBufferColors[j] = pointj.GetLightColor();
				Vector3 positionj = pointj.GetLightPosition();
				_planePointBufferPositions[j] = new Vector4(positionj.x, positionj.y, positionj.z, 0f);
				_planePointBufferRanges[j] = pointj.GetLightRange();
				_planePointBufferIntensities[j] = pointj.GetLightInetnsity();
			}
		}
		Shader.SetGlobalInt(PlanePointConsts.SHADER_PROPERTY_PLANE_POINT_BUFFER_COUNT, bufferCount);
		Shader.SetGlobalVectorArray(PlanePointConsts.SHADER_PROPERTY_PLANE_POINT_BUFFER_COLORS, _planePointBufferColors);
		Shader.SetGlobalVectorArray(PlanePointConsts.SHADER_PROPERTY_PLANE_POINT_BUFFER_POSITIONS, _planePointBufferPositions);
		Shader.SetGlobalFloatArray(PlanePointConsts.SHADER_PROPERTY_PLANE_POINT_BUFFER_RANGES, _planePointBufferRanges);
		Shader.SetGlobalFloatArray(PlanePointConsts.SHADER_PROPERTY_PLANE_POINT_BUFFER_INTENSITIES, _planePointBufferIntensities);
		int rimCount = Mathf.Min(CurrentRimEffect.Count, PlanePointConsts.NUMBER_PLANE_POINT_COUNT_MAX);
		for (int k = 0; k < rimCount; k++)
		{
			PlanePointHelper pointk = CurrentRimEffect[k];
			if (!(pointk == null))
			{
				_rimEffectColors[k] = pointk.GetLightColor();
				Vector3 positionk = pointk.GetLightPosition();
				_rimEffectPositions[k] = new Vector4(positionk.x, positionk.y, positionk.z, 0f);
				_rimEffectRanges[k] = pointk.GetLightRange();
				_rimEffectIntensities[k] = pointk.GetLightInetnsity();
			}
		}
		Shader.SetGlobalInt(PlanePointConsts.SHADER_PROPERTY_RIM_EFFECT_COUNT, rimCount);
		Shader.SetGlobalVectorArray(PlanePointConsts.SHADER_PROPERTY_RIM_EFFECT_COLORS, _rimEffectColors);
		Shader.SetGlobalVectorArray(PlanePointConsts.SHADER_PROPERTY_RIM_EFFECT_POSITIONS, _rimEffectPositions);
		Shader.SetGlobalFloatArray(PlanePointConsts.SHADER_PROPERTY_RIM_EFFECT_RANGES, _rimEffectRanges);
		Shader.SetGlobalFloatArray(PlanePointConsts.SHADER_PROPERTY_RIM_EFFECT_INTENSITIES, _rimEffectIntensities);
	}

	private void OnRevealingMapChanged(bool isRevealing)
	{
		_planePointBufferEnable = !isRevealing;
		Shader.SetGlobalFloat(PlanePointConsts.SHADER_PROPERTY_PLANE_POINT_BUFFER_ENABLE, _planePointBufferEnable ? 1f : 0f);
	}

	private void InitializePlanePointBuffer()
	{
		if (!SystemInfo.supportsComputeShaders || !Application.isPlaying)
		{
			return;
		}
		CleanUpPlanePointBufferRenderTexture();
		_planePointBuffer = CreatePlanePointBufferTexture("_PlanePointBuffer");
		if (_planePointComputeShader != null && _planePointBuffer != null)
		{
			_kernelIndex = _planePointComputeShader.FindKernel("CSMain");
			_planePointComputeShader.GetKernelThreadGroupSizes(_kernelIndex, out _kernelSizeX, out _kernelSizeY, out _kernelSizeZ);
			_planePointComputeShader.SetTexture(_kernelIndex, "_View", _planePointBuffer);
			_groupsX = Mathf.CeilToInt((float)_renderTextureResolution.x / (float)_kernelSizeX);
			_groupsY = Mathf.CeilToInt((float)_renderTextureResolution.z / (float)_kernelSizeY);
			Shader.SetGlobalTexture("_PlanePointBuffer", _planePointBuffer);
		}
		else
		{
			Debug.LogError("Failed to initialize plane point buffer - ComputeShader or RenderTexture is null");
		}
	}

	private RenderTexture CreatePlanePointBufferTexture(string textureName)
	{
		RenderTexture renderTexture = new RenderTexture(_renderTextureResolution.x, _renderTextureResolution.z, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
		renderTexture.name = textureName;
		renderTexture.enableRandomWrite = true;
		if (!renderTexture.Create())
		{
			Debug.LogError("Failed to create RenderTexture: " + textureName);
			return null;
		}
		return renderTexture;
	}

	private void CleanUpPlanePointBufferRenderTexture()
	{
		if (!Application.isPlaying || _planePointBuffer == null)
		{
			return;
		}
		if (_planePointBuffer.IsCreated())
		{
			_planePointBuffer.Release();
		}
		if (Application.isPlaying)
		{
			Object.Destroy(_planePointBuffer);
		}
		else
		{
			Object.DestroyImmediate(_planePointBuffer);
		}
		_planePointBuffer = null;
	}
}
