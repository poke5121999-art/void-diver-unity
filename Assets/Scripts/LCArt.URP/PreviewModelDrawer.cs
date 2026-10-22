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
		public static readonly int Surface;

		public static readonly int SrcBlend;

		public static readonly int DstBlend;

		public static readonly int ZWrite;

		public static readonly int ZTest;

		public static readonly int Mode;

		public static readonly int BaseColor;

		public static readonly int Color;

		public static readonly int Cull;
	}

	[Header("Preview Model Settings")]
	public GameObject PreviewObject;

	[Header("Transform Override")]
	public Vector3 PositionOffset;

	public Vector3 RotationOffset;

	public Vector3 ScaleMultiplier;

	[Range(0f, 1f)]
	public float Alpha;

	public Color ColorTint;

	public bool ShowWireframe;

	public Color WireframeColor;

	public bool ShowBound;

	public Color BoundColor;

	private List<RenderData> _cachedRenderData;

	private Material _wireframeMaterial;

	private float _lastAlpha;

	private Color _lastColorTint;

	private GameObject _lastPreviewModel;
}
