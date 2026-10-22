using System.Runtime.CompilerServices;

namespace SomeNet.Utility
{
	public static class Checker
	{
		public static void Assert(bool predicate, int errorCode, string message = null, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
		{
		}

		public static void Assert(int resultCode, string message = null, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
		{
		}
	}
}
