using System.Collections.Generic;

namespace SomeNet.Collections
{
	// MultiKeyDictionary<object,object,object> bodies @0x1816b1d10.. and iterator MoveNexts @0x1812d06e0, @0x1812d2630 (machine code checked); TryGetValue is ContainsKey followed by the indexer
	public class MultiKeyDictionary<K1, K2, V> : Dictionary<KeyTuple<K1, K2>, V>
	{
		public V this[K1 key1, K2 key2]
		{
			get
			{
				return base[new KeyTuple<K1, K2>(key1, key2)];
			}
			set
			{
				base[new KeyTuple<K1, K2>(key1, key2)] = value;
			}
		}

		public IEnumerable<MultiKeyValuePair<K1, K2, V>> KeyValuePairs
		{
			get
			{
				foreach (KeyValuePair<KeyTuple<K1, K2>, V> pair in this)
				{
					yield return new MultiKeyValuePair<K1, K2, V>(pair.Key.Key1, pair.Key.Key2, pair.Value);
				}
			}
		}

		public IEnumerable<K1> FirstKeys
		{
			get
			{
				HashSet<K1> keySet = new HashSet<K1>();
				foreach (KeyValuePair<KeyTuple<K1, K2>, V> pair in this)
				{
					if (keySet.Add(pair.Key.Key1))
					{
						yield return pair.Key.Key1;
					}
				}
			}
		}

		public void Add(K1 key1, K2 key2, V value)
		{
			Add(new KeyTuple<K1, K2>(key1, key2), value);
		}

		public bool ContainsKey(K1 key1, K2 key2)
		{
			return ContainsKey(new KeyTuple<K1, K2>(key1, key2));
		}

		public bool TryGetValue(K1 key1, K2 key2, out V value)
		{
			if (ContainsKey(new KeyTuple<K1, K2>(key1, key2)))
			{
				value = base[new KeyTuple<K1, K2>(key1, key2)];
				return true;
			}
			value = default(V);
			return false;
		}
	}
}
