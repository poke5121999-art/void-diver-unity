using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using Base.Utility;
using GameCommon.Enum;
using GameCommon.Table;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using SomeNet.Utility;

// UnityEngine.AnimationCurve is backed by native code that does not exist outside the player, so the tool run skips its JSON.
public sealed class SkipAnimationCurveConverter : JsonConverter<AnimationCurve>
{
	public override void WriteJson(JsonWriter writer, AnimationCurve value, JsonSerializer serializer)
	{
		writer.WriteNull();
	}

	public override AnimationCurve ReadJson(JsonReader reader, Type objectType, AnimationCurve existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		JToken.ReadFrom(reader);
		return null;
	}
}

// SkillAction's [OnDeserialized] hook builds an AnimationCurve (native) from MoveSpeedCurveData; the tool run drops the hook.
public sealed class NoCurveHookResolver : Newtonsoft.Json.Serialization.DefaultContractResolver
{
	protected override Newtonsoft.Json.Serialization.JsonObjectContract CreateObjectContract(Type objectType)
	{
		var contract = base.CreateObjectContract(objectType);
		if (objectType == typeof(GameCommon.Data.SkillAction) || objectType == typeof(GameCommon.Data.HitBoxInfo))
			contract.OnDeserializedCallbacks.Clear();
		return contract;
	}
}

public static class TableTests
{
	private const int ExamplesPerTable = 3;

