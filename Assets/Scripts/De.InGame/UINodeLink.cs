using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
[RequireComponent(typeof(CanvasRenderer))]
public class UINodeLink : MaskableGraphic
{
	[SerializeField]
	private bool _isActivated = true;

	[Header("Active Settings")]
	public Color activeColor = Color.white;

	public Material activeMaterial;

	[Header("Inactive Settings")]
	public Color inactiveColor = Color.gray;

	public Material inactiveMaterial;

	[Header("Connections")]
	public RectTransform sourceNodeTf;

	public RectTransform targetNodeTf;

	[Header("Line Settings")]
	public float lineThickness = 8f;

	public float cornerRadius = 80f;

	[Range(0f, 1f)]
	public float turnOffsetRatio = 0.5f;

	public int curveSegments = 10;

	[Header("Material & UV Settings")]
	[Tooltip("텍스처가 반복되는 밀도를 조절합니다. 점선 간격을 맞출 때 사용하세요.")]
	public float uvTiling = 0.01f;

	private Vector3 _lastSourceWorldPos;

	private Vector3 _lastTargetWorldPos;

	private Vector3 _lastSelfWorldPos;

	public bool IsActivated
	{
		get
		{
			return _isActivated;
		}
		set
		{
			if (_isActivated != value)
			{
				_isActivated = value;
				ApplyState();
			}
		}
	}

	private void ApplyState()
	{
		color = (_isActivated ? activeColor : inactiveColor);
		material = (_isActivated ? activeMaterial : inactiveMaterial);
		SetMaterialDirty();
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		ApplyState();
		_lastSelfWorldPos = (_lastTargetWorldPos = (_lastSourceWorldPos = Vector3.positiveInfinity));
		SetVerticesDirty();
	}

	private void LateUpdate()
	{
		if (!(sourceNodeTf == null) && !(targetNodeTf == null))
		{
			Vector3 position = sourceNodeTf.position;
			Vector3 position2 = targetNodeTf.position;
			Vector3 position3 = base.rectTransform.position;
			if (!(position == _lastSourceWorldPos) || !(position2 == _lastTargetWorldPos) || !(position3 == _lastSelfWorldPos))
			{
				_lastSourceWorldPos = position;
				_lastTargetWorldPos = position2;
				_lastSelfWorldPos = position3;
				SetVerticesDirty();
			}
		}
	}

	protected override void OnPopulateMesh(VertexHelper vh)
	{
		vh.Clear();
		if (!(sourceNodeTf == null) && !(targetNodeTf == null))
		{
			Vector2 start = base.rectTransform.InverseTransformPoint(sourceNodeTf.position);
			Vector2 end = base.rectTransform.InverseTransformPoint(targetNodeTf.position);
			List<Vector2> points = CalculatePath(start, end);
			DrawLine(vh, points);
		}
	}

	private List<Vector2> CalculatePath(Vector2 start, Vector2 end)
	{
		List<Vector2> list = new List<Vector2>();
		if (Mathf.Abs(start.x - end.x) < 1f)
		{
			list.Add(start);
			list.Add(end);
			return list;
		}
		float num = Mathf.Lerp(start.y, end.y, turnOffsetRatio);
		list.Add(start);
		float num2 = Mathf.Min(cornerRadius, Mathf.Abs(num - start.y), Mathf.Abs(end.x - start.x) * 0.5f);
		if (num2 > 0.1f)
		{
			float num3 = ((start.y < num) ? (0f - num2) : num2);
			float num4 = ((end.x > start.x) ? num2 : (0f - num2));
			AddBezierCurve(list, new Vector2(start.x, num + num3), new Vector2(start.x, num), new Vector2(start.x + num4, num));
			float num5 = ((start.x < end.x) ? (0f - num2) : num2);
			float num6 = ((end.y <= num) ? (0f - num2) : num2);
			AddBezierCurve(list, new Vector2(end.x + num5, num), new Vector2(end.x, num), new Vector2(end.x, num + num6));
			list.Add(end);
			return list;
		}
		list.Add(new Vector2(start.x, num));
		list.Add(new Vector2(end.x, num));
		list.Add(end);
		return list;
	}

	private void AddBezierCurve(List<Vector2> points, Vector2 p0, Vector2 p1, Vector2 p2)
	{
		for (int i = 0; i <= curveSegments; i++)
		{
			float num = (float)i / (float)curveSegments;
			float num2 = Mathf.Pow(1f - num, 2f);
			float num3 = 2f * (1f - num) * num;
			float num4 = Mathf.Pow(num, 2f);
			points.Add(p0 * num2 + p1 * num3 + p2 * num4);
		}
	}

	private void DrawLine(VertexHelper vh, List<Vector2> points)
	{
		if (points.Count < 2)
		{
			return;
		}
		UIVertex simpleVert = UIVertex.simpleVert;
		simpleVert.color = color;
		float num = 0f;
		float num2 = lineThickness * 0.5f;
		int currentVertCount = vh.currentVertCount;
		for (int i = 0; i < points.Count; i++)
		{
			Vector2 vector = points[i];
			Vector2 vector2;
			if (i == 0)
			{
				Vector2 normalized = (points[1] - points[0]).normalized;
				vector2 = new Vector2(0f - normalized.y, normalized.x);
			}
			else if (i == points.Count - 1)
			{
				Vector2 normalized2 = (points[i] - points[i - 1]).normalized;
				vector2 = new Vector2(0f - normalized2.y, normalized2.x);
				num += Vector2.Distance(vector, points[i - 1]);
			}
			else
			{
				Vector2 normalized3 = (vector - points[i - 1]).normalized;
				Vector2 normalized4 = (points[i + 1] - vector).normalized;
				Vector2 normalized5 = (normalized3 + normalized4).normalized;
				if (normalized5 == Vector2.zero)
				{
					normalized5 = normalized3;
				}
				float num3 = Mathf.Clamp(Vector2.Dot(normalized5, normalized3), 0.1f, 1f);
				vector2 = new Vector2((0f - normalized5.y) / num3, normalized5.x / num3);
				num += Vector2.Distance(vector, points[i - 1]);
			}
			float x = num * uvTiling;
			simpleVert.position = new Vector3(vector.x + vector2.x * num2, vector.y + vector2.y * num2, 0f);
			simpleVert.uv0 = new Vector4(x, 1f, 0f, 0f);
			vh.AddVert(simpleVert);
			simpleVert.position = new Vector3(vector.x - vector2.x * num2, vector.y - vector2.y * num2, 0f);
			simpleVert.uv0 = new Vector4(x, 0f, 0f, 0f);
			vh.AddVert(simpleVert);
			if (i > 0)
			{
				int num4 = currentVertCount + i * 2;
				vh.AddTriangle(num4 - 2, num4 - 1, num4);
				vh.AddTriangle(num4, num4 - 1, num4 + 1);
			}
		}
	}
}
