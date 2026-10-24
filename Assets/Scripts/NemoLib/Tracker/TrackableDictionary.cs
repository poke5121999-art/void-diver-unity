using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Tracker
{
	// TrackableDictionary<int,object> bodies @0x181c05690..0x181c09310 (machine code checked); Flush, Clear, Contains and Remove(item) follow them rather than the member names
	public class TrackableDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable where TValue : class, ITrackableDictionaryData<TKey, TValue>
	{
		internal readonly Dictionary<TKey, TValue> dict;

		public readonly TrackableDictionaryTracker<TKey, TValue> tracker;

		public bool HasChange => tracker.HasChange;

		public TValue this[TKey key]
		{
			get
			{
				return dict[key];
			}
			set
			{
				if (dict.TryGetValue(key, out var oldValue))
				{
					dict[key] = value;
					tracker.TrackModify(key, oldValue, value);
				}
				else
				{
					dict[key] = value;
					tracker.TrackAdd(key, value);
				}
			}
		}

		public ICollection<TKey> Keys => dict.Keys;

		public ICollection<TValue> Values => dict.Values;

		public int Count => dict.Count;

		public bool IsReadOnly => false;

		public TrackableDictionary()
		{
			dict = new Dictionary<TKey, TValue>();
			tracker = new TrackableDictionaryTracker<TKey, TValue>();
		}

		public TrackableDictionary(IDictionary<TKey, TValue> dictionary)
		{
			dict = new Dictionary<TKey, TValue>(dictionary);
			tracker = new TrackableDictionaryTracker<TKey, TValue>();
		}

		public bool ContainsKey(TKey key)
		{
			return dict.ContainsKey(key);
		}

		public void Add(TKey key, TValue value)
		{
			dict.Add(key, value);
			tracker.TrackAdd(key, value);
		}

		public void AddRange(IEnumerable<TValue> collection)
		{
			foreach (TValue item in collection)
			{
				Add(item.Key, item);
			}
		}

		public bool Remove(TKey key)
		{
			if (dict.TryGetValue(key, out var oldValue) && dict.Remove(key))
			{
				tracker.TrackRemove(key, oldValue);
				return true;
			}
			return false;
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
			return dict.TryGetValue(key, out value);
		}

		// Flush<int,object> @0x181c06810 and the Select lambda <>c.b__18_0 @0x180f38020: null when nothing changed, else one clone per pending change
		// (the old value for a Remove, the new value otherwise) carrying that change's operation, then the change map is cleared
		public TValue[] Flush()
		{
			if (tracker.ChangeMap.Any())
			{
				TValue[] result = tracker.ChangeMap.Values.Select(delegate(TrackableDictionaryTracker<TKey, TValue>.Change _)
				{
					TValue val = ((_.Operation == EDataOperation.Remove) ? _.OldValue : _.NewValue).Clone();
					val.Operation = _.Operation;
					return val;
				}).ToArray();
				tracker.ChangeMap.Clear();
				return result;
			}
			return null;
		}

		public void Add(KeyValuePair<TKey, TValue> item)
		{
			Add(item.Key, item.Value);
		}

		public void ClearDirty()
		{
			tracker.ChangeMap.Clear();
		}

		public void ClearWithDirty()
		{
			dict.Clear();
			tracker.ChangeMap.Clear();
		}

		public void Clear()
		{
			foreach (KeyValuePair<TKey, TValue> pair in dict)
			{
				tracker.TrackRemove(pair.Key, pair.Value);
			}
			dict.Clear();
		}

		public bool Contains(KeyValuePair<TKey, TValue> item)
		{
			return ((ICollection<KeyValuePair<TKey, TValue>>)dict).Contains(item);
		}

		public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			((ICollection<KeyValuePair<TKey, TValue>>)dict).CopyTo(array, arrayIndex);
		}

		public bool Remove(KeyValuePair<TKey, TValue> item)
		{
			if (((ICollection<KeyValuePair<TKey, TValue>>)dict).Remove(item))
			{
				tracker.TrackRemove(item.Key, item.Value);
				return true;
			}
			return false;
		}

		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			return dict.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return dict.GetEnumerator();
		}
	}
}
