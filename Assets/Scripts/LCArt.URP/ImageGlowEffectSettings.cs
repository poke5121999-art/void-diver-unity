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
	private RenderTextureFormat _renderTextureFormat;

	[SerializeField]
	private int _maxDistance;

	[SerializeField]
	private int _padding;

	public Material GlowEffectMaterial => null;

	public ComputeShader GlowEffectComputeShader => null;

	public FilterMode GlowEffectFilterMode => FilterMode.Point;

	public RenderTextureFormat GlowEffectFormat => RenderTextureFormat.ARGB32;

	public int MaxDistance => 0;

	public int Padding => 0;
}
