using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEngine;

namespace VD.EditorTools
{
    // Batchmode: -executeMethod VD.EditorTools.AddressablesSetup.Run [-vdAddressMap tools/rip/address_map.json]
    // Recreates the shipped game's Addressables content from tools/rip/address_map.json:
    //   {"entries": {"<address>": {"path": "Assets/...", "type": "...", "labels": ["..."], "bundle": "<group>"}}, ...}
    // One group per original bundle, the original address and labels on every entry, and play mode set to
    // "Use Asset Database" so the game's own Addressables.LoadAssetAsync(address) calls resolve in the editor.
    public static class AddressablesSetup
    {
        public static void Run()
        {
            string mapPath = Arg("-vdAddressMap", "tools/rip/address_map.json");
            var root = JObject.Parse(File.ReadAllText(mapPath));
            var entries = (JObject)root["entries"];

            var settings = AddressableAssetSettingsDefaultObject.GetSettings(true);
            int added = 0, missing = 0;
            var groups = new Dictionary<string, AddressableAssetGroup>();
            var labels = new HashSet<string>(settings.GetLabels());

            foreach (var kv in entries)
            {
                string address = kv.Key;
                var e = (JObject)kv.Value;
                string path = (string)e["path"];
                string guid = AssetDatabase.AssetPathToGUID(path);
                if (string.IsNullOrEmpty(guid))
                {
                    missing++;
                    continue;
                }
                string groupName = (string)e["bundle"] ?? "vd_default";
                if (!groups.TryGetValue(groupName, out var group))
                {
                    group = settings.FindGroup(groupName) ?? settings.CreateGroup(groupName, false, false, false, null,
                        typeof(BundledAssetGroupSchema), typeof(ContentUpdateGroupSchema));
                    groups[groupName] = group;
                }
                var entry = settings.FindAssetEntry(guid) ?? settings.CreateOrMoveEntry(guid, group, false, false);
                // An asset reachable under several addresses keeps the first; the rest are listed as aliases.
                if (string.IsNullOrEmpty(entry.address) || entry.address == path)
                    entry.SetAddress(address, false);
                foreach (var label in e["labels"]?.Values<string>() ?? Enumerable.Empty<string>())
                {
                    if (labels.Add(label)) settings.AddLabel(label, false);
                    entry.SetLabel(label, true, false, false);
                }
                added++;
            }

            int fast = settings.DataBuilders.FindIndex(b => b != null && b.GetType().Name == "BuildScriptFastMode");
            if (fast >= 0) settings.ActivePlayModeDataBuilderIndex = fast;
            settings.SetDirty(AddressableAssetSettings.ModificationEvent.BatchModification, null, true, true);
            AssetDatabase.SaveAssets();
            Debug.Log($"VD_ADDRESSABLES entries={added} missingPaths={missing} groups={groups.Count} labels={labels.Count} playMode={(fast >= 0 ? "AssetDatabase" : "unchanged")}");
        }

        static string Arg(string name, string fallback)
        {
            var args = System.Environment.GetCommandLineArgs();
            int i = System.Array.IndexOf(args, name);
            return i >= 0 && i + 1 < args.Length ? args[i + 1] : fallback;
        }
    }
}
