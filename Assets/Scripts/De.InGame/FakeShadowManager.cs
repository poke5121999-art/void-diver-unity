using System.Collections.Generic;
using UniRx;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
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

	public static Subject<FakeShadowManager> OnInstantiated = new Subject<FakeShadowManager>();

	[HideInInspector]
	[SerializeField]
	private List<FakeShadowPoint> _screenSpaceShadowPoints = new List<FakeShadowPoint>();

	[HideInInspector]
	[SerializeField]
	private List<FakeShadowPoint> _currentScreenSpacePoints = new List<FakeShadowPoint>();

	[HideInInspector]
	[SerializeField]
	private List<FakeShadowPoint> _currentObjectPoints = new List<FakeShadowPoint>();

	[Header("Blob Shadow Texture")]
	[SerializeField]
	private Texture _blobShadowTexture;

	[Header("Debug Gizmos")]
	[Tooltip("씬뷰에 등록된 모든 Point + 컬링 활성 Point + 카메라 frustum 표시")]
	[SerializeField]
	private bool _drawDebugGizmos;

	private Vector4[] _pointPositions = new Vector4[FakeShadowConsts.NUMBER_SHADOW_POINT_COUNT_MAX];

	private float[] _pointRanges = new float[FakeShadowConsts.NUMBER_SHADOW_POINT_COUNT_MAX];

	private int _remainCountForChecking;

	private float _frustumMargin;

	private const string _blobShadowKeyword = "_DE_FAKE_SHADOW_BLOB";

	private readonly Plane[] _frustumPlanes = new Plane[6];

	public Texture BlobShadowTexture => _blobShadowTexture;

	public IReadOnlyList<FakeShadowPoint> CurrentObjectPoints => _currentObjectPoints;

	private Camera GetTargetCamera()
	{
		if (!Application.isPlaying)
		{
			return _targetCamera;
		}
		return (_cameraManager != null) ? _cameraManager.MainCamera : null;
	}

	protected override void Awake()
	{
		if (Application.isPlaying)
		{
			base.Awake();
		}
		_screenSpaceShadowPoints.Clear();
		_currentScreenSpacePoints.Clear();
		_currentObjectPoints.Clear();
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
		OnInstantiated.OnNext(this);
		_remainCountForChecking = 0;
	}

	private void OnDestroy()
	{
		if (Instance == this)
		{
			Instance = null;
		}
	}

	public void RegisterPointToManager(FakeShadowPoint point)
	{
		if (!_screenSpaceShadowPoints.Contains(point))
		{
			_screenSpaceShadowPoints.Add(point);
		}
	}

	public void UnregisterPointFromManager(FakeShadowPoint point)
	{
		if (_screenSpaceShadowPoints.Contains(point))
		{
			_screenSpaceShadowPoints.Remove(point);
		}
	}

	private void LateUpdate()
	{
		Camera targetCamera = GetTargetCamera();
		if (!(targetCamera == null))
		{
			_remainCountForChecking--;
			if (_remainCountForChecking <= 0)
			{
				UpdateVisiblePoints();
				_remainCountForChecking = FakeShadowConsts.NUMBER_CAMERA_CHECK_COUNT_FRAME;
			}
			UpdateGlobalProperties();
		}
	}

	private void UpdateVisiblePoints()
	{
		Camera targetCamera = GetTargetCamera();
		if (targetCamera == null)
		{
			return;
		}
		_currentScreenSpacePoints.Clear();
		_currentObjectPoints.Clear();
		GeometryUtility.CalculateFrustumPlanes(targetCamera, _frustumPlanes);
		foreach (FakeShadowPoint point in _screenSpaceShadowPoints)
		{
			if (point == null || !point.gameObject.activeSelf || !point.enabled)
			{
				continue;
			}
			Bounds bounds = new Bounds(point.Position, Vector3.one * ((point.Range + _frustumMargin) * 2f));
			if (GeometryUtility.TestPlanesAABB(_frustumPlanes, bounds))
			{
				if (point.ScreenSpaceEnable)
				{
					_currentScreenSpacePoints.Add(point);
				}
				if (point.ObjectEnable)
				{
					_currentObjectPoints.Add(point);
				}
			}
		}
	}

	private void UpdateGlobalProperties()
	{
		int num = (Shader.IsKeywordEnabled(_blobShadowKeyword) ? 0 : _currentScreenSpacePoints.Count);
		Shader.SetGlobalFloat(FakeShadowConsts.SHADER_PROPERTY_POINT_COUNT, num);
		int num2 = Mathf.Min(num, FakeShadowConsts.NUMBER_SHADOW_POINT_COUNT_MAX);
		for (int i = 0; i < num2; i++)
		{
			if (_currentScreenSpacePoints[i] == null)
			{
				_pointRanges[i] = 0f;
				continue;
			}
			_pointRanges[i] = _currentScreenSpacePoints[i].Range;
			Vector3 position = _currentScreenSpacePoints[i].Position;
			_pointPositions[i] = new Vector4(position.x, position.y, position.z, 0f);
		}
		Shader.SetGlobalFloatArray(FakeShadowConsts.SHADER_PROPERTY_POINT_RANGES, _pointRanges);
		Shader.SetGlobalVectorArray(FakeShadowConsts.SHADER_PROPERTY_POINT_POSITIONS, _pointPositions);
		VolumeStack stack = VolumeManager.instance.stack;
		Camera targetCamera = GetTargetCamera();
		FakeShadowVolumeComponent fakeShadowVolumeComponent = null;
		if (stack != null)
		{
			if (targetCamera != null)
			{
				UniversalAdditionalCameraData component = targetCamera.GetComponent<UniversalAdditionalCameraData>();
				if (component != null)
				{
					VolumeManager.instance.Update(stack, component.volumeTrigger, component.volumeLayerMask);
				}
			}
			fakeShadowVolumeComponent = stack.GetComponent<FakeShadowVolumeComponent>();
		}
		if ((bool)fakeShadowVolumeComponent)
		{
			Shader.SetGlobalColor(FakeShadowConsts.SHADER_PROPERTY_SHADOW_COLOR, fakeShadowVolumeComponent.ShadowColor.value);
			Shader.SetGlobalFloat(FakeShadowConsts.SHADER_PROPERTY_SHADOW_STANDARD_Y, fakeShadowVolumeComponent.ShadowStandardY.value);
			Shader.SetGlobalFloat(FakeShadowConsts.SHADER_PROPERTY_SCREEN_SPACE_SHADOW_ALPHA, fakeShadowVolumeComponent.ScreenSpaceShadowAlpha.value);
			Shader.SetGlobalFloat(FakeShadowConsts.SHADER_PROPERTY_SCREEN_SPACE_OBJECT_SHADOW_DIRECTIONAL_ALPHA, fakeShadowVolumeComponent.ScreenSpaceObjectShadowDirectionalAlpha.value);
			Shader.SetGlobalFloat(FakeShadowConsts.SHADER_PROPERTY_SCREEN_SPACE_OBJECT_SHADOW_DIRECTIONAL_LENGTH, fakeShadowVolumeComponent.ScreenSpaceObjectShadowDirectionalLength.value);
			Shader.SetGlobalFloat(FakeShadowConsts.SHADER_PROPERTY_SCREEN_SPACE_OBJECT_SHADOW_ADDITIONAL_ALPHA, fakeShadowVolumeComponent.ScreenSpaceObjectShadowAdditionalAlpha.value);
			Shader.SetGlobalFloat(FakeShadowConsts.SHADER_PROPERTY_SCREEN_SPACE_OBJECT_SHADOW_ADDITIONAL_LENGTH, fakeShadowVolumeComponent.ScreenSpaceObjectShadowAdditionalLength.value);
		}
	}

	private void OnDrawGizmos()
	{
		if (!_drawDebugGizmos)
		{
			return;
		}
		Gizmos.color = new Color(1f, 1f, 1f, 0.15f);
		foreach (FakeShadowPoint screenSpaceShadowPoint in _screenSpaceShadowPoints)
		{
			if (!(screenSpaceShadowPoint == null))
			{
				Gizmos.DrawWireSphere(screenSpaceShadowPoint.Position, screenSpaceShadowPoint.Range);
			}
		}
		Gizmos.color = new Color(1f, 1f, 0f, 0.8f);
		foreach (FakeShadowPoint currentScreenSpacePoint in _currentScreenSpacePoints)
		{
			if (!(currentScreenSpacePoint == null))
			{
				Gizmos.DrawWireSphere(currentScreenSpacePoint.Position, currentScreenSpacePoint.Range);
				Gizmos.DrawCube(currentScreenSpacePoint.Position, Vector3.one * 0.15f);
			}
		}
		Gizmos.color = new Color(1f, 0f, 1f, 0.8f);
		foreach (FakeShadowPoint currentObjectPoint in _currentObjectPoints)
		{
			if (!(currentObjectPoint == null))
			{
				Gizmos.DrawWireSphere(currentObjectPoint.Position, currentObjectPoint.Range * 0.98f);
			}
		}
		Camera targetCamera = GetTargetCamera();
		if (targetCamera != null)
		{
			Gizmos.color = new Color(0f, 1f, 1f, 0.5f);
			Gizmos.matrix = targetCamera.transform.localToWorldMatrix;
			Gizmos.DrawFrustum(Vector3.zero, targetCamera.fieldOfView, targetCamera.farClipPlane, targetCamera.nearClipPlane, targetCamera.aspect);
			Gizmos.matrix = Matrix4x4.identity;
		}
	}
}
