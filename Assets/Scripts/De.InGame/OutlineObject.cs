using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[DefaultExecutionOrder(200)]
public class OutlineObject : MonoBehaviour
{
	private enum EOutlineShaderVariant
	{
		Default = 0,
		Billboard = 1
	}

	private struct OutlineRendererData
	{
		public Renderer Renderer;

		public MeshFilter MeshFilter;

		public SkinnedMeshRenderer SkinnedMeshRenderer;

		public EOutlineShaderVariant Variant;
	}

	private struct OutlineMaterialSet
	{
		public Material StencilMaterial;

		public Material[] OverlayMaterials;
	}

	private struct CachedDrawData
	{
		public Renderer Renderer;

		public MeshFilter MeshFilter;

		public SkinnedMeshRenderer SkinnedMeshRenderer;

		public Mesh BakedMesh;

		public Material StencilMaterial;

		public Material[] OverlayMaterials;
	}

	[Header("Outline Common Setting")]
	[Tooltip("아웃라인 대상의 렌더러 타입을 알려줍니다. 필수 사항은 아니지만, None이 아닌 Mesh 나 Image 중 하나로 명시하면 런타임 동작에 도움이 됩니다.")]
	[SerializeField]
	private EOutlineObjectType _outlineObjectType;

	[Header("Outline Enable")]
	[FormerlySerializedAs("_enable")]
	[SerializeField]
	private bool _outlineEnable;

	[Header("Outline Setting")]
	[SerializeField]
	private Color _color;

	[SerializeField]
	private float _stroke;

	[Header("Outline Z Flatten")]
	[Tooltip("서브메쉬 Z 압축 후 카메라 앞쪽으로의 오프셋 (내부 아웃라인 제거용)")]
	[SerializeField]
	[Min(0f)]
	private float _zOffset;

	[Header("Outline Renderers")]
	[Tooltip("아웃라인을 적용할 게임 오브젝트를 지정합니다. 비어있으면 모든 자식 Renderer에 적용됩니다.")]
	[SerializeField]
	private List<GameObject> _targetObjects;

	[Tooltip("ContextMenu로 미리 캐싱된 Renderer 목록. 있으면 런타임 검색을 건너뜁니다.")]
	[SerializeField]
	private List<Renderer> _preCachedRenderers;

	[SerializeField]
	private List<Image> _images;

	private const string _outlineTargetShaderSettingPath = "Scriptable/Art/Outline/OutlineTargetShaderSettings";

	private static OutlineTargetShaderSettings _cachedTargetSettings;

	private List<OutlineRendererData> _outlineRenderers;

	private Dictionary<(int texId, EOutlineShaderVariant variant), OutlineMaterialSet> _materialSets;

	private List<CachedDrawData> _cachedDrawData;

	private Dictionary<SkinnedMeshRenderer, Mesh> _skinnedBakeMeshes;

	private Dictionary<Image, Material> _instancedImageMaterials;

	private bool _isInitialized;

	private SkeletonMaterialOverrider _skeletonMaterialOverrider;

	private static readonly Vector4[] _offsets;

	private static readonly int _billboardEnableProperty;

	public EOutlineObjectType OutlineObjectType => EOutlineObjectType.None;

	public Color OutlineColor => default;

	public float Stroke => 0f;

	private static OutlineTargetShaderSettings CachedTargetSetting => null;

	private void Start()
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

	private void Initialize()
	{
	}

	private void CollectRendererComponents()
	{
	}

	private void CollectMeshRenderers()
	{
	}

	private void TryAddRenderer(Renderer r)
	{
	}

	private void CollectImages()
	{
	}

	private void InitializeImageMaterials()
	{
	}

	private EOutlineShaderVariant DetectShaderVariant(Renderer renderer)
	{
		return EOutlineShaderVariant.Default;
	}

	private void CreateOutlineMaterials()
	{
	}

	private void BuildDrawCache()
	{
	}

	private void SetBillboardKeyword(Material mat, EOutlineShaderVariant variant)
	{
	}

	private void UpdateFlattenViewZ()
	{
	}

	private void LateUpdate()
	{
	}

	private void UpdateOutline()
	{
	}

	public void SetEnable(bool enable)
	{
	}

	public void SetColor(Color color)
	{
	}

	public void SetStroke(float stroke)
	{
	}

	private void UpdateOutlineMaterialProperties()
	{
	}

	private void SetOutlineEnableOnRenderers(bool enable)
	{
	}

	private void CopyBillboardPropertiesToMaterial(Material source, Material target)
	{
	}

	private Texture GetMainTextureFromMaterial(Material mat)
	{
		return null;
	}

	private Texture GetMainTexture(Renderer renderer, int submeshIndex)
	{
		return null;
	}

	[ContextMenu("Cache Renderers")]
	private void CacheRenderers()
	{
	}

	[ContextMenu("Cache Images")]
	private void CacheImages()
	{
	}

	[ContextMenu("Clear Cache")]
	private void ClearCache()
	{
	}
}
