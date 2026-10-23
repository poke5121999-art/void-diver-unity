using UnityEngine;
using UnityEngine.Serialization;

public class RandomBoneShakeStateBehaviour : StateMachineBehaviour
{
	[Tooltip("RandomBoneShaker가 읽을 Animator Float 파라미터 이름")]
	[FormerlySerializedAs("parameterName")]
	[SerializeField]
	private string _parameterName = "BoneShake";

	[Tooltip("normalizedTime(0~1)에 따른 흔들림 비율 커브")]
	[FormerlySerializedAs("ratioCurve")]
	[SerializeField]
	private AnimationCurve _ratioCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.1f, 1f), new Keyframe(0.9f, 1f), new Keyframe(1f, 0f));

	[Tooltip("루핑 스테이트인 경우 normalizedTime 반복")]
	[FormerlySerializedAs("loop")]
	[SerializeField]
	private bool _loop;

	private int _paramHash;

	private bool _hashResolved;

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (!_hashResolved)
		{
			_paramHash = Animator.StringToHash(_parameterName);
			_hashResolved = true;
		}
		animator.SetFloat(_paramHash, _ratioCurve.Evaluate(GetTime(stateInfo)));
	}

	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		animator.SetFloat(_paramHash, _ratioCurve.Evaluate(GetTime(stateInfo)));
	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		animator.SetFloat(_paramHash, _ratioCurve.Evaluate(1f));
	}

	private float GetTime(AnimatorStateInfo stateInfo)
	{
		float normalizedTime = stateInfo.normalizedTime;
		if (!_loop)
		{
			return Mathf.Clamp01(normalizedTime);
		}
		return Mathf.Repeat(normalizedTime, 1f);
	}
}
