using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using Object = UnityEngine.Object;

namespace VD.PlayTests
{
    // Dive support for A9a_LoungeQuestFlow: the pieces of A5/A6 a quest task needs inside a stage (walk to a point or an
    // object, fight what is in the way, loot, hold F on a trigger or an NPC, leave through a terminal), copied from
    // A6_BossFlow at c1dfb2dd so that file can keep changing. Every cheat goes through A9aDive.Cheats, which the test
    // reports.
    public static class A9aDive
    {
        static readonly Dictionary<string, int> s_cheatCounts = new Dictionary<string, int>();

        // What the dive cheated with, "type prop=value xN" (the HP/stress refill runs many times, so counted).
        public static List<string> Cheats => s_cheatCounts.Select(kv => $"{kv.Key} x{kv.Value}").ToList();

        public static void ResetCheats() => s_cheatCounts.Clear();

        static Type GameType(string fullName) =>
            AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType(fullName)).FirstOrDefault(t => t != null);

        // A cheat request to the host, as SROptions sends it (GameChannel.SendToHost).
        public static void SendCheat(string type, params (string prop, object value)[] props)
        {
            var request = Activator.CreateInstance(GameType(type));
            foreach (var (p, v) in props) request.GetType().GetProperty(p).SetValue(request, v);
            var channel = Flow.Get(Flow.Find("GameManager"), "_gameChannel");
            channel.GetType().GetMethod("SendToHost").Invoke(channel, new[] { request });
            string key = $"{type} {string.Join(" ", props.Select(x => $"{x.prop}={(x.value is float f ? (f > 1f ? "max" : f.ToString()) : x.value)}"))}";
            s_cheatCounts[key] = (s_cheatCounts.TryGetValue(key, out int n) ? n : 0) + 1;
        }

        static readonly HashSet<string> s_shots = new HashSet<string>();

        public static IEnumerator Shot(string key)
        {
            if (!s_shots.Add(key)) yield break;
            Flow.Log($"shot {key} {MeState()}");
            yield return Review.Shot(key);
        }

        static readonly HashSet<int> s_looted = new HashSet<int>();
        static readonly List<Vector3> s_waypoints = new List<Vector3>();

        public static string CampaignNow() => Flow.Value(Flow.Get(Flow.Find("GameManager"), "_gameContext"), "RxCampaignId")?.ToString();
        public static IEnumerator OpenCampaignPopup()
        {
            for (int attempt = 0; attempt < 5 && Flow.Find("CampaignPopupPresenter") == null; attempt++)
            {
                var elara = Flow.FindNpc(700000);
                yield return Flow.InteractNpc(elara, "Elara campaign");
                yield return Flow.WaitUntil(() => Flow.Find("DialogPopupView") != null || Flow.Find("SelectionPopupView") != null || Flow.Find("CampaignPopupPresenter") != null, 4f, "Elara reaction");
                if (Flow.Find("DialogPopupView") != null)
                {
                    yield return Flow.AdvanceDialogs(() => Flow.Find("DialogPopupView") == null, 120f, "Elara before her menu");
                    yield return new WaitForSecondsRealtime(1f);
                    continue;
                }
                if (Flow.Find("SelectionPopupView") != null)
                {
                    yield return new WaitForSecondsRealtime(0.8f);
                    yield return Flow.SelectFunction(elara, "Campaign");
                }
                // A campaign still running from the last dive (Normal campaigns stay open after an escape): Elara asks to
                // abandon it first (InGameScene.OnCampaignNpcAsync), then the list opens.
                yield return Flow.WaitUntil(() => Flow.Find("CampaignPopupPresenter") != null || Flow.Find("TextPopupView") != null, 8f, "CampaignPopupPresenter");
                if (Flow.Find("TextPopupView") != null)
                {
                    var text = Flow.Find("TextPopupView");
                    Flow.Log($"Elara asks to abandon campaign {CampaignNow()}");
                    yield return Flow.WaitUntil(() => Flow.Ready(Flow.Button(Flow.Find("TextPopupView"), "Confirm_Button")), 5f, "abandon confirm");
                    yield return Flow.Click(Flow.Find("TextPopupView"), "Confirm_Button");
                    yield return Flow.WaitUntil(() => Flow.Find("CampaignPopupPresenter") != null, 10f, "CampaignPopupPresenter after abandoning");
                }
            }
        }

        public static List<MonoBehaviour> CampaignSlots() => Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
            .Where(m => m != null && m.isActiveAndEnabled && m.GetType().Name == "CampaignSlotPresenter").ToList();

        public static string BoothCheck()
        {
            var booth = Flow.Find("PhoneBooth");
            var gm = Flow.Find("GameManager");
            if (booth == null || gm == null) return "no booth";
            var id = Flow.Get(gm, "LocalClientId");
            try { return booth.GetType().GetMethod("CheckCanEnter")?.Invoke(booth, new[] { id })?.ToString() ?? "?"; }
            catch (Exception e) { return $"threw {e.InnerException?.GetType().Name}"; }
        }

        public static IEnumerator EnterBooth()
        {
            var booth = Flow.Find("PhoneBooth");
            if (booth == null) { Flow.Log("no PhoneBooth in the lounge"); yield break; }
            for (int attempt = 0; attempt < 3 && State() != "StageReady" && Flow.ActiveScene == "LoungeScene"; attempt++)
            {
                yield return Flow.InteractObject(booth, 0.8f, "dive booth");
                yield return Flow.WaitUntil(() => State() == "StageReady" || Flow.ActiveScene != "LoungeScene", 4f, "StageReady");
                Flow.Log($"booth attempt {attempt}: state={State()} check={BoothCheck()} focus={Flow.Focus()}");
            }
            yield return Flow.WaitUntil(() => Flow.ActiveScene == "StageScene" && Flow.StageState() == "Playing", 240f, "stage Playing");
            Flow.Log($"stage {Flow.LoadState()}");
        }

