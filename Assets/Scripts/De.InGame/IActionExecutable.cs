using UnityEngine;

public interface IActionExecutable : IActionStats
{
	GameObjectManager ObjectManager { get; }

	GameObject GameObject { get; }

	ulong NetworkObjectId { get; }
}
