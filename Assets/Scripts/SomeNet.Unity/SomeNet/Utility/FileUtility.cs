using System.IO;

namespace SomeNet.Utility
{
	public static class FileUtility
	{
		public static bool AreFilesEqual(string file1, string file2)
		{
			const int BufferSize = 0x100000;
			byte[] buffer1 = new byte[BufferSize];
			byte[] buffer2 = new byte[BufferSize];
			using (FileStream stream1 = new FileStream(file1, FileMode.Open, FileAccess.Read, FileShare.None))
			using (FileStream stream2 = new FileStream(file2, FileMode.Open, FileAccess.Read, FileShare.None))
			{
				if (stream1.Length != stream2.Length)
				{
					return false;
				}
				int read1;
				while ((read1 = stream1.Read(buffer1, 0, BufferSize)) > 0)
				{
					int read2 = stream2.Read(buffer2, 0, BufferSize);
					if (read1 != read2)
					{
						return false;
					}
					for (int i = 0; i < read1; i++)
					{
						if (buffer1[i] != buffer2[i])
						{
							return false;
						}
					}
				}
			}
			return true;
		}
	}
}
