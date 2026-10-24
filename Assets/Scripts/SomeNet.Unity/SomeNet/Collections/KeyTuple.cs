using System;

namespace SomeNet.Collections
{
	public struct KeyTuple<K1, K2> : IEquatable<KeyTuple<K1, K2>>
	{
		private readonly K1 key1;

		private readonly K2 key2;

		public K1 Key1 => key1;

		public K2 Key2 => key2;

		public KeyTuple(K1 key1, K2 key2)
		{
			this.key1 = key1;
			this.key2 = key2;
		}

		// KeyTuple<object,object> @0x1813e6850 (hash), @0x1813e4fc0 (Equals(object)), @0x1813e4c30 (Equals(KeyTuple)) (machine code checked):
		// plain virtual GetHashCode/Equals on the keys, no EqualityComparer; a null key throws
		public override int GetHashCode()
		{
			return key1.GetHashCode() ^ key2.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (obj == null || GetType() != obj.GetType())
			{
				return false;
			}
			return Equals((KeyTuple<K1, K2>)obj);
		}

		public bool Equals(KeyTuple<K1, K2> other)
		{
			return other.key1.Equals(key1) && other.key2.Equals(key2);
		}
	}
}
