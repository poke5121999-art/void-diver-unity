using UnityEngine;

[ExecuteAlways]
public class BackgroundWaterWaveEmitter : MonoBehaviour
{
	[Header("Wave Settings")]
	[SerializeField]
	private float _waveDelay;

	[SerializeField]
	private float _waveRadius;

	[Header("References")]
	[SerializeField]
	private ParticleSystem _targetParticleSystem;

	private float _delayTimer;

	private float _prevParticleTime;

	private bool _wasPlaying;

	private bool _isWaiting;

	private void OnEnable()
	{
	}

	private void LateUpdate()
	{
	}

	private void StartWave()
	{
	}

	private void OnDrawGizmosSelected()
	{
	}
}
