using UnityEngine;

[ExecuteAlways]
public class BackgroundWaterFootstep : MonoBehaviour
{
	[Header("Footstep Settings")]
	[SerializeField]
	private float _stepDistance = 1f;

	[SerializeField]
	private float _waveRadius = 0.5f;

	[SerializeField]
	private Vector3 _worldOffset;

	private Vector3 _prevPosition;

	private float _accumulatedDistance;

	private void OnEnable()
	{
		_prevPosition = transform.position;
		_accumulatedDistance = 0f;
	}

	private void LateUpdate()
	{
		UpdateMovement();
	}

	private void UpdateMovement()
	{
		Vector3 position = transform.position;
		float dx = position.x - _prevPosition.x;
		float dz = position.z - _prevPosition.z;
		float dist = (float)System.Math.Sqrt(dx * dx + 0f + dz * dz);
		_prevPosition = position;
		_accumulatedDistance += dist;
		if (_stepDistance > 0f && _accumulatedDistance >= _stepDistance)
		{
			_accumulatedDistance -= _stepDistance;
			CreateWave(position + _worldOffset);
		}
	}

	private void CreateWave(Vector3 position)
	{
		float radius = Mathf.Clamp(_waveRadius, 0f, 5f);
		if (MapVisualAreaLoader.Instance != null)
		{
			MapVisualAreaLoader.Instance.SpawnWave(position, radius);
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = new Color(0.2f, 0.6f, 1f, 1f);
		Gizmos.DrawWireSphere(transform.position + _worldOffset, _waveRadius);
	}
}
