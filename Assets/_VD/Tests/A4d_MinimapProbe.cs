using System.Collections;
using System.Linq;
using System.Reflection;
using System.Text;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace VD.PlayTests
{
    // A4d: minimap probe for the tutorial stage. Plays like A4 into the first fight, then logs every Graphic under the
    // minimap views (VD_MINI lines: sprite, its texture, packed or not, rects, uv, mesh, material, shader, colour, size on
    // screen) and shoots the minimap with each player-marker part off in turn (captures/review/mini-*.png).
    public class A4d_MinimapProbe
    {
        [TearDown]
        public void Restore()
        {
            Flow.RestoreInput();
            Flow.RestorePlatform();
        }

        static IEnumerator A4(string method, params object[] args) =>
            (IEnumerator)typeof(A4_TutorialFlow).GetMethod(method, BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, args);

        static void Log(string s) => Debug.LogWarning("VD_MINI " + s);

        static string Path(Transform t, Transform root) => t == root || t.parent == null ? t.name : Path(t.parent, root) + "/" + t.name;

        static string V(Vector2 v) => $"({v.x:F2},{v.y:F2})";

        static string R(Rect r) => $"[{r.x:F1},{r.y:F1} {r.width:F1}x{r.height:F1}]";

        public static void Dump(string phase)
        {
#if UNITY_EDITOR
            Log($"{phase} packerMode={UnityEditor.EditorSettings.spritePackerMode}");
#endif
            foreach (var viewName in new[] { "InGameMiniMapView", "InGameMiniMapPanelView" })
            {
                var view = Flow.Find(viewName);
                if (view == null) { Log($"{phase} {viewName} not found"); continue; }
                Log($"{phase} {viewName} active={view.isActiveAndEnabled}");
                foreach (var g in view.GetComponentsInChildren<Graphic>(true))
                {
                    var sb = new StringBuilder();
                    sb.Append($"{phase} {viewName}:{Path(g.transform, view.transform)} {g.GetType().Name} on={g.isActiveAndEnabled}");
                    sb.Append($" color={g.color} size={V(g.rectTransform.rect.size)} scale={g.transform.lossyScale.x:F3}");
                    var corners = new Vector3[4];
                    g.rectTransform.GetWorldCorners(corners);
                    sb.Append($" world={V(corners[0])}-{V(corners[2])}");
                    var mat = g.materialForRendering;
                    sb.Append($" mat={(g.material ? g.material.name : "-")} render={(mat ? mat.name : "-")} shader={(mat ? mat.shader.name : "-")}");
                    sb.Append($" mainTex={(g.mainTexture ? g.mainTexture.name + " " + g.mainTexture.width + "x" + g.mainTexture.height : "-")}");
                    if (g is Image img)
                    {
                        var s = img.sprite;
                        sb.Append($" type={img.type} spriteMesh={img.useSpriteMesh} preserve={img.preserveAspect}");
                        if (s != null)
                        {
                            sb.Append($" sprite={s.name} tex={(s.texture ? s.texture.name + " " + s.texture.width + "x" + s.texture.height : "-")}");
                            sb.Append($" packed={s.packed} mode={(s.packed ? s.packingMode.ToString() : "-")} rect={R(s.rect)} texRect={R(s.textureRect)} texOff={V(s.textureRectOffset)}");
                            sb.Append($" pivot={V(s.pivot)} border={s.border} ppu={s.pixelsPerUnit}");
                            sb.Append($" verts={s.vertices.Length} uv={string.Join(" ", s.uv.Select(V))}");
                        }
                        else sb.Append(" sprite=-");
                    }
                    else if (g is RawImage raw)
                        sb.Append($" raw={(raw.texture ? raw.texture.name : "-")} uvRect={R(raw.uvRect)}");
                    Log(sb.ToString());
                }
            }
        }

        static IEnumerator ShotMarkerParts(string phase)
        {
            yield return Review.Shot($"mini-{phase}");
            var view = Flow.Find("InGameMiniMapView");
            if (view == null) yield break;
            foreach (var part in new[] { "Vision", "Icon" })
            {
                var gs = view.GetComponentsInChildren<Graphic>(false).Where(g => g.name == part && g.enabled).ToList();
                foreach (var g in gs) g.enabled = false;
                yield return null;
                yield return Review.Shot($"mini-{phase}-no{part}");
                foreach (var g in gs) g.enabled = true;
            }
        }

        [UnityTest, Timeout(2400000)]
        public IEnumerator ProbeMinimap()
        {
            LogAssert.ignoreFailingMessages = true;
            Flow.Tag = "VD_A4D";
            SceneManager.LoadScene(0);
            MonoBehaviour title = null;
            yield return Flow.WaitUntil(() => (title = Flow.Find("StartSceneView")) != null
                && (Flow.Get(title, "Buttons") as GameObject)?.activeInHierarchy == true
                && Flow.Ready(Flow.Button(title, "Play_Button")), 120f, "title menu");
            Assert.IsTrue(Flow.LastWaitOk, "title menu did not come up");
            yield return new WaitForSecondsRealtime(3f);
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
            yield return A4("AdvanceDialogs", (System.Func<bool>)(() => Flow.ActiveScene == "StageScene"), 900f, "prologue", null, null, null, null);
            Assert.AreEqual("StageScene", Flow.ActiveScene, "prologue did not lead to the tutorial stage");
            yield return Flow.WaitUntil(() => Flow.StageState() == "Playing", 240f, "stage Playing");
            yield return Flow.WaitUntil(() => Flow.Find("DialogPopupView") != null, 30f, "tutorial dialog");
            yield return new WaitForSecondsRealtime(2f);
            yield return A4("AdvanceDialogs", (System.Func<bool>)(() => Flow.Find("DialogPopupView") == null), 120f, "tutorial intro", null, null, null, null);
            yield return new WaitForSecondsRealtime(3f);
            Flow.RouteInputToGame();
            Dump("hud");
            yield return ShotMarkerParts("hud");

            yield return A4("Go", new Vector3(19.4f, 0f, 21.5f), 1.0f, 40f, "trigger 100091", 0f, false);
            yield return A4("Go", new Vector3(13f, 0f, 15f), 1.5f, 40f, "monster room", 5f, false);
            yield return A4("Fight", 12f, 20f, "first combat", "mini-first-combat", 0f);
            Dump("combat");
            yield return ShotMarkerParts("combat");
            Assert.Pass("minimap probe done");
        }
    }
}
