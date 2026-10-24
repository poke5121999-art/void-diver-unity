using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace VD.PlayTests
{
    // A5b: what draws magenta on the dive map (campaign 1101) next to the boundary fog. Plays A5's lounge part into the
    // stage, freezes it, takes the camera off its rig and looks at the map edges (captures/review/fogprobe-look*.png).
    // Where a look shows magenta it shoots with one renderer feature, then one shader's renderers, off at a time and logs
    // how much magenta each shot keeps (VD_FOGPROBE lines). Run 15 (OpenGL): only DecalRendererFeature off cleared it.
    public class ProbeRunner : MonoBehaviour { }

    public class A5b_FogProbe
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void PrepareSaves()
        {
            if (!Flow.RunFor("A5b_FogProbe")) return;
            Application.logMessageReceivedThreaded += (message, stack, type) =>
            {
                if (type == LogType.Log || type == LogType.Warning) return;
                try { if (!LogAssert.ignoreFailingMessages) LogAssert.ignoreFailingMessages = true; }
                catch (InvalidOperationException) { }
            };
            Flow.Tag = "VD_A5B";
            Flow.SeedSaves("tools/fixtures/saves-post-tutorial");
            Flow.UseLocalPlatform();
        }

        [TearDown]
        public void Restore()
        {
            Flow.RestoreInput();
            Flow.RestorePlatform();
        }

        static IEnumerator A5(string method, params object[] args) =>
            (IEnumerator)typeof(A5_DiveFlow).GetMethod(method, BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, args);

        static void Log(string s) => Debug.LogWarning("VD_FOGPROBE " + s);

        static string Path(Transform t) => t.parent == null ? t.name : Path(t.parent) + "/" + t.name;

        // Magenta pixels (r and b high, g low) in a shot, and their bounding rect in GUI coordinates (y down).
        static int Magenta(string key, out RectInt box)
        {
            var tex = new Texture2D(2, 2);
            tex.LoadImage(File.ReadAllBytes(System.IO.Path.Combine("captures", "review", key + ".png")));
            var px = tex.GetPixels32();
            int n = 0, x0 = int.MaxValue, y0 = int.MaxValue, x1 = -1, y1 = -1;
            for (int i = 0; i < px.Length; i++)
            {
                var c = px[i];
                if (c.r > 150 && c.b > 150 && c.g < c.r * 0.6f)
                {
                    n++;
                    int x = i % tex.width, y = tex.height - 1 - i / tex.width;
                    x0 = Mathf.Min(x0, x); x1 = Mathf.Max(x1, x); y0 = Mathf.Min(y0, y); y1 = Mathf.Max(y1, y);
                }
            }
            box = n > 0 ? new RectInt(x0, y0, x1 - x0 + 1, y1 - y0 + 1) : new RectInt();
            Object.Destroy(tex);
            return n;
        }

        static Rect ScreenRect(Camera cam, Bounds b)
        {
            float xmin = float.MaxValue, ymin = float.MaxValue, xmax = float.MinValue, ymax = float.MinValue;
            for (int i = 0; i < 8; i++)
            {
                var p = b.center + Vector3.Scale(b.extents, new Vector3((i & 1) == 0 ? -1 : 1, (i & 2) == 0 ? -1 : 1, (i & 4) == 0 ? -1 : 1));
                var v = cam.WorldToViewportPoint(p);
                float x = v.x * Review.Width, y = (1f - v.y) * Review.Height;
                xmin = Mathf.Min(xmin, x); xmax = Mathf.Max(xmax, x); ymin = Mathf.Min(ymin, y); ymax = Mathf.Max(ymax, y);
            }
            return Rect.MinMaxRect(xmin, ymin, xmax, ymax);
        }

        [UnityTest, Timeout(1800000)]
        public IEnumerator ProbeDiveMagenta()
        {
            LogAssert.ignoreFailingMessages = true;
            Flow.Tag = "VD_A5B";
            yield return Flow.ContinueToLounge();
            Assert.AreEqual("LoungeScene", Flow.ActiveScene, "Continue did not reach the lounge");
            Flow.RouteInputToGame();
            yield return A5("Talk", 700000, "Elara 80200 start");
            foreach (var (id, who) in new[] { (700013, "Narcis"), (700008, "Felix"), (700001, "Lucas") })
                yield return A5("Talk", id, $"{who} 80200");
            yield return A5("Talk", 700000, "Elara 80200 finish");
            yield return A5("StartCampaign");
            yield return A5("Briefing");
            yield return A5("EnterBooth");
            Assert.AreEqual("StageScene", Flow.ActiveScene, "the dive booth did not lead into the stage");
            yield return new WaitForSecondsRealtime(4f);

            // Watch for magenta while A5's own dive runs (maps are random; it shows near the boundary), then freeze the
            // game and bisect.
            var map = GameObject.Find("Map");
            if (map != null)
                Log($"sectors me={(Flow.MyCharacter() ? Flow.MyCharacter().transform.position.ToString() : "-")} " + string.Join(" ", map.transform.Cast<Transform>().Select(t => $"{t.name}@{t.position.x:F0},{t.position.z:F0}")));
            // Freeze, take the camera off its rig and look at the map edges (inner sectors next to the 1801/1901 boundary
            // sectors), where the A5 shots went magenta.
            yield return new WaitForSecondsRealtime(3f);
            Time.timeScale = 0f;
            Flow.Hold();
            var cam0 = Camera.main;
            var me0 = Flow.MyCharacter() ? Flow.MyCharacter().transform.position : Vector3.zero;
            var offset = cam0.transform.position - me0;
            foreach (var b in cam0.GetComponents<Behaviour>().Concat(cam0.GetComponentsInParent<Behaviour>()))
                if (!(b is Camera) && !(b is UnityEngine.Rendering.Universal.UniversalAdditionalCameraData) && b.enabled) { Log($"camera rig off: {b.GetType().Name} on {b.name}"); b.enabled = false; }
            var decals = Object.FindObjectsByType<UnityEngine.Rendering.Universal.DecalProjector>(FindObjectsSortMode.None);
            Log($"decals={decals.Length} without DecalScreenSpaceProjector pass={decals.Count(d => d.material && d.material.FindPass("DecalScreenSpaceProjector") < 0)} shaders=[{string.Join(",", decals.Where(d => d.material).Select(d => d.material.shader.name).Distinct())}]");
            int baseN = 0;
            RectInt box = default;
            var targets = new[] { new Vector3(107, 0, 10), new Vector3(105, 0, 3), new Vector3(10, 0, 10), new Vector3(3, 0, 50), new Vector3(105, 0, 85), new Vector3(50, 0, 3), new Vector3(50, 0, 87), new Vector3(3, 0, 85), new Vector3(115, 0, 45) };
            for (int k = 0; k < targets.Length; k++)
            {
                var tgt = targets[k];
                cam0.transform.position = tgt + offset;
                yield return Review.Shot($"fogprobe-look{k}");
                baseN = Magenta($"fogprobe-look{k}", out box);
                Log($"look {tgt} magenta={baseN} decalsInView={decals.Count(d => d && d.isActiveAndEnabled && ScreenRect(cam0, new Bounds(d.transform.position, d.size)).Overlaps(new Rect(0, 0, Review.Width, Review.Height)))}");
                if (baseN > 3000) break;
            }
            if (baseN <= 3000) { Assert.Pass("no magenta at any look"); yield break; }
            yield return Review.Shot("fogprobe-base");
            baseN = Magenta("fogprobe-base", out box);
            var cam = Camera.main;
            Log($"base magenta={baseN} box={box} me={(Flow.MyCharacter() ? Flow.MyCharacter().transform.position.ToString() : "-")} camera bg={(cam ? cam.backgroundColor.ToString() : "-")} clear={(cam ? cam.clearFlags.ToString() : "-")}");
            Log($"globalKeywords=[{string.Join(",", Shader.enabledGlobalKeywords.Select(k => k.name))}]");
            if (cam != null)
            {
                var old = cam.backgroundColor;
                cam.backgroundColor = Color.green;
                yield return Review.Shot("fogprobe-green");
                Log($"green-bg magenta={Magenta("fogprobe-green", out _)}");
                cam.backgroundColor = old;
            }
            // Renderer features of the camera and its overlays, one off at a time.
            var cams = new List<Camera> { cam };
            if (cam != null && cam.TryGetComponent<UnityEngine.Rendering.Universal.UniversalAdditionalCameraData>(out var cd) && cd.cameraStack != null) cams.AddRange(cd.cameraStack.Where(c => c));
            var feats = cams.Where(c => c && c.TryGetComponent<UnityEngine.Rendering.Universal.UniversalAdditionalCameraData>(out _))
                .SelectMany(c => (typeof(UnityEngine.Rendering.Universal.ScriptableRenderer).GetProperty("rendererFeatures", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                    ?.GetValue(c.GetComponent<UnityEngine.Rendering.Universal.UniversalAdditionalCameraData>().scriptableRenderer) as List<UnityEngine.Rendering.Universal.ScriptableRendererFeature>) ?? new List<UnityEngine.Rendering.Universal.ScriptableRendererFeature>())
                .Where(f => f != null && f.isActive).Distinct().ToList();
            int i = 0;
            foreach (var f in feats)
            {
                f.SetActive(false);
                string key = $"fogprobe-f{i:D2}";
                yield return Review.Shot(key);
                Log($"feature-off {key} {f.name} {f.GetType().Name} magenta={Magenta(key, out _)} (base {baseN})");
                f.SetActive(true);
                i++;
            }
            var mbox = new Rect(box.x, box.y, box.width, box.height);
            var cands = Object.FindObjectsByType<Renderer>(FindObjectsSortMode.None)
                .Where(r => r.enabled && r.gameObject.activeInHierarchy && cam != null && ScreenRect(cam, r.bounds).Overlaps(mbox)).ToList();
            foreach (var r in cands)
                Log($"cand {Path(r.transform)} {r.GetType().Name} bounds={r.bounds.center}/{r.bounds.size} mats={string.Join(" | ", r.sharedMaterials.Select(m => m == null ? "null" : $"{m.name}<{(m.shader ? m.shader.name : "null")}> kw=[{string.Join(",", m.enabledKeywords.Select(k => k.name))}]"))}");
            i = 0;
            foreach (var group in cands.GroupBy(r => r.sharedMaterial && r.sharedMaterial.shader ? r.sharedMaterial.shader.name : "null"))
            {
                var list = group.ToList();
                foreach (var r in list) r.enabled = false;
                string key = $"fogprobe-s{i:D2}";
                yield return Review.Shot(key);
                Log($"shader-off {key} {group.Key} renderers={list.Count} magenta={Magenta(key, out _)} (base {baseN})");
                foreach (var r in list) r.enabled = true;
                i++;
            }
            Time.timeScale = 1f;
            Assert.Pass("fog probe done");
        }
    }
}
