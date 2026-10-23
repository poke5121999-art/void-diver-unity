using System;
using System.Collections.Generic;
using System.IO;
using GameCommon.Data;
using UniRx;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
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
	private float _sightScaleFactor = 0.25f;

	[SerializeField]
	[Range(0.005f, 0.2f)]
	private float _sightUpdateInterval = 0.025f;

	public static readonly Subject<MinimapSightCore> OnInstantiated = new Subject<MinimapSightCore>();

	private static readonly int _minimapSightEnableId = Shader.PropertyToID("_MinimapSightEnable");

	private static readonly int _minimapInStageId = Shader.PropertyToID("_GlobalMinimapInStage");

	private static readonly int _layerSdfTexId = Shader.PropertyToID("_GlobalLayerSDFTex");

	private static readonly int _layerPaddedSizeId = Shader.PropertyToID("_GlobalLayerPaddedSize");

	private static readonly int _layerGridOriginId = Shader.PropertyToID("_GlobalLayerGridOrigin");

	private static readonly int _layerGridSizeId = Shader.PropertyToID("_GlobalLayerGridSize");

	private static readonly int _layerInnerSdfMinId = Shader.PropertyToID("_GlobalLayerInnerSDFMin");

	private static readonly int _layerInnerSdfMaxId = Shader.PropertyToID("_GlobalLayerInnerSDFMax");

	private static readonly int _layerOuterSdfMinId = Shader.PropertyToID("_GlobalLayerOuterSDFMin");

	private static readonly int _layerOuterSdfMaxId = Shader.PropertyToID("_GlobalLayerOuterSDFMax");

	private static readonly int _outerColorId = Shader.PropertyToID("_GlobalMinimapOuterColor");

	private static readonly int _outerSightMaskColorId = Shader.PropertyToID("_GlobalMinimapSightMaskColor");

	private static readonly int _passedAreaColorId = Shader.PropertyToID("_GlobalMinimapPassedAreaColor");

	private static readonly int _outerNoiseTexId = Shader.PropertyToID("_GlobalMinimapOuterNoiseTex");

	private static readonly int _outerNoiseScaleId = Shader.PropertyToID("_GlobalMinimapOuterNoiseScale");

	private static readonly int _outerNoiseSpeedId = Shader.PropertyToID("_GlobalMinimapOuterNoiseSpeed");

	private static readonly int _outerNoiseStretchId = Shader.PropertyToID("_GlobalMinimapOuterNoiseStretch");

	private static readonly int _outerDissolveOffsetId = Shader.PropertyToID("_GlobalMinimapOuterDissolveOffset");

	private static readonly int _outerSmokeIntensityId = Shader.PropertyToID("_GlobalMinimapOuterSmokeIntensity");

	private static readonly int _minimapSightTexId = Shader.PropertyToID("_MinimapSightTexture");

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

	private int _kernelScaleExpand = -1;

	private int _kernelJfaSeed = -1;

	private int _kernelJfaStep = -1;

	private int _kernelJfaToSdf = -1;

	private bool _layerSdfReady;

	private int _layerPaddingCells;

	private const int _maxPlayerNum = 3;

	private readonly List<MinimapSightUnit> _minimapSightUnits = new List<MinimapSightUnit>();

	private RenderTexture _maskRenderTexture;

	private ComputeBuffer _unitBuffer;

	private int _sightKernel = -1;

	private bool _sightSystemReady;

	private float _sightNextUpdateTime;

	public static MinimapSightCore Instance { get; private set; }

	public float RatioWorldToTexture
	{
		get
		{
			if (!_miniMapSettings)
			{
				return 1f;
			}
			return _miniMapSettings.GetTextureSize() / 30f;
		}
	}

	public float RatioTextureToWorld => 1f / RatioWorldToTexture;

	public static bool IsSightMaskEnabled
	{
		get
		{
			if (Instance == null)
			{
				return false;
			}
			if (Instance._minimapVisualSettings == null)
			{
				return false;
			}
			return Instance._minimapVisualSettings.EnableSightMaskSystem;
		}
	}

	public static RenderTexture MaskRenderTexture
	{
		get
		{
			if (Instance == null)
			{
				return null;
			}
			return Instance._maskRenderTexture;
		}
	}

	public static int MapColumns
	{
		get
		{
			if (Instance == null)
			{
				return 0;
			}
			return Instance._mapColumns;
		}
	}

	public static int MapRows
	{
		get
		{
			if (Instance == null)
			{
				return 0;
			}
			return Instance._mapRows;
		}
	}

	public static int SectorCount
	{
		get
		{
			if (Instance == null)
			{
				return 0;
			}
			if (Instance._sectorOffsets == null)
			{
				return 0;
			}
			return Instance._sectorOffsets.Length;
		}
	}

	public static bool IsMapReady
	{
		get
		{
			if (Instance == null)
			{
				return false;
			}
			return Instance._mapDataReady;
		}
	}

	public FixedSector[] CurrentFixedSectors => _fixedSectors;

	public int CurrentMapLayer => _currentMapLayer;

	public static Vector3 GetSectorOffset(int idx)
	{
		if (!(Instance == null) && Instance._sectorOffsets != null && idx >= 0 && idx < Instance._sectorOffsets.Length)
		{
			return Instance._sectorOffsets[idx];
		}
		return Vector3.zero;
	}

	public static void SetMinimapSightEnable(bool enable)
	{
		Shader.SetGlobalFloat(_minimapSightEnableId, enable ? 1f : 0f);
	}

	public static void RequestRebuildLayer()
	{
		if (!(Instance == null))
		{
			Instance.RebuildLayerSDF(Instance._currentMapLayer);
		}
	}

	public static bool RegisterSightUnit(MinimapSightUnit unit)
	{
		if (!IsSightMaskEnabled)
		{
			return false;
		}
		if (Instance == null)
		{
			return false;
		}
		if (unit == null)
		{
			return false;
		}
		if (Instance._minimapSightUnits.Count >= 3)
		{
			return false;
		}
		if (Instance._minimapSightUnits.Contains(unit))
		{
			return false;
		}
		Instance._minimapSightUnits.Add(unit);
		return true;
	}

	public static bool UnregisterSightUnit(MinimapSightUnit unit)
	{
		if (Instance == null)
		{
			return false;
		}
		if (unit == null)
		{
			return false;
		}
		return Instance._minimapSightUnits.Remove(unit);
	}

	public void BindMapData(Vector3[] sectorOffsets, int columns, int rows, Vector3 boundMin, Vector3 boundMax)
	{
		_sectorOffsets = sectorOffsets;
		_mapColumns = columns;
		_mapRows = rows;
		_boundMin = boundMin;
		_boundMax = boundMax;
		_mapDataReady = sectorOffsets != null && sectorOffsets.Length != 0;
		InitializeLayerSdfCompute();
		PublishOuterParamGlobals();
		PublishInnerSdfGlobals();
		InitializeSightRenderTexture();
	}

	public void BindLayer(int mapLayer, FixedSector[] fixedSectors)
	{
		_currentMapLayer = mapLayer;
		_fixedSectors = fixedSectors;
		RebuildLayerSDF(mapLayer);
	}

	public void BindRevealing(bool isInStage, bool isRevealing)
	{
		Shader.SetGlobalFloat(_minimapInStageId, isInStage ? 1f : 0f);
		if (IsSightMaskEnabled)
		{
			SetMinimapSightEnable(!isRevealing && isInStage);
		}
		else
		{
			Shader.SetGlobalFloat(_minimapSightEnableId, 0f);
		}
	}

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Debug.LogWarning("LC ART : Warning Multiple MinimapSightCore \u2014 destroying duplicate");
			UnityEngine.Object.Destroy(this);
			return;
		}
		Instance = this;
		OnInstantiated.OnNext(this);
		PublishOuterParamGlobals();
		PublishInnerSdfGlobals();
	}

	private void OnDestroy()
	{
		ReleaseLayerSdfResources();
		ReleaseSightRenderTexture();
		_minimapSightUnits.Clear();
		if (Instance == this)
		{
			Instance = null;
		}
	}

	private void LateUpdate()
	{
		UpdateSightRenderTexture();
	}

	private int ComputePaddingCells()
	{
		if (_minimapVisualSettings == null || _miniMapSettings == null || !_mapDataReady)
		{
			return 0;
		}
		if (_minimapVisualSettings.LayerInputPaddingOverride >= 0)
		{
			return _minimapVisualSettings.LayerInputPaddingOverride;
		}
		float x = _miniMapSettings.GetResolution().x;
		if (x <= 0f)
		{
			return 0;
		}
		int num = Mathf.RoundToInt((float)_mapColumns * _minimapVisualSettings.OuterSizeUnit / x);
		if (num < 0)
		{
			num = 0;
		}
		return num;
	}

	private void InitializeLayerSdfCompute()
	{
		if (!SystemInfo.supportsComputeShaders || _layerSdfCompute == null || !_mapDataReady)
		{
			return;
		}
		_kernelScaleExpand = _layerSdfCompute.FindKernel("CSScaleExpand");
		_kernelJfaSeed = _layerSdfCompute.FindKernel("CSJFASeed");
		_kernelJfaStep = _layerSdfCompute.FindKernel("CSJFAStep");
		_kernelJfaToSdf = _layerSdfCompute.FindKernel("CSJFAToSDF");
		int num = ComputePaddingCells();
		_layerPaddingCells = num;
		int num2 = _mapColumns + num * 2;
		int num3 = _mapRows + num * 2;
		int num4 = ((_minimapVisualSettings != null) ? _minimapVisualSettings.LayerSdfScale : 4);
		int num5 = 1;
		if (num4 > 0)
		{
			num5 = num4;
		}
		int h = num5 * num3;
		int w = num5 * num2;
		ReleaseLayerSdfResources();
		_layerInputTex = new Texture2D(num2, num3, TextureFormat.RGBA32, mipChain: false, linear: true);
		_layerInputTex.filterMode = FilterMode.Point;
		_layerInputTex.wrapMode = TextureWrapMode.Clamp;
		_layerMaskRT = CreateLayerRT(w, h, RenderTextureFormat.ARGB32, FilterMode.Point);
		_layerSdfRT = CreateLayerRT(w, h, RenderTextureFormat.ARGB32, FilterMode.Bilinear);
		_layerJfaA = CreateLayerRT(w, h, RenderTextureFormat.RGFloat, FilterMode.Point);
		_layerJfaB = CreateLayerRT(w, h, RenderTextureFormat.RGFloat, FilterMode.Point);
		if (_layerMaskRT == null || _layerSdfRT == null || _layerJfaA == null || _layerJfaB == null)
		{
			ReleaseLayerSdfResources();
		}
		else
		{
			_layerSdfReady = true;
		}
	}

	private RenderTexture CreateLayerRT(int w, int h, RenderTextureFormat fmt, FilterMode filter)
	{
		RenderTexture renderTexture = new RenderTexture(w, h, 0, fmt, RenderTextureReadWrite.Linear);
		renderTexture.enableRandomWrite = true;
		renderTexture.filterMode = filter;
		renderTexture.wrapMode = TextureWrapMode.Clamp;
		if (!renderTexture.Create())
		{
			renderTexture.Release();
			return null;
		}
		return renderTexture;
	}

	private void ReleaseLayerSdfResources()
	{
		_layerSdfReady = false;
		DestroyLayerRT(ref _layerMaskRT);
		DestroyLayerRT(ref _layerSdfRT);
		DestroyLayerRT(ref _layerJfaA);
		DestroyLayerRT(ref _layerJfaB);
		if (_layerInputTex != null)
		{
			UnityEngine.Object.Destroy(_layerInputTex);
			_layerInputTex = null;
		}
	}

	private void DestroyLayerRT(ref RenderTexture rt)
	{
		if (!(rt == null))
		{
			rt.Release();
			UnityEngine.Object.Destroy(rt);
			rt = null;
		}
	}

	private void RebuildLayerSDF(int mapLayer)
	{
		if (_layerSdfReady && !(_layerSdfCompute == null) && _mapDataReady)
		{
			FillLayerMaskTexture(mapLayer, _fixedSectors);
			DispatchLayerSdfPipeline();
			PublishLayerSdfGlobals();
		}
	}

	private void FillLayerMaskTexture(int mapLayer, FixedSector[] fixedSectors)
	{
		int width = _layerInputTex.width;
		int height = _layerInputTex.height;
		int layerPaddingCells = _layerPaddingCells;
		int num = width - layerPaddingCells * 2;
		int num2 = height - layerPaddingCells * 2;
		Color32[] array = new Color32[width * height];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = new Color32(0, 0, 0, byte.MaxValue);
		}
		bool flag = fixedSectors != null && fixedSectors.Length != 0;
		for (int j = 0; j < num2; j++)
		{
			for (int k = 0; k < num; k++)
			{
				Color32 color = new Color32(byte.MaxValue, 0, 0, byte.MaxValue);
				if (flag)
				{
					FixedSector? sector = FindFixedSector(fixedSectors, k, j);
					if (IsEmptyFixedSector(sector) || sector.Value.Layer != mapLayer)
					{
						color = new Color32(0, 0, 0, byte.MaxValue);
					}
				}
				array[(layerPaddingCells + j) * width + layerPaddingCells + k] = color;
			}
		}
		_layerInputTex.SetPixels32(array);
		_layerInputTex.Apply(updateMipmaps: false);
	}

	private static FixedSector? FindFixedSector(FixedSector[] sectors, int indexX, int indexZ)
	{
		if (sectors != null)
		{
			for (int i = 0; i < sectors.Length; i++)
			{
				if (sectors[i].IndexX == indexX && sectors[i].IndexZ == indexZ)
				{
					return sectors[i];
				}
			}
		}
		return null;
	}

	private static bool IsEmptyFixedSector(FixedSector? sector)
	{
		if (!sector.HasValue)
		{
			return true;
		}
		return sector.Value.Layer < 0;
	}

	private void DispatchLayerSdfPipeline()
	{
		ComputeShader layerSdfCompute = _layerSdfCompute;
		int width = _layerInputTex.width;
		int height = _layerInputTex.height;
		int width2 = _layerMaskRT.width;
		int height2 = _layerMaskRT.height;
		int threadGroupsX = (int)Math.Ceiling((float)width2 * 0.125f);
		int threadGroupsY = (int)Math.Ceiling((float)height2 * 0.125f);
		float value = ((_minimapVisualSettings != null) ? _minimapVisualSettings.LayerSdfSpread : 8f);
		layerSdfCompute.SetInts("_InputSize", width, height);
		layerSdfCompute.SetInts("_OutputSize", width2, height2);
		layerSdfCompute.SetInt("_Scale", width2 / width);
		layerSdfCompute.SetInt("_SourceChannel", 0);
		layerSdfCompute.SetTexture(_kernelScaleExpand, "_BinaryInput", _layerInputTex);
		layerSdfCompute.SetTexture(_kernelScaleExpand, "_MaskOutput", _layerMaskRT);
		layerSdfCompute.Dispatch(_kernelScaleExpand, threadGroupsX, threadGroupsY, 1);
		layerSdfCompute.SetTexture(_kernelJfaSeed, "_MaskInput", _layerMaskRT);
		layerSdfCompute.SetTexture(_kernelJfaSeed, "_JFABuffer", _layerJfaA);
		layerSdfCompute.Dispatch(_kernelJfaSeed, threadGroupsX, threadGroupsY, 1);
		bool flag = true;
		for (int num = Mathf.Max(width2, height2) / 2; num >= 1; num >>= 1)
		{
			RenderTexture texture = (flag ? _layerJfaA : _layerJfaB);
			RenderTexture texture2 = (flag ? _layerJfaB : _layerJfaA);
			layerSdfCompute.SetTexture(_kernelJfaStep, "_JFABufferRead", texture);
			layerSdfCompute.SetTexture(_kernelJfaStep, "_JFABuffer", texture2);
			layerSdfCompute.SetInt("_JFAStep", num);
			layerSdfCompute.SetInts("_OutputSize", width2, height2);
			layerSdfCompute.Dispatch(_kernelJfaStep, threadGroupsX, threadGroupsY, 1);
			flag = !flag;
		}
		RenderTexture texture3 = (flag ? _layerJfaA : _layerJfaB);
		layerSdfCompute.SetInt("_OutputChannel", 0);
		layerSdfCompute.SetFloat("_SDFSpread", value);
		layerSdfCompute.SetTexture(_kernelJfaToSdf, "_BinaryInput", _layerInputTex);
		layerSdfCompute.SetTexture(_kernelJfaToSdf, "_JFABufferRead", texture3);
		layerSdfCompute.SetTexture(_kernelJfaToSdf, "_MaskInput", _layerMaskRT);
		layerSdfCompute.SetTexture(_kernelJfaToSdf, "_SDFOutput", _layerSdfRT);
		layerSdfCompute.SetInts("_OutputSize", width2, height2);
		layerSdfCompute.Dispatch(_kernelJfaToSdf, threadGroupsX, threadGroupsY, 1);
	}

	private void PublishLayerSdfGlobals()
	{
		if (!(_layerSdfRT == null) && !(_layerInputTex == null))
		{
			int width = _layerInputTex.width;
			int height = _layerInputTex.height;
			Shader.SetGlobalTexture(_layerSdfTexId, _layerSdfRT);
			Shader.SetGlobalVector(_layerPaddedSizeId, new Vector4(width, height, _layerPaddingCells, 0f));
			PublishOuterGridUVGlobals();
		}
	}

	private void PublishOuterGridUVGlobals()
	{
		if (_miniMapSettings == null || _minimapVisualSettings == null || !_mapDataReady)
		{
			return;
		}
		Vector2 resolution = _miniMapSettings.GetResolution();
		if (resolution.x > 0f && _mapColumns > 0)
		{
			float num = (float)_mapColumns * resolution.x;
			float num2 = (float)_mapRows * resolution.y;
			Vector2 one = Vector2.one;
			float num3 = one.x * _minimapVisualSettings.OuterSizeUnit * (float)_mapColumns;
			float num4 = one.y * _minimapVisualSettings.OuterSizeUnit * (float)_mapColumns;
			float num5 = _layerPaddingCells * 2;
			float num6 = num3 + num3 + num;
			float num7 = num4 + num4 + num2;
			float num8 = resolution.x * num5 + num;
			float num9 = resolution.y * num5 + num2;
			if (num6 > 0f && num7 > 0f && num8 > 0f)
			{
				Shader.SetGlobalVector(_layerGridOriginId, new Vector4((num6 - num8) * 0.5f / num6, (num7 - num9) * 0.5f / num7, 0f, 0f));
				Shader.SetGlobalVector(_layerGridSizeId, new Vector4(num8 / num6, num9 / num7, 0f, 0f));
			}
		}
	}

	private void PublishInnerSdfGlobals()
	{
		if (!(_minimapVisualSettings == null))
		{
			Shader.SetGlobalFloat(_layerInnerSdfMinId, _minimapVisualSettings.InnerSDFMin);
			Shader.SetGlobalFloat(_layerInnerSdfMaxId, _minimapVisualSettings.InnerSDFMax);
		}
	}

	private void PublishOuterParamGlobals()
	{
		MinimapVisualSettings minimapVisualSettings = _minimapVisualSettings;
		if (!(minimapVisualSettings == null))
		{
			Shader.SetGlobalColor(_outerColorId, minimapVisualSettings.OuterColor);
			Shader.SetGlobalColor(_outerSightMaskColorId, minimapVisualSettings.SightMaskColor);
			Shader.SetGlobalColor(_passedAreaColorId, minimapVisualSettings.PassedAreaColor);
			if (minimapVisualSettings.OuterNoiseTex != null)
			{
				Shader.SetGlobalTexture(_outerNoiseTexId, minimapVisualSettings.OuterNoiseTex);
			}
			Shader.SetGlobalFloat(_outerNoiseScaleId, minimapVisualSettings.OuterNoiseScale);
			Shader.SetGlobalFloat(_outerNoiseSpeedId, minimapVisualSettings.OuterNoiseSpeed);
			Shader.SetGlobalVector(_outerNoiseStretchId, new Vector4(minimapVisualSettings.OuterNoiseStretch.x, minimapVisualSettings.OuterNoiseStretch.y, 0f, 0f));
			Shader.SetGlobalFloat(_outerDissolveOffsetId, minimapVisualSettings.OuterDissolveOffset);
			Shader.SetGlobalFloat(_outerSmokeIntensityId, minimapVisualSettings.OuterSmokeIntensity);
			Shader.SetGlobalFloat(_layerOuterSdfMinId, minimapVisualSettings.OuterSDFMin);
			Shader.SetGlobalFloat(_layerOuterSdfMaxId, minimapVisualSettings.OuterSDFMax);
		}
	}

	private void InitializeSightRenderTexture()
	{
		ReleaseSightRenderTexture();
		if (!SystemInfo.supportsComputeShaders || !IsSightMaskEnabled || _sightComputeShader == null || !_mapDataReady)
		{
			return;
		}
		float num = (_boundMax.x - _boundMin.x) * RatioWorldToTexture * _sightScaleFactor;
		float num2 = (_boundMax.z - _boundMin.z) * RatioWorldToTexture * _sightScaleFactor;
		if (!(num < 1f) && !(num2 < 1f))
		{
			_maskRenderTexture = new RenderTexture((int)num, (int)num2, 0, (GraphicsFormat)6);
			_maskRenderTexture.enableRandomWrite = true;
			if (!_maskRenderTexture.Create())
			{
				_maskRenderTexture.Release();
				_maskRenderTexture = null;
				return;
			}
			Shader.SetGlobalTexture(_minimapSightTexId, _maskRenderTexture);
			_sightKernel = _sightComputeShader.FindKernel("SightUnits");
			_sightSystemReady = true;
			CollectActiveSightUnits();
		}
	}

	private void CollectActiveSightUnits()
	{
		_minimapSightUnits.Clear();
		MinimapSightUnit[] array = UnityEngine.Object.FindObjectsByType<MinimapSightUnit>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
		for (int i = 0; i < array.Length && _minimapSightUnits.Count < 3; i++)
		{
			if (array[i] != null && !_minimapSightUnits.Contains(array[i]))
			{
				_minimapSightUnits.Add(array[i]);
			}
		}
	}

	private void ReleaseSightRenderTexture()
	{
		_sightSystemReady = false;
		if (_unitBuffer != null)
		{
			_unitBuffer.Dispose();
			_unitBuffer = null;
		}
		if (_maskRenderTexture != null)
		{
			if (_maskRenderTexture.IsCreated())
			{
				_maskRenderTexture.Release();
			}
			UnityEngine.Object.Destroy(_maskRenderTexture);
			_maskRenderTexture = null;
			Shader.SetGlobalTexture(_minimapSightTexId, Texture2D.blackTexture);
		}
	}

	private void UpdateSightRenderTexture()
	{
		if (!_sightSystemReady || _minimapVisualSettings == null || !_minimapVisualSettings.EnableSightSystemInStage || _minimapSightUnits.Count == 0 || Time.time < _sightNextUpdateTime)
		{
			return;
		}
		_sightNextUpdateTime = Time.time + _sightUpdateInterval;
		SightDrawUnit[] array = new SightDrawUnit[_minimapSightUnits.Count];
		float unitRadius = _minimapVisualSettings.UnitRadius;
		float ratioWorldToTexture = RatioWorldToTexture;
		float sightScaleFactor = _sightScaleFactor;
		for (int i = 0; i < _minimapSightUnits.Count; i++)
		{
			MinimapSightUnit minimapSightUnit = _minimapSightUnits[i];
			if (!(minimapSightUnit == null))
			{
				Vector3 position = minimapSightUnit.transform.position;
				array[i].WorldPos = new Vector2(RatioWorldToTexture * _sightScaleFactor * (position.x - _boundMin.x), RatioWorldToTexture * _sightScaleFactor * (minimapSightUnit.transform.position.z - _boundMin.z));
				array[i].Radius = ratioWorldToTexture * unitRadius * sightScaleFactor;
			}
		}
		if (_unitBuffer != null)
		{
			_unitBuffer.Dispose();
		}
		_unitBuffer = new ComputeBuffer(array.Length, 12);
		_unitBuffer.SetData(array);
		_sightComputeShader.SetInt("unitCount", array.Length);
		_sightComputeShader.SetFloat("_RatioWorldToTexture", RatioWorldToTexture);
		_sightComputeShader.SetBuffer(_sightKernel, "units", _unitBuffer);
		_sightComputeShader.SetTexture(_sightKernel, "result", _maskRenderTexture);
		_sightComputeShader.Dispatch(_sightKernel, (int)Math.Ceiling((float)_maskRenderTexture.width * 0.125f), (int)Math.Ceiling((float)_maskRenderTexture.height * 0.125f), 1);
		_unitBuffer.Dispose();
		_unitBuffer = null;
	}

	[ContextMenu("Debug: Save Layer SDF Texture")]
	private void DebugSaveLayerSdf()
	{
		if (_layerSdfRT == null)
		{
			Debug.LogWarning("[Minimap] Layer SDF RT\uac00 null. Play \uc911\uc5d0 \ud638\ucd9c\ud558\uac70\ub098 Stage \uc9c4\uc785 \ud6c4 \uc2dc\ub3c4");
			return;
		}
		RenderTexture active = RenderTexture.active;
		RenderTexture.active = _layerSdfRT;
		Texture2D texture2D = new Texture2D(_layerSdfRT.width, _layerSdfRT.height, TextureFormat.RGBA32, mipChain: false);
		texture2D.ReadPixels(new Rect(0f, 0f, _layerSdfRT.width, _layerSdfRT.height), 0, 0);
		texture2D.Apply();
		RenderTexture.active = active;
		byte[] bytes = texture2D.EncodeToPNG();
		string fullPath = Path.GetFullPath(Path.Combine(Application.dataPath, "..", "Temp", "MinimapLayerSDF.png"));
		Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
		File.WriteAllBytes(fullPath, bytes);
		UnityEngine.Object.Destroy(texture2D);
		Debug.Log("[Minimap] Layer SDF saved: " + fullPath);
	}

	[ContextMenu("Debug: Save Layer Input Mask")]
	private void DebugSaveLayerMask()
	{
		if (_layerInputTex == null)
		{
			Debug.LogWarning("[Minimap] Layer input texture null");
			return;
		}
		byte[] bytes = _layerInputTex.EncodeToPNG();
		string fullPath = Path.GetFullPath(Path.Combine(Application.dataPath, "..", "Temp", "MinimapLayerMask.png"));
		Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
		File.WriteAllBytes(fullPath, bytes);
		Debug.Log("[Minimap] Layer mask saved: " + fullPath);
	}
}
