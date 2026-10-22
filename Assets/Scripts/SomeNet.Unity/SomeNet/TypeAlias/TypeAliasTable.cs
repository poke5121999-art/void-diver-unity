using System;
using System.Collections.Generic;
using System.Reflection;

namespace SomeNet.TypeAlias
{
	public class TypeAliasTable
	{
		private Dictionary<int, Type> _alias2TypeMap;

		private Dictionary<Type, int> _type2AliasMap;

		public TypeAliasTable()
		{
		}

		public TypeAliasTable(IEnumerable<Assembly> assemblies, Func<Type, bool> filter = null)
		{
		}

		private void BuildTable(IEnumerable<Assembly> assemblies, Func<Type, bool> filter)
		{
		}

		public void AddTypeAlias(Type type)
		{
		}

		public void AddTypeAlias(Type type, int alias)
		{
		}

		public Type GetType(int alias)
		{
			return null;
		}

		public Type[] GetTypes()
		{
			return null;
		}

		public int GetAlias(Type type)
		{
			return 0;
		}

		public static int CalculateAlias(string name)
		{
			return 0;
		}

		private static uint CalculateDjb2Hash(string str)
		{
			return 0u;
		}
	}
}
