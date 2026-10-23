using UnityEngine;

public class ProjectileTest : MonoBehaviour
{
	public Vector3 areaSize;

	public float speed;

	public float accelerationAmount;

	public GameObject hitVFXPrefab;

	public float hitVFXDuration;

	private Vector3 targetPosition;

	private float currentSpeed;

	private float currentAcceleration;

	private void Start()
	{
		targetPosition = GetRandomPosition();
		base.transform.LookAt(targetPosition);
		currentAcceleration = accelerationAmount;
		currentSpeed = 0f;
	}

	private void Update()
	{
		currentSpeed += currentAcceleration * Time.deltaTime;
		base.transform.position = Vector3.MoveTowards(base.transform.position, targetPosition, currentSpeed * Time.deltaTime);
		if (base.transform.position == targetPosition)
		{
			targetPosition = GetRandomPosition();
			base.transform.LookAt(targetPosition);
			currentAcceleration = accelerationAmount;
			currentSpeed = 0f;
			Object.Instantiate(hitVFXPrefab, base.transform.position, Quaternion.identity);
		}
	}

	private Vector3 GetRandomPosition()
	{
		return new Vector3(Random.Range((0f - areaSize.x) * 0.5f, areaSize.x * 0.5f), Random.Range((0f - areaSize.y) * 0.5f, areaSize.y * 0.5f), Random.Range((0f - areaSize.z) * 0.5f, areaSize.z * 0.5f));
	}
}
