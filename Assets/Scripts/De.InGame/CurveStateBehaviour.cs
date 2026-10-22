using UnityEngine;
using UnityEngine.Serialization;

public class CurveStateBehaviour : StateMachineBehaviour
{
	[Tooltip("이 스테이트에서 구동할 커브 바인딩 목록")]
	[FormerlySerializedAs("loop")]
	public CurveBinding[] Bindings;

	[Tooltip("루핑 스테이트인 경우 normalizedTime을 0~1 반복 (기본: 클램프)")]
	[FormerlySerializedAs("loop")]
	public bool Loop;

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		float normalizedTime = GetNormalizedTime(stateInfo);
		for (int i = 0; i < Bindings.Length; i++)
		{
			animator.SetFloat(Bindings[i].Name, Bindings[i].Curve.Evaluate(normalizedTime));
		}
	}

	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		float normalizedTime = GetNormalizedTime(stateInfo);
		for (int i = 0; i < Bindings.Length; i++)
		{
			animator.SetFloat(Bindings[i].Name, Bindings[i].Curve.Evaluate(normalizedTime));
		}
	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		for (int i = 0; i < Bindings.Length; i++)
		{
			animator.SetFloat(Bindings[i].Name, Bindings[i].Curve.Evaluate(1f));
		}
	}

	private float GetNormalizedTime(AnimatorStateInfo stateInfo)
	{
		if (Loop)
		{
			return stateInfo.normalizedTime % 1f;
		}
		return Mathf.Clamp01(stateInfo.normalizedTime);
	}
}
