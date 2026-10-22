using UnityEngine;

[ExecuteAlways]
public class BackgroundWaterFootstep : MonoBehaviour
{
	[Header("Footstep Settings")]
	[SerializeField]
	private float _stepDistance;

	[SerializeField]
	private float _waveRadius;

	[SerializeField]
	private Vector3 _worldOffset;

	private Vector3 _prevPosition;

	private float _accumulatedDistance;

	private void OnEnable()
	{
	}

	private void LateUpdate()
	{
	}

	private void UpdateMovement()
	{
	}

	private void CreateWave(Vector3 position)
	{
	}

	private void OnDrawGizmosSelected()
	{
	}
}
