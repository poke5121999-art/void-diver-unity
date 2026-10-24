using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace VD.PlayTests
{
    // A4b: HUD probe. Plays A4 (title -> tutorial -> lounge) while a watcher logs VD_HUD lines for the HUD pieces that drew
    // wrong: the skill and quick-slot icon Images (sprite, texture, material, shader), the chat log lines (TMP colour,
    // font, material and the shader that draws them) and the campaign-complete card (CanvasGroup alpha), and takes
    // captures/review/hud-*.png shots of them.
    // Run with an extra "-vdFlowFor A4_TutorialFlow" argument: Flow.PrepareFlowRun (fresh profile, Local platform) only
    // looks for the A4 test name on the command line.
    public class A4b_HudProbe
    {
        [TearDown]
        public void Restore()
        {
            Flow.RestoreInput();
            Flow.RestorePlatform();
        }

        [UnityTest, Timeout(2400000)]
        public IEnumerator ProbeHud()
        {
            var go = new GameObject("VD_HudWatcher");
            Object.DontDestroyOnLoad(go);
            go.AddComponent<HudWatcher>();
            yield return new A4_TutorialFlow().NewGameToTutorial();
        }

        class HudWatcher : MonoBehaviour
        {
            readonly HashSet<string> _seen = new HashSet<string>();
            float _next;
            float _cardSince = -1f;
            int _cardShots;
            int _chatShots;

            static void Log(string s) => Debug.LogWarning($"VD_HUD t={Time.realtimeSinceStartup:F1} scene={Flow.ActiveScene} {s}");

            static string Describe(Graphic g)
            {
                if (g == null) return "null";
                var img = g as Image;
                var m = g.materialForRendering;
                string sprite = img == null ? "-" : img.sprite == null ? "NULL" : $"{img.sprite.name} tex={(img.sprite.texture ? img.sprite.texture.name : "null")} packed={img.sprite.packed}";
                return $"{g.name} active={g.isActiveAndEnabled} col={g.color} sprite={sprite} mat={(g.material ? g.material.name : "null")} " +
                       $"render={(m ? m.name + "/" + m.shader.name + " sup=" + m.shader.isSupported : "null")}";
            }

            void Update()
            {
                if (Time.realtimeSinceStartup < _next) return;
                _next = Time.realtimeSinceStartup + 1f;
                string scene = Flow.ActiveScene;

                foreach (var t in new[] { "InGameSkillSlotView", "InGameQuickSlotView", "InGameWeaponSlotView" })
                {
                    var views = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).Where(v => v != null && v.GetType().Name == t && v.isActiveAndEnabled).ToList();
                    for (int i = 0; i < views.Count; i++)
                    {
                        var icon = Flow.Get(views[i], "Icon") as Graphic;
                        string d = Describe(icon);
                        if (_seen.Add($"{scene}|{t}|{views[i].name}|{d}"))
                            Log($"{t}[{i}] {views[i].transform.parent?.name}/{views[i].name} icon: {d}");
                    }
                }

                var chats = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).Where(v => v != null && v.GetType().Name == "ChatSlotView" && v.isActiveAndEnabled).ToList();
                foreach (var c in chats)
                {
                    var tmp = Flow.Get(c, "Text") as TextMeshProUGUI;
                    if (tmp == null || string.IsNullOrEmpty(tmp.text)) continue;
                    var m = tmp.materialForRendering;
                    string d = $"'{tmp.text.Substring(0, Mathf.Min(40, tmp.text.Length))}' col={tmp.color} vcol={tmp.canvasRenderer.GetColor()} alpha={tmp.canvasRenderer.GetInheritedAlpha():F2} font={(tmp.font ? tmp.font.name : "null")} " +
                               $"shared={(tmp.fontSharedMaterial ? tmp.fontSharedMaterial.name + "/" + tmp.fontSharedMaterial.shader.name : "null")} " +
                               $"render={(m ? m.name + "/" + m.shader.name + " sup=" + m.shader.isSupported + " face=" + (m.HasProperty("_FaceColor") ? m.GetColor("_FaceColor").ToString() : "-") : "null")} " +
                               $"comps=[{string.Join(",", c.GetComponentsInParent<MonoBehaviour>(true).Select(x => x == null ? "MISSING" : x.GetType().Name).Distinct())}]";
                    if (_seen.Add($"chat|{d}")) Log($"ChatSlot {d}");
                }
                if (chats.Count >= 2 && _chatShots == 0 && scene != "LoungeScene")
                {
                    _chatShots++;
                    StartCoroutine(Review.Shot("hud-chat"));
                }

                var card = Flow.Find("CampaignCompletePopupView");
                if (card != null)
                {
                    if (_cardSince < 0f) _cardSince = Time.realtimeSinceStartup;
                    var cg = card.GetComponent<CanvasGroup>();
                    var anim = card.GetComponent<Animator>();
                    var st = anim != null && anim.isActiveAndEnabled ? anim.GetCurrentAnimatorStateInfo(0) : default;
                    Log($"CampaignCard since={Time.realtimeSinceStartup - _cardSince:F1}s alpha={(cg ? cg.alpha : -1f):F2} anim={(anim ? anim.isActiveAndEnabled + " t=" + st.normalizedTime.ToString("F2") + " len=" + st.length.ToString("F2") : "none")}");
                    if (_cardShots == 0 && Time.realtimeSinceStartup - _cardSince > 2.0f)
                    {
                        _cardShots++;
                        StartCoroutine(Review.Shot("hud-campaign-complete"));
                    }
                }
                else _cardSince = -1f;
            }
        }
    }
}
