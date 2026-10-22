using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class IndicatorCirclePostVisualCorrector : MonoBehaviour
{
	[SerializeField]
	private float _meshRadius;

	[SerializeField]
	private float _gabDistance;

	[SerializeField]
	private float _patternWorldScale;

	[SerializeField]
	private float _middleThinRadius;

	[SerializeField]
	private float _middleHideRadius;

	[SerializeField]
	private Transform _scaleSource;

	[SerializeField]
	private Transform _outerTransform;

	[SerializeField]
	private Transform _middleTransform;

	[SerializeField]
	private Transform _innerTransform;

	[SerializeField]
	private List<Transform> _outerPatterns;

	[SerializeField]
	private List<Transform> _middlePatterns;

	[SerializeField]
	private List<Transform> _innerPatterns;

	[SerializeField]
	private Renderer _ringRenderer;

	private MaterialPropertyBlock _mpb;

	private float _lastLineScaling;

	private float _lastMeshRadius;

	private float _lastGabDistance;

	private static readonly int _lineScalingId;

	private static readonly int _meshRadiusId;

	private static readonly int _gabDistanceId;

	private const float _epsilon = 0.0001f;

	private void LateUpdate()
	{
	}

	private void ApplyMiddlePatterns(float currentScale)
	{
	}

	private void ApplyMaterialPropertyBlock()
	{
	}

	private static void SetWorldLossyScale(Transform t, float worldLossy)
	{
	}

	private static void ApplyWorldLossyScale(List<Transform> list, float worldLossy)
	{
	}

	private static void SetUniformScaleIfChanged(Transform t, float s)
	{
	}
}
