using System.Collections.Generic;

namespace SomeNet.Collections
{
	public class LimitedSizeQueue<T>
	{
		private Queue<T> queue;

		private int maxSize;

		public int Count => 0;

		public LimitedSizeQueue(int maxSize)
		{
		}

		public void Enqueue(T item)
		{
		}

		public T Dequeue()
		{
			return default;
		}

		public T Peek()
		{
			return default;
		}
	}
}
