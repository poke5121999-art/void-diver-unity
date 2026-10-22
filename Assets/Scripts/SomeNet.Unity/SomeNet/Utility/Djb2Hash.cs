namespace SomeNet.Utility
{
	public static class Djb2Hash
	{
		public static uint GetHash(byte[] data)
		{
			if (data.Length == 0)
			{
				return 0u;
			}
			uint hash = 5381u;
			for (int i = 0; i < data.Length; i++)
			{
				hash = (hash << 5) + hash + data[i];
			}
			return hash;
		}

		public static int GetPositiveHash(byte[] data)
		{
			if (data == null || data.Length == 0)
			{
				return 0;
			}
			return MakePositiveHash(GetHash(data));
		}

		public static uint GetHash(byte[] data, int index, int length)
		{
			if (data == null || length == 0)
			{
				return 0u;
			}
			uint hash = 5381u;
			int end = index + length;
			for (int i = index; i < end; i++)
			{
				hash = (hash << 5) + hash + data[i];
			}
			return hash;
		}

		public static int GetPositiveHash(byte[] data, int index, int length)
		{
			if (data == null || length == 0)
			{
				return 0;
			}
			return MakePositiveHash(GetHash(data, index, length));
		}

		public static uint GetHash(string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				return 0u;
			}
			uint hash = 5381u;
			for (int i = 0; i < str.Length; i++)
			{
				hash = (hash << 5) + hash + str[i];
			}
			return hash;
		}

		public static int GetPositiveHash(string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				return 0;
			}
			return MakePositiveHash(GetHash(str));
		}

		public static int GetPositiveLessHash(string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				return 0;
			}
			return MakePositiveLessHash(GetHash(str));
		}

		private static int MakePositiveHash(uint hash)
		{
			int result = (int)(hash & 0x7FFFFFFF);
			return (result == 0) ? 1 : result;
		}

		private static int MakePositiveLessHash(uint hash)
		{
			int result = (int)(hash & 0x7FFFFFF);
			return (result == 0) ? 1 : result;
		}
	}
}
