using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using UniRx;
using Unity.Collections;
using Unity.Jobs;
using GameCommon.Enum;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;

[DefaultExecutionOrder(110)]
public class PlaneSightCore : MonoBase
{
	public struct SightStruct
	{
		public Vector3 WorldPos;

		public Vector3 BoundsPos;

		public float FrontRange;

		public float BackRange;

		public float Alpha;

		public float Toggle;
	}

	public struct RayStruct
	{
		public Vector4 HitPoint;

		public Vector4 HitPoint1;

		public Vector4 HitPoint2;

		public int HitPointCount;
	}

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private CameraManager _cameraManager;

	private CameraPostProcessingController _cameraPostProcessingController;

	private CompositeDisposable _disposable;

	private CharacterModel _characterModel;

	public const float NUMBER_DIFFERENCE_DISTANCE = 0.1f;

	private const int _rayBatchCount = 16;

	private const int _rayNum = 64;

	private const int _maxPlayerNum = 3;

	private const float _rayRatio = 0.75f;

	private const int _raycastCountStep = 4;

	private const float _diffDistanceThreshold = 0.1f;

	private const float _diffNormalDotThreshold = 0.1f;

	private const float _fixedRangeFactor = 12f;

	public static PlaneSightCore Instance;

	public static Subject<PlaneSightCore> OnInstantiated = new Subject<PlaneSightCore>();

	[CompilerGenerated]
	private static Action<bool> m_OnRevealingMapChanged;

	private bool _isUpateFrame = true;

	[Header("Plane Sight Setting: Reveal Map")]
	[SerializeField]
	private bool _isRevealingMap;

	[Header("Plane Sight Setting: Culling")]
	[PlaneSightTargetCameraLayer]
	[SerializeField]
	private int _cullingLayer;

	[Header("Plane Sight Setting: Collision")]
	[SerializeField]
	[FormerlySerializedAs("wallLayerMask")]
	private LayerMask _wallLayerMask = 10;

	private int _rayFrontNum;

	private int _rayBackNum;

	private bool _isInitialzied;

	[Header("Plane Sight Setting: Scriptable Data")]
	[SerializeField]
	private PlaneSightDesignScriptableData _planeSightDesignScriptableData;

	[SerializeField]
	private PlaneSightVisualScriptableData _planeSightVisualScriptableData;

	[Header("Plane Sight Setting: Debug")]
	[SerializeField]
	private bool _isDrawingGizmo;

	[Header("Plane Sight Setting: Render Texture")]
	[SerializeField]
	[FormerlySerializedAs("computeShader")]
	private ComputeShader _computeShader;

	private RenderTextureFormat _renderTextureFormat = RenderTextureFormat.Default;

	[Header("Plane Sight Debug!")]
	[SerializeField]
	private RenderTexture _viewRenderTarget;

	private int _kernel;

	private uint _kernelSizeX;

	private uint _kernelSizeY;

	private uint _kernelSizeZ;

	private NativeArray<RaycastCommand> _raycastCommands;

	private NativeArray<RaycastHit> _raycastHits;

	private NativeArray<float> _rayDists;

	private NativeArray<bool> _raycastActives;

	private ComputeBuffer _characterBuffer;

	private int _characterCount;

	private int _allocatedCharacterCount = -1;

	private Vector4[] _sightUnitPositions;

	private float[] _sightUnitCloseRanges;

	private float[] _sightUnitFarRanges;

	private float[] _sightUnitAlphas;

	private Dictionary<int, List<PlaneSightUnit>> _layerSightUnits = new Dictionary<int, List<PlaneSightUnit>>();

	private List<PlaneSightUnit> _activeSightUnits;

	private Dictionary<CharacterModel, PlaneSightUnit> _cachedPlaneSightUnit = new Dictionary<CharacterModel, PlaneSightUnit>();

	private PlaneSightUnit _mainPlaneSightUnit;

	private List<IPlaneSightDetector> _planeSightDetectors = new List<IPlaneSightDetector>();

	private SightStruct[] _sightStructs;

	private ComputeBuffer _rayBuffer;

	private NativeArray<RayStruct> _rayStructs;

	private float _currentIntensity;

	private float _targetIntensity;

	public bool IsRevealingMap => _isRevealingMap;

	public LayerMask WallLayerMask => _wallLayerMask;

	public PlaneSightDesignScriptableData DesignScriptableData => _planeSightDesignScriptableData;

	public PlaneSightVisualScriptableData VisualScriptableData => _planeSightVisualScriptableData;

