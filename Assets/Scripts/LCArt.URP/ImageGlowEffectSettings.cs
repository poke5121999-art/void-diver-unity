using UnityEngine;

[CreateAssetMenu(fileName = "Image Glow Effect Settings", menuName = "LCArt/Image Glow Effect Settings")]
public class ImageGlowEffectSettings : ScriptableObject
{
	[SerializeField]
	private Material _material;

	[SerializeField]
	private ComputeShader _computeShader;

	[SerializeField]
	private FilterMode _filterMode;

	[SerializeField]
	private RenderTextureFormat _renderTextureFormat = RenderTextureFormat.RFloat;

	[SerializeField]
	private int _maxDistance = 8;

	[SerializeField]
	private int _padding = 8;

	public Material GlowEffectMaterial => _material;

	public ComputeShader GlowEffectComputeShader => _computeShader;

	public FilterMode GlowEffectFilterMode => _filterMode;

	public RenderTextureFormat GlowEffectFormat => _renderTextureFormat;

	public int MaxDistance => _maxDistance;

	public int Padding => _padding;
}
