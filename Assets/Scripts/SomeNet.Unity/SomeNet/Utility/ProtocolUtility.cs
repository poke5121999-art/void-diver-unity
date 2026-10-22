using System;
using System.Collections.Generic;
using System.Reflection;
using ProtoBuf.Meta;

namespace SomeNet.Utility
{
	public static class ProtocolUtility
	{
		public static RuntimeTypeModel CreateTypeModel(IEnumerable<Assembly> assemblies)
		{
			return null;
		}

		private static void AddTypes(RuntimeTypeModel typeModel, Assembly assembly)
		{
		}

		private static IEnumerable<Type> GetTypesSafely(Assembly assembly)
		{
			return null;
		}

		public static T DeepCopy<T>(T self)
		{
			return default;
		}
	}
}
