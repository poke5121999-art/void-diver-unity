using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace VD.PlayTests
{
    // A1b: which renderer lifts the title's cyan pit. Boots like A1, freezes the title at A1's pose (the clip's),
    // lists every renderer whose screen bounds reach the pit region with its material values, then measures the pit's
    // luma with post-processing off, with each renderer feature off, with each shader group off and with each renderer
    // off. Lines: VD_PIT.
    public class A1b_PitProbe
    {
        // docs/review/regions.json "pit", image coordinates (y down).
        static readonly Rect Pit = Rect.MinMaxRect(0.52f, 0.42f, 0.68f, 0.62f);

        static void Log(string s) => Debug.LogWarning("VD_PIT " + s);

        static string Path(Transform t) => t.parent == null ? t.name : Path(t.parent) + "/" + t.name;

        // Reference regions (docs/review/regions.json title-menu), to tell a pit-only change from a frame-wide one.
        static readonly Rect Ring = Rect.MinMaxRect(0.45f, 0.1f, 0.78f, 0.3f);
        static readonly Rect City = Rect.MinMaxRect(0.3f, 0.3f, 0.5f, 0.6f);

        static float baseMean;

        // Mean and 75th percentile of Rec.601 luma over a region, as parity.py measures it.
        static (float mean, float p75) Measure(Texture2D tex, Color32[] px, Rect r)
        {
            int x0 = Mathf.RoundToInt(r.xMin * tex.width), x1 = Mathf.RoundToInt(r.xMax * tex.width);
            int y0 = Mathf.RoundToInt((1 - r.yMax) * tex.height), y1 = Mathf.RoundToInt((1 - r.yMin) * tex.height);
            var l = new List<float>();
            for (int y = y0; y < y1; y++)
                for (int x = x0; x < x1; x++)
                {
                    var c = px[y * tex.width + x];
                    l.Add(0.299f * c.r + 0.587f * c.g + 0.114f * c.b);
                }
            l.Sort();
            return (l.Average(), l[(int)(l.Count * 0.75f)]);
        }

        static IEnumerator Shot(string label)
        {
            var key = "pit-" + label;
            yield return Review.Shot(key);
            var tex = new Texture2D(2, 2);
            tex.LoadImage(File.ReadAllBytes(Path_(key)));
            var px = tex.GetPixels32();
            var (mean, p75) = Measure(tex, px, Pit);
            var ring = Measure(tex, px, Ring);
            var city = Measure(tex, px, City);
            Object.Destroy(tex);
            if (label == "base") baseMean = mean;
            Log($"luma {label} mean={mean:F1} p75={p75:F0} delta={mean - baseMean:F1} ring={ring.mean:F1} city={city.mean:F1}");
        }

        static string Path_(string key) => System.IO.Path.Combine("captures", "review", key + ".png");

        // Screen rect (viewport, y down) of a renderer's bounds as the main camera sees them.
        static Rect ScreenRect(Camera cam, Bounds b)
        {
            float xmin = 1e9f, ymin = 1e9f, xmax = -1e9f, ymax = -1e9f;
            for (int i = 0; i < 8; i++)
            {
                var p = b.center + Vector3.Scale(b.extents, new Vector3((i & 1) == 0 ? -1 : 1, (i & 2) == 0 ? -1 : 1, (i & 4) == 0 ? -1 : 1));
                var v = cam.WorldToViewportPoint(p);
                if (v.z <= 0) continue;
                xmin = Mathf.Min(xmin, v.x); xmax = Mathf.Max(xmax, v.x);
                ymin = Mathf.Min(ymin, 1 - v.y); ymax = Mathf.Max(ymax, 1 - v.y);
            }
            return xmin > xmax ? Rect.zero : Rect.MinMaxRect(xmin, ymin, xmax, ymax);
        }

        static float Overlap(Rect a, Rect b)
        {
            float w = Mathf.Min(a.xMax, b.xMax) - Mathf.Max(a.xMin, b.xMin), h = Mathf.Min(a.yMax, b.yMax) - Mathf.Max(a.yMin, b.yMin);
            return w > 0 && h > 0 ? w * h / (b.width * b.height) : 0;
        }

        static string Props(Material m, Renderer r)
        {
            var block = new MaterialPropertyBlock();
            r.GetPropertyBlock(block);
            var sb = new StringBuilder();
            var s = m.shader;
            for (int i = 0; i < s.GetPropertyCount(); i++)
            {
                var n = s.GetPropertyName(i);
                switch (s.GetPropertyType(i))
                {
                    case ShaderPropertyType.Color:
                        sb.Append($" {n}={m.GetColor(n)}{(block.HasColor(n) ? "/blk" + block.GetColor(n) : "")}"); break;
                    case ShaderPropertyType.Vector:
                        sb.Append($" {n}={m.GetVector(n)}{(block.HasVector(n) ? "/blk" + block.GetVector(n) : "")}"); break;
                    case ShaderPropertyType.Float:
                    case ShaderPropertyType.Range:
                        sb.Append($" {n}={m.GetFloat(n):G4}{(block.HasFloat(n) ? "/blk" + block.GetFloat(n) : "")}"); break;
                    case ShaderPropertyType.Texture:
                        var t = m.GetTexture(n);
                        sb.Append($" {n}={(t ? t.name : "-")}"); break;
                }
            }
            return sb.ToString();
        }

        static string Particles(Renderer r)
        {
            if (!r.TryGetComponent<ParticleSystem>(out var ps)) return "";
            var main = ps.main;
            var em = ps.emission;
            var col = ps.colorOverLifetime;
            return $" ps(count={ps.particleCount} max={main.maxParticles} startColor={main.startColor.mode}:{main.startColor.color}/{main.startColor.colorMin}/{main.startColor.colorMax} " +
                $"rate={em.rateOverTime.constant:G4} col={col.enabled} simSpeed={main.simulationSpeed} unscaled={main.useUnscaledTime} playing={ps.isPlaying} " +
                $"mode={((ParticleSystemRenderer)r).renderMode} mesh={(((ParticleSystemRenderer)r).mesh ? ((ParticleSystemRenderer)r).mesh.name : "-")})";
        }

        static UnityEngine.Playables.PlayableDirector director;
        static Animator bgAnim;

        // Boot like A1, wait for the menu, then hold A1's camera pose (the clip's), Anim_LoungeBG at lightPhase (0..1 of
        // its 20 s loop) and seeded particles, so every shot after this sees the same frame on any run and API.
        static IEnumerator FreezeAtA1Pose(float lightPhase)
        {
            SceneManager.LoadScene(0);
            // A1 takes the clip's pose at the title timeline's time 12 s after boot (it sways every 39.15 s), but the
            // title is still fading in then; freezing there froze the fade and every shot came out black (pit-run1/2).
            // So note that phase, wait for the menu and the fade, then for the same phase one sway later.
            yield return new WaitForSecondsRealtime(12f);
            director = Object.FindObjectsByType<UnityEngine.Playables.PlayableDirector>(FindObjectsSortMode.None)
                .FirstOrDefault(d => d.playableAsset != null && d.playableAsset.name == "TL_Title_01");
            Assert.IsNotNull(director, "TL_Title_01 director not found at 12 s");
            double phase = director.time;
            MonoBehaviour title = null;
            yield return Flow.WaitUntil(() => (title = Flow.Find("StartSceneView")) != null
                && (Flow.Get(title, "Buttons") as GameObject)?.activeInHierarchy == true
                && Flow.Ready(Flow.Button(title, "Play_Button")), 120f, "title menu");
            yield return new WaitForSecondsRealtime(3f);
            const double sway = 39.15;
            for (float waited = 0; waited < 45f; waited += Time.unscaledDeltaTime)
            {
                if (director.time - phase > 5 && System.Math.Abs(director.time - phase - sway * System.Math.Round((director.time - phase) / sway)) < 0.05) break;
                yield return null;
            }
            Log($"pose director={director.time:F2} phase={phase:F2} cam={(Camera.main ? Camera.main.transform.position : Vector3.zero):F2}");
            // Freeze that pose without Time.timeScale = 0: pause the title timeline, the animators (the fade is over by now; Anim_LoungeBG moves and dims
            // the point lights on a 20 s loop) and the particle systems so each shot
            // below sees the same frame.
            director.Pause();
            foreach (var an in Object.FindObjectsByType<Animator>(FindObjectsSortMode.None)) an.speed = 0;
            // Same light phase in every run: Anim_LoungeBG at lightPhase of its 20 s loop.
            bgAnim = Object.FindObjectsByType<Animator>(FindObjectsSortMode.None).FirstOrDefault(a => a.name == "LoungeBG");
            if (bgAnim)
            {
                bgAnim.Play(bgAnim.GetCurrentAnimatorStateInfo(0).shortNameHash, 0, lightPhase);
                bgAnim.Update(0);
            }
            foreach (var l in Object.FindObjectsByType<Light>(FindObjectsSortMode.None).Where(l => l.type == LightType.Point).OrderBy(l => l.name))
                Log($"frozen light {l.name} int={l.intensity:F2} col={l.color} pos={l.transform.position:F2}");
            // Particles are random per run: replay every root system from a fixed seed for 8 s, then hold it, so two
            // runs (GL and Vulkan) draw the same smoke.
            foreach (var ps in Object.FindObjectsByType<ParticleSystem>(FindObjectsSortMode.None).Where(p => p.transform.parent == null || p.transform.parent.GetComponentInParent<ParticleSystem>() == null))
            {
                ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                foreach (var c in ps.GetComponentsInChildren<ParticleSystem>(true)) { c.useAutoRandomSeed = false; c.randomSeed = 12345; }
                ps.Simulate(8f, true, true, false);
                ps.Pause(true);
            }
            yield return null;
        }

        [UnityTest, Timeout(1800000)]
        public IEnumerator ProbePit()
        {
            LogAssert.ignoreFailingMessages = true;
            yield return FreezeAtA1Pose(0.1f);
            yield return null;
            var cam = Camera.main;
            Assert.IsNotNull(cam, "main camera not found on the title");
            yield return Shot("base");
            Log($"frozen cam={cam.transform.position:F2}");

            var data = cam.GetUniversalAdditionalCameraData();
            data.renderPostProcessing = false;
            yield return Shot("nopost");
            data.renderPostProcessing = true;

            var features = (typeof(ScriptableRenderer).GetProperty("rendererFeatures", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public)
                ?.GetValue(data.scriptableRenderer) as List<ScriptableRendererFeature>) ?? new List<ScriptableRendererFeature>();
            foreach (var f in features.Where(f => f != null && f.isActive).ToList())
            {
                f.SetActive(false);
                yield return Shot("feat-" + f.name.Replace(' ', '_'));
                f.SetActive(true);
            }

            foreach (var l in Object.FindObjectsByType<Light>(FindObjectsSortMode.None).Where(l => l.isActiveAndEnabled).ToList())
            {
                Log($"light {Path(l.transform)} {l.type} col={l.color} int={l.intensity:F3} range={l.range:F2} pos={l.transform.position:F2} mode={l.lightmapBakeType} render={l.renderMode} layers={l.cullingMask}");
                l.enabled = false;
                yield return Shot("light-" + l.name.Replace(' ', '_'));
                l.enabled = true;
            }
            yield return Shot("base2");

            var pit = Object.FindObjectsByType<Renderer>(FindObjectsSortMode.None)
                .Where(r => r.enabled && r.gameObject.activeInHierarchy && r.gameObject.layer != LayerMask.NameToLayer("UI") && (cam.cullingMask & (1 << r.gameObject.layer)) != 0)
                .Select(r => (r, rect: ScreenRect(cam, r.bounds)))
                .Where(x => Overlap(x.rect, Pit) > 0)
                .OrderByDescending(x => Overlap(x.rect, Pit)).ToList();
            foreach (var (r, rect) in pit)
            {
                Log($"rend {Path(r.transform)} {r.GetType().Name} vis={r.isVisible} layer={LayerMask.LayerToName(r.gameObject.layer)} sort={r.sortingLayerName}/{r.sortingOrder} " +
                    $"cover={Overlap(rect, Pit):F2} screen={rect}{Particles(r)}");
                foreach (var m in r.sharedMaterials.Where(m => m))
                    Log($"  mat {m.name} shader={m.shader.name} q={m.renderQueue} kw=[{string.Join(",", m.shaderKeywords)}]{Props(m, r)}");
            }

            foreach (var g in pit.GroupBy(x => x.r.sharedMaterial ? x.r.sharedMaterial.shader.name : "null").ToList())
            {
                foreach (var x in g) x.r.enabled = false;
                yield return Shot("shader-" + g.Key.Replace('/', '_').Replace(' ', '_') + "-" + g.Count());
                foreach (var x in g) x.r.enabled = true;
            }
            int i = 0;
            foreach (var (r, _) in pit.Take(80))
            {
                r.enabled = false;
                yield return Shot($"r{i:D2}");
                Log($"r{i:D2} = {Path(r.transform)}");
                r.enabled = true;
                i++;
            }
            Assert.Pass($"{pit.Count} renderers reach the pit");
        }

        // The pit's luma over Anim_LoungeBG's whole 20 s loop at A1's pose: the loop flickers the point lights in the pit
        // (Light_8 2..20, Light_7 10..18), so one shot at an arbitrary phase says little about a lift. 40 phases, 0.5 s apart.
        [UnityTest, Timeout(1800000)]
        public IEnumerator PitOverTime()
        {
            LogAssert.ignoreFailingMessages = true;
            yield return FreezeAtA1Pose(0f);
            Assert.IsNotNull(bgAnim, "LoungeBG animator not found");
            var lights = Object.FindObjectsByType<Light>(FindObjectsSortMode.None).Where(l => l.type == LightType.Point).OrderBy(l => l.name).ToList();
            var hash = bgAnim.GetCurrentAnimatorStateInfo(0).shortNameHash;
            yield return Shot("warmup");  // the first shot after the freeze misses the held particles (pit-run4 base vs base2)
            for (int i = 0; i < 40; i++)
            {
                bgAnim.Play(hash, 0, i / 40f);
                bgAnim.Update(0);
                yield return null;
                var li = string.Join(" ", lights.Select(l => $"{l.name.Replace("Point Light_", "L")}={l.intensity:F1}"));
                yield return Shot($"t{i:D2}");
                Log($"t{i:D2} bg={i * 0.5f:F1}s {li}");
            }
        }
    }
}
