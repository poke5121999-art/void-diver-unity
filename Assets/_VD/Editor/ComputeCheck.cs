using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Unity.Collections;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;

namespace VD.EditorTools
{
    // Gate for the compute shaders rebuilt from the game's DXBC (tools/shaders/compute.sh -> Assets/_VD/Shaders/Compute):
    //   -executeMethod VD.EditorTools.ComputeCheck.Run [-vdComputeDir Assets/_VD/Shaders/Compute] [-vdComputeDump Temp/vd_compute]
    // 1. Import messages of every .compute (VD_COMPUTE_ERROR).
    // 2. Every `#pragma kernel` is found (HasKernel/FindKernel), its thread group size matches the [numthreads] of the
    //    source, and it compiles for Direct3D11 (what the game shipped), OpenGLCore (this editor) and Vulkan; the compiled
    //    blobs go to -vdComputeDump (the D3D11 one is DXBC, the GL one GLSL text) for comparison with the originals.
    // 3. References: the ComputeShader every game asset that held one now points at (VD_COMPUTE_REF; "stale" = still an
    //    exported .asset, i.e. tools/shaders/apply.sh has not run since compute.sh).
    // 4. GPU dispatch on the editor's device with readback, for the kernels whose result can be predicted from the
    //    disassembly by hand (including the two that load from their UAV, the D3D-vs-GL format case).
    // Log: VD_COMPUTE_KERNEL / VD_COMPUTE_GPU lines, then
    // VD_COMPUTE_CHECK shaders=.. kernels=.. found=.. compileFailed=.. gpuFailed=.. staleRefs=.. errors=..
    public static class ComputeCheck
    {
        static readonly Regex KernelPragma = new Regex(@"^\s*#pragma\s+kernel\s+(\w+)", RegexOptions.Multiline);
        static readonly Regex NumThreads = new Regex(@"\[numthreads\((\d+),\s*(\d+),\s*(\d+)\)\]\s*\n\s*void\s+(\w+)\(");
        static readonly string[] RefHolders =
        {
            "Assets/AssetBundles/dependencies_assets_prefab/VisualManagers.prefab",
            "Assets/AssetBundles/dependencies_assets_prefab/FilmVisualManagers.prefab",
            "Assets/AssetBundles/dependencies_assets_scriptable/BackgroundVisualAreaSetting.asset",
            "Assets/AssetBundles/remote_prefab_assets_scene/LoungeScene.prefab",
            "Assets/AssetBundles/remote_prefab_assets_scene/StageScene.prefab",
        };

        static int errors, gpuFailed;

