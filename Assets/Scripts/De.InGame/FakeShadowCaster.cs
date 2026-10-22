using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteAlways]
public class FakeShadowCaster : MonoBehaviour
{
	private struct CasterData
	{
		public Renderer Renderer;

		public MeshFilter MeshFilter;

		public SkinnedMeshRenderer SkinnedMeshRenderer;

		public Mesh BakedMesh;
	}

	private const string _planeSightKeyword = "_DE_FAKE_SHADOW_PLANE_SIGHT";

	private const string _blobShadowKeyword = "_DE_FAKE_SHADOW_BLOB";

	private const string _directionalOnlyKeyword = "_DE_FAKE_SHADOW_DIRECTIONAL_ONLY";

	private const int _maxPointCount = 9;

	[Header("Ground")]
	[SerializeField]
	private float _groundY;

	[SerializeField]
	private float _groundOffset = 0.01f;

	[Header("Mesh Transform")]
	[SerializeField]
	private Vector3 _meshScale = Vector3.one;

	[SerializeField]
	private Vector3 _meshRotation = Vector3.zero;

	[Header("Mesh Source")]
	[Tooltip("직접 메쉬를 지정합니다. 비어있으면 자동 수집합니다.")]
	[SerializeField]
	private MeshRenderer[] _manualMeshRenderers;

	[SerializeField]
	private SkinnedMeshRenderer[] _manualSkinnedMeshRenderers;

	[Header("Options")]
	[SerializeField]
	private bool _includeChildren = true;

	[Tooltip("비활성화된 렌더러도 수집합니다 (자동 수집 시에만 적용).")]
	[SerializeField]
	private bool _includeInactive;

	[Header("Sprite Mode")]
	[Tooltip("Renderer 없이 Quad + Sprite로 그림자를 투영합니다.")]
	[SerializeField]
	private bool _spriteMode;

	[Header("Shader")]
	[Tooltip("PlaneSight(시야 시스템) 적용 여부. 캐릭터 캐스터만 true. 일반/blob 캐스터는 false.")]
	[SerializeField]
	private bool _usePlaneSight = true;

	[Tooltip("UnifiedShadow + GPU Instancing 켜진 머티리얼 에셋. 빌드 INSTANCING_ON 변종 유지용. 미할당 시 Shader.Find 폴백(빌드 캐스터 누락 위험).")]
	[SerializeField]
	private Material _shadowMaterialSource;

	[Header("Blob Shadow Setting")]
	[SerializeField]
	private float _blobShadowRadius = 0.5f;

	private static readonly List<FakeShadowCaster> _activeCasters = new List<FakeShadowCaster>();

	private readonly List<CasterData> _casterDataList = new List<CasterData>();

	private static Material _sharedMatPlaneSight;

	private static Material _sharedMatNoPlaneSight;

	private Material _shadowMaterial;

	private MaterialPropertyBlock _materialPropertyBlock;

	private readonly Matrix4x4[] _instanceMatrices = new Matrix4x4[9];

	private readonly float[] _instanceIsPoint = new float[9];

	private readonly Vector4[] _instanceLightData = new Vector4[9];

	private readonly float[] _instanceRange = new float[9];

	private readonly float[] _instanceAlpha = new float[9];

	private readonly float[] _instanceScale = new float[9];

	private int _currentInstanceCount;

	private bool _isReadyToDraw;

	[CompilerGenerated]
	private Action<MaterialPropertyBlock, Renderer> m_OnSetupMaterialProperties;

	private static Mesh _sharedQuadMesh;

	private Texture _spriteTexture;

	private Transform _spriteTransform;

	private Mesh _blobQuadMesh;

	private static readonly int _propIsPoint = Shader.PropertyToID("_FakeShadowIsPoint");

	private static readonly int _propLightData = Shader.PropertyToID("_FakeShadowLightData");

	private static readonly int _propGroundY = Shader.PropertyToID("_FakeShadowGroundY");

	private static readonly int _propRange = Shader.PropertyToID("_FakeShadowPointRange");

	private static readonly int _propAlpha = Shader.PropertyToID("_FakeShadowAlpha");

	private static readonly int _propScale = Shader.PropertyToID("_FakeShadowScale");

	private static readonly int _propMainTex = Shader.PropertyToID("_MainTex");

	private static readonly int _propMainTexST = Shader.PropertyToID("_MainTex_ST");

	public static IReadOnlyList<FakeShadowCaster> ActiveCasters => _activeCasters;

