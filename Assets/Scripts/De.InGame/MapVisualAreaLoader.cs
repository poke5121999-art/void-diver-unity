using System.Collections.Generic;
using System.IO;
using GameCommon.Enum;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;

[ExecuteAlways]
public class MapVisualAreaLoader : MonoBase
{
	private struct WaveInstance
	{
		public bool Active;

		public int BufferIndex;

		public Vector3 Position;

		public float Radius;

		public float ElapsedTime;
	}

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private GameMapManager _gameMapManager;

	[Header("== 설정 ==")]
	[SerializeField]
	private MapVisualAreaSetting _setting;

	[Header("== 씬별 ==")]
	[FormerlySerializedAs("_waterMapTexture")]
	[SerializeField]
	private Texture2D _areaMapTexture;

	[SerializeField]
	private bool _enableVisualArea = true;

	[SerializeField]
	private GameObject _boundsSource;

	private bool _runtimeBoundsSet;

	private RenderTexture _sdfRT;

	private RenderTexture _maskRT;

	private RenderTexture _jfaBufferA;

	private RenderTexture _jfaBufferB;

	private int _kernelDiagonalExpand = -1;

	private int _kernelScaleExpand = -1;

	private int _kernelJFASeed = -1;

	private int _kernelJFAStep = -1;

	private int _kernelJFAToSDF = -1;

	private const string _waveCategory = "WaterSurface";

	private const int _maxWaveInstances = 24;

	private const float _waveDuration = 2f;

	private RenderTexture _waveMapRT;

	private PlaneReflectionObject _planeReflection;

	private int _waveKernelIndex = -1;

	private WaveInstance[] _waveInstances = new WaveInstance[24];

	public static MapVisualAreaLoader Instance { get; private set; }

	protected override void Awake()
	{
		if (Application.isPlaying)
		{
			base.Awake();
		}
		if (!base.enabled)
		{
			base.enabled = true;
		}
	}

	private void OnEnable()
	{
		if (Instance != null && Instance != this)
		{
			if (!Application.isPlaying)
			{
				Object.DestroyImmediate(base.gameObject);
			}
			else
			{
				Object.Destroy(base.gameObject);
			}
			return;
		}
		Instance = this;
		InitializeSDFCompute();
		InitializeWaveCompute();
		Apply();
	}

	private void Start()
	{
		if (_gameManager != null)
		{
			_gameManager.RxStageState.Where((EStageState s) => s == EStageState.Intro || s == EStageState.Playing).Take(1).Subscribe(delegate
			{
				OnStageLoadCompleted();
			}).AddTo(this);
		}
	}

	private void OnStageLoadCompleted()
	{
		if (_gameMapManager == null)
		{
			return;
		}
		VisualAreaData[] array = Object.FindObjectsByType<VisualAreaData>(FindObjectsSortMode.None);
		if (array == null || array.Length == 0)
		{
			Shader.SetGlobalInt(MapVisualAreaShaderIds.AreaEnable, 0);
			ClearReflectionBounds();
			return;
		}
		for (int i = 0; i < array.Length; i++)
		{
			if (!(array[i].areaMapTexture != null))
			{
				continue;
			}
			(int Width, int Height) mapSize = _gameMapManager.MapSize;
			int num = mapSize.Width * 30;
			int num2 = mapSize.Height * 30;
			Texture2D texture2D = new Texture2D(num, num2, TextureFormat.RGBA32, false);
			Color[] array2 = new Color[num2 * num];
			for (int j = 0; j < array2.Length; j++)
			{
				array2[j] = new Color(0f, 0f, 0f, 1f);
			}
			texture2D.SetPixels(array2);
			for (int k = 0; k < array.Length; k++)
			{
				VisualAreaData visualAreaData = array[k];
				if (visualAreaData.areaMapTexture == null)
				{
					continue;
				}
				Vector3 position = visualAreaData.transform.position;
				int num3 = Mathf.RoundToInt(position.x);
				int num4 = Mathf.RoundToInt(position.z);
				Color[] pixels = visualAreaData.areaMapTexture.GetPixels();
				int width = visualAreaData.areaMapTexture.width;
				int height = visualAreaData.areaMapTexture.height;
				for (int l = 0; l < height; l++)
				{
					int num5 = num4 + l;
					if (num2 <= num5)
					{
						break;
					}
					for (int m = 0; m < width; m++)
					{
						int num6 = num3 + m;
						if (num <= num6)
						{
							break;
						}
						if (num6 >= 0 && num5 >= 0)
						{
							texture2D.SetPixel(num6, num5, pixels[l * width + m]);
						}
					}
				}
			}
			texture2D.Apply();
			BuildSDFFromBinary(texture2D);
			if (!Application.isPlaying)
			{
				Object.DestroyImmediate(texture2D);
			}
			else
			{
				Object.Destroy(texture2D);
			}
			if (_sdfRT != null)
			{
				Shader.SetGlobalTexture(MapVisualAreaShaderIds.AreaMap, _sdfRT);
				Shader.SetGlobalInt(MapVisualAreaShaderIds.AreaEnable, 1);
				if (_gameMapManager.SectorIds.Length != 0)
				{
					ApplyMapBoundsToShader();
					_runtimeBoundsSet = true;
				}
			}
			ApplyReflectionBounds(array);
			_enableVisualArea = true;
			Apply();
			return;
		}
		bool flag = false;
		for (int n = 0; n < array.Length; n++)
		{
			if (array[n].areaBounds != null && array[n].areaBounds.Length != 0)
			{
				flag = true;
				break;
			}
		}
		Shader.SetGlobalInt(MapVisualAreaShaderIds.AreaEnable, 0);
		if (flag)
		{
			ApplyReflectionBounds(array);
		}
		else
		{
			ClearReflectionBounds();
		}
	}

