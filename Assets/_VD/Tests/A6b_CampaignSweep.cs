using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace VD.PlayTests
{
    // A6b: every campaign the demo can reach (docs/review/a6/campaigns.tsv, reachable_in_demo = yes) loads, plays and ends
    // cleanly once. One case per campaign id (Campaign(<id>)); cases share one play session, so a run filtered to several
    // ids (-testFilter 'A6b_CampaignSweep.Campaign\((101|102|103)\)') walks them one after another from the lounge.
    // Per campaign:
    // - Start it with the cheat menu's own call (SROptions.StartCampaign: GameManager.StartCampaign(id, Normal,
    //   GameCampaignManager.GetCampaignAnomalyType(id))). Normal campaigns are otherwise a random pick in Elara's menu
    //   (GetPlayableCampaignIds), so a given id cannot be chosen like a player. Story campaigns get Elara's briefing.
    // - Dive through the booth, take one shot of the map and HUD (a6b-<id>), explore ExploreBudget seconds over NavMesh
    //   waypoints, fight what comes near, and shoot the roaming patrol boss (TCampaign.PatrolMonsterId: Dark Young
    //   301010, Hatman 300022) once it is in view (a6b-<id>-patrol).
    // - Leave by the game's own extraction: an activated SafeExit (hold F), else the nearest WaveExit (activate, hold out
    //   in its field). Only when neither ends the dive in time, the cheat menu's escape (ReqForceEscape) does; the case
    //   logs "forced escape" and the TSV says so.
    // - Dive Results, Confirm; if the campaign is still open (its tasks not done), give it up the player's way with the
    //   abandon route (Elara's Campaign function: "abandon?" Confirm, GameManager.CloseCampaign(GiveUp)) so the next case
    //   starts from a Ready room.
    // The diver is level 7 (ReqCheatLevelUp, the demo's LevelMax) and invincible in the stage (cheat menu option
    // ReqCheatApplyCharacterOptions): the sweep is about loading and ending each map, not about surviving it.
    // A case fails on any Error/Exception/Assert the game logs while it runs, on a load failure logged as a warning
    // (missing Addressables key, resource or prefab), or on a step that does not finish in time.
    public class A6b_CampaignSweep
    {
        const string Fixture = "tools/fixtures/saves-post-tutorial";
        const float ExploreBudget = 75f;

        public static readonly int[] Ids =
        {
            101, 102, 103, 104, 105, 106, 107, 108, 111, 112, 113, 114, 115, 116, 117, 118,
            201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 1104, 1201, 1202,
        };

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void PrepareSaves()
        {
            if (!Flow.RunFor("A6b_CampaignSweep")) return;
            Application.logMessageReceivedThreaded -= IgnoreBootErrors;
            Application.logMessageReceivedThreaded += IgnoreBootErrors;
            Flow.Tag = "VD_A6B";
            Flow.SeedSaves(Fixture);
            Flow.UseLocalPlatform();
        }

        static void IgnoreBootErrors(string message, string stackTrace, LogType type)
        {
            if (type == LogType.Log || type == LogType.Warning) return;
            try { if (!LogAssert.ignoreFailingMessages) LogAssert.ignoreFailingMessages = true; }
            catch (InvalidOperationException) { }
        }

        [OneTimeTearDown]
        public void Restore()
        {
            Flow.RestoreInput();
            Flow.RestorePlatform();
        }

        // ---- the session (shared by the cases) ----

        static bool s_session;
        static readonly Dictionary<int, string> s_results = new Dictionary<int, string>();

        static IEnumerator Session()
        {
            if (s_session && Flow.ActiveScene == "LoungeScene") yield break;
            LogAssert.ignoreFailingMessages = true;
            Flow.Tag = "VD_A6B";
            yield return Flow.ContinueToLounge();
            Assert.AreEqual("LoungeScene", Flow.ActiveScene, "Continue did not reach the lounge");
            Flow.RouteInputToGame();
            // Lounge quest 80200 first, as A5/A6 do: Elara's quest lines take her F until it is done.
            yield return Talk(700000, "Elara 80200 start");
            foreach (var (id, who) in new[] { (700013, "Narcis"), (700008, "Felix"), (700001, "Lucas") })
                yield return Talk(id, $"{who} 80200");
            yield return Talk(700000, "Elara 80200 finish");
            for (int i = 0; i < 8 && (Flow.Get(Flow.Find("PlayerManager"), "Level") is int lv ? lv : 99) < 7; i++)
            {
                SendCheat("ReqCheatLevelUp");
                yield return new WaitForSecondsRealtime(0.8f);
            }
            Flow.Log($"session: level={Flow.Get(Flow.Find("PlayerManager"), "Level")} {Flow.LoadState()}");
            yield return DrainDialogs("session");
            s_session = true;
        }

        [UnityTest, Timeout(1800000)]
        public IEnumerator Campaign([ValueSource(nameof(Ids))] int id)
        {
            LogAssert.ignoreFailingMessages = true;
            Flow.Tag = "VD_A6B";
            yield return Session();
            Flow.GameLog.Start();
            string stuck = null, escape = "-";
            bool patrolSeen = false;
            float t0 = Time.realtimeSinceStartup;
            try
            {
                yield return Run(id, s => stuck ??= s, e => escape = e, p => patrolSeen = p);
            }
            finally
            {
                Flow.GameLog.Stop();
            }
            var problems = Flow.GameLog.Problems();
            string verdict = stuck != null ? $"STUCK {stuck}" : problems.Count > 0 ? $"ERRORS {problems.Count}" : "PASS";
            s_results[id] = $"{verdict} escape={escape} patrol={(patrolSeen ? "seen" : "-")} {Time.realtimeSinceStartup - t0:F0}s";
            Flow.Log($"RESULT {id} {s_results[id]}");
            Flow.Log($"sweep so far: {string.Join(" | ", s_results.Select(kv => $"{kv.Key}={kv.Value}"))}");
            Assert.IsNull(stuck, $"campaign {id} stuck: {stuck}");
            Flow.GameLog.AssertClean($"campaign {id}");
        }

        static IEnumerator Run(int id, Action<string> fail, Action<string> escaped, Action<bool> patrol)
        {
            // A Ready room: the previous case's campaign closed.
            yield return CloseOpenCampaign("before");
            if (Room() != "Ready") { fail($"room {Room()} before start"); yield break; }
            if (id >= 1100) yield return ClearStoryBefore(id);

            var gm = Flow.Find("GameManager");
            var cm = Flow.Find("GameCampaignManager");
            var anomaly = cm.GetType().GetMethod("GetCampaignAnomalyType").Invoke(cm, new object[] { id });
            var difficulty = Enum.Parse(GameType("GameCommon.Enum.EDifficulty") ?? anomaly.GetType().Assembly.GetTypes().First(t => t.Name == "EDifficulty"), "Normal");
            Flow.Log($"start {id}: cheat GameManager.StartCampaign({id}, Normal, {anomaly}) patrol={PatrolId(id)}");
            gm.GetType().GetMethod("StartCampaign").Invoke(gm, new[] { (object)id, difficulty, anomaly });
            yield return Flow.WaitUntil(() => CampaignNow() == id.ToString(), 10f, $"campaign {id} started");
            if (!Flow.LastWaitOk) { fail("campaign did not start"); yield break; }
            yield return Flow.WaitUntil(() => Flow.TopPopup() == null, 20f, "start card gone");
            yield return new WaitForSecondsRealtime(1f);

            // Story campaigns: Elara's briefing (Step_00001 TalkToNPC) before the booth opens. Normal ones need none.
            if (BoothCheck() != "Success") yield return Briefing();
            Flow.Log($"booth check {BoothCheck()}");
            yield return EnterBooth();
            if (Flow.ActiveScene != "StageScene" || Flow.StageState() != "Playing") { fail($"stage did not load ({Flow.ActiveScene}/{Flow.StageState()})"); yield break; }

            yield return new WaitForSecondsRealtime(3f);
            SendCheat("ReqCheatApplyCharacterOptions", ("Invincibility", true));
            yield return DrainDialogs("stage start");
            Flow.Log($"stage {id}: {MeState()} monsters={Flow.Monsters().Count()} {Flow.LoadState()}");
            yield return Review.Shot($"a6b-{id}");

            bool seen = false;
            yield return Explore(id, () => seen = true);
            patrol(seen);
            string how = null;
            yield return Escape(e => how = e);
            escaped(how ?? "none");
            if (how == null) { fail("no escape"); yield break; }
            yield return Results(id, fail);
        }

        // ---- lounge ----

        static string Room() => Flow.Value(Flow.Get(Flow.Find("GameManager"), "_roomContext"), "RxRoomState")?.ToString() ?? "-";
        static string CampaignNow() => Flow.Value(Flow.Get(Flow.Find("GameManager"), "_gameContext"), "RxCampaignId")?.ToString();

        static Type GameType(string fullName) =>
            AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType(fullName)).FirstOrDefault(t => t != null);

        static object CampaignRow(int id)
        {
            var table = GameType("GameCommon.Table.TCampaign")?.GetProperty("Table")?.GetValue(null);
            try { return table?.GetType().GetProperty("Item")?.GetValue(table, new object[] { id }); }
            catch (System.Reflection.TargetInvocationException) { return null; }
        }

        static int PatrolId(int id) => Flow.Get(CampaignRow(id), "PatrolMonsterCount") is int n && n > 0 && Flow.Get(CampaignRow(id), "PatrolMonsterId") is int m ? m : 0;

        static void SendCheat(string type, params (string prop, object value)[] props)
        {
            var request = Activator.CreateInstance(GameType(type));
            foreach (var (p, v) in props) request.GetType().GetProperty(p).SetValue(request, v);
            var channel = Flow.Get(Flow.Find("GameManager"), "_gameChannel");
            channel.GetType().GetMethod("SendToHost").Invoke(channel, new[] { request });
            Flow.Log($"cheat {type} {string.Join(" ", props.Select(x => $"{x.prop}={x.value}"))}");
        }

        static IEnumerator Talk(int npcId, string what)
        {
            var npc = Flow.FindNpc(npcId);
            if (npc == null) { Flow.Log($"talk {what}: npc {npcId} not here"); yield break; }
            yield return Flow.InteractNpc(npc, what);
            yield return Flow.WaitUntil(() => Flow.Find("DialogPopupView") != null || Flow.Find("SelectionPopupView") != null, 4f, $"{what} reaction");
            if (Flow.Find("DialogPopupView") != null)
                yield return Flow.AdvanceDialogs(() => Flow.Find("DialogPopupView") == null, 120f, what);
            else if (Flow.Find("SelectionPopupView") != null) yield return Flow.Keys(0.2f, Key.Escape);
            yield return new WaitForSecondsRealtime(1f);
        }

        static IEnumerator DrainDialogs(string what)
        {
            for (int i = 0; i < 4; i++)
            {
                if (Flow.Find("DialogPopupView") != null) yield return Flow.AdvanceDialogs(() => Flow.Find("DialogPopupView") == null, 90f, what);
                else if (Flow.TopPopup() != null) { Flow.Log($"{what}: popup {Flow.TopPopup()}, Esc"); yield return Flow.Keys(0.2f, Key.Escape); }
                else yield break;
                yield return new WaitForSecondsRealtime(0.8f);
            }
        }

        // Story campaign N's predecessors cleared, as A6 does (PlayerManager.SetClearedCampaign, SROptions.ClearAllCampaigns).
        static IEnumerator ClearStoryBefore(int id)
        {
            var pm = Flow.Find("PlayerManager");
            var story = new[] { 1101, 1102, 1103, 1104, 1105, 1201, 1202 };
            foreach (var c in story.Where(c => c < id))
                pm.GetType().GetMethod("SetClearedCampaign")?.Invoke(pm, new object[] { c });
            Flow.Log($"cheat SetClearedCampaign({string.Join(",", story.Where(c => c < id))})");
            yield return null;
        }

        static string BoothCheck()
        {
            var booth = Flow.Find("PhoneBooth");
            var gm = Flow.Find("GameManager");
            if (booth == null || gm == null) return "no booth";
            try { return booth.GetType().GetMethod("CheckCanEnter")?.Invoke(booth, new[] { Flow.Get(gm, "LocalClientId") })?.ToString() ?? "?"; }
            catch (Exception e) { return $"threw {e.InnerException?.GetType().Name}"; }
        }

        static IEnumerator Briefing()
        {
            yield return new WaitForSecondsRealtime(3.5f);
            for (int attempt = 0; attempt < 3 && BoothCheck() != "Success"; attempt++)
            {
                yield return Talk(700000, "Elara briefing");
                yield return Flow.WaitUntil(() => BoothCheck() == "Success", 5f, "booth open");
            }
        }

        static IEnumerator EnterBooth()
        {
            var booth = Flow.Find("PhoneBooth");
            if (booth == null) yield break;
            for (int attempt = 0; attempt < 3 && State() != "StageReady" && Flow.ActiveScene == "LoungeScene"; attempt++)
            {
                yield return Flow.InteractObject(booth, 0.8f, "dive booth");
                yield return Flow.WaitUntil(() => State() == "StageReady" || Flow.ActiveScene != "LoungeScene", 4f, "StageReady");
            }
            yield return Flow.WaitUntil(() => Flow.ActiveScene == "StageScene" && Flow.StageState() == "Playing", 240f, "stage Playing");
        }

        // ---- the stage ----

        static Component Me() => Flow.MyCharacter();
        static string State() => Flow.Get(Me(), "State")?.ToString() ?? "-";
        static bool Over() => State() == "Dead" || State() == "Escaped" || Flow.ActiveScene != "StageScene" || Flow.StageState() == "Ended";
        static object Model(Component m) => Flow.Get(m, "UnitModel");
        static int MonsterId(Component m) => Flow.Get(Model(m), "TableId") is int id ? id : 0;

        static string MeState()
        {
            var me = Me();
            return me == null ? "me=-" : $"hp={Flow.Hp(me)} state={State()} pos={me.transform.position}";
        }

        static Component Enemy(float range)
        {
            var me = Me();
            if (me == null) return null;
            return Flow.Monsters().Where(m => Flow.Get(Model(m), "State")?.ToString() is string s && s != "Dead" && s != "Dying")
                .Select(m => (m, d: Flow.Flat(m.transform.position - me.transform.position).magnitude))
                .Where(x => x.d < range).OrderBy(x => x.d).Select(x => x.m).FirstOrDefault();
        }

        static IEnumerator Fight(Component m, float seconds)
        {
            float start = Time.realtimeSinceStartup;
            int clicks = 0;
            while (m && Flow.Alive(m) && Time.realtimeSinceStartup - start < seconds && !Over())
            {
                if (Flow.Find("DialogPopupView") != null) { yield return DrainDialogs("dialog in a fight"); continue; }
                var me = Me();
                float d = Flow.Flat(m.transform.position - me.transform.position).magnitude;
                if (d > 12f) break;
                if (d > 1.8f) { yield return Flow.WalkTo(() => m && Flow.Alive(m) ? m.transform.position : (Vector3?)null, 1.5f, 2f, "approach"); continue; }
                Flow.Hold();
                Flow.Aim(m.transform.position + Vector3.up * 0.5f);
                var cam = Camera.main;
                var at = cam ? (Vector2)cam.WorldToScreenPoint(m.transform.position + Vector3.up * 0.5f) : Vector2.zero;
                yield return Flow.MouseClick(at, ++clicks % 4 == 0 ? MouseButton.Right : MouseButton.Left);
                yield return new WaitForSecondsRealtime(0.35f);
            }
        }

        static IEnumerator Explore(int id, Action seen)
        {
            var waypoints = new List<Vector3>();
            var tri = UnityEngine.AI.NavMesh.CalculateTriangulation();
            if (tri.vertices.Length > 0)
            {
                var b = new Bounds(tri.vertices[0], Vector3.zero);
                foreach (var v in tri.vertices) b.Encapsulate(v);
                for (float x = b.min.x + 6f; x < b.max.x; x += 12f)
                    for (float z = b.min.z + 6f; z < b.max.z; z += 12f)
                        if (UnityEngine.AI.NavMesh.SamplePosition(new Vector3(x, b.center.y, z), out var hit, 3f, UnityEngine.AI.NavMesh.AllAreas))
                            waypoints.Add(hit.position);
            }
            int patrolId = PatrolId(id);
            bool patrolShot = false;
            float start = Time.realtimeSinceStartup;
            int kills = 0;
            Flow.Log($"explore {id}: {waypoints.Count} waypoints, patrol {patrolId}");
            while (!Over() && Time.realtimeSinceStartup - start < ExploreBudget)
            {
                if (Flow.Find("DialogPopupView") != null || Flow.TopPopup() != null) { yield return DrainDialogs("dialog in the dive"); continue; }
                var me = Me();
                if (me == null) break;
                // The roaming patrol boss: shoot it once it is on screen near the diver, then trade a few blows.
                var boss = Flow.Monsters().FirstOrDefault(m => patrolId != 0 && MonsterId(m) == patrolId || Flow.Get(Model(m), "IsBoss") is bool ib && ib);
                if (boss && Flow.Flat(boss.transform.position - me.transform.position).magnitude < 10f && !patrolShot)
                {
                    patrolShot = true;
                    seen();
                    Flow.Log($"patrol boss {MonsterId(boss)} at {boss.transform.position} hp={Flow.Hp(boss)} {MeState()}");
                    Flow.Aim(boss.transform.position);
                    yield return new WaitForSecondsRealtime(0.5f);
                    yield return Review.Shot($"a6b-{id}-patrol");
                    yield return Fight(boss, 15f);
                    continue;
                }
                var m = Enemy(6f);
                if (m != null) { yield return Fight(m, 12f); if (!Flow.Alive(m)) kills++; continue; }
                if (waypoints.Count == 0) break;
                var p = me.transform.position;
                var wp = waypoints.OrderBy(w => Flow.Flat(w - p).sqrMagnitude).First();
                waypoints.Remove(wp);
                yield return Flow.Go(wp, 2f, 20f, "explore", run: true, stop: () => Enemy(5f) != null || Flow.Find("DialogPopupView") != null);
            }
            Flow.Log($"explore {id} end: kills={kills} patrolSeen={patrolShot} {MeState()}");
        }

        // How many stacks of a buff the diver carries (UnitModel buffs), for the escape field's 3113.
        static string BuffStack(int buffId)
        {
            var model = Me();
            try
            {
                var m = model?.GetType().GetMethods().FirstOrDefault(x => x.Name == "GetBuffStack" && x.GetParameters().Length == 1);
                if (m != null) return m.Invoke(model, new object[] { buffId })?.ToString();
                var has = model?.GetType().GetMethods().FirstOrDefault(x => x.Name == "HasBuff" && x.GetParameters().Length == 1);
                return has?.Invoke(model, new object[] { buffId })?.ToString() ?? "?";
            }
            catch (Exception e) { return e.GetType().Name; }
        }

        static IEnumerable<MonoBehaviour> Exits(string type) => Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
            .Where(m => m != null && m.isActiveAndEnabled && m.GetType().Name == type);

        // Flow.Go gives up after one 20 s leg without a closed door in the way; a far exit needs several legs (runs 2-5:
        // "blocked" on long paths in 103, 106, 114, 115, 117, 201, 1202). Keep going while the distance shrinks.
        static IEnumerator GoFar(Vector3 goal, float arrive, string what)
        {
            float start = Time.realtimeSinceStartup, best = float.MaxValue;
            int noProgress = 0;
            while (Time.realtimeSinceStartup - start < 150f && Me() != null && !Over())
            {
                float d = Flow.Flat(goal - Me().transform.position).magnitude;
                if (d <= arrive) break;
                if (d < best - 1f) { best = d; noProgress = 0; }
                else if (++noProgress >= 3) { Flow.Log($"go far {what}: no progress at d={d:F1}"); break; }
                yield return Flow.Go(goal, arrive, 40f, what, run: true);
                if (Flow.Find("DialogPopupView") != null || Flow.TopPopup() != null) yield return DrainDialogs($"dialog on the way to {what}");
                if (noProgress > 0)
                {
                    // Off a corner the 8 keyboard directions cannot follow (trap 37): a short step aside.
                    yield return Flow.Keys(0.5f, noProgress % 2 == 1 ? Key.A : Key.D);
                }
            }
        }

        static IEnumerator HoldAt(Component exit, float seconds, Func<bool> done)
        {
            yield return GoFar(exit.transform.position, 0.9f, exit.GetType().Name);
            Flow.Aim(exit.transform.position);
            yield return Flow.WaitUntil(() => Flow.Focused(exit), 3f, "focus exit");
            Flow.Hold(Key.F);
            float t = Time.realtimeSinceStartup;
            while (Time.realtimeSinceStartup - t < seconds && !done()) yield return null;
            Flow.Hold();
        }

        // The game's own extraction first; the cheat menu's escape only when it does not end the dive.
        static IEnumerator Escape(Action<string> how)
        {
            if (Over()) { how("over"); yield break; }
            bool Gone() => Over() || Flow.Find("EscapeSuccessPopupView") != null;
            var me = Me();
            var safe = Exits("SafeExit").Where(e => Flow.Get(e, "ExitState")?.ToString() == "Activated")
                .OrderBy(e => Flow.Flat(e.transform.position - me.transform.position).sqrMagnitude).FirstOrDefault();
            if (safe != null)
            {
                Flow.Log($"escape: SafeExit at {safe.transform.position}");
                for (int i = 0; i < 2 && !Gone(); i++) yield return HoldAt(safe, 8f, Gone);
                if (Gone()) { how("SafeExit"); yield break; }
            }
            var wave = Exits("WaveExit").Where(e => Flow.Get(e, "ExitState")?.ToString() != "Finished")
                .OrderBy(e => Flow.Flat(e.transform.position - me.transform.position).sqrMagnitude).FirstOrDefault();
            if (wave != null)
            {
                Flow.Log($"escape: WaveExit at {wave.transform.position} {Flow.Get(wave, "ExitState")}");
                for (int i = 0; i < 3 && Flow.Get(wave, "ExitState")?.ToString() == "Deactivated" && !Gone(); i++)
                    yield return HoldAt(wave, 4f, () => Gone() || Flow.Get(wave, "ExitState")?.ToString() != "Deactivated");
                // Activating (a wave) then Activated: the escape field (SpecialField 4001, SphereFieldExit) stacks buff 3113 on
                // a diver inside it until the escape. Stand in it like a player waiting for the booth: no chasing monsters
                // out of it (the diver is invincible), only swings at what is in reach.
                float t = Time.realtimeSinceStartup, nextLog = t;
                while (!Gone() && Time.realtimeSinceStartup - t < 120f && Flow.Get(wave, "ExitState")?.ToString() != "Deactivated")
                {
                    if (Flow.Find("DialogPopupView") != null) { yield return DrainDialogs("dialog at the exit"); continue; }
                    float d = wave ? Flow.Flat(wave.transform.position - Me().transform.position).magnitude : 0f;
                    if (Time.realtimeSinceStartup > nextLog)
                    {
                        nextLog += 5f;
                        Flow.Log($"at the exit: {Flow.Get(wave, "ExitState")} remain={Flow.Value(wave, "_syncStateRemainTime"):F1} d={d:F1} stack3113={BuffStack(3113)} {MeState()}");
                    }
                    if (wave && d > 1.5f) { yield return Flow.WalkTo(() => wave ? wave.transform.position : (Vector3?)null, 1.0f, 3f, "back to the exit"); continue; }
                    var m = Enemy(1.6f);
                    if (m != null)
                    {
                        Flow.Aim(m.transform.position + Vector3.up * 0.5f);
                        var cam = Camera.main;
                        yield return Flow.MouseClick(cam ? (Vector2)cam.WorldToScreenPoint(m.transform.position + Vector3.up * 0.5f) : Vector2.zero);
                    }
                    yield return new WaitForSecondsRealtime(0.5f);
                }
                Flow.Log($"escape: WaveExit {Flow.Get(wave, "ExitState")} after {Time.realtimeSinceStartup - t:F0}s gone={Gone()}");
                if (Gone()) { how("WaveExit"); yield break; }
            }
            Flow.Log($"escape: forced escape (no exit ended the dive) {MeState()}");
            SendCheat("ReqForceEscape");
            yield return Flow.WaitUntil(Gone, 20f, "forced escape");
            how(Gone() ? "forced" : null);
        }

        static IEnumerator Results(int id, Action<string> fail)
        {
            yield return Flow.WaitUntil(() => Flow.ActiveScene == "LoungeScene", 240f, "back in the lounge");
            if (Flow.ActiveScene != "LoungeScene") { fail("did not return to the lounge"); yield break; }
            yield return Flow.WaitUntil(() => Flow.Find("StageResultPopupView") != null, 120f, "Dive Results");
            var view = Flow.Find("StageResultPopupView");
            if (view == null) { fail("no Dive Results"); yield break; }
            yield return new WaitForSecondsRealtime(4f);
            for (int i = 0; i < 6 && Flow.Find("StageResultPopupView") != null; i++)
            {
                if (Flow.Ready(Flow.Button(view, "Confirm_Button"))) yield return Flow.Click(view, "Confirm_Button");
                else if (Flow.Ready(Flow.Button(view, "SkipButton"))) yield return Flow.Click(view, "SkipButton");
                yield return new WaitForSecondsRealtime(2f);
            }
            yield return Flow.WaitUntil(() => Flow.Find("StageResultPopupView") == null && Flow.StageState() == "Playing", 60f, "lounge after the dive");
            if (!Flow.LastWaitOk) { fail("Dive Results did not close"); yield break; }
            yield return DrainDialogs("lounge after the dive");
            yield return CloseOpenCampaign("after");
            Flow.Log($"after {id}: room={Room()} campaign={CampaignNow()} {Flow.LoadState()}");
        }

        // A campaign still open after its dive (its tasks not done) keeps the room Playing. The player closes it at Elara:
        // her Campaign function asks "abandon <campaign>?" (InGameScene.OnCampaignNpcAsync -> TextPopup, Confirm ->
        // GameManager.CloseCampaign(GiveUp)) and opens the campaign menu, which Esc closes. A quest line she has for the
        // campaign (QuestCompleteSub when its tasks are done) plays first and closes it the normal way.
        static IEnumerator CloseOpenCampaign(string when)
        {
            for (int attempt = 0; attempt < 3; attempt++)
            {
                if (Room() == "Ready" && (CampaignNow() == "0" || CampaignNow() == null)) yield break;
                Flow.Log($"{when}: campaign {CampaignNow()} still open (room {Room()}); abandon at Elara");
                var elara = Flow.FindNpc(700000);
                if (elara == null) break;
                yield return Flow.InteractNpc(elara, "Elara abandon");
                yield return Flow.WaitUntil(() => Flow.Find("DialogPopupView") != null || Flow.Find("SelectionPopupView") != null || Flow.Find("TextPopupView") != null, 4f, "Elara reaction");
                if (Flow.Find("DialogPopupView") != null) { yield return Flow.AdvanceDialogs(() => Flow.Find("DialogPopupView") == null, 120f, "Elara line"); continue; }
                if (Flow.Find("SelectionPopupView") != null)
                {
                    yield return new WaitForSecondsRealtime(0.8f);
                    yield return Flow.SelectFunction(elara, "Campaign");
                }
                yield return Flow.WaitUntil(() => Flow.Find("TextPopupView") != null, 5f, "abandon confirmation");
                var confirm = Flow.Find("TextPopupView");
                if (confirm != null)
                {
                    Flow.Log($"{when}: confirm '{Flow.Get(Flow.Get(confirm, "Message"), "text")}'");
                    yield return Flow.Click(confirm, "Confirm_Button");
                }
                yield return Flow.WaitUntil(() => Room() == "Ready" && CampaignNow() == "0", 10f, "campaign closed");
                yield return new WaitForSecondsRealtime(1f);
                if (Flow.Find("CampaignPopupPresenter") != null) yield return Flow.Keys(0.2f, Key.Escape);
                yield return DrainDialogs($"{when} abandon");
            }
        }
    }
}
