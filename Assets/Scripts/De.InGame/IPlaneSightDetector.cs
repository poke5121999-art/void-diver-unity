using UnityEngine;

public interface IPlaneSightDetector
{
	void RegisterToManagerAsDetector();

	void UnRegisterFromManagerAsDetector();

	bool GetIsThisDetected(GameObject gameObject, bool ignoreToggle = false);

	Transform GetTransform();
}
