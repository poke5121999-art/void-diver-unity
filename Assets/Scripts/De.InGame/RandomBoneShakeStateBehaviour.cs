using UnityEngine;
using UnityEngine.Serialization;

public class RandomBoneShakeStateBehaviour : StateMachineBehaviour
{
	[Tooltip("RandomBoneShaker가 읽을 Animator Float 파라미터 이름")]
	[FormerlySerializedAs("parameterName")]
	[SerializeField]
	private string _parameterName;

	[Tooltip("normalizedTime(0~1)에 따른 흔들림 비율 커브")]
	[FormerlySerializedAs("ratioCurve")]
	[SerializeField]
	private AnimationCurve _ratioCurve;

	[Tooltip("루핑 스테이트인 경우 normalizedTime 반복")]
	[FormerlySerializedAs("loop")]
	[SerializeField]
	private bool _loop;

	private int _paramHash;

	private bool _hashResolved;

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
	}

	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
	}

	private float GetTime(AnimatorStateInfo stateInfo)
	{
		return 0f;
	}
}
