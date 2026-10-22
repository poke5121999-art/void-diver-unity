using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace VD.EditorTools
{
    // Gate for the rebuilt shaders in Assets/_VD/Shaders (tools/shaders), usable before materials are remapped:
    //   -executeMethod VD.EditorTools.ShaderGate.Run -vdScene <scene> -vdPrefab <path@x,y,z;...> -vdTarget x,y,z -vdOut captures/x.png
    // 1. Reports import-time compile messages of every shader under Assets/_VD/Shaders (VD_SHADER_ERROR lines).
    // 2. Opens the scene, instantiates the prefabs, and swaps every material whose shader is an AssetRipper dummy to the
    //    rebuilt shader of the same name (in memory only; nothing is saved).
    // 3. Renders a capture with the same rig as Capture (FOV 10, offset (25, 21, -25)); variant compile errors show up
    //    in the log as "Shader error in".
    public static class ShaderGate
    {
        public static void Run()
        {
            string shaderDir = Arg("-vdShaderDir", "Assets/_VD/Shaders");
            var rebuilt = new Dictionary<string, Shader>();
            int errors = 0, warnings = 0;
            foreach (string path in Directory.GetFiles(shaderDir, "*.shader", SearchOption.TopDirectoryOnly).OrderBy(p => p))
            {
                string p = path.Replace('\\', '/');
                var shader = AssetDatabase.LoadAssetAtPath<Shader>(p);
                if (shader == null) { Debug.LogError($"VD_SHADER_ERROR {p}: did not import as a Shader"); errors++; continue; }
                rebuilt[shader.name] = shader;
                foreach (var m in ShaderUtil.GetShaderMessages(shader))
                {
                    if (m.severity == UnityEditor.Rendering.ShaderCompilerMessageSeverity.Error)
                    {
                        errors++;
                        Debug.LogError($"VD_SHADER_ERROR {shader.name} [{m.platform}] {m.file}:{m.line}: {m.message}");
                    }
                    else
                    {
                        warnings++;
                        Debug.Log($"VD_SHADER_WARNING {shader.name} [{m.platform}] {m.file}:{m.line}: {m.message}");
                    }
                }
                if (ShaderUtil.ShaderHasError(shader) && errors == 0)
                {
                    errors++;
                    Debug.LogError($"VD_SHADER_ERROR {shader.name}: ShaderHasError without messages");
                }
            }
            Debug.Log($"VD_SHADERGATE import shaders={rebuilt.Count} errors={errors} warnings={warnings}");

            string scene = Arg("-vdScene", "Assets/Scenes/InGame/LoungeScene.unity");
            string outPath = Arg("-vdOut", "captures/shadergate.png");
            Vector3 target = ParseVec(Arg("-vdTarget", "0,0,0"));
            int w = int.Parse(Arg("-vdWidth", "1920")), h = int.Parse(Arg("-vdHeight", "1080"));

            EditorSceneManager.OpenScene(scene, OpenSceneMode.Single);
            foreach (string spec in Arg("-vdPrefab", "").Split(';', StringSplitOptions.RemoveEmptyEntries))
            {
                var parts = spec.Split('@');
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(parts[0]);
                if (prefab == null) throw new FileNotFoundException("prefab not found", parts[0]);
                var inst = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                if (parts.Length > 1) inst.transform.position = ParseVec(parts[1]);
            }

            // swap dummies -> rebuilt, in memory
            int swapped = 0, unswapped = 0;
            var swappedNames = new SortedDictionary<string, int>();
            var missing = new SortedDictionary<string, int>();
            var seen = new HashSet<Material>();
            var mats = new List<Material>();
            foreach (var r in UnityEngine.Object.FindObjectsByType<Renderer>(FindObjectsInactive.Include, FindObjectsSortMode.None))
                mats.AddRange(r.sharedMaterials);
            if (RenderSettings.skybox != null) mats.Add(RenderSettings.skybox);
            foreach (var mat in mats)
            {
                if (mat == null || mat.shader == null || !seen.Add(mat)) continue;
                if (!IsDummy(mat.shader)) continue;
                if (rebuilt.TryGetValue(mat.shader.name, out var mine))
                {
                    swappedNames.TryGetValue(mine.name, out int c); swappedNames[mine.name] = c + 1;
                    mat.shader = mine;
                    swapped++;
                }
                else
                {
                    missing.TryGetValue(mat.shader.name, out int c); missing[mat.shader.name] = c + 1;
                    unswapped++;
                }
            }
            foreach (var kv in swappedNames) Debug.Log($"VD_SHADERGATE swapped {kv.Value} material(s) -> {kv.Key}");
            foreach (var kv in missing) Debug.Log($"VD_SHADERGATE no rebuilt shader for {kv.Value} material(s) on {kv.Key}");

            var go = new GameObject("VD_CaptureCamera");
            var cam = go.AddComponent<Camera>();
            cam.fieldOfView = float.Parse(Arg("-vdFov", "10"));
            cam.nearClipPlane = 0.1f;
            cam.farClipPlane = 500f;
            cam.transform.position = target + ParseVec(Arg("-vdOffset", "25,21,-25"));
            cam.transform.LookAt(target);
            var rt = new RenderTexture(w, h, 24, RenderTextureFormat.ARGB32);
            cam.targetTexture = rt;
            cam.Render();
            RenderTexture.active = rt;
            var tex = new Texture2D(w, h, TextureFormat.RGB24, false);
            tex.ReadPixels(new Rect(0, 0, w, h), 0, 0);
            tex.Apply();
            Directory.CreateDirectory(Path.GetDirectoryName(outPath));
            File.WriteAllBytes(outPath, tex.EncodeToPNG());
            Debug.Log($"VD_SHADERGATE_RESULT errors={errors} warnings={warnings} swapped={swapped} unswapped={unswapped} wrote {outPath}");
        }

        static readonly Dictionary<Shader, bool> dummy = new Dictionary<Shader, bool>();

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

        static string Arg(string name, string fallback)
        {
            var args = Environment.GetCommandLineArgs();
            int i = Array.IndexOf(args, name);
            return i >= 0 && i + 1 < args.Length ? args[i + 1] : fallback;
        }

        static Vector3 ParseVec(string s)
        {
            var p = s.Split(',');
            return new Vector3(float.Parse(p[0]), float.Parse(p[1]), float.Parse(p[2]));
        }
    }
}
