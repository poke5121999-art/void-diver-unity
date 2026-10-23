using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;

namespace VD.EditorTools
{
    // Still captures for checking the rebuilt shaders (tools/shaders) on a sector, VFX and characters. Run it through
    // tools/shaders/shot.sh, which restores ProjectSettings afterwards (see -vdPipeline/-vdLinear):
    //   -executeMethod VD.EditorTools.ShaderShots.Run -vdPrefab <path[@x,y,z]>[;...] -vdOut captures/x.png
    //     [-vdScene <scene, default LoungeScene for its lights>] [-vdTarget x,y,z (default: first prefab position)]
    //     [-vdOffset 25,21,-25] [-vdFov 10] [-vdTimes 0.3,0.8] [-vdBg r,g,b] [-vdAnim <spine animation name>]
    //     [-vdPipeline <URP asset path> | none] [-vdRenderer <renderer index>] [-vdLinear 1]
    //     [-vdFeaturesOff all|<name part>,...] [-vdControl 1 [-vdControlShader <shader name>]]
    //     [-vdHideShader <shader name part>] (disables renderers using it)
    // -vdFeaturesOff turns renderer features (game code in LCArt.URP) off for that shot only, to tell a broken feature
    // from a broken shader; -vdControl 1 adds a cube with the package's URP/Lit shader at the target as a reference.
    // The committed GraphicsSettings point at a URP asset GUID that no longer exists (pre-deterministic-GUID export), so
    // the editor would render with the built-in pipeline, where every URP shader (all of the game's) draws magenta.
    // -vdPipeline (default Assets/MonoBehaviour/UniveralRP_VD.asset, the game's own) assigns the pipeline in memory for
    // the capture; -vdLinear 1 switches to the game's linear color space if the project is not linear already.
    // Particle systems are simulated to each time in -vdTimes (one image per time, suffix _t<ms>); Spine skeletons
    // (found by reflection, no assembly reference) are initialised and posed so their mesh exists in edit mode.
    // Prefabs are placed in memory only; nothing is saved.
    // -vdShotList <file>: several shots in one editor start, one per line, each line "-vdName value ..." arguments
    // (values without spaces) that override the command line for that shot; # starts a comment line.
    public static class ShaderShots
    {
        static Dictionary<string, string> overrides = new Dictionary<string, string>();

        public static void Run()
        {
            string list = Arg("-vdShotList", "");
            if (list == "") { Shot(); return; }
            foreach (string line in File.ReadAllLines(list))
            {
                if (line.Trim() == "" || line.TrimStart().StartsWith("#")) continue;
                var tok = line.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
                overrides = new Dictionary<string, string>();
                for (int i = 0; i + 1 < tok.Length; i += 2) overrides[tok[i]] = tok[i + 1];
                try { Shot(); }
                catch (Exception e) { Debug.LogError($"VD_SHOT_FAILED {line}: {e}"); }
            }
        }

