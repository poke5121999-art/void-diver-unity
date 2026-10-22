using System.Collections.Generic;
using UniRx;
using UnityEngine;
using VContainer;

[DefaultExecutionOrder(-100)]
[ExecuteAlways]
public class FakeShadowManager : MonoBase
{
	[Inject]
	private CameraManager _cameraManager;

	[SerializeField]
	private Camera _targetCamera;

	public static FakeShadowManager Instance;

	public static Subject<FakeShadowManager> OnInstantiated;

	[HideInInspector]
	[SerializeField]
	private List<FakeShadowPoint> _screenSpaceShadowPoints;

	[HideInInspector]
	[SerializeField]
	private List<FakeShadowPoint> _currentScreenSpacePoints;

	[HideInInspector]
	[SerializeField]
	private List<FakeShadowPoint> _currentObjectPoints;

	[Header("Blob Shadow Texture")]
	[SerializeField]
	private Texture _blobShadowTexture;

	[Header("Debug Gizmos")]
	[Tooltip("씬뷰에 등록된 모든 Point + 컬링 활성 Point + 카메라 frustum 표시")]
	[SerializeField]
	private bool _drawDebugGizmos;

	private Vector4[] _pointPositions;

	private float[] _pointRanges;

	private int _remainCountForChecking;

	private float _frustumMargin;

	private const string _blobShadowKeyword = "_DE_FAKE_SHADOW_BLOB";

	private readonly Plane[] _frustumPlanes;

	public Texture BlobShadowTexture => null;

	public IReadOnlyList<FakeShadowPoint> CurrentObjectPoints => null;

	private Camera GetTargetCamera()
	{
		return null;
	}

	protected override void Awake()
	{
	}

	private void OnEnable()
	{
	}

	private void OnDestroy()
	{
	}

	public void RegisterPointToManager(FakeShadowPoint point)
	{
	}

	public void UnregisterPointFromManager(FakeShadowPoint point)
	{
	}

	private void LateUpdate()
	{
	}

	private void UpdateVisiblePoints()
	{
	}

	private void UpdateGlobalProperties()
	{
	}

	private void OnDrawGizmos()
	{
	}
}