	private static readonly string OracleDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "vd-src", "vd-ref", "json");

	private sealed class Mismatch
	{
		public string Column;
		public string Key;
		public string Expected;
		public string Actual;
	}

	// The reference assemblies carry two copies of a netcode helper type, which makes the runtime refuse to load any type
	// from them. Rewriting the copies in the run directory before first use lets game types that reference them resolve.
	private static void RepairReferenceAssemblies()
	{
		Assembly cecil = Assembly.LoadFrom(Path.Combine(AppContext.BaseDirectory, "Mono.Cecil.dll"));
		// Cecil resolves the game assembly by file name while writing; the test exe stands in for it during the rewrite.
		string stand = Path.Combine(AppContext.BaseDirectory, "De.Base.dll");
		File.Copy(Path.Combine(AppContext.BaseDirectory, "test.exe"), stand, true);
		try
		{
			RewriteDuplicates(cecil);
		}
		finally
		{
			File.Delete(stand);
		}
	}

	private static void RewriteDuplicates(Assembly cecil)
	{
		foreach (string path in Directory.GetFiles(AppContext.BaseDirectory, "*.dll"))
		{
			string file = Path.GetFileName(path);
			if (file != "NemoLib.dll")
			{
				continue;
			}
			// Both Mono.Cecil and Unity.Burst.Cecil are referenced, so Cecil is driven by reflection to avoid the type clash.
			Type definitionType = cecil.GetType("Mono.Cecil.AssemblyDefinition");
			object assembly;
			try
			{
				assembly = definitionType.GetMethod("ReadAssembly", new[] { typeof(string) }).Invoke(null, new object[] { path });
			}
			catch (TargetInvocationException)
			{
				continue;
			}
			object module = definitionType.GetProperty("MainModule").GetValue(assembly);
			var types = (IList)module.GetType().GetProperty("Types").GetValue(module);
			var seen = new HashSet<string>();
			bool changed = false;
			foreach (object type in types.Cast<object>().ToList())
			{
				var fullName = (string)type.GetType().GetProperty("FullName").GetValue(type);
				if (!seen.Add(fullName))
				{
					types.Remove(type);
					changed = true;
				}
			}
			if (changed)
			{
				using (var stream = new MemoryStream())
				{
					definitionType.GetMethod("Write", new[] { typeof(Stream) }).Invoke(assembly, new object[] { stream });
					((IDisposable)assembly).Dispose();
					File.WriteAllBytes(path, stream.ToArray());
				}
			}
			else
			{
				((IDisposable)assembly).Dispose();
			}
		}
	}

	public static int Main()
	{
		RepairReferenceAssemblies();
		return Run();
	}

	private static int Run()
	{
		SomeNet.Utility.JsonUtility.AddConverter(new SkipAnimationCurveConverter());
		var jsonSettings = (JsonSerializerSettings)typeof(SomeNet.Utility.JsonUtility).GetField("settings", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
		jsonSettings.ContractResolver = new NoCurveHookResolver();
		TableHelper.LoadEncryptedTables();

		string tableDir = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "TableEncrypted");
		var names = Directory.GetFiles(tableDir)
			.Where(f => f.EndsWith(".bytes") || f.EndsWith(".csv"))
			.Select(f => Path.GetFileNameWithoutExtension(f))
			.OrderBy(n => n, StringComparer.Ordinal)
			.ToList();

		int ok = 0;
		int fail = 0;
		foreach (string name in names)
		{
			string result;
			try
			{
				result = CompareTable(name);
			}
			catch (Exception e)
			{
				result = "exception " + (e.InnerException ?? e).GetType().Name + ": " + (e.InnerException ?? e).Message;
			}
			if (result == null)
			{
				ok++;
				Console.WriteLine("TABLE " + name + " OK");
			}
			else
			{
				fail++;
				Console.WriteLine("TABLE " + name + " FAIL " + result);
			}
		}

		string literal = CheckCharacterLiteral();
		if (literal != null)
		{
			fail++;
			Console.WriteLine("TABLE Character.literal FAIL " + literal);
		}
		else
		{
			ok++;
			Console.WriteLine("TABLE Character.literal OK");
		}

		Console.WriteLine("TABLES ok=" + ok + " fail=" + fail);
		return fail == 0 ? 0 : 1;
	}

	private static string CheckCharacterLiteral()
	{
		TCharacter c = TCharacter.Table[100001];
		var errors = new List<string>();
		void Expect(string field, object expected, object actual)
		{
			if (!Equals(expected, actual))
			{
				errors.Add(field + " expected " + expected + " got " + actual);
			}
		}
		Expect("Atk", 0, c.Atk);
		Expect("Def", 43, c.Def);
		Expect("Hp", 630, c.Hp);
		Expect("MoveSpeed", 2.3f, c.MoveSpeed);
		Expect("WeaponType", EWeaponType.Sword, c.WeaponType);
		Expect("ActiveSkillIds", "10010300,10010400,10010700,10010600,10010500", c.ActiveSkillIds == null ? null : string.Join(",", c.ActiveSkillIds));
		return errors.Count == 0 ? null : string.Join("; ", errors);
	}

	private static string CompareTable(string name)
	{
		Type rowType = typeof(TCharacter).Assembly.GetType("GameCommon.Table.T" + name);
		if (rowType == null)
		{
			return "no row type T" + name;
		}
		object table = rowType.GetProperty("Table", BindingFlags.Public | BindingFlags.Static).GetValue(null);
		if (table == null)
		{
			return "table not loaded";
		}
		string oraclePath = Path.Combine(OracleDir, name + ".json");
		if (!File.Exists(oraclePath))
		{
			return "no oracle " + oraclePath;
		}
		JArray expectedRows = JArray.Parse(File.ReadAllText(oraclePath));

		PropertyInfo[] props = rowType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
			.Where(p => p.CanRead && p.CanWrite && p.Name != "Pk" && p.Name != "OnDataLoaded")
			.ToArray();
		PropertyInfo pkProp = rowType.GetProperty("Pk");

		var actualRows = new Dictionary<string, object>();
		foreach (object row in (IEnumerable)table.GetType().GetProperty("Values").GetValue(table))
		{
			actualRows[Key(Canon(pkProp.GetValue(row), pkProp.PropertyType))] = row;
		}

		var problems = new List<string>();
		int count = (int)table.GetType().GetProperty("Count").GetValue(table);
		if (count != expectedRows.Count)
		{
			problems.Add("rows expected " + expectedRows.Count + " got " + count);
		}

		var byColumn = new Dictionary<string, List<Mismatch>>();
		foreach (JObject expectedRow in expectedRows)
		{
			var expectedByName = new Dictionary<string, JToken>();
			foreach (JProperty p in expectedRow.Properties())
			{
				expectedByName[p.Name.Split(':')[0].Trim()] = p.Value;
			}
			string key = Key(Norm(expectedByName[props[0].Name]));
			if (!actualRows.TryGetValue(key, out object actualRow))
			{
				AddMismatch(byColumn, props[0].Name, key, key, "<missing row>");
				continue;
			}
			foreach (PropertyInfo prop in props)
			{
				if (!expectedByName.TryGetValue(prop.Name, out JToken expectedValue))
				{
					AddMismatch(byColumn, prop.Name, key, "<no oracle column>", "");
					continue;
				}
				JToken expected = Norm(FixEnums(expectedValue, prop.PropertyType));
				JToken actual = Canon(prop.GetValue(actualRow), prop.PropertyType);
				if (expected is JValue emptyGoods && emptyGoods.Type == JTokenType.String && actual == null && (string)emptyGoods == "None")
				{
					expected = null;
				}
				if (!Same(expected, actual))
				{
					string path = FirstDifference(expected, actual, "");
					AddMismatch(byColumn, prop.Name, key, path != null ? "@" + path.Split('\u0001')[0] : Show(expected), path != null ? path.Split('\u0001')[1] : Show(actual));
				}
			}
		}

		PropertyInfo groupProp = rowType.GetProperty("GroupTable", BindingFlags.Public | BindingFlags.Static);
		if (groupProp != null)
		{
			object group = groupProp.GetValue(null);
			int grouped = 0;
			foreach (object pair in (IEnumerable)group)
			{
				grouped += ((Array)pair.GetType().GetProperty("Value").GetValue(pair)).Length;
			}
			if (group == null || grouped != count)
			{
				problems.Add("GroupTable holds " + grouped + " rows, table has " + count);
			}
		}

		var roundTrip = new Dictionary<string, List<Mismatch>>();
		MethodInfo toCsv = rowType.GetMethod("ToCsv");
		MethodInfo parse = rowType.GetMethod("Parse");
		foreach (object row in actualRows.Values)
		{
			string[] cells = SomeNet.Table.CsvHelper.Parse((string)toCsv.Invoke(row, null), false)[0];
			object copy = Activator.CreateInstance(rowType);
			parse.Invoke(copy, new object[] { cells });
			foreach (PropertyInfo prop in props)
			{
				JToken before = Canon(prop.GetValue(row), prop.PropertyType);
				JToken after = Canon(prop.GetValue(copy), prop.PropertyType);
				if (!Same(before, after))
				{
					AddMismatch(roundTrip, prop.Name, Key(Canon(pkProp.GetValue(row), pkProp.PropertyType)), Show(before), Show(after));
				}
			}
		}

		foreach (var pair in roundTrip.OrderBy(p => p.Key, StringComparer.Ordinal))
		{
			string examples = string.Join(" | ", pair.Value.Take(ExamplesPerTable).Select(m => "pk=" + m.Key + " before " + m.Expected + " after " + m.Actual));
			problems.Add("roundtrip col " + pair.Key + " x" + pair.Value.Count + " " + examples);
		}

		foreach (var pair in byColumn.OrderBy(p => p.Key, StringComparer.Ordinal))
		{
			string examples = string.Join(" | ", pair.Value.Take(ExamplesPerTable).Select(m => "pk=" + m.Key + " want " + m.Expected + " got " + m.Actual));
			problems.Add("col " + pair.Key + " x" + pair.Value.Count + " " + examples);
		}
		return problems.Count == 0 ? null : string.Join("; ", problems);
	}

	private static void AddMismatch(Dictionary<string, List<Mismatch>> byColumn, string column, string key, string expected, string actual)
	{
		if (!byColumn.TryGetValue(column, out List<Mismatch> list))
		{
			byColumn[column] = list = new List<Mismatch>();
		}
		list.Add(new Mismatch { Column = column, Key = key, Expected = expected, Actual = actual });
	}

	private static string Key(JToken token)
	{
		return token == null ? "null" : token.ToString(Newtonsoft.Json.Formatting.None);
	}

	private static string Show(JToken token)
	{
		string text = token == null ? "null" : token.ToString(Newtonsoft.Json.Formatting.None);
		return text.Length > 160 ? text.Substring(0, 160) + "..." : text;
	}

	// The oracle writes some enum cells as their numeric value; the loaded rows carry names.
	private static JToken FixEnums(JToken token, Type type)
	{
		Type element = type.IsArray ? type.GetElementType() : type;
		if (!element.IsEnum)
		{
			return token;
		}
		JToken Fix(JToken t)
		{
			bool numeric = t.Type == JTokenType.Integer || (t.Type == JTokenType.String && int.TryParse((string)t, NumberStyles.Integer, CultureInfo.InvariantCulture, out _));
			if (numeric && Enum.IsDefined(element, Convert.ToInt32((string)t, CultureInfo.InvariantCulture)))
			{
				return new JValue(Enum.GetName(element, Convert.ToInt32((string)t, CultureInfo.InvariantCulture)));
			}
			return t;
		}
		return token is JArray array ? new JArray(array.Select(Fix)) : Fix(token);
	}

	// Value of a loaded row field, rendered the way the oracle stores it, then normalised.
	private static JToken Canon(object value, Type type, bool topLevel = true)
	{
		if (value == null)
		{
			return null;
		}
		if (value is string || value is bool || type.IsPrimitive)
		{
			return Norm(new JValue(value));
		}
		if (type.IsEnum)
		{
			return Norm(new JValue(value.ToString()));
		}
		if (value is Array array)
		{
			Type element = array.GetType().GetElementType();
			if (element.IsPrimitive || element.IsEnum || element == typeof(string))
			{
				return Norm(new JArray(array.Cast<object>().Select(Plain)));
			}
			if (element.IsValueType)
			{
				return Norm(new JArray(array.Cast<object>().Select(e => Canon(e, element, false) ?? JValue.CreateNull())));
			}
		}
		if (topLevel && type.IsValueType && !type.Namespace.StartsWith("UnityEngine") && Equals(value, Activator.CreateInstance(type)))
		{
			return null;
		}
		string json = SomeNet.Utility.JsonUtility.Serialize(value);
		return Norm(JToken.Parse(json));
	}

	private static JToken Plain(object value)
	{
		return value is Enum ? new JValue(value.ToString()) : new JValue(value);
	}

	// Collapses representation differences: numeric/boolean strings, "a:b:c" tokens, empty values, float noise.
	private static JToken Norm(JToken token)
	{
		switch (token.Type)
		{
		case JTokenType.Null:
		case JTokenType.Undefined:
			return null;
		case JTokenType.Integer:
		case JTokenType.Float:
			return new JValue(Convert.ToDouble(((JValue)token).Value, CultureInfo.InvariantCulture));
		case JTokenType.Boolean:
			return token.DeepClone();
		case JTokenType.String:
			return NormString((string)token);
		case JTokenType.Array:
		{
			var items = token.Select(Norm).ToList();
			return items.Count == 0 ? null : TrimToken(new JArray(items.Select(i => i ?? JValue.CreateNull())));
		}
		case JTokenType.Object:
		{
			var result = new JObject();
			foreach (JProperty p in ((JObject)token).Properties().OrderBy(p => p.Name, StringComparer.Ordinal))
			{
				JToken value = p.Name == "MoveSpeedCurve" ? null : p.Name == "$type" ? new JValue(((string)p.Value).Split(',')[0].Trim()) : Norm(p.Value);
				if (value != null)
				{
					result[p.Name] = value;
				}
			}
			return result.Count == 0 ? null : result;
		}
		default:
			return token.DeepClone();
		}
	}

	// "Type:Id:0" and ["Type", id] are the same goods/condition token: drop trailing zero parts, unwrap a lone name.
	private static JToken TrimToken(JArray array)
	{
		if (array.Count < 2 || array[0].Type != JTokenType.String)
		{
			return array;
		}
		var items = array.ToList();
		while (items.Count > 1 && items[items.Count - 1].Type == JTokenType.Float && (double)items[items.Count - 1] == 0)
		{
			items.RemoveAt(items.Count - 1);
		}
		return items.Count == 1 ? items[0] : new JArray(items);
	}

	private static JToken NormString(string text)
	{
		text = text.Replace("{$}", ",").Trim();
		if (text.Length == 0)
		{
			return null;
		}
		if (double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out double number) && !text.Contains(" "))
		{
			return new JValue(number);
		}
		if (text.Equals("true", StringComparison.OrdinalIgnoreCase))
		{
			return new JValue(true);
		}
		if (text.Equals("false", StringComparison.OrdinalIgnoreCase))
		{
			return new JValue(false);
		}
		if (text.Contains(":") && !text.Contains(" ") && text.Length < 200)
		{
			return TrimToken(new JArray(text.Split(':').Select(p => NormString(p) ?? new JValue(""))));
		}
		return new JValue(text);
	}

	private static string FirstDifference(JToken a, JToken b, string path)
	{
		if (Same(a, b))
		{
			return null;
		}
		if (a is JObject ao && b is JObject bo)
		{
			foreach (string name in ao.Properties().Select(p => p.Name).Union(bo.Properties().Select(p => p.Name)))
			{
				string inner = FirstDifference(ao[name], bo[name], path + "." + name);
				if (inner != null)
				{
					return inner;
				}
			}
		}
		else if (a is JArray aa && b is JArray ba && aa.Count == ba.Count)
		{
			for (int i = 0; i < aa.Count; i++)
			{
				string inner = FirstDifference(aa[i], ba[i], path + "[" + i + "]");
				if (inner != null)
				{
					return inner;
				}
			}
		}
		return path + " want " + Show(a) + "\u0001got " + Show(b);
	}

	private static bool Same(JToken a, JToken b)
	{
		if (a == null || b == null)
		{
			return a == null && b == null;
		}
		if (a.Type == JTokenType.Float && b.Type == JTokenType.Float)
		{
			double x = (double)a;
			double y = (double)b;
			return x == y || Math.Abs(x - y) <= 1e-3 + 1e-4 * Math.Max(Math.Abs(x), Math.Abs(y));
		}
		if (a.Type != b.Type)
		{
			return false;
		}
		if (a is JArray aa)
		{
			var ba = (JArray)b;
			return aa.Count == ba.Count && aa.Zip(ba, Same).All(r => r);
		}
		if (a is JObject ao)
		{
			var bo = (JObject)b;
			return ao.Count == bo.Count && ao.Properties().All(p => bo.TryGetValue(p.Name, out JToken other) && Same(p.Value, other));
		}
		return JToken.DeepEquals(a, b);
	}
}
