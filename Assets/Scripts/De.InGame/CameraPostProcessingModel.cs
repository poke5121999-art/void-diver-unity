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

	public CameraPostProcessingScriptableObject StressPostProcessing => null;

	public Volume LowHpVolume => null;

	public Volume SightEnableVolume => null;

	public Volume DamageHpVolume => null;

	public Volume HealingHpVolume => null;

	public Volume OneShotStressVolume => null;
}