        public static void Run()
        {
            errors = 0;
            gpuFailed = 0;
            string dir = Arg("-vdComputeDir", "Assets/_VD/Shaders/Compute");
            string dump = Arg("-vdComputeDump", "Temp/vd_compute");
            Directory.CreateDirectory(dump);
            var compile = typeof(ShaderUtil).GetMethod("CompileComputeShaderVariant", BindingFlags.NonPublic | BindingFlags.Static);
            if (compile == null) Fail("ShaderUtil.CompileComputeShaderVariant not found; per-API compile skipped");
            var apis = new[]
            {
                (GraphicsDeviceType.Direct3D11, BuildTarget.StandaloneWindows64),
                (GraphicsDeviceType.OpenGLCore, BuildTarget.StandaloneLinux64),
                (GraphicsDeviceType.Vulkan, BuildTarget.StandaloneLinux64),
            };
            int shaders = 0, kernels = 0, found = 0, compileFailed = 0;
            var toCompile = new List<(ComputeShader, string, int)>();
            foreach (string path in Directory.GetFiles(dir, "*.compute").OrderBy(p => p))
            {
                string p = path.Replace('\\', '/');
                var cs = AssetDatabase.LoadAssetAtPath<ComputeShader>(p);
                if (cs == null) { Fail($"{p}: did not import as a ComputeShader"); continue; }
                shaders++;
                foreach (var m in ShaderUtil.GetComputeShaderMessages(cs))
                {
                    if (m.severity == ShaderCompilerMessageSeverity.Error) Fail($"{cs.name} [{m.platform}] {m.file}:{m.line}: {m.message}");
                    else Debug.Log($"VD_COMPUTE_WARNING {cs.name} [{m.platform}] {m.file}:{m.line}: {m.message}");
                }
                string src = File.ReadAllText(p);
                var groups = NumThreads.Matches(src).Cast<Match>().ToDictionary(m => m.Groups[4].Value,
                    m => new[] { uint.Parse(m.Groups[1].Value), uint.Parse(m.Groups[2].Value), uint.Parse(m.Groups[3].Value) });
                foreach (Match km in KernelPragma.Matches(src))
                {
                    string k = km.Groups[1].Value;
                    kernels++;
                    int idx;
                    try { idx = cs.FindKernel(k); }
                    catch (ArgumentException e) { Fail($"{cs.name}.{k}: FindKernel threw {e.Message}"); continue; }
                    if (!cs.HasKernel(k)) { Fail($"{cs.name}.{k}: HasKernel false"); continue; }
                    found++;
                    cs.GetKernelThreadGroupSizes(idx, out uint x, out uint y, out uint z);
                    if (!groups.TryGetValue(k, out var want) || want[0] != x || want[1] != y || want[2] != z)
                        Fail($"{cs.name}.{k}: thread group {x},{y},{z} differs from the source [numthreads]");
                    Debug.Log($"VD_COMPUTE_KERNEL {cs.name}.{k} index={idx} group={x},{y},{z} supported={cs.IsSupported(idx)}");
                    toCompile.Add((cs, k, idx));
                }
            }
            // after this compile the ComputeShader objects are not safe to dispatch from (FindKernel segfaulted in
            // compute-1), so the per-API compile runs last
            if (SystemInfo.supportsComputeShaders) GpuTests(dir);
            else Fail($"GPU tests skipped: {SystemInfo.graphicsDeviceType} has no compute support");

            foreach (var (cs, k, idx) in toCompile)
            {
                {
                    var results = new List<string>();
                    if (compile != null)
                    {
                        foreach (var (api, target) in apis)
                        {
                            ShaderData.VariantCompileInfo r;
                            try { r = (ShaderData.VariantCompileInfo)compile.Invoke(null, new object[] { cs, idx, new string[0], api, target }); }
                            catch (Exception e) { compileFailed++; Fail($"{cs.name}.{k} {api}: exception {e.InnerException?.Message ?? e.Message}"); continue; }
                            if (!r.Success)
                            {
                                compileFailed++;
                                var msg = r.Messages?.FirstOrDefault(mm => mm.severity == ShaderCompilerMessageSeverity.Error);
                                Fail($"{cs.name}.{k} {api}: {msg?.message} ({msg?.file}:{msg?.line})");
                                continue;
                            }
                            File.WriteAllBytes(Path.Combine(dump, $"{cs.name}_{k}_{api}.bin"), r.ShaderData ?? new byte[0]);
                            results.Add($"{api}={r.ShaderData?.Length ?? 0}B");
                        }
                    }
                    Debug.Log($"VD_COMPUTE_COMPILE {cs.name}.{k} {string.Join(" ", results)}");
                }
            }

            int staleRefs = 0;
            foreach (string holder in RefHolders)
            {
                if (AssetDatabase.LoadMainAssetAtPath(holder) == null) { Debug.Log($"VD_COMPUTE_REF {holder}: missing"); continue; }
                foreach (string dep in AssetDatabase.GetDependencies(holder, false))
                {
                    if (AssetDatabase.GetMainAssetTypeAtPath(dep) != typeof(ComputeShader)) continue;
                    bool rebuilt = dep.StartsWith(dir + "/");
                    if (!rebuilt) staleRefs++;
                    Debug.Log($"VD_COMPUTE_REF {holder} -> {dep}{(rebuilt ? "" : " (stale)")}");
                }
            }
            foreach (string guid in AssetDatabase.FindAssets("t:ComputeShader", new[] { "Assets" }))
            {
                string p = AssetDatabase.GUIDToAssetPath(guid);
                if (!p.StartsWith(dir + "/")) Debug.Log($"VD_COMPUTE_OTHER {p}");
            }

            Debug.Log($"VD_COMPUTE_CHECK shaders={shaders} kernels={kernels} found={found} compileFailed={compileFailed} gpuFailed={gpuFailed} staleRefs={staleRefs} errors={errors} device={SystemInfo.graphicsDeviceType}");
        }

        // ---------------------------------------------------------------- GPU dispatch with readback

