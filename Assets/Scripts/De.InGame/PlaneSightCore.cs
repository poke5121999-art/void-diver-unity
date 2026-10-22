using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UniRx;
using Unity.Collections;
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

	public static Subject<PlaneSightCore> OnInstantiated;

	[CompilerGenerated]
	private static Action<bool> m_OnRevealingMapChanged;

	private bool _isUpateFrame;

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
	private LayerMask _wallLayerMask;

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

	private RenderTextureFormat _renderTextureFormat;

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

	private int _allocatedCharacterCount;

	private Vector4[] _sightUnitPositions;

	private float[] _sightUnitCloseRanges;

	private float[] _sightUnitFarRanges;

	private float[] _sightUnitAlphas;

	private Dictionary<int, List<PlaneSightUnit>> _layerSightUnits;

	private List<PlaneSightUnit> _activeSightUnits;

	private Dictionary<CharacterModel, PlaneSightUnit> _cachedPlaneSightUnit;

	private PlaneSightUnit _mainPlaneSightUnit;

	private List<IPlaneSightDetector> _planeSightDetectors;

	private SightStruct[] _sightStructs;

	private ComputeBuffer _rayBuffer;

	private NativeArray<RayStruct> _rayStructs;

	private float _currentIntensity;

	private float _targetIntensity;

	public bool IsRevealingMap => false;

	public LayerMask WallLayerMask => default;

	public PlaneSightDesignScriptableData DesignScriptableData => null;

	public PlaneSightVisualScriptableData VisualScriptableData => null;

	public RenderTexture ViewRenderTarget => null;

	public int CurrentCharacterCount => 0;

	public List<IPlaneSightDetector> PlaneSightDetectors => null;

	public int CullingLayer => 0;

	public static event Action<bool> OnRevealingMapChanged
	{
		[CompilerGenerated]
		add
		{
		}
		[CompilerGenerated]
		remove
		{
		}
	}

	protected override void Awake()
	{
	}

	private void Start()
	{
	}

	private void OnValidate()
	{
	}

	private void OnDisable()
	{
	}

	private void OnDestroy()
	{
	}

	public void EnablePlaneSight(bool isEnable)
	{
	}

	public CharacterModel FindCharacterModel(ulong clientId)
	{
		return null;
	}

	public void EnablePlaneSight(float weight)
	{
	}

	public void ChangeCullingLayer(int layer)
	{
	}

	private void ProcessSightFrame()
	{
	}

	private void LateUpdate()
	{
	}

	public void SetIsRevealingMap(bool targetValue)
	{
	}

	private void InitializeTexturePassInfo()
	{
	}

	private void InitializeBrightnessProperties()
	{
	}

	private void InitializeRaycast()
	{
	}

	private void InitailizePlaneSightVisual()
	{
	}

	private void InitializeCameraPostProcessingController()
	{
	}

	private void RenewComuteBuffer()
	{
	}

	private RenderTexture CreateRenderTexture(string rtName)
	{
		return null;
	}

	private void CleanUpRaycastJob()
	{
	}

	private void CleanUpBuffers()
	{
	}

	private void CleanUpPlaneSightRenderTexture()
	{
	}

	private bool IsValidComputeBuffer()
	{
		return false;
	}

	private void UpdateSightInfo()
	{
	}

	private void ProcessRaycast()
	{
	}

	public bool RegisterPlaneSightUnit(in PlaneSightUnit planeSightUnit)
	{
		return false;
	}

	public bool UnRegisterPlaneSightUnit(in PlaneSightUnit planeSightUnit)
	{
		return false;
	}

	public bool RegisterDetector(IPlaneSightDetector planeSightDetector)
	{
		return false;
	}

	public bool UnRegisterDetector(IPlaneSightDetector planeSightDetector)
	{
		return false;
	}

	public static void GetRaycastAngleAndDistance(in int index, in int tryCount, in int tryMaxCount, in float viewAngle, in float viewDirectionAngle, in float longDistance, in float shortDistance, in float deltaFrontAngle, in float deltaBackAngle, in float divisionRatio, in float sightRoundness, out float outAngle, out float outDistance, out bool isFront)
	{
		outAngle = default;
		outDistance = default;
		isFront = default;
	}

	public bool GetIsDiffHitPoints(in float dist0, in RaycastHit hit0, in float dist1, in RaycastHit hit1, in float inDiffNormalDotThreshold, in float inDiffDistanceThreshold, in bool isDrawOnGizmo = false)
	{
		return false;
	}

	private void OnViewTargetChanged(ulong clientId)
	{
	}

	private void OnStageLoadCompleted()
	{
	}

	private void OnStageClosed()
	{
	}

	private void OnLightChanged(bool toggle)
	{
	}

	private void UpdateVignetteIntensity()
	{
	}
}
