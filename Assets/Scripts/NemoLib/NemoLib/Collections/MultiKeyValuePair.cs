namespace NemoLib.Collections
{
	public struct MultiKeyValuePair<K1, K2, V>
	{
		public K1 Key1 { get; private set; }

		public K2 Key2 { get; private set; }

		public V Value { get; private set; }

		public MultiKeyValuePair(K1 key1, K2 key2, V value)
		{
			Key1 = key1;
			Key2 = key2;
			Value = value;
		}
	}
}
