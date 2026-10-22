using System.Collections;
using System.Collections.Generic;

namespace Tracker
{
	// TODO(decomp): generic type has no pseudo-C; semantics reconstructed from member signatures (Clear vs ClearWithDirty, Flush merge are guesses)
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

		public TValue[] Flush()
		{
			List<TValue> result = new List<TValue>(tracker.ChangeMap.Count);
			foreach (TrackableDictionaryTracker<TKey, TValue>.Change change in tracker.ChangeMap.Values)
			{
				TValue source = ((change.Operation == EDataOperation.Remove) ? change.OldValue : change.NewValue);
				if (source != null)
				{
					TValue clone = source.Clone();
					clone.Operation = change.Operation;
					result.Add(clone);
				}
			}
			tracker.Clear();
			return result.ToArray();
		}

		public void Add(KeyValuePair<TKey, TValue> item)
		{
			Add(item.Key, item.Value);
		}

		public void ClearDirty()
		{
			tracker.Clear();
		}

		public void ClearWithDirty()
		{
			dict.Clear();
			tracker.Clear();
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
			return dict.TryGetValue(item.Key, out var value) && EqualityComparer<TValue>.Default.Equals(value, item.Value);
		}

		public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			((ICollection<KeyValuePair<TKey, TValue>>)dict).CopyTo(array, arrayIndex);
		}

		public bool Remove(KeyValuePair<TKey, TValue> item)
		{
			return Contains(item) && Remove(item.Key);
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
