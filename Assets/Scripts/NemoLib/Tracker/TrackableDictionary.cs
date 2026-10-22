using System.Collections;
using System.Collections.Generic;

namespace Tracker
{
	public class TrackableDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable where TValue : class, ITrackableDictionaryData<TKey, TValue>
	{
		internal readonly Dictionary<TKey, TValue> dict;

		public readonly TrackableDictionaryTracker<TKey, TValue> tracker;

		public bool HasChange => false;

		public TValue this[TKey key]
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public ICollection<TKey> Keys => null;

		public ICollection<TValue> Values => null;

		public int Count => 0;

		public bool IsReadOnly => false;

		public TrackableDictionary()
		{
		}

		public TrackableDictionary(IDictionary<TKey, TValue> dictionary)
		{
		}

		public bool ContainsKey(TKey key)
		{
			return false;
		}

		public void Add(TKey key, TValue value)
		{
		}

		public void AddRange(IEnumerable<TValue> collection)
		{
		}

		public bool Remove(TKey key)
		{
			return false;
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
			value = null;
			return false;
		}

		public TValue[] Flush()
		{
			return null;
		}

		public void Add(KeyValuePair<TKey, TValue> item)
		{
		}

		public void ClearDirty()
		{
		}

		public void ClearWithDirty()
		{
		}

		public void Clear()
		{
		}

		public bool Contains(KeyValuePair<TKey, TValue> item)
		{
			return false;
		}

		public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
		}

		public bool Remove(KeyValuePair<TKey, TValue> item)
		{
			return false;
		}

		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			return null;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return null;
		}
	}
}
