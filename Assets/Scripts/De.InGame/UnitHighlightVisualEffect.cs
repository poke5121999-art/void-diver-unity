using System.Collections.Generic;
using DG.Tweening;
using GameCommon.Enum;
using UnityEngine;

[DefaultExecutionOrder(200)]
public class UnitHighlightVisualEffect : MonoBehaviour
{
	private struct HighlightRendererData
	{
		public Renderer Renderer;

		public MeshFilter MeshFilter;

		public SkinnedMeshRenderer SkinnedMeshRenderer;
	}

	private struct HighlightMaterialSet
	{
		public Material[] GlowlineMaterials;

		public Material SilhouetteMaterial;
	}

	private struct CachedDrawData
	{
		public Renderer Renderer;

		public MeshFilter MeshFilter;

		public SkinnedMeshRenderer SkinnedMeshRenderer;

		public Mesh BakedMesh;

		public Material[] GlowlineMaterials;

		public Material SilhouetteMaterial;
	}

	[Header("Highlight Refs")]
	[SerializeField]
	private Shader _highlightShader;

	[Tooltip("아웃라인 대상 렌더러를 직접 지정합니다. 비어있으면 모든 자식 Renderer에 적용합니다.")]
	[SerializeField]
	private List<GameObject> _targetObjects;

	[SerializeField]
	private List<Renderer> _preCachedRenderers;

	[Header("Highlight Effect Enable")]
	[SerializeField]
	private bool _highlightEnable;

	[Header("Highlight Effect Setting")]
	[SerializeField]
	private EUnitHighlightType _unitHighlightType;

	[SerializeField]
	private EUnitHighlightColor _unitHighlightColor;

	[Header("Glowline Stroke")]
	[Tooltip("Glowline 픽셀 두께 (스크린 스페이스).")]
	[SerializeField]
	[Min(0f)]
	private float _glowlineStrokePixels;

	private static readonly Vector4[] _glowlineOffsets;

	private static readonly int _idMainTex;

	private static readonly int _idColor0;

	private static readonly int _idColor1;

	private static readonly int _idPixelOffset;

	private static readonly int _idSilhouetteScale;

	private static readonly int _idVisibility;

	private static readonly int _idDissolveProgress;

	private static readonly int _idOpticalThreshold;

	private static readonly int _idOpticalPositive;

	private static readonly int _idOpticalNegative;

	private static readonly int _idDeathProgressOnSource;

	private static readonly int _idGhostEnable;

	private static readonly int _idFakeShadowEnable;

	private List<HighlightRendererData> _highlightRenderers;

	private Dictionary<int, HighlightMaterialSet> _materialSets;

	private List<CachedDrawData> _cachedDrawData;

	private Dictionary<SkinnedMeshRenderer, Mesh> _skinnedBakeMeshes;

	private MaterialPropertyBlock _tempMPB;

	private Tween _silhouetteTween;

	private Vector3 _currentSilhouetteScale;

	private bool _isInitialized;

	private void Start()
	{
	}

	private void OnDestroy()
	{
	}

	private void Initialize()
	{
	}

	private void CollectRenderers()
	{
	}

	private void TryAddRenderer(Renderer r)
	{
	}

	private void CreateHighlightMaterials()
	{
	}

	private void BuildDrawCache()
	{
	}

	private void LateUpdate()
	{
	}

	private void ApplyPerFrameProperties(CachedDrawData data, float visibility, float dissolve)
	{
	}

	private float ResolveVisibility(Renderer renderer)
	{
		return 0f;
	}

	private float ResolveFloatProperty(Renderer renderer, int propId, float defaultValue)
	{
		return 0f;
	}

	public void SetHighlightLineEnable(bool enabled)
	{
	}

	public void SetHighlightColorType(EUnitHighlightColor colorType)
	{
	}

	public void SetHighlightSilhouetteType(EUnitHighlightType type)
	{
	}

	private void UpdateGlowlineMaterialProperties()
	{
	}

	private void CopyBillboardProperties(Material source, Material target)
	{
	}

	private Texture GetMainTextureFromMaterial(Material mat)
	{
		return null;
	}

	private void StopSilhouetteEffectAndReset()
	{
	}

	private void PlayHighlightSilhouetteEffect()
	{
	}

	private Color GetHighlightColor(EUnitHighlightColor highlightUnitColor)
	{
		return default;
	}

	private Color GetHighlightColor1(EUnitHighlightColor highlightUnitColor)
	{
		return default;
	}

	[ContextMenu("Cache Renderers")]
	private void CacheRenderers()
	{
	}

	private Renderer[] CollectFromTargets()
	{
		return null;
	}
}
