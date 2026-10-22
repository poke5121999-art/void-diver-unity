using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace VD.EditorTools
{
    // Batchmode: -executeMethod VD.EditorTools.Census.Run
    // Writes captures/census.json and one "VD_CENSUS ..." summary line to the log.
    public static class Census
    {
        public static void Run()
        {
            var report = new StringBuilder();
            report.Append("{\n");

            var prefabMissing = new List<string>();
            int prefabs = 0, missingInPrefabs = 0;
            foreach (string guid in AssetDatabase.FindAssets("t:Prefab"))
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                var go = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (go == null) continue;
                prefabs++;
                int n = CountMissing(go);
                if (n > 0)
                {
                    missingInPrefabs += n;
                    prefabMissing.Add(path);
                }
            }

            var sceneLines = new List<string>();
            int missingInScenes = 0;
            foreach (var scene in EditorBuildSettings.scenes)
            {
                var s = EditorSceneManager.OpenScene(scene.path, OpenSceneMode.Single);
                int n = s.GetRootGameObjects().Sum(CountMissing);
                missingInScenes += n;
                sceneLines.Add($"    \"{scene.path}\": {n}");
            }

            var byShader = new Dictionary<string, int>();
            int materials = 0, stubShaded = 0, noShader = 0;
            foreach (string guid in AssetDatabase.FindAssets("t:Material"))
            {
                var mat = AssetDatabase.LoadAssetAtPath<Material>(AssetDatabase.GUIDToAssetPath(guid));
                if (mat == null) continue;
                materials++;
                string key;
                if (mat.shader == null || mat.shader.name == "Hidden/InternalErrorShader")
                {
                    noShader++;
                    key = "<none>";
                }
                else
                {
                    bool stub = IsDummy(mat.shader);
                    if (stub) stubShaded++;
                    key = (stub ? "STUB " : "") + mat.shader.name;
                }
                byShader.TryGetValue(key, out int c);
                byShader[key] = c + 1;
            }

            report.Append($"  \"prefabs\": {prefabs},\n  \"missingScriptsInPrefabs\": {missingInPrefabs},\n");
            report.Append($"  \"prefabsWithMissing\": [{string.Join(",", prefabMissing.Take(200).Select(p => $"\n    \"{p}\""))}],\n");
            report.Append($"  \"scenes\": {{\n{string.Join(",\n", sceneLines)}\n  }},\n");
            report.Append($"  \"materials\": {materials},\n  \"materialsOnStubShader\": {stubShaded},\n  \"materialsWithoutShader\": {noShader},\n");
            report.Append($"  \"materialsByShader\": {{{string.Join(",", byShader.OrderByDescending(kv => kv.Value).Select(kv => $"\n    \"{kv.Key}\": {kv.Value}"))}\n  }}\n}}\n");

            Directory.CreateDirectory("captures");
            File.WriteAllText("captures/census.json", report.ToString());
            Debug.Log($"VD_CENSUS prefabs={prefabs} prefabsWithMissing={prefabMissing.Count} missingInPrefabs={missingInPrefabs} " +
                      $"missingInScenes={missingInScenes} materials={materials} stubShader={stubShaded} noShader={noShader}");
        }

        static readonly Dictionary<Shader, bool> dummy = new Dictionary<Shader, bool>();

        // AssetRipper Free writes placeholder shader code marked with this comment.
        static bool IsDummy(Shader shader)
        {
            if (!dummy.TryGetValue(shader, out bool d))
            {
                string path = AssetDatabase.GetAssetPath(shader);
                d = path.EndsWith(".shader") && File.ReadAllText(path).Contains("DummyShaderTextExporter");
                dummy[shader] = d;
            }
            return d;
        }

        static int CountMissing(GameObject root)
        {
            int n = 0;
            foreach (var t in root.GetComponentsInChildren<Transform>(true))
                n += GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(t.gameObject);
            return n;
        }
    }
}
