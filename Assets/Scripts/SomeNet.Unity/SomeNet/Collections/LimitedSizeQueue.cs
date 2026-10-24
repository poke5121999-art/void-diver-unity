using System.Collections.Generic;

namespace SomeNet.Collections
{
	public class LimitedSizeQueue<T>
	{
		private Queue<T> queue;

		private int maxSize;

		public int Count => queue.Count;

		public LimitedSizeQueue(int maxSize)
		{
			this.maxSize = maxSize;
			queue = new Queue<T>();
		}

		// at capacity (Count == maxSize): dequeues the oldest first (fully-shared body @0x1814139c0 machine code checked)
		public void Enqueue(T item)
		{
			if (queue.Count == maxSize)
			{
				queue.Dequeue();
			}
			queue.Enqueue(item);
		}

		public T Dequeue()
		{
			return queue.Dequeue();
		}

		public T Peek()
		{
			return queue.Peek();
		}
	}
}
