using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

public class CameraPostProcessingModel : MonoBehaviour
{
	[FormerlySerializedAs("_sanityPostProcessing")]
	[SerializeField]
	private CameraPostProcessingScriptableObject _stressPostProcessing;

	[SerializeField]
	private Volume _lowHpVolume;

	[SerializeField]
	private Volume _sightEnableVolume;

	[SerializeField]
	private Volume _damageHpVolume;

	[SerializeField]
	private Volume _healingHpVolume;

	[SerializeField]
	private Volume _oneShotStressVolume;

	public CameraPostProcessingScriptableObject StressPostProcessing => _stressPostProcessing;

	public Volume LowHpVolume => _lowHpVolume;

	public Volume SightEnableVolume => _sightEnableVolume;

	public Volume DamageHpVolume => _damageHpVolume;

	public Volume HealingHpVolume => _healingHpVolume;

	public Volume OneShotStressVolume => _oneShotStressVolume;
}
