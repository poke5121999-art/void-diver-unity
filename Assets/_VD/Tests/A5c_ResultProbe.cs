using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace VD.PlayTests
{
    // A5c: Dive Results probe. Continues to the lounge from the post-tutorial fixture, hands the GameManager a death result
    // like A5's (Paradox, four Lost Items, kill/box EXP) and opens the popup the way LoungeScene does, at 1280x720 like
    // the shots. Logs the scroll state every 0.25 s (VD_A5C lines: content vs viewport height, normalized position,
    // Confirm/Skip) and shoots it after 4 s (captures/review/a5c-result-settled.png), so the reveal can be timed without a
    // full dive. At batchmode's 640x480 the content fits and never scrolls (docs/DECODING.md trap 46).
    public class A5c_ResultProbe
    {
        const string Fixture = "tools/fixtures/saves-post-tutorial";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void PrepareSaves()
        {
            if (!Flow.RunFor("A5c_ResultProbe")) return;
            Application.logMessageReceivedThreaded -= IgnoreBootErrors;
            Application.logMessageReceivedThreaded += IgnoreBootErrors;
            Flow.Tag = "VD_A5C";
            Flow.SeedSaves(Fixture);
            Flow.UseLocalPlatform();
        }

        static void IgnoreBootErrors(string message, string stackTrace, LogType type)
        {
            if (type == LogType.Log || type == LogType.Warning) return;
            try { if (!LogAssert.ignoreFailingMessages) LogAssert.ignoreFailingMessages = true; }
            catch (InvalidOperationException) { }
        }

        [TearDown]
        public void Restore()
        {
            Flow.RestoreInput();
            Flow.RestorePlatform();
        }

        static Type T(string name) => AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType(name)).FirstOrDefault(t => t != null)
            ?? throw new InvalidOperationException("type not found: " + name);

        const BindingFlags All = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

        static void Set(object o, string member, object value)
        {
            var t = o.GetType();
            var f = t.GetField(member, All);
            if (f != null) { f.SetValue(o, value); return; }
            t.GetProperty(member, All).SetValue(o, value);
        }

        static Array ArrayOf(Type element, params object[] items)
        {
            var a = Array.CreateInstance(element, items.Length);
            for (int i = 0; i < items.Length; i++) a.SetValue(items[i], i);
            return a;
        }

        static int[] ParadoxIds()
        {
            var table = T("GameCommon.Table.TParadox").GetProperty("Table", All).GetValue(null);
            var rows = ((IEnumerable)table.GetType().GetProperty("Values", All).GetValue(table)).Cast<object>().ToArray();
            object pos = rows.FirstOrDefault(r => (bool)Flow.Get(r, "IsPositive")), neg = rows.FirstOrDefault(r => !(bool)Flow.Get(r, "IsPositive"));
            return new[] { pos, neg }.Where(r => r != null).Select(r => (int)Flow.Get(r, "Id")).ToArray();
        }

        static object MakeResult(MonoBehaviour gameManager)
        {
            ulong me = (ulong)Flow.Get(gameManager, "LocalClientId");
            var goodsType = T("GameCommon.Data.Goods");
            var make = T("GameCommon.Data.GoodsCreator").GetMethod("MakeItemGoods", All);
            object Item(int id, long n) => make.Invoke(null, new object[] { id, n, 0L });

            var player = Activator.CreateInstance(T("StageResultPlayerInfo"));
            Set(player, "ClientId", me);
            Set(player, "State", Enum.ToObject(T("GameCommon.Enum.EActionState"), 5));
            Set(player, "CharacterId", 100001);
            Set(player, "UserName", "Diver");
            Set(player, "IsLostItem", true);
            Set(player, "HighGradeGoods", ArrayOf(goodsType));
            Set(player, "LostGoods", ArrayOf(goodsType, Item(8061, 2), Item(8024, 1), Item(3002, 3), Item(8033, 3)));
            Set(player, "GainedParadoxIds", ParadoxIds());
            Set(player, "RemovedParadoxIds", new int[0]);
            Set(player, "InitialParadoxIds", new int[0]);

            var local = Activator.CreateInstance(T("StageResultLocalInfo"));
            Set(local, "ClientId", me);
            Set(local, "BaseExpFromRewardBox", 30L);
            Set(local, "BaseExpFromMonsterKill", 10L);

            var info = Activator.CreateInstance(T("StageResultInfo"));
            Set(info, "PlayerInfos", ArrayOf(T("StageResultPlayerInfo"), player));
            Set(info, "LocalInfos", ArrayOf(T("StageResultLocalInfo"), local));
            Set(info, "CampaignId", 1101);
            // Every quest slot reads its (progress, goal) from CampaignSubTasks; the dive fills one per task of the campaign.
            var tasks = T("GameCommon.Table.TCampaignTask").GetProperty("GroupTable", All).GetValue(null);
            var getItem = tasks.GetType().GetMethods(All).First(m => m.Name == "get_Item" && m.GetParameters().Length == 1 && m.GetParameters()[0].ParameterType == typeof(int));
            var rows = (Array)getItem.Invoke(tasks, new object[] { 1101 });
            var subTasks = (IDictionary)Flow.Get(info, "CampaignSubTasks");
            foreach (var row in rows) subTasks[(int)Flow.Get(row, "Id")] = (0L, 5L);
            return info;
        }

        static string ScrollState(MonoBehaviour view)
        {
            var scroll = Flow.Get(view, "ScrollView") as ScrollRect;
            if (scroll == null) return "scroll=null";
            var confirm = Flow.Get(view, "Confirm") as GameObject;
            var skip = Flow.Get(view, "SkipButton") as Button;
            return $"pos={scroll.verticalNormalizedPosition:F3} content={scroll.content.rect.height:F1} viewport={scroll.viewport.rect.height:F1} " +
                   $"enabled={scroll.enabled} confirm={(confirm != null && confirm.activeInHierarchy)} skip={(skip != null && skip.gameObject.activeInHierarchy)} " +
                   $"auto={Flow.Get(Flow.Find("StageResultPopupPresenter"), "_isAutoScrolling")} dt={Time.unscaledDeltaTime:F3} " +
                   $"kill={ExpText(view, "ExpMonsterKill_Value")} box={ExpText(view, "ExpRewardBox_Value")}";
        }

        static string ExpText(MonoBehaviour view, string member)
        {
            var rows = Flow.Get(view, "Exp_List") as GameObject[];
            var slot = rows != null && rows.Length > 0 && rows[0] != null ? rows[0].GetComponent("StageResultPlayerExpSlotView") : null;
            return slot == null ? "-" : (Flow.Get(Flow.Get(slot, member), "text") as string ?? "-");
        }

        [UnityTest, Timeout(900000)]
        public IEnumerator ResultReveal()
        {
            LogAssert.ignoreFailingMessages = true;
            Flow.Tag = "VD_A5C";
            yield return Flow.ContinueToLounge();
            Assert.AreEqual("LoungeScene", Flow.ActiveScene, "Continue did not reach the lounge");
            // Batchmode's screen is 640x480: at 4:3 the canvas is taller than the shots' 16:9 and the result content fits
            // without scrolling. Ask for the shots' 1280x720 so the popup lays out as it does on a 16:9 screen.
#if UNITY_EDITOR
            try { UnityEditor.PlayModeWindow.SetCustomRenderingResolution(1280, 720, "VD 1280x720"); }
            catch (Exception e) { Flow.Log("resolution: " + e.Message); }
#endif
            yield return new WaitForSecondsRealtime(2f);
            Flow.Log($"screen={Screen.width}x{Screen.height}");

            var gameManager = Flow.Find("GameManager");
            Set(gameManager, "StageResultInfo", MakeResult(gameManager));
            var lounge = Flow.Find("LoungeScene");
            lounge.GetType().GetMethod("OpenStageResultPopupAsync", All).Invoke(lounge, null);
            yield return Flow.WaitUntil(() => Flow.Find("StageResultPopupView") != null, 30f, "Dive Results");
            var view = Flow.Find("StageResultPopupView");
            Assert.IsNotNull(view, "Dive Results did not open");
            float opened = Time.realtimeSinceStartup;
            Flow.Log($"opened frame={Time.frameCount} {ScrollState(view)}");
            // Log first (a shot stalls the frame loop for seconds), then shoot mid-reveal and settled.
            while (Time.realtimeSinceStartup - opened < 4f && Flow.Find("StageResultPopupView") != null)
            {
                Flow.Log($"t+{Time.realtimeSinceStartup - opened:F2} frame={Time.frameCount} {ScrollState(view)}");
                yield return new WaitForSecondsRealtime(0.25f);
            }
            yield return Review.Shot("a5c-result-settled");
            Flow.Log($"end {ScrollState(view)}");
        }
    }
}