	private void OnDisable()
	{
		if (Instance == this)
		{
			Shader.SetGlobalInt(MapVisualAreaShaderIds.AreaEnable, 0);
			ReleaseSDFCompute();
			Instance = null;
		}
		ReleaseAllWaves();
	}

	private void OnDestroy()
	{
		ReleaseSDFCompute();
		ReleaseWaveCompute();
		ReleaseAllWaves();
		if (Instance == this)
		{
			Instance = null;
		}
	}

	private void OnValidate()
	{
		Apply();
	}

	private void LateUpdate()
	{
		UpdateWaveInstances();
		DispatchWaveCompute();
	}

	public void SpawnWave(Vector3 position, float radius)
	{
		if (ShaderBufferPointManager.Instance == null)
		{
			return;
		}
		for (int i = 0; i < _waveInstances.Length; i++)
		{
			if (!_waveInstances[i].Active)
			{
				int num = ShaderBufferPointManager.Instance.AllocateSlot("WaterSurface");
				if (num < 0)
				{
					break;
				}
				_waveInstances[i] = new WaveInstance
				{
					Active = true,
					BufferIndex = num,
					Position = position,
					Radius = radius,
					ElapsedTime = 0f
				};
				break;
			}
		}
	}

	private void Apply()
	{
		if (_setting == null)
		{
			return;
		}
		_setting.ApplyShaderGlobals();
		if (_areaMapTexture != null || _sdfRT != null)
		{
			if (_enableVisualArea)
			{
				if (_setting.SdfComputeShader != null && _areaMapTexture != null)
				{
					BuildSDFFromBinary(_areaMapTexture);
				}
				Texture value = ((_sdfRT != null) ? ((Texture)_sdfRT) : ((Texture)_areaMapTexture));
				Shader.SetGlobalTexture(MapVisualAreaShaderIds.AreaMap, value);
				Shader.SetGlobalInt(MapVisualAreaShaderIds.AreaEnable, 1);
				if (!_runtimeBoundsSet)
				{
					ApplyBoundsIfNeeded();
				}
			}
			else
			{
				Shader.SetGlobalInt(MapVisualAreaShaderIds.AreaEnable, 0);
			}
		}
		else
		{
			Shader.SetGlobalInt(MapVisualAreaShaderIds.AreaEnable, 0);
		}
		if (!Application.isPlaying)
		{
			VisualAreaData[] componentsInChildren = ((_boundsSource != null) ? _boundsSource : base.gameObject).GetComponentsInChildren<VisualAreaData>(true);
			if (componentsInChildren == null || componentsInChildren.Length == 0)
			{
				ClearReflectionBounds();
			}
			else
			{
				ApplyReflectionBounds(componentsInChildren);
			}
		}
	}

