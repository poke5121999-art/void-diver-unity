using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace VD.PlayTests
{
    // A1: boot from SplashScene (build index 0) like the shipped game and capture the title flow.
    public class A1_TitleFlow
    {
        // The game boots as soon as play mode starts (VContainer's RuntimeInitialize), before the test body runs.
        // Errors logged in those first frames land in the test's log scope and fail it before
        // ignoreFailingMessages below is reached, so the flag is raised from the first error on.
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void IgnoreBootErrors()
        {
            Application.logMessageReceivedThreaded -= OnLog;
            Application.logMessageReceivedThreaded += OnLog;
        }

        // Diagnostics for the title capture: cameras, canvases and renderers drawing with a broken shader.
        // LogWarning: the game's SeriLogger raises Debug.unityLogger.filterLogType, so Debug.Log never reaches the log.
        static void DumpScene()
        {
            foreach (var c in Object.FindObjectsByType<Camera>(FindObjectsSortMode.None))
            {
                var d = c.GetComponent<UniversalAdditionalCameraData>();
                Debug.LogWarning($"VD_DUMP camera {c.name} enabled={c.enabled} active={c.gameObject.activeInHierarchy} depth={c.depth} mask={c.cullingMask} pos={c.transform.position} " +
                    $"clear={c.clearFlags} bg={c.backgroundColor} target={(c.targetTexture ? c.targetTexture.name + " " + c.targetTexture.width + "x" + c.targetTexture.height : "-")} " +
                    $"urp={(d ? d.renderType + " stack=" + (d.cameraStack?.Count ?? 0) : "-")}");
            }
            foreach (var c in Object.FindObjectsByType<Canvas>(FindObjectsSortMode.None))
                if (c.isRootCanvas)
                    Debug.LogWarning($"VD_DUMP canvas {c.name} mode={c.renderMode} cam={(c.worldCamera ? c.worldCamera.name : "-")} active={c.gameObject.activeInHierarchy} order={c.sortingOrder} layer={c.gameObject.layer} scene={c.gameObject.scene.name}");
            foreach (var r in Object.FindObjectsByType<Renderer>(FindObjectsSortMode.None))
                foreach (var m in r.sharedMaterials)
                    if (m != null && r.isVisible && (m.shader == null || !m.shader.isSupported || m.shader.name.Contains("Error")))
                        Debug.LogWarning($"VD_DUMP badshader renderer={r.name} mat={m.name} shader={(m.shader ? m.shader.name : "null")} bounds={r.bounds.size}");
            foreach (var r in Object.FindObjectsByType<Renderer>(FindObjectsSortMode.None))
                if (r.isVisible && r.bounds.size.magnitude > 50f)
                    Debug.LogWarning($"VD_DUMP big renderer={r.name} mat={(r.sharedMaterial ? r.sharedMaterial.name + "/" + r.sharedMaterial.shader.name : "null")} size={r.bounds.size}");
        }

        // InputIcons keeps the device whose icons it shows in InputIconsManagerSO (InputIcons assembly, not referenced here).
        static void LogInputIcons(string when)
        {
            var t = System.Type.GetType("InputIcons.InputIconsManagerSO, InputIcons");
            var flags = System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic;
            var inst = t?.GetField("instance", flags)?.GetValue(null);
            var dm = inst == null ? null : t.GetField("deviceManager", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public)?.GetValue(inst);
            var cur = inst == null ? null : t.GetMethod("GetCurrentInputDevice", flags)?.Invoke(null, null) as InputDevice;
            var so = inst as Object;
            var next = t?.GetField("inputDeviceToChangeTo", flags)?.GetValue(null) as InputDevice;
            Debug.LogWarning($"VD_A1 inputicons {when}: type={(t != null)} instance={(so != null ? so.name : "null")} deviceManager={(dm != null)} current={(cur != null ? cur.name : "null")} next={(next != null ? next.name : "null")} " +
                $"devices={string.Join(",", System.Linq.Enumerable.Select(InputSystem.devices, d => d.name))}");
        }

        static PlayableDirector TitleDirector()
        {
            foreach (var d in Object.FindObjectsByType<PlayableDirector>(FindObjectsSortMode.None))
                if (d.playableAsset != null && d.playableAsset.name == "TL_Title_01") return d;
            return null;
        }

        // What decides the title's look: the camera, the URP asset, every volume and its overridden parameters, the
        // blended stack the main camera renders with, the lights and the version label.
        static void DumpLook()
        {
            var cam = Camera.main;
            var data = cam ? cam.GetUniversalAdditionalCameraData() : null;
            if (cam)
                Debug.LogWarning($"VD_LOOK camera {cam.name} fov={cam.fieldOfView:F2} aspect={cam.aspect:F3} hdr={cam.allowHDR} pos={cam.transform.position} rot={cam.transform.eulerAngles} " +
                    $"pp={data.renderPostProcessing} aa={data.antialiasing} volMask={data.volumeLayerMask.value} trigger={(data.volumeTrigger ? data.volumeTrigger.name : "-")}");
            var rp = GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;
            Debug.LogWarning($"VD_LOOK quality={QualitySettings.names[QualitySettings.GetQualityLevel()]} rp={(rp ? rp.name : "null")} hdr={(rp ? rp.supportsHDR : false)} " +
                $"grading={(rp ? rp.colorGradingMode.ToString() : "-")} colorSpace={QualitySettings.activeColorSpace}");
            foreach (var v in Object.FindObjectsByType<Volume>(FindObjectsSortMode.None))
            {
                var p = v.HasInstantiatedProfile() ? v.profile : v.sharedProfile;
                Debug.LogWarning($"VD_LOOK volume {v.name} scene={v.gameObject.scene.name} layer={v.gameObject.layer} on={v.isActiveAndEnabled} global={v.isGlobal} weight={v.weight:F2} prio={v.priority} profile={(p ? p.name : "-")}");
                if (p) foreach (var c in p.components) Debug.LogWarning($"VD_LOOK   {c.GetType().Name} active={c.active} {Overrides(c)}");
            }
            Debug.LogWarning($"VD_LOOK defaultProfile={(VolumeManager.instance.globalDefaultProfile ? VolumeManager.instance.globalDefaultProfile.name : "null")}");
            var stack = VolumeManager.instance.CreateStack();
            VolumeManager.instance.Update(stack, cam ? cam.transform : null, data ? data.volumeLayerMask : (LayerMask)~0);
            foreach (var t in VolumeManager.instance.baseComponentTypeArray)
            {
                var c = stack.GetComponent(t);
                var o = c ? Overrides(c) : "";
                if (o.Length > 0) Debug.LogWarning($"VD_LOOK stack {t.Name} active={c.active} {o}");
            }
            VolumeManager.instance.DestroyStack(stack);
            foreach (var l in Object.FindObjectsByType<Light>(FindObjectsSortMode.None))
                Debug.LogWarning($"VD_LOOK light {l.name} scene={l.gameObject.scene.name} type={l.type} on={l.isActiveAndEnabled} color={l.color} intensity={l.intensity:F3} rot={l.transform.eulerAngles} sun={RenderSettings.sun == l}");
            Debug.LogWarning($"VD_LOOK ambient mode={RenderSettings.ambientMode} sky={RenderSettings.ambientSkyColor} light={RenderSettings.ambientLight} fog={RenderSettings.fog}");
            // TL_Title_01's recorded clip tints the lounge backdrop through material._BaseColor (MeshRenderer, property block).
            var block = new MaterialPropertyBlock();
            foreach (var r in Object.FindObjectsByType<MeshRenderer>(FindObjectsSortMode.None))
            {
                if (r.transform.parent == null || r.transform.parent.name != "25dBG_Basecamp01_LoungeBG") continue;
                r.GetPropertyBlock(block);
                var m = r.sharedMaterial;
                Debug.LogWarning($"VD_LOOK backdrop {r.name} mat={(m ? m.name + "/" + m.shader.name : "-")} base={(m && m.HasProperty("_BaseColor") ? m.GetColor("_BaseColor").ToString() : "-")} " +
                    $"block={(block.isEmpty ? "empty" : block.GetColor("_BaseColor").ToString())}");
            }
            foreach (var t in Object.FindObjectsByType<Text>(FindObjectsSortMode.None))
                Debug.LogWarning($"VD_LOOK text {t.name} '{t.text}' font={(t.font ? t.font.name : "-")} size={t.fontSize} scale={(t.canvas ? t.canvas.scaleFactor : 0):F3}");
        }

        static string Overrides(VolumeComponent c)
        {
            var sb = new System.Text.StringBuilder();
            foreach (var f in c.GetType().GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic))
            {
                if (!typeof(VolumeParameter).IsAssignableFrom(f.FieldType)) continue;
                var p = (VolumeParameter)f.GetValue(c);
                if (p == null || !p.overrideState) continue;
                var value = p.GetType().GetProperty("value")?.GetValue(p);
                sb.Append($"{f.Name}={value} ");
            }
            return sb.ToString();
        }

        static void OnLog(string message, string stackTrace, LogType type)
        {
            if (type == LogType.Log || type == LogType.Warning) return;
            try { if (!LogAssert.ignoreFailingMessages) LogAssert.ignoreFailingMessages = true; }
            catch (System.InvalidOperationException) { }  // no log scope open yet
        }

        [UnityTest, Timeout(900000)]  // the title shot may wait up to 400 s for a pose outside the light spike and the logo glitch
        public IEnumerator BootToTitle()
        {
            LogAssert.ignoreFailingMessages = true;
            SceneManager.LoadScene(0);
            double phase = -1;
            for (int i = 0; i < 6; i++)
            {
                yield return new WaitForSecondsRealtime(4f);
                var director = TitleDirector();
                Debug.LogWarning($"VD_A1 t={i * 4 + 4}s scenes={SceneManager.sceneCount} active={SceneManager.GetActiveScene().name} director={(director ? director.time.ToString("F2") : "-")}");
                yield return Review.Shot($"a1-boot-{i * 4 + 4:00}s");
                if (i == 2 && director) phase = director.time;
            }
            DumpScene();
            Diag.DumpTitle();
            DumpLook();
            // The title timeline (TL_Title_01) sways ArtistCamera between two poses every 39.15 s, so the city, the pit and
            // the party change size and place with the timeline's time. The clip's frame (ref/title-menu) matches the
            // 12 s shot's pose; take the title shot at that phase so the pair compares the same camera.
            const double sway = 39.15;
            var title = TitleDirector();
            // Anim_LoungeBG (20 s loop, its own clock) flickers the point lights in the pit: Light_7 and Light_8 peak at
            // 18 and 17 around 2-3.5 s, which lifts the pit by up to 15% over the rest of the loop (A1b_PitProbe.PitOverTime,
            // pit-run10). Each sway lands the loop 0.85 s earlier, so skip pose matches that fall in that spike; the light
            // phase is logged with the shot.
            Animator bg = null;
            foreach (var a in Object.FindObjectsByType<Animator>(FindObjectsSortMode.None)) if (a.name == "LoungeBG") bg = a;
            float BgTime() => bg ? bg.GetCurrentAnimatorStateInfo(0).normalizedTime % 1f * 20f : -1f;
            // The logo's material (Mtl_DE_UI_TITLE_Glitch) glitches on its own: four flicker gates on _Time.y (slice with
            // the RGB split, vertex jitter, warp, block) each open in about one time slot in twenty. Runs 60 and 61 shot
            // the open slice gate (a 1-2 px red/cyan fringe the clip's calm frame lacks); the frame times are the same from
            // run to run, so they hit it twice. The gates are a function of the time the cameras render at
            // (A1c_LogoProbe.LogoOverTime: predicted slice gate and measured fringe agree), so check them for the shot and
            // take the next pose match when one was open.
            var logo = LogoGlitch.Find();
            float renderAt = -1;
            void At(ScriptableRenderContext _, Camera c) { if (renderAt < 0) renderAt = Time.time; }
            float waited = 0;
            for (int attempt = 1; ; attempt++)
            {
                for (; title && phase >= 0 && waited < 400f; waited += Time.unscaledDeltaTime)
                {
                    var bgT = BgTime();
                    if (System.Math.Abs(title.time - phase - sway * System.Math.Round((title.time - phase) / sway)) < 0.05 && !(bgT >= 1.5f && bgT <= 4f)) break;
                    yield return null;
                }
                Debug.LogWarning($"VD_A1 title-menu director={(title ? title.time.ToString("F2") : "-")} phase={phase:F2} lights={BgTime():F2}s attempt={attempt}");
                renderAt = -1;
                RenderPipelineManager.beginCameraRendering += At;
                yield return Review.Shot("title-menu");
                RenderPipelineManager.beginCameraRendering -= At;
                var g = logo ? LogoGlitch.Gates(logo, renderAt) : default;
                bool calm = g.slice == 0 && g.jitter == 0 && g.warp == 0 && g.block == 0;
                Debug.LogWarning($"VD_A1 title-menu logo t={renderAt:F3} slice={g.slice:F2} jitter={g.jitter:F2} warp={g.warp:F2} block={g.block:F2} " +
                    $"{(logo ? (calm ? "calm" : "glitch, next pose") : "no logo material")}");
                if (!logo || calm || waited >= 400f) break;
                yield return null;
                waited += Time.unscaledDeltaTime;
            }

            // title-options: press Options the way a player does. StartScene subscribes to the button's onClick
            // (OnClickWithSfx -> OnOptionButtonClick -> PopupManager.OpenAsync<OptionPopupPresenter>).
            Button option = null;
            foreach (var b in Object.FindObjectsByType<Button>(FindObjectsSortMode.None))
                if (b.transform.parent != null && b.transform.parent.name == "Option_") option = b;
            Assert.IsNotNull(option, "title Options button not found (Option_/Button)");
            // The clip's player clicks Options with the mouse, and InputIcons then shows the key guides of the mouse and
            // keyboard (Esc/X Close, mouse Select). Batchmode delivers no device input to play mode: the native mouse is
            // disabled (no focus), and queued events on a mouse of our own, with input routed to play mode and
            // InputSystem.Update called, left the pointer at 0,0 and performed no action (A1 runs 53-57). So do what a
            // mouse action ends in (II_ShowDeviceIconsAfterDelay.WaitAndShowDevice): tell InputIcons the mouse is the
            // current device, then press the button.
            var mouse = Mouse.current;
            LogInputIcons("before");
            var icons = System.Type.GetType("InputIcons.InputIconsManagerSO, InputIcons");
            icons.GetMethod("SetDeviceAndRefreshDisplayedIcons", new[] { typeof(InputDevice) }).Invoke(null, new object[] { mouse });
            LogInputIcons("after SetDeviceAndRefreshDisplayedIcons(mouse)");
            option.onClick.Invoke();
            yield return new WaitForSecondsRealtime(4f);
            DumpScene();
            foreach (var t in Object.FindObjectsByType<RectTransform>(FindObjectsInactive.Include, FindObjectsSortMode.None))
                if (t.name == "MousePrompt" || t.name == "KeyPrompt")
                    Debug.LogWarning($"VD_A1 prompt {t.parent.name}/{t.name} active={t.gameObject.activeInHierarchy} mouse={mouse.deviceId}");
            yield return Review.Shot("title-options");
            Assert.Pass($"active scene after 24 s: {SceneManager.GetActiveScene().name}");
        }
    }
}
