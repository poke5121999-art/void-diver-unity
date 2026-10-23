using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class IndicatorCirclePostVisualCorrector : MonoBehaviour
{
	[SerializeField]
	private float _meshRadius = 2f;

	[SerializeField]
	private float _gabDistance = 0.1f;

	[SerializeField]
	private float _patternWorldScale = 1f;

	[SerializeField]
	private float _middleThinRadius = 1f;

	[SerializeField]
	private float _middleHideRadius = 0.5f;

	[SerializeField]
	private Transform _scaleSource;

	[SerializeField]
	private Transform _outerTransform;

	[SerializeField]
	private Transform _middleTransform;

	[SerializeField]
	private Transform _innerTransform;

	[SerializeField]
	private List<Transform> _outerPatterns = new List<Transform>();

	[SerializeField]
	private List<Transform> _middlePatterns = new List<Transform>();

	[SerializeField]
	private List<Transform> _innerPatterns = new List<Transform>();

	[SerializeField]
	private Renderer _ringRenderer;

	private MaterialPropertyBlock _mpb;

	private float _lastLineScaling = float.NaN;

	private float _lastMeshRadius = float.NaN;

	private float _lastGabDistance = float.NaN;

	private static readonly int _lineScalingId = Shader.PropertyToID("_LineScaling");

	private static readonly int _meshRadiusId = Shader.PropertyToID("_MeshRadius");

	private static readonly int _gabDistanceId = Shader.PropertyToID("_GabDistance");

	private const float _epsilon = 0.0001f;

	private void LateUpdate()
	{
		if (_meshRadius <= 0.0001f)
		{
			return;
		}
		Transform transform = ((_scaleSource != null) ? _scaleSource : base.transform);
		float x = transform.localScale.x;
		if (!(x <= 0.0001f))
		{
			float gabDistance = _gabDistance;
			float num = Mathf.Max(x - _gabDistance, 0.0001f);
			SetWorldLossyScale(_outerTransform, x / _meshRadius);
			SetWorldLossyScale(_middleTransform, (x - gabDistance * 0.5f) / _meshRadius);
			SetWorldLossyScale(_innerTransform, num / _meshRadius);
			ApplyWorldLossyScale(_outerPatterns, _patternWorldScale);
			ApplyWorldLossyScale(_innerPatterns, _patternWorldScale);
			ApplyMiddlePatterns(x);
			ApplyMaterialPropertyBlock();
		}
	}

	private void ApplyMiddlePatterns(float currentScale)
	{
		float middleHideRadius = _middleHideRadius;
		float middleThinRadius = _middleThinRadius;
		for (int i = 0; i < _middlePatterns.Count; i++)
		{
			Transform transform = _middlePatterns[i];
			if (!(transform == null))
			{
				if (currentScale <= middleHideRadius || (currentScale <= middleThinRadius && i % 2 != 0))
				{
					SetUniformScaleIfChanged(transform, 0f);
				}
				else
				{
					SetWorldLossyScale(transform, _patternWorldScale);
				}
			}
		}
	}

	private void ApplyMaterialPropertyBlock()
	{
		if (_ringRenderer == null)
		{
			return;
		}
		Transform transform = ((_outerTransform != null) ? _outerTransform : base.transform);
		float num = Mathf.Max(transform.lossyScale.x, 0.0001f);
		if (Mathf.Abs(num - _lastLineScaling) > 0.0001f || Mathf.Abs(_meshRadius - _lastMeshRadius) > 0.0001f || Mathf.Abs(_gabDistance - _lastGabDistance) > 0.0001f)
		{
			if (_mpb == null)
			{
				_mpb = new MaterialPropertyBlock();
			}
			_ringRenderer.GetPropertyBlock(_mpb);
			_mpb.SetFloat(_lineScalingId, num);
			_mpb.SetFloat(_meshRadiusId, _meshRadius);
			_mpb.SetFloat(_gabDistanceId, _gabDistance);
			_ringRenderer.SetPropertyBlock(_mpb);
			_lastMeshRadius = _meshRadius;
			_lastGabDistance = _gabDistance;
			_lastLineScaling = num;
		}
	}

	private static void SetWorldLossyScale(Transform t, float worldLossy)
	{
		if (!(t == null))
		{
			float num = ((t.parent != null) ? t.parent.lossyScale.x : 1f);
			if (!(num < 0.0001f))
			{
				SetUniformScaleIfChanged(t, worldLossy / num);
			}
		}
	}

	private static void ApplyWorldLossyScale(List<Transform> list, float worldLossy)
	{
		if (list == null)
		{
			return;
		}
		for (int i = 0; i < list.Count; i++)
		{
			Transform transform = list[i];
			if (!(transform == null))
			{
				float num = ((transform.parent != null) ? transform.parent.lossyScale.x : 1f);
				if (!(num < 0.0001f))
				{
					SetUniformScaleIfChanged(transform, worldLossy / num);
				}
			}
		}
	}

	private static void SetUniformScaleIfChanged(Transform t, float s)
	{
		if (!(t == null) && (t.localScale - new Vector3(s, s, s)).sqrMagnitude > 1E-08f)
		{
			t.localScale = new Vector3(s, s, s);
		}
	}
}
