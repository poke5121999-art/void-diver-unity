using System;

namespace SomeNet.Collections
{
	public struct KeyTuple<K1, K2> : IEquatable<KeyTuple<K1, K2>>
	{
		private readonly K1 key1;

		private readonly K2 key2;

		public K1 Key1 => default;

		public K2 Key2 => default;

		public KeyTuple(K1 key1, K2 key2)
		{
			this.key1 = default;
			this.key2 = default;
		}

		public override int GetHashCode()
		{
			return 0;
		}

		public override bool Equals(object obj)
		{
			return false;
		}

		public bool Equals(KeyTuple<K1, K2> other)
		{
			return false;
		}
	}
}
