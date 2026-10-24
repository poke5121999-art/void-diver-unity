using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace SomeNet.TypeAlias
{
	public static class TypeUtility
	{
		public struct TypeAndAttribute<TAttribute>
		{
			public Type Type;

			public TAttribute Attribute;
		}

		private static ConcurrentDictionary<string, Type> _typeCacheMap = new ConcurrentDictionary<string, Type>();

		public static IAssemblyProvider AssemblyProvider { get; set; } = new AssemblyProvider();

		public static Type GetType(string typeName)
		{
			if (typeName.Contains(", Version="))
			{
				return Type.GetType(typeName);
			}
			if (_typeCacheMap.TryGetValue(typeName, out var cached))
			{
				return cached;
			}
			foreach (Assembly assembly in AssemblyProvider.GetAssemblies())
			{
				foreach (Type type in GetTypes(assembly))
				{
					// vtable +0x378 is Type.FullName (metadata slot 36, machine code checked @0x182f7aae6)
					if (type.FullName == typeName)
					{
						_typeCacheMap.TryAdd(typeName, type);
						return type;
					}
				}
			}
			return null;
		}

		public static Assembly GetContainingAssembly(Type type)
		{
			// vtable +0x388 is Type.Assembly (metadata slot 37, machine code checked @0x182db6afc)
			return type.Assembly;
		}

		public static IEnumerable<Type> GetTypes(Assembly assembly)
		{
			Type[] typeArray;
			try
			{
				typeArray = assembly.GetTypes();
			}
			catch (ReflectionTypeLoadException ex)
			{
				typeArray = ex.Types;
			}
			for (int index = 0; index < typeArray.Length; index++)
			{
				Type type = typeArray[index];
				if (type != null)
				{
					yield return type;
				}
			}
		}

		// TODO(decomp): ProtoContractAttribute instantiation body is at 0x0 and the shared (object) body is only the iterator factory; MoveNext shape (GetTypes filter, GetCustomAttribute<T> != null) written from the state machine fields (type, attr)
		public static IEnumerable<TypeAndAttribute<TAttribute>> GetTypesContainingAttribute<TAttribute>(Assembly assembly) where TAttribute : Attribute
		{
			foreach (Type type in GetTypes(assembly))
			{
				TAttribute attr = type.GetCustomAttribute<TAttribute>();
				if (attr != null)
				{
					yield return new TypeAndAttribute<TAttribute>
					{
						Type = type,
						Attribute = attr
					};
				}
			}
		}

		public static IEnumerable<MethodInfo> GetExtensionMethods(Assembly assembly, Type extendedType)
		{
			return from m in GetTypes(assembly).SelectMany((Type t) => t.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
				where m.IsDefined(typeof(ExtensionAttribute), inherit: false)
				where m.GetParameters()[0].ParameterType == extendedType
				select m;
		}

		public static MethodInfo GetExtensionMethod(Assembly assembly, Type extendedType, string methodName)
		{
			return GetTypes(assembly).Select((Type t) => t.GetMethod(methodName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)).Where((MethodInfo m) => m != null).Where((MethodInfo m) => m.IsDefined(typeof(ExtensionAttribute), inherit: false))
				.Where((MethodInfo m) => m.GetParameters()[0].ParameterType == extendedType)
				.FirstOrDefault();
		}
	}
}
