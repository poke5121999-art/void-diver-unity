using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class IndicatorBoxPostVisualCorrector : MonoBehaviour
{
	[SerializeField]
	private Transform _sourceTransform;

	[SerializeField]
	private float _meshSize = 1f;

	[SerializeField]
	private float _gabDistance = 0.1f;

	[SerializeField]
	private float _patternWorldScale = 1f;

	[SerializeField]
	private Transform _outerTransform;

	[SerializeField]
	private Transform _innerTransform;

	[SerializeField]
	private float _cornerHideThreshold = 0.5f;

	[SerializeField]
	private List<Transform> _cornerPatterns = new List<Transform>();

	[SerializeField]
	private Transform _trianglePattern;

	[SerializeField]
	private float _triangleWidthAtMinScale = 0.5f;

	[SerializeField]
	private float _triangleWidthAtMaxScale = 5f;

	[SerializeField]
	private float _triangleMinScale = 0.5f;

	[SerializeField]
	private float _triangleMaxScale = 1.5f;

	[SerializeField]
	private List<Transform> _etcPatterns = new List<Transform>();

	[SerializeField]
	private Renderer _ringRenderer;

	private MaterialPropertyBlock _mpb;

	private float _lastLineScaling = float.NaN;

	private float _lastLengthScaling = float.NaN;

	private float _lastGabDistance = float.NaN;

	private static readonly int _lineScalingId = Shader.PropertyToID("_LineScaling");

	private static readonly int _lengthScalingId = Shader.PropertyToID("_LengthScaling");

	private static readonly int _gabDistanceId = Shader.PropertyToID("_GabDistance");

	private const float _epsilon = 0.0001f;

	private void LateUpdate()
	{
		if (!(_meshSize > 0.0001f))
		{
			return;
		}
		Transform transform = ((_sourceTransform != null) ? _sourceTransform : base.transform);
		float x = transform.localScale.x;
		float z = transform.localScale.z;
		if (x > 0.0001f)
		{
			SetWorldLossyScaleX(_outerTransform, x / _meshSize);
			SetWorldLossyScaleX(_innerTransform, Mathf.Max(x - _gabDistance * 2f, 0.0001f) / _meshSize);
			float worldScale = ((x > _cornerHideThreshold) ? _patternWorldScale : 0f);
			ApplyPatternsWorldUniform(_cornerPatterns, worldScale);
			ApplyPatternWorldUniform(_trianglePattern, ComputeTriangleScale(x));
			ApplyPatternsWorldUniform(_etcPatterns, _patternWorldScale);
			ApplyMaterialPropertyBlock(x, z);
		}
	}

	private float ComputeTriangleScale(float worldX)
	{
		float num = Mathf.Min(_triangleWidthAtMinScale, _triangleWidthAtMaxScale);
		float num2 = Mathf.Max(_triangleWidthAtMinScale, _triangleWidthAtMaxScale);
		float t = Mathf.Clamp01((worldX - num) / Mathf.Max(num2 - num, 0.0001f));
		return Mathf.Lerp(_triangleMinScale, _triangleMaxScale, t);
	}

	private void ApplyMaterialPropertyBlock(float lineScaling, float lengthScaling)
	{
		if (_ringRenderer == null)
		{
			return;
		}
		bool flag = float.IsNaN(_lastLineScaling) || Mathf.Abs(lineScaling - _lastLineScaling) > 0.0001f;
		bool flag2 = float.IsNaN(_lastLengthScaling) || Mathf.Abs(lengthScaling - _lastLengthScaling) > 0.0001f;
		bool flag3 = true;
		if (!float.IsNaN(_lastGabDistance))
		{
			flag3 = Mathf.Abs(_gabDistance - _lastGabDistance) > 0.0001f;
		}
		if (flag || flag2 || flag3)
		{
			if (_mpb == null)
			{
				_mpb = new MaterialPropertyBlock();
			}
			_ringRenderer.GetPropertyBlock(_mpb);
			_mpb.SetFloat(_lineScalingId, lineScaling);
			_mpb.SetFloat(_lengthScalingId, lengthScaling);
			_mpb.SetFloat(_gabDistanceId, _gabDistance);
			_ringRenderer.SetPropertyBlock(_mpb);
			_lastGabDistance = _gabDistance;
			_lastLineScaling = lineScaling;
			_lastLengthScaling = lengthScaling;
		}
	}

	private static Vector3 GetParentLossyScale(Transform t)
	{
		return (t.parent != null) ? t.parent.lossyScale : Vector3.one;
	}

	private static void SetWorldLossyScaleX(Transform t, float worldLossyX)
	{
		if (!(t == null))
		{
			float num = GetParentLossyScale(t).x;
			if (Mathf.Abs(num) < 0.0001f)
			{
				num = 1f;
			}
			Vector3 localScale = t.localScale;
			float num2 = worldLossyX / num;
			if ((localScale.x - num2) * (localScale.x - num2) > 1E-08f)
			{
				t.localScale = new Vector3(num2, localScale.y, localScale.z);
			}
		}
	}

	private static void ApplyPatternsWorldUniform(List<Transform> list, float worldScale)
	{
		if (list != null)
		{
			for (int i = 0; i < list.Count; i++)
			{
				ApplyPatternWorldUniform(list[i], worldScale);
			}
		}
	}

	private static void ApplyPatternWorldUniform(Transform t, float worldScale)
	{
		if (t == null)
		{
			return;
		}
		Vector3 parentLossyScale = GetParentLossyScale(t);
		float num = parentLossyScale.x;
		if (Mathf.Abs(num) < 0.0001f)
		{
			num = 1f;
		}
		float num2 = parentLossyScale.y;
		if (Mathf.Abs(num2) < 0.0001f)
		{
			num2 = 1f;
		}
		float num3 = parentLossyScale.z;
		if (Mathf.Abs(num3) < 0.0001f)
		{
			num3 = 1f;
		}
		Vector3 vector = new Vector3(worldScale / num, worldScale / num2, worldScale / num3);
		if ((t.localScale - vector).sqrMagnitude > 1E-08f)
		{
			t.localScale = vector;
		}
	}
}
