using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.Rendering;
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
    //
    // CompileAll (Batch step "shadergate"): compiles every shipped variant of every rebuilt shader, not only the ones a
    // capture happens to draw. Variants are read back from the generated #if chains (one per stage, one #include per
    // shipped variant); a variant whose include content repeats within a pass is compiled once.
    //   -vdPlatforms OpenGLCore,D3D   (default; OpenGLCore is what this Linux editor renders with, D3D what the game ran)
    // Log: VD_VARIANT_ERROR <shader> | <pass> | <stage> | <platform> | <keywords> | <first message>, then
    // VD_SHADERGATE_COMPILE shaders=.. variants=.. failed=.. failedShaders=..
    public static class ShaderGate
    {
        static readonly Regex ChainCond = new Regex(@"^\s*#(?:el)?if\s+(.*)$");
        static readonly Regex Defined = new Regex(@"defined\((\w+)\)");
        static readonly Regex Include = new Regex("^\\s*#include\\s+\"([^\"]+)\"");

        public static void CompileAll()
        {
            string shaderDir = Arg("-vdShaderDir", "Assets/_VD/Shaders");
            var platforms = Arg("-vdPlatforms", "OpenGLCore,D3D").Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(p => (ShaderCompilerPlatform)Enum.Parse(typeof(ShaderCompilerPlatform), p)).ToArray();
            int shaders = 0, variants = 0, failed = 0;
            var failedShaders = new SortedSet<string>();
            using var sha = SHA1.Create();
            foreach (string path in Directory.GetFiles(shaderDir, "*.shader", SearchOption.TopDirectoryOnly).OrderBy(p => p))
            {
                string p = path.Replace('\\', '/');
                var shader = AssetDatabase.LoadAssetAtPath<Shader>(p);
                if (shader == null) { Debug.LogError($"VD_VARIANT_ERROR {p}: did not import as a Shader"); failed++; failedShaders.Add(p); continue; }
                shaders++;
                int before = failed, mine = 0;
                var data = ShaderUtil.GetShaderData(shader);
                for (int si = 0; si < data.SubshaderCount; si++)
                {
                    var ss = data.GetSubshader(si);
                    for (int pi = 0; pi < ss.PassCount; pi++)
                    {
                        var pass = ss.GetPass(pi);
                        string src = pass.SourceCode ?? "";
                        var seen = new HashSet<string>();
                        foreach (var (stageTag, type) in new[] { ("progVertex", UnityEditor.Rendering.ShaderType.Vertex), ("progFragment", UnityEditor.Rendering.ShaderType.Fragment) })
                        {
                            if (!pass.HasShaderStage(type)) continue;
                            foreach (var (keywords, include) in Chain(src, stageTag))
                            {
                                string inc = Path.Combine(shaderDir, include);
                                string key = stageTag + ":" + (File.Exists(inc) ? Convert.ToBase64String(sha.ComputeHash(File.ReadAllBytes(inc))) : include);
                                if (!seen.Add(key)) continue;
                                foreach (var platform in platforms)
                                {
                                    variants++; mine++;
                                    if (variants % 200 == 0) Debug.Log($"VD_SHADERGATE_PROGRESS variants={variants} failed={failed} at {shader.name}");
                                    var target = platform == ShaderCompilerPlatform.D3D ? BuildTarget.StandaloneWindows64 : BuildTarget.StandaloneLinux64;
                                    ShaderData.VariantCompileInfo r;
                                    try { r = pass.CompileVariant(type, keywords, platform, target); }
                                    catch (Exception e)
                                    {
                                        failed++; failedShaders.Add(shader.name);
                                        Debug.LogError($"VD_VARIANT_ERROR {shader.name} | {pass.Name} | {type} | {platform} | {string.Join(" ", keywords)} | exception {e.Message}");
                                        continue;
                                    }
                                    if (r.Success) continue;
                                    failed++; failedShaders.Add(shader.name);
                                    var msg = r.Messages?.FirstOrDefault(m => m.severity == ShaderCompilerMessageSeverity.Error);
                                    Debug.LogError($"VD_VARIANT_ERROR {shader.name} | {pass.Name} | {type} | {platform} | {string.Join(" ", keywords)} | {include} | {msg?.message} ({msg?.file}:{msg?.line})");
                                }
                            }
                        }
                    }
                }
                if (string.IsNullOrEmpty(shader.name) || mine == 0)
                {
                    // a ShaderLab parse error imports as a nameless error shader with no passes to compile
                    failed++; failedShaders.Add(p);
                    var msg = ShaderUtil.GetShaderMessages(shader).FirstOrDefault(m => m.severity == ShaderCompilerMessageSeverity.Error);
                    Debug.LogError($"VD_VARIANT_ERROR {p} | import | name='{shader.name}' variants={mine} | {msg.message} (line {msg.line})");
                }
                Debug.Log($"VD_SHADERGATE_SHADER {p} {shader.name} compiled={mine} failed={failed - before}");
            }
            Debug.Log($"VD_SHADERGATE_COMPILE shaders={shaders} variants={variants} failed={failed} failedShaders={failedShaders.Count} [{string.Join(", ", failedShaders)}]");
        }

        // (keywords, include path) per #if/#elif/#else branch of the dispatch chain that follows "// ---- <stage>".
        static IEnumerable<(string[], string)> Chain(string src, string stageTag)
        {
            var lines = src.Split('\n');
            int i = Array.FindIndex(lines, l => l.Trim() == "// ---- " + stageTag);
            if (i < 0) yield break;
            string[] cond = null;
            for (i++; i < lines.Length; i++)
            {
                string l = lines[i].Trim();
                if (l.StartsWith("// ---- ") || l == "#endif") yield break;
                var m = ChainCond.Match(l);
                if (m.Success) { cond = Defined.Matches(m.Groups[1].Value).Select(x => x.Groups[1].Value).ToArray(); continue; }
                if (l == "#else") { cond = null; continue; }
                var inc = Include.Match(l);
                if (inc.Success && cond != null) yield return (cond, inc.Groups[1].Value);
            }
        }

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
            cam.fieldOfView = float.Parse(Arg("-vdFov", "10"), System.Globalization.CultureInfo.InvariantCulture);
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
            return new Vector3(float.Parse(p[0], System.Globalization.CultureInfo.InvariantCulture), float.Parse(p[1], System.Globalization.CultureInfo.InvariantCulture), float.Parse(p[2], System.Globalization.CultureInfo.InvariantCulture));
        }
    }
}
