using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace VD.PlayTests
{
    // A4c: render probe for the tutorial stage. Plays like A4 (title, New Game, prologue) into the stage, then at the
    // tutorial start and in the first fight logs what draws near the player (VD_PROBE lines: cameras, URP asset, renderer
    // features, volume stack, renderers and their materials) and takes shots with one feature or one renderer off at a
    // time (captures/review/probe-*.png), to tell which object or pass draws what.
    public class A4c_StageProbe
    {
        [TearDown]
        public void Restore()
        {
            Flow.RestoreInput();
            Flow.RestorePlatform();
        }

        static IEnumerator A4(string method, params object[] args) =>
            (IEnumerator)typeof(A4_TutorialFlow).GetMethod(method, BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, args);

        [UnityTest, Timeout(2400000)]
        public IEnumerator ProbeTutorialStage()
        {
            LogAssert.ignoreFailingMessages = true;
            Flow.Tag = "VD_A4C";
            SceneManager.LoadScene(0);
            MonoBehaviour title = null;
            yield return Flow.WaitUntil(() => (title = Flow.Find("StartSceneView")) != null
                && (Flow.Get(title, "Buttons") as GameObject)?.activeInHierarchy == true
                && Flow.Ready(Flow.Button(title, "Play_Button")), 120f, "title menu");
            Assert.IsTrue(Flow.LastWaitOk, "title menu did not come up");
            yield return new WaitForSecondsRealtime(3f);
            yield return Review.Shot("probe-title");
            yield return Flow.Click(title, "Play_Button");
            yield return Flow.WaitView("InputNamePopupView", 30f);
            var nameView = Flow.Find("InputNamePopupView");
            yield return new WaitForSecondsRealtime(1f);
            Flow.Type(Flow.Get(nameView, "InputField"), "Diver");
            yield return Flow.Click(nameView, "Confirm_Button");
            yield return Flow.WaitView("TextPopupView", 20f);
            yield return Flow.Click(Flow.Find("TextPopupView"), "Confirm_Button");
            yield return Flow.WaitView("RoomDifficultyPopupView", 30f);
            yield return new WaitForSecondsRealtime(1f);
            yield return Flow.Click(Flow.Find("RoomDifficultyPopupView"), "Confirm_Button");
            yield return Flow.WaitUntil(() => Flow.Find("LocalRoomCreationPopupView") != null || Flow.ActiveScene == "LoungeScene", 60f, "room popup or lounge");
            var room = Flow.Find("LocalRoomCreationPopupView");
            if (room != null) yield return Flow.Click(room, "BtnConfirm_Button");
            yield return Flow.WaitUntil(() => Flow.ActiveScene == "LoungeScene", 180f, "LoungeScene");
            yield return Flow.WaitUntil(() => Flow.StageState() == "Playing" || Flow.Find("DialogPopupView") != null, 240f, "lounge Playing or prologue dialog");
            yield return new WaitForSecondsRealtime(2f);
            RenderProbe.Dump("lounge-pre");
            yield return Review.Shot("probe-lounge-pre");
            yield return A4("AdvanceDialogs", (System.Func<bool>)(() => Flow.ActiveScene == "StageScene"), 900f, "prologue", null, null, null, null);
            Assert.AreEqual("StageScene", Flow.ActiveScene, "prologue did not lead to the tutorial stage");
            yield return Flow.WaitUntil(() => Flow.StageState() == "Playing", 240f, "stage Playing");
            yield return Flow.WaitUntil(() => Flow.Find("DialogPopupView") != null, 30f, "tutorial dialog");
            yield return new WaitForSecondsRealtime(2f);

            RenderProbe.Dump("start");
            yield return RenderProbe.Bisect("start");

            yield return A4("AdvanceDialogs", (System.Func<bool>)(() => Flow.Find("DialogPopupView") == null), 120f, "tutorial intro", null, null, null, null);
            yield return new WaitForSecondsRealtime(3f);
            Flow.RouteInputToGame();
            RenderProbe.Dump("hud");
            yield return RenderProbe.Bisect("hud");

            yield return A4("Go", new Vector3(19.4f, 0f, 21.5f), 1.0f, 40f, "trigger 100091", 0f, false);
            yield return A4("Go", new Vector3(13f, 0f, 15f), 1.5f, 40f, "monster room", 5f, false);
            RenderProbe.Dump("combat");
            yield return RenderProbe.Bisect("combat");

            // Attack effects: basic attacks and the Q skill on the nearest monster, shot right after each press (the slash
            // and the skill circle last a fraction of a second).
            var nearest = typeof(A4_TutorialFlow).GetMethod("NearestAliveMonster", BindingFlags.NonPublic | BindingFlags.Static);
            for (int n = 0; n < 4; n++)
            {
                var m = nearest.Invoke(null, new object[] { 12f }) as Component;
                if (m == null) break;
                yield return A4("WalkTo", (System.Func<Vector3?>)(() => m ? m.transform.position : (Vector3?)null), 1.5f, 4f, "probe approach", false);
                Flow.Hold();
                var cam = Camera.main;
                Vector2 aim = cam ? (Vector2)cam.WorldToScreenPoint(m.transform.position) : new Vector2(Screen.width / 2f, Screen.height / 2f);
                if (n == 2) yield return Flow.Keys(0.05f, UnityEngine.InputSystem.Key.Q);
                else yield return Flow.MouseClick(aim);
                foreach (var dt in new[] { 0.05f, 0.12f, 0.25f })
                {
                    yield return new WaitForSecondsRealtime(dt);
                    yield return Review.Shot($"probe-fight{n}-{(int)(dt * 100):D2}");
                }
                yield return new WaitForSecondsRealtime(0.6f);
            }
            Assert.Pass("probe done");
        }
    }

    // A4c lounge: boots from saves taken after the tutorial (VD_PROBE_SAVES, default the end of A4 run 25: pet 1001 set,
    // campaign 1100 cleared), presses Continue and dumps the lounge around the player and the pet.
    public class A4c_LoungeProbe
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void PrepareSaves()
        {
            if (!Flow.RunFor("A4c_LoungeProbe")) return;
            // like Flow.PrepareFlowRun: errors the game logs while it boots would fail the test before its body runs
            Application.logMessageReceivedThreaded += (message, stack, type) =>
            {
                if (type == LogType.Log || type == LogType.Warning) return;
                try { if (!LogAssert.ignoreFailingMessages) LogAssert.ignoreFailingMessages = true; }
                catch (System.InvalidOperationException) { }
            };
            Flow.FreshProfile();
            string from = System.Environment.GetEnvironmentVariable("VD_PROBE_SAVES") ?? "captures/review/saves-prev-20261024-130354";
            string dir = System.IO.Path.Combine(System.IO.Directory.GetParent(Application.dataPath).FullName, "SavesDir");
            foreach (var f in System.IO.Directory.GetFiles(from, "*.sav"))
                System.IO.File.Copy(f, System.IO.Path.Combine(dir, System.IO.Path.GetFileName(f)), true);
            Debug.LogWarning($"VD_PROBE saves from {from}");
            Flow.UseLocalPlatform();
        }

        [TearDown]
        public void Restore()
        {
            Flow.RestoreInput();
            Flow.RestorePlatform();
        }

        [UnityTest, Timeout(1200000)]
        public IEnumerator ProbeLounge()
        {
            LogAssert.ignoreFailingMessages = true;
            Flow.Tag = "VD_A4C";
            SceneManager.LoadScene(0);
            MonoBehaviour title = null;
            yield return Flow.WaitUntil(() => (title = Flow.Find("StartSceneView")) != null
                && (Flow.Get(title, "Buttons") as GameObject)?.activeInHierarchy == true
                && Flow.Ready(Flow.Button(title, "Play_Button")), 120f, "title menu");
            Assert.IsTrue(Flow.LastWaitOk, "title menu did not come up");
            yield return new WaitForSecondsRealtime(3f);
            yield return Review.Shot("probe-title");
            Flow.Log($"title play='{Flow.Get(Flow.Get(title, "Play_Button_Text"), "text")}'");
            yield return Flow.Click(title, "Play_Button");
            float start = Time.realtimeSinceStartup;
            while (Flow.ActiveScene != "LoungeScene" && Time.realtimeSinceStartup - start < 120f)
            {
                foreach (var (view, button) in new[] { ("RoomDifficultyPopupView", "Confirm_Button"), ("LocalRoomCreationPopupView", "BtnConfirm_Button"), ("TextPopupView", "Confirm_Button") })
                    if (Flow.Find(view) != null && Flow.Ready(Flow.Button(Flow.Find(view), button))) yield return Flow.Click(Flow.Find(view), button);
                yield return new WaitForSecondsRealtime(0.5f);
            }
            yield return Flow.WaitUntil(() => Flow.StageState() == "Playing", 240f, "lounge Playing");
            yield return new WaitForSecondsRealtime(6f);
            foreach (var m in Object.FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None))
                if (m != null && m.GetType().Name.StartsWith("Pet"))
                    Debug.LogWarning($"VD_PROBE pet {m.GetType().Name} {m.name} active={m.isActiveAndEnabled} pos={m.transform.position}");
            var gm = Flow.Find("GameManager");
            Debug.LogWarning($"VD_PROBE lounge petId={Flow.Get(Flow.Find("PlayerManager"), "SelectedPetId")} me={(Flow.MyCharacter() ? Flow.MyCharacter().transform.position.ToString() : "-")}");
            RenderProbe.Dump("lounge");
            yield return Review.Shot("probe-lounge");
            Assert.Pass("lounge probe done");
        }
    }

    public static class RenderProbe
    {
        static void Log(string s) => Debug.LogWarning("VD_PROBE " + s);

        static string Path(Transform t) => t.parent == null ? t.name : Path(t.parent) + "/" + t.name;

        static Vector3 Me() { var me = Flow.MyCharacter(); return me ? me.transform.position : Camera.main ? Camera.main.transform.position : Vector3.zero; }

        static ScriptableRenderer RendererOf(Camera c) =>
            c != null && c.TryGetComponent<UniversalAdditionalCameraData>(out var d) ? d.scriptableRenderer : null;

        static List<ScriptableRendererFeature> Features(ScriptableRenderer r) =>
            r == null ? new List<ScriptableRendererFeature>() :
            (typeof(ScriptableRenderer).GetProperty("rendererFeatures", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)?.GetValue(r) as List<ScriptableRendererFeature>)
            ?? new List<ScriptableRendererFeature>();

        static Camera BaseCamera() =>
            Camera.allCameras.Where(c => c.enabled && c.targetTexture == null && c.TryGetComponent<UniversalAdditionalCameraData>(out var d) && d.renderType == CameraRenderType.Base)
                .OrderBy(c => c.depth).FirstOrDefault();

        public static void Dump(string tag)
        {
            var me = Me();
            var asset = UniversalRenderPipeline.asset;
            Log($"{tag} screen={Screen.width}x{Screen.height} quality={QualitySettings.GetQualityLevel()} asset={(asset ? asset.name : "-")} renderScale={(asset ? asset.renderScale : -1)} upscale={(asset ? asset.upscalingFilter.ToString() : "-")} msaa={(asset ? asset.msaaSampleCount : -1)} hdr={(asset ? asset.supportsHDR : false)} rtMax={Shader.GetGlobalFloat("_VD_RT_MAX_HEIGHT")} dyn={ScalableBufferManager.widthScaleFactor} me={me}");
            foreach (var c in Camera.allCameras.Concat(Object.FindObjectsByType<Camera>(FindObjectsInactive.Include, FindObjectsSortMode.None)).Distinct())
            {
                c.TryGetComponent<UniversalAdditionalCameraData>(out var d);
                var stack = d != null && d.cameraStack != null ? string.Join(",", d.cameraStack.Where(x => x).Select(x => x.name)) : "";
                Log($"{tag} cam {Path(c.transform)} en={c.enabled && c.gameObject.activeInHierarchy} depth={c.depth} tt={(c.targetTexture ? $"{c.targetTexture.name}:{c.targetTexture.width}x{c.targetTexture.height}" : "-")} px={c.pixelWidth}x{c.pixelHeight} rect={c.rect} ortho={c.orthographic}/{c.orthographicSize:F2} fov={c.fieldOfView:F1} mask={c.cullingMask:X} type={(d ? d.renderType.ToString() : "noURP")} renderer={(d ? d.scriptableRenderer?.GetType().Name + ":" + Flow.Get(d, "m_RendererIndex") : "-")} pp={(d ? d.renderPostProcessing : false)} aa={(d ? d.antialiasing + "/" + d.antialiasingQuality : "-")} stack=[{stack}] dynres={c.allowDynamicResolution} msaa={c.allowMSAA} pos={c.transform.position}");
                foreach (var f in Features(RendererOf(c)))
                    Log($"{tag} feat {c.name} {f?.name} {f?.GetType().Name} active={f?.isActive}");
            }
            var stackV = VolumeManager.instance.stack;
            var comps = (Flow.Get(stackV, "components") as System.Collections.IDictionary)?.Values.Cast<VolumeComponent>().ToList() ?? new List<VolumeComponent>();
            foreach (var comp in comps)
            {
                if (comp == null) continue;
                var type = comp.GetType();
                var sb = new StringBuilder();
                foreach (var p in comp.parameters.Select((p, i) => (p, i)))
                {
                    var name = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).FirstOrDefault(fi => fi.GetValue(comp) == p.p)?.Name ?? p.i.ToString();
                    var v = p.p.GetType().GetProperty("value")?.GetValue(p.p);
                    sb.Append($" {name}={v}");
                }
                Log($"{tag} vol {type.Name} active={comp.active}{sb}");
            }
            foreach (var v in Object.FindObjectsByType<Volume>(FindObjectsSortMode.None))
                Log($"{tag} volume {Path(v.transform)} en={v.isActiveAndEnabled} global={v.isGlobal} w={v.weight} prio={v.priority} profile={(v.sharedProfile ? v.sharedProfile.name : "-")} comps={(v.sharedProfile ? string.Join(",", v.sharedProfile.components.Select(x => x.GetType().Name + (x.active ? "" : "(off)"))) : "")}");
            foreach (var r in Object.FindObjectsByType<Renderer>(FindObjectsSortMode.None))
            {
                if (!r.enabled || !r.gameObject.activeInHierarchy) continue;
                var b = r.bounds;
                var flat = new Vector2(b.center.x - me.x, b.center.z - me.z);
                if (flat.magnitude - new Vector2(b.extents.x, b.extents.z).magnitude > 6f) continue;
                if (r.gameObject.layer == LayerMask.NameToLayer("UI")) continue;
                var mats = string.Join(" | ", r.sharedMaterials.Select(m => m == null ? "null" :
                    $"{m.name}<{m.shader.name}>q{m.renderQueue}{(m.mainTexture ? " tex=" + m.mainTexture.name : "")} kw=[{string.Join(",", m.shaderKeywords)}]"));
                Log($"{tag} rend {Path(r.transform)} {r.GetType().Name} vis={r.isVisible} layer={LayerMask.LayerToName(r.gameObject.layer)} sort={r.sortingLayerName}/{r.sortingOrder} c={b.center} s={b.size} mats={mats}");
            }
            foreach (var p in Object.FindObjectsByType<DecalProjector>(FindObjectsSortMode.None))
                Log($"{tag} decal {Path(p.transform)} en={p.isActiveAndEnabled} size={p.size} mat={(p.material ? p.material.name + "<" + p.material.shader.name + ">" : "null")} pos={p.transform.position}");
            foreach (var l in Object.FindObjectsByType<Light>(FindObjectsSortMode.None))
                if (l.isActiveAndEnabled && (l.type == LightType.Directional || (l.transform.position - me).magnitude < 15f))
                    Log($"{tag} light {Path(l.transform)} {l.type} col={l.color} int={l.intensity} range={l.range} angle={l.spotAngle} shadows={l.shadows} cookie={(l.cookie ? l.cookie.name : "-")} pos={l.transform.position}");
            foreach (var name in new[] { "_PlaneSightRenderTarget", "_GlobalIsRuntime", "_GlobalCharacterShadowColor", "_ScreenBlurProgress", "_ScreenBlurCustomShaderProgress", "_ScreenBlurDegree", "_GlobalScreenHeightBlurDegree", "_GlobalScreenDepthBlurDegree", "_VD_GRAB_SPINE", "_VD_GRAB_GLASS", "_VD_GRAB_EFFECT" })
                Log($"{tag} global {name} tex={(Shader.GetGlobalTexture(name) ? Shader.GetGlobalTexture(name).name + ":" + Shader.GetGlobalTexture(name).width + "x" + Shader.GetGlobalTexture(name).height : "-")} f={Shader.GetGlobalFloat(name)} v={Shader.GetGlobalVector(name)}");
        }

        // One shot as is, then one with each renderer feature of the base camera off, then one with each large renderer
        // near the player off.
        public static IEnumerator Bisect(string tag)
        {
            yield return Review.Shot($"probe-{tag}");
            var cam = BaseCamera();
            int i = 0;
            var cams = new List<Camera> { cam };
            if (cam != null && cam.TryGetComponent<UniversalAdditionalCameraData>(out var cd) && cd.cameraStack != null) cams.AddRange(cd.cameraStack.Where(c => c));
            foreach (var f in cams.SelectMany(c => Features(RendererOf(c))).Where(f => f != null && f.isActive).Distinct().ToList())
            {
                f.SetActive(false);
                yield return null;
                yield return Review.Shot($"probe-{tag}-f{i:D2}-{f.name.Replace(' ', '_')}");
                f.SetActive(true);
                i++;
            }
            var me = Me();
            var big = Object.FindObjectsByType<Renderer>(FindObjectsSortMode.None)
                .Where(r => r.enabled && r.gameObject.activeInHierarchy && r.gameObject.layer != LayerMask.NameToLayer("UI"))
                .Where(r => { var b = r.bounds; return new Vector2(b.center.x - me.x, b.center.z - me.z).magnitude < 4f && Mathf.Max(b.size.x, b.size.z) > 1.2f && Mathf.Max(b.size.x, b.size.z) < 30f; })
                .OrderBy(r => new Vector2(r.bounds.center.x - me.x, r.bounds.center.z - me.z).magnitude).Take(30).ToList();
            i = 0;
            foreach (var r in big)
            {
                r.enabled = false;
                yield return null;
                yield return Review.Shot($"probe-{tag}-r{i:D2}");
                Log($"{tag} bisect r{i:D2} = {Path(r.transform)} s={r.bounds.size}");
                r.enabled = true;
                i++;
            }
        }
    }
}
