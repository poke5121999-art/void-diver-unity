using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
[RequireComponent(typeof(CanvasRenderer))]
public class UINodeLink : MaskableGraphic
{
	[SerializeField]
	private bool _isActivated;

	[Header("Active Settings")]
	public Color activeColor;

	public Material activeMaterial;

	[Header("Inactive Settings")]
	public Color inactiveColor;

	public Material inactiveMaterial;

	[Header("Connections")]
	public RectTransform sourceNodeTf;

	public RectTransform targetNodeTf;

	[Header("Line Settings")]
	public float lineThickness;

	public float cornerRadius;

	[Range(0f, 1f)]
	public float turnOffsetRatio;

	public int curveSegments;

	[Header("Material & UV Settings")]
	[Tooltip("텍스처가 반복되는 밀도를 조절합니다. 점선 간격을 맞출 때 사용하세요.")]
	public float uvTiling;

	private Vector3 _lastSourceWorldPos;

	private Vector3 _lastTargetWorldPos;

	private Vector3 _lastSelfWorldPos;

	public bool IsActivated
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	private void ApplyState()
	{
	}

	protected override void OnEnable()
	{
	}

	private void LateUpdate()
	{
	}

	protected override void OnPopulateMesh(VertexHelper vh)
	{
	}

	private List<Vector2> CalculatePath(Vector2 start, Vector2 end)
	{
		return null;
	}

	private void AddBezierCurve(List<Vector2> points, Vector2 p0, Vector2 p1, Vector2 p2)
	{
	}

	private void DrawLine(VertexHelper vh, List<Vector2> points)
	{
	}
}
