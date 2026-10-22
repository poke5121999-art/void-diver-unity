using System.Collections.Generic;

namespace SomeNet.Collections
{
	// TODO(decomp): generic type has no body in the binary (open generic); bodies written from member signatures
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

		// TODO(decomp): overflow policy (drop oldest vs. ignore new) not readable from the binary; assumed drop oldest
		public void Enqueue(T item)
		{
			if (queue.Count >= maxSize)
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
