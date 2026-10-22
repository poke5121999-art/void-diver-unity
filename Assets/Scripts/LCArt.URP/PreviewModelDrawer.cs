using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class PreviewModelDrawer : MonoBehaviour
{
	private class RenderData
	{
		public Mesh Mesh;

		public Material Material;

		public Material OriginalMaterial;

		public Matrix4x4 LocalMatrix;
	}

	private static class ShaderIDs
	{
		public static readonly int Surface = Shader.PropertyToID("_Surface");

		public static readonly int SrcBlend = Shader.PropertyToID("_SrcBlend");

		public static readonly int DstBlend = Shader.PropertyToID("_DstBlend");

		public static readonly int ZWrite = Shader.PropertyToID("_ZWrite");

		public static readonly int ZTest = Shader.PropertyToID("_ZTest");

		public static readonly int Mode = Shader.PropertyToID("_Mode");

		public static readonly int BaseColor = Shader.PropertyToID("_BaseColor");

		public static readonly int Color = Shader.PropertyToID("_Color");

		public static readonly int Cull = Shader.PropertyToID("_Cull");
	}

	[Header("Preview Model Settings")]
	public GameObject PreviewObject;

	[Header("Transform Override")]
	public Vector3 PositionOffset = Vector3.zero;

	public Vector3 RotationOffset = Vector3.zero;

	public Vector3 ScaleMultiplier = Vector3.one;

	[Range(0f, 1f)]
	public float Alpha = 0.5f;

	public Color ColorTint = Color.white;

	public bool ShowWireframe = true;

	public Color WireframeColor = new Color(0f, 1f, 1f, 1f);

	public bool ShowBound = true;

	public Color BoundColor = new Color(1f, 1f, 0f, 0.1f);

	private List<RenderData> _cachedRenderData = new List<RenderData>();

	private Material _wireframeMaterial;

	private float _lastAlpha;

	private Color _lastColorTint;

	private GameObject _lastPreviewModel;
}
