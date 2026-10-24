using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;

namespace VD.EditorTools
{
    // Batchmode: -executeMethod VD.EditorTools.Batch.Run -vdSteps spriteatlas [-vdSpriteAtlases tools/rip/sprite_atlases.json]
    // AssetRipper does not export SpriteAtlas objects. tools/rip/sprite_atlases.json (sprite_atlases_extract.py) lists the 29 shipped
    // atlases with their original GUID and packed sprites (resolved to the exported Sprite assets). This rebuilds each as
    // Assets/_VD/SpriteAtlas/<m_Name>.spriteatlasv2, include-in-build,
    // with a .meta carrying the ORIGINAL GUID so Addressables keys and AssetReferences resolve.
    // The atlases only work packed: with Sprite Packer mode Disabled (the default of a project without the game's
    // EditorSettings) SpriteAtlas.GetSprite returns null in Play mode, AssetManager.GetSprite logs "Resource load failed"
    // and every icon Image draws as a white square. Mode SpriteAtlasV2 packs both the legacy and the v2 files (6000.3,
    // measured), so this step sets it and the check packs every atlas and looks a sprite up by name.
    public static class SpriteAtlasRebuild
    {
        const string OutDir = "Assets/_VD/SpriteAtlas";
        const string TmpDir = OutDir + "/_tmp";

        public static void Run()
        {
            if (EditorSettings.spritePackerMode != SpritePackerMode.SpriteAtlasV2)
            {
                Debug.Log($"VD_SPRITEATLAS sprite packer mode {EditorSettings.spritePackerMode} -> {SpritePackerMode.SpriteAtlasV2}");
                EditorSettings.spritePackerMode = SpritePackerMode.SpriteAtlasV2;
            }
            string json = Batch_Arg("-vdSpriteAtlases", "tools/rip/sprite_atlases.json");
            var atlases = JArray.Parse(File.ReadAllText(json));
            Directory.CreateDirectory(OutDir);
            Directory.CreateDirectory(TmpDir);
            AssetDatabase.Refresh();

            int built = 0, spritesTotal = 0, missing = 0;
            var wanted = new List<(string path, string name, string guid, int expected)>();
            foreach (JObject a in atlases)
            {
                string name = (string)a["name"], guid = (string)a["guid"];
                var sprites = new List<Object>();
                foreach (JObject s in a["sprites"])
                {
                    string p = (string)s["project"];
                    var sp = string.IsNullOrEmpty(p) ? null : LoadSprite(p);
                    if (sp == null) { missing++; Debug.LogWarning($"VD_SPRITEATLAS missing sprite {name}/{(string)s["name"]} path={p}"); continue; }
                    sprites.Add(sp);
                }
                // Every atlas is written as v2. The 6 Ui_* catalog addresses show the original project's .spriteatlasv2 paths
                // and the Icon_* addresses are bare names; a project in v2 mode keeps no legacy atlas, and a legacy atlas
                // writes its packed sprites into the tracked file at every pack, a v2 atlas into Library.
                string legacy = $"{OutDir}/{name}.spriteatlas";
                if (File.Exists(legacy)) { AssetDatabase.DeleteAsset(legacy); Debug.Log($"VD_SPRITEATLAS legacy atlas replaced by v2: {legacy}"); }
                string tmp = $"{TmpDir}/{name}.spriteatlasv2", fin = $"{OutDir}/{name}.spriteatlasv2";
                var asset = new SpriteAtlasAsset();
                asset.Add(sprites.ToArray());
                asset.SetIncludeInBuild(true);
                SpriteAtlasAsset.Save(asset, tmp);
                // Save writes the file only; the .meta (and include-in-build, which v2 keeps in its importer) comes from
                // importing it.
                AssetDatabase.ImportAsset(tmp, ImportAssetOptions.ForceSynchronousImport);
                if (AssetImporter.GetAtPath(tmp) is SpriteAtlasImporter imp && !imp.includeInBuild)
                {
                    imp.includeInBuild = true;
                    imp.SaveAndReimport();
                }
                AssetDatabase.SaveAssets();
                string body = File.ReadAllText(tmp), meta = File.ReadAllText(tmp + ".meta");
                meta = Regex.Replace(meta, "^guid: [0-9a-f]{32}", "guid: " + guid, RegexOptions.Multiline);
                File.WriteAllText(fin, body);
                File.WriteAllText(fin + ".meta", meta);
                AssetDatabase.DeleteAsset(tmp);
                wanted.Add((fin, name, guid, sprites.Count));
                built++; spritesTotal += sprites.Count;
            }
            AssetDatabase.DeleteAsset(TmpDir);
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);

