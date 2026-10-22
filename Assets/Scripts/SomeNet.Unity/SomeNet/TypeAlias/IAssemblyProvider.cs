using System.Collections.Generic;
using System.Reflection;

namespace SomeNet.TypeAlias
{
	public interface IAssemblyProvider
	{
		IEnumerable<Assembly> GetAssemblies();
	}
}
