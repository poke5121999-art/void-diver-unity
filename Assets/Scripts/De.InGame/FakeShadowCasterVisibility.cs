using UnityEngine;

[RequireComponent(typeof(FakeShadowCaster))]
[ExecuteAlways]
public class FakeShadowCasterVisibility : MonoBehaviour
{
	private static readonly int _propDeathProgress = Shader.PropertyToID("_DeathProgress");

	private static readonly int _propGhostEnable = Shader.PropertyToID("_GhostEnable");

	private static readonly int _propFakeShadowEnable = Shader.PropertyToID("_FakeShadowEnable");

	private static readonly int _propVisibility = Shader.PropertyToID("_FakeShadowVisibility");

	private FakeShadowCaster _caster;

	private MaterialPropertyBlock _tempMPB;

	private void OnEnable()
	{
		if (_tempMPB == null)
		{
			_tempMPB = new MaterialPropertyBlock();
		}
		_caster = GetComponent<FakeShadowCaster>();
		if (_caster != null)
		{
			_caster.OnSetupMaterialProperties += SetVisibilityProperties;
		}
	}

	private void OnDisable()
	{
		if (_caster != null)
		{
			_caster.OnSetupMaterialProperties -= SetVisibilityProperties;
		}
	}

	private void SetVisibilityProperties(MaterialPropertyBlock mpb, Renderer renderer)
	{
		if (!(renderer == null))
		{
			renderer.GetPropertyBlock(_tempMPB);
			float rendererFloat = GetRendererFloat(renderer, _propDeathProgress, 0f);
			float rendererFloat2 = GetRendererFloat(renderer, _propGhostEnable, 0f);
			float num = GetRendererFloat(renderer, _propFakeShadowEnable, 1f);
			if (rendererFloat >= 0.5f)
			{
				num = 0f;
			}
			if (rendererFloat2 >= 0.5f)
			{
				num = 0f;
			}
			mpb.SetFloat(_propVisibility, num);
		}
	}

	private float GetRendererFloat(Renderer renderer, int propId, float defaultValue)
	{
		float @float = _tempMPB.GetFloat(propId);
		if (@float != 0f)
		{
			return @float;
		}
		Material sharedMaterial = renderer.sharedMaterial;
		if (sharedMaterial != null && sharedMaterial.HasFloat(propId))
		{
			return sharedMaterial.GetFloat(propId);
		}
		return defaultValue;
	}
}
