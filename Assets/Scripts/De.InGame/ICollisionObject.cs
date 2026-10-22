public interface ICollisionObject
{
	NetworkBase NetworkBase { get; }

	ulong NetworkObjectId => 0uL;

	bool CanTrigger(ulong clientId);

	void OnTriggerEnter(ulong sourceClientId);
}