	private void InitializeSDFCompute()
	{
		if (SystemInfo.supportsComputeShaders && !(_setting == null) && !(_setting.SdfComputeShader == null))
		{
			ComputeShader sdfComputeShader = _setting.SdfComputeShader;
			_kernelDiagonalExpand = sdfComputeShader.FindKernel("CSDiagonalExpand");
			_kernelScaleExpand = sdfComputeShader.FindKernel("CSScaleExpand");
			_kernelJFASeed = sdfComputeShader.FindKernel("CSJFASeed");
			_kernelJFAStep = sdfComputeShader.FindKernel("CSJFAStep");
			_kernelJFAToSDF = sdfComputeShader.FindKernel("CSJFAToSDF");
		}
	}

	private void BuildSDFFromBinary(Texture binaryInput)
	{
		if (_setting == null || _setting.SdfComputeShader == null || _kernelDiagonalExpand < 0)
		{
			return;
		}
		ComputeShader sdfComputeShader = _setting.SdfComputeShader;
		int width = binaryInput.width;
		int height = binaryInput.height;
		int sdfScale = _setting.SdfScale;
		int num = sdfScale * width;
		int num2 = sdfScale * height;
		if (_maskRT == null || _maskRT.width != num || _maskRT.height != num2)
		{
			ReleaseSDFCompute();
			RenderTextureDescriptor desc = new RenderTextureDescriptor(num, num2, RenderTextureFormat.ARGB32, 0);
			desc.sRGB = false;
			desc.enableRandomWrite = true;
			_maskRT = new RenderTexture(desc);
			_maskRT.filterMode = FilterMode.Point;
			_maskRT.wrapMode = TextureWrapMode.Clamp;
			if (!_maskRT.Create())
			{
				ReleaseSDFCompute();
				return;
			}
			RenderTextureDescriptor desc2 = new RenderTextureDescriptor(num, num2, RenderTextureFormat.ARGB32, 0);
			desc2.sRGB = false;
			desc2.enableRandomWrite = true;
			_sdfRT = new RenderTexture(desc2);
			_sdfRT.filterMode = FilterMode.Bilinear;
			_sdfRT.wrapMode = TextureWrapMode.Clamp;
			if (!_sdfRT.Create())
			{
				ReleaseSDFCompute();
				return;
			}
			_jfaBufferA = new RenderTexture(num, num2, 0, RenderTextureFormat.RGFloat);
			_jfaBufferA.enableRandomWrite = true;
			_jfaBufferA.filterMode = FilterMode.Point;
			if (!_jfaBufferA.Create())
			{
				ReleaseSDFCompute();
				return;
			}
			_jfaBufferB = new RenderTexture(num, num2, 0, RenderTextureFormat.RGFloat);
			_jfaBufferB.enableRandomWrite = true;
			_jfaBufferB.filterMode = FilterMode.Point;
			if (!_jfaBufferB.Create())
			{
				ReleaseSDFCompute();
				return;
			}
		}
		int groupsX = Mathf.CeilToInt((float)num * 0.125f);
		int groupsY = Mathf.CeilToInt((float)num2 * 0.125f);
		sdfComputeShader.SetInts("_InputSize", width, height);
		sdfComputeShader.SetInts("_OutputSize", num, num2);
		sdfComputeShader.SetInt("_Scale", sdfScale);
		sdfComputeShader.SetInt("_OutputChannel", 2);
		sdfComputeShader.SetTexture(_kernelDiagonalExpand, "_BinaryInput", binaryInput);
		sdfComputeShader.SetTexture(_kernelDiagonalExpand, "_MaskOutput", _maskRT);
		sdfComputeShader.Dispatch(_kernelDiagonalExpand, groupsX, groupsY, 1);
		RunJFAPipeline(binaryInput, groupsX, groupsY, num, num2, _setting.SdfSpread, 2);
		sdfComputeShader.SetInt("_SourceChannel", 0);
		sdfComputeShader.SetTexture(_kernelScaleExpand, "_BinaryInput", binaryInput);
		sdfComputeShader.SetTexture(_kernelScaleExpand, "_MaskOutput", _maskRT);
		sdfComputeShader.Dispatch(_kernelScaleExpand, groupsX, groupsY, 1);
		RunJFAPipeline(binaryInput, groupsX, groupsY, num, num2, _setting.RoofSdfSpread, 0);
	}

