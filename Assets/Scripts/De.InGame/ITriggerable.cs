using UnityEngine;

public interface ITriggerable
{
	int TriggerId { get; }

	bool CanTriggered { get; }

	Vector3 Position { get; }

	void SetTriggerable(bool canTriggered);
}
