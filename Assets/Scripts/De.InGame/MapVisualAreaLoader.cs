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
	private bool _enableVisualArea;

	[SerializeField]
	private GameObject _boundsSource;

	private bool _runtimeBoundsSet;

	private RenderTexture _sdfRT;

	private RenderTexture _maskRT;

	private RenderTexture _jfaBufferA;

	private RenderTexture _jfaBufferB;

	private int _kernelDiagonalExpand;

	private int _kernelScaleExpand;

	private int _kernelJFASeed;

	private int _kernelJFAStep;

	private int _kernelJFAToSDF;

	private const string _waveCategory = "WaterSurface";

	private const int _maxWaveInstances = 24;

	private const float _waveDuration = 2f;

	private RenderTexture _waveMapRT;

	private PlaneReflectionObject _planeReflection;

	private int _waveKernelIndex;

	private WaveInstance[] _waveInstances;

	public static MapVisualAreaLoader Instance { get; private set; }

	protected override void Awake()
	{
	}

	private void OnEnable()
	{
	}

	private void Start()
	{
	}

	private void OnStageLoadCompleted()
	{
	}

	private void OnDisable()
	{
	}

	private void OnDestroy()
	{
	}

	private void OnValidate()
	{
	}

	private void LateUpdate()
	{
	}

	public void SpawnWave(Vector3 position, float radius)
	{
	}

	private void Apply()
	{
	}

	private void InitializeSDFCompute()
	{
	}

	private void BuildSDFFromBinary(Texture binaryInput)
	{
	}

	private void RunJFAPipeline(Texture binaryInput, int groupsX, int groupsY, int outW, int outH, float spread, int outputChannel)
	{
	}

	[ContextMenu("Debug: Save SDF Textures")]
	private void DebugSaveSDFTextures()
	{
	}

	private static void SaveRT(RenderTexture rt, string path)
	{
	}

	private void ReleaseSDFCompute()
	{
	}

	private void InitializeWaveCompute()
	{
	}

	private void DispatchWaveCompute()
	{
	}

	private void UpdateWaveInstances()
	{
	}

	private void ReleaseAllWaves()
	{
	}

	private void ReleaseWaveCompute()
	{
	}

	private PlaneReflectionObject EnsurePlaneReflectionObject()
	{
		return null;
	}

	private void ApplyReflectionBounds(VisualAreaData[] areaDatas)
	{
	}

	private void ApplyReflectionBoundsFromSource()
	{
	}

	private void ClearReflectionBounds()
	{
	}

	private void ApplyMapBoundsToShader()
	{
	}

	private void ApplyBoundsIfNeeded()
	{
	}
}