	public RenderTexture ViewRenderTarget => _viewRenderTarget;

	public int CurrentCharacterCount => Mathf.Min(_characterCount, 3);

	public List<IPlaneSightDetector> PlaneSightDetectors => _planeSightDetectors;

	public int CullingLayer => _cullingLayer;

	public static event Action<bool> OnRevealingMapChanged
	{
		[CompilerGenerated]
		add
		{
			Action<bool> action = m_OnRevealingMapChanged;
			Action<bool> action2;
			do
			{
				action2 = action;
				Action<bool> value2 = (Action<bool>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref m_OnRevealingMapChanged, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<bool> action = m_OnRevealingMapChanged;
			Action<bool> action2;
			do
			{
				action2 = action;
				Action<bool> value2 = (Action<bool>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref m_OnRevealingMapChanged, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	protected override void Awake()
	{
		base.Awake();
		if (Instance != null)
		{
			Debug.LogWarning("LC ART : Warning Multiple Plane Sight Cores");
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		Instance = this;
		OnInstantiated.OnNext(this);
		InitializeBrightnessProperties();
		InitailizePlaneSightVisual();
		InitializeRaycast();
		InitializeCameraPostProcessingController();
	}

	private void Start()
	{
		InitializeTexturePassInfo();
		EnablePlaneSight(!_isRevealingMap);
		_isInitialzied = true;
		if (!(_gameManager != null))
		{
			_isUpateFrame = true;
			return;
		}
		_gameManager.RxIsRevealingMap.Subscribe(SetIsRevealingMap).AddTo(this);
		_gameManager.RxViewTargetClientId.Subscribe(OnViewTargetChanged).AddTo(this);
		_gameManager.RxStageState.Where((EStageState s) => s == EStageState.Intro || s == EStageState.Playing).Take(1).Subscribe(delegate
		{
			OnStageLoadCompleted();
		})
			.AddTo(this);
		_gameManager.RxStageState.Where((EStageState s) => s == EStageState.Closed).Subscribe(delegate
		{
			OnStageClosed();
		})
			.AddTo(this);
	}

	private void OnValidate()
	{
		EnablePlaneSight(!_isRevealingMap);
	}

	private void OnDisable()
	{

	}

	private void OnDestroy()
	{
		CleanUpRaycastJob();
		Shader.SetGlobalFloat(PlaneSightConsts.SHADER_PROPERTY_ENABLE, 0f);
		CleanUpBuffers();
		CleanUpPlaneSightRenderTexture();
		_disposable?.Dispose();
		m_OnRevealingMapChanged = null;
		Instance = null;
	}

	public void EnablePlaneSight(bool isEnable)
	{
		Shader.SetGlobalFloat(PlaneSightConsts.SHADER_PROPERTY_ENABLE, isEnable ? 1f : 0f);
	}

	public CharacterModel FindCharacterModel(ulong clientId)
	{
		if (!(_gameManager != null))
		{
			return null;
		}
		return _gameManager.FindCharacterModel(clientId);
	}

	public void EnablePlaneSight(float weight)
	{
		Shader.SetGlobalFloat(PlaneSightConsts.SHADER_PROPERTY_ENABLE, weight);
	}

	public void ChangeCullingLayer(int layer)
	{
		_cullingLayer = layer;
	}

	private void ProcessSightFrame()
	{
		if (!_isInitialzied || _isRevealingMap || !_isUpateFrame)
		{
			return;
		}
		if (_layerSightUnits.ContainsKey(_cullingLayer))
		{
			_activeSightUnits = _layerSightUnits[_cullingLayer];
			_characterCount = _activeSightUnits.Count;
			if (_characterCount != 0)
			{
				_isUpateFrame = false;
				ProcessRaycast();
			}
		}
	}

	private void LateUpdate()
	{
		UpdateVignetteIntensity();
		ProcessSightFrame();
	}

	public void SetIsRevealingMap(bool targetValue)
	{
		_isRevealingMap = targetValue;
		EnablePlaneSight(!targetValue);
		m_OnRevealingMapChanged?.Invoke(_isRevealingMap);
	}

	private void InitializeTexturePassInfo()
	{
		if (SystemInfo.supportsComputeShaders)
		{
			CleanUpPlaneSightRenderTexture();
			_viewRenderTarget = CreateRenderTexture("PlaneSightRenderTexture");
			if (!(_viewRenderTarget == null))
			{
				Shader.SetGlobalTexture(PlaneSightConsts.SHADER_PROPERTY_RENDER_TARGET, _viewRenderTarget);
				_kernel = _computeShader.FindKernel("CSMain");
				_computeShader.GetKernelThreadGroupSizes(_kernel, out _kernelSizeX, out _kernelSizeY, out _kernelSizeZ);
				_computeShader.SetTexture(_kernel, "_View", _viewRenderTarget);
			}
		}
	}

	private void InitializeBrightnessProperties()
	{
		_sightUnitPositions = new Vector4[3];
		_sightUnitCloseRanges = new float[3];
		_sightUnitFarRanges = new float[3];
		_sightUnitAlphas = new float[3];
	}

	private void InitializeRaycast()
	{
		_raycastCommands = new NativeArray<RaycastCommand>(64, Allocator.Persistent);
		_raycastHits = new NativeArray<RaycastHit>(64, Allocator.Persistent);
		_rayDists = new NativeArray<float>(64, Allocator.Persistent);
		_raycastActives = new NativeArray<bool>(64, Allocator.Persistent);
		_rayStructs = new NativeArray<RayStruct>(192, Allocator.Persistent);
		_rayFrontNum = 48;
		_rayBackNum = 16;
	}

	private void InitailizePlaneSightVisual()
	{
		if (!(bool)_planeSightVisualScriptableData)
		{
			Shader.SetGlobalFloat(PlaneSightConsts.SHADER_PROPERTY_SIGHT_SHARPNESS_POWER, 1f);
			Shader.SetGlobalFloat(PlaneSightConsts.SHADER_PROPERTY_SIGHT_SHARPNESS_BOUNDARY, 0f);
			Shader.SetGlobalFloat(PlaneSightConsts.SHADER_PROPERTY_SIGHT_INTERSECTION_SMOOTHNESS, 0f);
			Shader.SetGlobalColor(PlaneSightConsts.SHADER_PROPERTY_SIGHT_COLOR_INNER, Color.white);
			Shader.SetGlobalColor(PlaneSightConsts.SHADER_PROPERTY_SIGHT_COLOR_BOUNDARY, Color.white);
			Shader.SetGlobalFloat(PlaneSightConsts.SHADER_PROPERTY_SIGHT_COLOR_LERP, 0.25f);
			Shader.SetGlobalFloat(PlaneSightConsts.SHADER_PROPERTY_SIGHT_COLOR_WIDTH, 0.25f);
			Shader.SetGlobalFloat(PlaneSightConsts.SHADER_PROPERTY_SIGHT_SILLHOUETTE_DISTANCE, 10f);
			Shader.SetGlobalFloat(PlaneSightConsts.SHADER_PROPERTY_SEE_THROUGH_BOUNDARY, 0f);
			Shader.SetGlobalFloat(PlaneSightConsts.SHADER_PROPERTY_SEE_THROUGH_POWER, 1f);
			Shader.SetGlobalFloat(PlaneSightConsts.SHADER_PROPERTY_SEE_THROUGH_HEIGHT, -0.2f);
			Shader.SetGlobalFloat(PlaneSightConsts.SHADER_PROPERTY_SEE_THROUGH_INTERSECTION_SMOOTHNESS, 0f);
			Shader.SetGlobalFloat(PlaneSightConsts.SHADER_PROPERTY_SEE_THROUGH_DISTANCE_MAX, 8f);
			Shader.SetGlobalFloat(PlaneSightConsts.SHADER_PROPERTY_SEE_THROUGH_DISTANCE_MIN, 13f);
		}
		else
		{
			Shader.SetGlobalFloat(PlaneSightConsts.SHADER_PROPERTY_SIGHT_SHARPNESS_POWER, _planeSightVisualScriptableData.SightSharpnessPower);
			Shader.SetGlobalFloat(PlaneSightConsts.SHADER_PROPERTY_SIGHT_SHARPNESS_BOUNDARY, _planeSightVisualScriptableData.SightSharpnessBoundary);
			Shader.SetGlobalFloat(PlaneSightConsts.SHADER_PROPERTY_SIGHT_INTERSECTION_SMOOTHNESS, _planeSightVisualScriptableData.SightIntersectionSmoothness);
			Shader.SetGlobalColor(PlaneSightConsts.SHADER_PROPERTY_SIGHT_COLOR_INNER, _planeSightVisualScriptableData.SightInnerColor);
			Shader.SetGlobalColor(PlaneSightConsts.SHADER_PROPERTY_SIGHT_COLOR_BOUNDARY, _planeSightVisualScriptableData.SightBoundaryColor);
			Shader.SetGlobalFloat(PlaneSightConsts.SHADER_PROPERTY_SIGHT_COLOR_LERP, _planeSightVisualScriptableData.SightColorLerp);
			Shader.SetGlobalFloat(PlaneSightConsts.SHADER_PROPERTY_SIGHT_COLOR_WIDTH, _planeSightVisualScriptableData.SightColorWidth);
			Shader.SetGlobalFloat(PlaneSightConsts.SHADER_PROPERTY_SIGHT_SILLHOUETTE_DISTANCE, 10f);
			Shader.SetGlobalFloat(PlaneSightConsts.SHADER_PROPERTY_SEE_THROUGH_BOUNDARY, _planeSightVisualScriptableData.SeeThroughBoundary);
			Shader.SetGlobalFloat(PlaneSightConsts.SHADER_PROPERTY_SEE_THROUGH_POWER, _planeSightVisualScriptableData.SeeThroughPower);
			Shader.SetGlobalFloat(PlaneSightConsts.SHADER_PROPERTY_SEE_THROUGH_HEIGHT, _planeSightVisualScriptableData.SeeThroughHeight);
			Shader.SetGlobalFloat(PlaneSightConsts.SHADER_PROPERTY_SEE_THROUGH_INTERSECTION_SMOOTHNESS, _planeSightVisualScriptableData.SeeThroughIntersectionSmoothness);
			Shader.SetGlobalFloat(PlaneSightConsts.SHADER_PROPERTY_SEE_THROUGH_DISTANCE_MAX, _planeSightVisualScriptableData.SeeThroughDistanceMax);
			Shader.SetGlobalFloat(PlaneSightConsts.SHADER_PROPERTY_SEE_THROUGH_DISTANCE_MIN, _planeSightVisualScriptableData.SeeThroughDistanceMin);
		}
	}

	private void InitializeCameraPostProcessingController()
	{
		_cameraPostProcessingController = ((_cameraManager == null) ? null : _cameraManager.GetComponent<CameraPostProcessingController>());
	}

	private void RenewComuteBuffer()
	{
		int num = Mathf.Min(_characterCount, 3);
		if (_rayBuffer == null)
		{
			_rayBuffer = new ComputeBuffer(192, Marshal.SizeOf(typeof(RayStruct)));
		}
		if (_allocatedCharacterCount != num)
		{
			_characterBuffer?.Dispose();
			_characterBuffer = new ComputeBuffer(num, Marshal.SizeOf(typeof(SightStruct)));
			_sightStructs = new SightStruct[num];
			_allocatedCharacterCount = num;
		}
	}

	private RenderTexture CreateRenderTexture(string rtName)
	{
		RenderTexture renderTexture = new RenderTexture(PlaneCameraBoundConsts.RESOLUTION_RENDER_TEXTURE.x, PlaneCameraBoundConsts.RESOLUTION_RENDER_TEXTURE.z, 0, _renderTextureFormat, RenderTextureReadWrite.Linear);
		renderTexture.name = string.Format(rtName, Array.Empty<object>());
		renderTexture.enableRandomWrite = true;
		if (!renderTexture.Create())
		{
			renderTexture.Release();
			return null;
		}
		return renderTexture;
	}

	private void CleanUpRaycastJob()
	{
		if (_raycastCommands.IsCreated)
		{
			_raycastCommands.Dispose();
		}
		if (_raycastHits.IsCreated)
		{
			_raycastHits.Dispose();
		}
		if (_rayDists.IsCreated)
		{
			_rayDists.Dispose();
		}
		if (_raycastActives.IsCreated)
		{
			_raycastActives.Dispose();
		}
		if (_rayStructs.IsCreated)
		{
			_rayStructs.Dispose();
		}
		Shader.SetGlobalFloat(PlaneSightConsts.SHADER_PROPERTY_ENABLE, 0f);
	}

	private void CleanUpBuffers()
	{
		_characterBuffer?.Dispose();
		_characterBuffer = null;
		_rayBuffer?.Dispose();
		_rayBuffer = null;
		_allocatedCharacterCount = -1;
	}

	private void CleanUpPlaneSightRenderTexture()
	{
		if (_viewRenderTarget != null)
		{
			if (_viewRenderTarget.IsCreated())
			{
				_viewRenderTarget.Release();
			}
			if (!Application.isPlaying)
			{
				UnityEngine.Object.DestroyImmediate(_viewRenderTarget);
			}
			else
			{
				UnityEngine.Object.Destroy(_viewRenderTarget);
			}
			_viewRenderTarget = null;
		}
	}

	private bool IsValidComputeBuffer()
	{
		if (_computeShader != null && _sightStructs != null)
		{
			return _rayBuffer != null;
		}
		return false;
	}

	private void UpdateSightInfo()
	{
		if (!(_computeShader != null) || _sightStructs == null || _rayBuffer == null)
		{
			return;
		}
		if (!_layerSightUnits.ContainsKey(_cullingLayer))
		{
			return;
		}
		_computeShader.SetInt(PlaneSightConsts.SHADER_PROPERTY_SIGHT_COUNT, Mathf.Min(_characterCount, 3));
		_computeShader.SetInt(PlaneSightConsts.SHADER_PROPERTY_RAY_DETECTION_COUNT, 64);
		_computeShader.SetInt(PlaneSightConsts.SHADER_PROPERTY_RAY_DETECTION_FRONT_COUNT, _rayFrontNum);
		for (int i = 0; i < Mathf.Min(_characterCount, 3); i++)
		{
			if ((bool)_activeSightUnits[i].SightInfo.SightTransform)
			{
				_sightStructs[i].WorldPos = _activeSightUnits[i].SightInfo.SightTransform.position + _activeSightUnits[i].PivotOffset;
				_sightUnitPositions[i] = _activeSightUnits[i].SightInfo.SightTransform.position + _activeSightUnits[i].PivotOffset;
			}
			_sightStructs[i].BoundsPos = PlaneCameraBoundCore.NormalizePositionUsingBoundBox(in _sightStructs[i].WorldPos);
			_sightStructs[i].Alpha = _activeSightUnits[i].SightAlpha;
			_sightStructs[i].FrontRange = _activeSightUnits[i].SightRange;
			_sightStructs[i].BackRange = _activeSightUnits[i].SightBackRange;
			_sightStructs[i].Toggle = _activeSightUnits[i].SightToggleFactor;
			_sightUnitCloseRanges[i] = _activeSightUnits[i].SightBackRange;
			_sightUnitFarRanges[i] = _activeSightUnits[i].SightRange;
			_sightUnitAlphas[i] = _activeSightUnits[i].SightAlpha;
		}
		_characterBuffer?.SetData(_sightStructs);
		_computeShader.SetBuffer(_kernel, PlaneSightConsts.SHADER_PROPERTY_RAY_BUFFER, _characterBuffer);
		Shader.SetGlobalFloat(PlaneSightConsts.SHADER_PROPERTY_SIGHT_COUNT, Mathf.Min(_characterCount, 3));
		Shader.SetGlobalVectorArray(PlaneSightConsts.SHADER_PROPERTY_SIGHT_POSITIONS, _sightUnitPositions);
		Shader.SetGlobalFloatArray(PlaneSightConsts.SHADER_PROPERTY_SIGHT_BACK_RANGES, _sightUnitCloseRanges);
		Shader.SetGlobalFloatArray(PlaneSightConsts.SHADER_PROPERTY_SIGHT_FRONT_RANGES, _sightUnitFarRanges);
		Shader.SetGlobalFloatArray(PlaneSightConsts.SHADER_PROPERTY_SIGHT_ALPHAS, _sightUnitAlphas);
	}

	private void ProcessRaycast()
	{
		if (_viewRenderTarget == null || _kernelSizeX == 0)
		{
			return;
		}
		if (!_layerSightUnits.ContainsKey(_cullingLayer))
		{
			return;
		}
		for (int i = 0; i < _characterCount; i++)
		{
			if (_activeSightUnits[i] == null || _activeSightUnits[i].SightInfo == null || !(bool)_activeSightUnits[i].SightInfo.SightTransform)
			{
				continue;
			}
			Vector3 easedWorldDirection = _activeSightUnits[i].EasedWorldDirection;
			Vector3 normalized = new Vector3(easedWorldDirection.x, 0f, easedWorldDirection.z).normalized;
			float viewDirectionAngle = Vector3.SignedAngle(Vector3.forward, normalized, Vector3.up);
			float sightAngle = _activeSightUnits[i].SightAngle;
			float sightBackRange = _activeSightUnits[i].SightBackRange;
			Vector3 pivotPos = _activeSightUnits[i].SightInfo.SightTransform.position + _activeSightUnits[i].PivotOffset;
			float deltaFrontAngle = sightAngle / (float)(_rayFrontNum - 2);
			float deltaBackAngle = (360f - sightAngle) / (float)(_rayBackNum - 2);
			float sightRoundness = ((bool)_planeSightVisualScriptableData) ? _planeSightVisualScriptableData.SightRoundness : 1f;
			PlaneSightRaycastJob raycastJob = new PlaneSightRaycastJob
			{
				TryCount = 1,
				TryMaxCount = 1,
				LongDist = 12f,
				BackDist = sightBackRange,
				ViewAngle = sightAngle,
				ViewDirectionAngle = viewDirectionAngle,
				DeltaFrontAngle = deltaFrontAngle,
				DeltaBackAngle = deltaBackAngle,
				DivisionRatio = 0f,
				SightRoundness = sightRoundness,
				PivotPos = pivotPos,
				LayerMask = _wallLayerMask,
				RaycastCommands = _raycastCommands,
				RayDistances = _rayDists,
				RaycastActives = _raycastActives
			};
			JobHandle dependsOn = raycastJob.Schedule(64, 16);
			dependsOn = RaycastCommand.ScheduleBatch(_raycastCommands, _raycastHits, 64, dependsOn);
			dependsOn.Complete();
			sightRoundness = ((bool)_planeSightVisualScriptableData) ? _planeSightVisualScriptableData.SightRoundness : 1f;
			PlaneSightRaycastConditionJob conditionJob = new PlaneSightRaycastConditionJob
			{
				TryCount = 1,
				TryMaxCount = 1,
				RayNum = 64,
				CharacterIndex = i,
				DiffDistanceThreshold = 0.1f,
				DiffNormalDotThreshold = 0.1f,
				IsDrawOnGizmo = _isDrawingGizmo,
				PivotPos = pivotPos,
				ViewAngle = sightAngle,
				ViewDirectionAngle = viewDirectionAngle,
				DeltaFrontAngle = deltaFrontAngle,
				DeltaBackAngle = deltaBackAngle,
				DivisionRatio = 0f,
				SightRoundness = sightRoundness,
				RaycastHits = _raycastHits,
				RayDistances = _rayDists,
				RayStructs = _rayStructs,
				RaycastActives = _raycastActives
			};
			conditionJob.Schedule(64, 16).Complete();
			for (int tryCount = 2; tryCount < 5; tryCount++)
			{
				float divisionRatio = Mathf.Max(0f, (float)tryCount - 1f) * 0.25f;
				sightRoundness = ((bool)_planeSightVisualScriptableData) ? _planeSightVisualScriptableData.SightRoundness : 1f;
				raycastJob = new PlaneSightRaycastJob
				{
					TryCount = tryCount,
					TryMaxCount = 4,
					LongDist = 12f,
					BackDist = sightBackRange,
					ViewAngle = sightAngle,
					ViewDirectionAngle = viewDirectionAngle,
					DeltaFrontAngle = deltaFrontAngle,
					DeltaBackAngle = deltaBackAngle,
					DivisionRatio = divisionRatio,
					SightRoundness = sightRoundness,
					PivotPos = pivotPos,
					LayerMask = _wallLayerMask,
					RaycastCommands = _raycastCommands,
					RayDistances = _rayDists,
					RaycastActives = _raycastActives
				};
				dependsOn = raycastJob.Schedule(64, 16);
				dependsOn = RaycastCommand.ScheduleBatch(_raycastCommands, _raycastHits, 64, dependsOn);
				dependsOn.Complete();
				sightRoundness = ((bool)_planeSightVisualScriptableData) ? _planeSightVisualScriptableData.SightRoundness : 1f;
				conditionJob = new PlaneSightRaycastConditionJob
				{
					TryCount = tryCount,
					TryMaxCount = 4,
					RayNum = 64,
					CharacterIndex = i,
					DiffDistanceThreshold = 0.1f,
					DiffNormalDotThreshold = 0.1f,
					IsDrawOnGizmo = _isDrawingGizmo,
					PivotPos = pivotPos,
					ViewAngle = sightAngle,
					ViewDirectionAngle = viewDirectionAngle,
					DeltaFrontAngle = deltaFrontAngle,
					DeltaBackAngle = deltaBackAngle,
					DivisionRatio = divisionRatio,
					SightRoundness = sightRoundness,
					RaycastHits = _raycastHits,
					RayDistances = _rayDists,
					RayStructs = _rayStructs,
					RaycastActives = _raycastActives
				};
				conditionJob.Schedule(64, 16).Complete();
			}
		}
		RenewComuteBuffer();
		UpdateSightInfo();
		if (_rayBuffer != null)
		{
			_rayBuffer.SetData(_rayStructs);
			_computeShader.SetBuffer(_kernel, "_RayBuffer", _rayBuffer);
			_computeShader.Dispatch(_kernel, PlaneCameraBoundConsts.RESOLUTION_RENDER_TEXTURE.x / (int)_kernelSizeX, PlaneCameraBoundConsts.RESOLUTION_RENDER_TEXTURE.z / (int)_kernelSizeY, (int)_kernelSizeZ);
			_isUpateFrame = true;
		}
	}

	public bool RegisterPlaneSightUnit(in PlaneSightUnit planeSightUnit)
	{
		if (planeSightUnit == null)
		{
			return false;
		}
		if (!_layerSightUnits.ContainsKey(planeSightUnit.gameObject.layer))
		{
			_layerSightUnits[planeSightUnit.gameObject.layer] = new List<PlaneSightUnit>();
		}
		if (_layerSightUnits[planeSightUnit.gameObject.layer].Count > 2)
		{
			return false;
		}
		if (_layerSightUnits[planeSightUnit.gameObject.layer].Contains(planeSightUnit))
		{
			return false;
		}
		_layerSightUnits[planeSightUnit.gameObject.layer].Add(planeSightUnit);
		return true;
	}

	public bool UnRegisterPlaneSightUnit(in PlaneSightUnit planeSightUnit)
	{
		if (planeSightUnit == null)
		{
			return false;
		}
		if (!_layerSightUnits.ContainsKey(planeSightUnit.gameObject.layer))
		{
			return false;
		}
		if (!_layerSightUnits[planeSightUnit.gameObject.layer].Contains(planeSightUnit))
		{
			return false;
		}
		_layerSightUnits[planeSightUnit.gameObject.layer].Remove(planeSightUnit);
		return true;
	}

	public bool RegisterDetector(IPlaneSightDetector planeSightDetector)
	{
		if (planeSightDetector == null)
		{
			return false;
		}
		if (_planeSightDetectors.Contains(planeSightDetector))
		{
			return false;
		}
		_planeSightDetectors.Add(planeSightDetector);
		return true;
	}

	public bool UnRegisterDetector(IPlaneSightDetector planeSightDetector)
	{
		if (planeSightDetector == null)
		{
			return false;
		}
		if (!_planeSightDetectors.Contains(planeSightDetector))
		{
			return false;
		}
		_planeSightDetectors.Remove(planeSightDetector);
		return true;
	}

	public static void GetRaycastAngleAndDistance(in int index, in int tryCount, in int tryMaxCount, in float viewAngle, in float viewDirectionAngle, in float longDistance, in float shortDistance, in float deltaFrontAngle, in float deltaBackAngle, in float divisionRatio, in float sightRoundness, out float outAngle, out float outDistance, out bool isFront)
	{
		if (index == 0)
		{
			outAngle = viewDirectionAngle;
			outDistance = 0f;
			isFront = true;
			float num = (float)Mathf.FloorToInt(viewDirectionAngle / deltaFrontAngle) * deltaFrontAngle - outAngle;
			num = Mathf.Repeat(num, 360f);
			outAngle = num * divisionRatio + outAngle;
		}
		else if (index == Instance._rayFrontNum - 1)
		{
			outAngle = viewAngle + viewDirectionAngle;
			outDistance = 0f;
			isFront = true;
			outAngle = Mathf.Repeat(outAngle - viewAngle * 0.5f, 360f);
			return;
		}
		else if (index == Instance._rayFrontNum)
		{
			outAngle = viewAngle + viewDirectionAngle;
			outDistance = shortDistance;
			isFront = false;
			float num2 = (float)Mathf.CeilToInt(viewDirectionAngle / deltaBackAngle) * deltaBackAngle + viewAngle - outAngle;
			num2 = Mathf.Repeat(num2, 360f);
			outAngle = num2 * divisionRatio + outAngle;
		}
		else if (index == Instance._rayFrontNum - 1 + Instance._rayBackNum)
		{
			outAngle = viewDirectionAngle;
			outDistance = shortDistance;
			isFront = false;
		}
		else if (index < Instance._rayFrontNum)
		{
			outAngle = (float)Mathf.CeilToInt(((float)(index - 1) * deltaFrontAngle + viewDirectionAngle) / deltaFrontAngle) * deltaFrontAngle;
			outDistance = longDistance;
			isFront = true;
			if (index != Instance._rayFrontNum - 1)
			{
				if (index != Instance._rayFrontNum - 2)
				{
					outAngle = deltaFrontAngle * divisionRatio + outAngle;
				}
				else
				{
					float num3 = Mathf.Repeat(viewAngle + viewDirectionAngle - outAngle, 360f);
					outAngle = num3 * divisionRatio + outAngle;
				}
			}
		}
		else
		{
			outAngle = (float)Mathf.CeilToInt(((float)(index - Instance._rayFrontNum - 1) * deltaBackAngle + viewDirectionAngle) / deltaBackAngle) * deltaBackAngle + viewAngle;
			outDistance = shortDistance;
			isFront = false;
			if (index != Instance._rayBackNum * 2 - 1)
			{
				if (index != Instance._rayFrontNum - 2 + Instance._rayBackNum)
				{
					outAngle = deltaBackAngle * divisionRatio + outAngle;
				}
				else
				{
					float num4 = Mathf.Repeat(viewDirectionAngle - outAngle, 360f);
					outAngle = num4 * divisionRatio + outAngle;
				}
			}
		}
		outAngle = Mathf.Repeat(outAngle - viewAngle * 0.5f, 360f);
	}

	public bool GetIsDiffHitPoints(in float dist0, in RaycastHit hit0, in float dist1, in RaycastHit hit1, in float inDiffNormalDotThreshold, in float inDiffDistanceThreshold, in bool isDrawOnGizmo = false)
	{
		if (0.1f < Mathf.Abs(dist0 - dist1))
		{
			return false;
		}
		if (hit0.distance > Mathf.Epsilon && hit1.distance > Mathf.Epsilon)
		{
			if (isDrawOnGizmo)
			{
				Debug.DrawLine(hit0.point, hit0.point + hit0.normal, Color.black);
				Debug.DrawLine(hit1.point, hit1.point + hit1.normal, Color.black);
			}
			if (Vector3.Dot(hit0.normal, hit1.normal) < inDiffNormalDotThreshold)
			{
				return true;
			}
			float num = Mathf.Abs(hit0.distance - hit1.distance);
			return num > inDiffDistanceThreshold;
		}
		return hit1.distance > Mathf.Epsilon != hit0.distance > Mathf.Epsilon;
	}

	private void OnViewTargetChanged(ulong clientId)
	{
		_disposable?.Dispose();
		_characterModel = _gameManager.FindCharacterModel(clientId);
		if (!(_characterModel != null))
		{
			return;
		}
		_disposable = new CompositeDisposable();
		_characterModel.RxIsLightOn.Subscribe(OnLightChanged).AddTo(_disposable);
		if (!_cachedPlaneSightUnit.ContainsKey(_characterModel) || !(_cachedPlaneSightUnit[_characterModel] != null))
		{
			_mainPlaneSightUnit = _characterModel.GetComponentInChildren<PlaneSightUnit>();
			_cachedPlaneSightUnit[_characterModel] = _mainPlaneSightUnit;
		}
		else
		{
			_mainPlaneSightUnit = _cachedPlaneSightUnit[_characterModel];
		}
		_targetIntensity = (_characterModel.IsLightOn ? 1f : 0f);
	}

	private void OnStageLoadCompleted()
	{
		if ((object)_gameManager != null)
		{
			_currentIntensity = (_gameManager.IsInStage ? 0f : 1f);
		}
	}

	private void OnStageClosed()
	{
		_targetIntensity = 1f;
		_currentIntensity = 1f;
	}

	private void OnLightChanged(bool toggle)
	{
		_targetIntensity = (toggle ? 1f : 0f);
	}

	private void UpdateVignetteIntensity()
	{
		float num = _isRevealingMap ? 1f : 0f;
		float num2 = 1f;
		if (_mainPlaneSightUnit != null && _mainPlaneSightUnit.SightInfo != null)
		{
			num2 = (_mainPlaneSightUnit.SightInfo.IsBlinded ? 0f : 1f);
		}
		float num3 = num2 * _targetIntensity;
		if (num3 <= num)
		{
			num3 = num;
		}
		num3 = Mathf.Clamp01(num3);
		float num4 = (num3 <= _currentIntensity) ? _planeSightDesignScriptableData.SightVignetteNegativeDelta : _planeSightDesignScriptableData.SightVignetteDelta;
		if (0.0001f <= Mathf.Abs(_currentIntensity - num3))
		{
			num3 = Mathf.Lerp(_currentIntensity, num3, Time.deltaTime * num4);
		}
		_currentIntensity = num3;
		if ((bool)_cameraPostProcessingController)
		{
			_cameraPostProcessingController.SightEnableIntensity = Mathf.Clamp01(_currentIntensity);
		}
	}
}
