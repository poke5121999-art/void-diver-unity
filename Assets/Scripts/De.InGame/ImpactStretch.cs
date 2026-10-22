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
	private string _parameterName;

	[Header("Scale")]
	[Tooltip("스케일을 적용할 로컬 축 (예: Y축 길이 = (0,1,0))")]
	[FormerlySerializedAs("scaleAxis")]
	[SerializeField]
	private Vector3 _scaleAxis;

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
	}

	private void Start()
	{
	}

	private void LateUpdate()
	{
	}

	public void SetStretchAmount(float amount)
	{
	}

	private void ResolveAnimator()
	{
	}
}