        static void Shot()
        {
            string scene = Arg("-vdScene", "Assets/Scenes/InGame/LoungeScene.unity");
            string prefabSpecs = Arg("-vdPrefab", "");
            string outPath = Arg("-vdOut", "captures/shadershot.png");
            int w = int.Parse(Arg("-vdWidth", "1280")), h = int.Parse(Arg("-vdHeight", "720"));
            float[] times = Arg("-vdTimes", "0.5").Split(',').Select(F).ToArray();

            UsePipeline(Arg("-vdPipeline", "Assets/MonoBehaviour/UniveralRP_VD.asset"));
            if (Arg("-vdLinear", "0") == "1" && PlayerSettings.colorSpace != ColorSpace.Linear)
            {
                PlayerSettings.colorSpace = ColorSpace.Linear;
                Debug.Log("VD_SHOT color space set to Linear (in memory)");
            }

            EditorSceneManager.OpenScene(scene, OpenSceneMode.Single);
            var roots = new List<GameObject>();
            foreach (string spec in prefabSpecs.Split(';', StringSplitOptions.RemoveEmptyEntries))
            {
                var parts = spec.Split('@');
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(parts[0]);
                if (prefab == null) throw new FileNotFoundException("prefab not found", parts[0]);
                var inst = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                inst.transform.position = parts.Length > 1 ? ParseVec(parts[1]) : Vector3.zero;
                inst.SetActive(true);
                PoseSpine(inst, Arg("-vdAnim", ""));
                roots.Add(inst);
            }
            Vector3 at = ParseVec(Arg("-vdTarget", roots.Count > 0 ? Fmt(roots[0].transform.position) : "0,0,0"));

            var shaderUse = new SortedDictionary<string, int>();
            foreach (var inst in roots)
                foreach (var r in inst.GetComponentsInChildren<Renderer>(true))
                    foreach (var m in r.sharedMaterials)
                        if (m != null) { shaderUse.TryGetValue(m.shader.name, out int c); shaderUse[m.shader.name] = c + 1; }
            foreach (var kv in shaderUse) Debug.Log($"VD_SHOT shader {kv.Key} x{kv.Value}{(kv.Key.StartsWith("Hidden/InternalErrorShader") ? " (error)" : "")}");

            var go = new GameObject("VD_ShotCamera");
            var cam = go.AddComponent<Camera>();
            cam.fieldOfView = F(Arg("-vdFov", "10"));
            cam.nearClipPlane = 0.1f;
            cam.farClipPlane = 500f;
            cam.transform.position = at + ParseVec(Arg("-vdOffset", "25,21,-25"));
            cam.transform.LookAt(at);
            int rendererIndex = int.Parse(Arg("-vdRenderer", "-1"));
            if (rendererIndex >= 0)
            {
                var t = Type.GetType("UnityEngine.Rendering.Universal.UniversalAdditionalCameraData, Unity.RenderPipelines.Universal.Runtime");
                var data = t != null ? go.AddComponent(t) : null;
                t?.GetMethod("SetRenderer")?.Invoke(data, new object[] { rendererIndex });
                Debug.Log($"VD_SHOT camera renderer index {rendererIndex} ({(data != null ? "set" : "URP camera data type not found")})");
            }
            string bg = Arg("-vdBg", "");
            if (bg != "")
            {
                var c = ParseVec(bg);
                cam.clearFlags = CameraClearFlags.SolidColor;
                cam.backgroundColor = new Color(c.x, c.y, c.z, 1);
            }

            string hide = Arg("-vdHideShader", "");
            if (hide != "")
                foreach (var inst in roots)
                    foreach (var r in inst.GetComponentsInChildren<Renderer>(true))
                        if (r.sharedMaterials.Any(m => m != null && m.shader.name.Contains(hide)))
                        {
                            r.enabled = false;
                            Debug.Log($"VD_SHOT hid renderer {r.name} ({hide})");
                        }
            if (Arg("-vdControl", "0") == "1")
            {
                var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.position = at + Vector3.up * 0.5f;
                cube.GetComponent<Renderer>().sharedMaterial = new Material(Shader.Find(Arg("-vdControlShader", "Universal Render Pipeline/Lit"))) { color = new Color(0.8f, 0.2f, 0.2f) };
            }
            var restore = SetFeatures(Arg("-vdFeaturesOff", ""));

            var systems = roots.SelectMany(r => r.GetComponentsInChildren<ParticleSystem>(true)).ToArray();
            foreach (float t in times)
            {
                foreach (var ps in systems)
                    if (ps.transform.parent == null || ps.transform.parent.GetComponentInParent<ParticleSystem>() == null)
                        ps.Simulate(t, true, true, false);
                string path = times.Length > 1 ? Path.ChangeExtension(outPath, null) + $"_t{(int)(t * 1000)}.png" : outPath;
                Render(cam, w, h, path);
                Debug.Log($"VD_SHOT global keywords after render: {string.Join(" ", Shader.enabledGlobalKeywords.Select(k => k.name))}");
                Debug.Log($"VD_SHOT wrote {path} featuresOff={Arg("-vdFeaturesOff", "")} prefab={prefabSpecs} t={t} particles={systems.Sum(p => p.particleCount)} pipeline={(GraphicsSettings.currentRenderPipeline != null ? GraphicsSettings.currentRenderPipeline.name : "built-in")} colorSpace={PlayerSettings.colorSpace}");
            }
            restore();
        }

