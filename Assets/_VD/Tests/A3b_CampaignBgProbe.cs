using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace VD.PlayTests
{
    // A3b: probe for the campaign-select backgrounds (Elara -> Story Campaign -> first chapter). Boots from the A3 fixture
    // saves, opens the campaign popup like A3 does (its private steps, by reflection), and in the list and detail layouts
    // logs every camera, playable director (time, tracks muted), RawImage and renderer of the background prefabs (VD_CBG
    // lines), saves each camera's RenderTexture (captures/review/cbg-*.png) and takes shots.
    public class A3b_CampaignBgProbe
    {
        const string Fixture = "tools/fixtures/saves-post-tutorial";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void PrepareSaves()
        {
            if (!Flow.RunFor("A3b_CampaignBgProbe")) return;
            Application.logMessageReceivedThreaded += (message, stack, type) =>
            {
                if (type == LogType.Log || type == LogType.Warning) return;
                try { if (!LogAssert.ignoreFailingMessages) LogAssert.ignoreFailingMessages = true; }
                catch (InvalidOperationException) { }
            };
            Flow.FreshProfile();
            string dir = System.IO.Path.Combine(Directory.GetParent(Application.dataPath).FullName, "SavesDir");
            Directory.CreateDirectory(dir);
            foreach (var f in Directory.GetFiles(Fixture, "*.sav"))
                File.Copy(f, System.IO.Path.Combine(dir, System.IO.Path.GetFileName(f)), true);
            Flow.UseLocalPlatform();
        }

        [TearDown]
        public void Restore()
        {
            Flow.RestoreInput();
            Flow.RestorePlatform();
        }

        static object A3(string method, params object[] args) =>
            typeof(A3_NpcFlow).GetMethod(method, BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, args);

        static void Log(string s) => Debug.LogWarning("VD_CBG " + s);

        static string Path(Transform t) => t.parent == null ? t.name : Path(t.parent) + "/" + t.name;

        public static void Dump(string phase)
        {
            foreach (var c in Object.FindObjectsByType<Camera>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            {
                var d = c.GetComponent<UniversalAdditionalCameraData>();
                var rt = c.targetTexture;
                Log($"{phase} cam {Path(c.transform)} on={c.isActiveAndEnabled} rt={(rt ? $"{rt.name} {rt.width}x{rt.height} {rt.format} depth={rt.depth}" : "-")} mask={c.cullingMask:X} " +
                    $"pos={c.transform.position} rot={c.transform.eulerAngles} fov={c.fieldOfView:F1} ortho={c.orthographic} near={c.nearClipPlane} far={c.farClipPlane} " +
                    $"clear={c.clearFlags} bg={c.backgroundColor} depth={c.depth} urp={(d ? $"{d.renderType} post={d.renderPostProcessing} stack={d.cameraStack?.Count}" : "-")}");
            }
            foreach (var pd in Object.FindObjectsByType<PlayableDirector>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            {
                var tracks = new StringBuilder();
                if (pd.playableAsset != null)
                    foreach (var o in pd.playableAsset.outputs)
                    {
                        var t = o.sourceObject;
                        if (t == null) continue;
                        bool muted = t.GetType().GetProperty("muted")?.GetValue(t) is bool m && m;
                        var b = pd.GetGenericBinding(t);
                        tracks.Append($" [{t.GetType().Name}:{t.name}{(muted ? " MUTED" : "")} -> {(b ? b.name : "-")}]");
                    }
                Log($"{phase} director {Path(pd.transform)} on={pd.isActiveAndEnabled} asset={(pd.playableAsset ? pd.playableAsset.name : "-")} time={pd.time:F2}/{pd.duration:F2} state={pd.state} graph={pd.playableGraph.IsValid()}{tracks}");
            }
            foreach (var ri in Object.FindObjectsByType<RawImage>(FindObjectsInactive.Include, FindObjectsSortMode.None))
                if (Path(ri.transform).Contains("Campaign"))
                    Log($"{phase} raw {Path(ri.transform)} on={ri.isActiveAndEnabled} tex={(ri.texture ? ri.texture.name + " " + ri.texture.width + "x" + ri.texture.height : "-")} color={ri.color} mat={(ri.materialForRendering ? ri.materialForRendering.shader.name : "-")} size={ri.rectTransform.rect.size}");
            foreach (var r in Object.FindObjectsByType<Renderer>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            {
                string p = Path(r.transform);
                if (!p.Contains("Campaign")) continue;
                var mats = string.Join(";", r.sharedMaterials.Select(m => m ? $"{m.name}<{m.shader.name}{(m.shader.isSupported ? "" : " UNSUPPORTED")}>" : "null"));
                Log($"{phase} rend {p} {r.GetType().Name} on={r.enabled && r.gameObject.activeInHierarchy} layer={LayerMask.LayerToName(r.gameObject.layer)} bounds={r.bounds.center}/{r.bounds.size} scale={r.transform.lossyScale} mats={mats}");
            }
        }

        static void SaveRTs(string phase)
        {
            foreach (var c in Object.FindObjectsByType<Camera>(FindObjectsSortMode.None))
            {
                var rt = c.targetTexture;
                if (!rt || !c.isActiveAndEnabled) continue;
                var prev = RenderTexture.active;
                RenderTexture.active = rt;
                var tex = new Texture2D(rt.width, rt.height, TextureFormat.RGBA32, false);
                tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
                tex.Apply();
                RenderTexture.active = prev;
                string file = $"captures/review/cbg-{phase}-{c.name.Replace(' ', '_')}.png";
                File.WriteAllBytes(file, tex.EncodeToPNG());
                Object.Destroy(tex);
                Log($"{phase} saved {file}");
            }
        }

        [UnityTest, Timeout(1200000)]
        public IEnumerator ProbeCampaignBackground()
        {
            LogAssert.ignoreFailingMessages = true;
            Flow.Tag = "VD_A3B";
            SceneManager.LoadScene(0);
            MonoBehaviour title = null;
            yield return Flow.WaitUntil(() => (title = Flow.Find("StartSceneView")) != null
                && (Flow.Get(title, "Buttons") as GameObject)?.activeInHierarchy == true
                && Flow.Ready(Flow.Button(title, "Play_Button")), 120f, "title menu");
            Assert.IsTrue(Flow.LastWaitOk, "title menu did not come up");
            yield return Flow.Click(title, "Play_Button");
            float start = Time.realtimeSinceStartup;
            while (Flow.ActiveScene != "LoungeScene" && Time.realtimeSinceStartup - start < 180f)
            {
                foreach (var (view, button) in new[] { ("LocalRoomCreationPopupView", "BtnConfirm_Button"), ("RoomDifficultyPopupView", "Confirm_Button"), ("TextPopupView", "Confirm_Button") })
                    if (Flow.Find(view) != null && Flow.Ready(Flow.Button(Flow.Find(view), button))) yield return Flow.Click(Flow.Find(view), button);
                yield return new WaitForSecondsRealtime(0.5f);
            }
            Assert.AreEqual("LoungeScene", Flow.ActiveScene, "Continue did not reach the lounge");
            yield return Flow.WaitUntil(() => Flow.StageState() == "Playing", 240f, "lounge Playing");
            yield return new WaitForSecondsRealtime(2f);
            if (Flow.Find("DialogPopupView") != null)
                yield return (IEnumerator)A3("AdvanceDialogs", (Func<bool>)(() => Flow.Find("DialogPopupView") == null), 120f, "arrival dialog");
            Flow.RouteInputToGame();

            // Lounge quest 80200: Elara plays her quest line instead of her menu until Narcis, Felix and Lucas are talked to.
            foreach (int id in new[] { 700000, 700013, 700008, 700001 })
            {
                var other = (Component)A3("FindNpc", id);
                if (other == null) continue;
                yield return (IEnumerator)A3("Interact", other, $"npc {id}");
                yield return Flow.WaitUntil(() => Flow.Find("DialogPopupView") != null || Flow.Find("SelectionPopupView") != null, 4f, $"npc {id} reaction");
                if (Flow.Find("DialogPopupView") != null)
                    yield return (IEnumerator)A3("AdvanceDialogs", (Func<bool>)(() => Flow.Find("DialogPopupView") == null), 120f, $"npc {id} dialog");
                for (int i = 0; i < 3 && Flow.Find("SelectionPopupView") != null; i++)
                {
                    yield return Flow.Keys(0.2f, UnityEngine.InputSystem.Key.Escape);
                    yield return new WaitForSecondsRealtime(0.5f);
                }
                yield return new WaitForSecondsRealtime(1f);
            }

            var npc = (Component)A3("FindNpc", 700000);
            Assert.IsNotNull(npc, "Elara not in the lounge");
            int functionId = ((int[])A3("FunctionIds", npc)).FirstOrDefault(id => (string)A3("FunctionType", id) == "Campaign");
            for (int attempt = 0; attempt < 6 && Flow.Find("CampaignPopupPresenter") == null; attempt++)
            {
                yield return (IEnumerator)A3("Interact", npc, "Elara");
                yield return Flow.WaitUntil(() => Flow.Find("SelectionPopupView") != null || Flow.Find("DialogPopupView") != null || Flow.Find("CampaignPopupPresenter") != null, 4f, "Elara reaction");
                if (Flow.Find("DialogPopupView") != null)
                {
                    yield return (IEnumerator)A3("AdvanceDialogs", (Func<bool>)(() => Flow.Find("DialogPopupView") == null), 120f, "Elara dialog");
                    yield return new WaitForSecondsRealtime(1f);
                    continue;
                }
                if (Flow.Find("SelectionPopupView") != null)
                {
                    yield return new WaitForSecondsRealtime(0.8f);
                    yield return (IEnumerator)A3("Select", functionId, npc, "cbg");
                }
                yield return Flow.WaitUntil(() => Flow.Find("CampaignPopupPresenter") != null, 8f, "CampaignPopupPresenter");
            }
            Assert.IsNotNull(Flow.Find("CampaignPopupPresenter"), "campaign popup did not open");
            yield return new WaitForSecondsRealtime(2f);
            Dump("list");
            SaveRTs("list");
            yield return Review.Shot("cbg-list");

            yield return (IEnumerator)A3("ClickSlot", "CampaignSlotPresenter", "cbg");
            yield return Flow.WaitUntil(() => Flow.Find("CampaignDetailPanelPresenter") != null, 5f, "detail panel");
            yield return new WaitForSecondsRealtime(0.3f);
            Dump("detail0");
            yield return new WaitForSecondsRealtime(1.5f);
            Dump("detail");
            SaveRTs("detail");
            yield return Review.Shot("cbg-detail");
            Assert.Pass("campaign background probe done");
        }
    }
}
