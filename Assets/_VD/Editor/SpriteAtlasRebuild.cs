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
    // Assets/_VD/SpriteAtlas/<m_Name>.spriteatlas (legacy) or .spriteatlasv2 (the 6 that shipped as v2), include-in-build,
    // with a .meta carrying the ORIGINAL GUID so Addressables keys and AssetReferences resolve.
    public static class SpriteAtlasRebuild
    {
        const string OutDir = "Assets/_VD/SpriteAtlas";
        const string TmpDir = OutDir + "/_tmp";

        public static void Run()
        {
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
                bool v2 = ((string)a["catalogAddress"] ?? "").EndsWith(".spriteatlasv2");
                var sprites = new List<Object>();
                foreach (JObject s in a["sprites"])
                {
                    string p = (string)s["project"];
                    var sp = string.IsNullOrEmpty(p) ? null : LoadSprite(p);
                    if (sp == null) { missing++; Debug.LogWarning($"VD_SPRITEATLAS missing sprite {name}/{(string)s["name"]} path={p}"); continue; }
                    sprites.Add(sp);
                }
                string ext = v2 ? ".spriteatlasv2" : ".spriteatlas";
                string tmp = $"{TmpDir}/{name}{ext}", fin = $"{OutDir}/{name}{ext}";
                if (v2)
                {
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
                }
                else
                {
                    var atlas = new SpriteAtlas();
                    SpriteAtlasExtensions.Add(atlas, sprites.ToArray());
                    SpriteAtlasExtensions.SetIncludeInBuild(atlas, true);
                    AssetDatabase.CreateAsset(atlas, tmp);
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

            int ok = 0;
            foreach (var w in wanted)
            {
                string g = AssetDatabase.AssetPathToGUID(w.path);
                // Both formats import as a SpriteAtlas; spriteCount needs a pack, and SpriteAtlasAsset has no GetPackables in
                // 6000.3, so the sprites are counted from the "packables:" list of the saved YAML (same key in v1 and v2).
                bool imported = AssetDatabase.LoadMainAssetAtPath(w.path) is SpriteAtlas;
                int count = CountPackables(w.path);
                bool good = g == w.guid && imported && count == w.expected;
                if (good) ok++;
                Debug.Log($"VD_SPRITEATLAS {w.name} guid={(g == w.guid ? "original" : "MISMATCH " + g)} imported={imported} sprites={count}/{w.expected} {(good ? "ok" : "FAIL")}");
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
