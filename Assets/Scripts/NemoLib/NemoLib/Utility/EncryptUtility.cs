using System.IO;
using System.Text;

namespace NemoLib.Utility
{
	public static class EncryptUtility
	{
		public static class CsvDecryptor
		{
			private const byte KEY = 90;

			public static string Decrypt(byte[] encryptedBytes)
			{
				for (int i = 0; i < encryptedBytes.Length; i++)
				{
					encryptedBytes[i] ^= KEY;
				}
				return Encoding.UTF8.GetString(encryptedBytes);
			}
		}

		private const byte K1 = 18;

		private const byte K2 = 52;

		private static byte GetKey(byte externalKey)
		{
			return (byte)(externalKey ^ K1 ^ K2);
		}

		public static void EncryptFile(string inputPath, string outputPath, byte externalKey)
		{
			string text = File.ReadAllText(inputPath, Encoding.UTF8);
			byte[] bytes = Encoding.UTF8.GetBytes(text);
			byte key = GetKey(externalKey);
			for (int i = 0; i < bytes.Length; i++)
			{
				bytes[i] ^= key;
			}
			File.WriteAllBytes(outputPath, bytes);
		}

		public static string DecryptFile(string filePath, byte externalKey)
		{
			if (!File.Exists(filePath))
			{
				throw new FileNotFoundException("Encrypted file not found: " + filePath);
			}
			byte[] array = File.ReadAllBytes(filePath);
			byte key = GetKey(externalKey);
			for (int i = 0; i < array.Length; i++)
			{
				array[i] ^= key;
			}
			return Encoding.UTF8.GetString(array);
		}
	}
}
