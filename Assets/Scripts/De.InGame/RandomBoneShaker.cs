using UnityEngine;
using UnityEngine.Serialization;

[DefaultExecutionOrder(102)]
public class RandomBoneShaker : MonoBehaviour
{
	[Header("Source")]
	[Tooltip("Float 파라미터를 읽을 Animator (비어있으면 자동 탐색)")]
	[FormerlySerializedAs("targetAnimator")]
	[SerializeField]
	private Animator _targetAnimator;

	[Tooltip("RandomBoneShakeSMB에서 기록하는 Animator Float 파라미터 이름")]
	[FormerlySerializedAs("parameterName")]
	[SerializeField]
	private string _parameterName;

	[Header("Target Bones")]
	[Tooltip("흔들림을 적용할 본 목록")]
	[FormerlySerializedAs("bones")]
	[SerializeField]
	private Transform[] _bones;

	[Header("Shake Settings")]
	[Tooltip("최대 흔들림 각도 (도)")]
	[FormerlySerializedAs("maxAngle")]
	[SerializeField]
	private float _maxAngle;

	[Tooltip("흔들림 변화 속도 (높을수록 빠르게 떨림)")]
	[FormerlySerializedAs("speed")]
	[SerializeField]
	private float _speed;

	[Tooltip("X축 흔들림 가중치")]
	[FormerlySerializedAs("weightX")]
	[SerializeField]
	private float _weightX;

	[Tooltip("Y축 흔들림 가중치")]
	[FormerlySerializedAs("weightY")]
	[SerializeField]
	private float _weightY;

	[Tooltip("Z축 흔들림 가중치")]
	[FormerlySerializedAs("weightZ")]
	[SerializeField]
	private float _weightZ;

	[Header("Per-Bone Variation")]
	[Tooltip("본마다 랜덤 시드 오프셋을 줘서 각각 다르게 흔들림")]
	[FormerlySerializedAs("bonePhaseSpread")]
	[SerializeField]
	private float _bonePhaseSpread;

	private Animator _resolvedAnimator;

	private int _paramHash;

	private float[] _boneSeeds;

	private void Awake()
	{
	}

	private void LateUpdate()
	{
	}

	private void InitBoneSeeds()
	{
	}

	private void ResolveAnimator()
	{
	}
}
