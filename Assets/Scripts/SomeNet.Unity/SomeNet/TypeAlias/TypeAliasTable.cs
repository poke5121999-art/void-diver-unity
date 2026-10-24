using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ProtoBuf;

namespace SomeNet.TypeAlias
{
	public class TypeAliasTable
	{
		private Dictionary<int, Type> _alias2TypeMap;

		private Dictionary<Type, int> _type2AliasMap;

		public TypeAliasTable()
			: this(TypeUtility.AssemblyProvider.GetAssemblies(), null)
		{
		}

		public TypeAliasTable(IEnumerable<Assembly> assemblies, Func<Type, bool> filter = null)
		{
			BuildTable(assemblies, filter);
		}

		private void BuildTable(IEnumerable<Assembly> assemblies, Func<Type, bool> filter)
		{
			_alias2TypeMap = new Dictionary<int, Type>();
			_type2AliasMap = new Dictionary<Type, int>();
			if (assemblies == null)
			{
				return;
			}
			foreach (Assembly assembly in assemblies)
			{
				foreach (TypeUtility.TypeAndAttribute<ProtoContractAttribute> item in TypeUtility.GetTypesContainingAttribute<ProtoContractAttribute>(assembly))
				{
					Type type = item.Type;
					if (filter == null || filter(type))
					{
						// vtable +0x378 is Type.FullName (machine code checked @0x182f79b42)
						AddTypeAlias(type, CalculateAlias(type.FullName));
					}
				}
			}
		}

		public void AddTypeAlias(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			// vtable +0x378 is Type.FullName (machine code checked @0x182f797c6)
			AddTypeAlias(type, CalculateAlias(type.FullName));
		}

		public void AddTypeAlias(Type type, int alias)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (alias <= 0)
			{
				throw new ArgumentOutOfRangeException("alias");
			}
			if (_alias2TypeMap.TryGetValue(alias, out var value))
			{
				throw new InvalidOperationException(string.Format("Alias conflict. {0} and {1} have same alias {2}.", type, value, alias));
			}
			_type2AliasMap.Add(type, alias);
			_alias2TypeMap.Add(alias, type);
		}

		public Type GetType(int alias)
		{
			if (_alias2TypeMap.TryGetValue(alias, out var value))
			{
				return value;
			}
			return null;
		}

		public Type[] GetTypes()
		{
			return _alias2TypeMap.Values.ToArray();
		}

		public int GetAlias(Type type)
		{
			if (_type2AliasMap.TryGetValue(type, out var value))
			{
				return value;
			}
			return 0;
		}

		public static int CalculateAlias(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				return 0;
			}
			uint hash = 5381u;
			for (int i = 0; i < name.Length; i++)
			{
				hash = hash * 33 + name[i];
			}
			int result = (int)(hash & 0x7FFFFFFF);
			if (result < 128)
			{
				result *= 131;
			}
			return result;
		}

		private static uint CalculateDjb2Hash(string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				return 0u;
			}
			uint hash = 5381u;
			for (int i = 0; i < str.Length; i++)
			{
				hash = hash * 33 + str[i];
			}
			return hash;
		}
	}
}
