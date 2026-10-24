using System;
using System.Collections.Generic;
using System.Linq;

namespace Tracker
{
	// TrackableDictionaryTracker<int,object> @0x181c03670 (TrackAdd), @0x181c04ac0 (TrackRemove), @0x181c043a0 (TrackModify) (machine code checked):
	// the three Track methods are a state table over the pending operation of the key, with three impossible transitions that throw
	public class TrackableDictionaryTracker<TKey, TValue> where TValue : class, ITrackableDictionaryData<TKey, TValue>
	{
		public class Change
		{
			public EDataOperation Operation;

			public TValue OldValue;

			public TValue NewValue;
		}

		public readonly Dictionary<TKey, Change> ChangeMap = new Dictionary<TKey, Change>();

		public bool HasChange => ChangeMap.Any();

		public void TrackAdd(TKey key, TValue newValue)
		{
			ChangeMap.TryGetValue(key, out var change);
			switch (change?.Operation ?? EDataOperation.None)
			{
			case EDataOperation.None:
				ChangeMap[key] = new Change
				{
					Operation = EDataOperation.Add,
					NewValue = newValue
				};
				break;
			case EDataOperation.Add:
				throw new InvalidOperationException("Add after add is impossible.");
			case EDataOperation.Remove:
				ChangeMap[key] = new Change
				{
					Operation = EDataOperation.Replace,
					OldValue = change.OldValue,
					NewValue = newValue
				};
				break;
			case EDataOperation.Replace:
				change.NewValue = newValue;
				break;
			}
		}

		public void TrackRemove(TKey key, TValue oldValue)
		{
			ChangeMap.TryGetValue(key, out var change);
			switch (change?.Operation ?? EDataOperation.None)
			{
			case EDataOperation.None:
				ChangeMap[key] = new Change
				{
					Operation = EDataOperation.Remove,
					OldValue = oldValue
				};
				break;
			case EDataOperation.Add:
				ChangeMap.Remove(key);
				break;
			case EDataOperation.Remove:
				throw new InvalidOperationException("Remove after remove is impossbile.");
			case EDataOperation.Replace:
				ChangeMap[key] = new Change
				{
					Operation = EDataOperation.Remove,
					OldValue = change.OldValue
				};
				break;
			}
		}

		public void TrackModify(TKey key, TValue oldValue, TValue newValue)
		{
			ChangeMap.TryGetValue(key, out var change);
			switch (change?.Operation ?? EDataOperation.None)
			{
			case EDataOperation.None:
				ChangeMap[key] = new Change
				{
					Operation = EDataOperation.Replace,
					OldValue = oldValue,
					NewValue = newValue
				};
				break;
			case EDataOperation.Add:
			case EDataOperation.Replace:
				change.NewValue = newValue;
				break;
			case EDataOperation.Remove:
				throw new InvalidOperationException("Modify after remove is impossible.");
			}
		}

		public void Clear()
		{
			ChangeMap.Clear();
		}
	}
}
