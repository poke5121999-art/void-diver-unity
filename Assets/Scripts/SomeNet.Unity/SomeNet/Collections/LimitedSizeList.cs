using System.Collections.Generic;

namespace SomeNet.Collections
{
	// TODO(decomp): generic type has no body in the binary (open generic); bodies written from member signatures
	public class LimitedSizeList<T> : List<T>
	{
		private int maxSize;

		public LimitedSizeList(int maxSize)
		{
			this.maxSize = maxSize;
		}

		// TODO(decomp): overflow policy (drop oldest vs. ignore new) not readable from the binary; assumed drop oldest
		public new void Add(T item)
		{
			if (Count >= maxSize)
			{
				RemoveAt(0);
			}
			base.Add(item);
		}
	}
}
