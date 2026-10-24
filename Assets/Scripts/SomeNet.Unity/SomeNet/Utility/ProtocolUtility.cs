using System;
using System.Collections.Generic;
using System.IO;
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
					// Add(type, false) then SetSurrogate(type) on the returned MetaType (machine code checked @0x182f7800d)
					typeModel.Add(type, false).SetSurrogate(type);
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
			// Serialize into a MemoryStream, rewind, Deserialize (shared body @0x180d7b5e0 machine code checked; protobuf-net Serializer.DeepClone is the same sequence)
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Serializer.Serialize(memoryStream, self);
				memoryStream.Position = 0L;
				return Serializer.Deserialize<T>(memoryStream);
			}
		}
	}
}