        static void GpuTests(string dir)
        {
            const int W = 16, H = 16;
            ComputeShader Load(string n) => AssetDatabase.LoadAssetAtPath<ComputeShader>($"{dir}/{n}.compute");

            // MinimapSightMaskPass.SightUnits (MinimapSightCore: R8G8_UNorm target, 12-byte units): loads result.x, keeps it
            // as the floor of x, y starts at 0; both become max(smoothstep falloff of every unit). One unit at (8, 8),
            // radius 4, _RatioWorldToTexture 1: t = saturate((d - 4) / (max(4 - 1, 0) - 4)) = saturate(4 - d).
            {
                var cs = Load("MinimapSightMaskPass");
                int k = cs.FindKernel("SightUnits");
                var rt = Rt(W, H, GraphicsFormat.R8G8_UNorm);
                Fill(rt, new Color(0.6f, 0.3f, 0f, 0f));
                var before = Read(rt);
                var buf = new ComputeBuffer(1, 12);
                buf.SetData(new[] { 8f, 8f, 4f });
                cs.SetInt("unitCount", 1);
                cs.SetFloat("_RatioWorldToTexture", 1f);
                cs.SetBuffer(k, "units", buf);
                cs.SetTexture(k, "result", rt);
                cs.Dispatch(k, W / 8, H / 8, 1);
                var after = Read(rt);
                int bad = 0;
                for (int py = 0; py < H; py++)
                for (int px = 0; px < W; px++)
                {
                    int i = (py * W + px) * 2;
                    float d = Mathf.Sqrt((px - 8f) * (px - 8f) + (py - 8f) * (py - 8f));
                    float t = Mathf.Clamp01(4f - d), s = t * t * (3f - 2f * t);
                    float wantR = Mathf.Max(before[i] / 255f, s), wantG = s;
                    if (Mathf.Abs(after[i] / 255f - wantR) > 1.5f / 255f || Mathf.Abs(after[i + 1] / 255f - wantG) > 1.5f / 255f) bad++;
                }
                Gpu("MinimapSightMaskPass.SightUnits", bad, $"R8G8 prefill r={before[0]} g={before[1]}; corner r={after[0]} g={after[1]}; center g={after[(8 * W + 8) * 2 + 1]}");
                buf.Release();
                rt.Release();
            }

            // VisualAreaMapSDF.CSJFAToSDF (MapVisualAreaLoader / MinimapSightCore: ARGB32 _SDFOutput, RGFloat JFA buffer):
            // sdf = saturate(+-|id - seed| / _SDFSpread * 0.5 + 0.5) (+ where _MaskInput.b > 0.5) written to r when
            // _OutputChannel is 0, g and b loaded back from _SDFOutput, a = 1. Seed (0, 0) everywhere, mask empty, spread 8.
            {
                var cs = Load("VisualAreaMapSDF");
                int k = cs.FindKernel("CSJFAToSDF");
                var jfa = Rt(W, H, GraphicsFormat.R32G32_SFloat);
                var mask = Rt(W, H, GraphicsFormat.R8G8B8A8_UNorm);
                var sdf = Rt(W, H, GraphicsFormat.R8G8B8A8_UNorm);
                Fill(jfa, Color.clear);
                Fill(mask, Color.clear);
                Fill(sdf, new Color(0.2f, 0.4f, 0.6f, 0.1f));
                var before = Read(sdf);
                cs.SetInts("_OutputSize", W, H);
                cs.SetFloat("_SDFSpread", 8f);
                cs.SetInt("_OutputChannel", 0);
                cs.SetTexture(k, "_JFABufferRead", jfa);
                cs.SetTexture(k, "_MaskInput", mask);
                cs.SetTexture(k, "_SDFOutput", sdf);
                cs.Dispatch(k, W / 8, H / 8, 1);
                var after = Read(sdf);
                int bad = 0;
                for (int py = 0; py < H; py++)
                for (int px = 0; px < W; px++)
                {
                    int i = (py * W + px) * 4;
                    float want = Mathf.Clamp01(-Mathf.Sqrt(px * px + py * py) / 8f * 0.5f + 0.5f);
                    if (Mathf.Abs(after[i] / 255f - want) > 1.5f / 255f || after[i + 1] != before[i + 1] || after[i + 2] != before[i + 2] || after[i + 3] != 255) bad++;
                }
                Gpu("VisualAreaMapSDF.CSJFAToSDF", bad, $"prefill {before[0]},{before[1]},{before[2]},{before[3]}; (0,0)={after[0]},{after[1]},{after[2]},{after[3]}; (4,0) r={after[16]}");
                jfa.Release();
                mask.Release();
                sdf.Release();
            }

            // PlaneLightBufferPass.CSMain (PlanePointManager: ARGBFloat): no points -> (0, 0, 0, 1) everywhere.
            {
                var cs = Load("PlaneLightBufferPass");
                int k = cs.FindKernel("CSMain");
                var rt = Rt(W, H, GraphicsFormat.R32G32B32A32_SFloat);
                Fill(rt, new Color(0.5f, 0.5f, 0.5f, 0.5f));
                cs.SetVector("_PlaneBoundMinPos", Vector3.zero);
                cs.SetVector("_PlaneBoundMaxPos", new Vector3(16f, 0f, 16f));
                cs.SetVector("_PlaneBoundSize", Vector3.one);
                cs.SetVector("_PlaneBoundResolution", new Vector4(W, 1f, H, 0f));
                cs.SetInt("_PlanePointBufferCount", 0);
                cs.SetTexture(k, "_View", rt);
                cs.Dispatch(k, W / 8, H / 8, 1);
                var f = Floats(Read(rt));
                int bad = 0;
                for (int i = 0; i < W * H; i++)
                    if (f[i * 4] != 0f || f[i * 4 + 1] != 0f || f[i * 4 + 2] != 0f || f[i * 4 + 3] != 1f) bad++;
                Gpu("PlaneLightBufferPass.CSMain", bad, $"(0,0)={f[0]},{f[1]},{f[2]},{f[3]}");
                rt.Release();
            }

            // PlaneSightTexturePass.CSMain (PlaneSightCore): _GlobalPlaneSystemEnable < 0.001 -> (1, 1, 1, 1) everywhere.
            {
                var cs = Load("PlaneSightTexturePass");
                int k = cs.FindKernel("CSMain");
                var rt = Rt(W, H, GraphicsFormat.R8G8B8A8_UNorm);
                Fill(rt, Color.clear);
                var sight = new ComputeBuffer(1, 40);
                var ray = new ComputeBuffer(1, 52);
                cs.SetFloat("_GlobalPlaneSystemEnable", 0f);
                cs.SetBuffer(k, "_SightBuffer", sight);
                cs.SetBuffer(k, "_RayBuffer", ray);
                cs.SetTexture(k, "_View", rt);
                cs.Dispatch(k, W / 8, H / 8, 1);
                var b = Read(rt);
                int bad = b.Count(v => v != 255);
                Gpu("PlaneSightTexturePass.CSMain", bad, $"(0,0)={b[0]},{b[1]},{b[2]},{b[3]}");
                sight.Release();
                ray.Release();
                rt.Release();
            }
        }

