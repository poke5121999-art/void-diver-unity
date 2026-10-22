namespace Tracker
{
	public interface ITrackableData<TValue>
	{
		EDataOperation Operation { get; set; }

		TValue Clone();
	}
}
