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

		public override bool CanSeek => true;

		public override bool CanWrite => true;

		public override long Length => _length;

		public override long Position
		{
			get
			{
				return _pos;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				_pos = (int)value;
			}
		}

		public ArraySegment<byte>? Tail => _tail;

		public HeadTailWriteStream(ArraySegment<byte> head, int tailSize = 0)
		{
			_head = head;
			if (tailSize > 0)
			{
				_tail = new ArraySegment<byte>(new byte[tailSize]);
			}
			_size = head.Count + tailSize;
		}

		public override void Flush()
		{
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			long position;
			switch (origin)
			{
			case SeekOrigin.Begin:
				position = offset;
				break;
			case SeekOrigin.Current:
				position = _pos + offset;
				break;
			case SeekOrigin.End:
				position = _length + offset;
				break;
			default:
				throw new ArgumentException("Invalid origin");
			}
			Position = position;
			return position;
		}

		public override void SetLength(long length)
		{
			_length = (int)length;
			EnsureCapacity((int)length);
		}

		private void EnsureCapacity(int size)
		{
			if (size <= _size)
			{
				return;
			}
			int needed = size - _head.Count;
			int oldTailLength = _tail.HasValue ? _tail.Value.Count : 0;
			int newTailLength = Math.Max(needed, 256);
			newTailLength = Math.Max(newTailLength, oldTailLength * 2);
			byte[] array = new byte[newTailLength];
			if (_tail.HasValue)
			{
				Array.Copy(_tail.Value.Array, _tail.Value.Offset, array, 0, oldTailLength);
			}
			_tail = new ArraySegment<byte>(array);
			_size = _head.Count + newTailLength;
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			throw new IOException();
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			int endPos = _pos + count;
			EnsureCapacity(endPos);
			int headCount = _head.Count;
			if (endPos <= headCount)
			{
				int dst = _pos + _head.Offset;
				if (count > 8)
				{
					Array.Copy(buffer, offset, _head.Array, dst, count);
				}
				else
				{
					for (int i = 0; i < count; i++)
					{
						_head.Array[dst + i] = buffer[offset + i];
					}
				}
			}
			else if (_pos < headCount)
			{
				int headPart = headCount - _pos;
				int tailPart = endPos - headCount;
				// Binary copies from buffer index 0 / headPart here and ignores `offset` (kept as decompiled).
				Array.Copy(buffer, 0, _head.Array, _pos + _head.Offset, headPart);
				Array.Copy(buffer, headPart, _tail.Value.Array, _tail.Value.Offset, tailPart);
			}
			else
			{
				int dst = _tail.Value.Offset - headCount + _pos;
				if (count > 8)
				{
					Array.Copy(buffer, offset, _tail.Value.Array, dst, count);
				}
				else
				{
					for (int j = 0; j < count; j++)
					{
						_tail.Value.Array[dst + j] = buffer[offset + j];
					}
				}
			}
			_pos = endPos;
			if (_length < endPos)
			{
				_length = endPos;
			}
		}

		public void GetBuffers(int pos, int length, out ArraySegment<byte> segment0, out ArraySegment<byte> segment1)
		{
			int end = pos + length;
			if (pos < 0 || end > _length)
			{
				throw new ArgumentOutOfRangeException();
			}
			int headCount = _head.Count;
			if (pos < headCount && end <= headCount)
			{
				segment0 = new ArraySegment<byte>(_head.Array, _head.Offset + pos, length);
				segment1 = default;
			}
			else if (pos >= headCount && end >= headCount)
			{
				segment0 = new ArraySegment<byte>(_tail.Value.Array, _tail.Value.Offset - headCount + pos, length);
				segment1 = default;
			}
			else
			{
				int headPart = headCount - pos;
				segment0 = new ArraySegment<byte>(_head.Array, _head.Offset + pos, headPart);
				segment1 = new ArraySegment<byte>(_tail.Value.Array, _tail.Value.Offset, length - headPart);
			}
		}
	}
}
