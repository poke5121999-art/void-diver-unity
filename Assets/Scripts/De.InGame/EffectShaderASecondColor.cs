using System;
using UnityEngine;

[ExecuteAlways]
public class EffectShaderASecondColor : MonoBehaviour
{
	[Header("Secondary Color Settings")]
	[GradientUsage(true)]
	public Gradient secondaryGradient;

	private string texturePropertyName = "_SecColorRamp";

	[NonSerialized]
	private Texture2D _rampTexture;

	private ParticleSystemRenderer _renderer;

	private MaterialPropertyBlock _mpb;

	private void OnEnable()
	{
		BakeTexture();
	}

	private void OnValidate()
	{
		BakeTexture();
	}

	private void BakeTexture()
	{
		if (_renderer == null)
		{
			_renderer = GetComponent<ParticleSystemRenderer>();
		}
		if (_renderer == null)
		{
			return;
		}
		if (_mpb == null)
		{
			_mpb = new MaterialPropertyBlock();
		}
		if (_rampTexture == null || _rampTexture.format != TextureFormat.RGBAHalf)
		{
			if (_rampTexture != null)
			{
				if (Application.isPlaying)
				{
					UnityEngine.Object.Destroy(_rampTexture);
				}
				else
				{
					UnityEngine.Object.DestroyImmediate(_rampTexture);
				}
			}
			_rampTexture = new Texture2D(256, 1, TextureFormat.RGBAHalf, false);
			_rampTexture.wrapMode = TextureWrapMode.Clamp;
			_rampTexture.filterMode = FilterMode.Bilinear;
			_rampTexture.name = "Procedural Ramp";
		}
		for (int i = 0; i < 256; i++)
		{
			_rampTexture.SetPixel(i, 0, secondaryGradient.Evaluate((float)i / 255f));
		}
		_rampTexture.Apply();
		_renderer.GetPropertyBlock(_mpb);
		_mpb.SetTexture(texturePropertyName, _rampTexture);
		_renderer.SetPropertyBlock(_mpb);
	}
}
