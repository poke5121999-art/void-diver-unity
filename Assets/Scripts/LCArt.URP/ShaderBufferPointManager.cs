using System.Collections.Generic;
using UnityEngine;

public class ShaderBufferPointManager : MonoBehaviour
{
	private class CategoryBuffer
	{
		public int MaxCount;

		public bool[] Enables;

		public Vector4[] Positions;

		public int ActiveCount;

		public string ShaderProperty;
	}

	private static ShaderBufferPointManager _instance;

	private static readonly List<ShaderBufferPoint> _pending;

	[SerializeField]
	private ShaderPointRegistry _definition;

	private readonly Dictionary<string, CategoryBuffer> _table;

	private readonly Dictionary<string, List<ShaderBufferPoint>> _byCategory;

	private readonly List<Vector4> _scratch;

	private static Vector3 _sortCenter;

	public static ShaderBufferPointManager Instance => null;

	private void Awake()
	{
	}

	private void OnDestroy()
	{
	}

	private void BuildTable()
	{
	}

	public int AllocateSlot(string category)
	{
		return 0;
	}

	public void ReleaseSlot(string category, int index)
	{
	}

	public void UpdatePosition(string category, int index, Vector3 position, float radius)
	{
	}

	public void SetCategoryPoints(string category, List<Vector4> points)
	{
	}

	public void SendToShader()
	{
	}

	public static void Register(ShaderBufferPoint point)
	{
	}

	public static void Unregister(ShaderBufferPoint point)
	{
	}

	private void AddPoint(ShaderBufferPoint point)
	{
	}

	private void RemovePoint(ShaderBufferPoint point)
	{
	}

	private void LateUpdate()
	{
	}

	private static Vector3 GetCameraGroundCenter()
	{
		return default;
	}

	private static int CompareByPlanarDistance(ShaderBufferPoint a, ShaderBufferPoint b)
	{
		return 0;
	}

	private static float PlanarSqrDistance(Vector3 position)
	{
		return 0f;
	}
}
