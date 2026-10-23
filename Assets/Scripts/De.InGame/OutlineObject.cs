using System.Collections.Generic;
using System.Linq;
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
	private Color _color = Color.white;

	[SerializeField]
	private float _stroke = 1f;

	[Header("Outline Z Flatten")]
	[Tooltip("서브메쉬 Z 압축 후 카메라 앞쪽으로의 오프셋 (내부 아웃라인 제거용)")]
	[SerializeField]
	[Min(0f)]
	private float _zOffset = 0.1f;

	[Header("Outline Renderers")]
	[Tooltip("아웃라인을 적용할 게임 오브젝트를 지정합니다. 비어있으면 모든 자식 Renderer에 적용됩니다.")]
	[SerializeField]
	private List<GameObject> _targetObjects = new List<GameObject>();

	[Tooltip("ContextMenu로 미리 캐싱된 Renderer 목록. 있으면 런타임 검색을 건너뜁니다.")]
	[SerializeField]
	private List<Renderer> _preCachedRenderers = new List<Renderer>();

	[SerializeField]
	private List<Image> _images = new List<Image>();

	private const string _outlineTargetShaderSettingPath = "Scriptable/Art/Outline/OutlineTargetShaderSettings";

	private static OutlineTargetShaderSettings _cachedTargetSettings;

	private List<OutlineRendererData> _outlineRenderers = new List<OutlineRendererData>();

	private Dictionary<(int texId, EOutlineShaderVariant variant), OutlineMaterialSet> _materialSets = new Dictionary<(int texId, EOutlineShaderVariant variant), OutlineMaterialSet>();

	private List<CachedDrawData> _cachedDrawData = new List<CachedDrawData>();

	private Dictionary<SkinnedMeshRenderer, Mesh> _skinnedBakeMeshes = new Dictionary<SkinnedMeshRenderer, Mesh>();

	private Dictionary<Image, Material> _instancedImageMaterials = new Dictionary<Image, Material>();

	private bool _isInitialized;

	private SkeletonMaterialOverrider _skeletonMaterialOverrider;

	private static readonly Vector4[] _offsets = new Vector4[4]
	{
		new Vector4(0f, 1f, 0f, 0f),
		new Vector4(0f, -1f, 0f, 0f),
		new Vector4(-1f, 0f, 0f, 0f),
		new Vector4(1f, 0f, 0f, 0f)
	};

	private static readonly int _billboardEnableProperty = Shader.PropertyToID("_BillboardEnable");

	public EOutlineObjectType OutlineObjectType => _outlineObjectType;

	public Color OutlineColor => _color;

	public float Stroke => _stroke;

	private static OutlineTargetShaderSettings CachedTargetSetting
	{
		get
		{
			if (_cachedTargetSettings == null)
			{
				_cachedTargetSettings = Resources.Load<OutlineTargetShaderSettings>("Scriptable/Art/Outline/OutlineTargetShaderSettings");
				return _cachedTargetSettings;
			}
			return _cachedTargetSettings;
		}
	}

	private void Start()
	{
		Initialize();
	}

	private void OnEnable()
	{
		UpdateOutline();
		SetOutlineEnableOnRenderers(_outlineEnable);
	}

	private void OnDisable()
	{
		UpdateOutline();
		SetOutlineEnableOnRenderers(enable: false);
	}

	private void OnDestroy()
	{
		SetOutlineEnableOnRenderers(enable: false);
		foreach (OutlineMaterialSet value in _materialSets.Values)
		{
			if (value.StencilMaterial != null)
			{
				Object.Destroy(value.StencilMaterial);
			}
			if (value.OverlayMaterials != null)
			{
				Material[] overlayMaterials = value.OverlayMaterials;
				for (int i = 0; i < overlayMaterials.Length; i++)
				{
					if (overlayMaterials[i] != null)
					{
						Object.Destroy(overlayMaterials[i]);
					}
				}
			}
		}
		_materialSets.Clear();
		foreach (Material value2 in _instancedImageMaterials.Values)
		{
			if (value2 != null)
			{
				Object.Destroy(value2);
			}
		}
		_instancedImageMaterials.Clear();
		foreach (Mesh value3 in _skinnedBakeMeshes.Values)
		{
			if (value3 != null)
			{
				Object.Destroy(value3);
			}
		}
		_skinnedBakeMeshes.Clear();
		_cachedDrawData.Clear();
	}

	private void Initialize()
	{
		CollectRendererComponents();
		CreateOutlineMaterials();
		InitializeImageMaterials();
		BuildDrawCache();
		_isInitialized = true;
		UpdateOutline();
		SetOutlineEnableOnRenderers(_outlineEnable);
	}

	private void CollectRendererComponents()
	{
		switch (_outlineObjectType)
		{
		case EOutlineObjectType.None:
			CollectMeshRenderers();
			CollectImages();
			break;
		case EOutlineObjectType.Mesh:
			CollectMeshRenderers();
			break;
		case EOutlineObjectType.Image:
			CollectImages();
			break;
		}
	}

	private void CollectMeshRenderers()
	{
		_outlineRenderers.Clear();
		if (_preCachedRenderers != null && _preCachedRenderers.Count > 0)
		{
			foreach (Renderer preCachedRenderer in _preCachedRenderers)
			{
				TryAddRenderer(preCachedRenderer);
			}
		}
		else if (_targetObjects != null && _targetObjects.Count > 0)
		{
			foreach (GameObject targetObject in _targetObjects)
			{
				if (targetObject == null)
				{
					continue;
				}
				Renderer[] components = targetObject.GetComponents<Renderer>();
				for (int i = 0; i < components.Length; i++)
				{
					TryAddRenderer(components[i]);
				}
			}
		}
		else
		{
			Renderer[] componentsInChildren = GetComponentsInChildren<Renderer>(includeInactive: true);
			for (int j = 0; j < componentsInChildren.Length; j++)
			{
				TryAddRenderer(componentsInChildren[j]);
			}
		}
		_skeletonMaterialOverrider = GetComponentInChildren<SkeletonMaterialOverrider>();
	}

	private void TryAddRenderer(Renderer r)
	{
		if (r == null)
		{
			return;
		}
		MeshFilter component = r.GetComponent<MeshFilter>();
		SkinnedMeshRenderer skinnedMeshRenderer = r as SkinnedMeshRenderer;
		if (!(component == null) || !(skinnedMeshRenderer == null))
		{
			_outlineRenderers.Add(new OutlineRendererData
			{
				Renderer = r,
				MeshFilter = component,
				SkinnedMeshRenderer = skinnedMeshRenderer,
				Variant = DetectShaderVariant(r)
			});
		}
	}

	private void CollectImages()
	{
		if (_images != null && _images.Count <= 0)
		{
			_images.Clear();
			_images = GetComponentsInChildren<Image>(includeInactive: true).ToList();
		}
	}

	private void InitializeImageMaterials()
	{
		if (_instancedImageMaterials != null)
		{
			foreach (Material value in _instancedImageMaterials.Values)
			{
				if (value != null)
				{
					Object.Destroy(value);
				}
			}
			_instancedImageMaterials.Clear();
		}
		if (_images == null)
		{
			return;
		}
		foreach (Image image in _images)
		{
			if (!(image == null))
			{
				Material material = new Material(image.material);
				material.hideFlags = HideFlags.DontSave;
				image.material = material;
				_instancedImageMaterials[image] = material;
			}
		}
	}

	private EOutlineShaderVariant DetectShaderVariant(Renderer renderer)
	{
		if (renderer == null || renderer.sharedMaterial == null)
		{
			return EOutlineShaderVariant.Default;
		}
		Material sharedMaterial = renderer.sharedMaterial;
		if (sharedMaterial.shader.name.Contains("Character"))
		{
			return EOutlineShaderVariant.Billboard;
		}
		if (sharedMaterial.HasProperty(_billboardEnableProperty) && sharedMaterial.GetFloat(_billboardEnableProperty) > 0.5f)
		{
			return EOutlineShaderVariant.Billboard;
		}
		return EOutlineShaderVariant.Default;
	}

	private void CreateOutlineMaterials()
	{
		Shader shader = Shader.Find(OutlineConsts.SHADER_NAME_STENCIL_WRITE);
		Shader shader2 = Shader.Find(OutlineConsts.SHADER_NAME_OVERLAY);
		if (shader == null || shader2 == null)
		{
			return;
		}
		OutlineTargetShaderSettings cachedTargetSetting = CachedTargetSetting;
		foreach (OutlineRendererData outlineRenderer in _outlineRenderers)
		{
			if (outlineRenderer.Renderer == null)
			{
				continue;
			}
			Material[] sharedMaterials = outlineRenderer.Renderer.sharedMaterials;
			EOutlineShaderVariant variant = outlineRenderer.Variant;
			for (int i = 0; i < sharedMaterials.Length; i++)
			{
				if (cachedTargetSetting != null && !cachedTargetSetting.IsEnableTargetShader(sharedMaterials[i].shader))
				{
					continue;
				}
				Texture mainTextureFromMaterial = GetMainTextureFromMaterial(sharedMaterials[i]);
				if (mainTextureFromMaterial == null)
				{
					continue;
				}
				(int, EOutlineShaderVariant) key = (mainTextureFromMaterial.GetInstanceID(), variant);
				if (_materialSets.ContainsKey(key))
				{
					continue;
				}
				Material material = new Material(shader);
				material.hideFlags = HideFlags.DontSave;
				SetBillboardKeyword(material, variant);
				material.SetTexture(OutlineConsts.SHADER_PROPERTY_MAIN_TEX, mainTextureFromMaterial);
				if (variant == EOutlineShaderVariant.Billboard)
				{
					CopyBillboardPropertiesToMaterial(sharedMaterials[i], material);
				}
				Material[] array = new Material[4];
				for (int j = 0; j < 4; j++)
				{
					Material material2 = new Material(shader2);
					material2.hideFlags = HideFlags.DontSave;
					array[j] = material2;
					SetBillboardKeyword(array[j], variant);
					array[j].SetTexture(OutlineConsts.SHADER_PROPERTY_MAIN_TEX, mainTextureFromMaterial);
					array[j].SetColor(OutlineConsts.SHADER_PROPERTY_OBJECT_COLOR, _color);
					array[j].SetVector(OutlineConsts.SHADER_PROPERTY_PIXEL_OFFSET, _offsets[j] * _stroke);
					if (variant == EOutlineShaderVariant.Billboard)
					{
						CopyBillboardPropertiesToMaterial(sharedMaterials[i], array[j]);
					}
				}
				_materialSets[key] = new OutlineMaterialSet
				{
					StencilMaterial = material,
					OverlayMaterials = array
				};
			}
		}
	}

	private void BuildDrawCache()
	{
		_cachedDrawData.Clear();
		foreach (OutlineRendererData outlineRenderer in _outlineRenderers)
		{
			if (outlineRenderer.Renderer == null)
			{
				continue;
			}
			Mesh mesh;
			if (outlineRenderer.MeshFilter != null)
			{
				mesh = outlineRenderer.MeshFilter.sharedMesh;
			}
			else
			{
				if (!(outlineRenderer.SkinnedMeshRenderer != null))
				{
					continue;
				}
				mesh = outlineRenderer.SkinnedMeshRenderer.sharedMesh;
			}
			if (mesh == null)
			{
				continue;
			}
			Mesh value = null;
			if (outlineRenderer.SkinnedMeshRenderer != null && !_skinnedBakeMeshes.TryGetValue(outlineRenderer.SkinnedMeshRenderer, out value))
			{
				value = new Mesh();
				_skinnedBakeMeshes[outlineRenderer.SkinnedMeshRenderer] = value;
			}
			Material[] sharedMaterials = outlineRenderer.Renderer.sharedMaterials;
			for (int i = 0; i < mesh.subMeshCount; i++)
			{
				Texture texture = (i < sharedMaterials.Length) ? GetMainTextureFromMaterial(sharedMaterials[i]) : null;
				if (!(texture == null) && _materialSets.TryGetValue((texture.GetInstanceID(), outlineRenderer.Variant), out var value2))
				{
					_cachedDrawData.Add(new CachedDrawData
					{
						Renderer = outlineRenderer.Renderer,
						MeshFilter = outlineRenderer.MeshFilter,
						SkinnedMeshRenderer = outlineRenderer.SkinnedMeshRenderer,
						BakedMesh = value,
						StencilMaterial = value2.StencilMaterial,
						OverlayMaterials = value2.OverlayMaterials
					});
				}
			}
		}
	}

	private void SetBillboardKeyword(Material mat, EOutlineShaderVariant variant)
	{
		if (!(mat == null))
		{
			if (variant == EOutlineShaderVariant.Billboard)
			{
				mat.EnableKeyword("_BILLBOARD");
			}
			else
			{
				mat.DisableKeyword("_BILLBOARD");
			}
		}
	}

	private void UpdateFlattenViewZ()
	{
		Camera main = Camera.main;
		if (main == null)
		{
			return;
		}
		Matrix4x4 worldToCameraMatrix = main.worldToCameraMatrix;
		Vector3 position = base.transform.position;
		float value = _zOffset + (position.y * worldToCameraMatrix.m21 + position.x * worldToCameraMatrix.m20 + position.z * worldToCameraMatrix.m22 + worldToCameraMatrix.m23);
		if (_materialSets == null)
		{
			return;
		}
		foreach (OutlineMaterialSet value2 in _materialSets.Values)
		{
			value2.StencilMaterial.SetFloat(OutlineConsts.SHADER_PROPERTY_FLATTEN_VIEW_Z, value);
		}
	}

	private void LateUpdate()
	{
		if (!_isInitialized || _cachedDrawData == null || _cachedDrawData.Count == 0)
		{
			return;
		}
		UpdateFlattenViewZ();
		if (_skinnedBakeMeshes != null)
		{
			foreach (KeyValuePair<SkinnedMeshRenderer, Mesh> skinnedBakeMesh in _skinnedBakeMeshes)
			{
				if (skinnedBakeMesh.Key != null && skinnedBakeMesh.Key.enabled && skinnedBakeMesh.Key.gameObject.activeInHierarchy)
				{
					skinnedBakeMesh.Key.BakeMesh(skinnedBakeMesh.Value);
				}
			}
		}
		for (int i = 0; i < _cachedDrawData.Count; i++)
		{
			CachedDrawData cachedDrawData = _cachedDrawData[i];
			if (cachedDrawData.Renderer == null || !cachedDrawData.Renderer.enabled || !cachedDrawData.Renderer.gameObject.activeInHierarchy || !cachedDrawData.Renderer.isVisible)
			{
				continue;
			}
			Mesh mesh;
			if (cachedDrawData.SkinnedMeshRenderer != null)
			{
				mesh = cachedDrawData.BakedMesh;
			}
			else
			{
				if (!(cachedDrawData.MeshFilter != null))
				{
					continue;
				}
				mesh = cachedDrawData.MeshFilter.sharedMesh;
			}
			if (mesh == null)
			{
				continue;
			}
			Matrix4x4 matrix;
			if (cachedDrawData.SkinnedMeshRenderer == null)
			{
				matrix = cachedDrawData.Renderer.transform.localToWorldMatrix;
			}
			else
			{
				matrix = cachedDrawData.Renderer.transform.localToWorldMatrix;
				Transform rootBone = cachedDrawData.SkinnedMeshRenderer.rootBone;
				if (rootBone != null)
				{
					Vector3 lossyScale = rootBone.lossyScale;
					Vector3 s = new Vector3(
						(lossyScale.x == 0f) ? 1f : (1f / lossyScale.x),
						(lossyScale.y == 0f) ? 1f : (1f / lossyScale.y),
						(lossyScale.z == 0f) ? 1f : (1f / lossyScale.z));
					matrix *= Matrix4x4.Scale(s);
				}
			}
			int layer = cachedDrawData.Renderer.gameObject.layer;
			int subMeshCount = mesh.subMeshCount;
			for (int j = 0; j < subMeshCount; j++)
			{
				Graphics.DrawMesh(mesh, matrix, cachedDrawData.StencilMaterial, layer, null, j);
				if (_outlineEnable)
				{
					for (int k = 0; k < 4; k++)
					{
						Graphics.DrawMesh(mesh, matrix, cachedDrawData.OverlayMaterials[k], layer, null, j);
					}
				}
			}
		}
	}

	private void UpdateOutline()
	{
		if (_isInitialized && (_outlineObjectType == EOutlineObjectType.None || _outlineObjectType == EOutlineObjectType.Image) && _images != null)
		{
			foreach (Image item in _images.Where((Image img) => img != null))
			{
				if (_instancedImageMaterials.TryGetValue(item, out var value))
				{
					value.SetFloat(OutlineConsts.SHADER_PROPERTY_OBJECT_ENABLE, _outlineEnable ? 1f : 0f);
					value.SetColor(OutlineConsts.SHADER_PROPERTY_OBJECT_COLOR, _color);
					value.SetFloat(OutlineConsts.SHADER_PROPERTY_OBJECT_STROKE, _stroke / OutlineConsts.NUMBER_STROKE_MAX);
				}
			}
		}
	}

	public void SetEnable(bool enable)
	{
		_outlineEnable = enable;
		UpdateOutline();
	}

	public void SetColor(Color color)
	{
		_color = color;
		UpdateOutline();
		UpdateOutlineMaterialProperties();
	}

	public void SetStroke(float stroke)
	{
		_stroke = Mathf.Clamp(stroke, 0f, OutlineConsts.NUMBER_STROKE_MAX);
		UpdateOutline();
		UpdateOutlineMaterialProperties();
	}

	private void UpdateOutlineMaterialProperties()
	{
		foreach (OutlineMaterialSet value in _materialSets.Values)
		{
			if (value.OverlayMaterials == null)
			{
				continue;
			}
			for (int i = 0; i < value.OverlayMaterials.Length; i++)
			{
				Material material = value.OverlayMaterials[i];
				if (!(material == null))
				{
					material.SetColor(OutlineConsts.SHADER_PROPERTY_OBJECT_COLOR, _color);
					material.SetVector(OutlineConsts.SHADER_PROPERTY_PIXEL_OFFSET, _offsets[i] * _stroke);
				}
			}
		}
	}

	private void SetOutlineEnableOnRenderers(bool enable)
	{
		if (!(_skeletonMaterialOverrider == null))
		{
			float value = enable ? 1f : 0f;
			_skeletonMaterialOverrider.InstnancedMaterials.ForEach(delegate(Material m)
			{
				if (!(m == null))
				{
					m.SetFloat(OutlineConsts.SHADER_PROPERTY_OBJECT_ENABLE, value);
				}
			});
		}
	}

	private void CopyBillboardPropertiesToMaterial(Material source, Material target)
	{
		if (!(source == null) && source.HasProperty(OutlineConsts.SHADER_PROPERTY_OPTICAL_Y_THRESHOLD))
		{
			target.SetFloat(OutlineConsts.SHADER_PROPERTY_OPTICAL_Y_THRESHOLD, source.GetFloat(OutlineConsts.SHADER_PROPERTY_OPTICAL_Y_THRESHOLD));
			target.SetFloat(OutlineConsts.SHADER_PROPERTY_OPTICAL_Y_POSITIVE, source.GetFloat(OutlineConsts.SHADER_PROPERTY_OPTICAL_Y_POSITIVE));
			target.SetFloat(OutlineConsts.SHADER_PROPERTY_OPTICAL_Y_NEGATIVE, source.GetFloat(OutlineConsts.SHADER_PROPERTY_OPTICAL_Y_NEGATIVE));
		}
	}

	private Texture GetMainTextureFromMaterial(Material mat)
	{
		if (mat == null)
		{
			return null;
		}
		if (mat.HasTexture("_BaseMap"))
		{
			return mat.GetTexture("_BaseMap");
		}
		if (mat.HasTexture("_MainTex"))
		{
			return mat.GetTexture("_MainTex");
		}
		return mat.mainTexture;
	}

	private Texture GetMainTexture(Renderer renderer, int submeshIndex)
	{
		Material[] sharedMaterials = renderer.sharedMaterials;
		if (submeshIndex >= sharedMaterials.Length)
		{
			return null;
		}
		return GetMainTextureFromMaterial(sharedMaterials[submeshIndex]);
	}

	[ContextMenu("Cache Renderers")]
	private void CacheRenderers()
	{
		_preCachedRenderers.Clear();
		if (_targetObjects != null && _targetObjects.Count > 0)
		{
			foreach (GameObject targetObject in _targetObjects)
			{
				if (targetObject == null)
				{
					continue;
				}
				Renderer[] components = targetObject.GetComponents<Renderer>();
				foreach (Renderer renderer in components)
				{
					if (!(renderer == null) && (renderer.GetComponent<MeshFilter>() != null || renderer is SkinnedMeshRenderer))
					{
						_preCachedRenderers.Add(renderer);
					}
				}
			}
			return;
		}
		Renderer[] componentsInChildren = GetComponentsInChildren<Renderer>(includeInactive: true);
		foreach (Renderer renderer2 in componentsInChildren)
		{
			if (!(renderer2 == null) && (renderer2.GetComponent<MeshFilter>() != null || renderer2 is SkinnedMeshRenderer))
			{
				_preCachedRenderers.Add(renderer2);
			}
		}
	}

	[ContextMenu("Cache Images")]
	private void CacheImages()
	{
		_images.Clear();
		_images = GetComponentsInChildren<Image>(includeInactive: true).ToList();
	}

	[ContextMenu("Clear Cache")]
	private void ClearCache()
	{
		_preCachedRenderers.Clear();
		_images.Clear();
	}
}