        // Sets isActive=false on every renderer feature (all loaded ScriptableRendererData) whose asset name contains one
        // of the comma-separated parts ("all" for every feature); returns an action that restores the previous states.
        static Action SetFeatures(string spec)
        {
            if (spec == "") return () => { };
            var parts = spec.Split(',', StringSplitOptions.RemoveEmptyEntries);
            var dataType = Type.GetType("UnityEngine.Rendering.Universal.ScriptableRendererData, Unity.RenderPipelines.Universal.Runtime");
            var featType = Type.GetType("UnityEngine.Rendering.Universal.ScriptableRendererFeature, Unity.RenderPipelines.Universal.Runtime");
            var changed = new List<(UnityEngine.Object, bool)>();
            foreach (var data in Resources.FindObjectsOfTypeAll(dataType))
            {
                var list = (System.Collections.IList)dataType.GetProperty("rendererFeatures").GetValue(data);
                foreach (UnityEngine.Object f in list)
                {
                    if (f == null) continue;
                    if (!parts.Any(p => p == "all" || f.name.Contains(p))) continue;
                    bool was = (bool)featType.GetProperty("isActive").GetValue(f);
                    featType.GetMethod("SetActive").Invoke(f, new object[] { false });
                    changed.Add((f, was));
                    Debug.Log($"VD_SHOT feature off {data.name}/{f.name} (was {(was ? "on" : "off")})");
                }
            }
            return () => { foreach (var (f, was) in changed) featType.GetMethod("SetActive").Invoke(f, new object[] { was }); };
        }

        internal static void UsePipeline(string path)
        {
            if (path == "none") return;
            var asset = AssetDatabase.LoadAssetAtPath<RenderPipelineAsset>(path);
            if (asset == null) throw new FileNotFoundException("render pipeline asset not found", path);
            GraphicsSettings.defaultRenderPipeline = asset;
            QualitySettings.renderPipeline = asset;
            Debug.Log($"VD_SHOT render pipeline {path} (in memory)");
        }

        static string Fmt(Vector3 v) => string.Join(",", new[] { v.x, v.y, v.z }.Select(f => f.ToString(System.Globalization.CultureInfo.InvariantCulture)));

        static void PoseSpine(GameObject root, string anim)
        {
            foreach (var mb in root.GetComponentsInChildren<MonoBehaviour>(true))
            {
                if (mb == null) continue;
                var type = mb.GetType();
                if (type.Namespace != "Spine.Unity") continue;
                try
                {
                    var init = type.GetMethods(BindingFlags.Public | BindingFlags.Instance).FirstOrDefault(m => m.Name == "Initialize" && m.GetParameters().Length >= 1 && m.GetParameters()[0].ParameterType == typeof(bool));
                    if (init == null) continue;
                    var ps = init.GetParameters();
                    var args = new object[ps.Length];
                    args[0] = true;
                    for (int i = 1; i < ps.Length; i++) args[i] = ps[i].HasDefaultValue ? ps[i].DefaultValue : null;
                    init.Invoke(mb, args);
                    if (anim != "")
                    {
                        var nameField = type.GetProperty("AnimationName");
                        nameField?.SetValue(mb, anim);
                    }
                    type.GetMethod("Update", BindingFlags.Public | BindingFlags.Instance, null, new[] { typeof(float) }, null)?.Invoke(mb, new object[] { 0f });
                    type.GetMethod("LateUpdate", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, Type.EmptyTypes, null)?.Invoke(mb, null);
                    Debug.Log($"VD_SHOT spine {type.Name} on {mb.name} posed");
                }
                catch (Exception e)
                {
                    Debug.LogWarning($"VD_SHOT spine {type.Name} on {mb.name} failed: {e.InnerException?.Message ?? e.Message}");
                }
            }
        }

        static void Render(Camera cam, int w, int h, string outPath)
        {
            var rt = new RenderTexture(w, h, 24, RenderTextureFormat.ARGB32);
            cam.targetTexture = rt;
            cam.Render();
            RenderTexture.active = rt;
            var tex = new Texture2D(w, h, TextureFormat.RGB24, false);
            tex.ReadPixels(new Rect(0, 0, w, h), 0, 0);
            tex.Apply();
            Directory.CreateDirectory(Path.GetDirectoryName(outPath));
            File.WriteAllBytes(outPath, tex.EncodeToPNG());
            RenderTexture.active = null;
            cam.targetTexture = null;
            UnityEngine.Object.DestroyImmediate(rt);
        }

        internal static string Arg(string name, string fallback)
        {
            if (overrides.TryGetValue(name, out var v)) return v;
            var args = Environment.GetCommandLineArgs();
            int i = Array.IndexOf(args, name);
            return i >= 0 && i + 1 < args.Length ? args[i + 1] : fallback;
        }

        // Invariant culture: this host runs a vi_VN locale, where float.Parse("0.15") is 15 (comma decimal separator).
        static float F(string s) => float.Parse(s, System.Globalization.CultureInfo.InvariantCulture);

        static Vector3 ParseVec(string s)
        {
            var p = s.Split(',');
            return new Vector3(F(p[0]), F(p[1]), F(p[2]));
        }
    }
}
