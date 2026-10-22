using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class IndicatorBoxPostVisualCorrector : MonoBehaviour
{
	[SerializeField]
	private Transform _sourceTransform;

	[SerializeField]
	private float _meshSize;

	[SerializeField]
	private float _gabDistance;

	[SerializeField]
	private float _patternWorldScale;

	[SerializeField]
	private Transform _outerTransform;

	[SerializeField]
	private Transform _innerTransform;

	[SerializeField]
	private float _cornerHideThreshold;

	[SerializeField]
	private List<Transform> _cornerPatterns;

	[SerializeField]
	private Transform _trianglePattern;

	[SerializeField]
	private float _triangleWidthAtMinScale;

	[SerializeField]
	private float _triangleWidthAtMaxScale;

	[SerializeField]
	private float _triangleMinScale;

	[SerializeField]
	private float _triangleMaxScale;

	[SerializeField]
	private List<Transform> _etcPatterns;

	[SerializeField]
	private Renderer _ringRenderer;

	private MaterialPropertyBlock _mpb;

	private float _lastLineScaling;

	private float _lastLengthScaling;

	private float _lastGabDistance;

	private static readonly int _lineScalingId;

	private static readonly int _lengthScalingId;

	private static readonly int _gabDistanceId;

	private const float _epsilon = 0.0001f;

	private void LateUpdate()
	{
	}

	private float ComputeTriangleScale(float worldX)
	{
		return 0f;
	}

	private void ApplyMaterialPropertyBlock(float lineScaling, float lengthScaling)
	{
	}

	private static void SetWorldLossyScaleX(Transform t, float worldLossyX)
	{
	}

	private static void ApplyPatternsWorldUniform(List<Transform> list, float worldScale)
	{
	}

	private static void ApplyPatternWorldUniform(Transform t, float worldScale)
	{
	}
}
