using System;

namespace GameCommon
{
	[AttributeUsage(AttributeTargets.Enum)]
	public class LocalizableEnumAttribute : Attribute
	{
		public string[] ExtraKeys { get; }

		public LocalizableEnumAttribute(params string[] extraKeys)
		{
		}
	}
}
