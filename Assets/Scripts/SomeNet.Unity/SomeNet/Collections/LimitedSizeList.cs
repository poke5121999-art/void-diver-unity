using System.Collections.Generic;

namespace SomeNet.Collections
{
	public class LimitedSizeList<T> : List<T>
	{
		private int maxSize;

		public LimitedSizeList(int maxSize)
		{
			this.maxSize = maxSize;
		}

		// at capacity: drops the oldest entry (RemoveAt(0) then add; fully-shared body @0x1814136f0 machine code checked)
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