        static RenderTexture Rt(int w, int h, GraphicsFormat f)
        {
            var rt = new RenderTexture(w, h, 0, f) { enableRandomWrite = true, filterMode = FilterMode.Point };
            rt.Create();
            return rt;
        }

        static void Fill(RenderTexture rt, Color c)
        {
            var prev = RenderTexture.active;
            RenderTexture.active = rt;
            GL.Clear(false, true, c);
            RenderTexture.active = prev;
        }

        // raw bytes in the texture's own format
        static byte[] Read(RenderTexture rt)
        {
            var req = AsyncGPUReadback.Request(rt);
            req.WaitForCompletion();
            if (req.hasError) { Fail($"readback of {rt.graphicsFormat} failed"); return new byte[rt.width * rt.height * 16]; }
            return req.GetData<byte>().ToArray();
        }

        static float[] Floats(byte[] b)
        {
            var f = new float[b.Length / 4];
            Buffer.BlockCopy(b, 0, f, 0, f.Length * 4);
            return f;
        }

        static void Gpu(string what, int bad, string detail)
        {
            if (bad > 0) gpuFailed++;
            Debug.Log($"VD_COMPUTE_GPU {what} {(bad == 0 ? "ok" : "FAILED")} badPixels={bad} {detail}");
        }

        static void Fail(string msg)
        {
            errors++;
            Debug.LogError("VD_COMPUTE_ERROR " + msg);
        }

        static string Arg(string name, string fallback)
        {
            var args = Environment.GetCommandLineArgs();
            int i = Array.IndexOf(args, name);
            return i >= 0 && i + 1 < args.Length ? args[i + 1] : fallback;
        }
    }
}