	private void RunJFAPipeline(Texture binaryInput, int groupsX, int groupsY, int outW, int outH, float spread, int outputChannel)
	{
		ComputeShader sdfComputeShader = _setting.SdfComputeShader;
		sdfComputeShader.SetInts("_OutputSize", outW, outH);
		sdfComputeShader.SetTexture(_kernelJFASeed, "_MaskInput", _maskRT);
		sdfComputeShader.SetTexture(_kernelJFASeed, "_JFABuffer", _jfaBufferA);
		sdfComputeShader.Dispatch(_kernelJFASeed, groupsX, groupsY, 1);
		bool flag = true;
		for (int num = Mathf.Max(outW, outH) / 2; num >= 1; num >>= 1)
		{
			RenderTexture texture = (flag ? _jfaBufferA : _jfaBufferB);
			RenderTexture texture2 = (flag ? _jfaBufferB : _jfaBufferA);
			sdfComputeShader.SetTexture(_kernelJFAStep, "_JFABufferRead", texture);
			sdfComputeShader.SetTexture(_kernelJFAStep, "_JFABuffer", texture2);
			sdfComputeShader.SetInt("_JFAStep", num);
			sdfComputeShader.SetInts("_OutputSize", outW, outH);
			sdfComputeShader.Dispatch(_kernelJFAStep, groupsX, groupsY, 1);
			flag = !flag;
		}
		RenderTexture texture3 = (flag ? _jfaBufferA : _jfaBufferB);
		sdfComputeShader.SetInt("_OutputChannel", outputChannel);
		sdfComputeShader.SetTexture(_kernelJFAToSDF, "_BinaryInput", binaryInput);
		sdfComputeShader.SetTexture(_kernelJFAToSDF, "_JFABufferRead", texture3);
		sdfComputeShader.SetTexture(_kernelJFAToSDF, "_MaskInput", _maskRT);
		sdfComputeShader.SetTexture(_kernelJFAToSDF, "_SDFOutput", _sdfRT);
		sdfComputeShader.SetFloat("_SDFSpread", spread);
		sdfComputeShader.SetInts("_OutputSize", outW, outH);
		sdfComputeShader.Dispatch(_kernelJFAToSDF, groupsX, groupsY, 1);
	}

	[ContextMenu("Debug: Save SDF Textures")]
	private void DebugSaveSDFTextures()
	{
		if (_maskRT == null && _sdfRT == null)
		{
			Debug.LogWarning("[MapVisualAreaLoader] maskRT/sdfRT 모두 null — BuildSDFFromBinary가 실행되지 않았음");
			return;
		}
		string text = "Assets/LCArt/URP/PlaneWaterSurface/Debug";
		if (!Directory.Exists(text))
		{
			Directory.CreateDirectory(text);
		}
		if (_maskRT != null)
		{
			SaveRT(_maskRT, text + "/Debug_VisualAreaMask.png");
		}
		if (_sdfRT != null)
		{
			SaveRT(_sdfRT, text + "/Debug_VisualAreaSDF.png");
		}
		Debug.Log("[MapVisualAreaLoader] 디버그 텍스처 저장 완료 → " + text + "/");
	}

	private static void SaveRT(RenderTexture rt, string path)
	{
		RenderTexture active = RenderTexture.active;
		RenderTexture.active = rt;
		Texture2D texture2D = new Texture2D(rt.width, rt.height, TextureFormat.RGBA32, false);
		texture2D.ReadPixels(new Rect(0f, 0f, rt.width, rt.height), 0, 0);
		texture2D.Apply();
		RenderTexture.active = active;
		File.WriteAllBytes(path, texture2D.EncodeToPNG());
		if (!Application.isPlaying)
		{
			Object.DestroyImmediate(texture2D);
		}
		else
		{
			Object.Destroy(texture2D);
		}
	}

	private void ReleaseSDFCompute()
	{
		SafeRelease(ref _sdfRT);
		SafeRelease(ref _maskRT);
		SafeRelease(ref _jfaBufferA);
		SafeRelease(ref _jfaBufferB);
		static void SafeRelease(ref RenderTexture rt)
		{
			if (rt != null)
			{
				rt.Release();
				if (!Application.isPlaying)
				{
					Object.DestroyImmediate(rt);
				}
				else
				{
					Object.Destroy(rt);
				}
				rt = null;
			}
		}
	}

