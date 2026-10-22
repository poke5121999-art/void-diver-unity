using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SomeNet.Utility
{
	public static class JsonUtility
	{
		// Resolves "$type" ignoring the assembly part so tables load when the game assemblies are compiled under another name.
		private sealed class AssemblyAgnosticBinder : DefaultSerializationBinder
		{
			private readonly ConcurrentDictionary<string, Type> _byName = new ConcurrentDictionary<string, Type>();

			public override Type BindToType(string assemblyName, string typeName)
			{
				return _byName.GetOrAdd(typeName + "," + assemblyName, _ => Resolve(assemblyName, typeName));
			}

			private Type Resolve(string assemblyName, string typeName)
			{
				try
				{
					Type type = base.BindToType(assemblyName, typeName);
					if (type != null)
					{
						return type;
					}
				}
				catch (Exception)
				{
				}
				return AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType(typeName, false)).FirstOrDefault(t => t != null);
			}
		}

		private static JsonSerializerSettings settings = CreateSettings();

		private static JsonSerializerSettings CreateSettings()
		{
			var created = new JsonSerializerSettings
			{
				MaxDepth = null,
				ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
				NullValueHandling = NullValueHandling.Ignore,
				TypeNameHandling = TypeNameHandling.Auto,
				FloatFormatHandling = FloatFormatHandling.DefaultValue,
				Culture = CultureInfo.InvariantCulture,
				FloatParseHandling = FloatParseHandling.Double,
				SerializationBinder = new AssemblyAgnosticBinder(),
			};
			created.Converters.Add(new SafeFloatConverter());
			return created;
		}

		public static string Serialize(object value)
		{
			return JsonConvert.SerializeObject(value, settings);
		}

		public static T Deserialize<T>(string value)
		{
			return JsonConvert.DeserializeObject<T>(value, settings);
		}

		public static void AddConverter(params JsonConverter[] converters)
		{
			lock (settings)
			{
				foreach (JsonConverter converter in converters)
				{
					if (!settings.Converters.Any(c => c.GetType() == converter.GetType()))
					{
						settings.Converters.Add(converter);
					}
				}
			}
		}
	}
}
