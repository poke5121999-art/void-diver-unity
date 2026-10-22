using UnityEngine;

public interface ITarget
{
	ulong NetworkObjectId { get; }

	Vector3 Position { get; }
}