	private void InitializeWaveCompute()
	{
		if (!SystemInfo.supportsComputeShaders || _setting == null || _setting.WaveComputeShader == null)
		{
			return;
		}
		_waveKernelIndex = _setting.WaveComputeShader.FindKernel("CSWaveMap");
		if (_waveMapRT == null)
		{
			int waveMapResolution = _setting.WaveMapResolution;
			_waveMapRT = new RenderTexture(waveMapResolution, waveMapResolution, 0, RenderTextureFormat.ARGBHalf);
			_waveMapRT.enableRandomWrite = true;
			_waveMapRT.filterMode = FilterMode.Bilinear;
			_waveMapRT.wrapMode = TextureWrapMode.Clamp;
			if (!_waveMapRT.Create())
			{
				_waveMapRT.Release();
				_waveMapRT = null;
			}
		}
	}

	private void DispatchWaveCompute()
	{
		if (_setting == null || _setting.WaveComputeShader == null || _waveMapRT == null || _waveKernelIndex < 0)
		{
			return;
		}
		ComputeShader waveComputeShader = _setting.WaveComputeShader;
		int waveMapResolution = _setting.WaveMapResolution;
		Vector4 globalVector = Shader.GetGlobalVector(MapVisualAreaShaderIds.OverallMapBoundMin);
		Vector4 globalVector2 = Shader.GetGlobalVector(MapVisualAreaShaderIds.OverallMapBoundMax);
		float num = globalVector2.x - globalVector.x;
		float num2 = globalVector2.z - globalVector.z;
		float num3 = (globalVector2.x + globalVector.x) * 0.5f;
		float num4 = (globalVector2.z + globalVector.z) * 0.5f;
		waveComputeShader.SetTexture(_waveKernelIndex, MapVisualAreaShaderIds.WaveResult, _waveMapRT);
		waveComputeShader.SetVector(MapVisualAreaShaderIds.WaveMapParams, new Vector4(num3, num4, num, num2));
		waveComputeShader.SetInts(MapVisualAreaShaderIds.WaveMapResolution, waveMapResolution, waveMapResolution);
		int num5 = Mathf.CeilToInt((float)waveMapResolution * 0.125f);
		waveComputeShader.Dispatch(_waveKernelIndex, num5, num5, 1);
		Shader.SetGlobalTexture(MapVisualAreaShaderIds.GlobalWaveMapTexture, _waveMapRT);
		Shader.SetGlobalVector(MapVisualAreaShaderIds.GlobalWaveMapParams, new Vector4(num3, num4, num, num2));
	}

	private void UpdateWaveInstances()
	{
		if (ShaderBufferPointManager.Instance == null)
		{
			return;
		}
		bool flag = false;
		for (int i = 0; i < _waveInstances.Length; i++)
		{
			if (_waveInstances[i].Active)
			{
				_waveInstances[i].ElapsedTime += Time.deltaTime;
				float num = Mathf.Clamp01(_waveInstances[i].ElapsedTime / 2f);
				if (num >= 1f)
				{
					ShaderBufferPointManager.Instance.ReleaseSlot("WaterSurface", _waveInstances[i].BufferIndex);
					_waveInstances[i].Active = false;
				}
				else
				{
					ShaderBufferPointManager.Instance.UpdatePosition("WaterSurface", _waveInstances[i].BufferIndex, new Vector3(_waveInstances[i].Position.x, num, _waveInstances[i].Position.z), _waveInstances[i].Radius);
				}
				flag = true;
			}
		}
		if (flag)
		{
			ShaderBufferPointManager.Instance.SendToShader();
		}
	}

	private void ReleaseAllWaves()
	{
		if (ShaderBufferPointManager.Instance == null)
		{
			return;
		}
		for (int i = 0; i < _waveInstances.Length; i++)
		{
			if (_waveInstances[i].Active)
			{
				ShaderBufferPointManager.Instance.ReleaseSlot("WaterSurface", _waveInstances[i].BufferIndex);
				_waveInstances[i].Active = false;
			}
		}
		ShaderBufferPointManager.Instance.SendToShader();
	}

	private void ReleaseWaveCompute()
	{
		if (_waveMapRT != null)
		{
			_waveMapRT.Release();
			if (!Application.isPlaying)
			{
				Object.DestroyImmediate(_waveMapRT);
			}
			else
			{
				Object.Destroy(_waveMapRT);
			}
			_waveMapRT = null;
		}
	}

