using System;
using System.IO;

namespace SomeNet.Utility
{
	public static class BinaryEncodedInt
	{
		public static void Write32BitEncodedInt(this Stream stream, int value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			stream.Write(bytes, 0, bytes.Length);
		}

		public static int Read32BitEncodedInt(this Stream stream)
		{
			byte[] buffer = new byte[4];
			stream.Read(buffer, 0, 4);
			return BitConverter.ToInt32(buffer, 0);
		}

		public static byte[] ReadBytes(this Stream stream, int length)
		{
			byte[] buffer = new byte[length];
			stream.Read(buffer, 0, length);
			return buffer;
		}

		public static void Write7BitEncodedInt(this Stream stream, int value)
		{
			uint v = (uint)value;
			while (true)
			{
				byte low = (byte)(v & 0x7F);
				v >>= 7;
				if (v == 0)
				{
					stream.WriteByte(low);
					return;
				}
				stream.WriteByte((byte)(low | 0x80));
			}
		}

		public static int Read7BitEncodedInt(this Stream stream)
		{
			int result = 0;
			int shift = 0;
			int count = 0;
			while (true)
			{
				int b = stream.ReadByte();
				result |= (b & 0x7F) << (shift & 0x1F);
				shift += 7;
				if ((b & 0x80) == 0)
				{
					break;
				}
				if (++count >= 5)
				{
					break;
				}
			}
			return result;
		}

		public static void WriteZigZag7BitEncodedInt(this Stream stream, int value)
		{
			stream.Write7BitEncodedInt((value << 1) ^ (value >> 31));
		}

		public static int ReadZigZag7BitEncodedInt(this Stream stream)
		{
			int v = stream.Read7BitEncodedInt();
			return (v >> 1) ^ -(v & 1);
		}
	}
}
