using System.Collections.Generic;

namespace Tracker
{
	public class TrackableDictionaryTracker<TKey, TValue> where TValue : class, ITrackableDictionaryData<TKey, TValue>
	{
		public class Change
		{
			public EDataOperation Operation;

			public TValue OldValue;

			public TValue NewValue;
		}

		public readonly Dictionary<TKey, Change> ChangeMap;

		public bool HasChange => false;

		public void TrackAdd(TKey key, TValue newValue)
		{
		}

		public void TrackRemove(TKey key, TValue oldValue)
		{
		}

		public void TrackModify(TKey key, TValue oldValue, TValue newValue)
		{
		}

		public void Clear()
		{
		}
	}
}
