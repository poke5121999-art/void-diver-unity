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
	private string _parameterName = "BoneShake";

	[Header("Target Bones")]
	[Tooltip("흔들림을 적용할 본 목록")]
	[FormerlySerializedAs("bones")]
	[SerializeField]
	private Transform[] _bones;

	[Header("Shake Settings")]
	[Tooltip("최대 흔들림 각도 (도)")]
	[FormerlySerializedAs("maxAngle")]
	[SerializeField]
	private float _maxAngle = 15f;

	[Tooltip("흔들림 변화 속도 (높을수록 빠르게 떨림)")]
	[FormerlySerializedAs("speed")]
	[SerializeField]
	private float _speed = 20f;

	[Tooltip("X축 흔들림 가중치")]
	[FormerlySerializedAs("weightX")]
	[SerializeField]
	private float _weightX = 1f;

	[Tooltip("Y축 흔들림 가중치")]
	[FormerlySerializedAs("weightY")]
	[SerializeField]
	private float _weightY = 0.5f;

	[Tooltip("Z축 흔들림 가중치")]
	[FormerlySerializedAs("weightZ")]
	[SerializeField]
	private float _weightZ = 1f;

	[Header("Per-Bone Variation")]
	[Tooltip("본마다 랜덤 시드 오프셋을 줘서 각각 다르게 흔들림")]
	[FormerlySerializedAs("bonePhaseSpread")]
	[SerializeField]
	private float _bonePhaseSpread = 100f;

	private Animator _resolvedAnimator;

	private int _paramHash;

	private float[] _boneSeeds;

	private void Awake()
	{
		ResolveAnimator();
		_paramHash = Animator.StringToHash(_parameterName);
		InitBoneSeeds();
	}

	private void LateUpdate()
	{
		if (_bones == null || _bones.Length == 0 || _resolvedAnimator == null)
		{
			return;
		}
		float @float = _resolvedAnimator.GetFloat(_paramHash);
		if (@float < 0.001f)
		{
			return;
		}
		float num = Time.time * _speed;
		float num2 = @float * _maxAngle;
		for (int i = 0; i < _bones.Length; i++)
		{
			if (!(_bones[i] == null))
			{
				float num3 = ((_boneSeeds != null && i < _boneSeeds.Length) ? _boneSeeds[i] : ((float)i * _bonePhaseSpread));
				float num4 = Mathf.PerlinNoise(num3 + num, 0f);
				float num5 = Mathf.PerlinNoise(0f, num3 + num + 31.7f);
				float num6 = Mathf.PerlinNoise(num3 + num + 73.2f, num3 + num);
				Vector3 euler = new Vector3((num4 - 0.5f) * 2f * num2 * _weightX, (num5 - 0.5f) * 2f * num2 * _weightY, (num6 - 0.5f) * 2f * num2 * _weightZ);
				_bones[i].localRotation = Quaternion.Euler(euler) * _bones[i].localRotation;
			}
		}
	}

	private void InitBoneSeeds()
	{
		if (_bones == null)
		{
			return;
		}
		_boneSeeds = new float[_bones.Length];
		for (int i = 0; i < _bones.Length; i++)
		{
			_boneSeeds[i] = (float)i * _bonePhaseSpread + Random.Range(0f, _bonePhaseSpread);
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
