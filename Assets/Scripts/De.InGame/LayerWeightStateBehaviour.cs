using UnityEngine;
using UnityEngine.Serialization;

public class LayerWeightStateBehaviour : StateMachineBehaviour
{
	[Tooltip("이 스테이트에서 제어할 레이어 weight 바인딩 목록")]
	[FormerlySerializedAs("bindings")]
	[SerializeField]
	private LayerWeightBinding[] _bindings = new LayerWeightBinding[0];

	[Tooltip("루핑 스테이트인 경우 normalizedTime을 0~1 반복 (기본: 클램프)")]
	[FormerlySerializedAs("loop")]
	[SerializeField]
	private bool _loop;

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		Apply(animator, stateInfo);
	}

	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		Apply(animator, stateInfo);
	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		for (int i = 0; i < _bindings.Length; i++)
		{
			animator.SetLayerWeight(_bindings[i].LayerIndex, _bindings[i].WeightCurve.Evaluate(1f));
		}
	}

	private void Apply(Animator animator, AnimatorStateInfo stateInfo)
	{
		float num = stateInfo.normalizedTime;
		num = (_loop ? Mathf.Repeat(num, 1f) : Mathf.Clamp01(num));
		for (int i = 0; i < _bindings.Length; i++)
		{
			animator.SetLayerWeight(_bindings[i].LayerIndex, _bindings[i].WeightCurve.Evaluate(num));
		}
	}
}
