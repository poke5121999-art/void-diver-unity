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

	private static readonly List<ShaderBufferPoint> _pending = new List<ShaderBufferPoint>();

	[SerializeField]
	private ShaderPointRegistry _definition;

	private readonly Dictionary<string, CategoryBuffer> _table = new Dictionary<string, CategoryBuffer>();

	private readonly Dictionary<string, List<ShaderBufferPoint>> _byCategory = new Dictionary<string, List<ShaderBufferPoint>>();

	private readonly List<Vector4> _scratch = new List<Vector4>();

	private static Vector3 _sortCenter;

	public static ShaderBufferPointManager Instance => _instance;

	private void Awake()
	{
		if (_instance != null && _instance != this)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		_instance = this;
		BuildTable();
		for (int i = 0; i < _pending.Count; i++)
		{
			AddPoint(_pending[i]);
		}
		_pending.Clear();
	}

	private void OnDestroy()
	{
		if (_instance == this)
		{
			_instance = null;
		}
	}

	private void BuildTable()
	{
		_table.Clear();
		if (_definition == null)
		{
			return;
		}
		foreach (ShaderPointRegistry.CategoryDefinition categoryDefinition in _definition.CategoryDefinitions)
		{
			if (categoryDefinition != null && !string.IsNullOrEmpty(categoryDefinition.name) && !_table.ContainsKey(categoryDefinition.name))
			{
				int num = ((categoryDefinition.maxCount < 1) ? 1 : categoryDefinition.maxCount);
				_table[categoryDefinition.name] = new CategoryBuffer
				{
					MaxCount = num,
					Enables = new bool[num],
					Positions = new Vector4[num],
					ShaderProperty = "_GlobalBufferPoints_" + categoryDefinition.name.Replace(" ", "_")
				};
			}
		}
	}

	public int AllocateSlot(string category)
	{
		if (_table.TryGetValue(category, out var value))
		{
			for (int i = 0; i < value.MaxCount; i++)
			{
				if (!value.Enables[i])
				{
					value.Enables[i] = true;
					value.ActiveCount++;
					return i;
				}
			}
		}
		return -1;
	}

	public void ReleaseSlot(string category, int index)
	{
		if (_table.TryGetValue(category, out var value) && index >= 0 && index < value.MaxCount && value.Enables[index])
		{
			value.Enables[index] = false;
			value.Positions[index] = Vector4.zero;
			value.ActiveCount--;
		}
	}

	public void UpdatePosition(string category, int index, Vector3 position, float radius)
	{
		if (_table.TryGetValue(category, out var value) && index >= 0 && index < value.MaxCount)
		{
			value.Positions[index] = new Vector4(position.x, position.y, position.z, radius);
		}
	}

	public void SetCategoryPoints(string category, List<Vector4> points)
	{
		if (!_table.TryGetValue(category, out var value))
		{
			return;
		}
		int num = ((points != null) ? Mathf.Min(points.Count, value.MaxCount) : 0);
		for (int i = 0; i < value.MaxCount; i++)
		{
			if (i < num)
			{
				value.Positions[i] = points[i];
				value.Enables[i] = true;
			}
			else
			{
				value.Positions[i] = Vector4.zero;
				value.Enables[i] = false;
			}
		}
		value.ActiveCount = num;
	}

	public void SendToShader()
	{
		foreach (CategoryBuffer value in _table.Values)
		{
			Shader.SetGlobalVectorArray(value.ShaderProperty, value.Positions);
		}
	}

	public static void Register(ShaderBufferPoint point)
	{
		if (point == null)
		{
			return;
		}
		if (_instance != null)
		{
			_instance.AddPoint(point);
		}
		else
		{
			_pending.Add(point);
		}
	}

	public static void Unregister(ShaderBufferPoint point)
	{
		if (!(point == null))
		{
			if (_instance != null)
			{
				_instance.RemovePoint(point);
			}
			else
			{
				_pending.Remove(point);
			}
		}
	}

	private void AddPoint(ShaderBufferPoint point)
	{
		if (!(point == null) && !string.IsNullOrEmpty(point.Category))
		{
			if (!_byCategory.TryGetValue(point.Category, out var value))
			{
				value = new List<ShaderBufferPoint>();
				_byCategory[point.Category] = value;
			}
			if (!value.Contains(point))
			{
				value.Add(point);
			}
		}
	}

	private void RemovePoint(ShaderBufferPoint point)
	{
		if (!(point == null) && !string.IsNullOrEmpty(point.Category) && _byCategory.TryGetValue(point.Category, out var value))
		{
			value.Remove(point);
		}
	}

	private void LateUpdate()
	{
		_sortCenter = GetCameraGroundCenter();
		foreach (KeyValuePair<string, List<ShaderBufferPoint>> item in _byCategory)
		{
			string key = item.Key;
			List<ShaderBufferPoint> value = item.Value;
			value.Sort(CompareByPlanarDistance);
			_scratch.Clear();
			for (int i = 0; i < value.Count; i++)
			{
				ShaderBufferPoint shaderBufferPoint = value[i];
				if (!(shaderBufferPoint == null))
				{
					Vector3 position = shaderBufferPoint.transform.position;
					Vector3 lossyScale = shaderBufferPoint.transform.lossyScale;
					float w = Mathf.Max(lossyScale.x, Mathf.Max(lossyScale.y, lossyScale.z));
					_scratch.Add(new Vector4(position.x, position.y, position.z, w));
				}
			}
			SetCategoryPoints(key, _scratch);
		}
		SendToShader();
	}

	private static Vector3 GetCameraGroundCenter()
	{
		Camera main = Camera.main;
		if (main == null)
		{
			return Vector3.zero;
		}
		Ray ray = main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
		if (Mathf.Abs(ray.direction.y) > 1E-05f)
		{
			float num = (0f - ray.origin.y) / ray.direction.y;
			if (num > 0f)
			{
				return ray.origin + ray.direction * num;
			}
		}
		Vector3 position = main.transform.position;
		return new Vector3(position.x, 0f, position.z);
	}

	private static int CompareByPlanarDistance(ShaderBufferPoint a, ShaderBufferPoint b)
	{
		if (a == null)
		{
			return (!(b == null)) ? 1 : 0;
		}
		if (b == null)
		{
			return -1;
		}
		return PlanarSqrDistance(a.transform.position).CompareTo(PlanarSqrDistance(b.transform.position));
	}

	private static float PlanarSqrDistance(Vector3 position)
	{
		float num = position.x - _sortCenter.x;
		float num2 = position.z - _sortCenter.z;
		return num * num + num2 * num2;
	}
}
