using UnityEngine;

public class BackgroundShaderOverrider : MonoBehaviour
{
	[SerializeField]
	private string _levelName;

	private static readonly int _backgroundTypeId = Shader.PropertyToID("_BackgroundType");

	private static MaterialPropertyBlock _mpb;

	private void Start()
	{
		BackgroundShaderOverrideManager.Register(this);
	}

	public void ApplyOverride()
	{
		BackgroundShaderOverrideTable table = BackgroundShaderOverrideManager.Instance.Table;
		if (!(table == null) && table.TryGetBackgroundType(_levelName, out var type))
		{
			if (!table.UseMaterialPropertyBlock)
			{
				ApplyBackgroundType("LCArt/Background/shader_DE_BG_Base", type);
			}
			else
			{
				ApplyBackgroundTypeMPB("LCArt/Background/shader_DE_BG_Base", type);
			}
		}
	}

	private void ApplyBackgroundType(string baseShaderName, EBackgroundShaderType backgroundType)
	{
		if (string.IsNullOrWhiteSpace(baseShaderName))
		{
			return;
		}
		BackgroundShaderOverrideManager manager = BackgroundShaderOverrideManager.Instance;
		if (manager == null)
		{
			return;
		}
		MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
		if (renderers == null)
		{
			return;
		}
		for (int i = 0; i < renderers.Length; i++)
		{
			MeshRenderer renderer = renderers[i];
			Material[] sharedMaterials = renderer.sharedMaterials;
			if (sharedMaterials == null)
			{
				continue;
			}
			Material[] newMaterials = new Material[sharedMaterials.Length];
			bool changed = false;
			for (int j = 0; j < sharedMaterials.Length; j++)
			{
				Material material = sharedMaterials[j];
				Shader shader = (material != null) ? material.shader : null;
				if (shader != null && shader.name == baseShaderName)
				{
					material = manager.GetOwnedMaterial(material, backgroundType);
					material.SetInt(_backgroundTypeId, (int)backgroundType);
					changed = true;
				}
				newMaterials[j] = material;
			}
			if (changed)
			{
				renderer.sharedMaterials = newMaterials;
			}
		}
	}

	private void ApplyBackgroundTypeMPB(string baseShaderName, EBackgroundShaderType backgroundType)
	{
		if (string.IsNullOrWhiteSpace(baseShaderName))
		{
			return;
		}
		MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
		if (renderers == null)
		{
			return;
		}
		if (_mpb == null)
		{
			_mpb = new MaterialPropertyBlock();
		}
		for (int i = 0; i < renderers.Length; i++)
		{
			MeshRenderer renderer = renderers[i];
			Material[] sharedMaterials = renderer.sharedMaterials;
			if (sharedMaterials == null)
			{
				continue;
			}
			for (int j = 0; j < sharedMaterials.Length; j++)
			{
				Shader shader = (sharedMaterials[j] != null) ? sharedMaterials[j].shader : null;
				if (shader != null && shader.name == baseShaderName)
				{
					renderer.GetPropertyBlock(_mpb);
					_mpb.SetInt(_backgroundTypeId, (int)backgroundType);
					renderer.SetPropertyBlock(_mpb);
					break;
				}
			}
		}
	}

	public void SetLevelName(string levelName)
	{
		_levelName = levelName;
	}
}
