using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
	private float _groundOffset;

	[Header("Mesh Transform")]
	[SerializeField]
	private Vector3 _meshScale;

	[SerializeField]
	private Vector3 _meshRotation;

	[Header("Mesh Source")]
	[Tooltip("직접 메쉬를 지정합니다. 비어있으면 자동 수집합니다.")]
	[SerializeField]
	private MeshRenderer[] _manualMeshRenderers;

	[SerializeField]
	private SkinnedMeshRenderer[] _manualSkinnedMeshRenderers;

	[Header("Options")]
	[SerializeField]
	private bool _includeChildren;

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
	private bool _usePlaneSight;

	[Tooltip("UnifiedShadow + GPU Instancing 켜진 머티리얼 에셋. 빌드 INSTANCING_ON 변종 유지용. 미할당 시 Shader.Find 폴백(빌드 캐스터 누락 위험).")]
	[SerializeField]
	private Material _shadowMaterialSource;

	[Header("Blob Shadow Setting")]
	[SerializeField]
	private float _blobShadowRadius;

	private static readonly List<FakeShadowCaster> _activeCasters;

	private readonly List<CasterData> _casterDataList;

	private static Material _sharedMatPlaneSight;

	private static Material _sharedMatNoPlaneSight;

	private Material _shadowMaterial;

	private MaterialPropertyBlock _materialPropertyBlock;

	private readonly Matrix4x4[] _instanceMatrices;

	private readonly float[] _instanceIsPoint;

	private readonly Vector4[] _instanceLightData;

	private readonly float[] _instanceRange;

	private readonly float[] _instanceAlpha;

	private readonly float[] _instanceScale;

	private int _currentInstanceCount;

	private bool _isReadyToDraw;

	[CompilerGenerated]
	private Action<MaterialPropertyBlock, Renderer> m_OnSetupMaterialProperties;

	private static Mesh _sharedQuadMesh;

	private Texture _spriteTexture;

	private Transform _spriteTransform;

	private Mesh _blobQuadMesh;

	private static readonly int _propIsPoint;

	private static readonly int _propLightData;

	private static readonly int _propGroundY;

	private static readonly int _propRange;

	private static readonly int _propAlpha;

	private static readonly int _propScale;

	private static readonly int _propMainTex;

	private static readonly int _propMainTexST;

	public static IReadOnlyList<FakeShadowCaster> ActiveCasters => null;

	public event Action<MaterialPropertyBlock, Renderer> OnSetupMaterialProperties
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

	private void OnValidate()
	{
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	private void OnDestroy()
	{
	}

	private void ReleaseBlobQuadMesh()
	{
	}

	public void SetSprite(Sprite sprite)
	{
	}

	public void SetSpriteTransform(Transform t)
	{
	}

	private static void EnsureQuadMesh()
	{
	}

	private void EnsureBlobQuadMesh()
	{
	}

	private void CollectRenderers()
	{
	}

	private void CollectManualRenderers()
	{
	}

	private void CollectAutoRenderers()
	{
	}

	private void ReleaseBakedMeshes()
	{
	}

	private void EnsureMaterial()
	{
	}

	private Material GetSharedMaterial(bool planeSight)
	{
		return null;
	}

	private Material CreateSharedMaterial(bool planeSight)
	{
		return null;
	}

	private void LateUpdate()
	{
	}

	private bool UseBlobShadow()
	{
		return false;
	}

	private void UpdateBlobShadow()
	{
	}

	private void UpdateMeshShadows()
	{
	}

	public void ExecuteDraw(CommandBuffer cmd)
	{
	}

	private int BuildInstanceData(float projectionY)
	{
		return 0;
	}

	private void BakeSkinnedMeshes()
	{
	}

	private void DrawBlobInstanced(CommandBuffer cmd)
	{
	}

	private void DrawAllCastersInstanced(CommandBuffer cmd, int instanceCount)
	{
	}
}
