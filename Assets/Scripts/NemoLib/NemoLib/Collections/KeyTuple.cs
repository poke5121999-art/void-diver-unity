using System;
using System.Collections.Generic;

namespace NemoLib.Collections
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

		public override int GetHashCode()
		{
			// TODO(decomp): generic type has no pseudo-C; hash combination is a stand-in (not observable, only equality matters)
			return (EqualityComparer<K1>.Default.GetHashCode(key1) * 397) ^ EqualityComparer<K2>.Default.GetHashCode(key2);
		}

		public override bool Equals(object obj)
		{
			return obj is KeyTuple<K1, K2> other && Equals(other);
		}

		public bool Equals(KeyTuple<K1, K2> other)
		{
			return EqualityComparer<K1>.Default.Equals(key1, other.key1) && EqualityComparer<K2>.Default.Equals(key2, other.key2);
		}
	}
}
