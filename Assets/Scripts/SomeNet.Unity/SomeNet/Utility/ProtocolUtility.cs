using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ProtoBuf;
using ProtoBuf.Meta;

namespace SomeNet.Utility
{
	public static class ProtocolUtility
	{
		public static RuntimeTypeModel CreateTypeModel(IEnumerable<Assembly> assemblies)
		{
			RuntimeTypeModel typeModel = RuntimeTypeModel.Create("CreateTypeModel");
			foreach (Assembly assembly in assemblies)
			{
				AddTypes(typeModel, assembly);
			}
			return typeModel;
		}

		private static void AddTypes(RuntimeTypeModel typeModel, Assembly assembly)
		{
			foreach (Type type in GetTypesSafely(assembly))
			{
				if ((type.IsClass || type.IsValueType) && Attribute.GetCustomAttribute(type, typeof(ProtoContractAttribute)) != null && !typeModel.CanSerialize(type))
				{
					// TODO(decomp): the registration call after CanSerialize is past the decoded range; assumed typeModel.Add(type, true) @0x182f7800d
					typeModel.Add(type, true);
				}
			}
		}

		private static IEnumerable<Type> GetTypesSafely(Assembly assembly)
		{
			Type[] types;
			try
			{
				types = assembly.GetTypes();
			}
			catch (ReflectionTypeLoadException ex)
			{
				types = ex.Types;
			}
			return types.Where(x => x != null);
		}

		public static T DeepCopy<T>(T self)
		{
			// TODO(decomp): generic method has no body of its own in the binary (@0x0); assumed protobuf-net DeepClone.
			return Serializer.DeepClone(self);
		}
	}
}
