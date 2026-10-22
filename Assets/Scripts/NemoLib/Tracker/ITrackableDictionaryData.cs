namespace Tracker
{
	public interface ITrackableDictionaryData<TKey, TValue>
	{
		EDataOperation Operation { get; set; }

		TKey Key { get; }

		TValue Clone();
	}
}
