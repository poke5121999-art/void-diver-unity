using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class GroundNavigation : MonoBehaviour
{
	[Header("Path Settings")]
	[SerializeField]
	private Transform _startPoint;

	[SerializeField]
	private Transform _endPoint;

	[Header("Ribbon Settings")]
	[SerializeField]
	private float _ribbonWidth = 1f;

	[SerializeField]
	private float _uvTiling = 1f;

	[SerializeField]
	private float _fixedHeight = 0.05f;

	[Range(1f, 8f)]
	[SerializeField]
	private int _segmentCount = 1;

	[SerializeField]
	private bool _smoothCorners = true;

	[SerializeField]
	private float _cutoffStartDistance = 1f;

	[SerializeField]
	private float _cutoffEndDistance;

	[Header("Camera Perspective Correction")]
	[SerializeField]
	private Vector3 _cameraDirection = new Vector3(-1f, 0f, 1f);

	[Range(0f, 2f)]
	[SerializeField]
	private float _uvPerspectiveScale = 1f;

	[Range(0f, 2f)]
	[SerializeField]
	private float _widthPerspectiveScale = 1f;

	[Header("Real-Time Update")]
	[Range(0.01f, 2f)]
	[SerializeField]
	private float _navUpdateInterval = 0.125f;

	private Coroutine _updateCoroutine;

	[Header("References")]
	[SerializeField]
	private MeshFilter _meshFilter;

	private Mesh _ribbonMesh;

	private readonly List<Vector3> _vertices = new List<Vector3>(256);

	private readonly List<int> _triangles = new List<int>(768);

	private readonly List<Vector2> _uvs = new List<Vector2>(256);

	private readonly List<Vector2> _uvs2 = new List<Vector2>(256);

	private readonly List<Vector3> _subdivideVertices = new List<Vector3>(512);

	private readonly List<int> _subdivideTriangles = new List<int>(1536);

	private readonly List<Vector2> _subdivideUVs = new List<Vector2>(512);

	private readonly List<Vector2> _subdivideUVs2 = new List<Vector2>(512);

	private readonly List<Vector3> _pathCornersBuffer = new List<Vector3>(32);

	private readonly List<Vector3> _finalPathBuffer = new List<Vector3>(64);

	private readonly List<Vector3> _cutoffBuffer = new List<Vector3>(64);

	private float[] _visualDistanceToEnd = new float[64];

	private NavMeshPath _navMeshPath;

	private Vector3[] _cornersBuffer = new Vector3[128];

	private readonly List<Vector3> _normals = new List<Vector3>(256);

	private Transform _transform;

	private bool _pathDirty = true;

	private bool _meshDirty = true;

	private Vector3 _lastStartPos;

	private Vector3 _lastEndPos;

	private const float _positionChangeThreshold = 0.001f;

	private const float _sampleMaxDistance = 2f;

	public Transform StartPoint
	{
		get
		{
			return _startPoint;
		}
		set
		{
			_startPoint = value;
			_pathDirty = true;
		}
	}

	public Transform EndPoint
	{
		get
		{
			return _endPoint;
		}
		set
		{
			_endPoint = value;
			_pathDirty = true;
		}
	}

	private void Awake()
	{
		_transform = base.transform;
		_navMeshPath = new NavMeshPath();
	}

	private void OnEnable()
	{
		if (_ribbonMesh != null)
		{
			UnityEngine.Object.Destroy(_ribbonMesh);
		}
		_ribbonMesh = new Mesh();
		_ribbonMesh.name = "Path Ribbon Mesh";
		if (_meshFilter != null)
		{
			_meshFilter.mesh = _ribbonMesh;
		}
		_pathDirty = true;
		_meshDirty = true;
		if (_updateCoroutine != null)
		{
			StopCoroutine(_updateCoroutine);
		}
		_updateCoroutine = StartCoroutine(UpdateCoroutine());
	}

	private void OnDisable()
	{
		if (_updateCoroutine != null)
		{
			StopCoroutine(_updateCoroutine);
			_updateCoroutine = null;
		}
		if (_ribbonMesh != null)
		{
			UnityEngine.Object.Destroy(_ribbonMesh);
			_ribbonMesh = null;
		}
	}

	private void OnDestroy()
	{
		if (_updateCoroutine != null)
		{
			StopCoroutine(_updateCoroutine);
			_updateCoroutine = null;
		}
		if (_ribbonMesh != null)
		{
			UnityEngine.Object.Destroy(_ribbonMesh);
			_ribbonMesh = null;
		}
	}

	private void LateUpdate()
	{
		if (_startPoint == null || _endPoint == null)
		{
			return;
		}
		Vector3 position = _startPoint.position;
		Vector3 position2 = _endPoint.position;
		if ((position - _lastStartPos).sqrMagnitude > 0.001f || (position2 - _lastEndPos).sqrMagnitude > 0.001f)
		{
			_lastStartPos = position;
			_lastEndPos = position2;
			_meshDirty = true;
		}
		if (_meshDirty)
		{
			UpdateMesh();
			_meshDirty = false;
		}
	}

	public void UpdatePathCorner()
	{
		_pathDirty = true;
		_meshDirty = true;
	}

	private void UpdateNavPath()
	{
		_pathCornersBuffer.Clear();
		if (!(_startPoint == null) && !(_endPoint == null) && !(_startPoint == _endPoint))
		{
			Vector3 position = _startPoint.position;
			Vector3 position2 = _endPoint.position;
			if (!((position - position2).sqrMagnitude < 0.001f))
			{
				CalculateNavPath(position, position2);
			}
		}
	}

	private void UpdateMesh()
	{
		if (_ribbonMesh == null)
		{
			return;
		}
		if (_pathDirty)
		{
			_pathCornersBuffer.Clear();
			if (!(_startPoint == null) && !(_endPoint == null) && !(_startPoint == _endPoint))
			{
				Vector3 position = _startPoint.position;
				Vector3 position2 = _endPoint.position;
				if (!((position - position2).sqrMagnitude < 0.001f))
				{
					CalculateNavPath(position, position2);
				}
			}
			_pathDirty = false;
		}
		if (_startPoint == null || _endPoint == null)
		{
			_ribbonMesh.Clear();
			return;
		}
		_finalPathBuffer.Clear();
		_finalPathBuffer.Add(_startPoint.position);
		for (int i = 0; i < _pathCornersBuffer.Count; i++)
		{
			_finalPathBuffer.Add(_pathCornersBuffer[i]);
		}
		_finalPathBuffer.Add(_endPoint.position);
		if (_finalPathBuffer.Count < 2)
		{
			_ribbonMesh.Clear();
			return;
		}
		GenerateMesh();
		if (_segmentCount > 1)
		{
			SubdivideMesh(_segmentCount);
		}
		if (_transform != null)
		{
			_transform.position = Vector3.zero;
			_transform.rotation = Quaternion.identity;
			_transform.localScale = Vector3.one;
		}
	}

	public void EnableRealTimeUpdate()
	{
		if (_updateCoroutine != null)
		{
			StopCoroutine(_updateCoroutine);
		}
		_updateCoroutine = StartCoroutine(UpdateCoroutine());
	}

	public void DisableRealTimeUpdate()
	{
		if (_updateCoroutine != null)
		{
			StopCoroutine(_updateCoroutine);
			_updateCoroutine = null;
		}
	}

	private IEnumerator UpdateCoroutine()
	{
		WaitForSeconds waitForSeconds = new WaitForSeconds(_navUpdateInterval);
		while (true)
		{
			_pathDirty = true;
			_meshDirty = true;
			yield return waitForSeconds;
		}
	}

	private void CalculateNavPath(Vector3 start, Vector3 end)
	{
		NavMeshHit hit;
		bool flag = NavMesh.SamplePosition(start, out hit, 2f, -1);
		NavMeshHit hit2;
		bool flag2 = NavMesh.SamplePosition(end, out hit2, 2f, -1);
		Vector3 sourcePosition = (flag ? hit.position : start);
		Vector3 targetPosition = (flag2 ? hit2.position : end);
		if (!NavMesh.CalculatePath(sourcePosition, targetPosition, -1, _navMeshPath))
		{
			return;
		}
		int num = _navMeshPath.GetCornersNonAlloc(_cornersBuffer);
		while (num == _cornersBuffer.Length)
		{
			_cornersBuffer = new Vector3[_cornersBuffer.Length * 2];
			num = _navMeshPath.GetCornersNonAlloc(_cornersBuffer);
		}
		if (num < 2)
		{
			return;
		}
		if (_navMeshPath.status == NavMeshPathStatus.PathComplete)
		{
			if ((_cornersBuffer[num - 1] - end).sqrMagnitude > 0.01f)
			{
				for (int i = 1; i < num; i++)
				{
					_pathCornersBuffer.Add(_cornersBuffer[i]);
				}
				return;
			}
			if (num - 1 < 2)
			{
				return;
			}
			for (int j = 1; j < num - 1; j++)
			{
				_pathCornersBuffer.Add(_cornersBuffer[j]);
			}
		}
		else if (_navMeshPath.status == NavMeshPathStatus.PathPartial)
		{
			for (int k = 1; k < num; k++)
			{
				_pathCornersBuffer.Add(_cornersBuffer[k]);
			}
		}
	}

	private void SubdivideMesh(int subdivisions)
	{
		if (_ribbonMesh == null || subdivisions < 2 || _vertices.Count < 4)
		{
			return;
		}
		int num = _vertices.Count / 2;
		_subdivideVertices.Clear();
		_subdivideTriangles.Clear();
		_subdivideUVs.Clear();
		_subdivideUVs2.Clear();
		int num2 = (num * 6 - 6) * subdivisions;
		int num3 = (num * 2 - 2) * subdivisions + 2;
		if (_subdivideVertices.Capacity < num3)
		{
			_subdivideVertices.Capacity = num3;
			_subdivideUVs.Capacity = num3;
			_subdivideUVs2.Capacity = num3;
		}
		if (_subdivideTriangles.Capacity < num2)
		{
			_subdivideTriangles.Capacity = num2;
		}
		for (int i = 0; i < num - 1; i++)
		{
			int num4 = 2 + i * 2;
			Vector3 a = _vertices[num4 - 2];
			Vector3 a2 = _vertices[num4 - 1];
			Vector2 vector = _uvs[num4 - 2];
			Vector2 vector2 = _uvs2[num4 - 2];
			Vector3 b = _vertices[num4];
			Vector3 b2 = _vertices[num4 + 1];
			Vector2 vector3 = _uvs[num4];
			Vector2 vector4 = _uvs2[num4];
			for (int j = ((i != 0) ? 1 : 0); j <= subdivisions; j++)
			{
				float t = (float)j / (float)subdivisions;
				_subdivideVertices.Add(Vector3.Lerp(a, b, t));
				_subdivideVertices.Add(Vector3.Lerp(a2, b2, t));
				float y = Mathf.Lerp(vector.y, vector3.y, t);
				_subdivideUVs.Add(new Vector2(0f, y));
				_subdivideUVs.Add(new Vector2(1f, y));
				float y2 = Mathf.Lerp(vector2.y, vector4.y, t);
				_subdivideUVs2.Add(new Vector2(0f, y2));
				_subdivideUVs2.Add(new Vector2(1f, y2));
			}
		}
		int num5 = _subdivideVertices.Count / 2 - 1;
		int num6 = 2;
		for (int k = 0; k < num5; k++)
		{
			_subdivideTriangles.Add(num6 - 2);
			_subdivideTriangles.Add(num6);
			_subdivideTriangles.Add(num6 - 1);
			_subdivideTriangles.Add(num6 - 1);
			_subdivideTriangles.Add(num6);
			_subdivideTriangles.Add(num6 + 1);
			num6 += 2;
		}
		_ribbonMesh.Clear();
		_ribbonMesh.SetVertices(_subdivideVertices);
		_ribbonMesh.SetTriangles(_subdivideTriangles, 0);
		_ribbonMesh.SetUVs(0, _subdivideUVs);
		_ribbonMesh.SetUVs(1, _subdivideUVs2);
		EnsureUpNormals(_subdivideVertices.Count);
		_ribbonMesh.SetNormals(_normals);
		_ribbonMesh.RecalculateBounds();
	}

	private void GenerateMesh()
	{
		if (_ribbonMesh == null)
		{
			return;
		}
		ApplyCutoff();
		int count = _cutoffBuffer.Count;
		if (count < 2)
		{
			_ribbonMesh.Clear();
			return;
		}
		_vertices.Clear();
		_triangles.Clear();
		_uvs.Clear();
		_uvs2.Clear();
		if (_visualDistanceToEnd.Length < count)
		{
			_visualDistanceToEnd = new float[count * 2];
		}
		Vector3 rhs = new Vector3(_cameraDirection.x, 0f, _cameraDirection.z).normalized;
		float num = 0f;
		for (int i = 0; i < count - 1; i++)
		{
			num += Vector3.Distance(_cutoffBuffer[i], _cutoffBuffer[i + 1]);
		}
		float num2 = 0f;
		for (int num3 = count - 1; num3 >= 0; num3--)
		{
			_visualDistanceToEnd[num3] = num2;
			if (num3 > 0)
			{
				Vector3 vector = _cutoffBuffer[num3 - 1] - _cutoffBuffer[num3];
				float magnitude = vector.magnitude;
				Vector3 lhs = new Vector3(vector.x, 0f, vector.z);
				if (lhs.sqrMagnitude > 0.0001f)
				{
					lhs = lhs.normalized;
				}
				float t = Mathf.Clamp01(Mathf.Abs(Vector3.Dot(lhs, rhs)));
				num2 += Mathf.Lerp(1f, _uvPerspectiveScale, t) * magnitude;
			}
		}
		float num4 = 0f;
		for (int j = 0; j < count; j++)
		{
			Vector3 vector2 = _cutoffBuffer[j];
			Vector3 vector3;
			Vector3 vector4;
			if (j == 0)
			{
				vector3 = (_cutoffBuffer[1] - vector2).normalized;
				vector4 = vector3;
			}
			else if (j == count - 1)
			{
				vector3 = (vector2 - _cutoffBuffer[j - 1]).normalized;
				vector4 = vector3;
			}
			else
			{
				Vector3 vector5 = vector2 - _cutoffBuffer[j - 1];
				Vector3 vector6 = _cutoffBuffer[j + 1] - vector2;
				vector3 = vector6.normalized;
				vector4 = (vector6 + vector5).normalized;
			}
			Vector3 lhs2 = new Vector3(vector3.x, 0f, vector3.z);
			if (lhs2.sqrMagnitude > 0.0001f)
			{
				lhs2 = lhs2.normalized;
			}
			float t2 = Mathf.Clamp01(Mathf.Abs(Vector3.Dot(lhs2, rhs)));
			float num5 = Mathf.Lerp(_widthPerspectiveScale, 1f, t2) * _ribbonWidth * 0.5f;
			Vector3 item;
			Vector3 item2;
			if (!_smoothCorners || j < 1 || j >= count - 1)
			{
				Vector3 vector7 = new Vector3(vector3.z, 0f, 0f - vector3.x);
				if (vector7.sqrMagnitude > 0.0001f)
				{
					vector7 = vector7.normalized;
				}
				item = new Vector3(vector2.x - vector7.x * num5, _fixedHeight, vector2.z - vector7.z * num5);
				item2 = new Vector3(vector2.x + vector7.x * num5, _fixedHeight, vector2.z + vector7.z * num5);
			}
			else
			{
				Vector3 normalized = (vector2 - _cutoffBuffer[j - 1]).normalized;
				Vector3 normalized2 = (_cutoffBuffer[j + 1] - vector2).normalized;
				float num6 = normalized2.x * normalized.z - normalized2.z * normalized.x;
				Vector3 vector8 = new Vector3(vector3.z, 0f, 0f - vector3.x);
				Vector3 vector9 = new Vector3(vector4.z, 0f, 0f - vector4.x);
				if (vector8.sqrMagnitude > 0.0001f)
				{
					vector8 = vector8.normalized;
				}
				if (vector9.sqrMagnitude > 0.0001f)
				{
					vector9 = vector9.normalized;
				}
				if (num6 < 0f)
				{
					item = new Vector3(vector2.x - vector8.x * num5, _fixedHeight, vector2.z - vector8.z * num5);
					item2 = new Vector3(vector2.x + vector9.x * num5, _fixedHeight, vector2.z + vector9.z * num5);
				}
				else
				{
					item = new Vector3(vector2.x - vector9.x * num5, _fixedHeight, vector2.z - vector9.z * num5);
					item2 = new Vector3(vector2.x + vector8.x * num5, _fixedHeight, vector2.z + vector8.z * num5);
				}
			}
			_vertices.Add(item);
			_vertices.Add(item2);
			float y = _visualDistanceToEnd[j] * _uvTiling;
			_uvs.Add(new Vector2(0f, y));
			_uvs.Add(new Vector2(1f, y));
			float y2 = ((num > 0f) ? (num4 / num) : 0f);
			_uvs2.Add(new Vector2(0f, y2));
			_uvs2.Add(new Vector2(1f, y2));
			if (j < count - 1)
			{
				num4 += Vector3.Distance(vector2, _cutoffBuffer[j + 1]);
				int num7 = j * 2;
				_triangles.Add(num7);
				_triangles.Add(num7 + 2);
				_triangles.Add(num7 + 1);
				_triangles.Add(num7 + 1);
				_triangles.Add(num7 + 2);
				_triangles.Add(num7 + 3);
			}
		}
		_ribbonMesh.Clear();
		_ribbonMesh.SetVertices(_vertices);
		_ribbonMesh.SetTriangles(_triangles, 0);
		_ribbonMesh.SetUVs(0, _uvs);
		_ribbonMesh.SetUVs(1, _uvs2);
		EnsureUpNormals(_vertices.Count);
		_ribbonMesh.SetNormals(_normals);
		_ribbonMesh.RecalculateBounds();
	}

	private void EnsureUpNormals(int count)
	{
		if (count < _normals.Count)
		{
			_normals.RemoveRange(count, _normals.Count - count);
		}
		while (_normals.Count < count)
		{
			_normals.Add(Vector3.up);
		}
	}

	private void ApplyCutoff()
	{
		_cutoffBuffer.Clear();
		int count = _finalPathBuffer.Count;
		if (count < 2)
		{
			return;
		}
		float num = 0f;
		for (int i = 0; i < count - 1; i++)
		{
			num += Vector3.Distance(_finalPathBuffer[i], _finalPathBuffer[i + 1]);
		}
		if (num <= _cutoffEndDistance + _cutoffStartDistance)
		{
			return;
		}
		float num2 = 0f;
		Vector3 vector = _finalPathBuffer[0];
		int num3 = -1;
		if (_cutoffStartDistance > 0f)
		{
			for (int j = 0; j < count - 1; j++)
			{
				Vector3 a = _finalPathBuffer[j];
				Vector3 b = _finalPathBuffer[j + 1];
				float num4 = Vector3.Distance(a, b);
				if (_cutoffStartDistance <= num2 + num4)
				{
					float t = Mathf.Clamp01((_cutoffStartDistance - num2) / num4);
					vector = Vector3.Lerp(a, b, t);
					num3 = j;
					break;
				}
				num2 += num4;
			}
		}
		int num5 = count - 1;
		Vector3 vector2 = _finalPathBuffer[count - 1];
		float num6 = num - _cutoffEndDistance;
		if (_cutoffEndDistance > 0f)
		{
			float num7 = 0f;
			for (int k = 0; k < count - 1; k++)
			{
				Vector3 a2 = _finalPathBuffer[k];
				Vector3 b2 = _finalPathBuffer[k + 1];
				float num8 = Vector3.Distance(a2, b2);
				if (num6 <= num7 + num8)
				{
					float t2 = Mathf.Clamp01((num6 - num7) / num8);
					vector2 = Vector3.Lerp(a2, b2, t2);
					num5 = k;
					break;
				}
				num7 += num8;
			}
		}
		_cutoffBuffer.Add(vector);
		for (int l = num3 + 1; l < count && l <= num5 && (!(_cutoffEndDistance > 0f) || l != num5 + 1); l++)
		{
			_cutoffBuffer.Add(_finalPathBuffer[l]);
		}
		if (_cutoffBuffer.Count != 0 && (_cutoffBuffer[_cutoffBuffer.Count - 1] - vector2).sqrMagnitude <= 1E-06f)
		{
			return;
		}
		_cutoffBuffer.Add(vector2);
	}
}
