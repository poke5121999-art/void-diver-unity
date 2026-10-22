using System.Collections.Generic;

namespace Tracker
{
	// TODO(decomp): generic type has no pseudo-C; change-merging rules are reconstructed from the Change shape and EDataOperation
	public class TrackableDictionaryTracker<TKey, TValue> where TValue : class, ITrackableDictionaryData<TKey, TValue>
	{
		public class Change
		{
			public EDataOperation Operation;

			public TValue OldValue;

			public TValue NewValue;
		}

		public readonly Dictionary<TKey, Change> ChangeMap = new Dictionary<TKey, Change>();

		public bool HasChange => ChangeMap.Count > 0;

		public void TrackAdd(TKey key, TValue newValue)
		{
			if (ChangeMap.TryGetValue(key, out var change))
			{
				if (change.Operation == EDataOperation.Remove)
				{
					change.Operation = EDataOperation.Replace;
				}
				change.NewValue = newValue;
			}
			else
			{
				ChangeMap[key] = new Change
				{
					Operation = EDataOperation.Add,
					NewValue = newValue
				};
			}
		}

		public void TrackRemove(TKey key, TValue oldValue)
		{
			if (ChangeMap.TryGetValue(key, out var change))
			{
				if (change.Operation == EDataOperation.Add)
				{
					ChangeMap.Remove(key);
					return;
				}
				change.Operation = EDataOperation.Remove;
				change.NewValue = null;
			}
			else
			{
				ChangeMap[key] = new Change
				{
					Operation = EDataOperation.Remove,
					OldValue = oldValue
				};
			}
		}

		public void TrackModify(TKey key, TValue oldValue, TValue newValue)
		{
			if (ChangeMap.TryGetValue(key, out var change))
			{
				change.NewValue = newValue;
			}
			else
			{
				ChangeMap[key] = new Change
				{
					Operation = EDataOperation.Replace,
					OldValue = oldValue,
					NewValue = newValue
				};
			}
		}

		public void Clear()
		{
			ChangeMap.Clear();
		}
	}
}
