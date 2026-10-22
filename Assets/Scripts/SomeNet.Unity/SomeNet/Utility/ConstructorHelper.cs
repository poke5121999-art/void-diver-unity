using System;

namespace SomeNet.Utility
{
	public static class ConstructorHelper
	{
		public delegate object ConstructorDelegate(params object[] args);

		public static ConstructorDelegate CreateConstructor(Type type, params Type[] parameters)
		{
			return null;
		}
	}
}
