using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace VD.EditorTools
{
    // Batchmode: -executeMethod VD.EditorTools.CatalogDump.Run [-vdCatalog <catalog.bin>] [-vdCatalogOut captures/catalog.json]
    // Reads the shipped game's binary Addressables catalog with the package's own (internal) reader and writes one
    // JSON object per location: keys, primary key, internal id (asset GUID for bundled assets), type, provider, and the
    // bundle that holds it.
    public static class CatalogDump
    {
        public static void Run()
        {
            string path = Arg("-vdCatalog", "Assets/StreamingAssets/aa/catalog.bin");
            string outPath = Arg("-vdCatalogOut", "captures/catalog.json");

            var dataType = typeof(Addressables).Assembly.GetType("UnityEngine.AddressableAssets.ResourceLocators.ContentCatalogData");
            const BindingFlags any = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;
            var load = dataType.GetMethod("LoadFromFile", any, null, new[] { typeof(string), typeof(bool) }, null);
            var data = load.Invoke(null, new object[] { path, false });
            var locator = (IResourceLocator)dataType.GetMethod("CreateCustomLocator", any).Invoke(data, new object[] { "", null });

            var byLocation = new Dictionary<string, (IResourceLocation loc, List<string> keys)>();
            int keyCount = 0;
            foreach (object key in locator.Keys)
            {
                keyCount++;
                if (!locator.Locate(key, null, out var locs)) continue;
                foreach (var loc in locs)
                {
                    string id = loc.InternalId + "|" + loc.ResourceType?.FullName;
                    if (!byLocation.TryGetValue(id, out var entry))
                        byLocation[id] = entry = (loc, new List<string>());
                    entry.keys.Add(key.ToString());
                }
            }

            var sb = new StringBuilder("[\n");
            bool first = true;
            foreach (var (loc, keys) in byLocation.Values.OrderBy(e => e.loc.PrimaryKey, StringComparer.Ordinal))
            {
                var bundles = loc.HasDependencies ? loc.Dependencies.Take(1).Select(d => BundleName(d.InternalId)) : Enumerable.Empty<string>();
                if (!first) sb.Append(",\n");
                first = false;
                sb.Append("{\"primaryKey\":").Append(Q(loc.PrimaryKey))
                  .Append(",\"internalId\":").Append(Q(loc.InternalId))
                  .Append(",\"type\":").Append(Q(loc.ResourceType?.FullName))
                  .Append(",\"provider\":").Append(Q(loc.ProviderId))
                  .Append(",\"keys\":[").Append(string.Join(",", keys.Distinct().Select(Q))).Append(']')
                  .Append(",\"bundles\":[").Append(string.Join(",", bundles.Select(Q))).Append("]}");
            }
            sb.Append("\n]\n");
            Directory.CreateDirectory(Path.GetDirectoryName(outPath));
            File.WriteAllText(outPath, sb.ToString());
            Debug.Log($"VD_CATALOG keys={keyCount} locations={byLocation.Count} -> {outPath}");
        }

        // "{RuntimePath}\\StandaloneWindows64\\remote_prefab_assets_unit_<hash>.bundle" -> "remote_prefab_assets_unit"
        static string BundleName(string internalId)
        {
            string file = internalId.Substring(internalId.LastIndexOfAny(new[] { '/', '\\' }) + 1);
            int cut = file.LastIndexOf('_');
            return cut > 0 ? file.Substring(0, cut) : file;
        }

        static string Q(string s) => s == null ? "null" : "\"" + s.Replace("\\", "\\\\").Replace("\"", "\\\"") + "\"";

        static string Arg(string name, string fallback)
        {
            var args = Environment.GetCommandLineArgs();
            int i = Array.IndexOf(args, name);
            return i >= 0 && i + 1 < args.Length ? args[i + 1] : fallback;
        }
    }
}
