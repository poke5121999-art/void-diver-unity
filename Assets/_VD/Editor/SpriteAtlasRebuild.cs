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
                // Packing as shipped: every sprite of the game's atlases carries settingsRaw 67 in its m_RD (packed,
                // Rectangle, no rotation). Unity's default for a new atlas is tight packing with rotation, and a UI Image
                // draws a sprite's whole rect from the atlas (it ignores the tight outline and the rotation), so a
                // tight-packed or rotated sprite shows a neighbour's pixels: the minimap's player marker (img_lighthead,
                // triangle_16 in Ui_Common) drew as a big grey disc.
                var (tight, rotate, raws) = ShippedPacking(a["sprites"]);
                if (AssetImporter.GetAtPath(tmp) is SpriteAtlasImporter imp)
                {
                    var ps = imp.packingSettings;
                    ps.enableTightPacking = tight;
                    ps.enableRotation = rotate;
                    imp.packingSettings = ps;
                    imp.includeInBuild = true;
                    imp.SaveAndReimport();
                }
                Debug.Log($"VD_SPRITEATLAS {name} packing tight={tight} rotation={rotate} settingsRaw={raws}");
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

        // The shipped packing of an atlas, read from its sprites' m_RD.settingsRaw (bit 0 packed, bit 1 packing mode
        // 1 = Rectangle 0 = Tight, bits 2-5 rotation). A sprite imported from a .png has no shipped render data and
        // does not vote.
        static (bool tight, bool rotate, string raws) ShippedPacking(JToken sprites)
        {
            bool tight = false, rotate = false;
            var seen = new SortedSet<int>();
            foreach (JObject s in sprites)
            {
                string p = (string)s["project"];
                if (string.IsNullOrEmpty(p) || !p.EndsWith(".asset") || !File.Exists(p)) continue;
                var m = Regex.Match(File.ReadAllText(p), @"m_RD:.*?settingsRaw: (\d+)", RegexOptions.Singleline);
                if (!m.Success) continue;
                int raw = int.Parse(m.Groups[1].Value);
                seen.Add(raw);
                if ((raw & 1) == 0) continue;
                if ((raw >> 1 & 1) == 0) tight = true;
                if ((raw >> 2 & 15) != 0) rotate = true;
            }
            return (tight, rotate, string.Join(",", seen));
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
