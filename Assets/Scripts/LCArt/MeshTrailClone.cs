using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class MeshTrailClone : MonoBehaviour
{
	private Vector3 _targetPosition;

	private Quaternion _targetRotation;

	private Vector3 _targetLocalScale;

	private MeshFilter _meshFilter;

	private Mesh _runtimeMesh;

	private static readonly List<Vector3> _sharedVertices;

	private static readonly List<Vector3> _sharedNormals;

	private static readonly List<Vector4> _sharedTangents;

	private static readonly List<Vector2> _sharedUV;

	private static readonly List<int> _sharedTriangles;

	private float _progress;

	private float _duration;

	private Color _startColor;

	private Color _endColor;

	private Color[] _cachedColors;

	private void Awake()
	{
	}

	private void LateUpdate()
	{
	}

	private void KeepWorldTransform()
	{
	}

	private bool IsValidRuntimeMesh()
	{
		return false;
	}

	private bool IsValidVertexColorCaches(int vertexCount)
	{
		return false;
	}

	private void UpdateMesh()
	{
	}

	public void SetCloneMesh(Mesh sourceMesh)
	{
	}

	public void ActiveClone(float period, Color startColor, Color endColor, Vector3 position, Quaternion rotation, Vector3 localScale)
	{
	}

	public void DeactiveClone()
	{
	}
}
