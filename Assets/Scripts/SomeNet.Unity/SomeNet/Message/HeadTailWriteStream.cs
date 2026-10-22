using System;
using System.IO;

namespace SomeNet.Message
{
	public class HeadTailWriteStream : Stream
	{
		private ArraySegment<byte> _head;

		private ArraySegment<byte>? _tail;

		private int _pos;

		private int _length;

		private int _size;

		public override bool CanRead => false;

		public override bool CanSeek => false;

		public override bool CanWrite => false;

		public override long Length => 0L;

		public override long Position
		{
			get
			{
				return 0L;
			}
			set
			{
			}
		}

		public ArraySegment<byte>? Tail => null;

		public HeadTailWriteStream(ArraySegment<byte> head, int tailSize = 0)
		{
		}

		public override void Flush()
		{
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			return 0L;
		}

		public override void SetLength(long length)
		{
		}

		private void EnsureCapacity(int size)
		{
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			return 0;
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
		}

		public void GetBuffers(int pos, int length, out ArraySegment<byte> segment0, out ArraySegment<byte> segment1)
		{
			segment0 = default;
			segment1 = default;
		}
	}
}
