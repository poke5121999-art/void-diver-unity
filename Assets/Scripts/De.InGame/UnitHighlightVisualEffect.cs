using System.Collections.Generic;
using System.Linq;
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
	private List<GameObject> _targetObjects = new List<GameObject>();

	[SerializeField]
	private List<Renderer> _preCachedRenderers = new List<Renderer>();

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
	private float _glowlineStrokePixels = 1.5f;

	private static readonly Vector4[] _glowlineOffsets = new Vector4[4]
	{
		new Vector4(1f, 0f, 0f, 0f),
		new Vector4(-1f, 0f, 0f, 0f),
		new Vector4(0f, 1f, 0f, 0f),
		new Vector4(0f, -1f, 0f, 0f)
	};

	private static readonly int _idMainTex = Shader.PropertyToID("_MainTex");

	private static readonly int _idColor0 = Shader.PropertyToID("_HighlightLineColor");

	private static readonly int _idColor1 = Shader.PropertyToID("_HighlightLineColor1");

	private static readonly int _idPixelOffset = Shader.PropertyToID("_HighlightLinePixelOffset");

	private static readonly int _idSilhouetteScale = Shader.PropertyToID("_HighlightSilhouetteScale");

	private static readonly int _idVisibility = Shader.PropertyToID("_FakeShadowVisibility");

	private static readonly int _idDissolveProgress = Shader.PropertyToID("_DissolveProgress");

	private static readonly int _idOpticalThreshold = Shader.PropertyToID("_OpticalRatioYThreshold");

	private static readonly int _idOpticalPositive = Shader.PropertyToID("_OpticalRatioYPositiveRatio");

	private static readonly int _idOpticalNegative = Shader.PropertyToID("_OpticalRatioYNegativeUnit");

	private static readonly int _idDeathProgressOnSource = Shader.PropertyToID("_DeathProgress");

	private static readonly int _idGhostEnable = Shader.PropertyToID("_GhostEnable");

	private static readonly int _idFakeShadowEnable = Shader.PropertyToID("_FakeShadowEnable");

	private List<HighlightRendererData> _highlightRenderers = new List<HighlightRendererData>();

	private Dictionary<int, HighlightMaterialSet> _materialSets = new Dictionary<int, HighlightMaterialSet>();

	private List<CachedDrawData> _cachedDrawData = new List<CachedDrawData>();

	private Dictionary<SkinnedMeshRenderer, Mesh> _skinnedBakeMeshes = new Dictionary<SkinnedMeshRenderer, Mesh>();

	private MaterialPropertyBlock _tempMPB;

	private Tween _silhouetteTween;

	private Vector3 _currentSilhouetteScale = Vector3.one;

	private bool _isInitialized;

	private void Start()
	{
		Initialize();
	}

	private void OnDestroy()
	{
		if (_silhouetteTween != null)
		{
			_silhouetteTween.Kill();
		}
		foreach (HighlightMaterialSet value in _materialSets.Values)
		{
			if (value.GlowlineMaterials != null)
			{
				for (int i = 0; i < value.GlowlineMaterials.Length; i++)
				{
					if (value.GlowlineMaterials[i] != null)
					{
						Object.Destroy(value.GlowlineMaterials[i]);
					}
				}
			}
			if (value.SilhouetteMaterial != null)
			{
				Object.Destroy(value.SilhouetteMaterial);
			}
		}
		_materialSets.Clear();
		foreach (Mesh value2 in _skinnedBakeMeshes.Values)
		{
			if (value2 != null)
			{
				Object.Destroy(value2);
			}
		}
		_skinnedBakeMeshes.Clear();
		_cachedDrawData.Clear();
	}

	private void Initialize()
	{
		if (_tempMPB == null)
		{
			_tempMPB = new MaterialPropertyBlock();
		}
		CollectRenderers();
		CreateHighlightMaterials();
		BuildDrawCache();
		_isInitialized = true;
	}

	private void CollectRenderers()
	{
		_highlightRenderers.Clear();
		if (_preCachedRenderers != null && _preCachedRenderers.Count >= 1)
		{
			foreach (Renderer preCachedRenderer in _preCachedRenderers)
			{
				TryAddRenderer(preCachedRenderer);
			}
		}
		else if (_targetObjects == null || _targetObjects.Count < 1)
		{
			Renderer[] componentsInChildren = GetComponentsInChildren<Renderer>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				TryAddRenderer(componentsInChildren[i]);
			}
		}
		else
		{
			foreach (GameObject targetObject in _targetObjects)
			{
				if (!(targetObject == null))
				{
					Renderer[] components = targetObject.GetComponents<Renderer>();
					for (int j = 0; j < components.Length; j++)
					{
						TryAddRenderer(components[j]);
					}
				}
			}
		}
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
			_highlightRenderers.Add(new HighlightRendererData
			{
				Renderer = r,
				MeshFilter = component,
				SkinnedMeshRenderer = skinnedMeshRenderer
			});
		}
	}

	private void CreateHighlightMaterials()
	{
		if (_highlightShader == null)
		{
			return;
		}
		foreach (HighlightRendererData highlightRenderer in _highlightRenderers)
		{
			if (highlightRenderer.Renderer == null)
			{
				continue;
			}
			Material[] sharedMaterials = highlightRenderer.Renderer.sharedMaterials;
			for (int i = 0; i < sharedMaterials.Length; i++)
			{
				Texture mainTextureFromMaterial = GetMainTextureFromMaterial(sharedMaterials[i]);
				if (mainTextureFromMaterial == null)
				{
					continue;
				}
				int instanceID = mainTextureFromMaterial.GetInstanceID();
				if (_materialSets.ContainsKey(instanceID))
				{
					continue;
				}
				Material[] array = new Material[4];
				for (int j = 0; j < 4; j++)
				{
					Material material = new Material(_highlightShader);
					material.hideFlags = (HideFlags)52;
					array[j] = material;
					array[j].SetTexture(_idMainTex, mainTextureFromMaterial);
					array[j].SetVector(_idPixelOffset, _glowlineOffsets[j] * _glowlineStrokePixels);
					CopyBillboardProperties(sharedMaterials[i], array[j]);
				}
				Material material2 = new Material(_highlightShader);
				material2.hideFlags = (HideFlags)52;
				material2.SetTexture(_idMainTex, mainTextureFromMaterial);
				CopyBillboardProperties(sharedMaterials[i], material2);
				_materialSets[instanceID] = new HighlightMaterialSet
				{
					GlowlineMaterials = array,
					SilhouetteMaterial = material2
				};
			}
		}
		UpdateGlowlineMaterialProperties();
	}

	private void BuildDrawCache()
	{
		_cachedDrawData.Clear();
		foreach (HighlightRendererData highlightRenderer in _highlightRenderers)
		{
			if (highlightRenderer.Renderer == null)
			{
				continue;
			}
			Mesh mesh;
			if (highlightRenderer.MeshFilter != null)
			{
				mesh = highlightRenderer.MeshFilter.sharedMesh;
			}
			else
			{
				if (!(highlightRenderer.SkinnedMeshRenderer != null))
				{
					continue;
				}
				mesh = highlightRenderer.SkinnedMeshRenderer.sharedMesh;
			}
			if (mesh == null)
			{
				continue;
			}
			Mesh value = null;
			if (highlightRenderer.SkinnedMeshRenderer != null && !_skinnedBakeMeshes.TryGetValue(highlightRenderer.SkinnedMeshRenderer, out value))
			{
				value = new Mesh();
				_skinnedBakeMeshes[highlightRenderer.SkinnedMeshRenderer] = value;
			}
			Material[] sharedMaterials = highlightRenderer.Renderer.sharedMaterials;
			for (int i = 0; i < mesh.subMeshCount; i++)
			{
				Texture texture = ((i < sharedMaterials.Length) ? GetMainTextureFromMaterial(sharedMaterials[i]) : null);
				if (!(texture == null) && _materialSets.TryGetValue(texture.GetInstanceID(), out var value2))
				{
					_cachedDrawData.Add(new CachedDrawData
					{
						Renderer = highlightRenderer.Renderer,
						MeshFilter = highlightRenderer.MeshFilter,
						SkinnedMeshRenderer = highlightRenderer.SkinnedMeshRenderer,
						BakedMesh = value,
						GlowlineMaterials = value2.GlowlineMaterials,
						SilhouetteMaterial = value2.SilhouetteMaterial
					});
				}
			}
		}
	}

	private void LateUpdate()
	{
		if (!_isInitialized || !_highlightEnable || _cachedDrawData.Count == 0)
		{
			return;
		}
		foreach (KeyValuePair<SkinnedMeshRenderer, Mesh> skinnedBakeMesh in _skinnedBakeMeshes)
		{
			SkinnedMeshRenderer key = skinnedBakeMesh.Key;
			if (key != null && key.enabled && key.gameObject.activeInHierarchy)
			{
				key.BakeMesh(skinnedBakeMesh.Value);
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
			if (cachedDrawData.SkinnedMeshRenderer != null)
			{
				matrix = cachedDrawData.Renderer.transform.localToWorldMatrix;
				Transform rootBone = cachedDrawData.SkinnedMeshRenderer.rootBone;
				if (rootBone != null)
				{
					Vector3 lossyScale = rootBone.lossyScale;
					matrix *= Matrix4x4.Scale(new Vector3((lossyScale.x == 0f) ? 1f : (1f / lossyScale.x), (lossyScale.y == 0f) ? 1f : (1f / lossyScale.y), (lossyScale.z == 0f) ? 1f : (1f / lossyScale.z)));
				}
			}
			else
			{
				matrix = cachedDrawData.Renderer.transform.localToWorldMatrix;
			}
			int layer = cachedDrawData.Renderer.gameObject.layer;
			float visibility = ResolveVisibility(cachedDrawData.Renderer);
			float dissolve = ResolveFloatProperty(cachedDrawData.Renderer, _idDissolveProgress, 0f);
			ApplyPerFrameProperties(cachedDrawData, visibility, dissolve);
			int subMeshCount = mesh.subMeshCount;
			for (int j = 0; j < subMeshCount; j++)
			{
				for (int k = 0; k < 4; k++)
				{
					Graphics.DrawMesh(mesh, matrix, cachedDrawData.GlowlineMaterials[k], layer, null, j);
				}
				Graphics.DrawMesh(mesh, matrix, cachedDrawData.SilhouetteMaterial, layer, null, j);
			}
		}
	}

	private void ApplyPerFrameProperties(CachedDrawData data, float visibility, float dissolve)
	{
		for (int i = 0; i < 4; i++)
		{
			data.GlowlineMaterials[i].SetFloat(_idVisibility, visibility);
			data.GlowlineMaterials[i].SetFloat(_idDissolveProgress, dissolve);
		}
		if (data.SilhouetteMaterial != null)
		{
			data.SilhouetteMaterial.SetFloat(_idVisibility, visibility);
			data.SilhouetteMaterial.SetFloat(_idDissolveProgress, dissolve);
			data.SilhouetteMaterial.SetVector(_idSilhouetteScale, new Vector4(_currentSilhouetteScale.x, _currentSilhouetteScale.y, _currentSilhouetteScale.z, 1f));
		}
	}

	private float ResolveVisibility(Renderer renderer)
	{
		float num = ResolveFloatProperty(renderer, _idDeathProgressOnSource, 0f);
		float num2 = ResolveFloatProperty(renderer, _idGhostEnable, 0f);
		float result = ResolveFloatProperty(renderer, _idFakeShadowEnable, 1f);
		if (0.5f <= num)
		{
			result = 0f;
		}
		if (0.5f <= num2)
		{
			result = 0f;
		}
		return result;
	}

	private float ResolveFloatProperty(Renderer renderer, int propId, float defaultValue)
	{
		renderer.GetPropertyBlock(_tempMPB);
		float @float = _tempMPB.GetFloat(propId);
		if (@float == 0f)
		{
			Material sharedMaterial = renderer.sharedMaterial;
			if (sharedMaterial != null && sharedMaterial.HasFloat(propId))
			{
				return sharedMaterial.GetFloat(propId);
			}
			return defaultValue;
		}
		return @float;
	}

	public void SetHighlightLineEnable(bool enabled)
	{
		_highlightEnable = enabled;
		if (enabled)
		{
			PlayHighlightSilhouetteEffect();
			return;
		}
		if (_silhouetteTween != null)
		{
			_silhouetteTween.Kill();
		}
		_currentSilhouetteScale = Vector3.one;
	}

	public void SetHighlightColorType(EUnitHighlightColor colorType)
	{
		_unitHighlightColor = colorType;
		UpdateGlowlineMaterialProperties();
	}

	public void SetHighlightSilhouetteType(EUnitHighlightType type)
	{
		_unitHighlightType = type;
	}

	private void UpdateGlowlineMaterialProperties()
	{
		Color highlightColor = GetHighlightColor(_unitHighlightColor);
		Color highlightColor2 = GetHighlightColor1(_unitHighlightColor);
		foreach (HighlightMaterialSet value in _materialSets.Values)
		{
			Material[] glowlineMaterials = value.GlowlineMaterials;
			if (glowlineMaterials != null)
			{
				for (int i = 0; i < glowlineMaterials.Length; i++)
				{
					if (!(glowlineMaterials[i] == null))
					{
						glowlineMaterials[i].SetColor(_idColor0, highlightColor);
						glowlineMaterials[i].SetColor(_idColor1, highlightColor2);
						glowlineMaterials[i].SetVector(_idPixelOffset, _glowlineOffsets[i] * _glowlineStrokePixels);
					}
				}
			}
			if (value.SilhouetteMaterial != null)
			{
				value.SilhouetteMaterial.SetColor(_idColor0, highlightColor);
				value.SilhouetteMaterial.SetColor(_idColor1, highlightColor2);
			}
		}
	}

	private void CopyBillboardProperties(Material source, Material target)
	{
		if (!(source == null) && !(target == null))
		{
			if (source.HasFloat(_idOpticalThreshold))
			{
				target.SetFloat(_idOpticalThreshold, source.GetFloat(_idOpticalThreshold));
			}
			if (source.HasFloat(_idOpticalPositive))
			{
				target.SetFloat(_idOpticalPositive, source.GetFloat(_idOpticalPositive));
			}
			if (source.HasFloat(_idOpticalNegative))
			{
				target.SetFloat(_idOpticalNegative, source.GetFloat(_idOpticalNegative));
			}
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

	private void StopSilhouetteEffectAndReset()
	{
		if (_silhouetteTween != null)
		{
			_silhouetteTween.Kill();
		}
		_currentSilhouetteScale = Vector3.one;
	}

	private void PlayHighlightSilhouetteEffect()
	{
		if (_silhouetteTween != null)
		{
			_silhouetteTween.Kill();
		}
		float x = 2.5f;
		float y = 2.5f;
		float endX = 1f;
		float duration = 0.2f;
		Ease ease = Ease.InCubic;
		switch (_unitHighlightType)
		{
		case EUnitHighlightType.None:
			x = 1f;
			y = 1f;
			duration = 0f;
			break;
		case EUnitHighlightType.Burst:
			x = 1.75f;
			y = 1.75f;
			duration = 0.1f;
			break;
		case EUnitHighlightType.LargeBurst:
			x = 2f;
			y = 2f;
			duration = 0.1f;
			break;
		case EUnitHighlightType.Spark:
			x = 0.2f;
			y = 5f;
			endX = 1.25f;
			duration = 0.25f;
			ease = Ease.OutCubic;
			break;
		case EUnitHighlightType.Squeeze:
			x = 7.5f;
			y = 0.125f;
			ease = Ease.InOutCubic;
			break;
		}
		Vector3 anim = new Vector3(x, y, 1f);
		_currentSilhouetteScale = anim;
		_silhouetteTween = DOTween.To(() => anim, delegate(Vector3 v)
		{
			anim = v;
			_currentSilhouetteScale = v;
		}, new Vector3(endX, 1f, 1f), duration).SetEase(ease).OnComplete(delegate
		{
			_currentSilhouetteScale = Vector3.one;
		});
	}

	private Color GetHighlightColor(EUnitHighlightColor highlightUnitColor)
	{
		switch (highlightUnitColor)
		{
		case EUnitHighlightColor.Orange:
			return new Color(1f, 0.7f, 0.2f, 1f);
		case EUnitHighlightColor.Pink:
			return new Color(1f, 0.3f, 0.6f, 1f);
		case EUnitHighlightColor.Blue:
			return new Color(0.3f, 0.65f, 1f, 1f);
		case EUnitHighlightColor.Green:
			return new Color(0.3f, 1f, 0.3f, 1f);
		default:
			return new Color(1f, 0.1f, 0.1f, 1f);
		}
	}

	private Color GetHighlightColor1(EUnitHighlightColor highlightUnitColor)
	{
		switch (highlightUnitColor)
		{
		case EUnitHighlightColor.Red:
			return new Color(1f, 0.5f, 0f, 1f);
		case EUnitHighlightColor.Orange:
			return new Color(1f, 0.4f, 0.1f, 1f);
		case EUnitHighlightColor.Pink:
			return new Color(1f, 0f, 0.3f, 1f);
		case EUnitHighlightColor.Blue:
			return new Color(0f, 0.5f, 1f, 1f);
		case EUnitHighlightColor.Green:
			return new Color(0.5f, 1f, 0.5f, 1f);
		default:
			return new Color(1f, 0.4f, 0f, 1f);
		}
	}

	[ContextMenu("Cache Renderers")]
	private void CacheRenderers()
	{
		_preCachedRenderers.Clear();
		Renderer[] array = ((_targetObjects == null || _targetObjects.Count < 1) ? GetComponentsInChildren<Renderer>(true) : CollectFromTargets());
		for (int i = 0; i < array.Length; i++)
		{
			Renderer renderer = array[i];
			if (!(renderer == null) && (renderer.GetComponent<MeshFilter>() != null || renderer is SkinnedMeshRenderer))
			{
				_preCachedRenderers.Add(renderer);
			}
		}
	}

	private Renderer[] CollectFromTargets()
	{
		List<Renderer> list = new List<Renderer>();
		foreach (GameObject targetObject in _targetObjects)
		{
			if (!(targetObject == null))
			{
				list.AddRange(targetObject.GetComponents<Renderer>());
			}
		}
		return list.ToArray();
	}
}
