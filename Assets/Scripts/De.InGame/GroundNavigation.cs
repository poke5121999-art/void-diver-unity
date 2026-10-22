using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class GroundNavigation : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CUpdateCoroutine_003Ed__54 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public GroundNavigation _003C_003E4__this;

		private WaitForSeconds _003CwaitForSeconds_003E5__2;

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return null;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return null;
			}
		}

		[DebuggerHidden]
		public _003CUpdateCoroutine_003Ed__54(int _003C_003E1__state)
		{
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
		}
	}

	[Header("Path Settings")]
	[SerializeField]
	private Transform _startPoint;

	[SerializeField]
	private Transform _endPoint;

	[Header("Ribbon Settings")]
	[SerializeField]
	private float _ribbonWidth;

	[SerializeField]
	private float _uvTiling;

	[SerializeField]
	private float _fixedHeight;

	[Range(1f, 8f)]
	[SerializeField]
	private int _segmentCount;

	[SerializeField]
	private bool _smoothCorners;

	[SerializeField]
	private float _cutoffStartDistance;

	[SerializeField]
	private float _cutoffEndDistance;

	[Header("Camera Perspective Correction")]
	[SerializeField]
	private Vector3 _cameraDirection;

	[Range(0f, 2f)]
	[SerializeField]
	private float _uvPerspectiveScale;

	[Range(0f, 2f)]
	[SerializeField]
	private float _widthPerspectiveScale;

	[Header("Real-Time Update")]
	[Range(0.01f, 2f)]
	[SerializeField]
	private float _navUpdateInterval;

	private Coroutine _updateCoroutine;

	[Header("References")]
	[SerializeField]
	private MeshFilter _meshFilter;

	private Mesh _ribbonMesh;

	private readonly List<Vector3> _vertices;

	private readonly List<int> _triangles;

	private readonly List<Vector2> _uvs;

	private readonly List<Vector2> _uvs2;

	private readonly List<Vector3> _subdivideVertices;

	private readonly List<int> _subdivideTriangles;

	private readonly List<Vector2> _subdivideUVs;

	private readonly List<Vector2> _subdivideUVs2;

	private readonly List<Vector3> _pathCornersBuffer;

	private readonly List<Vector3> _finalPathBuffer;

	private readonly List<Vector3> _cutoffBuffer;

	private float[] _visualDistanceToEnd;

	private NavMeshPath _navMeshPath;

	private Vector3[] _cornersBuffer;

	private readonly List<Vector3> _normals;

	private Transform _transform;

	private bool _pathDirty;

	private bool _meshDirty;

	private Vector3 _lastStartPos;

	private Vector3 _lastEndPos;

	private const float _positionChangeThreshold = 0.001f;

	private const float _sampleMaxDistance = 2f;

	public Transform StartPoint
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public Transform EndPoint
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	private void Awake()
	{
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	private void OnDestroy()
	{
	}

	private void LateUpdate()
	{
	}

	public void UpdatePathCorner()
	{
	}

	private void UpdateNavPath()
	{
	}

	private void UpdateMesh()
	{
	}

	public void EnableRealTimeUpdate()
	{
	}

	public void DisableRealTimeUpdate()
	{
	}

	[IteratorStateMachine(typeof(_003CUpdateCoroutine_003Ed__54))]
	private IEnumerator UpdateCoroutine()
	{
		return null;
	}

	private void CalculateNavPath(Vector3 start, Vector3 end)
	{
	}

	private void SubdivideMesh(int subdivisions)
	{
	}

	private void GenerateMesh()
	{
	}

	private void EnsureUpNormals(int count)
	{
	}

	private void ApplyCutoff()
	{
	}
}
