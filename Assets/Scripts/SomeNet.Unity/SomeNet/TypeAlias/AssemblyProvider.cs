using System;
using System.Collections.Generic;
using System.Reflection;

namespace SomeNet.TypeAlias
{
	public class AssemblyProvider : IAssemblyProvider
	{
		public IEnumerable<Assembly> GetAssemblies()
		{
			return AppDomain.CurrentDomain.GetAssemblies();
		}
	}
}
