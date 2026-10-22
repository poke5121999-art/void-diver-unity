using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class CutsceneCharacterMaterialController : MonoBehaviour
{
	private struct OriginalValues
	{
		public Color EffectColor;

		public float Effect;

		public Color DissolveColor;

		public float DissolveProgress;

		public float GhostEnable;
	}

	private static readonly int _effectColorID = Shader.PropertyToID("_EffectColor");

	private static readonly int _effectID = Shader.PropertyToID("_Effect");

	private static readonly int _dissolveColorID = Shader.PropertyToID("_DissolveColor");

	private static readonly int _dissolveProgressID = Shader.PropertyToID("_DissolveProgress");

	private static readonly int _ghostEnableID = Shader.PropertyToID("_GhostEnable");

	private List<Material> _materials;

	private OriginalValues[] _originals;

	private Renderer[] _renderers;

	private MaterialPropertyBlock _mpb;

	private bool _usePropertyBlock;

	private bool _initialized;

	public bool Initialize()
	{
		if (_initialized)
		{
			return true;
		}
		SkeletonMaterialOverrider overrider = GetComponentInChildren<SkeletonMaterialOverrider>();
		if (overrider != null && overrider.InstnancedMaterials != null && overrider.InstnancedMaterials.Count >= 1)
		{
			_materials = overrider.InstnancedMaterials;
			_usePropertyBlock = false;
			_originals = new OriginalValues[_materials.Count];
			for (int i = 0; i < _materials.Count; i++)
			{
				Material material = _materials[i];
				if (!(material == null))
				{
					_originals[i] = new OriginalValues
					{
						EffectColor = (material.HasProperty(_effectColorID) ? material.GetColor(_effectColorID) : Color.white),
						Effect = (material.HasProperty(_effectID) ? material.GetFloat(_effectID) : 0f),
						DissolveColor = (material.HasProperty(_dissolveColorID) ? material.GetColor(_dissolveColorID) : new Color(0f, 0f, 0f, 1f)),
						DissolveProgress = (material.HasProperty(_dissolveProgressID) ? material.GetFloat(_dissolveProgressID) : 0f),
						GhostEnable = (material.HasProperty(_ghostEnableID) ? material.GetFloat(_ghostEnableID) : 0f)
					};
				}
			}
			_initialized = true;
			return true;
		}
		_renderers = GetComponentsInChildren<Renderer>();
		if (_renderers == null || _renderers.Length == 0)
		{
			return false;
		}
		_mpb = new MaterialPropertyBlock();
		_usePropertyBlock = true;
		_initialized = true;
		return true;
	}

	public void ApplyEffect(Color color, float intensity)
	{
		if (_usePropertyBlock)
		{
			ApplyPropertyBlock(_effectColorID, color, _effectID, intensity);
		}
		else
		{
			ApplyMaterial(_effectColorID, color, _effectID, intensity);
		}
	}

	public void ApplyDeath(Color color, float progress)
	{
		if (_usePropertyBlock)
		{
			ApplyPropertyBlock(_dissolveColorID, color, _dissolveProgressID, progress);
		}
		else
		{
			ApplyMaterial(_dissolveColorID, color, _dissolveProgressID, progress);
		}
	}

	public void ApplyGhost(float enable)
	{
		if (_usePropertyBlock)
		{
			ApplyPropertyBlockFloat(_ghostEnableID, enable);
		}
		else
		{
			ApplyMaterialFloat(_ghostEnableID, enable);
		}
	}

	public void ResetEffect()
	{
		if (_usePropertyBlock)
		{
			ResetPropertyBlockPair(_effectColorID, Color.white, _effectID, 0f);
		}
		else if (_materials != null && _originals != null)
		{
			for (int i = 0; i < _materials.Count; i++)
			{
				Material material = _materials[i];
				if (!(material == null))
				{
					material.SetColor(_effectColorID, _originals[i].EffectColor);
					material.SetFloat(_effectID, _originals[i].Effect);
				}
			}
		}
	}

	public void ResetDeath()
	{
		if (_usePropertyBlock)
		{
			ResetPropertyBlockPair(_dissolveColorID, new Color(0f, 0f, 0f, 1f), _dissolveProgressID, 0f);
		}
		else if (_materials != null && _originals != null)
		{
			for (int i = 0; i < _materials.Count; i++)
			{
				Material material = _materials[i];
				if (!(material == null))
				{
					material.SetColor(_dissolveColorID, _originals[i].DissolveColor);
					material.SetFloat(_dissolveProgressID, _originals[i].DissolveProgress);
				}
			}
		}
	}

	public void ResetGhost()
	{
		if (_usePropertyBlock)
		{
			ResetPropertyBlockFloat(_ghostEnableID, 0f);
		}
		else if (_materials != null && _originals != null)
		{
			for (int i = 0; i < _materials.Count; i++)
			{
				Material material = _materials[i];
				if (!(material == null))
				{
					material.SetFloat(_ghostEnableID, _originals[i].GhostEnable);
				}
			}
		}
	}

	private void ApplyMaterial(int colorId, Color color, int floatId, float value)
	{
		if (_materials == null)
		{
			return;
		}
		foreach (Material material in _materials)
		{
			if (!(material == null))
			{
				material.SetColor(colorId, color);
				material.SetFloat(floatId, value);
			}
		}
	}

	private void ApplyMaterialFloat(int floatId, float value)
	{
		if (_materials == null)
		{
			return;
		}
		foreach (Material material in _materials)
		{
			if (!(material == null))
			{
				material.SetFloat(floatId, value);
			}
		}
	}

	private void ApplyPropertyBlock(int colorId, Color color, int floatId, float value)
	{
		if (_renderers == null)
		{
			return;
		}
		for (int i = 0; i < _renderers.Length; i++)
		{
			Renderer renderer = _renderers[i];
			if (!(renderer == null))
			{
				renderer.GetPropertyBlock(_mpb);
				_mpb.SetColor(colorId, color);
				_mpb.SetFloat(floatId, value);
				renderer.SetPropertyBlock(_mpb);
			}
		}
	}

	private void ApplyPropertyBlockFloat(int floatId, float value)
	{
		if (_renderers == null)
		{
			return;
		}
		for (int i = 0; i < _renderers.Length; i++)
		{
			Renderer renderer = _renderers[i];
			if (!(renderer == null))
			{
				renderer.GetPropertyBlock(_mpb);
				_mpb.SetFloat(floatId, value);
				renderer.SetPropertyBlock(_mpb);
			}
		}
	}

	private void ResetPropertyBlockPair(int colorId, Color defaultColor, int floatId, float defaultValue)
	{
		if (_renderers == null)
		{
			return;
		}
		for (int i = 0; i < _renderers.Length; i++)
		{
			Renderer renderer = _renderers[i];
			if (!(renderer == null))
			{
				renderer.GetPropertyBlock(_mpb);
				_mpb.SetColor(colorId, defaultColor);
				_mpb.SetFloat(floatId, defaultValue);
				renderer.SetPropertyBlock(_mpb);
			}
		}
	}

	private void ResetPropertyBlockFloat(int floatId, float defaultValue)
	{
		if (_renderers == null)
		{
			return;
		}
		for (int i = 0; i < _renderers.Length; i++)
		{
			Renderer renderer = _renderers[i];
			if (!(renderer == null))
			{
				renderer.GetPropertyBlock(_mpb);
				_mpb.SetFloat(floatId, defaultValue);
				renderer.SetPropertyBlock(_mpb);
			}
		}
	}
}
