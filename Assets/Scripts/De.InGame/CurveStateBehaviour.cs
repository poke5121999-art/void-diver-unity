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
	}

	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
	}

	private float GetNormalizedTime(AnimatorStateInfo stateInfo)
	{
		return 0f;
	}
}
