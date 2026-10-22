using UnityEngine;

public interface IDissolveTarget
{
	float Progress { get; }

	void SetProgress(float value, Color edgeColor);
}
