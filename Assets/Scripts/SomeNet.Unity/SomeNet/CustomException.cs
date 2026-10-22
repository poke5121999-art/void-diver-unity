using System;
using System.Runtime.CompilerServices;

namespace SomeNet
{
	public class CustomException : Exception
	{
		private readonly string customStackTrace;

		public int ResultCode { get; private set; }

		public string MemberName { get; private set; }

		public string FilePath { get; private set; }

		public int LineNumber { get; private set; }

		public override string StackTrace => null;

		public CustomException(int resultCode, string message = "", [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0, string stackTrace = null)
		{
		}
	}
}
