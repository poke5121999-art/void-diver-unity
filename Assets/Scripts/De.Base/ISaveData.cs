public interface ISaveData
{
	string AccountId { get; set; }

	ISaveData Clone();

	void Normalize();
}
