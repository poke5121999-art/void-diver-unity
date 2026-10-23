using System.Linq;
using UnityEngine;

[ExecuteAlways]
public class ShadowMonsterVisualEffect : MonoBehaviour
{
	private readonly int _propertyGhostEnable = Shader.PropertyToID("_GhostEnable");

	private readonly int _propertyIsLocalGhost = Shader.PropertyToID("_IsLocalGhost");

	private readonly int _propertyIsReflected = Shader.PropertyToID("_IsReflected");

	[Header("Shadow Monster Ghost Effect Setting")]
	[SerializeField]
	private bool _isLocalGhost = true;

	[SerializeField]
	private SkeletonMaterialOverrider _skeletonMaterialOverrider;

	private bool _ghostEnable = true;

	private bool _isInitialized;

	private void Start()
	{
		if (Application.isPlaying)
		{
			_isInitialized = true;
			UpdateShadowMonsterVisualEffect();
		}
	}

	private void OnEnable()
	{
		if (!Application.isPlaying)
		{
			ApplyEditorPreview();
		}
		else if (_isInitialized)
		{
			_ghostEnable = true;
			UpdateShadowMonsterVisualEffect();
		}
	}

	private void OnDisable()
	{
		if (!Application.isPlaying)
		{
			ClearEditorPreview();
			return;
		}
		_ghostEnable = false;
		UpdateShadowMonsterVisualEffect();
	}

	private void OnDestroy()
	{
		if (!Application.isPlaying)
		{
			ClearEditorPreview();
			return;
		}
		_ghostEnable = false;
		UpdateShadowMonsterVisualEffect();
	}

	private void UpdateShadowMonsterVisualEffect()
	{
		if (_skeletonMaterialOverrider == null || _skeletonMaterialOverrider.InstnancedMaterials == null)
		{
			return;
		}
		foreach (Material material in _skeletonMaterialOverrider.InstnancedMaterials.Where((Material _) => _ != null))
		{
			material.SetFloat(_propertyGhostEnable, _ghostEnable ? 1f : 0f);
			material.SetFloat(_propertyIsLocalGhost, 1f);
			material.SetFloat(_propertyIsReflected, 0f);
		}
	}

	private void ApplyEditorPreview()
	{
		if (Application.isPlaying)
		{
			return;
		}
		Renderer[] componentsInChildren = GetComponentsInChildren<Renderer>(true);
		if (componentsInChildren == null)
		{
			return;
		}
		MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
		foreach (Renderer renderer in componentsInChildren)
		{
			if (!(renderer == null))
			{
				renderer.GetPropertyBlock(materialPropertyBlock);
				materialPropertyBlock.SetFloat(_propertyGhostEnable, 1f);
				materialPropertyBlock.SetFloat(_propertyIsLocalGhost, _isLocalGhost ? 1f : 0f);
				materialPropertyBlock.SetFloat(_propertyIsReflected, 0f);
				renderer.SetPropertyBlock(materialPropertyBlock);
			}
		}
	}

	private void ClearEditorPreview()
	{
		if (Application.isPlaying)
		{
			return;
		}
		Renderer[] componentsInChildren = GetComponentsInChildren<Renderer>(true);
		if (componentsInChildren == null)
		{
			return;
		}
		foreach (Renderer renderer in componentsInChildren)
		{
			if (!(renderer == null))
			{
				renderer.SetPropertyBlock(null);
			}
		}
	}

	public void SetIsLocalGhost(bool isLocal)
	{
		_isLocalGhost = isLocal;
		UpdateShadowMonsterVisualEffect();
	}

	[ContextMenu("Assign Renderer Components")]
	private void GetRendererComponents()
	{
		_skeletonMaterialOverrider = GetComponentInChildren<SkeletonMaterialOverrider>();
	}
}
