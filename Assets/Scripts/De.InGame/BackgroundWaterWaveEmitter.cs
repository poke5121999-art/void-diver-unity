using UnityEngine;

[ExecuteAlways]
public class BackgroundWaterWaveEmitter : MonoBehaviour
{
	[Header("Wave Settings")]
	[SerializeField]
	private float _waveDelay;

	[SerializeField]
	private float _waveRadius = 1f;

	[Header("References")]
	[SerializeField]
	private ParticleSystem _targetParticleSystem;

	private float _delayTimer;

	private float _prevParticleTime;

	private bool _wasPlaying;

	private bool _isWaiting;

	private void OnEnable()
	{
		if (_targetParticleSystem == null)
		{
			_targetParticleSystem = GetComponent<ParticleSystem>();
		}
	}

	private void LateUpdate()
	{
		bool isPlaying = _targetParticleSystem != null && _targetParticleSystem.isPlaying;
		float time = ((_targetParticleSystem != null) ? _targetParticleSystem.time : 0f);
		if (!isPlaying && _wasPlaying)
		{
			_isWaiting = false;
		}
		if (isPlaying && (!_wasPlaying || _prevParticleTime > time))
		{
			_delayTimer = _waveDelay;
			_isWaiting = _waveDelay > 0f;
			if (_waveDelay <= 0f)
			{
				StartWave();
			}
		}
		_prevParticleTime = time;
		_wasPlaying = isPlaying;
		if (_isWaiting)
		{
			_delayTimer -= Time.deltaTime;
			if (_delayTimer <= 0f)
			{
				_isWaiting = false;
				StartWave();
			}
		}
	}

	private void StartWave()
	{
		float radius = Mathf.Clamp(_waveRadius, 0f, 5f);
		if (MapVisualAreaLoader.Instance != null)
		{
			MapVisualAreaLoader.Instance.SpawnWave(base.transform.position, radius);
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = new Color(0.2f, 0.6f, 1f, 1f);
		Gizmos.DrawWireSphere(base.transform.position, _waveRadius);
	}
}
