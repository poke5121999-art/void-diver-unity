using System.IO;

namespace SomeNet.Utility
{
	public static class BinaryEncodedInt
	{
		public static void Write32BitEncodedInt(this Stream stream, int value)
		{
		}

		public static int Read32BitEncodedInt(this Stream stream)
		{
			return 0;
		}

		public static byte[] ReadBytes(this Stream stream, int length)
		{
			return null;
		}

		public static void Write7BitEncodedInt(this Stream stream, int value)
		{
		}

		public static int Read7BitEncodedInt(this Stream stream)
		{
			return 0;
		}

		public static void WriteZigZag7BitEncodedInt(this Stream stream, int value)
		{
		}

		public static int ReadZigZag7BitEncodedInt(this Stream stream)
		{
			return 0;
		}
	}
}