            SpriteAtlasUtility.PackAllAtlases(EditorUserBuildSettings.activeBuildTarget, false);
            int ok = 0;
            foreach (var w in wanted)
            {
                string g = AssetDatabase.AssetPathToGUID(w.path);
                // Both formats import as a SpriteAtlas; SpriteAtlasAsset has no GetPackables in 6000.3, so the sprites are
                // counted from the "packables:" list of the saved YAML (same key in v1 and v2), and the pack is checked
                // the way the game reads it: spriteCount and GetSprite by name.
                var atlas = AssetDatabase.LoadMainAssetAtPath(w.path) as SpriteAtlas;
                int count = CountPackables(w.path), packed = atlas != null ? atlas.spriteCount : -1;
                string probe = null;
                if (packed > 0)
                {
                    var arr = new Sprite[packed];
                    atlas.GetSprites(arr);
                    probe = arr[0] != null ? arr[0].name.Replace("(Clone)", "") : null;
                }
                bool found = probe != null && atlas.GetSprite(probe) != null;
                bool good = g == w.guid && atlas != null && count == w.expected && packed == w.expected && found;
                if (good) ok++;
                Debug.Log($"VD_SPRITEATLAS {w.name} guid={(g == w.guid ? "original" : "MISMATCH " + g)} imported={atlas != null} sprites={count}/{w.expected} packed={packed} getSprite({probe})={found} {(good ? "ok" : "FAIL")}");
            }
            Debug.Log($"VD_SPRITEATLAS_DONE atlases={built}/{atlases.Count} verified={ok} sprites={spritesTotal} missingSprites={missing}");
        }

        // 13 shipped sprites were sub-assets of their texture (one GUID for both); the export keeps that GUID on the .png but
        // imports it as a Default texture, so it has no Sprite. Import it as a single Sprite, as the original project did:
        // the Sprite then has the original fileID 21300000 and both the atlas and anything else referencing it resolve.
        static Sprite LoadSprite(string path)
        {
            var sp = AssetDatabase.LoadAssetAtPath<Sprite>(path);
            if (sp != null) return sp;
            if (AssetImporter.GetAtPath(path) is TextureImporter ti && ti.textureType != TextureImporterType.Sprite)
            {
                ti.textureType = TextureImporterType.Sprite;
                ti.spriteImportMode = SpriteImportMode.Single;
                ti.SaveAndReimport();
                sp = AssetDatabase.LoadAssetAtPath<Sprite>(path);
                Debug.Log($"VD_SPRITEATLAS texture imported as Sprite: {path} ({(sp != null ? "ok" : "still no Sprite")})");
            }
            return sp;
        }

        static int CountPackables(string path)
        {
            var lines = File.ReadAllLines(path);
            int i = System.Array.FindIndex(lines, l => l.TrimStart().StartsWith("packables:"));
            if (i < 0) return -1;
            int indent = lines[i].Length - lines[i].TrimStart().Length, n = 0;
            for (int j = i + 1; j < lines.Length; j++)
            {
                string t = lines[j].TrimStart();
                int ind = lines[j].Length - t.Length;
                if (t.StartsWith("- ") && ind >= indent) n++;
                else break;
            }
            return n;
        }

        static string Batch_Arg(string name, string fallback)
        {
            var args = System.Environment.GetCommandLineArgs();
            int i = System.Array.IndexOf(args, name);
            return i >= 0 && i + 1 < args.Length ? args[i + 1] : fallback;
        }
    }
}
