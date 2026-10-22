using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[DefaultExecutionOrder(101)]
public class ImpactShake : MonoBehaviour
{
	[Header("Source")]
	[Tooltip("Float 파라미터를 읽을 Animator (비어있으면 자동 탐색)")]
	[FormerlySerializedAs("targetAnimator")]
	[SerializeField]
	private Animator _targetAnimator;

	[Tooltip("CurveStateBehaviour에서 기록하는 Animator Float 파라미터 이름")]
	[FormerlySerializedAs("parameterName")]
	[SerializeField]
	private string _parameterName = "ImpactShake";

	[Header("Bone Chain")]
	[Tooltip("진동을 적용할 루트 본")]
	[FormerlySerializedAs("rootBone")]
	[SerializeField]
	private Transform _rootBone;

	[Tooltip("체인 깊이 제한 (0 = 무제한)")]
	[FormerlySerializedAs("maxChainDepth")]
	[SerializeField]
	private int _maxChainDepth;

	[Header("Primary Wave")]
	[Tooltip("1차 진동 주파수 (Hz)")]
	[FormerlySerializedAs("frequency")]
	[SerializeField]
	private float _frequency = 8f;

	[Tooltip("최대 진동 각도 (도)")]
	[FormerlySerializedAs("amplitude")]
	[SerializeField]
	private float _amplitude = 20f;

	[Tooltip("본 간 위상 오프셋 (라디안)")]
	[FormerlySerializedAs("phaseOffsetPerBone")]
	[SerializeField]
	private float _phaseOffsetPerBone = 0.8f;

	[Header("Secondary Wave")]
	[Tooltip("2차 파동 주파수 비율 (1차 대비)")]
	[FormerlySerializedAs("secondaryFrequencyRatio")]
	[SerializeField]
	private float _secondaryFrequencyRatio = 2.3f;

	[Tooltip("2차 파동 진폭 비율 (1차 대비)")]
	[Range(0f, 1f)]
	[FormerlySerializedAs("secondaryAmplitudeRatio")]
	[SerializeField]
	private float _secondaryAmplitudeRatio = 0.3f;

	[Header("Axis")]
	[Tooltip("로컬 X축 진동 가중치")]
	[FormerlySerializedAs("axisWeightX")]
	[SerializeField]
	private float _axisWeightX = 1f;

	[Tooltip("로컬 Z축 진동 가중치")]
	[FormerlySerializedAs("axisWeightZ")]
	[SerializeField]
	private float _axisWeightZ = 0.3f;

	[Tooltip("X-Z 위상차 (라디안). π/2=원형, 0=직선")]
	[FormerlySerializedAs("axisPhaseOffset")]
	[SerializeField]
	private float _axisPhaseOffset = 1.57f;

	[Header("Distribution")]
	[Tooltip("루트(0) → 팁(1) 진폭 배율 커브")]
	[FormerlySerializedAs("amplitudeCurve")]
	[SerializeField]
	private AnimationCurve _amplitudeCurve = AnimationCurve.Linear(0f, 0.2f, 1f, 1f);

	private Transform[] _boneChain;

	private Animator _resolvedAnimator;

	private int _paramHash;

	private void Awake()
	{
		BuildBoneChain();
		ResolveAnimator();
		_paramHash = Animator.StringToHash(_parameterName);
	}

	private void LateUpdate()
	{
		if (_boneChain == null || _boneChain.Length == 0 || _resolvedAnimator == null)
		{
			return;
		}
		float @float = _resolvedAnimator.GetFloat(_paramHash);
		if (@float < 0.001f)
		{
			return;
		}
		float time = Time.time;
		int num = _boneChain.Length;
		for (int i = 0; i < num; i++)
		{
			Transform transform = _boneChain[i];
			if (!(transform == null))
			{
				float time2 = ((num > 1) ? ((float)i / (float)(num - 1)) : 0f);
				float num2 = @float * _amplitude * _amplitudeCurve.Evaluate(time2);
				float num3 = time * _frequency * MathF.PI * 2f;
				float num4 = (float)i * _phaseOffsetPerBone;
				float num5 = num3 * _secondaryFrequencyRatio;
				float x = (Mathf.Sin(num4 * 0.7f + num5) * _secondaryAmplitudeRatio + Mathf.Sin(num3 + num4)) * num2 * _axisWeightX;
				float z = (Mathf.Sin(num4 * 1.1f + num5 + _axisPhaseOffset) * _secondaryAmplitudeRatio + Mathf.Sin(num3 + num4 + _axisPhaseOffset)) * num2 * _axisWeightZ;
				transform.localRotation = transform.localRotation * Quaternion.Euler(x, 0f, z);
			}
		}
	}

	private void BuildBoneChain()
	{
		if (_rootBone == null)
		{
			_rootBone = ((base.transform.childCount > 0) ? base.transform.GetChild(0) : base.transform);
		}
		List<Transform> list = new List<Transform>();
		list.Add(_rootBone);
		if (_rootBone.childCount != 0)
		{
			CollectChain(_rootBone.GetChild(0), list, 1);
		}
		_boneChain = list.ToArray();
	}

	private void CollectChain(Transform current, List<Transform> list, int depth)
	{
		while (true)
		{
			list.Add(current);
			if ((_maxChainDepth > 0 && _maxChainDepth <= depth) || current.childCount == 0)
			{
				break;
			}
			depth++;
			current = current.GetChild(0);
		}
	}

	private void ResolveAnimator()
	{
		_resolvedAnimator = _targetAnimator;
		if (_resolvedAnimator == null)
		{
			_resolvedAnimator = GetComponent<Animator>();
		}
		if (_resolvedAnimator == null)
		{
			_resolvedAnimator = GetComponentInChildren<Animator>();
		}
		if (_resolvedAnimator == null)
		{
			_resolvedAnimator = GetComponentInParent<Animator>();
		}
	}
}
