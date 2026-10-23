using UnityEngine;

public class LightFlicker : MonoBehaviour
{
	public enum FlickerPattern
	{
		Smooth = 0,
		Neon = 1,
		RandomBurst = 2
	}

	private Light _pointLight;

	[Header("Flicker Settings")]
	[Tooltip("Choose the desired flickering pattern.")]
	public FlickerPattern CurrentPattern;

	[Tooltip("Minimum intensity factor relative to the light's initial intensity.")]
	[Range(0f, 2f)]
	public float MinFactor = 0.125f;

	[Tooltip("Maximum intensity factor relative to the light's initial intensity.")]
	[Range(0f, 2f)]
	public float MaxFactor = 1f;

	[Tooltip("Controls the speed or frequency of the flicker for all patterns.")]
	public float FlickerSpeed = 1f;

	[Tooltip("Random seed for the flicker pattern. Use 0 for truly random behavior, or a fixed number for repeatable patterns.")]
	public int RandomSeed;

	[Header("Pattern Specific Settings")]
	[Tooltip("For Neon pattern: Controls how sharp the on/off transition is. Higher values (closer to 1) make it more binary. (0.01 - 1)")]
	[Range(0.01f, 0.99f)]
	public float NeonThreshold = 0.5f;

	[Tooltip("For Random Burst pattern: How likely a burst is to occur. Higher values mean more frequent bursts. (0 - 1)")]
	[Range(0f, 1f)]
	public float BurstProbability = 0.1f;

	[Tooltip("For Random Burst pattern: How long each burst of light lasts in seconds.")]
	[Range(0.01f, 0.5f)]
	public float BurstDuration = 0.1f;

	private float _initialIntensity;

	private float _randomOffset;

	private float _burstTimer;

	private bool _isBursting;

	private void Awake()
	{
		_pointLight = base.gameObject.GetComponent<Light>();
		if (_pointLight == null)
		{
			Debug.LogError("LightFlicker: No 'Light' component found on this GameObject. Disabling script.", this);
			base.enabled = false;
			return;
		}
		_initialIntensity = _pointLight.intensity;
		Random.InitState((int)(base.gameObject.transform.position.x * 1000f + (float)RandomSeed + base.gameObject.transform.position.z * 1000f));
		_randomOffset = Random.value * 100f;
	}

	private void LateUpdate()
	{
		if (_pointLight == null)
		{
			return;
		}
		float num = 0f;
		float num2 = Mathf.PerlinNoise(FlickerSpeed * Time.time + _randomOffset, 0f);
		switch (CurrentPattern)
		{
		case FlickerPattern.Smooth:
			num = Mathf.Lerp(_initialIntensity * MinFactor, _initialIntensity * MaxFactor, num2);
			break;
		case FlickerPattern.Neon:
			num = ((!(num2 > NeonThreshold)) ? (_initialIntensity * MinFactor) : (_initialIntensity * MaxFactor));
			break;
		case FlickerPattern.RandomBurst:
			if (_isBursting)
			{
				_burstTimer -= Time.deltaTime;
				if (_burstTimer <= 0f)
				{
					num = _initialIntensity * MinFactor;
					_isBursting = false;
				}
				else
				{
					num = _initialIntensity * MaxFactor;
				}
			}
			else
			{
				num = MinFactor * _initialIntensity;
				if (num2 > 1f - BurstProbability)
				{
					num = _initialIntensity * MaxFactor;
					_burstTimer = BurstDuration;
					_isBursting = true;
				}
			}
			break;
		}
		_pointLight.intensity = num;
	}

	private void OnDisable()
	{
		if (!(_pointLight == null))
		{
			_pointLight.intensity = _initialIntensity;
		}
	}
}
