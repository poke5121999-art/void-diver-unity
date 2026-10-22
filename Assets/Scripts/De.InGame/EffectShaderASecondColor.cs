using System;
using UnityEngine;

[ExecuteAlways]
public class EffectShaderASecondColor : MonoBehaviour
{
	[Header("Secondary Color Settings")]
	[GradientUsage(true)]
	public Gradient secondaryGradient;

	private string texturePropertyName;

	[NonSerialized]
	private Texture2D _rampTexture;

	private ParticleSystemRenderer _renderer;

	private MaterialPropertyBlock _mpb;

	private void OnEnable()
	{
	}

	private void OnValidate()
	{
	}

	private void BakeTexture()
	{
	}
}
