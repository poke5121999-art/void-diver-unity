using System.Runtime.CompilerServices;

namespace SomeNet.Utility
{
	public static class Checker
	{
		public static void Assert(bool predicate, int errorCode, string message = null, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
		{
			if (!predicate)
			{
				throw new CustomException(errorCode, string.Format("[{0}] {1}", errorCode, message ?? ""), memberName, filePath, lineNumber);
			}
		}

		public static void Assert(int resultCode, string message = null, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
		{
			if (resultCode != 0)
			{
				throw new CustomException(resultCode, string.Format("[{0}] {1}", resultCode, message ?? ""), memberName, filePath, lineNumber);
			}
		}
	}
}