        // ---- the diver ----

        public static Component Me() => Flow.MyCharacter();
        public static string State() => Flow.Get(Me(), "State")?.ToString() ?? "-";
        public static float HpMax() => Flow.Get(Me(), "HpMax") is float f ? f : 1f;
        public static float HpFrac() => Flow.Hp(Me()) / Mathf.Max(1f, HpMax());
        public static bool Over() => State() == "Dead" || State() == "Escaped" || Flow.ActiveScene != "StageScene" || Flow.StageState() == "Ended";

        public static string MeState()
        {
            var me = Me();
            return me == null ? "me=-" : $"hp={Flow.Hp(me)}/{HpMax():F0} stress={Flow.Get(me, "Stress")} state={State()} pos={me.transform.position}";
        }

        // Recover (the cheat menu's HP refill) when the diver is low, in the whole stage; see the header. 1101 refills
        // under 35% and leaves stress alone.
        public static float s_hpFloor = 0.6f;
        public static bool s_stressRecover = true;

        public static void KeepAlive()
        {
            if (Me() == null || Over() || State() == "Dying") return;
            if (HpFrac() < s_hpFloor) SendCheat("ReqSetCharacterHp", ("Hp", HpMax()));
            if (!s_stressRecover) return;
            // Stress 100 keeps summoning shadow Dark Youngs (ShadowMonsterSpawn) and stuns the diver; a player rests it
            // off between fights, here it is the recover cheat (run story3: the 1104 painting never took the F).
            if (Convert.ToSingle(Flow.Get(Me(), "Stress") ?? 0f) >= 70f) SendCheat("ReqSetCharacterStress", ("Stress", 0f));
        }

        class Guardian : MonoBehaviour
        {
            float _next;
            void Update()
            {
                if (Flow.ActiveScene != "StageScene") { Destroy(gameObject); return; }
                if (Time.realtimeSinceStartup < _next) return;
                _next = Time.realtimeSinceStartup + 0.15f;
                KeepAlive();
            }
        }

        // A wait that still refills HP every frame (run s1201b: a 10-monster squad took 630 HP during a blind 3 s wait).
        public static IEnumerator Breathe(float seconds)
        {
            for (float end = Time.realtimeSinceStartup + seconds; Time.realtimeSinceStartup < end && !Over(); )
            {
                KeepAlive();
                yield return null;
            }
        }

        // ---- monsters ----

        public static object Model(Component m) => Flow.Get(m, "UnitModel");
        public static string MonsterState(Component m) => Flow.Get(Model(m), "State")?.ToString() ?? "?";
        static int MonsterId(Component m) => Flow.Get(Model(m), "TableId") is int id ? id : 0;
        public static Component Enemy(float range)
        {
            var me = Me();
            if (me == null) return null;
            return Flow.Monsters().Where(m => MonsterState(m) != "Dead" && MonsterState(m) != "Dying")
                .Select(m => (m, d: Flow.Flat(m.transform.position - me.transform.position).magnitude))
                .Where(x => x.d < range).OrderBy(x => x.d).Select(x => x.m).FirstOrDefault();
        }

        public static Vector2 Aimed(Component m)
        {
            var cam = Camera.main;
            return cam ? (Vector2)cam.WorldToScreenPoint(m.transform.position + Vector3.up * 0.5f) : new Vector2(Screen.width / 2f, Screen.height / 2f);
        }

        static int s_skillUses;

        // One exchange: close in, aim, left click; every fourth swing a skill (right button, Q, E, R in turn) (A5).
        public static IEnumerator Swing(Component m, int clicks)
        {
            // A dead boss lies there until it despawns (run 12: swinging at the destroyed Donovan threw).
            if (!m || !Flow.Alive(m) || Me() == null) { yield return new WaitForSecondsRealtime(0.3f); yield break; }
            var me = Me();
            float d = Flow.Flat(m.transform.position - me.transform.position).magnitude;
            if (d > 1.8f) { yield return Flow.WalkTo(() => Flow.Alive(m) ? m.transform.position : (Vector3?)null, 1.5f, 2f, $"approach {m.name}"); yield break; }
            Flow.Hold();
            Flow.Aim(m.transform.position + Vector3.up * 0.5f);
            if (clicks % 4 == 0)
            {
                var skill = (s_skillUses++) % 4;
                if (skill == 0) yield return Flow.MouseClick(Aimed(m), MouseButton.Right);
                else yield return Flow.Keys(0.12f, skill == 1 ? Key.Q : skill == 2 ? Key.E : Key.R);
            }
            else yield return Flow.MouseClick(Aimed(m));
            yield return new WaitForSecondsRealtime(0.35f);
        }

        // A small monster in the way: fight it for up to 30 s.
        public static IEnumerator Fight(Component m)
        {
            float start = Time.realtimeSinceStartup;
            int clicks = 0;
            string name = m.name;
            Flow.Log($"fight {name} {MonsterId(m)} hp={Flow.Hp(m)} {MeState()}");
            while (Flow.Alive(m) && Time.realtimeSinceStartup - start < 30f && !Over() && State() != "Dying")
            {
                if (Flow.Find("DialogPopupView") != null) { yield return Flow.AdvanceDialogs(() => Flow.Find("DialogPopupView") == null, 60f, "dialog in a fight"); continue; }
                if (Flow.Flat(m.transform.position - Me().transform.position).magnitude > 10f) break;
                KeepAlive();
                yield return Swing(m, ++clicks);
            }
            Flow.Log($"fight {name} end alive={Flow.Alive(m)} clicks={clicks} {MeState()}");
        }

        // ---- the stage ----

