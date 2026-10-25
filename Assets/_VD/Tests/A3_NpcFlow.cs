using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace VD.PlayTests
{
    // A3: the lounge NPCs' functions, used like a player: walk up with WASD, hold F, pick the function in the selection
    // popup, look at the panel (one review shot per docs/review/ref/npc-* key), close it with Esc.
    //
    // Assumptions (keep the run reproducible):
    // - Saves: the run starts from the profile the tutorial leaves (tools/fixtures/saves-post-tutorial, taken from SavesDir
    //   at the end of an A4 run: campaign 1100 cleared, IsTutorial false, level 1, 1500 gold, 10 coins). The Local platform
    //   (LocalStorage) keeps saves in <project>/SavesDir/<name>.sav; the hook below moves the saves there aside (like A4's
    //   fresh profile) and copies the fixture in, then the title's first button is Continue. Replaying the tutorial (A4,
    //   ~8 min) would reach the same lounge.
    // - Craft (Antikythera 700002) and artifact deal (customers 6000xx) are not in the lounge right after the tutorial:
    //   Antikythera unlocks with lounge quest 82100 (after campaign 1101), customers come with the tycoon (user level 3).
    //   The test places each with the game's own spawn call (GameObjectManager.SpawnNpc, what the cheat menu's "Spawn NPC"
    //   calls) next to the player and then uses it like the others.
    // - The fixture owns no artifact, so the deal would only say "No suitable goods available". Before talking to the
    //   customer the test gives one artifact of a category the deal wants with the cheat menu's equipment grant
    //   (SROptions.AddInventoryEquipment -> ReqCheatAddGoods), then negotiates and seals the deal.
    public class A3_NpcFlow
    {
        const string Fixture = "tools/fixtures/saves-post-tutorial";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void PrepareSaves()
        {
            if (!Flow.RunFor("A3_NpcFlow")) return;
            Application.logMessageReceivedThreaded -= IgnoreBootErrors;
            Application.logMessageReceivedThreaded += IgnoreBootErrors;
            Flow.FreshProfile();
            string dir = Path.Combine(Directory.GetParent(Application.dataPath).FullName, "SavesDir");
            Directory.CreateDirectory(dir);
            foreach (var f in Directory.GetFiles(Fixture, "*.sav"))
                File.Copy(f, Path.Combine(dir, Path.GetFileName(f)), true);
            Debug.LogWarning($"VD_A3 saves from {Fixture}: {string.Join(",", Directory.GetFiles(Fixture, "*.sav").Select(Path.GetFileName))}");
            Flow.UseLocalPlatform();
        }

        // Errors logged while the game boots, before the test body runs, would fail the test (same as A1/A4).
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

        // What each review key needs: the NPC, the function type (ENpcFunctionType name) and the presenter that opens.
        // Order: lounge quest 80200 (Lua LoungeQuest/80200) has Elara send the player to Narcis, Felix and Lucas first; until
        // they are talked to, F on Elara plays her quest line instead of her menu (Quest dialogs skip the selection popup).
        static readonly (string key, int npc, string function, string presenter, bool spawn)[] Visits =
        {
            ("npc-character-select", 700013, "CharacterSelect", "CharacterSettingPopupPresenter", false),  // Narcis
            ("npc-character-select-02", 700008, "CharacterSkill", "SkillSelectPopupPresenter", false),    // Felix
            ("npc-combat-trait", 700008, "Talent", "TalentPopupPresenter", false),                // Felix, Talent group 1
            ("npc-trade", 700001, "Shop", "ShopPopupPresenter", false),                            // Lucas, General Store
            ("npc-storage-02", 700014, "Storage", "StoragePopupPresenter", false),                 // Duster
            ("npc-campaign-select", 700000, "Campaign", "CampaignPopupPresenter", false),          // Elara
            ("npc-storage", 700006, "Stash", "MenuPopupPresenter", false),                         // Edman
            ("npc-craft", 700002, "Crafting", "CraftingPopupPresenter", true),                     // Antikythera
            ("npc-artifact-deal", 600001, "ArtifactDeal", "ArtifactDealPopupPresenter", true),     // customer 600001
        };

        [UnityTest, Timeout(1800000)]
        public IEnumerator UseLoungeNpcs() { yield return Flow.GameLog.Run("UseLoungeNpcs", UseLoungeNpcsBody()); }

        // ignoreFailingMessages only keeps Unity from failing at the first logged error; Flow.GameLog fails the test with the list.
        IEnumerator UseLoungeNpcsBody()
        {
            LogAssert.ignoreFailingMessages = true;
            Flow.Tag = "VD_A3";
            Flow.Log($"start platform={Flow.Platform()}");
            SceneManager.LoadScene(0);

            MonoBehaviour title = null;
            yield return Flow.WaitUntil(() => (title = Flow.Find("StartSceneView")) != null
                && (Flow.Get(title, "Buttons") as GameObject)?.activeInHierarchy == true
                && Flow.Ready(Flow.Button(title, "Play_Button")), 120f, "title menu");
            Assert.IsTrue(Flow.LastWaitOk, "title menu did not come up");
            string play = Flow.Get(Flow.Get(title, "Play_Button_Text"), "text") as string;
            Flow.Log($"title play='{play}' slot='{Flow.Get(Flow.Get(title, "SelectSlot_Player_Name"), "text")}'");
            yield return Flow.Click(title, "Play_Button");

            // Continue: on the Local platform the dev room popup (127.0.0.1:7777) comes first; confirm whatever asks.
            float start = Time.realtimeSinceStartup;
            while (Flow.ActiveScene != "LoungeScene" && Time.realtimeSinceStartup - start < 180f)
            {
                foreach (var (view, button) in new[] { ("LocalRoomCreationPopupView", "BtnConfirm_Button"), ("RoomDifficultyPopupView", "Confirm_Button"), ("TextPopupView", "Confirm_Button") })
                    if (Flow.Find(view) != null && Flow.Ready(Flow.Button(Flow.Find(view), button)))
                    {
                        Flow.Log($"continue: {view}");
                        yield return Flow.Click(Flow.Find(view), button);
                    }
                Assert.IsNull(Flow.Find("InputNamePopupView"), "the title started a new game: the fixture saves did not load");
                yield return new WaitForSecondsRealtime(0.5f);
            }
            Assert.AreEqual("LoungeScene", Flow.ActiveScene, "Continue did not reach the lounge");
            yield return Flow.WaitUntil(() => Flow.StageState() == "Playing", 240f, "lounge Playing");
            Assert.IsTrue(Flow.LastWaitOk, $"lounge load stuck: {Flow.LoadState()}");
            yield return new WaitForSecondsRealtime(2f);
            if (Flow.Find("DialogPopupView") != null)
                yield return AdvanceDialogs(() => Flow.Find("DialogPopupView") == null, 120f, "lounge arrival dialog");
            Flow.Log($"lounge {Flow.LoadState()} tutorial={Flow.Get(Flow.Get(Flow.Find("GameManager"), "_gameContext"), "IsTutorial")}");

            Flow.RouteInputToGame();
            foreach (var n in Npcs()) Flow.Log($"npc {n.name} id={NpcId(n)} pos={n.transform.position} functions=[{FunctionList(n)}]");

            // Elara first: her quest line (80200) puts the "talk to me" dialogs on Narcis, Felix and Lucas.
            var elara = FindNpc(700000);
            if (elara != null)
            {
                yield return Interact(elara, "Elara quest 80200");
                yield return Flow.WaitUntil(() => Flow.Find("DialogPopupView") != null || Flow.Find("SelectionPopupView") != null, 4f, "Elara reaction");
                if (Flow.Find("DialogPopupView") != null) yield return AdvanceDialogs(() => Flow.Find("DialogPopupView") == null, 120f, "Elara quest 80200");
                else if (Flow.Find("SelectionPopupView") != null) yield return Flow.Keys(0.2f, Key.Escape);
                yield return new WaitForSecondsRealtime(1f);
            }

            var reached = new List<string>();
            var missed = new List<string>();
            foreach (var v in Visits)
            {
                bool ok = false;
                yield return Visit(v.key, v.npc, v.function, v.presenter, v.spawn, r => ok = r);
                (ok ? reached : missed).Add(v.key);
            }
            Flow.Log($"end reached=[{string.Join(",", reached)}] missed=[{string.Join(",", missed)}]");
            Assert.IsEmpty(missed, $"functions not reached: {string.Join(", ", missed)}");
        }

        // ---- one NPC function ----

        static IEnumerator Visit(string key, int npcId, string function, string presenter, bool spawn, Action<bool> result)
        {
            Flow.Log($"visit {key}: npc {npcId} {function}");
            var npc = FindNpc(npcId);
            if (npc == null && spawn)
            {
                Spawn(npcId);
                yield return Flow.WaitUntil(() => FindNpc(npcId) != null && FunctionIds(FindNpc(npcId)).Length > 0, 10f, $"npc {npcId} spawned");
                npc = FindNpc(npcId);
                if (npc != null) Flow.Log($"spawned {npc.name} at {npc.transform.position} functions=[{FunctionList(npc)}]");
            }
            if (npc == null) { Flow.Log($"visit {key}: npc {npcId} not in the lounge"); result(false); yield break; }
            Flow.Log($"visit {key}: before {SensorState(npc)}");
            int functionId = FunctionIds(npc).FirstOrDefault(id => FunctionType(id) == function);
            if (functionId == 0) { Flow.Log($"visit {key}: npc {npcId} has no {function} (functions=[{FunctionList(npc)}])"); result(false); yield break; }
            // The deal popup lists the artifacts owned when it opens (ArtifactDealPopupPresenter.OnOpenAsync); the fixture
            // inventory has none, so give one first.
            if (function == "ArtifactDeal")
            {
                bool granted = false;
                yield return GrantDealArtifact(functionId, r => granted = r);
                if (!granted) { result(false); yield break; }
            }

            bool opened = false;
            for (int attempt = 0; attempt < 4 && !opened; attempt++)
            {
                yield return Interact(npc, $"{key} npc {npcId}");
                yield return Flow.WaitUntil(() => Flow.Find("SelectionPopupView") != null || Flow.Find("DialogPopupView") != null || Flow.Find(presenter) != null, 4f, $"{key} reaction");
                if (Flow.Find("DialogPopupView") != null)
                {
                    // A quest or story line the NPC says before its menu (OnNpcInteractionAsync plays those first).
                    yield return AdvanceDialogs(() => Flow.Find("DialogPopupView") == null, 120f, $"{key} dialog");
                    yield return new WaitForSecondsRealtime(1f);
                    continue;
                }
                var selection = Flow.Find("SelectionPopupView");
                if (selection != null)
                {
                    yield return new WaitForSecondsRealtime(0.8f);
                    if (key == "npc-campaign-select") yield return Review.Shot("npc-selection");
                    yield return Select(functionId, npc, key);
                }
                yield return Flow.WaitUntil(() => Flow.Find(presenter) != null, 8f, $"{key} {presenter}");
                opened = Flow.LastWaitOk;
                if (!opened) Flow.Log($"visit {key}: {presenter} did not open, top={TopPopup()}");
            }
            if (!opened) { result(false); yield break; }

            yield return new WaitForSecondsRealtime(2f);  // open tweens, scroller fill
            Flow.Log($"visit {key}: open {presenter} {PopupState()}");
            // The ref frames show the panel after the player's first click: the campaign's chapter detail (Hard, rewards,
            // Start Campaign) and a selected trait node with its info and Activate button.
            if (key == "npc-campaign-select")
            {
                yield return Review.Shot(key + "-list");
                yield return ClickSlot("CampaignSlotPresenter", key);
                yield return Flow.WaitUntil(() => Flow.Find("CampaignDetailPanelPresenter") != null, 5f, $"{key} detail panel");
                yield return new WaitForSecondsRealtime(1.5f);
            }
            else if (key == "npc-combat-trait")
            {
                yield return ClickSlot("TalentSlotPresenter", key);
                yield return new WaitForSecondsRealtime(1f);
            }
            else if (key == "npc-artifact-deal")
            {
                bool dealt = false;
                yield return Negotiate(key, r => dealt = r);
                result(dealt);
                yield break;
            }
            yield return Review.Shot(key);
            yield return Close(presenter, key);
            result(true);
        }

        // Hold F at the NPC once its outline focus is on (CharacterView.RxInteractiveObject); NPC HoldingTime is 0.5 s.
        static IEnumerator Interact(Component npc, string what)
        {
            yield return WalkTo(() => npc ? npc.transform.position : (Vector3?)null, 0.5f, 20f, what);
            var view = Flow.Get(Flow.Find("GameManager"), "MyCharacterView");
            // The character's interaction sensor is a 0.18 m capsule trigger and an NPC's Interactable capsule is 0.85 m, so the
            // character has to come within ~1 m (the NavMesh reaches 0.5 m of the counter NPCs). If the path end is still
            // short of that, keep pushing toward the NPC like a player.
            float push = Time.realtimeSinceStartup;
            while (!ReferenceEquals(Flow.Value(view, "RxInteractiveObject"), npc) && Time.realtimeSinceStartup - push < 2.5f && npc)
            {
                var me = Flow.MyCharacter();
                if (me == null) break;
                var d = Flat(npc.transform.position - me.transform.position);
                if (d.magnitude < 0.6f) break;
                float a = (d.z - d.x) / 2f, b = (d.x + d.z) / 2f, m = Mathf.Max(Mathf.Abs(a), Mathf.Abs(b));
                var keys = new List<Key>();
                if (Mathf.Abs(a) > 0.4f * m) keys.Add(a > 0 ? Key.W : Key.S);
                if (Mathf.Abs(b) > 0.4f * m) keys.Add(b > 0 ? Key.D : Key.A);
                Flow.Hold(keys.ToArray());
                yield return new WaitForSecondsRealtime(0.1f);
            }
            Flow.Hold();
            // The focus goes to the interactable nearest the aim point (CharacterView.GetClosestInteractableObject uses the
            // character position plus UnitModel.DeltaAimPos): put the mouse on the NPC as a player does (Lucas stands
            // next to the dive booth).
            var cam = Camera.main;
            if (cam && npc)
            {
                var mouse = Flow.Mouse;
                if (!mouse.enabled) InputSystem.EnableDevice(mouse);
                InputSystem.QueueStateEvent(mouse, new UnityEngine.InputSystem.LowLevel.MouseState { position = (Vector2)cam.WorldToScreenPoint(npc.transform.position + Vector3.up * 0.8f) });
                InputSystem.Update();
            }
            yield return Flow.WaitUntil(() => ReferenceEquals(Flow.Value(view, "RxInteractiveObject"), npc), 3f, $"focus {what}");
            Flow.Log($"interact {what} focus={Focus()}");
            if (!Flow.LastWaitOk) Flow.Log($"interact {what} no focus: {SensorState(npc)}");
            yield return Flow.Keys(0.7f, Key.F);
        }

        // Why the character's interaction focus is not on the NPC: the sensor list (CharacterView._interactiveObjectList),
        // the object registry, CanInteract, the HighObstacle raycast of CharacterView.HasObstacleBetween, the colliders.
        static string SensorState(Component target)
        {
            var gm = Flow.Find("GameManager");
            var view = Flow.Get(gm, "MyCharacterView");
            var me = Flow.MyCharacter();
            var sb = new System.Text.StringBuilder("sensor=[");
            if (Flow.Get(view, "_interactiveObjectList") is IEnumerable list)
                foreach (var o in list) sb.Append((o as Component) ? ((Component)o).name : "null").Append(',');
            sb.Append($"] dist={(me && target ? Flat(target.transform.position - me.transform.position).magnitude : -1):F2} meState={Flow.Get(me, "State")}");
            var om = Flow.Find("GameObjectManager");
            var reg = Flow.Get(om, "InteractiveObjects") as IDictionary;
            var nid = Flow.Get(target, "NetworkObjectId");
            sb.Append($" registered={(reg != null && nid != null ? reg.Contains(nid).ToString() : "?")}");
            var can = target.GetType().GetInterfaces().Select(i => i.GetMethod("CanInteract")).FirstOrDefault(m => m != null);
            var model = Flow.Get(Flow.Get(gm, "MyCharacterController"), "UnitModel") ?? Flow.Get(view, "UnitModel");
            try { sb.Append($" canInteract={can?.Invoke(target, new[] { model })}"); } catch (Exception e) { sb.Append($" canInteract threw {e.InnerException?.GetType().Name}"); }
            sb.Append($" individually={Flow.Get(target, "_isInteractableIndividually")} hasDialog={Flow.Get(target, "HasDialog")} npcState={Flow.Value(target, "RxNpcState")}");
            if (me && target)
            {
                var from = me.transform.position;
                var to = (Vector3)Flow.Get(target, "Position");
                int high = LayerMask.NameToLayer("HighObstacle");
                if (high >= 0 && Physics.Raycast(from, (to - from).normalized, out var hit, Vector3.Distance(from, to), 1 << high))
                    sb.Append($" highObstacle={hit.collider.name}@{hit.point} (npc pos {to})");
            }
            var sensor = Flow.Get(view, "interactionCollider") as Collider;
            if (sensor)
            {
                var rb = sensor.attachedRigidbody;
                sb.Append($" sensorCol={sensor.GetType().Name} en={sensor.enabled && sensor.gameObject.activeInHierarchy} trig={sensor.isTrigger} layer={LayerMask.LayerToName(sensor.gameObject.layer)} b={sensor.bounds.center}/{sensor.bounds.size}");
                sb.Append($" rb={(rb ? $"{rb.name} kin={rb.isKinematic} sleep={rb.IsSleeping()} detect={rb.detectCollisions} rbPos={rb.position} tPos={rb.transform.position}" : "none")}");
                int il = LayerMask.NameToLayer("Interactable");
                sb.Append($" ignore(sensor,Interactable)={Physics.GetIgnoreLayerCollision(sensor.gameObject.layer, il)}");
                var hits = Physics.OverlapBox(sensor.bounds.center, sensor.bounds.extents, Quaternion.identity, 1 << il, QueryTriggerInteraction.Collide);
                sb.Append($" overlap=[{string.Join(",", hits.Select(h => h.name))}]");
            }
            sb.Append($" timeScale={Time.timeScale} sim={Physics.simulationMode} autoSync={Physics.autoSyncTransforms}");
            foreach (var c in target.GetComponentsInChildren<Collider>(true))
                sb.Append($" col {c.name}:{c.GetType().Name} en={c.enabled && c.gameObject.activeInHierarchy} trig={c.isTrigger} layer={LayerMask.LayerToName(c.gameObject.layer)} b={c.bounds.center}/{c.bounds.size}");
            return sb.ToString();
        }

        // Click the selection popup's slot for the function (slot index = position in the NPC's function list).
        static IEnumerator Select(int functionId, Component npc, string key)
        {
            int index = Array.IndexOf(FunctionIds(npc), functionId);
            var slots = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
                .Where(m => m != null && m.isActiveAndEnabled && m.GetType().Name == "SelectionSlotPresenter").ToList();
            Flow.Log($"selection {key}: want index {index} slots=[{string.Join(", ", slots.Select(s => $"{Flow.Get(Flow.Get(s, "_model"), "Index")}:'{Flow.Get(Flow.Get(Flow.Get(s, "_view"), "Text"), "text")}'"))}]");
            var slot = slots.FirstOrDefault(s => Flow.Get(Flow.Get(s, "_model"), "Index") is int i && i == index);
            if (slot == null) { Flow.Log($"selection {key}: no slot {index}"); yield break; }
            var button = Flow.Get(Flow.Get(slot, "_view"), "Button") as UnityEngine.UI.Button;
            yield return Flow.WaitUntil(() => Flow.Ready(button), 5f, $"selection slot {index}");
            yield return new WaitForSecondsRealtime(0.3f);
            button.onClick.Invoke();
        }

        // Click the top-left slot of that presenter type (a chapter in the campaign list, the root trait), as a mouse click
        // on its Button does (BaseSlotPresenter.Button -> OnClick).
        static IEnumerator ClickSlot(string slotType, string key)
        {
            var slot = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
                .Where(m => m != null && m.isActiveAndEnabled && m.GetType().Name == slotType && Flow.Get(m, "Button") is UnityEngine.UI.Button)
                .OrderByDescending(m => Mathf.Round(m.transform.position.y)).ThenBy(m => m.transform.position.x).FirstOrDefault();
            if (slot == null) { Flow.Log($"{key}: no {slotType} to click"); yield break; }
            var button = (UnityEngine.UI.Button)Flow.Get(slot, "Button");
            yield return Flow.WaitUntil(() => Flow.Ready(button), 5f, $"{key} {slotType} {slot.name}");
            Flow.Log($"{key}: click {slotType} {slot.name}");
            button.onClick.Invoke();
        }

        // Esc closes a panel (each popup's Ui/Escape); a second Esc for panels that first leave a sub-page.
        static IEnumerator Close(string presenter, string key)
        {
            for (int i = 0; i < 3 && Flow.Find(presenter) != null; i++)
            {
                yield return Flow.Keys(0.2f, Key.Escape);
                yield return Flow.WaitUntil(() => Flow.Find(presenter) == null, 3f, $"{key} closed by Esc");
            }
            if (Flow.Find(presenter) != null)
            {
                Flow.Log($"close {key}: Esc left {presenter} open, top={TopPopup()} {PopupState()}");
                var p = Flow.Find(presenter);
                p.GetType().GetMethod("Close", new[] { typeof(object) })?.Invoke(p, new object[] { null });
                yield return new WaitForSecondsRealtime(1f);
                Flow.Log($"close {key}: WORKAROUND {presenter}.Close, open={(Flow.Find(presenter) != null)}");
            }
            // Popups opened on top (a confirmation) and the selection popup must be gone before the next NPC.
            yield return Flow.WaitUntil(() => TopPopup() == null, 5f, $"{key} popups gone");
            if (TopPopup() != null) Flow.Log($"close {key}: still open {TopPopup()} {PopupState()}");
            yield return new WaitForSecondsRealtime(0.5f);
        }

        // ---- the game's NPCs and tables, by reflection ----

        static IEnumerable<Component> Npcs() =>
            Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
                .Where(m => m != null && m.isActiveAndEnabled && (m.GetType().Name == "NpcObject" || m.GetType().BaseType?.Name == "NpcObject"));

        static int NpcId(Component n) => Flow.Get(n, "NpcId") is int id ? id : 0;

        // The nearest live NPC of that id (Narcis stands in two places).
        static Component FindNpc(int id)
        {
            var me = Flow.MyCharacter();
            return Npcs().Where(n => NpcId(n) == id)
                .OrderBy(n => me ? Flat(n.transform.position - me.transform.position).sqrMagnitude : 0f).FirstOrDefault();
        }

        static int[] FunctionIds(Component n) => Flow.Get(n, "NpcFunctionIds") as int[] ?? Array.Empty<int>();

        static Type s_tNpcFunction;

        static string FunctionType(int id)
        {
            s_tNpcFunction ??= AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType("GameCommon.Table.TNpcFunction")).FirstOrDefault(t => t != null);
            var table = s_tNpcFunction?.GetProperty("Table")?.GetValue(null);
            var row = table?.GetType().GetProperty("Item")?.GetValue(table, new object[] { id });
            return Flow.Get(row, "Type")?.ToString() ?? "?";
        }

        static string FunctionList(Component n) => string.Join(",", FunctionIds(n).Select(id => $"{id}:{FunctionType(id)}"));

        // A lounge NPC still locked (Antikythera) stands where its lounge spawn data puts it: GameObjectManager.ForceSpawnNpcs,
        // the cheat menu's "force spawn unlocked NPCs". A visitor (customer 6000xx) has no lounge spot:
        // GameObjectManager.SpawnNpc(npcId, position, showSpawnVfx) 2 m off the player, as the cheat menu's Spawn NPC does.
        static void Spawn(int npcId)
        {
            var om = Flow.Find("GameObjectManager");
            var me = Flow.MyCharacter();
            var datas = Flow.Get(om, "NpcSpawnDatas") as IEnumerable;
            if (datas != null && datas.Cast<object>().Any(d => Flow.Get(d, "NpcId") is int id && id == npcId))
            {
                Flow.Log($"spawn {npcId}: ForceSpawnNpcs ({string.Join(",", datas.Cast<object>().Select(d => Flow.Get(d, "NpcId")))})");
                om.GetType().GetMethod("ForceSpawnNpcs")?.Invoke(om, null);
                return;
            }
            var method = om?.GetType().GetMethods().FirstOrDefault(m => m.Name == "SpawnNpc" && m.GetParameters().Length == 3);
            if (method == null || me == null) { Flow.Log($"spawn {npcId}: no GameObjectManager.SpawnNpc (om={om?.GetType().Name})"); return; }
            // The cheat's spot (-2, +2) can land on another NPC (run 1: on Elara, whose quest line then took every F).
            // Take the first NavMesh point 2.5 m around the player with no NPC within 2 m.
            var p = me.transform.position;
            var spot = new Vector3(p.x - 2f, p.y, p.z + 2f);
            for (int i = 0; i < 16; i++)
            {
                float ang = i * Mathf.PI / 8f;
                var c = p + new Vector3(Mathf.Cos(ang), 0f, Mathf.Sin(ang)) * 2.5f;
                if (!UnityEngine.AI.NavMesh.SamplePosition(c, out var hit, 0.3f, UnityEngine.AI.NavMesh.AllAreas)) continue;
                if (Npcs().Any(n => Flat(n.transform.position - hit.position).magnitude < 2f)) continue;
                spot = hit.position;
                break;
            }
            var r = method.Invoke(om, new object[] { npcId, spot, true });
            Flow.Log($"spawn {npcId} at {spot} -> {r}");
        }

        // ---- the artifact deal ----

        static Type GameType(string fullName) =>
            AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType(fullName)).FirstOrDefault(t => t != null);

        static object Row(string tableType, int id)
        {
            var table = GameType(tableType)?.GetProperty("Table")?.GetValue(null);
            try { return table?.GetType().GetProperty("Item")?.GetValue(table, new object[] { id }); }
            catch (System.Reflection.TargetInvocationException) { return null; }
        }

        static IEnumerable<object> InventorySlots()
        {
            var inv = Flow.Get(Flow.Find("GameManager"), "MyInventory");
            return (Flow.Get(Flow.Get(inv, "Slots"), "Values") as IEnumerable)?.Cast<object>() ?? Enumerable.Empty<object>();
        }

        // An artifact the deal accepts, given the way the cheat menu's "장비 지급 (인벤)" does it (SROptions.AddInventoryEquipment:
        // GoodsCreator.MakeEquipmentGoods with TEquipment.PickPrefixes and the room's variant difficulty, sent to the host as
        // ReqCheatAddGoods, which CharacterController.OnCheatAddGoods puts in the inventory with TryGainGoods). The deal
        // (TNpcFunction.Value1 -> TArtifactDeal) takes the categories in PreferCategories, and an owned category of that pool
        // is always one of the dealable ones (ArtifactDealManager.PickPreferCategories).
        static IEnumerator GrantDealArtifact(int functionId, Action<bool> result)
        {
            int dealId = Flow.Get(Row("GameCommon.Table.TNpcFunction", functionId), "Value1") is int v ? v : 0;
            var cats = (Flow.Get(Row("GameCommon.Table.TArtifactDeal", dealId), "PreferCategories") as Array)?.Cast<object>().Select(c => c.ToString()).ToList()
                ?? new List<string>();
            var ext = GameType("GameCommon.Table.Extensions");
            var tEquipment = GameType("GameCommon.Table.TEquipment");
            var isArtifact = ext?.GetMethod("IsArtifact", new[] { tEquipment });
            var rows = (Flow.Get(tEquipment?.GetProperty("Table")?.GetValue(null), "Values") as IEnumerable)?.Cast<object>() ?? Enumerable.Empty<object>();
            var row = rows.Where(r => isArtifact != null && (bool)isArtifact.Invoke(null, new[] { r }) && cats.Contains(Flow.Get(r, "ArtifactCategory")?.ToString()))
                .OrderBy(r => Convert.ToInt32(Flow.Get(r, "Grade"))).ThenBy(r => (int)Flow.Get(r, "Id")).FirstOrDefault();
            Flow.Log($"deal {dealId}: prefer categories=[{string.Join(",", cats)}] artifact={(row == null ? "none" : $"{Flow.Get(row, "Id")} {Flow.Get(row, "ArtifactCategory")} {Flow.Get(row, "Grade")}")}");
            if (row == null) { result(false); yield break; }

            int id = (int)Flow.Get(row, "Id");
            var prefixes = ext.GetMethod("PickPrefixes", new[] { tEquipment })?.Invoke(null, new[] { row }) as int[];
            var variant = (byte)(int)Flow.Get(Flow.Get(Flow.Find("GameManager"), "_gameContext"), "VariantDifficulty");
            var goods = GameType("GameCommon.Data.GoodsCreator").GetMethod("MakeEquipmentGoods")
                .Invoke(null, new object[] { id, 0L, 1L, prefixes, null, null, variant });
            var array = Array.CreateInstance(GameType("GameCommon.Data.Goods"), 1);
            array.SetValue(goods, 0);
            var request = Activator.CreateInstance(GameType("ReqCheatAddGoods"));
            request.GetType().GetProperty("Goods").SetValue(request, array);
            var channel = Flow.Get(Flow.Find("GameManager"), "_gameChannel");
            channel.GetType().GetMethod("SendToHost").Invoke(channel, new[] { request });

            yield return Flow.WaitUntil(() => InventorySlots().Any(s => Flow.Get(s, "Type")?.ToString() == "Artifact"), 5f, $"artifact {id} in the inventory");
            Flow.Log($"deal {dealId}: gave {goods.GetType().Name} {id} prefixes=[{string.Join(",", prefixes ?? Array.Empty<int>())}] -> inventory=[{string.Join(",", InventorySlots().Where(s => Flow.Get(s, "IsEmpty") is false).Select(s => $"{Flow.Get(s, "Type")}:{Flow.Get(s, "Id")}"))}]");
            result(Flow.LastWaitOk);
        }

        // The negotiation as a player plays it: click the artifact in the inventory panel and confirm, wait for the hand of
        // cards, play the first card, then Deal Sealed and confirm; the complete popup follows. Shots: -select (the panel
        // before a pick), the review key (the dealt hand, the ref frame), -card (after one card), -complete.
        static IEnumerator Negotiate(string key, Action<bool> result)
        {
            var popup = Flow.Find("ArtifactDealPopupPresenter");
            var manager = Flow.Get(popup, "_artifactDealManager");
            string State() => Flow.Value(manager, "RxDealState")?.ToString();
            Flow.Log($"{key}: state={State()} categories=[{string.Join(",", ((Flow.Get(manager, "Categories") as IEnumerable)?.Cast<object>() ?? Enumerable.Empty<object>()))}]");
            yield return Review.Shot(key + "-select");

            var slot = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
                .Where(m => m != null && m.isActiveAndEnabled && m.GetType().Name == "InventoryGoodsSlotPresenter")
                .FirstOrDefault(m =>
                {
                    var model = Flow.Get(m, "_model");
                    var goodsSlot = Flow.Value(model, "RxGoodsSlot");
                    return goodsSlot != null && Flow.Get(goodsSlot, "IsEmpty") is false && Flow.Get(goodsSlot, "Type")?.ToString() == "Artifact"
                        && Flow.Get(model, "SlotCategory")?.ToString() is "InventorySlot" or "StashSlot" or "SafeSlot";
                });
            if (slot == null) { Flow.Log($"{key}: no artifact slot in the deal popup"); result(false); yield break; }
            var button = Flow.Get(slot, "Button") as UnityEngine.UI.Button;
            yield return Flow.WaitUntil(() => Flow.Ready(button), 5f, $"{key} artifact slot");
            Flow.Log($"{key}: click artifact slot {slot.name} ({Flow.Get(Flow.Get(slot, "_model"), "SlotCategory")})");
            button.onClick.Invoke();
            yield return Confirm($"{key} pick");
            yield return Flow.WaitUntil(() => State() == "AwaitAction", 20f, $"{key} hand dealt");
            Flow.Log($"{key}: state={State()} hand=[{string.Join(",", ((Flow.Get(manager, "Hand") as IEnumerable)?.Cast<object>() ?? Enumerable.Empty<object>()))}]");
            if (!Flow.LastWaitOk) { result(false); yield break; }
            yield return new WaitForSecondsRealtime(1.5f);
            yield return Review.Shot(key);

            var card = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
                .Where(m => m != null && m.isActiveAndEnabled && m.GetType().Name == "ArtifactDealCardPlayPresenter" && Flow.Ready(Flow.Get(m, "NavButton") as UnityEngine.UI.Button))
                .OrderBy(m => m.transform.position.x).FirstOrDefault();
            if (card != null)
            {
                Flow.Log($"{key}: play card {card.transform.parent?.name}/{card.name}");
                (Flow.Get(card, "NavButton") as UnityEngine.UI.Button).onClick.Invoke();
                yield return new WaitForSecondsRealtime(0.5f);
                yield return Flow.WaitUntil(() => State() is "AwaitAction" or "AwaitActionWithNoCount", 20f, $"{key} card resolved");
                Flow.Log($"{key}: after card state={State()} price={manager.GetType().GetMethod("GetDealPrice")?.Invoke(manager, null)}");
                yield return new WaitForSecondsRealtime(1.5f);
                yield return Review.Shot(key + "-card");
            }
            else Flow.Log($"{key}: no playable card");

            var view = Flow.Find("ArtifactDealPopupView");
            var deal = Flow.Button(view, "DealButtonLayout_Button");
            yield return Flow.WaitUntil(() => Flow.Ready(deal), 5f, $"{key} Deal Sealed");
            Flow.Log($"{key}: Deal Sealed");
            deal.onClick.Invoke();
            yield return Confirm($"{key} deal");
            yield return Flow.WaitUntil(() => Flow.Find("ArtifactDealCompletePopupView") != null, 20f, $"{key} complete popup");
            Flow.Log($"{key}: complete={(Flow.Find("ArtifactDealCompletePopupView") != null)} state={State()} {PopupState()}");
            if (!Flow.LastWaitOk) { result(false); yield break; }
            yield return new WaitForSecondsRealtime(2f);
            yield return Review.Shot(key + "-complete");
            yield return Close("ArtifactDealCompletePopupPresenter", key + "-complete");
            result(true);
        }

        // The two-button TextPopup (PopupManager.ShowConfirmationTextPopupAsync): press Confirm.
        static IEnumerator Confirm(string what)
        {
            yield return Flow.WaitUntil(() => Flow.Find("TextPopupView") != null && Flow.Ready(Flow.Button(Flow.Find("TextPopupView"), "Confirm_Button")), 5f, $"{what} confirmation");
            if (!Flow.LastWaitOk) { Flow.Log($"{what}: no confirmation, top={TopPopup()}"); yield break; }
            yield return new WaitForSecondsRealtime(0.3f);
            Flow.Button(Flow.Find("TextPopupView"), "Confirm_Button").onClick.Invoke();
            yield return Flow.WaitUntil(() => Flow.Find("TextPopupView") == null, 3f, $"{what} confirmation closed");
        }

        // ---- copied from A4_TutorialFlow (walking, focus, dialogs, popups) ----

        static Vector3 Flat(Vector3 v) => new Vector3(v.x, 0f, v.z);

        static string Focus()
        {
            var view = Flow.Get(Flow.Find("GameManager"), "MyCharacterView");
            var o = Flow.Value(view, "RxInteractiveObject") as Component;
            return o == null ? "-" : $"{o.GetType().Name}:{o.name}";
        }

        static string TopPopup()
        {
            foreach (var m in Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None))
                if (m != null && m.isActiveAndEnabled && m.GetType().Name.EndsWith("PopupView") && m.GetType().Name != "RadioToastView") return m.GetType().Name;
            return null;
        }

        static string PopupState()
        {
            var pm = Flow.Find("PopupManager");
            var list = Flow.Get(pm, "_popups") as IEnumerable;
            var sb = new System.Text.StringBuilder("popups=[");
            if (list != null) foreach (var p in list) sb.Append($"{p?.GetType().Name},");
            sb.Append($"] blur={Shader.GetGlobalFloat("_ScreenBlurProgress"):F2}");
            return sb.ToString();
        }

        // WASD toward a world point along the NavMesh (W = world (-x,+z), D = (+x,+z)).
        static IEnumerator WalkTo(Func<Vector3?> target, float arrive, float timeout, string what)
        {
            float start = Time.realtimeSinceStartup;
            Vector3 last = Vector3.zero;
            int stuck = 0, unstick = 0;
            while (Time.realtimeSinceStartup - start < timeout)
            {
                var me = Flow.MyCharacter();
                var t = target();
                if (me == null || t == null) break;
                Vector3 goal = t.Value - me.transform.position;
                goal.y = 0;
                if (goal.magnitude <= arrive) break;
                Vector3 d = goal;
                var path = new UnityEngine.AI.NavMeshPath();
                Vector3 dest = t.Value;
                if (UnityEngine.AI.NavMesh.SamplePosition(dest, out var near, 2.5f, UnityEngine.AI.NavMesh.AllAreas)) dest = near.position;
                if (UnityEngine.AI.NavMesh.CalculatePath(me.transform.position, dest, UnityEngine.AI.NavMesh.AllAreas, path) && path.corners.Length > 1)
                {
                    int c = 1;
                    while (c < path.corners.Length - 1 && Vector3.Distance(Flat(path.corners[c]), Flat(me.transform.position)) < 0.4f) c++;
                    d = path.corners[c] - me.transform.position;
                    d.y = 0;
                    // The NavMesh point nearest an NPC standing in an obstacle is as close as a player gets.
                    if (c == path.corners.Length - 1 && d.magnitude < 0.3f) break;
                }
                if (Flow.Find("DialogPopupView") != null) { Flow.Hold(); yield break; }
                float a = (d.z - d.x) / 2f, b = (d.x + d.z) / 2f, m = Mathf.Max(Mathf.Abs(a), Mathf.Abs(b));
                var keys = new List<Key>();
                if (Mathf.Abs(a) > 0.4f * m) keys.Add(a > 0 ? Key.W : Key.S);
                if (Mathf.Abs(b) > 0.4f * m) keys.Add(b > 0 ? Key.D : Key.A);
                Flow.Hold(keys.ToArray());
                yield return new WaitForSecondsRealtime(0.15f);
                stuck = (me.transform.position - last).sqrMagnitude < 0.0004f ? stuck + 1 : 0;
                last = me.transform.position;
                if (stuck > 14)
                {
                    // Caught on a corner the WASD diagonals cannot follow: step sideways once or twice, then go on.
                    if (++unstick > 2) { Flow.Log($"walk {what} stuck at {last}"); break; }
                    Flow.Log($"walk {what} stuck at {last}, side step {unstick}");
                    var side = unstick == 1 ? new[] { Key.A } : new[] { Key.D };
                    Flow.Hold(keys.Contains(Key.W) || keys.Contains(Key.S) ? side : (unstick == 1 ? new[] { Key.W } : new[] { Key.S }));
                    yield return new WaitForSecondsRealtime(0.4f);
                    stuck = 0;
                }
            }
            Flow.Hold();
            yield return null;
            var end = Flow.MyCharacter();
            Flow.Log($"walk {what} me={(end ? end.transform.position.ToString() : "-")} target={target()} after {Time.realtimeSinceStartup - start:F1}s");
        }

        static bool DialogDelaying()
        {
            var p = Flow.Find("DialogPopupPresenter");
            return p != null && Flow.Get(p, "_isDelayActive") is bool b && b;
        }

        static string DialogEntry() => Flow.Get(Flow.Get(Flow.Find("DialogPopupPresenter"), "_currentEntry"), "Message") as string;

        // Click through dialog lines (first click shows the whole line, the next moves on) until done() holds.
        static IEnumerator AdvanceDialogs(Func<bool> done, float timeout, string what)
        {
            float start = Time.realtimeSinceStartup;
            int clicks = 0;
            string lastLine = null;
            while (!done() && Time.realtimeSinceStartup - start < timeout)
            {
                var note = Flow.Find("NoteSystemPopupView");
                var dialog = Flow.Find("DialogPopupView");
                if (note != null && Flow.Ready(Flow.Button(note, "BaseBgDimmer")))
                {
                    Flow.Button(note, "BaseBgDimmer").onClick.Invoke();
                    clicks++;
                }
                else if (dialog != null && Flow.Ready(Flow.Button(dialog, "Background")) && !DialogDelaying())
                {
                    string line = DialogEntry();
                    if (line != lastLine) { Flow.Log($"dialog {what}: '{line}'"); lastLine = line; }
                    Flow.Button(dialog, "Background").onClick.Invoke();
                    clicks++;
                }
                yield return new WaitForSecondsRealtime(0.35f);
            }
            Flow.Log($"advance {what} done={done()} clicks={clicks} after {Time.realtimeSinceStartup - start:F1}s");
        }
    }
}
