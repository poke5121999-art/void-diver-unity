using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace VD.PlayTests
{
    // A4: a fresh profile presses New Game on the title and plays into the tutorial (campaign 1100), like docs/PLAN.md
    // "Luồng từ title vào game": name, difficulty, room, lounge prologue, tutorial stage. Captures use docs/review/ref keys.
    public class A4_TutorialFlow
    {
        [TearDown]
        public void Restore()
        {
            Flow.RestoreInput();
            Flow.RestorePlatform();
        }

        [UnityTest, Timeout(2400000)]  // the runner's default is 180 s; boot, prologue and stage take longer
        public IEnumerator NewGameToTutorial()
        {
            LogAssert.ignoreFailingMessages = true;
            Flow.Tag = "VD_A4";
            Flow.Log($"start platform={Flow.Platform()}");
            SceneManager.LoadScene(0);

            // Title: StartScene turns the menu on once input is enabled (SetMenuActive).
            MonoBehaviour title = null;
            yield return Flow.WaitUntil(() => (title = Flow.Find("StartSceneView")) != null
                && (Flow.Get(title, "Buttons") as GameObject)?.activeInHierarchy == true
                && Flow.Ready(Flow.Button(title, "Play_Button")), 120f, "title menu");
            Assert.IsTrue(Flow.LastWaitOk, "title menu did not come up");
            var nm = Flow.Find("NetworkManager");
            var transport = Flow.Get(Flow.Get(nm, "NetworkConfig"), "NetworkTransport");
            Flow.Log($"title play='{Flow.Get(Flow.Get(title, "Play_Button_Text"), "text")}' slot='{Flow.Get(Flow.Get(title, "SelectSlot_Player_Name"), "text")}' transport={transport?.GetType().Name}");
            // A player has moved the mouse by now. InputIcons (defaultStartDeviceType Auto) has no current device until an
            // input action performs, and every II_DeviceBasedObjectController for keyboard (the HUD's KeyGuidePanel with
            // "Control Guide O", the sector's TutorialGuides/Keyboard floor guides "Move + Look") stays hidden until then.
            yield return MoveMouseLikeAPlayer(new Vector2(0.62f, 0.41f));
            yield return Flow.Click(title, "Play_Button");

            // Name, then the "use this name?" confirmation.
            yield return Flow.WaitView("InputNamePopupView", 30f);
            Assert.IsTrue(Flow.LastWaitOk, "name popup did not open after New Game");
            var nameView = Flow.Find("InputNamePopupView");
            yield return new WaitForSecondsRealtime(1f);
            Flow.Type(Flow.Get(nameView, "InputField"), "Diver");
            yield return Review.Shot("a4-name");
            yield return Flow.Click(nameView, "Confirm_Button");
            yield return Flow.WaitView("TextPopupView", 20f);
            Assert.IsTrue(Flow.LastWaitOk, "name confirmation did not open");
            yield return Flow.Click(Flow.Find("TextPopupView"), "Confirm_Button");

            // Difficulty (default Junior).
            yield return Flow.WaitView("RoomDifficultyPopupView", 30f);
            Assert.IsTrue(Flow.LastWaitOk, "difficulty popup did not open");
            yield return new WaitForSecondsRealtime(1f);
            yield return Review.Shot("a4-difficulty");
            yield return Flow.Click(Flow.Find("RoomDifficultyPopupView"), "Confirm_Button");

            // Local platform only: the dev room popup with 127.0.0.1:7777 filled in.
            yield return Flow.WaitUntil(() => Flow.Find("LocalRoomCreationPopupView") != null || Flow.ActiveScene == "LoungeScene", 60f, "room popup or lounge");
            var room = Flow.Find("LocalRoomCreationPopupView");
            if (room != null) yield return Flow.Click(room, "BtnConfirm_Button");

            yield return Flow.WaitUntil(() => Flow.ActiveScene == "LoungeScene", 180f, "LoungeScene");
            Assert.IsTrue(Flow.LastWaitOk, "lounge did not load");
            Flow.Log($"lounge {Flow.LoadState()}");
            yield return Flow.WaitUntil(() => Flow.StageState() == "Playing" || Flow.Find("DialogPopupView") != null, 240f, "lounge Playing or prologue dialog");
            Flow.Log($"lounge {Flow.LoadState()}");
            yield return Review.Shot("a4-lounge-load");
            Assert.IsTrue(Flow.LastWaitOk, $"lounge load stuck: {Flow.LoadState()}");

            // Prologue (Lua Campaign/1100 OnLounge): dialog lines and the 7-page letter, advanced by clicking, until the
            // script calls ForceStartStage.
            yield return AdvanceDialogs(() => Flow.ActiveScene == "StageScene", 900f, "prologue", "a4-prologue", "a4-prologue-letter");
            Assert.AreEqual("StageScene", Flow.ActiveScene, "prologue did not lead to the tutorial stage");

            yield return Flow.WaitUntil(() => Flow.StageState() == "Playing", 240f, "stage Playing");
            Flow.DumpStage();

            // Tutorial start: Step_00003 opens the intro dialog (Gayoung's lines over a dark background), then Kenoma's
            // "Hey, wait!" (LQ_1100_214) over the lit start room with the "Move + Look" floor guide (ref tutorial-start/01),
            // then the HUD comes back.
            yield return Flow.WaitUntil(() => Flow.Find("DialogPopupView") != null, 30f, "tutorial dialog");
            if (Flow.LastWaitOk)
            {
                yield return new WaitForSecondsRealtime(2f);
                yield return Review.Shot("a4-tutorial-intro");
                yield return AdvanceDialogs(() => Flow.Find("DialogPopupView") == null, 120f, "tutorial intro", null, null,
                    matchText: "Hey, wait", matchShot: "tutorial-start", matchDelay: 0.3f);
            }
            else yield return Review.Shot("a4-tutorial-intro");
            Flow.Log($"tutorial guides {GuideState()}");
            yield return new WaitForSecondsRealtime(3f);
            yield return Review.Shot("a4-tutorial-hud");
            Flow.Log($"tutorial hud guides {GuideState()}");
            Flow.DumpStage();
            DumpNavMesh("a4-navmesh-start");

            // Walk: learn how WASD moves the character on this map, and look for Elara's radio hints and monsters.
            Flow.RouteInputToGame();
            bool radioShot = false, combatShot = false;
            // One diagonal step logs how the keys reach the game (device, action map, controller).
            Flow.Hold(Key.W);
            yield return new WaitForSecondsRealtime(0.5f);
            Flow.Log($"input W {InputState()}");
            Flow.Hold();

            // Elara's terminal next to the start point (InteractiveTrigger 11002, Lua TalkToEll_1): walk up, press F, and
            // her radio line comes up (RadioSay -> RadioToast), as the ref tutorial-dialog frame shows.
            var terminal = FindTrigger("InteractiveTrigger", 11002);
            if (terminal != null)
            {
                yield return WalkTo(() => terminal ? terminal.transform.position : (Vector3?)null, 1.0f, 15f, "Elara terminal");
                yield return Flow.Keys(0.6f, Key.F);
                yield return Flow.WaitUntil(() => Flow.Find("RadioToastView") != null || Flow.Find("DialogPopupView") != null, 6f, "radio toast (terminal)");
                if (Flow.Find("RadioToastView") != null)
                {
                    yield return new WaitForSecondsRealtime(1.5f);
                    radioShot = true;
                    yield return Review.Shot("tutorial-dialog");
                }
            }

            // The route of Lua Campaign/1100 through the 2x2 tutorial map (TCampaign 1100 FixedSectors, TSector spawn data):
            // glass door 1004 out of the start room (F), trigger 100091 (monsters nearby), the three 230001 monsters, the
            // dead man's box 1002 (key 300000), steel gate 2005 (key), traps (100092), boxes (100093), stress 80 and
            // shadow monsters (100094), vending machine (100095), battery out (100096), Elara 11003, boss cutscene 100097,
            // forced escape to the lounge.
            yield return Go(new Vector3(19.4f, 0f, 21.5f), 1.0f, 40f, "trigger 100091");
            yield return WaitRadio(radioShot ? null : "tutorial-dialog", 6f, "100091");

            // First fight: the 230001 monsters in the west room.
            yield return Go(new Vector3(13f, 0f, 15f), 1.5f, 40f, "monster room", stopNearMonster: 5f);
            yield return Fight(12f, 90f, "first combat", "tutorial-first-combat");
            combatShot = s_fightShot;

            // Dead man's box 1002 by trigger 100099, then the steel gate 2005 with its key.
            yield return Go(new Vector3(11.14f, 0f, 11.79f), 0.8f, 30f, "trigger 100099");
            yield return OpenBox(new Vector3(10.96f, 0f, 12.01f), "box 1002");
            yield return Go(new Vector3(7.6f, 0f, 14.1f), 0.8f, 30f, "gate 2005");
            yield return WaitRadio(null, 4f, "100098");

            // North sector 10001: fire traps (run), boxes, stress, vending machine.
            yield return Go(new Vector3(3.15f, 0f, 31.19f), 1.0f, 60f, "trigger 100092");
            yield return WaitRadio(null, 4f, "100092");
            yield return Go(new Vector3(7.95f, 0f, 51.47f), 1.5f, 90f, "trigger 100093", run: true);
            yield return WaitRadio(null, 4f, "100093");
            yield return Go(new Vector3(19.73f, 0f, 45.21f), 1.0f, 60f, "trigger 100094");
            yield return WaitRadio("tutorial-dialog", 6f, "100094");
            yield return Fight(10f, 60f, "shadow monsters", null, waitAppear: 15f);
            yield return Go(new Vector3(26.6f, 0f, 32.83f), 1.0f, 60f, "trigger 100095");
            yield return WaitRadio(null, 4f, "100095");

            // East sector 10004: battery, Elara 11003, the boss cutscene trigger.
            yield return Go(new Vector3(34.79f, 0f, 36.59f), 1.5f, 60f, "trigger 100096");
            var elara = FindTrigger("InteractiveTrigger", 11003);
            if (elara != null)
            {
                yield return Interact(elara, 0.6f, "Elara 11003");
                yield return Flow.WaitUntil(() => Flow.Find("DialogPopupView") != null, 6f, "Elara dialog");
                if (Flow.LastWaitOk) yield return AdvanceDialogs(() => Flow.Find("DialogPopupView") == null, 120f, "Elara dialog", null, null);
            }
            yield return Go(new Vector3(48.43f, 0f, 44.03f), 0.8f, 60f, "trigger 100097");

            // Boss cutscene, dialog, escape booth, ForceEscapeStage -> lounge (OnLounge step 10 = QuestComplete).
            float bossStart = Time.realtimeSinceStartup;
            while (Flow.ActiveScene != "LoungeScene" && Time.realtimeSinceStartup - bossStart < 300f)
            {
                if (Flow.Find("DialogPopupView") != null)
                    yield return AdvanceDialogs(() => Flow.Find("DialogPopupView") == null || Flow.ActiveScene == "LoungeScene", 120f, "boss dialog", null, null);
                yield return new WaitForSecondsRealtime(0.5f);
            }
            Flow.Log($"after boss scene={Flow.ActiveScene} {Flow.LoadState()}");
            if (Flow.ActiveScene == "LoungeScene")
            {
                yield return Flow.WaitUntil(() => Flow.StageState() == "Playing" || Flow.Find("DialogPopupView") != null, 240f, "lounge after tutorial");
                // QuestComplete in the lounge: Gayoung tells Elara and the owner she grew up exorcising (LQ_1100_524), the
                // ref lounge-npc-talk frame.
                yield return AdvanceDialogs(() => Flow.StageState() == "Playing" && Flow.Find("DialogPopupView") == null && Flow.Find("NoteSystemPopupView") == null, 300f, "tutorial complete", null, null,
                    matchText: "exorcism", matchShot: "lounge-npc-talk");
                // The campaign-complete card (title, COMPLETED, rewards) comes up over the lounge; a player clicks it away.
                yield return Flow.WaitUntil(() => Flow.Find("CampaignCompletePopupView") != null, 6f, "campaign complete popup");
                var done = Flow.Find("CampaignCompletePopupView");
                if (done != null)
                {
                    // OpenCampaignCompletePopup.anim: the card fades in by 0.67 s, holds to 2.67 s, fades out by 3.33 s and its
                    // OnAnimationEnd event closes it, so at 3 s only the blur behind it was left.
                    yield return new WaitForSecondsRealtime(2f);
                    yield return Review.Shot("a4-campaign-complete");
                    yield return Flow.Click(done, "BG");
                    yield return Flow.WaitUntil(() => Flow.Find("CampaignCompletePopupView") == null, 6f, "campaign complete popup closed");
                }
                Flow.Log($"lounge before arrival shot {PopupState()}");
                yield return new WaitForSecondsRealtime(3f);
                yield return Review.Shot("lounge-arrival");
                Flow.Log($"lounge arrival {Flow.LoadState()} {PopupState()}");
                yield return LoungeTour();
            }
            Flow.DumpStage();
            Flow.Log($"end radio={radioShot} combat={combatShot}");
            Assert.Pass($"reached {Flow.ActiveScene} stage={Flow.StageState()}");
        }

        static bool s_fightShot;

        // A2: free play in the lounge after the tutorial. Walk past the NPCs at the counter and the dive booth like a
        // player (WASD along the lounge NavMesh), then walk up to Elara and press F (InGameScene.OnNpcInteractionAsync:
        // her dialog, or her function selection).
        static IEnumerator LoungeTour()
        {
            Flow.RouteInputToGame();
            var npcs = new System.Collections.Generic.List<Component>();
            foreach (var m in Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None))
                if (m != null && (m.GetType().Name == "NpcObject" || m.GetType().BaseType?.Name == "NpcObject")) npcs.Add(m);
            foreach (var n in npcs)
                Flow.Log($"lounge npc {n.GetType().Name} {n.name} id={Flow.Get(n, "NpcId")} pos={n.transform.position} functions={Flow.Get(n, "HasFunction")} state={Flow.Value(n, "RxNpcState")} onMesh={UnityEngine.AI.NavMesh.SamplePosition(n.transform.position, out _, 2.5f, UnityEngine.AI.NavMesh.AllAreas)}");
            DumpNavMesh("a2-navmesh-lounge");
            var me = Flow.MyCharacter();
            Flow.Log($"lounge tour start {PopupState()}");
            if (me == null || npcs.Count == 0) { Flow.Log($"lounge tour: me={(me != null)} npcs={npcs.Count}"); yield break; }
            Vector3 home = me.transform.position;
            // Visit the NPCs from the nearest outward, stopping short of each as a player passing by does.
            npcs.Sort((a, b) => Flat(a.transform.position - home).sqrMagnitude.CompareTo(Flat(b.transform.position - home).sqrMagnitude));
            int legs = 0;
            bool walkShot = false;
            foreach (var n in npcs)
            {
                if (legs >= 4) break;
                var target = n;
                yield return WalkTo(() => target ? target.transform.position : (Vector3?)null, 2.5f, 15f, $"lounge {n.name}");
                legs++;
                if (!walkShot && legs >= 2) { yield return new WaitForSecondsRealtime(0.5f); yield return Review.Shot("lounge-walk"); walkShot = true; }
            }
            if (!walkShot) yield return Review.Shot("lounge-walk");

            Component elara = null;
            foreach (var n in npcs)
                if (Flow.Get(n, "NpcId") is int id && id == 700000) elara = n;  // TNpc 700000 엘라라 (campaign select)
            if (elara == null)
                foreach (var n in npcs) if (Flow.Get(n, "HasFunction") is bool f && f) { elara = n; break; }
            if (elara == null) { Flow.Log("lounge: no NPC to talk to"); yield break; }
            yield return Interact(elara, 0.3f, $"lounge npc {elara.name}");
            yield return Flow.WaitUntil(() => Flow.Find("DialogPopupView") != null || Flow.Find("SelectionPopupView") != null || TopPopup() != null, 6f, "npc talk");
            yield return new WaitForSecondsRealtime(1.5f);
            Flow.Log($"lounge npc talk: dialog={Flow.Find("DialogPopupView") != null} popup={TopPopup()} {DialogState()}");
            yield return Review.Shot("a2-npc-talk");
            if (Flow.Find("DialogPopupView") != null)
                yield return AdvanceDialogs(() => Flow.Find("DialogPopupView") == null, 120f, "lounge npc dialog", null, null);
            Flow.Log($"lounge tour end {Flow.LoadState()} popup={TopPopup()}");
        }

        // PopupManager's open popups (the screen blur stays on while any has IsBlurEffect) and the blur value.
        static string PopupState()
        {
            var pm = Flow.Find("PopupManager");
            var list = Flow.Get(pm, "_popups") as System.Collections.IEnumerable;
            var sb = new System.Text.StringBuilder("popups=[");
            if (list != null) foreach (var p in list) sb.Append($"{p?.GetType().Name}:blur={Flow.Get(p, "IsBlurEffect")},");
            sb.Append($"] blur={Shader.GetGlobalFloat("_ScreenBlurProgress"):F2}");
            return sb.ToString();
        }

        // The popup on top (any *PopupView active in the scene), by type name.
        static string TopPopup()
        {
            foreach (var m in Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None))
                if (m != null && m.isActiveAndEnabled && m.GetType().Name.EndsWith("PopupView") && m.GetType().Name != "RadioToastView") return m.GetType().Name;
            return null;
        }

        // Walk to a world point like a player: follow the NavMesh, open closed doors on the way with F (Entrance; the
        // closed door's NavMeshObstacle carves the doorway), click through dialogs that come up.
        static IEnumerator Go(Vector3 goal, float arrive, float timeout, string what, float stopNearMonster = 0f, bool run = false)
        {
            float start = Time.realtimeSinceStartup;
            int doors = 0;
            while (Time.realtimeSinceStartup - start < timeout)
            {
                var me = Flow.MyCharacter();
                if (me == null || Flow.ActiveScene != "StageScene") break;
                if (Flat(goal - me.transform.position).magnitude <= arrive) break;
                if (stopNearMonster > 0f && NearMonster(stopNearMonster)) { Flow.Log($"go {what}: monster within {stopNearMonster}"); break; }
                if (Flow.Find("DialogPopupView") != null)
                {
                    yield return AdvanceDialogs(() => Flow.Find("DialogPopupView") == null, 120f, $"dialog on the way to {what}", null, null);
                    continue;
                }
                yield return WalkTo(() => goal, arrive, Mathf.Min(20f, timeout - (Time.realtimeSinceStartup - start)), what, run);
                me = Flow.MyCharacter();
                if (me == null || Flat(goal - me.transform.position).magnitude <= arrive) break;
                if (Flow.Find("DialogPopupView") != null) continue;
                // The closed door the walkable region ends at (the glass door by the start room, the key gate west of
                // the monster room); the partial path can stop well short of it.
                var door = NearestClosedDoor(me.transform.position, 12f);
                if (door != null && doors < 4)
                {
                    doors++;
                    yield return OpenDoor(door);
                    continue;
                }
                if (stopNearMonster > 0f && NearMonster(stopNearMonster)) continue;
                Flow.Log($"go {what}: blocked at {me.transform.position}, no closed door within 12 m ({DoorList(me.transform.position)})");
                break;
            }
            var end = Flow.MyCharacter();
            Flow.Log($"go {what} end me={(end ? end.transform.position.ToString() : "-")} goal={goal} after {Time.realtimeSinceStartup - start:F1}s");
        }

        static string DoorShape(Component door)
        {
            var obs = Flow.Get(door, "_navMeshObstacle") as UnityEngine.AI.NavMeshObstacle;
            var ic = Flow.Get(door, "_interactionCollider") as BoxCollider;
            var hc = Flow.Get(door, "_hitAbleCollider") as BoxCollider;
            return $"fwd={Flow.Value(door, "_syncForward")} obstacle={(obs ? $"{obs.enabled}/{obs.carving} c={obs.transform.TransformPoint(obs.center)} s={obs.size} shape={obs.shape}" : "-")} " +
                   $"interact={(ic ? $"{ic.enabled} b={ic.bounds.center}/{ic.bounds.size} layer={LayerMask.LayerToName(ic.gameObject.layer)}" : "-")} " +
                   $"hit={(hc ? $"{hc.enabled} b={hc.bounds.center}/{hc.bounds.size} layer={LayerMask.LayerToName(hc.gameObject.layer)}" : "-")}";
        }

        // A NavMesh point on either side of the door (its forward axis), the reachable one nearest to the character.
        static Vector3? DoorStand(Component door)
        {
            var me = Flow.MyCharacter();
            var f = Flow.Value(door, "_syncForward") is Vector3 v && v.sqrMagnitude > 0.01f ? Flat(v).normalized : Vector3.forward;
            Vector3? best = null;
            float bestLen = float.MaxValue;
            foreach (float d in new[] { -1.0f, 1.0f, -1.5f, 1.5f })
            {
                var c = door.transform.position + f * d;
                if (!UnityEngine.AI.NavMesh.SamplePosition(c, out var hit, 0.6f, UnityEngine.AI.NavMesh.AllAreas)) { Flow.Log($"door stand {c} off NavMesh"); continue; }
                var path = new UnityEngine.AI.NavMeshPath();
                bool ok = UnityEngine.AI.NavMesh.CalculatePath(me.transform.position, hit.position, UnityEngine.AI.NavMesh.AllAreas, path);
                float len = 0f;
                for (int i = 1; i < path.corners.Length; i++) len += Vector3.Distance(path.corners[i - 1], path.corners[i]);
                Flow.Log($"door stand {hit.position} path={ok}/{path.status} len={len:F1}");
                if (ok && path.status == UnityEngine.AI.NavMeshPathStatus.PathComplete && len < bestLen) { bestLen = len; best = hit.position; }
            }
            return best;
        }

        // Why the character's interaction focus is not on the target: its sensor list, the object registry, line of sight.
        static string SensorState(Component target)
        {
            var view = Flow.Get(Flow.Find("GameManager"), "MyCharacterView");
            var list = Flow.Get(view, "_interactiveObjectList") as System.Collections.IEnumerable;
            var sb = new System.Text.StringBuilder("sensor=[");
            if (list != null) foreach (var o in list) sb.Append((o as Component) ? ((Component)o).name : "null").Append(',');
            var me = Flow.MyCharacter();
            sb.Append($"] dist={(me && target ? Flat(target.transform.position - me.transform.position).magnitude : -1):F2}");
            var om = Flow.Get(view, "ObjectManager") ?? Flow.Get(Flow.Find("GameManager"), "ObjectManager");
            var reg = Flow.Get(om, "InteractiveObjects") as System.Collections.IDictionary;
            var nid = Flow.Get(target, "NetworkObjectId");
            sb.Append($" registered={(reg != null && nid != null ? reg.Contains(nid).ToString() : "?")} state={Flow.Get(me, "State")}");
            if (me && target && Physics.Linecast(me.transform.position + Vector3.up * 0.5f, target.transform.position + Vector3.up * 0.5f, out var hit, 1 << LayerMask.NameToLayer("HighObstacle")))
                sb.Append($" highObstacle={hit.collider.name}@{hit.point}");
            return sb.ToString();
        }

        // The stage NavMesh as text (v x z / f a b c area) plus the doors, triggers, monsters and the character, for a
        // top-down picture of where the walkable regions connect.
        static void DumpNavMesh(string name)
        {
            var tri = UnityEngine.AI.NavMesh.CalculateTriangulation();
            var sb = new System.Text.StringBuilder();
            foreach (var v in tri.vertices) sb.Append($"v {v.x:F2} {v.y:F2} {v.z:F2}\n");
            for (int i = 0; i + 2 < tri.indices.Length; i += 3) sb.Append($"f {tri.indices[i]} {tri.indices[i + 1]} {tri.indices[i + 2]} {tri.areas[i / 3]}\n");
            foreach (var m in Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None))
            {
                if (m == null) continue;
                string n = m.GetType().Name;
                if (n == "Entrance" || n == "CollisionTrigger" || n == "InteractiveTrigger" || n == "MonsterController" || n == "RewardBox")
                    sb.Append($"p {n}:{(Flow.Get(m, "_triggerId") ?? m.name)} {m.transform.position.x:F2} {m.transform.position.z:F2}\n");
            }
            var me = Flow.MyCharacter();
            if (me) sb.Append($"p me {me.transform.position.x:F2} {me.transform.position.z:F2}\n");
            System.IO.Directory.CreateDirectory("captures/review");
            System.IO.File.WriteAllText($"captures/review/{name}.txt", sb.ToString());
            Flow.Log($"navmesh {name}: {tri.vertices.Length} vertices {tri.indices.Length / 3} triangles");
        }

        static string DoorState(Component door) => Flow.Value(door, "_syncLockState")?.ToString() ?? "?";

        static Component NearestClosedDoor(Vector3 from, float range)
        {
            Component best = null;
            float bestD = range;
            foreach (var m in Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None))
                if (m != null && m.GetType().Name == "Entrance" && DoorState(m) != "Opened")
                {
                    float d = Flat(m.transform.position - from).magnitude;
                    if (d < bestD) { bestD = d; best = m; }
                }
            return best;
        }

        static string DoorList(Vector3 from)
        {
            var sb = new System.Text.StringBuilder();
            foreach (var m in Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None))
                if (m != null && m.GetType().Name == "Entrance")
                    sb.Append($"{m.name}@{m.transform.position} {DoorState(m)} d={Flat(m.transform.position - from).magnitude:F1}; ");
            return sb.Length == 0 ? "no Entrance spawned" : sb.ToString();
        }

        static string Focus()
        {
            var view = Flow.Get(Flow.Find("GameManager"), "MyCharacterView");
            var o = Flow.Value(view, "RxInteractiveObject") as Component;
            return o == null ? "-" : $"{o.GetType().Name}:{o.name}";
        }

        // Stand at the object until the character's interaction focus (CharacterView.RxInteractiveObject, the closest
        // interactable in its sensor) is on it, then hold F (Entrance HoldingTime 0.4 s, boxes 0.1 s).
        static IEnumerator Interact(Component target, float hold, string what)
        {
            yield return WalkTo(() => target ? target.transform.position : (Vector3?)null, 1.0f, 12f, what);
            for (int i = 0; i < 3 && target; i++)
            {
                var view = Flow.Get(Flow.Find("GameManager"), "MyCharacterView");
                yield return Flow.WaitUntil(() => ReferenceEquals(Flow.Value(view, "RxInteractiveObject"), target), 2f, $"focus {what}");
                Flow.Log($"interact {what} try={i} focus={Focus()} {SensorState(target)}");
                yield return Flow.Keys(hold, Key.F);
                yield return new WaitForSecondsRealtime(0.6f);
                if (!ReferenceEquals(Flow.Value(view, "RxInteractiveObject"), target)) break;
                if (target.GetType().Name == "Entrance" && DoorState(target) == "Opened") break;
                if (Flow.Find("KeyInteractionPopupView") != null || Flow.Find("InventoryManagementPagePresenter") != null) break;
            }
        }

        static IEnumerator OpenDoor(Component door)
        {
            string before = DoorState(door);
            Flow.Log($"door {door.name} at {door.transform.position} state={before} {DoorShape(door)}");
            DumpNavMesh("a4-navmesh-door");
            var stand = DoorStand(door);
            if (stand != null) yield return WalkTo(() => stand, 0.3f, 12f, $"door {door.name} stand");
            yield return Interact(door, 0.8f, $"door {door.name}");
            if (Flow.Find("KeyInteractionPopupView") != null) yield return UseKey(door.name);
            yield return Flow.WaitUntil(() => !door || DoorState(door) == "Opened", 4f, $"door {door.name} open");
            Flow.Log($"door {door.name} {before} -> {(door ? DoorState(door) : "gone")} focus={Focus()}");
        }

        // KeyInteractionPopup (locked gate): pick the key item from the inventory panel, then Confirm, as a player
        // clicking the key slot and the Confirm button does (TryRegisterItem / OnConfirm are those handlers).
        static IEnumerator UseKey(string what)
        {
            var p = Flow.Find("KeyInteractionPopupPresenter");
            var model = Flow.Get(p, "_model");
            var ids = Flow.Get(model, "KeyItemIds") as int[];
            Flow.Log($"key popup for {what} keys={(ids == null ? "-" : string.Join(",", ids))} inventory: {InventoryIds()}");
            yield return new WaitForSecondsRealtime(1f);
            yield return Review.Shot("a4-key-popup");
            if (ids != null)
                foreach (var id in ids)
                {
                    var r = p.GetType().GetMethod("TryRegisterItem", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public)?.Invoke(p, new object[] { id });
                    Flow.Log($"key register {id} -> {r}");
                }
            yield return new WaitForSecondsRealtime(0.5f);
            p.GetType().GetMethod("OnConfirm", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)?.Invoke(p, null);
            yield return new WaitForSecondsRealtime(1f);
            Flow.Log($"key popup after confirm open={Flow.Find("KeyInteractionPopupView") != null}");
        }

        // Open a reward box with F and move everything in the looting panel into the inventory: hover a slot and
        // left-click it (InventoryGoodsSlotModel OnEnter / OnLeftClick, what the slot's pointer handlers raise).
        static IEnumerator OpenBox(Vector3 near, string what)
        {
            Component box = null;
            float bestD = 3f;
            foreach (var m in Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None))
                if (m != null && m.GetType().Name == "RewardBox" && Flat(m.transform.position - near).magnitude < bestD)
                { bestD = Flat(m.transform.position - near).magnitude; box = m; }
            if (box == null) { Flow.Log($"{what}: no RewardBox near {near}"); yield break; }
            yield return Interact(box, 0.5f, what);
            yield return Flow.WaitUntil(() => Flow.Find("LootingInventoryPanelPresenter") != null, 6f, $"{what} looting panel");
            var panel = Flow.Find("LootingInventoryPanelPresenter");
            if (panel == null) { Flow.Log($"{what}: no looting panel, focus={Focus()}"); yield break; }
            yield return new WaitForSecondsRealtime(1.5f);
            yield return Review.Shot("a4-loot");
            var slots = Flow.Get(panel, "_goodsSlotModels") as System.Collections.IList;
            int taken = 0;
            if (slots != null)
                foreach (var slot in slots)
                {
                    var gs = Flow.Value(slot, "RxGoodsSlot");
                    if (gs == null || Flow.Get(gs, "IsEmpty") is bool e && e) continue;
                    Flow.Log($"{what} slot {Flow.Get(slot, "GoodsId")} x{Flow.Get(gs, "Count")}");
                    Fire(slot, "OnEnter", slot);
                    yield return null;
                    Fire(slot, "OnLeftClick", slot);
                    taken++;
                    yield return new WaitForSecondsRealtime(0.4f);
                }
            yield return new WaitForSecondsRealtime(0.5f);
            Flow.Log($"{what} took {taken} slots, inventory: {InventoryIds()}");
            // Close the menu like a player: Esc (Ui/Escape -> page OnEscape -> MenuPopup closes), then Tab (Ui/CloseMenu).
            foreach (var key in new[] { Key.Escape, Key.Tab })
            {
                Flow.Log($"{what} close with {key}: before {UiState()}");
                yield return Flow.Keys(0.2f, key);
                yield return new WaitForSecondsRealtime(1f);
                Flow.Log($"{what} close with {key}: after {UiState()}");
                if (!MenuOpen()) break;
            }
            if (MenuOpen())
            {
                yield return Review.Shot("a4-loot-stuck");
                // Workaround so the route goes on: the popup's own Close (what its Ui/CloseMenu handler calls).
                var menu = Flow.Find("MenuPopupPresenter");
                menu?.GetType().GetMethod("Close", new[] { typeof(object) })?.Invoke(menu, new object[] { null });
                yield return new WaitForSecondsRealtime(1f);
                Flow.Log($"{what} WORKAROUND MenuPopupPresenter.Close: after {UiState()}");
            }
        }

        static void Fire(object owner, string subject, object arg)
        {
            var s = Flow.Get(owner, subject);
            var m = s?.GetType().GetMethod("OnNext");
            if (m == null) { Flow.Log($"fire {subject}: no subject"); return; }
            m.Invoke(s, new[] { arg });
        }

        static string InventoryIds()
        {
            var inv = Flow.Get(Flow.Find("GameManager"), "MyInventory");
            var slots = Flow.Get(inv, "Slots") as System.Collections.IEnumerable;  // IDictionary<int, GoodsSlot>
            var sb = new System.Text.StringBuilder();
            if (slots != null)
                foreach (var kv in slots)
                    if (Flow.Get(kv, "Value") is object gs)
                    if (gs != null && !(Flow.Get(gs, "IsEmpty") is bool e && e)) sb.Append($"{Flow.Get(gs, "Id")}x{Flow.Get(gs, "Count")},");
            return sb.Length == 0 ? "(empty)" : sb.ToString();
        }

        static bool MenuOpen() => Flow.Find("MenuPopupPresenter") != null || Flow.Get(Flow.MyCharacter(), "State")?.ToString() == "Interact";

        // Where Esc/Tab stop in a menu: the Ui action map and its Escape/CloseMenu actions, the block counts, the top popup.
        static string UiState()
        {
            var im = Flow.Get(Flow.Find("PlayerInputController"), "_inputManager");
            var ui = Flow.Get(im, "UiActions");
            var esc = Flow.Get(ui, "Escape") as InputAction;
            var cm = Flow.Get(ui, "CloseMenu") as InputAction;
            var blocks = Flow.Get(im, "_typeBlockCounts") as System.Collections.IDictionary;
            string b = "";
            if (blocks != null) foreach (System.Collections.DictionaryEntry e in blocks) b += $"{e.Key}:{e.Value},";
            var menu = Flow.Find("MenuPopupPresenter");
            return $"menu={(menu != null)} menuInput={Flow.Get(menu, "IsInputEnable")} uiMap={esc?.actionMap?.enabled} esc={esc?.enabled}/{esc?.phase} closeMenu={cm?.enabled}/{cm?.phase} " +
                   $"mask={esc?.actionMap?.asset?.bindingMask} blocks={b} state={Flow.Get(Flow.MyCharacter(), "State")} kbEsc={Keyboard.current?.escapeKey.isPressed}";
        }

        static bool Alive(Component m) => m != null && m.gameObject.activeInHierarchy && int.TryParse(Hp(m), out var hp) && hp > 0;

        static Component NearestAliveMonster(float range)
        {
            var me = Flow.MyCharacter();
            if (me == null) return null;
            Component best = null;
            float bestD = range;
            foreach (var m in Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None))
                if (m != null && m.GetType().Name == "MonsterController" && Alive(m))
                {
                    float d = Flat(m.transform.position - me.transform.position).magnitude;
                    if (d < bestD) { bestD = d; best = m; }
                }
            return best;
        }

        // Fight like a player: close in on the nearest live monster and swing at it with the left button, the mouse
        // aimed at it, until none is left within range.
        static IEnumerator Fight(float range, float timeout, string what, string shotKey, float waitAppear = 0f)
        {
            s_fightShot = false;
            float start = Time.realtimeSinceStartup, nextLog = start;
            int clicks = 0, kills = 0;
            Component last = null;
            while (Time.realtimeSinceStartup - start < timeout && Flow.ActiveScene == "StageScene")
            {
                if (Flow.Find("DialogPopupView") != null) { yield return AdvanceDialogs(() => Flow.Find("DialogPopupView") == null, 60f, $"dialog in {what}", null, null); continue; }
                var m = NearestAliveMonster(range);
                if (last != null && last != m && !Alive(last)) kills++;
                last = m;
                if (m == null && Time.realtimeSinceStartup - start < waitAppear) { yield return new WaitForSecondsRealtime(0.5f); continue; }
                if (m == null) break;
                var me = Flow.MyCharacter();
                float d = Flat(m.transform.position - me.transform.position).magnitude;
                if (d > 1.8f) { yield return WalkTo(() => m && Alive(m) ? m.transform.position : (Vector3?)null, 1.5f, 2f, $"{what} approach"); continue; }
                Flow.Hold();
                var cam = Camera.main;
                Vector2 aim = cam ? (Vector2)cam.WorldToScreenPoint(m.transform.position) : new Vector2(Screen.width / 2f, Screen.height / 2f);
                yield return Flow.MouseClick(aim);
                clicks++;
                if (shotKey != null && !s_fightShot && clicks >= 3) { yield return Review.Shot(shotKey); s_fightShot = true; }
                if (Time.realtimeSinceStartup > nextLog)
                {
                    nextLog += 5f;
                    Flow.Log($"fight {what} clicks={clicks} target hp={Hp(m)} d={d:F1} me hp={Hp(me)} state={Flow.Get(me, "State")}");
                }
                yield return new WaitForSecondsRealtime(0.25f);
            }
            var left = NearestAliveMonster(range);
            Flow.Log($"fight {what} end clicks={clicks} kills~{kills} left={(left ? Hp(left) + "@" + left.transform.position : "none")} me hp={Hp(Flow.MyCharacter())} after {Time.realtimeSinceStartup - start:F1}s");
        }

        static IEnumerator WaitRadio(string shotKey, float timeout, string what)
        {
            yield return Flow.WaitUntil(() => Flow.Find("RadioToastView") != null || Flow.Find("DialogPopupView") != null, timeout, $"radio {what}");
            if (Flow.Find("RadioToastView") != null && shotKey != null)
            {
                yield return new WaitForSecondsRealtime(1.5f);
                yield return Review.Shot(shotKey);
            }
            if (Flow.Find("DialogPopupView") != null)
                yield return AdvanceDialogs(() => Flow.Find("DialogPopupView") == null, 60f, $"dialog {what}", null, null);
        }

        // Where WASD stops: device state, the game's Player action map and its block counts, the controller's _forward.
        static string InputState()
        {
            var pic = Flow.Find("PlayerInputController");
            var im = Flow.Get(pic, "_inputManager");
            var player = Flow.Get(im, "PlayerActions");
            var blocks = Flow.Get(im, "_typeBlockCounts") as System.Collections.IDictionary;
            string b = "";
            if (blocks != null) foreach (System.Collections.DictionaryEntry e in blocks) b += $"{e.Key}:{e.Value},";
            var move = Flow.Get(player, "MoveKeyboard") as InputAction;
            return $"kb={Keyboard.current?.name} kbEnabled={Keyboard.current?.enabled} w={Keyboard.current?.wKey.isPressed} d={Keyboard.current?.dKey.isPressed} pic={(pic != null)} playerEnabled={Flow.Get(player, "enabled")} " +
                   $"move={move?.ReadValue<Vector2>()} moveEnabled={move?.enabled} phase={move?.phase} forward={Flow.Get(pic, "_forward")} canSend={Flow.Get(pic, "CanSendInput")} " +
                   $"blocks={b} hud={Flow.Value(Flow.Find("GameManager"), "RxActiveInGameHud")}";
        }

        static Component FindTrigger(string typeName, int id)
        {
            foreach (var m in Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None))
                if (m != null && m.GetType().Name == typeName && m.isActiveAndEnabled && Flow.Get(m, "_triggerId") is int t && t == id)
                    return m;
            return null;
        }

        static Component NearestMonster()
        {
            var me = Flow.MyCharacter();
            if (me == null) return null;
            Component best = null;
            float bestD = float.MaxValue;
            foreach (var m in Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None))
                if (m != null && m.GetType().Name == "MonsterController" && m.isActiveAndEnabled)
                {
                    float d = Vector3.Distance(m.transform.position, me.transform.position);
                    if (d < bestD) { bestD = d; best = m; }
                }
            return best;
        }

        // Walk with WASD toward a world point. The camera is isometric: W moves along world (-x,+z), D along (+x,+z)
        // (measured in A4 run 11), so a world direction splits into a W/S part and an A/D part.
        static IEnumerator WalkTo(System.Func<Vector3?> target, float arrive, float timeout, string what, bool run = false)
        {
            float start = Time.realtimeSinceStartup;
            Vector3 last = Vector3.zero;
            int stuck = 0;
            bool pathLogged = false;
            while (Time.realtimeSinceStartup - start < timeout)
            {
                var me = Flow.MyCharacter();
                var t = target();
                if (me == null || t == null) break;
                Vector3 goal = t.Value - me.transform.position;
                goal.y = 0;
                if (goal.magnitude <= arrive) break;
                // Rooms are walled: steer along the stage's NavMesh path (the monsters' own navigation) to the next corner.
                Vector3 d = goal;
                var path = new UnityEngine.AI.NavMeshPath();
                // A target inside an obstacle (Elara's terminal, a trigger box past the corridor's end) is off the NavMesh
                // and CalculatePath fails outright: path to the nearest NavMesh point instead, as a player walks up to it.
                Vector3 dest = t.Value;
                if (UnityEngine.AI.NavMesh.SamplePosition(dest, out var near, 2.5f, UnityEngine.AI.NavMesh.AllAreas)) dest = near.position;
                if (UnityEngine.AI.NavMesh.CalculatePath(me.transform.position, dest, UnityEngine.AI.NavMesh.AllAreas, path) && path.corners.Length > 1)
                {
                    int c = 1;
                    while (c < path.corners.Length - 1 && Vector3.Distance(Flat(path.corners[c]), Flat(me.transform.position)) < 0.4f) c++;
                    d = path.corners[c] - me.transform.position;
                    d.y = 0;
                }
                else if (!pathLogged) { pathLogged = true; Flow.Log($"walk {what} no NavMesh path from {me.transform.position} status={path.status} corners={path.corners.Length} last={(path.corners.Length > 0 ? path.corners[path.corners.Length - 1].ToString() : "-")}"); }
                if (Flow.Find("DialogPopupView") != null) { Flow.Hold(); yield break; }
                float a = (d.z - d.x) / 2f, b = (d.x + d.z) / 2f, m = Mathf.Max(Mathf.Abs(a), Mathf.Abs(b));
                var keys = new System.Collections.Generic.List<Key>();
                if (Mathf.Abs(a) > 0.4f * m) keys.Add(a > 0 ? Key.W : Key.S);
                if (Mathf.Abs(b) > 0.4f * m) keys.Add(b > 0 ? Key.D : Key.A);
                if (run) keys.Add(Key.LeftShift);
                Flow.Hold(keys.ToArray());
                yield return new WaitForSecondsRealtime(0.15f);
                stuck = (me.transform.position - last).sqrMagnitude < 0.0004f ? stuck + 1 : 0;
                last = me.transform.position;
                if (stuck > 14) { Flow.Log($"walk {what} stuck at {last}"); break; }
            }
            Flow.Hold();
            yield return null;
            var end = Flow.MyCharacter();
            Flow.Log($"walk {what} me={(end ? end.transform.position.ToString() : "-")} target={target()} after {Time.realtimeSinceStartup - start:F1}s");
        }

        static string Hp(Component unit)
        {
            foreach (var c in unit.GetComponents<Component>())
                if (Flow.Get(c, "Hp") is int hp) return hp.ToString();
            return "?";
        }

        static Vector3 Flat(Vector3 v) => new Vector3(v.x, 0f, v.z);

        static bool NearMonster(float range)
        {
            var me = Flow.MyCharacter();
            if (me == null) return false;
            foreach (var m in Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None))
                if (m != null && m.GetType().Name == "MonsterController" && m.isActiveAndEnabled
                    && Vector3.Distance(m.transform.position, me.transform.position) < range)
                    return true;
            return false;
        }

        // DialogPopupPresenter.DelayDialogAsync is running (a Lua pause between lines). A click then cancels the delay
        // (OnNext), and the cancelled UniTask never runs LuaApi's ContinueWith, so the Lua await would never finish; the
        // original binary does the same (no OperationCanceledException catch in <DelayDialogAsync>d__66). A player clicks
        // to advance the line on screen, so the test waits the pause out.
        static bool DialogDelaying()
        {
            var p = Flow.Find("DialogPopupPresenter");
            return p != null && Flow.Get(p, "_isDelayActive") is bool b && b;
        }

        // Why a typewriter stops after its first letter: its animator's delta time, loop, readiness, the coroutine.
        static string TypewriterState(object tw)
        {
            var ta = Flow.Get(tw, "TextAnimator");
            var tmp = Flow.Get(ta, "tmpComponent");
            return $"tw[active={(tw as Behaviour)?.isActiveAndEnabled} routine={Flow.Get(tw, "showRoutine") != null} speed={Flow.Get(tw, "internalSpeed")} " +
                   $"taDelta={Flow.Get(Flow.Get(ta, "time"), "deltaTime")} taTime={Flow.Get(Flow.Get(ta, "time"), "timeSinceStart")} loop={Flow.Get(ta, "animationLoop")} scale={Flow.Get(ta, "timeScale")} " +
                   $"taEnabled={(ta as Behaviour)?.isActiveAndEnabled} cached={Flow.Get(ta, "componentsCached")} canvas={(Flow.Get(tmp, "canvas") as Object) != null} " +
                   $"visible={Flow.Get(ta, "maxVisibleCharacters")} first={Flow.Get(ta, "firstVisibleCharacter")} allShown={Flow.Get(ta, "allLettersShown")} dt={Time.deltaTime:F3} ts={Time.timeScale}]";
        }

        // Route the devices to the game and move the mouse to a screen point (fractions of the screen, origin bottom
        // left), so the UI's Point action performs and InputIcons takes keyboard and mouse as the current device.
        static IEnumerator MoveMouseLikeAPlayer(Vector2 at)
        {
            Flow.RouteInputToGame();
            var mouse = Flow.Mouse;
            if (!mouse.enabled) InputSystem.EnableDevice(mouse);
            var to = new Vector2(at.x * Screen.width, at.y * Screen.height);
            float start = Time.realtimeSinceStartup;
            for (int i = 0; i <= 10 && !InputIconsKeyboard(); i++)
            {
                InputSystem.QueueStateEvent(mouse, new MouseState { position = Vector2.Lerp(to - new Vector2(60f, 40f), to, i / 10f) });
                InputSystem.Update();
                yield return new WaitForSecondsRealtime(0.1f);
            }
            yield return Flow.WaitUntil(InputIconsKeyboard, 5f, "InputIcons keyboard and mouse");
            Flow.Log($"mouse moved to {to} inputIconsDevice={InputIconsDevice()} after {Time.realtimeSinceStartup - start:F1}s");
        }

        static readonly System.Type s_inputIcons = System.Type.GetType("InputIcons.InputIconsManagerSO, InputIcons");

        static bool InputIconsKeyboard() =>
            s_inputIcons?.GetMethod("CurrentInputDeviceIsKeyboard")?.Invoke(null, null) is bool b && b;

        static string InputIconsDevice() =>
            (s_inputIcons?.GetMethod("GetCurrentInputDevice")?.Invoke(null, null) as InputDevice)?.name ?? "none";

        // The start room's device-gated guides: the HUD Control Guide and the sector's TutorialGuides/Keyboard group.
        static string GuideState()
        {
            string s = $"device={InputIconsDevice()}";
            foreach (var t in Object.FindObjectsByType<Transform>(FindObjectsInactive.Include, FindObjectsSortMode.None))
                if ((t.name == "Keyboard" && t.parent != null && t.parent.name == "TutorialGuides") || t.name == "KeyGuidePanel!")
                    s += $" {t.parent?.name}/{t.name}={t.gameObject.activeInHierarchy}";
            return s;
        }

        static string DialogEntry() => Flow.Get(Flow.Get(Flow.Find("DialogPopupPresenter"), "_currentEntry"), "Message") as string;

        static string DialogState()
        {
            var p = Flow.Find("DialogPopupPresenter");
            if (p == null) return "presenter=-";
            var entry = Flow.Get(p, "_currentEntry");
            string msg = Flow.Get(entry, "Message") as string;
            if (msg != null && msg.Length > 40) msg = msg.Substring(0, 40);
            var tw = Flow.Get(p, "_typeWriter");
            var ta = Flow.Get(tw, "TextAnimator");
            return $"delay={Flow.Get(p, "_isDelayActive")} writing={Flow.Get(p, "_isWritingText")} state={Flow.Get(p, "_state")} " +
                   $"needSelect={Flow.Get(p, "_needSelect")} canClose={Flow.Get(p, "_canClose")} queue={(Flow.Get(p, "_dialogQueue") as System.Collections.ICollection)?.Count} " +
                   $"entry='{msg}' twShowing={Flow.Get(tw, "isShowingText")} chars={Flow.Get(ta, "CharactersCount")} {TypewriterState(tw)}";
        }

        // Click through DialogPopup lines and NoteSystem letters until done() holds. The first click on a line shows the
        // rest of its text, the next one moves on (DialogPopupPresenter.OnNext), as a player clicking does.
        static IEnumerator AdvanceDialogs(System.Func<bool> done, float timeout, string what, string dialogShot, string noteShot,
            string matchText = null, string matchShot = null, float matchDelay = 1.5f)
        {
            float start = Time.realtimeSinceStartup, nextLog = start;
            int clicks = 0, dialogSeen = 0;
            Flow.Log($"advance {what}");
            while (!done() && Time.realtimeSinceStartup - start < timeout)
            {
                var note = Flow.Find("NoteSystemPopupView");
                var dialog = Flow.Find("DialogPopupView");
                if (note != null && Flow.Ready(Flow.Button(note, "BaseBgDimmer")))
                {
                    if (noteShot != null) { yield return new WaitForSecondsRealtime(1.5f); yield return Review.Shot(noteShot); noteShot = null; }
                    Flow.Button(note, "BaseBgDimmer").onClick.Invoke();
                    clicks++;
                }
                else if (dialog != null && Flow.Ready(Flow.Button(dialog, "Background")) && !DialogDelaying())
                {
                    if (dialogShot != null && ++dialogSeen == 6) { yield return Review.Shot(dialogShot); dialogShot = null; }
                    if (matchShot != null && DialogEntry()?.Contains(matchText) == true)
                    {
                        yield return new WaitForSecondsRealtime(matchDelay);
                        yield return Review.Shot(matchShot);
                        Flow.Log($"advance {what}: shot {matchShot} at {DialogState()}");
                        matchShot = null;
                    }
                    Flow.Button(dialog, "Background").onClick.Invoke();
                    clicks++;
                }
                if (Time.realtimeSinceStartup > nextLog)
                {
                    nextLog += 10f;
                    Flow.Log($"advance {what} clicks={clicks} note={note != null} dialog={dialog != null} {DialogState()} {Flow.LoadState()}");
                }
                yield return new WaitForSecondsRealtime(0.35f);
            }
            Flow.Log($"advance {what} done={done()} clicks={clicks} after {Time.realtimeSinceStartup - start:F1}s");
        }
    }
}
