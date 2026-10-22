using System.Collections.Generic;
using System.Reflection;

namespace SomeNet.TypeAlias
{
	public class FixedAssemblyProvider : IAssemblyProvider
	{
		private Assembly[] _assemblies;

		public FixedAssemblyProvider(Assembly[] assemblies)
		{
		}

		public IEnumerable<Assembly> GetAssemblies()
		{
			return null;
		}
	}
}
