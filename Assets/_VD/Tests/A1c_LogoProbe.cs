using System.Collections;
using System.IO;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace VD.PlayTests
{
    // The title logo (TitleImage!, Mtl_DE_UI_TITLE_Glitch) runs four glitch effects, each switched on by its own flicker
    // gate in the shader: hash(floor(_Time.y * rate)) above a threshold (0.95 in the material, so each is on in about one
    // time slot in twenty), eased by a smoothstep. The slice gate also drives the chroma split, the RGB fringe.
    public static class LogoGlitch
    {
        public const string Material = "Mtl_DE_UI_TITLE_Glitch";

        static float Frac(float x) => x - Mathf.Floor(x);

        // One gate as the shader computes it (p0_ps_3.hlsl / p0_vs_1.hlsl), in float.
        public static float Gate(float time, float rate, float threshold, float seed)
        {
            float k = Mathf.Floor(time * rate);
            float h = Frac(Mathf.Sin(k * 12.9898005f + seed) * 43758.5469f);
            float x = Mathf.Clamp01((h - threshold) / (1f - threshold));
            return x * x * (3f - 2f * x);
        }

        // Slice+chroma, vertex jitter, warp and block gates at shader time t (URP sets _Time.y = Time.time).
        public static (float slice, float jitter, float warp, float block) Gates(Material m, float t) => (
            Gate(t, m.GetFloat("_SliceFlickerRate"), m.GetFloat("_SliceFlickerThreshold"), 78.2330017f),
            Gate(t, m.GetFloat("_JitterFlickerRate"), m.GetFloat("_JitterFlickerThreshold"), 31.7409992f),
            Gate(t, m.GetFloat("_WarpFlickerRate"), m.GetFloat("_WarpFlickerThreshold"), 56.901001f),
            Gate(t, m.GetFloat("_BlockFlickerRate"), m.GetFloat("_BlockFlickerThreshold"), 113.269997f));

        public static Material Find()
        {
            foreach (var img in Object.FindObjectsByType<Image>(FindObjectsSortMode.None))
                if (img.material != null && img.material.name.StartsWith(Material) && img.isActiveAndEnabled) return img.material;
            return null;
        }

        // The logo box of docs/review/regions.json (title-menu "logo", x 0.03-0.27, y 0.03-0.17, y down): bright pixels and
        // those with |R-B| > 90, the RGB-split fringe the A1 review counted.
        public static (int bright, int fringe) Fringe(Texture2D tex)
        {
            var px = tex.GetPixels32();
            int w = tex.width, h = tex.height, n = 0, f = 0;
            for (int y = Mathf.RoundToInt((1 - 0.17f) * h); y < Mathf.RoundToInt((1 - 0.03f) * h); y++)
                for (int x = Mathf.RoundToInt(0.03f * w); x < Mathf.RoundToInt(0.27f * w); x++)
                {
                    var c = px[y * w + x];
                    if (Mathf.Max(c.r, Mathf.Max(c.g, c.b)) <= 180) continue;
                    n++;
                    if (Mathf.Abs(c.r - c.b) > 90) f++;
                }
            return (n, f);
        }
    }

    // A1c: the logo's RGB fringe over time. Boots like A1, waits for the title menu and shoots it frame after frame
    // without freezing anything, logging per shot the time the cameras rendered at, the four gates predicted from that
    // time and the fringe measured in the shot. Lines: VD_LOGO. Shots with any fringe are kept as logo-gNNN.png. Batchmode
    // steps game time by 1/3 s per shot and takes ~7 s of wall time per shot, so 180 shots cover 60 s in ~25 min.
    public class A1c_LogoProbe
    {
        static void Log(string s) => Debug.LogWarning("VD_LOGO " + s);

        [UnityTest, Timeout(1800000)]
        public IEnumerator LogoOverTime()
        {
            LogAssert.ignoreFailingMessages = true;
            SceneManager.LoadScene(0);
            MonoBehaviour title = null;
            yield return Flow.WaitUntil(() => (title = Flow.Find("StartSceneView")) != null
                && (Flow.Get(title, "Buttons") as GameObject)?.activeInHierarchy == true
                && Flow.Ready(Flow.Button(title, "Play_Button")), 120f, "title menu");
            yield return new WaitForSecondsRealtime(3f);
            var mat = LogoGlitch.Find();
            Assert.IsNotNull(mat, "no active Image draws with " + LogoGlitch.Material);
            Log($"material {mat.name} shader={mat.shader.name} " + string.Join(" ", new[] {
                "_SliceFlickerRate", "_SliceFlickerThreshold", "_GlitchStrength", "_ChromaStrength", "_JitterFlickerRate",
                "_JitterFlickerThreshold", "_VertexJitterStrength", "_WarpFlickerRate", "_WarpFlickerThreshold", "_WarpStrength",
                "_BlockFlickerRate", "_BlockFlickerThreshold", "_BlockStrength" }.Select(p => $"{p}={mat.GetFloat(p):G4}")));

            float renderTime = -1, shaderTime = -1;
            void Begin(ScriptableRenderContext _, Camera c) { if (renderTime < 0) renderTime = Time.time; }
            void End(ScriptableRenderContext _, Camera c) { shaderTime = Shader.GetGlobalVector("_Time").y; }
            RenderPipelineManager.beginCameraRendering += Begin;
            RenderPipelineManager.endCameraRendering += End;
            int shots = 180, withFringe = 0, predictedOn = 0, agree = 0;
            var path = Path.Combine("captures", "review", "logo-sweep.png");
            try
            {
                for (int i = 0; i < shots; i++)
                {
                    renderTime = -1;
                    yield return Review.Shot("logo-sweep");
                    var tex = new Texture2D(2, 2);
                    tex.LoadImage(File.ReadAllBytes(path));
                    var (bright, fringe) = LogoGlitch.Fringe(tex);
                    Object.Destroy(tex);
                    var g = LogoGlitch.Gates(mat, renderTime);
                    bool on = fringe > 0, predicted = g.slice > 0;
                    if (on) { withFringe++; File.Copy(path, Path.Combine("captures", "review", $"logo-g{i:D3}.png"), true); }
                    if (predicted) predictedOn++;
                    if (on == predicted) agree++;
                    Log($"s{i:D3} t={renderTime:F3} _Time.y={shaderTime:F3} slice={g.slice:F2} jitter={g.jitter:F2} warp={g.warp:F2} block={g.block:F2} " +
                        $"bright={bright} fringe={fringe} ({(bright > 0 ? (float)fringe / bright : 0):P0})");
                }
            }
            finally
            {
                RenderPipelineManager.beginCameraRendering -= Begin;
                RenderPipelineManager.endCameraRendering -= End;
            }
            Log($"summary shots={shots} withFringe={withFringe} predictedSliceOn={predictedOn} agree={agree}");
            Assert.Pass($"{withFringe}/{shots} shots with fringe, prediction agrees on {agree}");
        }
    }
}
