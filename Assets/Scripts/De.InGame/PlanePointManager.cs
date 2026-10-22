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

	public static Subject<PlanePointManager> OnInstantiated;

	protected List<PlanePointHelper> PlanePoints;

	protected List<PlanePointHelper> CurrentVisiblePoints;

	protected List<PlanePointHelper> CurrentPlanePointBuffer;

	protected List<PlanePointHelper> CurrentRimEffect;

	[Inject]
	private CameraManager _cameraManager;

	private int _checkFrame;

	private Vector4[] _planePointColors;

	private Vector4[] _planePointPositions;

	private float[] _planePointRanges;

	private float[] _planePointIntensities;

	private Vector4[] _planePointBufferColors;

	private Vector4[] _planePointBufferPositions;

	private float[] _planePointBufferRanges;

	private float[] _planePointBufferIntensities;

	private Vector4[] _rimEffectColors;

	private Vector4[] _rimEffectPositions;

	private float[] _rimEffectRanges;

	private float[] _rimEffectIntensities;

	[Header("Plane Point Buffer Enable")]
	[FormerlySerializedAs("_lightBufferEnable")]
	[SerializeField]
	private bool _planePointBufferEnable;

	[Header("Plane Point Buffer Settings")]
	[FormerlySerializedAs("_lightComputeShader")]
	[SerializeField]
	private ComputeShader _planePointComputeShader;

	[SerializeField]
	private Vector3Int _renderTextureResolution;

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

	private readonly Plane[] _frustumPlanes;

	private Camera CurrentTargetCamera => null;

	protected override void Awake()
	{
	}

	private void OnEnable()
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

	private void LateUpdate()
	{
	}

	public void RegisterPoint(PlanePointHelper point)
	{
	}

	public void UnRegisterPoint(PlanePointHelper point)
	{
	}

	private void UpdateVisiblePoints()
	{
	}

	private void OnRevealingMapChanged(bool isRevealing)
	{
	}

	private void InitializePlanePointBuffer()
	{
	}

	private RenderTexture CreatePlanePointBufferTexture(string textureName)
	{
		return null;
	}

	private void CleanUpPlanePointBufferRenderTexture()
	{
	}
}
