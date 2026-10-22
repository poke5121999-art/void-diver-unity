using System.Collections.Generic;
using GameCommon.Data;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

public class MinimapSightCore : MonoBehaviour
{
	private struct SightDrawUnit
	{
		public Vector2 WorldPos;

		public float Radius;
	}

	[SerializeField]
	private MiniMapSettings _miniMapSettings;

	[FormerlySerializedAs("_minimapSightSettings")]
	[SerializeField]
	private MinimapVisualSettings _minimapVisualSettings;

	[Header("Layer SDF")]
	[SerializeField]
	private ComputeShader _layerSdfCompute;

	[Header("Sight Mask (fog of war)")]
	[Tooltip("fog Unit RT 빌드용 ComputeShader. SO `EnableSightMaskSystem`이 true일 때만 사용")]
	[SerializeField]
	private ComputeShader _sightComputeShader;

	[SerializeField]
	[Range(0.05f, 1f)]
	private float _sightScaleFactor;

	[SerializeField]
	[Range(0.005f, 0.2f)]
	private float _sightUpdateInterval;

	public static readonly Subject<MinimapSightCore> OnInstantiated;

	private static readonly int _minimapSightEnableId;

	private static readonly int _minimapInStageId;

	private static readonly int _layerSdfTexId;

	private static readonly int _layerPaddedSizeId;

	private static readonly int _layerGridOriginId;

	private static readonly int _layerGridSizeId;

	private static readonly int _layerInnerSdfMinId;

	private static readonly int _layerInnerSdfMaxId;

	private static readonly int _layerOuterSdfMinId;

	private static readonly int _layerOuterSdfMaxId;

	private static readonly int _outerColorId;

	private static readonly int _outerSightMaskColorId;

	private static readonly int _passedAreaColorId;

	private static readonly int _outerNoiseTexId;

	private static readonly int _outerNoiseScaleId;

	private static readonly int _outerNoiseSpeedId;

	private static readonly int _outerNoiseStretchId;

	private static readonly int _outerDissolveOffsetId;

	private static readonly int _outerSmokeIntensityId;

	private static readonly int _minimapSightTexId;

	private Vector3[] _sectorOffsets;

	private int _mapColumns;

	private int _mapRows;

	private Vector3 _boundMin;

	private Vector3 _boundMax;

	private int _currentMapLayer;

	private FixedSector[] _fixedSectors;

	private bool _mapDataReady;

	private RenderTexture _layerMaskRT;

	private RenderTexture _layerSdfRT;

	private RenderTexture _layerJfaA;

	private RenderTexture _layerJfaB;

	private Texture2D _layerInputTex;

	private int _kernelScaleExpand;

	private int _kernelJfaSeed;

	private int _kernelJfaStep;

	private int _kernelJfaToSdf;

	private bool _layerSdfReady;

	private int _layerPaddingCells;

	private const int _maxPlayerNum = 3;

	private readonly List<MinimapSightUnit> _minimapSightUnits;

	private RenderTexture _maskRenderTexture;

	private ComputeBuffer _unitBuffer;

	private int _sightKernel;

	private bool _sightSystemReady;

	private float _sightNextUpdateTime;

	public static MinimapSightCore Instance { get; private set; }

	public float RatioWorldToTexture => 0f;

	public float RatioTextureToWorld => 0f;

	public static bool IsSightMaskEnabled => false;

	public static RenderTexture MaskRenderTexture => null;

	public static int MapColumns => 0;

	public static int MapRows => 0;

	public static int SectorCount => 0;

	public static bool IsMapReady => false;

	public FixedSector[] CurrentFixedSectors => null;

	public int CurrentMapLayer => 0;

	public static Vector3 GetSectorOffset(int idx)
	{
		return default;
	}

	public static void SetMinimapSightEnable(bool enable)
	{
	}

	public static void RequestRebuildLayer()
	{
	}

	public static bool RegisterSightUnit(MinimapSightUnit unit)
	{
		return false;
	}

	public static bool UnregisterSightUnit(MinimapSightUnit unit)
	{
		return false;
	}

	public void BindMapData(Vector3[] sectorOffsets, int columns, int rows, Vector3 boundMin, Vector3 boundMax)
	{
	}

	public void BindLayer(int mapLayer, FixedSector[] fixedSectors)
	{
	}

	public void BindRevealing(bool isInStage, bool isRevealing)
	{
	}

	private void Awake()
	{
	}

	private void OnDestroy()
	{
	}

	private void LateUpdate()
	{
	}

	private int ComputePaddingCells()
	{
		return 0;
	}

	private void InitializeLayerSdfCompute()
	{
	}

	private RenderTexture CreateLayerRT(int w, int h, RenderTextureFormat fmt, FilterMode filter)
	{
		return null;
	}

	private void ReleaseLayerSdfResources()
	{
	}

	private void DestroyLayerRT(ref RenderTexture rt)
	{
	}

	private void RebuildLayerSDF(int mapLayer)
	{
	}

	private void FillLayerMaskTexture(int mapLayer, FixedSector[] fixedSectors)
	{
	}

	private static FixedSector? FindFixedSector(FixedSector[] sectors, int indexX, int indexZ)
	{
		return null;
	}

	private static bool IsEmptyFixedSector(FixedSector? sector)
	{
		return false;
	}

	private void DispatchLayerSdfPipeline()
	{
	}

	private void PublishLayerSdfGlobals()
	{
	}

	private void PublishOuterGridUVGlobals()
	{
	}

	private void PublishInnerSdfGlobals()
	{
	}

	private void PublishOuterParamGlobals()
	{
	}

	private void InitializeSightRenderTexture()
	{
	}

	private void CollectActiveSightUnits()
	{
	}

	private void ReleaseSightRenderTexture()
	{
	}

	private void UpdateSightRenderTexture()
	{
	}

	[ContextMenu("Debug: Save Layer SDF Texture")]
	private void DebugSaveLayerSdf()
	{
	}

	[ContextMenu("Debug: Save Layer Input Mask")]
	private void DebugSaveLayerMask()
	{
	}
}
