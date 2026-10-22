namespace SomeNet.Utility
{
	public static class Djb2Hash
	{
		public static uint GetHash(byte[] data)
		{
			return 0u;
		}

		public static int GetPositiveHash(byte[] data)
		{
			return 0;
		}

		public static uint GetHash(byte[] data, int index, int length)
		{
			return 0u;
		}

		public static int GetPositiveHash(byte[] data, int index, int length)
		{
			return 0;
		}

		public static uint GetHash(string str)
		{
			return 0u;
		}

		public static int GetPositiveHash(string str)
		{
			return 0;
		}

		public static int GetPositiveLessHash(string str)
		{
			return 0;
		}

		private static int MakePositiveHash(uint hash)
		{
			return 0;
		}

		private static int MakePositiveLessHash(uint hash)
		{
			return 0;
		}
	}
}