	public event Action<MaterialPropertyBlock, Renderer> OnSetupMaterialProperties
	{
		[CompilerGenerated]
		add
		{
			Action<MaterialPropertyBlock, Renderer> action = m_OnSetupMaterialProperties;
			Action<MaterialPropertyBlock, Renderer> action2;
			do
			{
				action2 = action;
				Action<MaterialPropertyBlock, Renderer> value2 = (Action<MaterialPropertyBlock, Renderer>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref m_OnSetupMaterialProperties, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<MaterialPropertyBlock, Renderer> action = m_OnSetupMaterialProperties;
			Action<MaterialPropertyBlock, Renderer> action2;
			do
			{
				action2 = action;
				Action<MaterialPropertyBlock, Renderer> value2 = (Action<MaterialPropertyBlock, Renderer>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref m_OnSetupMaterialProperties, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	private void OnValidate()
	{
		_shadowMaterial = null;
		EnsureMaterial();
	}

	private void OnEnable()
	{
		if (!_spriteMode)
		{
			CollectRenderers();
		}
		else
		{
			EnsureQuadMesh();
		}
		EnsureBlobQuadMesh();
		EnsureMaterial();
		_materialPropertyBlock = new MaterialPropertyBlock();
		_activeCasters.Add(this);
	}

	private void OnDisable()
	{
		ReleaseBakedMeshes();
		ReleaseBlobQuadMesh();
		_activeCasters.Remove(this);
	}

	private void OnDestroy()
	{
		ReleaseBakedMeshes();
		ReleaseBlobQuadMesh();
		_activeCasters.Remove(this);
		_shadowMaterial = null;
	}

	private void ReleaseBlobQuadMesh()
	{
		if (!(_blobQuadMesh == null))
		{
			if (Application.isPlaying)
			{
				UnityEngine.Object.Destroy(_blobQuadMesh);
			}
			else
			{
				UnityEngine.Object.DestroyImmediate(_blobQuadMesh);
			}
			_blobQuadMesh = null;
		}
	}

	public void SetSprite(Sprite sprite)
	{
		_spriteTexture = ((sprite != null) ? sprite.texture : null);
	}

	public void SetSpriteTransform(Transform t)
	{
		_spriteTransform = t;
	}

	private static void EnsureQuadMesh()
	{
		if (!(_sharedQuadMesh != null))
		{
			_sharedQuadMesh = new Mesh
			{
				name = "FakeShadow_Quad"
			};
			_sharedQuadMesh.vertices = new Vector3[4]
			{
				new Vector3(-0.5f, -0.5f, 0f),
				new Vector3(0.5f, -0.5f, 0f),
				new Vector3(0.5f, 0.5f, 0f),
				new Vector3(-0.5f, 0.5f, 0f)
			};
			_sharedQuadMesh.uv = new Vector2[4]
			{
				new Vector2(0f, 0f),
				new Vector2(1f, 0f),
				new Vector2(1f, 1f),
				new Vector2(0f, 1f)
			};
			_sharedQuadMesh.triangles = new int[6] { 0, 2, 1, 0, 3, 2 };
			_sharedQuadMesh.RecalculateNormals();
			_sharedQuadMesh.hideFlags = HideFlags.HideAndDontSave;
		}
	}

	private void EnsureBlobQuadMesh()
	{
		if (!(_blobQuadMesh != null))
		{
			_blobQuadMesh = new Mesh
			{
				name = "FakeShadow_BlobQuad"
			};
			_blobQuadMesh.vertices = new Vector3[4]
			{
				new Vector3(-1f, 0f, -1f),
				new Vector3(1f, 0f, -1f),
				new Vector3(1f, 0f, 1f),
				new Vector3(-1f, 0f, 1f)
			};
			_blobQuadMesh.uv = new Vector2[4]
			{
				new Vector2(0f, 0f),
				new Vector2(1f, 0f),
				new Vector2(1f, 1f),
				new Vector2(0f, 1f)
			};
			_blobQuadMesh.triangles = new int[6] { 0, 2, 1, 0, 3, 2 };
			_blobQuadMesh.RecalculateNormals();
			_blobQuadMesh.hideFlags = HideFlags.HideAndDontSave;
		}
	}

	private void CollectRenderers()
	{
		_casterDataList.Clear();
		if ((_manualMeshRenderers == null || _manualMeshRenderers.Length == 0) && (_manualSkinnedMeshRenderers == null || _manualSkinnedMeshRenderers.Length == 0))
		{
			CollectAutoRenderers();
		}
		else
		{
			CollectManualRenderers();
		}
	}

	private void CollectManualRenderers()
	{
		if (_manualSkinnedMeshRenderers != null)
		{
			SkinnedMeshRenderer[] manualSkinnedMeshRenderers = _manualSkinnedMeshRenderers;
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in manualSkinnedMeshRenderers)
			{
				if (!(skinnedMeshRenderer == null))
				{
					_casterDataList.Add(new CasterData
					{
						Renderer = skinnedMeshRenderer,
						SkinnedMeshRenderer = skinnedMeshRenderer,
						BakedMesh = new Mesh()
					});
				}
			}
		}
		if (_manualMeshRenderers == null)
		{
			return;
		}
		MeshRenderer[] manualMeshRenderers = _manualMeshRenderers;
		foreach (MeshRenderer meshRenderer in manualMeshRenderers)
		{
			if (!(meshRenderer == null))
			{
				MeshFilter component = meshRenderer.GetComponent<MeshFilter>();
				if (component != null)
				{
					_casterDataList.Add(new CasterData
					{
						Renderer = meshRenderer,
						MeshFilter = component
					});
				}
			}
		}
	}

	private void CollectAutoRenderers()
	{
		Renderer[] array = (_includeChildren ? GetComponentsInChildren<Renderer>(_includeInactive) : GetComponents<Renderer>());
		Renderer[] array2 = array;
		foreach (Renderer renderer in array2)
		{
			if (renderer is SkinnedMeshRenderer skinnedMeshRenderer)
			{
				_casterDataList.Add(new CasterData
				{
					Renderer = renderer,
					SkinnedMeshRenderer = skinnedMeshRenderer,
					BakedMesh = new Mesh()
				});
			}
			else if (renderer is MeshRenderer)
			{
				MeshFilter component = renderer.GetComponent<MeshFilter>();
				if (component != null)
				{
					_casterDataList.Add(new CasterData
					{
						Renderer = renderer,
						MeshFilter = component
					});
				}
			}
		}
	}

	private void ReleaseBakedMeshes()
	{
		foreach (CasterData casterData in _casterDataList)
		{
			if (casterData.BakedMesh != null)
			{
				if (Application.isPlaying)
				{
					UnityEngine.Object.Destroy(casterData.BakedMesh);
				}
				else
				{
					UnityEngine.Object.DestroyImmediate(casterData.BakedMesh);
				}
			}
		}
		_casterDataList.Clear();
	}

	private void EnsureMaterial()
	{
		if (!(_shadowMaterial != null))
		{
			_shadowMaterial = GetSharedMaterial(_usePlaneSight);
		}
	}

	private Material GetSharedMaterial(bool planeSight)
	{
		if (planeSight)
		{
			if (_sharedMatPlaneSight == null)
			{
				_sharedMatPlaneSight = CreateSharedMaterial(planeSight: true);
			}
			return _sharedMatPlaneSight;
		}
		if (_sharedMatNoPlaneSight == null)
		{
			_sharedMatNoPlaneSight = CreateSharedMaterial(planeSight: false);
		}
		return _sharedMatNoPlaneSight;
	}

	private Material CreateSharedMaterial(bool planeSight)
	{
		Material material;
		if (_shadowMaterialSource != null)
		{
			material = new Material(_shadowMaterialSource);
		}
		else
		{
			Shader shader = Shader.Find(FakeShadowConsts.SHADER_NAME);
			if (shader == null)
			{
				return null;
			}
			material = new Material(shader);
		}
		material.hideFlags = HideFlags.HideAndDontSave;
		material.enableInstancing = true;
		if (planeSight)
		{
			material.EnableKeyword("_DE_FAKE_SHADOW_PLANE_SIGHT");
		}
		else
		{
			material.DisableKeyword("_DE_FAKE_SHADOW_PLANE_SIGHT");
		}
		return material;
	}

	private void LateUpdate()
	{
		_isReadyToDraw = false;
		if (UseBlobShadow())
		{
			UpdateBlobShadow();
		}
		else
		{
			UpdateMeshShadows();
		}
	}

	private bool UseBlobShadow()
	{
		if (!Shader.IsKeywordEnabled("_DE_FAKE_SHADOW_BLOB"))
		{
			return _spriteMode;
		}
		return true;
	}

	private void UpdateBlobShadow()
	{
		if (_shadowMaterial == null || _blobQuadMesh == null)
		{
			return;
		}
		FakeShadowManager instance = FakeShadowManager.Instance;
		if (instance == null)
		{
			return;
		}
		Texture blobShadowTexture = instance.BlobShadowTexture;
		if (blobShadowTexture == null)
		{
			return;
		}
		Transform transform = ((_spriteTransform != null) ? _spriteTransform : base.transform);
		if (transform.gameObject.activeInHierarchy)
		{
			Vector3 position = transform.position;
			float num = _groundOffset + _groundY;
			_instanceMatrices[0] = Matrix4x4.TRS(new Vector3(position.x, num, position.z), Quaternion.identity, new Vector3(_blobShadowRadius, 1f, _blobShadowRadius));
			_instanceIsPoint[0] = 0f;
			_instanceLightData[0] = new Vector4(0f, -1f, 0f, 0f);
			_instanceRange[0] = 0f;
			_instanceAlpha[0] = Shader.GetGlobalFloat(FakeShadowConsts.SHADER_PROPERTY_SCREEN_SPACE_OBJECT_SHADOW_DIRECTIONAL_ALPHA);
			_instanceScale[0] = 0f;
			_currentInstanceCount = 1;
			_materialPropertyBlock.SetFloat(_propGroundY, num);
			_materialPropertyBlock.SetFloatArray(_propIsPoint, _instanceIsPoint);
			_materialPropertyBlock.SetVectorArray(_propLightData, _instanceLightData);
			_materialPropertyBlock.SetFloatArray(_propRange, _instanceRange);
			_materialPropertyBlock.SetFloatArray(_propAlpha, _instanceAlpha);
			_materialPropertyBlock.SetFloatArray(_propScale, _instanceScale);
			_materialPropertyBlock.SetTexture(_propMainTex, blobShadowTexture);
			_materialPropertyBlock.SetVector(_propMainTexST, new Vector4(1f, 1f, 0f, 0f));
			_isReadyToDraw = true;
		}
	}

	private void UpdateMeshShadows()
	{
		if (_shadowMaterial == null)
		{
			return;
		}
		if (!_spriteMode)
		{
			if (_casterDataList.Count == 0)
			{
				return;
			}
		}
		else if (_spriteTexture == null || _sharedQuadMesh == null)
		{
			return;
		}
		float num = _groundOffset + _groundY;
		_currentInstanceCount = BuildInstanceData(num);
		if (_currentInstanceCount != 0)
		{
			if (!_spriteMode)
			{
				BakeSkinnedMeshes();
			}
			_materialPropertyBlock.SetFloat(_propGroundY, num);
			_materialPropertyBlock.SetFloatArray(_propIsPoint, _instanceIsPoint);
			_materialPropertyBlock.SetVectorArray(_propLightData, _instanceLightData);
			_materialPropertyBlock.SetFloatArray(_propRange, _instanceRange);
			_materialPropertyBlock.SetFloatArray(_propAlpha, _instanceAlpha);
			_materialPropertyBlock.SetFloatArray(_propScale, _instanceScale);
			_isReadyToDraw = true;
		}
	}

	public void ExecuteDraw(CommandBuffer cmd)
	{
		if (_isReadyToDraw)
		{
			if (UseBlobShadow())
			{
				DrawBlobInstanced(cmd);
			}
			else
			{
				DrawAllCastersInstanced(cmd, _currentInstanceCount);
			}
		}
	}

	private int BuildInstanceData(float projectionY)
	{
		int num = 0;
		Light sun = RenderSettings.sun;
		if (sun != null)
		{
			Vector3 forward = sun.transform.forward;
			float globalFloat = Shader.GetGlobalFloat(FakeShadowConsts.SHADER_PROPERTY_SCREEN_SPACE_OBJECT_SHADOW_DIRECTIONAL_ALPHA);
			if (Mathf.Abs(forward.y) > 0.001f && globalFloat > 0.001f)
			{
				float globalFloat2 = Shader.GetGlobalFloat(FakeShadowConsts.SHADER_PROPERTY_SCREEN_SPACE_OBJECT_SHADOW_DIRECTIONAL_LENGTH);
				Vector3 vector = -forward;
				_instanceIsPoint[0] = 0f;
				_instanceLightData[0] = new Vector4(vector.x, vector.y, vector.z, 0f);
				_instanceRange[0] = 0f;
				_instanceAlpha[0] = globalFloat;
				_instanceScale[0] = globalFloat2;
				num = 1;
			}
		}
		FakeShadowManager instance = FakeShadowManager.Instance;
		if (instance != null && !Shader.IsKeywordEnabled("_DE_FAKE_SHADOW_DIRECTIONAL_ONLY"))
		{
			IReadOnlyList<FakeShadowPoint> currentObjectPoints = instance.CurrentObjectPoints;
			int num2 = Mathf.Min(currentObjectPoints.Count, FakeShadowConsts.NUMBER_SHADOW_POINT_COUNT_MAX);
			float globalFloat3 = Shader.GetGlobalFloat(FakeShadowConsts.SHADER_PROPERTY_SCREEN_SPACE_OBJECT_SHADOW_ADDITIONAL_ALPHA);
			if (globalFloat3 > 0.001f)
			{
				float globalFloat4 = Shader.GetGlobalFloat(FakeShadowConsts.SHADER_PROPERTY_SCREEN_SPACE_OBJECT_SHADOW_ADDITIONAL_LENGTH);
				for (int i = 0; i < num2; i++)
				{
					if (num >= 9)
					{
						return num;
					}
					FakeShadowPoint fakeShadowPoint = currentObjectPoints[i];
					if (!(fakeShadowPoint == null))
					{
						_instanceIsPoint[num] = 1f;
						Vector3 position = fakeShadowPoint.Position;
						_instanceLightData[num] = new Vector4(position.x, position.y, position.z, 0f);
						_instanceRange[num] = fakeShadowPoint.CasterRange;
						_instanceAlpha[num] = globalFloat3;
						_instanceScale[num] = globalFloat4;
						num++;
					}
				}
			}
		}
		return num;
	}

	private void BakeSkinnedMeshes()
	{
		for (int i = 0; i < _casterDataList.Count; i++)
		{
			CasterData casterData = _casterDataList[i];
			if (casterData.SkinnedMeshRenderer != null && casterData.Renderer.enabled && casterData.Renderer.gameObject.activeInHierarchy)
			{
				casterData.SkinnedMeshRenderer.BakeMesh(casterData.BakedMesh);
			}
		}
	}

	private void DrawBlobInstanced(CommandBuffer cmd)
	{
		cmd.DrawMeshInstanced(_blobQuadMesh, 0, _shadowMaterial, 1, _instanceMatrices, 1, _materialPropertyBlock);
	}

	private void DrawAllCastersInstanced(CommandBuffer cmd, int instanceCount)
	{
		Matrix4x4 matrix4x = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(_meshRotation), _meshScale);
		for (int i = 0; i < _casterDataList.Count; i++)
		{
			CasterData casterData = _casterDataList[i];
			if (casterData.Renderer == null || !casterData.Renderer.enabled || !casterData.Renderer.gameObject.activeInHierarchy)
			{
				continue;
			}
			Mesh mesh = null;
			Matrix4x4 matrix4x2 = default(Matrix4x4);
			if (casterData.SkinnedMeshRenderer != null)
			{
				mesh = casterData.BakedMesh;
				matrix4x2 = casterData.SkinnedMeshRenderer.localToWorldMatrix;
				Transform rootBone = casterData.SkinnedMeshRenderer.rootBone;
				if (rootBone != null)
				{
					Vector3 lossyScale = rootBone.lossyScale;
					matrix4x2 *= Matrix4x4.Scale(new Vector3((lossyScale.x == 0f) ? 1f : (1f / lossyScale.x), (lossyScale.y == 0f) ? 1f : (1f / lossyScale.y), (lossyScale.z == 0f) ? 1f : (1f / lossyScale.z)));
				}
			}
			else if (casterData.MeshFilter != null && casterData.MeshFilter.sharedMesh != null)
			{
				mesh = casterData.MeshFilter.sharedMesh;
				matrix4x2 = casterData.Renderer.localToWorldMatrix;
			}
			if (mesh == null || mesh.vertexCount == 0)
			{
				continue;
			}
			m_OnSetupMaterialProperties?.Invoke(_materialPropertyBlock, casterData.Renderer);
			Matrix4x4 matrix4x3 = matrix4x2 * matrix4x;
			for (int j = 0; j < instanceCount; j++)
			{
				_instanceMatrices[j] = matrix4x3;
			}
			_materialPropertyBlock.SetVector(_propMainTexST, new Vector4(1f, 1f, 0f, 0f));
			Material[] sharedMaterials = casterData.Renderer.sharedMaterials;
			for (int k = 0; k < mesh.subMeshCount; k++)
			{
				if (k < sharedMaterials.Length && sharedMaterials[k] != null && sharedMaterials[k].mainTexture != null)
				{
					_materialPropertyBlock.SetTexture(_propMainTex, sharedMaterials[k].mainTexture);
				}
				m_OnSetupMaterialProperties?.Invoke(_materialPropertyBlock, casterData.Renderer);
				cmd.DrawMeshInstanced(mesh, k, _shadowMaterial, 1, _instanceMatrices, instanceCount, _materialPropertyBlock);
			}
		}
	}
}