	private PlaneReflectionObject EnsurePlaneReflectionObject()
	{
		if (!(_planeReflection != null))
		{
			_planeReflection = GetComponent<PlaneReflectionObject>();
			if (!(_planeReflection != null))
			{
				_planeReflection = base.gameObject.AddComponent<PlaneReflectionObject>();
			}
		}
		return _planeReflection;
	}

	private void ApplyReflectionBounds(VisualAreaData[] areaDatas)
	{
		List<Bounds> list = new List<Bounds>();
		for (int i = 0; i < areaDatas.Length; i++)
		{
			VisualAreaData visualAreaData = areaDatas[i];
			if (visualAreaData.areaBounds == null || visualAreaData.areaBounds.Length == 0)
			{
				continue;
			}
			Vector3 position = visualAreaData.transform.position;
			Bounds[] areaBounds = visualAreaData.areaBounds;
			for (int j = 0; j < areaBounds.Length; j++)
			{
				list.Add(new Bounds(areaBounds[j].center + position, areaBounds[j].size));
			}
		}
		if (list.Count < 1)
		{
			ClearReflectionBounds();
		}
		else
		{
			EnsurePlaneReflectionObject().SetOverrideBounds(list);
		}
	}

	private void ApplyReflectionBoundsFromSource()
	{
		VisualAreaData[] componentsInChildren = ((_boundsSource != null) ? _boundsSource : base.gameObject).GetComponentsInChildren<VisualAreaData>(true);
		if (componentsInChildren == null || componentsInChildren.Length == 0)
		{
			ClearReflectionBounds();
		}
		else
		{
			ApplyReflectionBounds(componentsInChildren);
		}
	}

	private void ClearReflectionBounds()
	{
		if (_planeReflection != null)
		{
			_planeReflection.ClearOverrideBounds();
		}
	}

	private void ApplyMapBoundsToShader()
	{
		Vector3 sectorOffset = _gameMapManager.GetSectorOffset(0);
		float num = sectorOffset.x + 30f;
		float num2 = sectorOffset.y + 0f;
		float num3 = sectorOffset.z + 30f;
		Vector3 sectorOffset2 = _gameMapManager.GetSectorOffset(0);
		float num4 = sectorOffset2.x;
		float num5 = sectorOffset2.y;
		float num6 = sectorOffset2.z;
		for (int i = 1; i < _gameMapManager.SectorIds.Length; i++)
		{
			Vector3 sectorOffset3 = _gameMapManager.GetSectorOffset(i);
			num = Mathf.Max(num, sectorOffset3.x + 30f);
			num2 = Mathf.Max(num2, sectorOffset3.y + 0f);
			num3 = Mathf.Max(num3, sectorOffset3.z + 30f);
			Vector3 sectorOffset4 = _gameMapManager.GetSectorOffset(i);
			num4 = Mathf.Min(num4, sectorOffset4.x);
			num5 = Mathf.Min(num5, sectorOffset4.y);
			num6 = Mathf.Min(num6, sectorOffset4.z);
		}
		Shader.SetGlobalVector(MapVisualAreaShaderIds.OverallMapBoundMax, new Vector4(num, num2, num3, 0f));
		Shader.SetGlobalVector(MapVisualAreaShaderIds.OverallMapBoundMin, new Vector4(num4, num5, num6, 0f));
		Shader.SetGlobalFloat(MapVisualAreaShaderIds.SectorWidth, 30f);
		Shader.SetGlobalFloat(MapVisualAreaShaderIds.SectorHeight, 30f);
	}

	private void ApplyBoundsIfNeeded()
	{
		if (_gameMapManager != null && _gameMapManager.SectorIds.Length != 0)
		{
			return;
		}
		Renderer[] componentsInChildren = ((_boundsSource != null) ? _boundsSource : base.gameObject).GetComponentsInChildren<Renderer>(true);
		if (componentsInChildren.Length != 0)
		{
			Bounds bounds = componentsInChildren[0].bounds;
			for (int i = 1; i < componentsInChildren.Length; i++)
			{
				bounds.Encapsulate(componentsInChildren[i].bounds);
			}
			Vector3 min = bounds.min;
			Vector3 max = bounds.max;
			Shader.SetGlobalVector(MapVisualAreaShaderIds.OverallMapBoundMin, new Vector4(min.x, min.y, min.z, 0f));
			Shader.SetGlobalVector(MapVisualAreaShaderIds.OverallMapBoundMax, new Vector4(max.x, max.y, max.z, 0f));
		}
	}
}
