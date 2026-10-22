using System;
using UnityEngine;
using UnityEngine.Serialization;

[DefaultExecutionOrder(102)]
public class ImpactStretch : MonoBehaviour
{
	[Serializable]
	public struct BoneTarget
	{
		[Tooltip("스케일을 조절할 본 Transform")]
		[FormerlySerializedAs("bone")]
		public Transform Bone;

		[Tooltip("최대 늘어남 배율 (ratio=1일 때 기본 스케일에 곱해질 값)")]
		[FormerlySerializedAs("stretchAmount")]
		public float StretchAmount;
	}

	[Serializable]
	public struct CompensateTarget
	{
		[Tooltip("역보상할 본 Transform")]
		[FormerlySerializedAs("bone")]
		public Transform Bone;

		[Tooltip("역보상 축 (스케일 변화를 상쇄할 축, 기본값 = scaleAxis와 동일)")]
		[FormerlySerializedAs("useCustomAxis")]
		public bool UseCustomAxis;

		[Tooltip("useCustomAxis가 true일 때 사용할 축")]
		[FormerlySerializedAs("customAxis")]
		public Vector3 CustomAxis;
	}

	[Header("Source")]
	[Tooltip("Float 파라미터를 읽을 Animator (비어있으면 자동 탐색)")]
	[FormerlySerializedAs("targetAnimator")]
	[SerializeField]
	private Animator _targetAnimator;

	[Tooltip("CurveStateBehaviour에서 기록하는 Animator Float 파라미터 이름")]
	[FormerlySerializedAs("parameterName")]
	[SerializeField]
	private string _parameterName = "ImpactStretch";

	[Header("Scale")]
	[Tooltip("스케일을 적용할 로컬 축 (예: Y축 길이 = (0,1,0))")]
	[FormerlySerializedAs("scaleAxis")]
	[SerializeField]
	private Vector3 _scaleAxis = Vector3.up;

	[Header("Targets")]
	[Tooltip("스케일을 조절할 본 목록")]
	[FormerlySerializedAs("targets")]
	[SerializeField]
	private BoneTarget[] _targets;

	[Header("Compensate")]
	[Tooltip("부모 스케일 변화에 영향 받지 않도록 역보상할 본 목록 (stretch 대상의 자식이어야 함)")]
	[FormerlySerializedAs("compensateTargets")]
	[SerializeField]
	private CompensateTarget[] _compensateTargets;

	private Animator _resolvedAnimator;

	private int _paramHash;

	private Vector3[] _baseScales;

	private Vector3[] _compensateBaseScales;

	private void Awake()
	{
		ResolveAnimator();
		_paramHash = Animator.StringToHash(_parameterName);
	}

	private void Start()
	{
		if (_targets == null)
		{
			return;
		}
		_baseScales = new Vector3[_targets.Length];
		for (int i = 0; i < _targets.Length; i++)
		{
			if (_targets[i].Bone != null)
			{
				_baseScales[i] = _targets[i].Bone.localScale;
			}
		}
		if (_compensateTargets == null)
		{
			return;
		}
		_compensateBaseScales = new Vector3[_compensateTargets.Length];
		for (int j = 0; j < _compensateTargets.Length; j++)
		{
			if (_compensateTargets[j].Bone != null)
			{
				_compensateBaseScales[j] = _compensateTargets[j].Bone.localScale;
			}
		}
	}

	private void LateUpdate()
	{
		if (_resolvedAnimator == null || _targets == null || _baseScales == null)
		{
			return;
		}
		float @float = _resolvedAnimator.GetFloat(_paramHash);
		for (int i = 0; i < _targets.Length; i++)
		{
			Transform bone = _targets[i].Bone;
			if (!(bone == null))
			{
				float num = 1f + (_targets[i].StretchAmount - 1f) * Mathf.Clamp01(@float);
				Vector3 localScale = _baseScales[i];
				if (_scaleAxis.x > 0.5f)
				{
					localScale.x *= num;
				}
				if (_scaleAxis.y > 0.5f)
				{
					localScale.y *= num;
				}
				if (_scaleAxis.z > 0.5f)
				{
					localScale.z *= num;
				}
				bone.localScale = localScale;
			}
		}
		if (_compensateTargets == null || _compensateBaseScales == null)
		{
			return;
		}
		for (int j = 0; j < _compensateTargets.Length; j++)
		{
			CompensateTarget compensateTarget = _compensateTargets[j];
			Transform bone2 = compensateTarget.Bone;
			if (bone2 == null || bone2.parent == null)
			{
				continue;
			}
			Vector3 vector = (compensateTarget.UseCustomAxis ? compensateTarget.CustomAxis : _scaleAxis);
			Vector3 localScale2 = bone2.parent.localScale;
			Vector3 localScale3 = _compensateBaseScales[j];
			if (vector.x > 0.5f && localScale2.x != 0f)
			{
				localScale3.x /= localScale2.x;
			}
			if (vector.y > 0.5f && localScale2.y != 0f)
			{
				localScale3.y /= localScale2.y;
			}
			if (vector.z > 0.5f && localScale2.z != 0f)
			{
				localScale3.z /= localScale2.z;
			}
			bone2.localScale = localScale3;
		}
	}

	public void SetStretchAmount(float amount)
	{
		if (_targets != null)
		{
			for (int i = 0; i < _targets.Length; i++)
			{
				_targets[i].StretchAmount = amount;
			}
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