        public static Component Trigger(int id) => Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
            .FirstOrDefault(m => m != null && m.GetType().Name == "InteractiveTrigger" && Flow.Get(m, "TriggerId") is int t && t == id);

        public static void DumpStageObjects(string when)
        {
            foreach (var m in Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None))
            {
                if (m == null) continue;
                string n = m.GetType().Name;
                if (n == "InteractiveTrigger") Flow.Log($"{when}: trigger {Flow.Get(m, "TriggerId")} {m.name} pos={m.transform.position} can={Flow.Get(m, "CanTriggered")} active={m.isActiveAndEnabled}");
                else if (n == "MiniMapMarker" || n.Contains("QuestMarker")) Flow.Log($"{when}: {n} {m.name} pos={m.transform.position}");
                else if (n == "NpcObject" || m.GetType().BaseType?.Name == "NpcObject") Flow.Log($"{when}: npc {Flow.NpcId(m)} pos={m.transform.position} state={Flow.Value(m, "RxNpcState") ?? Flow.Get(m, "NpcState")}");
            }
            Flow.Log($"{when}: {MeState()} monsters={Flow.Monsters().Count()} {Flow.LoadState()}");
        }

        public static float PathLength(Vector3 to)
        {
            var path = new UnityEngine.AI.NavMeshPath();
            if (Me() == null || !UnityEngine.AI.NavMesh.SamplePosition(to, out var hit, 2.5f, UnityEngine.AI.NavMesh.AllAreas)
                || !UnityEngine.AI.NavMesh.CalculatePath(Me().transform.position, hit.position, UnityEngine.AI.NavMesh.AllAreas, path)
                || path.status != UnityEngine.AI.NavMeshPathStatus.PathComplete) return float.MaxValue;
            float len = 0f;
            for (int i = 1; i < path.corners.Length; i++) len += Vector3.Distance(path.corners[i - 1], path.corners[i]);
            return len;
        }

        // The quest marker's goal: the nearer portal of the campaign's pair (SetQuestMarkerAtPortalPair; 1102 marks
        public static Component CollisionTriggerById(int id) => Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
            .FirstOrDefault(m => m != null && m.GetType().Name == "CollisionTrigger" && Flow.Get(m, "TriggerId") is int t && t == id);

        public static Vector3? ZonePosition(int zoneSpawnId)
        {
            var om = Flow.Find("GameObjectManager");
            try { return om?.GetType().GetMethod("GetZoneSpawnPosition")?.Invoke(om, new object[] { zoneSpawnId }) as Vector3?; }
            catch (Exception e) { Flow.Log($"zone {zoneSpawnId}: {e.InnerException?.Message}"); return null; }
        }

        // Several Flow.Go legs while the distance shrinks (one leg gives up after 20 s; A6b run 6), fighting what blocks.
        public static IEnumerator GoFar(Vector3 goal, float arrive, string what, float budget = 180f)
        {
            float start = Time.realtimeSinceStartup, best = float.MaxValue;
            int noProgress = 0;
            while (Time.realtimeSinceStartup - start < budget && Me() != null && !Over())
            {
                KeepAlive();
                if (Flow.Find("DialogPopupView") != null) { yield return Flow.AdvanceDialogs(() => Flow.Find("DialogPopupView") == null, 60f, $"dialog on the way to {what}"); continue; }
                float d = Flow.Flat(goal - Me().transform.position).magnitude;
                if (d <= arrive) break;
                var m = Enemy(2f);
                if (m != null && noProgress > 0) { yield return Fight(m); continue; }
                if (d < best - 1f) { best = d; noProgress = 0; }
                else if (++noProgress >= 4) { Flow.Log($"go far {what}: no progress at d={d:F1}"); break; }
                yield return Flow.Go(goal, arrive, 40f, what, run: true);
                if (noProgress > 0) yield return Flow.Keys(0.5f, noProgress % 2 == 1 ? Key.A : Key.D);
            }
        }

        // Fight what is near until done() or the time is up, staying around a point (a wave's spawn).
        public static IEnumerator Hold(Vector3 at, Func<bool> done, float budget, string what, Func<IEnumerator> onTick = null)
        {
            float start = Time.realtimeSinceStartup, nextLog = start;
            while (!done() && !Over() && Time.realtimeSinceStartup - start < budget)
            {
                KeepAlive();
                if (Flow.Find("DialogPopupView") != null) { yield return Flow.AdvanceDialogs(() => Flow.Find("DialogPopupView") == null, 60f, $"dialog in {what}"); continue; }
                if (Time.realtimeSinceStartup > nextLog) { nextLog += 10f; Flow.Log($"{what}: t={Time.realtimeSinceStartup - start:F0} monsters near={Flow.Monsters().Count(x => Flow.Flat(x.transform.position - Me().transform.position).magnitude < 15f)} {MeState()}"); }
                if (onTick != null) yield return onTick();
                var m = Enemy(12f);
                if (m != null) { yield return Fight(m); continue; }
                if (Flow.Flat(at - Me().transform.position).magnitude > 4f) { yield return Flow.WalkTo(() => at, 3f, 4f, $"back to {what}"); continue; }
                yield return new WaitForSecondsRealtime(0.5f);
            }
            Flow.Log($"{what}: done={done()} after {Time.realtimeSinceStartup - start:F0}s {MeState()}");
        }

        public static IEnumerator UseTrigger(int id, string what, string shot = null)
        {
            var t = Trigger(id);
            Flow.Log($"{what}: trigger {id} {(t ? $"at {t.transform.position} can={Flow.Get(t, "CanTriggered")}" : "missing")}");
            if (t == null) yield break;
            yield return GoFar(t.transform.position, 1.2f, what);
            yield return Flow.WaitUntil(() => !t || Flow.Get(t, "CanTriggered") is bool c && c, 10f, $"{what} triggerable");
            for (int i = 0; i < 6 && t && Flow.Get(t, "CanTriggered") is bool can && can && !Over(); i++)
            {
                // A monster on the diver (stun, hit reactions) eats the F; clear it first.
                for (int f = 0; f < 5 && Enemy(6f) is Component m && !Over(); f++) yield return Fight(m);
                yield return GoFar(t.transform.position, 1.2f, what);
                // The interaction sensor is small: run diag6 stood 0.96 m from equipment 2 for six tries with no focus.
                yield return Flow.InteractObject(t, 1.0f, what, i == 0 ? 1.0f : 0.5f);
                yield return Breathe(1f);
            }
            Flow.Log($"{what}: after F can={(t ? Flow.Get(t, "CanTriggered") : "gone")} {MeState()}");
            Assert.IsFalse(t && Flow.Get(t, "CanTriggered") is bool still && still, $"{what}: trigger {id} still takes F after 6 tries");
            if (shot != null) { yield return Breathe(1f); yield return Shot(shot); }
        }

        // 1104 The Compatible One: walk into collision 104011 at the gallery (ZONE 11043), the encounter cutscene
        // (IngameCutScene_Chapter_01_Campaign_4), the Voice of Liberation wave (WaveExecutor 11041); when it is cleared the
        // painting (trigger 11041) can be touched, and Lua drops booth 1007 at zone 11042.
        public static void Fire(object owner, string subject, object arg)
        {
            var sub = Flow.Get(owner, subject);
            sub?.GetType().GetMethod("OnNext")?.Invoke(sub, new[] { arg });
        }

        // ---- 1101: the first story, from the post-tutorial profile, no cleared-campaign cheat ----

        public static float Stress() => Convert.ToSingle(Flow.Get(Me(), "Stress") ?? 0f);

        // Shadow monsters (TMonster MonsterType Shadow, 9000xx) are the stress hallucinations; a looting player runs past them.
        public static Component RealEnemy(float range)
        {
            var me = Me();
            if (me == null) return null;
            return Flow.Monsters().Where(m => MonsterState(m) != "Dead" && MonsterState(m) != "Dying" && MonsterId(m) / 100000 != 9)
                .Select(m => (m, d: Flow.Flat(m.transform.position - me.transform.position).magnitude))
                .Where(x => x.d < range).OrderBy(x => x.d).Select(x => x.m).FirstOrDefault();
        }

        // Boxes anywhere (a player heads for the ones the map shows), loose drops and dead bodies close by (A5 IsLoot).
        public static Component NearestLoot()
        {
            var me = Me();
            if (me == null) return null;
            return Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
                .Where(m => m != null && m.isActiveAndEnabled && !s_looted.Contains(m.GetInstanceID()))
                .Select(m => (m, n: m.GetType().Name, d: Flow.Flat(m.transform.position - me.transform.position).magnitude))
                .Where(x => x.n == "RewardBox" && x.d < 80f || x.n == "DropGoods" && x.d < 10f
                    || x.n == "MonsterBody" && x.d < 8f && Flow.Get(Flow.Get(x.m, "_monsterModel"), "State")?.ToString() == "Dead")
                .OrderBy(x => x.d).Select(x => (Component)x.m).FirstOrDefault();
        }

        // Walk up, hold F; a box or body opens the looting panel: take each slot as the search reveals it (A5 Loot).
        public static IEnumerator Loot(Component target)
        {
            s_looted.Add(target.GetInstanceID());
            string kind = target.GetType().Name, name = target.name;
            Flow.Log($"loot {kind} {name} at {target.transform.position}");
            yield return GoFar(target.transform.position, 1.5f, $"loot {name}", 90f);
            if (!target || Over()) yield break;
            yield return Flow.WalkTo(() => target ? target.transform.position : (Vector3?)null, 0.9f, 8f, $"loot {name}");
            if (!target) yield break;
            Flow.Aim(target.transform.position);
            yield return Flow.WaitUntil(() => Flow.Focused(target), 3f, $"focus {name}");
            if (!Flow.LastWaitOk) { Flow.Log($"loot {name}: no focus ({Flow.Focus()})"); yield break; }
            float hold = Flow.Get(target, "HoldingTime") is float h ? h + 0.3f : 0.8f;
            yield return Flow.Keys(Mathf.Clamp(hold, 0.3f, 3f), Key.F);
            if (kind == "DropGoods") { yield return new WaitForSecondsRealtime(0.8f); Flow.Log($"picked up {name}: inventory {InventoryIds()}"); yield break; }
            yield return Flow.WaitUntil(() => Flow.Find("LootingInventoryPanelPresenter") != null, 6f, $"{name} looting panel");
            var panel = Flow.Find("LootingInventoryPanelPresenter");
            if (panel == null) { Flow.Log($"loot {name}: no looting panel, focus={Flow.Focus()} state={State()}"); yield break; }
            var slots = Flow.Get(panel, "_goodsSlotModels") as IList;
            var taken = new HashSet<object>();
            float searchStart = Time.realtimeSinceStartup;
            while (slots != null && Time.realtimeSinceStartup - searchStart < 60f && Flow.Find("LootingInventoryPanelPresenter") != null && !Over() && State() != "Dying")
            {
                bool pending = false;
                foreach (var slot in slots.Cast<object>().ToList())
                {
                    if (taken.Contains(slot)) continue;
                    var gs = Flow.Value(slot, "RxGoodsSlot");
                    if (gs == null || Flow.Get(gs, "IsEmpty") is bool e && e) continue;
                    if (!(Flow.Get(gs, "IsRevealed") is bool r && r)) { pending = true; continue; }
                    Flow.Log($"loot slot {Flow.Get(gs, "Id")} x{Flow.Get(gs, "Count")}");
                    Fire(slot, "OnEnter", slot);
                    yield return null;
                    Fire(slot, "OnLeftClick", slot);
                    taken.Add(slot);
                    yield return new WaitForSecondsRealtime(0.4f);
                }
                if (!pending) break;
                yield return new WaitForSecondsRealtime(0.3f);
            }
            Flow.Log($"loot {name} took {taken.Count} slots inventory: {InventoryIds()}");
            for (int i = 0; i < 2 && (Flow.Find("LootingInventoryPanelPresenter") != null || Flow.Find("MenuPopupPresenter") != null || State() == "Interact"); i++)
            {
                yield return Flow.Keys(0.2f, i == 0 ? Key.Escape : Key.Tab);
                yield return new WaitForSecondsRealtime(0.8f);
            }
        }

        public static string InventoryIds()
        {
            var inv = Flow.Get(Flow.Find("GameManager"), "MyInventory");
            var sb = new System.Text.StringBuilder();
            if (Flow.Get(inv, "Slots") is IEnumerable slots)
                foreach (var kv in slots)
                    if (Flow.Get(kv, "Value") is object gs && !(Flow.Get(gs, "IsEmpty") is bool e && e)) sb.Append($"{Flow.Get(gs, "Id")}x{Flow.Get(gs, "Count")},");
            return sb.Length == 0 ? "(empty)" : sb.ToString();
        }

        // NavMesh points 10 m apart across the stage (A5).
        public static void BuildWaypoints()
        {
            var tri = UnityEngine.AI.NavMesh.CalculateTriangulation();
            if (tri.vertices.Length == 0) return;
            var b = new Bounds(tri.vertices[0], Vector3.zero);
            foreach (var v in tri.vertices) b.Encapsulate(v);
            for (float x = b.min.x + 5f; x < b.max.x; x += 10f)
                for (float z = b.min.z + 5f; z < b.max.z; z += 10f)
                    if (UnityEngine.AI.NavMesh.SamplePosition(new Vector3(x, b.center.y, z), out var hit, 3f, UnityEngine.AI.NavMesh.AllAreas))
                        s_waypoints.Add(hit.position);
            Flow.Log($"waypoints {s_waypoints.Count} over {b.min}..{b.max}");
        }

        // The nearest terminal (WaveExit): hold F to activate it (ExitActivateInteractionTime 2 s), hold out the wave
        // while it charges, stay in its field until the escape takes the diver (A5 Escape).
        // The nearest terminal (WaveExit) not used up: hold F to activate it, hold out the wave while it charges, stay in its
        // field until the escape takes the diver (A5 Escape). A terminal that finished without taking the diver (he was
        // pulled away by a fight and the field takes those who stay) is used up: go to the next one.
        public static IEnumerator TerminalEscape()
        {
            for (int round = 0; round < 4 && !Over(); round++)
            {
                var exit = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
                    .Where(m => m != null && m.isActiveAndEnabled && m.GetType().Name == "WaveExit" && Flow.Get(m, "ExitState")?.ToString() != "Finished")
                    .OrderBy(m => PathLength(m.transform.position)).FirstOrDefault();
                if (exit == null) { Flow.Log("terminal: none left in the stage"); yield break; }
                string ExitNow() => exit ? $"{Flow.Get(exit, "ExitState")} remain={Flow.Get(exit, "StateRemainTime")}" : "gone";
                Flow.Log($"terminal round {round} at {exit.transform.position} {ExitNow()} path={PathLength(exit.transform.position):F0}");
                float start = Time.realtimeSinceStartup;
                while (!Over() && exit && Time.realtimeSinceStartup - start < 300f && Flow.Get(exit, "ExitState")?.ToString() == "Deactivated")
                {
                    yield return GoFar(exit.transform.position, 1.2f, "terminal", 120f);
                    if (Over() || !exit) break;
                    var m = RealEnemy(2.5f);
                    if (m != null) { yield return Fight(m); continue; }
                    yield return Flow.WalkTo(() => exit ? exit.transform.position : (Vector3?)null, 1.0f, 5f, "terminal");
                    Flow.Aim(exit.transform.position);
                    yield return Flow.WaitUntil(() => Flow.Focused(exit), 3f, "focus terminal");
                    yield return Flow.Keys(2.6f, Key.F);
                    yield return new WaitForSecondsRealtime(0.5f);
                    Flow.Log($"terminal after F {ExitNow()} focus={Flow.Focus()} {MeState()}");
                }
                if (Over() || !exit || Flow.Get(exit, "ExitState")?.ToString() == "Deactivated") { Flow.Log($"terminal not activated {ExitNow()} {MeState()}"); continue; }
                yield return new WaitForSecondsRealtime(1.5f);
                float hold = Time.realtimeSinceStartup, nextLog = hold;
                while (!Over() && exit && Flow.Get(exit, "ExitState")?.ToString() != "Finished" && Time.realtimeSinceStartup - hold < 120f)
                {
                    if (Time.realtimeSinceStartup > nextLog) { nextLog += 10f; Flow.Log($"terminal {ExitNow()} {MeState()}"); }
                    var m = RealEnemy(3.5f);
                    if (m != null) { yield return Fight(m); continue; }
                    if (exit && Flow.Flat(exit.transform.position - Me().transform.position).magnitude > 1.5f)
                        yield return Flow.WalkTo(() => exit ? exit.transform.position : (Vector3?)null, 0.8f, 3f, "back to the terminal");
                    yield return new WaitForSecondsRealtime(0.3f);
                }
                yield return new WaitForSecondsRealtime(2f);
                Flow.Log($"terminal end {ExitNow()} {MeState()} stage={Flow.StageState()}");
                if (Over()) yield break;
            }
        }

        public static IEnumerator ExploreLoot(Func<bool> done, float budget, string what)
        {
            float start = Time.realtimeSinceStartup, nextLog = start;
            while (!Over() && Time.realtimeSinceStartup - start < budget && !done())
            {
                if (Time.realtimeSinceStartup > nextLog) { nextLog += 30f; Flow.Log($"{what} t={Time.realtimeSinceStartup - start:F0} inventory={InventoryIds()} {MeState()}"); }
                if (State() == "Dying") { yield return new WaitForSecondsRealtime(1f); continue; }
                if (Flow.Find("DialogPopupView") != null) { yield return Flow.AdvanceDialogs(() => Flow.Find("DialogPopupView") == null, 60f, "dialog in the dive"); continue; }
                var m = RealEnemy(7f);
                if (m != null) { yield return Fight(m); continue; }
                var loot = NearestLoot();
                if (loot != null) { yield return Loot(loot); continue; }
                var me = Me();
                if (me == null || s_waypoints.Count == 0) { Flow.Log($"{what}: no waypoint left"); break; }
                var wp = s_waypoints.OrderBy(w => Flow.Flat(w - me.transform.position).sqrMagnitude).First();
                s_waypoints.Remove(wp);
                yield return Flow.Go(wp, 2f, 30f, "explore", run: true, stop: () => RealEnemy(6f) != null || NearestLoot() != null || Over() || done());
            }
            Flow.Log($"{what} end after {Time.realtimeSinceStartup - start:F0}s done={done()} inventory={InventoryIds()} {MeState()}");
        }

        // Where this stage will spawn monsters of these spawn groups (GameObjectManager._monsterSpawnDatas, offsets applied).
        // Monsters come into being only within MonsterSpawnRangeMax (15 m) of a diver, except patrols.
        public static List<Vector3> SpawnPoints(params int[] groupIds)
        {
            var om = Flow.Find("GameObjectManager");
            var pts = new List<Vector3>();
            if (Flow.Get(om, "_monsterSpawnDatas") is IEnumerable list)
                foreach (var d in list)
                    if (Flow.Get(d, "UseMonsterSpawnTable") is bool t && t && Flow.Get(d, "MonsterSpawnGroupId") is int g && groupIds.Contains(g)
                        && Flow.Get(d, "SpawnPosition") is Vector3 p) pts.Add(p);
            return pts;
        }

        public static IEnumerable<Component> MonstersOf(params int[] ids) =>
            Flow.Monsters().Where(m => ids.Contains(MonsterId(m))).Cast<Component>();

        public static Vector3 LastTargetAt;

        // Walk up to the nearest live monster of these table ids and fight it until none is left (phases come as further ids).
        public static IEnumerator Kill(string what, float budget, params int[] ids)
        {
            float start = Time.realtimeSinceStartup, nextLog = start;
            int clicks = 0;
            float lastSeen = start;
            while (!Over() && Time.realtimeSinceStartup - start < budget)
            {
                var me = Me();
                var target = MonstersOf(ids).OrderBy(x => Flow.Flat(x.transform.position - me.transform.position).sqrMagnitude).FirstOrDefault();
                if (target == null)
                {
                    // Between phases the dead body is replaced by the next one; give it a few seconds, and take what the
                    // fallen phase dropped (the quest item drops from the first phase of Hachishaku and the Soul Collector).
                    yield return LootAround(LastTargetAt, 12f, what);
                    if (Time.realtimeSinceStartup - lastSeen > 6f) break;
                    yield return Breathe(0.5f);
                    continue;
                }
                lastSeen = Time.realtimeSinceStartup;
                LastTargetAt = target.transform.position;
                if (State() == "Dying") { yield return Flow.WaitUntil(() => State() != "Dying" || Over(), 30f, "out of Dying"); continue; }
                if (Flow.Find("DialogPopupView") != null) { yield return Flow.AdvanceDialogs(() => Flow.Find("DialogPopupView") == null, 60f, $"dialog in {what}"); continue; }
                KeepAlive();
                float d = Flow.Flat(target.transform.position - me.transform.position).magnitude;
                if (Time.realtimeSinceStartup > nextLog) { nextLog += 10f; Flow.Log($"{what} t={Time.realtimeSinceStartup - start:F0} target={MonsterId(target)} hp={Flow.Hp(target)} d={d:F0} clicks={clicks} {MeState()}"); }
                if (d > 8f) { yield return GoFar(target.transform.position, 4f, what, 90f); continue; }
                var add = Enemy(1.8f);
                var pick = add != null && !ids.Contains(MonsterId(add)) && d > 3f ? add : target;
                yield return Swing(pick, ++clicks);
            }
            Flow.Log($"{what} end after {Time.realtimeSinceStartup - start:F0}s left={MonstersOf(ids).Count()} clicks={clicks} {MeState()}");
        }

        // The dead bodies (and drops) within range of a point: open each and take what it holds.
        public static IEnumerator LootAround(Vector3 at, float range, string what)
        {
            for (int i = 0; i < 6 && !Over(); i++)
            {
                var next = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
                    .Where(m => m != null && m.isActiveAndEnabled && !s_looted.Contains(m.GetInstanceID()))
                    .Select(m => (m, n: m.GetType().Name, d: Flow.Flat(m.transform.position - at).magnitude))
                    .Where(x => x.d < range && (x.n == "DropGoods" || x.n == "MonsterBody" && Flow.Get(Flow.Get(x.m, "_monsterModel"), "State")?.ToString() == "Dead"))
                    .OrderBy(x => x.d).Select(x => (Component)x.m).FirstOrDefault();
                if (next == null) break;
                Flow.Log($"{what}: loot {next.GetType().Name} {next.name} at {next.transform.position}");
                yield return Loot(next);
            }
        }

        // Walk to an NPC of the stage and press F on it; plays the dialog it opens.
        public static IEnumerator TalkTo(int npcId, string what, float budget = 120f)
        {
            var npc = Flow.FindNpc(npcId);
            if (npc == null) { Flow.Log($"{what}: npc {npcId} not in the stage"); yield break; }
            Flow.Log($"{what}: npc {npcId} at {npc.transform.position} state={Flow.Value(npc, "RxNpcState")}");
            yield return GoFar(npc.transform.position, 2.0f, what, budget);
            for (int i = 0; i < 3 && npc && !Over(); i++)
            {
                yield return Flow.InteractNpc(npc, what);
                yield return Flow.WaitUntil(() => Flow.Find("DialogPopupView") != null || Flow.Find("SelectionPopupView") != null || Flow.Find("ShopPopupPresenter") != null, 4f, $"{what} reaction");
                if (Flow.Find("DialogPopupView") != null) { yield return Flow.AdvanceDialogs(() => Flow.Find("DialogPopupView") == null, 90f, what); break; }
                if (Flow.Find("SelectionPopupView") != null || Flow.Find("ShopPopupPresenter") != null) break;
                yield return Breathe(1f);
            }
        }


        public static int BoxTableId(Component box) => Flow.Get(Flow.Get(box, "TableData"), "Id") is int id ? id : 0;

        public static Component Box(params int[] tableIds) => Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
            .Where(m => m != null && m.isActiveAndEnabled && m.GetType().Name == "RewardBox" && tableIds.Contains(BoxTableId(m)))
            .OrderBy(m => PathLength(m.transform.position)).FirstOrDefault();

        // ---- entering and leaving a Normal campaign ----

        // Elara's campaign list, a slot of this pool (the Normal tab when the list opened on Story; the scroller recycles its
        // cells, so the list is scrolled to the first campaign of the pool when none shows), Start Campaign, then the dive
        // booth, until the stage is playing. A campaign of the pool still running from the last dive goes straight to the
        // booth. The guardian (HP/stress refill) starts with the stage.
        public static int Campaign;

        public static IEnumerator Enter(params int[] pool)
        {
            // Elara's list offers 7 Normal campaigns picked by reputation level (GameCampaignManager.RefreshPlayableNormalCampaignIds)
            // and picks again when a campaign closes. None of the pool on offer: start one that is, then abandon it (Elara asks)
            // and look at the new list, as a player re-rolls the board.
            for (int roll = 0; roll < 12 && !pool.Any(c => c.ToString() == CampaignNow()); roll++)
            {
                yield return OpenCampaignPopup();
                Assert.IsNotNull(Flow.Find("CampaignPopupPresenter"), $"Elara's campaign list did not open, top={Flow.TopPopup()}");
                yield return new WaitForSecondsRealtime(2f);
                yield return PickCampaign(pool);
                Flow.Log($"campaign roll {roll}: running {CampaignNow()} pool [{string.Join(",", pool)}]");
            }
            Assert.IsTrue(pool.Any(c => c.ToString() == CampaignNow()), $"no campaign of [{string.Join(",", pool)}] was ever on offer, campaign={CampaignNow()}");
            Campaign = int.Parse(CampaignNow());
            yield return new WaitForSecondsRealtime(3.5f);
            yield return EnterBooth();
            Assert.AreEqual("StageScene", Flow.ActiveScene, "the dive booth did not lead into the stage");
            yield return new WaitForSecondsRealtime(3f);
            s_looted.Clear();
            s_waypoints.Clear();
            BuildWaypoints();
            var guardian = new GameObject("VD_A9A_Guardian").AddComponent<Guardian>();
            Object.DontDestroyOnLoad(guardian.gameObject);
            Flow.Log($"stage campaign={CampaignNow()} {MeState()} monsters={Flow.Monsters().Count()} {Flow.LoadState()}");
        }

        static string SlotIds() => string.Join(",", CampaignSlots().Select(s => Flow.Get(Flow.Get(s, "_model"), "CampaignId")));

        public static IEnumerator PickCampaign(int[] pool)
        {
            MonoBehaviour Slot() => CampaignSlots().FirstOrDefault(s => Flow.Get(Flow.Get(s, "_model"), "CampaignId") is int id && pool.Contains(id));
            Flow.Log($"campaign slots=[{SlotIds()}] pool=[{string.Join(",", pool)}]");
            if (Slot() == null && CampaignSlots().Count == 0 || Slot() == null && !CampaignSlots().Any(s => Flow.Get(Flow.Get(s, "_model"), "CampaignId") is int id && id < 1000))
            {
                var popup = Flow.Find("CampaignPopupPresenter");
                var toggle = Flow.Get(Flow.Get(popup, "_view"), "CategoryCommon_Toggle_Normal") as UnityEngine.UI.Toggle;
                if (toggle != null) { toggle.isOn = false; toggle.isOn = true; }
                yield return new WaitForSecondsRealtime(2f);
                Flow.Log($"campaign slots after the Normal tab=[{SlotIds()}]");
            }
            if (Slot() == null)
            {
                var scrollerPresenter = Flow.Find("CampaignScrollerPresenter");
                var models = Flow.Get(scrollerPresenter, "CellViewModels") as IList;
                var scroller = Flow.Get(scrollerPresenter, "Scroller");
                int index = -1;
                if (models != null) for (int i = 0; i < models.Count && index < 0; i++) if (Flow.Get(models[i], "CampaignId") is int id && pool.Contains(id)) index = i;
                Flow.Log($"campaign list: {models?.Count} entries, first of the pool at {index}");
                if (index >= 0 && scroller != null)
                {
                    var jump = scroller.GetType().GetMethod("JumpToDataIndex");
                    var args = jump.GetParameters().Select(pi => pi.Position == 0 ? (object)index : Type.Missing).ToArray();
                    jump.Invoke(scroller, System.Reflection.BindingFlags.OptionalParamBinding | System.Reflection.BindingFlags.InvokeMethod | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public, null, args, null);
                    yield return new WaitForSecondsRealtime(1.5f);
                    Flow.Log($"campaign slots after the jump=[{SlotIds()}]");
                }
            }
            var slot = Slot();
            if (slot == null)
            {
                slot = CampaignSlots().FirstOrDefault(sl => Flow.Get(Flow.Get(sl, "_model"), "CampaignId") is int id && id < 1000);
                Flow.Log($"none of [{string.Join(",", pool)}] on offer ({SlotIds()}); starting one to re-roll the board");
            }
            Assert.IsNotNull(slot, $"no Normal campaign in Elara's list (visible: {SlotIds()})");
            int campaignId = (int)Flow.Get(Flow.Get(slot, "_model"), "CampaignId");
            var slotButton = Flow.Get(slot, "Button") as UnityEngine.UI.Button;
            yield return Flow.WaitUntil(() => Flow.Ready(slotButton), 5f, $"campaign slot {campaignId}");
            slotButton.onClick.Invoke();
            MonoBehaviour detail = null;
            yield return Flow.WaitUntil(() => (detail = Flow.Find("CampaignDetailPanelPresenter")) != null
                && Flow.Get(Flow.Get(detail, "_model"), "CampaignId") is int id && id == campaignId, 6f, $"campaign {campaignId} detail");
            Assert.IsNotNull(detail, $"campaign {campaignId} detail did not open");
            yield return new WaitForSecondsRealtime(1.5f);
            var view = Flow.Get(detail, "_view");
            var ctx = Flow.Get(Flow.Find("GameManager"), "_gameContext");
            Flow.Log($"campaign {campaignId} difficulty={Flow.Value(ctx, "RxDifficulty")} text='{Flow.Get(Flow.Get(view, "Info_Difficulty_Text"), "text")}'");
            var start = Flow.Get(view, "Info_StartCampaignButton") as UnityEngine.UI.Button;
            yield return Flow.WaitUntil(() => Flow.Ready(start), 5f, "Start Campaign interactable");
            start.onClick.Invoke();
            yield return Flow.WaitUntil(() => CampaignNow() == campaignId.ToString(), 10f, "campaign started");
            yield return Flow.WaitUntil(() => Flow.TopPopup() == null, 15f, "campaign popups gone");
            if (Flow.Find("CampaignPopupPresenter") != null) yield return Flow.Keys(0.2f, Key.Escape);
            yield return new WaitForSecondsRealtime(1f);
        }

        // Out through a terminal, the banner, and the Dive Results back in the lounge (Confirm).
        public static IEnumerator Leave()
        {
            if (!Over()) yield return TerminalEscape();
            yield return Flow.WaitUntil(() => Flow.Find("EscapeSuccessPopupView") != null || Flow.Find("EscapeFailPopupView") != null
                || Flow.Find("CampaignFailPopupView") != null || Flow.ActiveScene != "StageScene", 180f, "dive end banner");
            Flow.Log($"dive end: top={Flow.TopPopup()} {MeState()}");
            yield return Flow.WaitUntil(() => Flow.ActiveScene == "LoungeScene", 240f, "back in the lounge");
            yield return Flow.WaitUntil(() => Flow.Find("StageResultPopupView") != null, 120f, "Dive Results");
            var view = Flow.Find("StageResultPopupView");
            if (view != null)
            {
                yield return new WaitForSecondsRealtime(6f);
                for (int i = 0; i < 6 && Flow.Find("StageResultPopupView") != null; i++)
                {
                    if (Flow.Ready(Flow.Button(view, "Confirm_Button"))) yield return Flow.Click(view, "Confirm_Button");
                    else if (Flow.Ready(Flow.Button(view, "SkipButton"))) yield return Flow.Click(view, "SkipButton");
                    yield return new WaitForSecondsRealtime(2f);
                }
            }
            yield return Flow.WaitUntil(() => Flow.Find("StageResultPopupView") == null && Flow.StageState() == "Playing", 60f, "lounge after the dive");
            if (Flow.Find("DialogPopupView") != null)
                yield return Flow.AdvanceDialogs(() => Flow.Find("DialogPopupView") == null, 120f, "lounge after the dive");
            yield return new WaitForSecondsRealtime(3f);
        }

        // What a stage holds, for choosing where to walk: monsters by table id, triggers, NPCs, reward boxes.
        public static void Scout(string label)
        {
            var me = Me();
            string D(Component c) => me ? $"{Flow.Flat(c.transform.position - me.transform.position).magnitude:F0}m" : "?";
            foreach (var g in Flow.Monsters().GroupBy(MonsterId).OrderBy(g => g.Key))
            {
                var first = g.First();
                Flow.Log($"scout {label}: monster {g.Key} x{g.Count()} {first.name} first at {first.transform.position} {D(first)} path={PathLength(first.transform.position):F0}");
            }
            foreach (var m in Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None))
            {
                if (m == null || !m.isActiveAndEnabled) continue;
                string n = m.GetType().Name;
                if (n == "InteractiveTrigger") Flow.Log($"scout {label}: trigger {Flow.Get(m, "TriggerId")} {m.name} at {m.transform.position} {D(m)} path={PathLength(m.transform.position):F0} can={Flow.Get(m, "CanTriggered")}");
                else if (n == "NpcObject" || m.GetType().BaseType?.Name == "NpcObject") Flow.Log($"scout {label}: npc {Flow.NpcId(m)} at {m.transform.position} {D(m)} path={PathLength(m.transform.position):F0}");
                else if (n == "RewardBox") Flow.Log($"scout {label}: box {m.name} {Flow.Get(m, "PrefabName")} at {m.transform.position} {D(m)}");
                else if (n == "WaveExit") Flow.Log($"scout {label}: terminal {m.name} at {m.transform.position} {D(m)} {Flow.Get(m, "ExitState")}");
            }
        }
    }
}
