using UnityEngine;
using UnityEngine.Serialization;

public class LayerWeightStateBehaviour : StateMachineBehaviour
{
	[Tooltip("이 스테이트에서 제어할 레이어 weight 바인딩 목록")]
	[FormerlySerializedAs("bindings")]
	[SerializeField]
	private LayerWeightBinding[] _bindings;

	[Tooltip("루핑 스테이트인 경우 normalizedTime을 0~1 반복 (기본: 클램프)")]
	[FormerlySerializedAs("loop")]
	[SerializeField]
	private bool _loop;

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
	}

	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
	}

	private void Apply(Animator animator, AnimatorStateInfo stateInfo)
	{
	}
}
