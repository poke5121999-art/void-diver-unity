using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace VD.PlayTests
{
    // A9b: the lounge SYSTEMS and NPC functions of docs/review/a9/lounge-content.tsv that no other test drives, used like a
    // player (walk, F, pick the function, click in the panel) with the game's own state asserted against table values.
    //
    // Start state of every case: Flow.SeedSaves("tools/fixtures/saves-post-tutorial"), then the title and Continue (DECODING
    // trap 55: the platform stays Local for the whole session, so no RestorePlatform in TearDown). Each case runs inside
    // Flow.GameLog.Run and ends with a log, never Assert.Pass (trap 56).
    //
    // CHEAT RULE. The game's own cheat requests (SROptions -> GameChannel.SendToHost) are allowed only for grind: the user
    // level, money (gold, coins) and items needed as input. Nothing else is cheated: no HP refill, no invincibility, no
    // skip-a-turn, no cleared campaign or quest, no NPC spawn, no unlock. The one exception is in the dive: the diver is a
    // level-1 character and the ATM / Woung stand up to 190 m from the booth past elites it cannot kill, so KeepAlive sends the
    // cheat menu's HP refill (ReqSetCharacterHp = HpMax under 75%: elites take 300 HP in a second) and stress reset (ReqSetCharacterStress 0 from 70), counted
    // in "cheats used" as hp/stress refills; potions on the quick slots are still used first. Cheat() refuses any other request, logs every use
    // ("cheat <Req> ...") and each case logs "cheats used" at its end. Cheats per case:
    //   TycoonAtLevel3        ReqCheatLevelUp x2 (level 3, Elara function 2), ReqCheatAddGoods (1 artifact 100011 as input).
    //   InteriorAtLevel4      ReqCheatLevelUp x3 (level 4, Elara function 3), ReqCheatSetCoin 300 (ATM 200 + exchange input).
    //   NpcFunctions          ReqCheatLevelUp x2 (level 3: gatekeeper/Pim unlock at 2, Ryujin at 3), ReqCheatAddGoods (3 junk
    //                         piles 7000 for the sofa trait; 1 artifact 100011 for Pim).
    //   SalesAndWoungChapter1 ReqCheatLevelUp x3 (level 4), ReqCheatSetCoin 7260 (interior 1250 + 6000, 10 for the dive ATM),
    //                         ReqCheatAddGoods (3 artifacts 100011; 10 item 10001 = the Woung's price;
    //                         5 item 2013 and 5 item 2001 = potions for the quick slots, PlayerManager.SetQuickSlot like the inventory page;
    //                         item 10001 topped up in the stage to the exit cost: 40 at level 4, 65 at level 6, ExitCost.csv; SafeExit/WaveExit both consume it). Hires, placements, the sale,
    //                         the dive ATM exchange and the shop are played.
    //   WoungChapter2         ReqCheatLevelUp x5 (level 6, campaign 201..211 UserLevel:6), ReqCheatSetCoin 30 (ATM input),
    //                         ReqCheatAddGoods (20 item 10001 = the Woung's price; 5 item 2013 and 5 item 2001 = potions).
    //
    // Expected numbers come from the tables (vd-src/vd-ref/tables), spelled as literals next to the check:
    // - Interior 111 ATM 200 coins, 121 first extra slot 1250 coins, 122 second 6000 coins (InteriorShop.csv).
    // - Coin exchange 1:1 online coin <-> item 10001 (CharacterController.OnExchangeCoin).
    // - Talent 2001 (sofa trait group 2) costs Gold 1000 + 3x item 7000 (Talent.csv).
    // - Pim buys an artifact at SellPrice x ArtifactShopCostMultiplier 0.7: artifact 100011 (Normal, SellPrice 600, no
    //   prefix: ArtifactGoods price = SellPrice) -> 420 gold.
    // - Ryujin shop group 2, product 60: item 2041 for 300 gold (ShopProduct.csv).
    // - Woung 700214 (shop 1001) product 83: item 82107 for 10 x item 10001; 700230 (shop 1011) product 190: item 7000 for 5;
    //   700215 (shop 1002) product 137: item 82201 for 20; 700231 (shop 1012) product 205: item 7000 for 5.
    // - Hire cost = Employee.HireCost + 300 x (Level-1)^2: 1000070 150, 1000085 400 at level 1.
    // - Tycoon sale of artifact 100011 (600 gold): max work progress 70 (TycoonDefaultMaxWorkProgressNormal), the employee
    //   adds 75 + 3 x Charm + 0.3 x Persuasion >= 84 per turn (one dive back = one turn, LoungeScene.OnPlaying), so the slot
    //   finishes at once. Price = 600 x (1 + floor(Persuasion x 2.5)/100), coins = price / CoinToGoldRate 48:
    //   1000001 (Persuasion 2) 630 -> 13, 1000070 (3) 642 -> 13, 1000085 (5) 672 -> 14. No employee skill targets Normal.
    //
    // Cases with a reason instead of a test (docs/review/a9/lounge-content.tsv rows):
    // - 600002-600011 (customers) spawn only after a dive, in sectors, when FeatureUnlock ArtifactDeal holds
    //   (LoungeQuestCleared:80100, which needs campaign 1101 cleared: not grind); 600012 is spawned by Lua 80100; 600013 has
    //   no spawn at all. The tycoon's own customers (8000xx, TycoonCustomer) are asserted in SalesAndWoungChapter1.
    // - 700005 owner (Npc unlock CampaignCleared:1101) and 700003 Evelyn (LoungeQuestCleared:82112): their unlock is a
    //   story/quest state, not grind.
    // - 700004 Rest: needs an active paradox (none in the fixture; paradoxes come from dives), the case checks the refusal.
    public class A9b_LoungeSystemsFlow
    {
        const string Fixture = "tools/fixtures/saves-post-tutorial";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void PrepareSaves()
        {
            if (!Flow.RunFor("A9b_LoungeSystemsFlow")) return;
            Application.logMessageReceivedThreaded -= IgnoreBootErrors;
            Application.logMessageReceivedThreaded += IgnoreBootErrors;
            Flow.Tag = "VD_A9B";
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
            // The Local platform stays for the whole session (trap 55).
        }

        [UnityTest, Timeout(3000000)] public IEnumerator TycoonAtLevel3() { yield return Flow.GameLog.Run("TycoonAtLevel3", TycoonBody()); }
        [UnityTest, Timeout(3000000)] public IEnumerator InteriorAtLevel4() { yield return Flow.GameLog.Run("InteriorAtLevel4", InteriorBody()); }
        [UnityTest, Timeout(3000000)] public IEnumerator NpcFunctions() { yield return Flow.GameLog.Run("NpcFunctions", NpcFunctionsBody()); }
        [UnityTest, Timeout(3000000)] public IEnumerator SalesAndWoungChapter1() { yield return Flow.GameLog.Run("SalesAndWoungChapter1", SalesBody()); }
        [UnityTest, Timeout(3000000)] public IEnumerator WoungChapter2() { yield return Flow.GameLog.Run("WoungChapter2", Woung2Body()); }

        // ---- case bodies ----

        // Elara function 2 at user level 3: open it, hire a recruit, put an artifact and an employee on sales slot 101, start.
        IEnumerator TycoonBody()
        {
            yield return Begin("TycoonAtLevel3");
            yield return Prep80200();
            yield return Level(3);
            yield return GiveArtifact(ArtifactId);
            long gold0 = Gold();

            bool open = false;
            yield return OpenFunction(700000, "Tycoon", "TycoonMenuPopupPresenter", "Elara Tycoon", r => open = r);
            Assert.IsTrue(open, "Elara's Tycoon function did not open the Tycoon menu");
            yield return Settle(1.5f);
            yield return Review.Shot("a9b-tycoon-main");

            yield return Hire(1000070, 150);
            Check(Gold() == gold0 - 150, $"hire 1000070: gold {gold0} -> {Gold()}, want {gold0 - 150}");
            yield return Review.Shot("a9b-tycoon-hire");

            yield return PlaceOnSlot(101, 1000001, ArtifactId);
            var slot = SalesSlot(101);
            Check(slot != null && Flow.Get(slot, "Goods") != null && (int)Flow.Get(Flow.Get(slot, "Goods"), "Id") == ArtifactId, "slot 101 holds artifact 100011");
            Check(slot != null && (int)Flow.Get(slot, "EmployeeId") == 1000001, "slot 101 employee 1000001");
            Check(slot != null && Flow.Get(slot, "IsStarted") is true, "slot 101 started");
            Check(slot != null && Convert.ToSingle(Flow.Get(slot, "MaxWorkProgress")) == 70f, $"slot 101 max work progress {(slot == null ? -1 : Flow.Get(slot, "MaxWorkProgress"))}, want 70");
            yield return Review.Shot("a9b-tycoon-started");

            // The same state read back from the saved profile (TycoonManager saves after each change).
            bool saved = false;
            yield return Flow.WaitUntil(() => saved = SavedSlot(101, ArtifactId, 1000001) && SavedHasEmployee(1000070), 15f, "slot 101 and employee 1000070 in PlayerContext_0.sav");
            Check(saved, "PlayerContext_0.sav has slot 101 (artifact 100011, employee 1000001, started) and employee 1000070");
            Check(UnlockFlagSet("TycoonUnlockFlags", 1), "TycoonUnlockFlags bit 1 (slot 101)");
            yield return Esc("TycoonMenuPopupPresenter");
            End("TycoonAtLevel3");
        }

        // Elara function 3 at level 4: buy the ATM (111, 200 coins), see it appear in the lounge, exchange coins at it.
        IEnumerator InteriorBody()
        {
            yield return Begin("InteriorAtLevel4");
            yield return Prep80200();
            yield return Level(4);
            yield return SetCoin(300);

            bool open = false;
            yield return OpenFunction(700000, "Interior", "InteriorPopupPresenter", "Elara Interior", r => open = r);
            Assert.IsTrue(open, "Elara's Interior function did not open the Interior popup");
            yield return Settle(1.5f);
            yield return Review.Shot("a9b-interior-list");
            Check(Flow.FindNpc(700150) == null, "ATM 700150 is not in the lounge before it is bought");

            yield return BuyInterior(111);
            Check(Coin() == 100, $"coins after the ATM: {Coin()}, want 300-200=100");
            Check(NpcLevel(700150) == 1, $"NpcLevel 700150 = {NpcLevel(700150)}, want 1");
            yield return Review.Shot("a9b-interior-bought");
            yield return Esc("InteriorPopupPresenter");

            // The bought ATM stands in the lounge (sector 9002 spawn data, NpcUnlockEvent) and works.
            yield return Flow.WaitUntil(() => Flow.FindNpc(700150) != null, 15f, "ATM 700150 spawned");
            Check(Flow.FindNpc(700150) != null, "ATM 700150 appeared in the lounge");
            bool atm = false;
            yield return OpenFunction(700150, "CoinExchange", "CoinExchangePopupPresenter", "ATM 700150", r => atm = r);
            Assert.IsTrue(atm, "the lounge ATM did not open the coin exchange");
            yield return Settle(1f);
            yield return Review.Shot("a9b-atm-open");
            yield return Exchange(100, toItem: true);
            Check(Coin() == 0, $"coins after exchanging 100: {Coin()}");
            Check(ItemCount(10001) == 100, $"item 10001 after exchanging 100 coins: {ItemCount(10001)}, want 100");
            yield return Exchange(40, toItem: false);
            Check(Coin() == 40, $"coins after taking 40 back: {Coin()}");
            Check(ItemCount(10001) == 60, $"item 10001 after taking 40 back: {ItemCount(10001)}, want 60");
            yield return Review.Shot("a9b-atm-done");
            yield return Esc("CoinExchangePopupPresenter");
            End("InteriorAtLevel4");
        }

        // Sofa 700004 (trait group 2, Rest), gatekeeper 700012, Pim 700015 (artifact shop), Ryujin 700016 (shop group 2).
        IEnumerator NpcFunctionsBody()
        {
            yield return Begin("NpcFunctions");
            yield return Prep80200();
            yield return Level(3);
            yield return GiveItem(7000, 3);
            yield return GiveArtifact(ArtifactId);
            Check(ItemCount(7000) >= 3, $"3 junk piles given: {ItemCount(7000)}");

            // Shogoth sofa: Rest first (no active paradox: refused, nothing opens, nothing is paid), then the trait.
            long gold = Gold();
            bool shown = false;
            yield return OpenFunctionRaw(700004, "Rest", "Sofa Rest", r => shown = r);
            yield return Settle(1.5f);
            Check(shown && Flow.TopPopup() == null && Gold() == gold, $"Rest without a paradox: selected={shown} top={Flow.TopPopup()} gold {gold} -> {Gold()}");
            if (Flow.Find("SelectionPopupView") != null) yield return Flow.Keys(0.2f, Key.Escape);   // Esc with nothing open is the pause menu
            yield return Settle(0.5f);

            bool talent = false;
            yield return OpenFunction(700004, "Talent", "TalentPopupPresenter", "Sofa Talent", r => talent = r);
            Check(talent, "the sofa's Talent function opened the trait tree");
            if (talent)
            {
                yield return Settle(2f);
                yield return Review.Shot("a9b-sofa-talent");
                long goldBefore = Gold(), junk = ItemCount(7000);
                yield return UnlockTalent(2001);
                Check(HasTalent(2001), "talent 2001 unlocked");
                Check(Gold() == goldBefore - 1000, $"gold {goldBefore} -> {Gold()}, want -1000");
                Check(ItemCount(7000) == junk - 3, $"item 7000 {junk} -> {ItemCount(7000)}, want -3");
                yield return Review.Shot("a9b-sofa-talent-done");
                yield return Esc("TalentPopupPresenter");
            }

            // Gatekeeper: the multiplayer room popup opens (Local platform counts as online); leave it.
            bool room = false;
            yield return OpenFunction(700012, "MultiplayCreate", "RoomSettingPopupPresenter", "Gatekeeper create room", r => room = r);
            Check(room, "gatekeeper MultiplayCreate opened RoomSettingPopupPresenter");
            yield return Settle(1f);
            yield return Review.Shot("a9b-gatekeeper-room");
            yield return Esc("RoomSettingPopupPresenter");

            // Pim sells one artifact: 100011 (SellPrice 600) x 0.7 = 420 gold.
            long goldPim = Gold();
            bool pim = false;
            yield return OpenFunction(700015, "ArtifactShop", "ArtifactShopPopupPresenter", "Pim", r => pim = r);
            Check(pim, "Pim's ArtifactShop opened");
            if (pim)
            {
                yield return Settle(1.5f);
                yield return Review.Shot("a9b-pim-shop");
                yield return SellArtifactToPim();
                Check(Gold() == goldPim + 420, $"Pim: gold {goldPim} -> {Gold()}, want +420");
                Check(CountArtifacts() == 0, "the artifact left the inventory");
                yield return Esc("ArtifactShopPopupPresenter");
            }

            // Ryujin: shop group 2, product 60 (item 2041) for 300 gold.
            long goldRyu = Gold(), item = ItemCount(2041);
            bool ryu = false;
            yield return OpenFunction(700016, "Shop", "ShopPopupPresenter", "Ryujin", r => ryu = r);
            Check(ryu, "Ryujin's Shop opened");
            if (ryu)
            {
                yield return Settle(1.5f);
                yield return Review.Shot("a9b-ryujin-shop");
                yield return BuyProduct(60);
                Check(Gold() == goldRyu - 300, $"Ryujin: gold {goldRyu} -> {Gold()}, want -300");
                Check(ItemCount(2041) == item + 1, $"Ryujin: item 2041 {item} -> {ItemCount(2041)}, want +1");
                yield return Esc("ShopPopupPresenter");
            }
            End("NpcFunctions");
        }

        // Interior slots 121/122 (1250 and 6000 coins) unlock sales slots 700107/700108; three employees each sell one
        // artifact (100011); the dive back is the turn; the dive ATM 700152 and a Woung shop are used on the way.
        IEnumerator SalesBody()
        {
            yield return Begin("SalesAndWoungChapter1");
            yield return Prep80200();
            yield return Level(4);
            yield return SetCoin(7260);
            for (int i = 0; i < 3; i++) yield return GiveArtifact(ArtifactId);
            Check(CountArtifacts() == 3, $"3 artifacts given: {CountArtifacts()}");
            yield return GiveItem(10001, 10);   // the Woung's price (10 real coins) as input; the dive ATM exchange is asserted on its own
            yield return PrepareHealing();

            bool open = false;
            yield return OpenFunction(700000, "Interior", "InteriorPopupPresenter", "Elara Interior", r => open = r);
            Assert.IsTrue(open, "Interior did not open");
            yield return Settle(1.5f);
            yield return BuyInterior(121);
            Check(Coin() == 7260 - 1250, $"coins after 121: {Coin()}, want 6010");
            yield return BuyInterior(122);
            Check(Coin() == 10, $"coins after 122: {Coin()}, want 10");
            Check(NpcLevel(700107) == 1 && NpcLevel(700108) == 1, $"NpcLevel 700107/700108 = {NpcLevel(700107)}/{NpcLevel(700108)}, want 1/1");
            yield return Esc("InteriorPopupPresenter");
            yield return Flow.WaitUntil(() => Flow.FindNpc(700107) != null && Flow.FindNpc(700108) != null, 15f, "sales slot NPCs 700107 and 700108");
            Check(Flow.FindNpc(700106) != null && Flow.FindNpc(700107) != null && Flow.FindNpc(700108) != null, "sales slot NPCs 700106/700107/700108 stand in the lounge");

            // Open the tycoon at the first slot NPC and hire two recruits: 150 + 400 gold.
            long gold0 = Gold();
            bool t = false;
            yield return OpenFunction(700106, "TycoonSalesSlot", "TycoonMenuPopupPresenter", "slot NPC 700106", r => t = r);
            Assert.IsTrue(t, "sales slot NPC 700106 did not open the Tycoon menu");
            yield return Settle(1.5f);
            Check(SalesSlot(102) != null && SalesSlot(103) != null, "sales slots 102 and 103 exist after the two interior purchases");
            Check(UnlockFlagSet("TycoonUnlockFlags", 2) && UnlockFlagSet("TycoonUnlockFlags", 3), "TycoonUnlockFlags bits 2 and 3");
            yield return Hire(1000070, 150);
            yield return Hire(1000085, 400);
            Check(Gold() == gold0 - 550, $"gold after two hires: {gold0} -> {Gold()}, want -550");
            yield return PlaceOnSlot(101, 1000001, ArtifactId);
            yield return Esc("TycoonMenuPopupPresenter");

            var placements = new[] { (700107, 102, 1000070), (700108, 103, 1000085) };
            foreach (var (npc, slotId, employee) in placements)
            {
                bool o = false;
                yield return OpenFunction(npc, "TycoonSalesSlot", "TycoonMenuPopupPresenter", $"slot NPC {npc}", r => o = r);
                Assert.IsTrue(o, $"slot NPC {npc} did not open the Tycoon menu");
                yield return Settle(1f);
                yield return PlaceOnSlot(slotId, employee, ArtifactId);
                yield return Esc("TycoonMenuPopupPresenter");
            }
            foreach (var id in new[] { 101, 102, 103 })
                Check(SalesSlot(id) != null && Flow.Get(SalesSlot(id), "IsStarted") is true, $"slot {id} started");
            yield return Review.Shot("a9b-sales-slots");

            // The dive (campaign 101, UserLevel:2): ATM 700152 (sector 4012) and a Woung (700214 zone spawn / 700230 sector 4013).
            var dive = new DiveReport();
            yield return DiveCampaign(101, 108, "Easy", DiveTasks(dive, coinsToExchange: 10, shops: Chapter1Shops), dive);
            Flow.Log($"dive report: {dive}");
            Check(dive.AtmDone, $"dive ATM 700152: {dive.AtmNote}");
            Check(dive.ShopDone, $"dive Woung: {dive.ShopNote}");
            Assert.AreEqual("LoungeScene", Flow.ActiveScene, "the dive did not come back to the lounge");

            // The turn ran: all three slots finished, each with a customer from NpcFunction TycoonCustomer (8000xx).
            var customers = TycoonCustomerNpcIds();
            foreach (var id in new[] { 101, 102, 103 })
            {
                var s = SalesSlot(id);
                Check(s != null && Flow.Get(s, "IsFinished") is true, $"slot {id} finished after the dive");
                int cust = s == null ? 0 : (int)Flow.Get(s, "TycoonCustomerNpcId");
                Check(customers.Contains(cust), $"slot {id} customer {cust} is a TycoonCustomer npc ({string.Join(",", customers)})");
            }
            var customerNpcs = Flow.Npcs().Where(n => customers.Contains(Flow.NpcId(n))).Select(Flow.NpcId).ToList();
            Flow.Log($"customer npcs in the lounge: [{string.Join(",", customerNpcs)}]");
            Check(customerNpcs.Count > 0, "tycoon customers stand at the slots");

            // Collect each result at its slot NPC: coins per the table maths (13, 13, 14).
            var want = new Dictionary<int, long> { { 101, 13 }, { 102, 13 }, { 103, 14 } };
            var npcOf = new Dictionary<int, int> { { 101, 700106 }, { 102, 700107 }, { 103, 700108 } };
            foreach (var id in new[] { 101, 102, 103 })
            {
                bool o = false;
                yield return OpenFunction(npcOf[id], "TycoonSalesSlot", "TycoonMenuPopupPresenter", $"collect slot {id}", r => o = r);
                if (!o) { Check(false, $"slot NPC {npcOf[id]} did not open the Tycoon menu to collect"); continue; }
                yield return Settle(1.5f);
                long coin = Coin();
                yield return ClickMain("BtnResult", $"slot {id} result");
                yield return Flow.WaitUntil(() => Coin() != coin, 10f, $"slot {id} coins");
                yield return Settle(1.5f);
                if (id == 101) yield return Review.Shot("a9b-tycoon-result");
                yield return DismissTycoonResult();
                Check(Coin() - coin == want[id], $"slot {id}: coins {coin} -> {Coin()}, want +{want[id]}");
                Check(SalesSlot(id) != null && Flow.Get(SalesSlot(id), "Goods") == null && (int)Flow.Get(SalesSlot(id), "EmployeeId") == 0, $"slot {id} empty again");
                yield return Esc("TycoonMenuPopupPresenter");
            }
            End("SalesAndWoungChapter1");
        }

        IEnumerator Woung2Body()
        {
            yield return Begin("WoungChapter2");
            yield return Prep80200();
            yield return Level(6);
            yield return SetCoin(30);
            yield return GiveItem(10001, 20);   // the Woung's price (20 real coins) as input; the dive ATM exchange is asserted on its own
            yield return PrepareHealing();
            var dive = new DiveReport();
            yield return DiveCampaign(201, 211, "Easy", DiveTasks(dive, coinsToExchange: 30, shops: Chapter2Shops), dive);
            Flow.Log($"dive report: {dive}");
            Check(dive.AtmDone, $"dive ATM 700152: {dive.AtmNote}");
            Check(dive.ShopDone, $"dive Woung: {dive.ShopNote}");
            End("WoungChapter2");
        }

        const int ArtifactId = 100011;

        // (npc id, shop function, product id, product item, price in item 10001)
        static readonly (int npc, int product, int item, int price)[] Chapter1Shops = { (700214, 83, 82107, 10), (700230, 190, 7000, 5) };
        static readonly (int npc, int product, int item, int price)[] Chapter2Shops = { (700215, 137, 82201, 20), (700231, 205, 7000, 5) };

        // ---- soft checks ----

        static readonly List<string> s_failed = new List<string>();
        static readonly List<string> s_cheats = new List<string>();

        static void Check(bool ok, string what)
        {
            Flow.Log($"CHECK {(ok ? "ok" : "FAIL")}: {what}");
            if (!ok) s_failed.Add(what);
        }

        static int s_hpRefills, s_stressRefills;

        static void End(string label)
        {
            Flow.Log($"{label} cheats used: [{string.Join("; ", s_cheats)}] dive refills: hp x{s_hpRefills}, stress x{s_stressRefills}");
            Flow.Log($"{label} end failed={s_failed.Count}");
            Assert.IsEmpty(s_failed, $"{label} failed checks:\n{string.Join("\n", s_failed)}");
        }

        // ---- start: title, Continue, the lounge ----

        static IEnumerator Begin(string label)
        {
            LogAssert.ignoreFailingMessages = true;
            Flow.Tag = "VD_A9B";
            s_failed.Clear();
            s_cheats.Clear();
            s_hpRefills = s_stressRefills = 0;
            Flow.Log($"{label} start platform={Flow.Platform()}");
            yield return FromTitle();
            Assert.AreEqual("LoungeScene", Flow.ActiveScene, "Continue did not reach the lounge");
            Assert.AreEqual("Playing", Flow.StageState(), $"lounge load stuck: {Flow.LoadState()}");
            Flow.RouteInputToGame();
            Flow.Log($"{label} lounge level={Level()} gold={Gold()} coin={Coin()} employees={Keys(Flow.Get(PM(), "Employees"))} recruits={Keys(Flow.Get(PM(), "RecruitableEmployees"))}");
        }

        // Same start as A6: a case that ended in a room is left like Return to Title, the fixture is copied back in and the
        // cached profile dropped, then Continue reads PlayerContext_0.sav like a fresh launch.
        static IEnumerator FromTitle()
        {
            bool inRoom = Flow.ActiveScene == "LoungeScene" || Flow.ActiveScene == "StageScene";
            if (inRoom)
            {
                var gm = Flow.Find("GameManager");
                Flow.Log($"return to title from {Flow.ActiveScene} {Flow.LoadState()} top={Flow.TopPopup()}");
                gm?.GetType().GetMethod("ClearStageResult")?.Invoke(gm, null);
                var channel = Flow.Get(gm, "_gameChannel");
                var rx = Flow.Get(channel, "RxConnectState");
                var valueProp = rx?.GetType().GetProperty("Value");
                valueProp?.SetValue(rx, Enum.ToObject(valueProp.PropertyType, 5));   // ERoomConnectState.BackToTitle
                var host = Flow.Get(gm, "_gameHost");
                if (Flow.Get(gm, "IsHost") is bool isHost && isHost) host?.GetType().GetMethod("CloseRoom")?.Invoke(host, null);
                else channel?.GetType().GetMethod("LeftRoom")?.Invoke(channel, null);
                yield return Flow.WaitUntil(() => Flow.Find("StartSceneView") != null, 60f, "title after Return to Title");
                Assert.IsNotNull(Flow.Find("StartSceneView"), $"Return to Title did not reach the title: {Flow.ActiveScene}");
            }
            else
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(0);
                yield return Flow.WaitUntil(() => Flow.Find("StartSceneView") != null, 120f, "title");
            }
            yield return new WaitForSecondsRealtime(2f);   // a lounge autosave in flight lands before the copy
            Flow.SeedSaves(Fixture);
            var pm = Flow.Find("PlayerManager");
            if (pm != null)
            {
                (Flow.Get(pm, "_playerDisposable") as IDisposable)?.Dispose();
                pm.GetType().GetField("_playerDisposable", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)?.SetValue(pm, null);
                (Flow.Get(pm, "_playerContext") as IDisposable)?.Dispose();
                pm.GetType().GetField("_playerContext", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)?.SetValue(pm, null);
            }
            yield return Flow.ContinueToLounge(loadTitle: false);
        }

        // Elara's lounge quest 80200 sends the player to Narcis, Felix and Lucas before her menu opens.
        static IEnumerator Prep80200()
        {
            yield return Flow.Talk(700000, "Elara 80200 start");
            foreach (var (id, who) in new[] { (700013, "Narcis"), (700008, "Felix"), (700001, "Lucas") })
                yield return Flow.Talk(id, $"{who} 80200");
            yield return Flow.Talk(700000, "Elara 80200 finish");
            yield return Settle(1f);
        }

        static IEnumerator Settle(float seconds) { yield return new WaitForSecondsRealtime(seconds); }

        // ---- cheats (grind only) ----

        // The dive's HP refill and stress reset (KeepAlive) go through the same gate; they are counted, not listed.
        static readonly string[] AllowedCheats =
            { "ReqCheatLevelUp", "ReqCheatSetGold", "ReqCheatSetCoin", "ReqCheatAddGoods", "ReqSetCharacterHp", "ReqSetCharacterStress" };

        static Type GameType(string fullName) =>
            AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType(fullName)).FirstOrDefault(t => t != null);

        static void Cheat(string type, params (string prop, object value)[] props)
        {
            Send(type, props);
            string what = $"{type} {string.Join(" ", props.Select(x => $"{x.prop}={(x.value is Array a ? a.Length + " goods" : x.value)}"))}".Trim();
            s_cheats.Add(what);
            Flow.Log($"cheat {what}");
        }

        static void Send(string type, params (string prop, object value)[] props)
        {
            Assert.Contains(type, AllowedCheats, $"{type} is not a grind cheat (user level, money, items, dive HP/stress)");
            var request = Activator.CreateInstance(GameType(type));
            foreach (var (p, v) in props)
            {
                var prop = request.GetType().GetProperty(p);
                prop.SetValue(request, v == null || prop.PropertyType.IsInstanceOfType(v) ? v : Convert.ChangeType(v, prop.PropertyType));
            }
            var channel = Flow.Get(Flow.Find("GameManager"), "_gameChannel");
            channel.GetType().GetMethod("SendToHost").Invoke(channel, new[] { request });
        }

        static IEnumerator Level(int level)
        {
            for (int i = 0; i < 12 && Level() < level; i++)
            {
                Cheat("ReqCheatLevelUp");
                yield return Flow.WaitUntil(() => Level() > 0, 1f, "level after the cheat");
                yield return new WaitForSecondsRealtime(1f);
            }
            Assert.AreEqual(level, Level(), "cheat level-up did not reach the level");
        }

        static IEnumerator SetCoin(long coin)
        {
            Cheat("ReqCheatSetCoin", ("Coin", coin));
            yield return Flow.WaitUntil(() => Coin() == coin, 5f, $"coin {coin}");
            Assert.AreEqual(coin, Coin(), "coin cheat");
        }

        // Potions for the dive: items as input (cheat), then the quick slots as the inventory page's quick slot setting does
        // (PlayerManager.SetQuickSlot): keys 1 and 2 use item 2013 (enhanced HP potion) and 2001 (HP drink).
        static IEnumerator PrepareHealing()
        {
            yield return GiveItem(2013, 5);
            yield return GiveItem(2001, 5);
            var pm = PM();
            pm.GetType().GetMethod("SetQuickSlot").Invoke(pm, new object[] { 0, 2013 });
            pm.GetType().GetMethod("SetQuickSlot").Invoke(pm, new object[] { 1, 2001 });
            Flow.Log($"quick slots = [{string.Join(",", ((System.Collections.IEnumerable)Flow.Get(pm, "QuickSlotItemIds")).Cast<object>())}]");
        }

        static IEnumerator GiveItem(int id, long count)
        {
            long before = ItemCount(id);
            var goods = GameType("GameCommon.Data.GoodsCreator").GetMethod("MakeItemGoods").Invoke(null, new object[] { id, 0L, count });
            var array = Array.CreateInstance(GameType("GameCommon.Data.Goods"), 1);
            array.SetValue(goods, 0);
            Cheat("ReqCheatAddGoods", ("Goods", array));
            yield return Flow.WaitUntil(() => ItemCount(id) >= before + count, 5f, $"item {id} x{count}");
        }

        // An artifact with no prefix, so its price is exactly the table's SellPrice (ArtifactGoods.GetSellPrice).
        static IEnumerator GiveArtifact(int id)
        {
            int before = CountArtifacts();
            var goods = GameType("GameCommon.Data.GoodsCreator").GetMethod("MakeEquipmentGoods")
                .Invoke(null, new object[] { id, 0L, 1L, new int[0], null, null, (byte)1 });
            var array = Array.CreateInstance(GameType("GameCommon.Data.Goods"), 1);
            array.SetValue(goods, 0);
            Cheat("ReqCheatAddGoods", ("Goods", array));
            yield return Flow.WaitUntil(() => CountArtifacts() > before, 5f, $"artifact {id} in the inventory");
            Assert.Greater(CountArtifacts(), before, "artifact grant");
        }

        // ---- the game's state ----

        static MonoBehaviour PM() => Flow.Find("PlayerManager");
        static long Gold() => Convert.ToInt64(Flow.Get(PM(), "Gold") ?? 0L);
        static long Coin() => Convert.ToInt64(Flow.Get(PM(), "Coin") ?? 0L);
        static int Level() => Flow.Get(PM(), "Level") is int l ? l : 0;

        static long ItemCount(int id)
        {
            var pm = PM();
            var goodsType = GameType("GameCommon.Data.Goods");
            var goods = GameType("GameCommon.Data.GoodsCreator").GetMethod("MakeItemGoods").Invoke(null, new object[] { id, 0L, 0L });
            var m = pm.GetType().GetMethod("GetTotalGoodsCount", new[] { goodsType, typeof(bool) });
            return Convert.ToInt64(m.Invoke(pm, new object[] { goods, false }));
        }

        static bool HasTalent(int id) => PM().GetType().GetMethod("HasTalent").Invoke(PM(), new object[] { id }) is true;

        static int NpcLevel(int npc) => Convert.ToInt32(PM().GetType().GetMethod("GetNpcLevel").Invoke(PM(), new object[] { npc }));

        static IEnumerable<object> InventorySlots()
        {
            var inv = Flow.Get(Flow.Find("GameManager"), "MyInventory");
            return (Flow.Get(Flow.Get(inv, "Slots"), "Values") as IEnumerable)?.Cast<object>() ?? Enumerable.Empty<object>();
        }

        static int CountArtifacts() => InventorySlots().Count(s => Flow.Get(s, "IsEmpty") is false && Flow.Get(s, "Type")?.ToString() == "Artifact");

        static string Keys(object dict)
        {
            var keys = Flow.Get(dict, "Keys") as IEnumerable;
            return keys == null ? "-" : string.Join(",", keys.Cast<object>());
        }

        static object DictGet(object dict, int key)
        {
            if (dict == null) return null;
            var tryGet = dict.GetType().GetMethod("TryGetValue");
            var args = new object[] { key, null };
            return tryGet != null && (bool)tryGet.Invoke(dict, args) ? args[1] : null;
        }

        static object SalesSlot(int id) => DictGet(Flow.Get(PM(), "SalesSlots"), id);

        static IEnumerable<int> TycoonCustomerNpcIds()
        {
            var table = GameType("GameCommon.Table.TNpcFunction")?.GetProperty("Table")?.GetValue(null);
            var rows = (Flow.Get(table, "Values") as IEnumerable)?.Cast<object>() ?? Enumerable.Empty<object>();
            return rows.Where(r => Flow.Get(r, "Type")?.ToString() == "TycoonCustomer").Select(r => (int)Flow.Get(r, "NpcId")).Distinct().ToList();
        }

        // The saved profile as the next Continue reads it (PlayerContext_0.sav through the game's SerializeHelper).
        static object SavedProfile()
        {
            try
            {
                string path = Path.Combine(Directory.GetParent(Application.dataPath).FullName, "SavesDir", "PlayerContext_0.sav");
                if (!File.Exists(path)) return null;
                var bytes = File.ReadAllBytes(path);
                var dataType = GameType("PlayerContextData");
                return GameType("SerializeHelper")?.GetMethod("Deserialize")?.MakeGenericMethod(dataType).Invoke(null, new object[] { bytes });
            }
            catch (Exception e) { Flow.Log($"saved profile read threw {e.GetType().Name}: {e.InnerException?.Message ?? e.Message}"); return null; }
        }

        static bool SavedSlot(int slotId, int artifactId, int employeeId)
        {
            var slots = Flow.Get(SavedProfile(), "SalesSlots") as IEnumerable;
            if (slots == null) return false;
            foreach (var s in slots)
                if ((int)Flow.Get(s, "Id") == slotId)
                {
                    var goods = Flow.Get(s, "Goods");
                    return goods != null && (int)Flow.Get(goods, "Id") == artifactId && (int)Flow.Get(s, "EmployeeId") == employeeId && Flow.Get(s, "IsStarted") is true;
                }
            return false;
        }

        static bool SavedHasEmployee(int id)
        {
            var list = Flow.Get(SavedProfile(), "Employees") as IEnumerable;
            return list != null && list.Cast<object>().Any(e => (int)Flow.Get(e, "Id") == id);
        }

        static bool UnlockFlagSet(string member, int bit)
        {
            var flags = Flow.Get(PM(), member);
            return flags?.GetType().GetMethod("IsSet")?.Invoke(flags, new object[] { bit }) is true;
        }

        // ---- opening a function like a player ----

        // Walk up, F; play the dialogs the NPC says first (Elara's quest lines come before her menu), pick the function in
        // the selection popup, wait for the presenter.
        static IEnumerator OpenFunction(int npcId, string function, string presenter, string what, Action<bool> result)
        {
            for (int attempt = 0; attempt < 8 && Flow.Find(presenter) == null; attempt++)
            {
                var npc = Flow.FindNpc(npcId);
                if (npc == null) { Flow.Log($"open {what}: npc {npcId} not in the lounge"); result(false); yield break; }
                if (attempt == 0) Flow.Log($"open {what}: npc {npcId} functions=[{string.Join(",", Flow.FunctionIds(npc).Select(id => $"{id}:{Flow.FunctionType(id)}"))}]");
                yield return Flow.InteractNpc(npc, what);
                if (!Flow.Focused(npc) && Flow.Find("DialogPopupView") == null && Flow.Find("SelectionPopupView") == null && Flow.Find(presenter) == null)
                {
                    yield return DiagNpc(npc, what);
                    yield return Approach(npc, what);
                    if (attempt >= 1 && !Flow.Focused(npc)) break;   // the same place again would stand in the same spot
                }
                yield return Flow.WaitUntil(() => Flow.Find("DialogPopupView") != null || Flow.Find("SelectionPopupView") != null || Flow.Find(presenter) != null, 4f, $"{what} reaction");
                if (Flow.Find("DialogPopupView") != null)
                {
                    yield return Flow.AdvanceDialogs(() => Flow.Find("DialogPopupView") == null, 120f, $"{what} dialog");
                    yield return new WaitForSecondsRealtime(1f);
                    continue;
                }
                if (Flow.Find("SelectionPopupView") != null)
                {
                    yield return new WaitForSecondsRealtime(0.8f);
                    yield return Flow.SelectFunction(npc, function);
                }
                yield return Flow.WaitUntil(() => Flow.Find(presenter) != null, 8f, $"{what} {presenter}");
            }
            result(Flow.Find(presenter) != null);
            if (Flow.Find(presenter) == null) Flow.Log($"open {what}: {presenter} did not open, top={Flow.TopPopup()} {Flow.PopupState()}");
        }

        // The diver's interaction sensor is small and the straight walk can end short of the NPC (a counter, a stool): try
        // NavMesh standing points around it, nearest first, until the focus is on it, then hold F.
        static IEnumerator Approach(Component npc, string what)
        {
            var me = Flow.MyCharacter();
            if (me == null || npc == null) yield break;
            var cands = new List<Vector3>();
            var center = npc.transform.position;
            for (float r = 0.9f; r <= 2.1f; r += 0.4f)
                for (int i = 0; i < 16; i++)
                {
                    var p = center + new Vector3(Mathf.Cos(i * Mathf.PI / 8f), 0f, Mathf.Sin(i * Mathf.PI / 8f)) * r;
                    if (!UnityEngine.AI.NavMesh.SamplePosition(p, out var h, 0.25f, UnityEngine.AI.NavMesh.AllAreas)) continue;
                    var path = new UnityEngine.AI.NavMeshPath();
                    if (!UnityEngine.AI.NavMesh.CalculatePath(me.transform.position, h.position, UnityEngine.AI.NavMesh.AllAreas, path) || path.status != UnityEngine.AI.NavMeshPathStatus.PathComplete) continue;
                    cands.Add(h.position);
                }
            var picked = new List<Vector3>();
            foreach (var c in cands.OrderBy(c => Flow.Flat(c - center).magnitude))
                if (picked.All(q => Flow.Flat(q - c).magnitude > 0.6f)) picked.Add(c);
            Flow.Log($"approach {what}: {cands.Count} standing points, trying {Mathf.Min(6, picked.Count)}: {string.Join(" ", picked.Take(6).Select(c => c.ToString()))}");
            foreach (var spot in picked.Take(6))
            {
                yield return Flow.WalkTo(() => spot, 0.25f, 8f, $"{what} spot");
                if (npc) Flow.Aim(npc.transform.position + Vector3.up * 0.8f);
                yield return Flow.WaitUntil(() => Flow.Focused(npc), 1.5f, $"focus {what} from {spot}");
                var now = Flow.MyCharacter();
                Flow.Log($"approach {what}: at {(now ? now.transform.position.ToString() : "-")} d={(now && npc ? Flow.Flat(npc.transform.position - now.transform.position).magnitude : -1):F2} focus={Flow.Focus()}");
                if (Flow.Focused(npc)) { yield return Flow.Keys(0.7f, Key.F); yield break; }
            }
        }

        static readonly HashSet<int> s_diagged = new HashSet<int>();

        // Why the diver cannot get its interaction focus on an NPC: where it stands, the NavMesh path, what is in the way.
        static IEnumerator DiagNpc(Component npc, string what)
        {
            var me = Flow.MyCharacter();
            if (me == null || npc == null || !s_diagged.Add(Flow.NpcId(npc))) yield break;
            var from = me.transform.position;
            var to = npc.transform.position;
            Flow.Log($"diag {what}: me={from} npc={Flow.NpcId(npc)}@{to} flat d={Flow.Flat(to - from).magnitude:F2} focus={Flow.Focus()}");
            var path = new UnityEngine.AI.NavMeshPath();
            bool near = UnityEngine.AI.NavMesh.SamplePosition(to, out var hit, 3f, UnityEngine.AI.NavMesh.AllAreas);
            bool ok = near && UnityEngine.AI.NavMesh.CalculatePath(from, hit.position, UnityEngine.AI.NavMesh.AllAreas, path);
            Flow.Log($"diag {what}: navmesh near npc={(near ? hit.position.ToString() : "none")} path={ok}/{path.status} corners=[{string.Join(" ", path.corners.Select(c => c.ToString()))}]");
            var around = Physics.OverlapSphere(to, 2.5f, ~0, QueryTriggerInteraction.Collide)
                .Select(c => $"{c.name}:{LayerMask.LayerToName(c.gameObject.layer)}{(c.isTrigger ? "/trig" : "")}@{c.bounds.center}")
                .Distinct().Take(25);
            Flow.Log($"diag {what}: colliders within 2.5 m of the npc: {string.Join(" | ", around)}");
            var dir = (to - from); dir.y = 0;
            var ray = Physics.RaycastAll(from + Vector3.up * 0.4f, dir.normalized, dir.magnitude, ~0, QueryTriggerInteraction.Ignore)
                .Select(h => $"{h.collider.name}:{LayerMask.LayerToName(h.collider.gameObject.layer)}@{h.point}");
            Flow.Log($"diag {what}: on the line to the npc: {string.Join(" | ", ray)}");
            yield return Review.Shot($"a9b-stuck-{Flow.NpcId(npc)}");
        }

        // The same without a presenter to wait for (a function that only toasts).
        static IEnumerator OpenFunctionRaw(int npcId, string function, string what, Action<bool> result)
        {
            var npc = Flow.FindNpc(npcId);
            if (npc == null) { result(false); yield break; }
            yield return Flow.InteractNpc(npc, what);
            if (!Flow.Focused(npc) && Flow.Find("DialogPopupView") == null && Flow.Find("SelectionPopupView") == null)
            {
                yield return DiagNpc(npc, what);
                yield return Approach(npc, what);
            }
            yield return Flow.WaitUntil(() => Flow.Find("DialogPopupView") != null || Flow.Find("SelectionPopupView") != null, 4f, $"{what} reaction");
            if (Flow.Find("DialogPopupView") != null)
            {
                yield return Flow.AdvanceDialogs(() => Flow.Find("DialogPopupView") == null, 120f, $"{what} dialog");
                yield return new WaitForSecondsRealtime(1f);
                yield return Flow.InteractNpc(npc, what);
                yield return Flow.WaitUntil(() => Flow.Find("SelectionPopupView") != null, 4f, $"{what} selection");
            }
            if (Flow.Find("SelectionPopupView") != null)
            {
                yield return new WaitForSecondsRealtime(0.8f);
                yield return Flow.SelectFunction(npc, function);
                result(true);
            }
            else result(false);
        }

        // Esc closes a panel, a second Esc for one that first leaves a sub-page; then nothing is left open.
        static IEnumerator Esc(string presenter)
        {
            for (int i = 0; i < 3 && Flow.Find(presenter) != null; i++)
            {
                yield return Flow.Keys(0.2f, Key.Escape);
                yield return Flow.WaitUntil(() => Flow.Find(presenter) == null, 3f, $"{presenter} closed by Esc");
            }
            if (Flow.Find(presenter) != null)
            {
                Flow.Log($"BUG? Esc left {presenter} open, top={Flow.TopPopup()} {Flow.PopupState()}");
                var p = Flow.Find(presenter);
                p.GetType().GetMethod("Close", new[] { typeof(object) })?.Invoke(p, new object[] { null });
                yield return new WaitForSecondsRealtime(1f);
            }
            yield return Flow.WaitUntil(() => Flow.TopPopup() == null, 5f, $"{presenter} popups gone");
            yield return new WaitForSecondsRealtime(0.5f);
        }

        // ---- clicking panel parts ----

        // The slot presenters read the pointer-click event (left/right), not Button.onClick alone: `left` sends that event.
        static IEnumerator ClickPresenter(string type, Func<MonoBehaviour, bool> pick, string what, bool left = false)
        {
            MonoBehaviour found = null;
            yield return Flow.WaitUntil(() => (found = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
                .FirstOrDefault(m => m != null && m.isActiveAndEnabled && m.GetType().Name == type && pick(m)
                    && Flow.Get(m, "Button") is UnityEngine.UI.Button b && Flow.Ready(b))) != null, 8f, what);
            if (found == null) { Flow.Log($"click {what}: no {type}"); yield break; }
            yield return new WaitForSecondsRealtime(0.3f);
            Flow.Log($"click {what}");
            var button = (UnityEngine.UI.Button)Flow.Get(found, "Button");
            if (!left) { button.onClick.Invoke(); yield break; }
            var data = new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current) { button = UnityEngine.EventSystems.PointerEventData.InputButton.Left, clickCount = 1 };
            UnityEngine.EventSystems.ExecuteEvents.Execute(button.gameObject, data, UnityEngine.EventSystems.ExecuteEvents.pointerClickHandler);
        }

        static IEnumerator ClickView(string viewType, string button, string what)
        {
            yield return Flow.WaitUntil(() => Flow.Find(viewType) != null && Flow.Ready(Flow.Button(Flow.Find(viewType), button)), 8f, what);
            if (!Flow.LastWaitOk) yield break;
            yield return new WaitForSecondsRealtime(0.3f);
            Flow.Log($"click {what}");
            Flow.Button(Flow.Find(viewType), button).onClick.Invoke();
        }

        // The two-button TextPopup: press Confirm.
        static IEnumerator ConfirmText(string what)
        {
            yield return ClickView("TextPopupView", "Confirm_Button", $"{what} confirmation");
            yield return Flow.WaitUntil(() => Flow.Find("TextPopupView") == null, 4f, $"{what} confirmation closed");
        }

        // ---- tycoon ----

        static IEnumerator ClickTab(string tab)
        {
            yield return ClickPresenter("TycoonTabPresenter", m => Flow.Get(Flow.Get(m, "_model"), "Tab")?.ToString() == tab, $"tycoon tab {tab}");
            yield return Settle(1f);
        }

        static IEnumerator Hire(int employeeId, long cost)
        {
            long gold = Gold();
            int count = (Flow.Get(Flow.Get(PM(), "Employees"), "Count") as int?) ?? 0;
            yield return ClickTab("EmployeeHire");
            yield return Flow.WaitUntil(() => Flow.Find("EmployeeHirePagePresenter") != null, 5f, "hire page");
            yield return Settle(1f);
            yield return ClickPresenter("EmployeeCardPresenter", m => Flow.Get(Flow.Get(m, "_model"), "EmployeeId") is int id && id == employeeId
                && Flow.Get(Flow.Get(m, "_model"), "SlotType")?.ToString() == "Hire", $"recruit card {employeeId}");
            yield return Flow.WaitUntil(() => Flow.Find("TycoonHireConfirmPopupView") != null, 5f, "hire confirm popup");
            yield return Settle(0.8f);
            yield return Review.Shot($"a9b-tycoon-hire-confirm-{employeeId}");
            yield return ClickView("TycoonHireConfirmPopupView", "Confirm_Button", "hire confirm");
            yield return Flow.WaitUntil(() => DictGet(Flow.Get(PM(), "Employees"), employeeId) != null, 8f, $"employee {employeeId} hired");
            Check(DictGet(Flow.Get(PM(), "Employees"), employeeId) != null, $"employee {employeeId} is in PlayerContext.Employees");
            Check(DictGet(Flow.Get(PM(), "RecruitableEmployees"), employeeId) == null, $"employee {employeeId} left the recruit list");
            Check(Gold() == gold - cost, $"hire {employeeId}: gold {gold} -> {Gold()}, want -{cost}");
            Check(((Flow.Get(Flow.Get(PM(), "Employees"), "Count") as int?) ?? 0) == count + 1, "employee count +1");
            yield return Flow.WaitUntil(() => Flow.Find("TycoonHireConfirmPopupView") == null, 4f, "hire popup closed");
            yield return ClickTab("Main");
        }

        static IEnumerator ClickMain(string button, string what)
        {
            yield return Flow.WaitUntil(() => Flow.Find("TycoonMainPagePresenter") != null && Flow.Ready(Flow.Get(Flow.Get(Flow.Find("TycoonMainPagePresenter"), "_view"), button) as UnityEngine.UI.Button), 8f, what);
            var b = Flow.Get(Flow.Get(Flow.Find("TycoonMainPagePresenter"), "_view"), button) as UnityEngine.UI.Button;
            if (b == null || !Flow.Ready(b)) { Flow.Log($"click {what}: {button} not ready"); yield break; }
            yield return new WaitForSecondsRealtime(0.3f);
            Flow.Log($"click {what}");
            b.onClick.Invoke();
        }

        // The menu is open on this slot (it was opened at its NPC, or it is the first slot): artifact, employee, start.
        static IEnumerator PlaceOnSlot(int slotId, int employeeId, int artifactId)
        {
            Assert.IsNotNull(SalesSlot(slotId), $"sales slot {slotId} missing");
            yield return ClickMain("BtnSetArtifact", $"slot {slotId} set artifact");
            yield return Flow.WaitUntil(() => Flow.Find("TycoonArtifactSettingPopupPresenter") != null, 6f, "artifact setting popup");
            yield return Settle(1.5f);
            yield return ClickPresenter("InventoryGoodsSlotPresenter", m =>
            {
                var model = Flow.Get(m, "_model");
                var gs = Flow.Value(model, "RxGoodsSlot");
                return gs != null && Flow.Get(gs, "IsEmpty") is false && Flow.Get(gs, "Type")?.ToString() == "Artifact" && (int)Flow.Get(gs, "Id") == artifactId
                    && Flow.Get(model, "SlotCategory")?.ToString() is "InventorySlot" or "StashSlot";
            }, "artifact in the picker");
            yield return ConfirmText("artifact pick");
            yield return Flow.WaitUntil(() => Flow.Get(SalesSlot(slotId), "Goods") != null, 8f, $"slot {slotId} artifact");
            Check(Flow.Get(SalesSlot(slotId), "Goods") != null && (int)Flow.Get(Flow.Get(SalesSlot(slotId), "Goods"), "Id") == artifactId, $"slot {slotId} holds artifact {artifactId}");
            yield return Flow.WaitUntil(() => Flow.Find("TycoonArtifactSettingPopupPresenter") == null, 4f, "artifact popup closed");
            yield return Settle(0.8f);

            yield return ClickMain("BtnSettingEmployee", $"slot {slotId} set employee");
            yield return Flow.WaitUntil(() => Flow.Find("EmployeeSettingPopupPresenter") != null, 6f, "employee setting popup");
            yield return Settle(1.5f);
            yield return ClickPresenter("EmployeeCardPresenter", m => Flow.Get(Flow.Get(m, "_model"), "EmployeeId") is int id && id == employeeId
                && Flow.Get(Flow.Get(m, "_model"), "SlotType")?.ToString() == "Setting", $"employee card {employeeId}");
            yield return ConfirmText("employee pick");
            yield return Flow.WaitUntil(() => (int)Flow.Get(SalesSlot(slotId), "EmployeeId") == employeeId, 8f, $"slot {slotId} employee");
            Check((int)Flow.Get(SalesSlot(slotId), "EmployeeId") == employeeId, $"slot {slotId} employee {employeeId}");
            yield return Flow.WaitUntil(() => Flow.Find("EmployeeSettingPopupPresenter") == null, 4f, "employee popup closed");
            yield return Settle(0.8f);

            yield return ClickMain("BtnStart", $"slot {slotId} start");
            yield return Flow.WaitUntil(() => Flow.Get(SalesSlot(slotId), "IsStarted") is true, 6f, $"slot {slotId} started");
        }

        static IEnumerator DismissTycoonResult()
        {
            for (int i = 0; i < 12; i++)
            {
                string top = Flow.TopPopup();
                if (top == "TycoonMenuPopupView" || top == null) { if (Flow.Find("TycoonResultPopupView") == null && Flow.Find("EmployeeLevelUpPopupView") == null) yield break; }
                var result = Flow.Find("TycoonResultPopupView");
                var up = Flow.Find("EmployeeLevelUpPopupView");
                if (result != null && Flow.Ready(Flow.Button(result, "Confirm_Button"))) Flow.Button(result, "Confirm_Button").onClick.Invoke();
                else if (up != null && Flow.Ready(Flow.Button(up, "Confirm_Btn"))) Flow.Button(up, "Confirm_Btn").onClick.Invoke();
                else if (result != null || up != null) yield return Flow.Keys(0.2f, Key.Escape);
                yield return new WaitForSecondsRealtime(1f);
            }
        }

        // ---- interior and the ATM ----

        static IEnumerator BuyInterior(int shopId)
        {
            var shop = Row("GameCommon.Table.TInteriorShop", shopId);
            int area = (int)Flow.Get(shop, "AreaId");
            string areaName = Flow.Get(Row("GameCommon.Table.TArea", area), "Name") as string;
            long coin = Coin();
            int price = (int)Flow.Get(shop, "ConsumeCoin");
            yield return ClickPresenter("TextSlotPresenter", m => Flow.Get(Flow.Get(m, "_model"), "Text") as string == areaName, $"interior area {areaName}");
            yield return Settle(1f);
            yield return ClickPresenter("InteriorSlotPresenter", m => Flow.Get(Flow.Get(m, "_model"), "InteriorShopId") is int id && id == shopId, $"interior item {shopId}");
            yield return ClickView("ConsumeConfirmPopupView", "Confirm_Button", $"interior {shopId} confirm");
            yield return Flow.WaitUntil(() => Coin() == coin - price, 10f, $"interior {shopId} paid");
            Check(Coin() == coin - price, $"interior {shopId}: coins {coin} -> {Coin()}, want -{price}");
            // The unlock plays a transition (PlayInteriorTransition: blur off, fade, camera, NpcLevel/decoration applied, fade
            // back, blur on) and the popup takes no Esc until it ends.
            var popup = Flow.Find("InteriorPopupPresenter");
            yield return Flow.WaitUntil(() => Flow.Get(popup, "IsBlurEffect") is false, 20f, $"interior {shopId} transition starts");
            yield return Flow.WaitUntil(() => Flow.Get(popup, "IsBlurEffect") is true, 120f, $"interior {shopId} transition ends");
            yield return Settle(1.5f);
        }

        static object Row(string tableType, int id)
        {
            var table = GameType(tableType)?.GetProperty("Table")?.GetValue(null);
            try { return table?.GetType().GetProperty("Item")?.GetValue(table, new object[] { id }); }
            catch (System.Reflection.TargetInvocationException) { return null; }
        }

        // The coin exchange popup is open: set the slider to the amount (as dragging it does) and Confirm.
        static IEnumerator Exchange(long amount, bool toItem)
        {
            var popup = Flow.Find("CoinExchangePopupPresenter");
            var view = Flow.Find("CoinExchangePopupView");
            if (popup == null || view == null) { Check(false, "coin exchange popup is not open"); yield break; }
            long coin = Coin(), item = ItemCount(10001);
            if (!toItem)
            {
                var toggle = Flow.Get(view, "ToggleGroup_ToggleOut") as UnityEngine.UI.Toggle;
                toggle.isOn = true;
                yield return Settle(0.8f);
            }
            var slider = Flow.Get(view, "Quantity_Slider") as UnityEngine.UI.Slider;
            yield return Flow.WaitUntil(() => slider.maxValue >= amount, 5f, "exchange slider range");
            slider.value = amount;
            yield return Settle(0.4f);
            Flow.Log($"exchange {(toItem ? "coins -> item 10001" : "item 10001 -> coins")}: slider={slider.value}/{slider.maxValue}");
            yield return Flow.Click(view, "Confirm_Button");
            yield return Flow.WaitUntil(() => Coin() != coin, 8f, "coins changed");
            yield return Flow.WaitUntil(() => ItemCount(10001) != item, 4f, "item 10001 changed");
            yield return Settle(0.8f);
        }

        // ---- talent, artifact shop, shop ----

        static IEnumerator UnlockTalent(int talentId)
        {
            yield return ClickPresenter("TalentSlotPresenter", m => Flow.Get(Flow.Get(m, "_model"), "TalentId") is int id && id == talentId, $"talent {talentId}");
            yield return Settle(1f);
            var panel = Flow.Find("TalentInfoPanelPresenter");
            var view = Flow.Get(panel, "_view");
            var upgrade = Flow.Get(view, "Upgrade_Button") as UnityEngine.UI.Button;
            yield return Flow.WaitUntil(() => Flow.Ready(upgrade), 5f, "talent Upgrade button");
            Flow.Log("click talent Upgrade");
            upgrade.onClick.Invoke();
            yield return ClickView("TalentConfirmPopupView", "Confirm_Button", "talent confirm");
            yield return Flow.WaitUntil(() => HasTalent(talentId), 8f, $"talent {talentId}");
            yield return Settle(1f);
        }

        static IEnumerator SellArtifactToPim()
        {
            yield return ClickPresenter("InventoryGoodsSlotPresenter", m =>
            {
                var model = Flow.Get(m, "_model");
                var gs = Flow.Value(model, "RxGoodsSlot");
                return gs != null && Flow.Get(gs, "IsEmpty") is false && Flow.Get(gs, "Type")?.ToString() == "Artifact"
                    && Flow.Get(model, "SlotCategory")?.ToString() is "InventorySlot" or "StashSlot";
            }, "artifact to sell");
            yield return ConfirmText("sell to Pim");
            yield return Flow.WaitUntil(() => CountArtifacts() == 0, 8f, "artifact sold");
            yield return Settle(1f);
        }

        // The shop panel is open: left-click the product (quantity popup, 1) and confirm.
        static IEnumerator BuyProduct(int productId)
        {
            var product = Row("GameCommon.Table.TShopProduct", productId);
            int gainId = (int)Flow.Get(Flow.Get(product, "GainGoodsData"), "Id");
            long before = ItemCount(gainId);
            yield return ClickPresenter("ShopProductSlotPresenter", m => Flow.Get(Flow.Get(m, "_model"), "Id") is int id && id == productId, $"shop product {productId}", left: true);
            yield return ClickView("GoodsQuantityPopupView", "Confirm_Button", "quantity confirm");
            yield return Flow.WaitUntil(() => ItemCount(gainId) > before, 8f, $"product {productId} bought");
            yield return Settle(1f);
        }

        // ---- the dive ----

        class DiveReport
        {
            public bool AtmDone, ShopDone;
            public string AtmNote = "not reached", ShopNote = "not reached";
            public override string ToString() => $"atm={AtmDone} ({AtmNote}) shop={ShopDone} ({ShopNote})";
        }

        // Elara's campaign list, the campaign (difficulty arrows), the booth, the stage with `tasks`, the way out, the results.
        static IEnumerator DiveCampaign(int campaignMin, int campaignMax, string difficulty, IEnumerator tasks, DiveReport report)
        {
            yield return OpenFunction(700000, "Campaign", "CampaignPopupPresenter", "Elara campaign", ok => { });
            Assert.IsNotNull(Flow.Find("CampaignPopupPresenter"), "the campaign list did not open");
            yield return Settle(2f);
            int campaignId = 0;
            yield return PickCampaign(campaignMin, campaignMax, difficulty, id => campaignId = id);
            Assert.AreEqual(campaignId.ToString(), CampaignNow(), $"campaign {campaignId} did not start");
            yield return Settle(2f);
            for (int i = 0; i < 3 && BoothCheck() != "Success"; i++)
            {
                Flow.Log($"booth check={BoothCheck()}, talking to Elara");
                yield return Flow.Talk(700000, "Elara before the dive");
            }
            yield return EnterBooth();
            Assert.AreEqual("StageScene", Flow.ActiveScene, "the dive booth did not lead into the stage");
            yield return Settle(3f);
            s_lastHeal = 0f; s_heals = 0; s_ignored.Clear();
            Flow.Log($"stage {Flow.LoadState()} npcs=[{string.Join(",", Flow.Npcs().Select(n => $"{Flow.NpcId(n)}@{n.transform.position}"))}]");
            yield return Review.Shot($"a9b-dive-{campaignId}-start");
            yield return tasks;
            if (!Over()) yield return Escape();
            yield return AfterDive();
            Check(s_banner == "EscapeSuccessPopupView" || s_banner == "Escaped", $"the diver left by the game's own extraction: end banner {s_banner} ({MeState()})");
            yield return Results();
        }

        static string CampaignNow() => Flow.Value(Flow.Get(Flow.Find("GameManager"), "_gameContext"), "RxCampaignId")?.ToString();

        // The Normal tab (the list opens on Story while 1101 is new), then a campaign of the wanted range: the Normal list is
        // seven random picks for the user level (GameCampaignManager.RefreshPlayableNormalCampaignIds).
        static IEnumerator PickCampaign(int campaignMin, int campaignMax, string difficulty, Action<int> chosen)
        {
            var popup = Flow.Find("CampaignPopupPresenter");
            var toggle = Flow.Get(Flow.Get(popup, "_view"), "CategoryCommon_Toggle_Normal") as UnityEngine.UI.Toggle;
            Assert.IsNotNull(toggle, "no Normal tab in the campaign list");
            toggle.isOn = true;
            yield return Settle(2f);
            Func<List<MonoBehaviour>> listSlots = () => Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
                .Where(m => m != null && m.isActiveAndEnabled && m.GetType().Name == "CampaignSlotPresenter").ToList();
            yield return Flow.WaitUntil(() => listSlots().Count > 0, 6f, "Normal campaign slots");
            var slots = listSlots();
            Flow.Log($"campaign slots=[{string.Join(",", slots.Select(s => Flow.Get(Flow.Get(s, "_model"), "CampaignId")))}] want {campaignMin}-{campaignMax}");
            var slot = slots.Where(s => Flow.Get(Flow.Get(s, "_model"), "CampaignId") is int id && id >= campaignMin && id <= campaignMax)
                .OrderBy(s => (int)Flow.Get(Flow.Get(s, "_model"), "CampaignId")).FirstOrDefault();
            Assert.IsNotNull(slot, $"no campaign of {campaignMin}-{campaignMax} in the list");
            int campaignId = (int)Flow.Get(Flow.Get(slot, "_model"), "CampaignId");
            chosen(campaignId);
            var slotButton = Flow.Get(slot, "Button") as UnityEngine.UI.Button;
            yield return Flow.WaitUntil(() => Flow.Ready(slotButton), 5f, $"campaign slot {campaignId}");
            slotButton.onClick.Invoke();
            MonoBehaviour detail = null;
            yield return Flow.WaitUntil(() => (detail = Flow.Find("CampaignDetailPanelPresenter")) != null
                && Flow.Get(Flow.Get(detail, "_model"), "CampaignId") is int id && id == campaignId, 6f, $"campaign {campaignId} detail");
            Assert.IsNotNull(detail, "campaign detail did not open");
            yield return Settle(1.5f);
            var view = Flow.Get(detail, "_view");
            var ctx = Flow.Get(Flow.Find("GameManager"), "_gameContext");
            var order = new[] { "Easy", "Normal", "Hard", "Insane" };
            for (int i = 0; i < 3 && Flow.Value(ctx, "RxDifficulty")?.ToString() != difficulty; i++)
            {
                bool up = Array.IndexOf(order, Flow.Value(ctx, "RxDifficulty")?.ToString()) < Array.IndexOf(order, difficulty);
                var b = Flow.Get(view, up ? "Info_Difficulty_NextButton" : "Info_Difficulty_PreButton") as UnityEngine.UI.Button;
                yield return Flow.WaitUntil(() => Flow.Ready(b), 3f, $"difficulty {(up ? "next" : "previous")}");
                if (!Flow.LastWaitOk) break;
                b.onClick.Invoke();
                yield return Settle(0.6f);
            }
            Flow.Log($"difficulty={Flow.Value(ctx, "RxDifficulty")} (wanted {difficulty})");
            var start = Flow.Get(view, "Info_StartCampaignButton") as UnityEngine.UI.Button;
            yield return Flow.WaitUntil(() => Flow.Ready(start), 5f, "Start Campaign interactable");
            start.onClick.Invoke();
            yield return Flow.WaitUntil(() => CampaignNow() == campaignId.ToString(), 10f, "campaign started");
            yield return Flow.WaitUntil(() => Flow.TopPopup() == null, 15f, "campaign popups gone");
            if (Flow.Find("CampaignPopupPresenter") != null) yield return Flow.Keys(0.2f, Key.Escape);
            yield return Settle(1f);
        }

        static string BoothCheck()
        {
            var booth = Flow.Find("PhoneBooth");
            var gm = Flow.Find("GameManager");
            if (booth == null || gm == null) return "no booth";
            var id = Flow.Get(gm, "LocalClientId");
            try { return booth.GetType().GetMethod("CheckCanEnter")?.Invoke(booth, new[] { id })?.ToString() ?? "?"; }
            catch (Exception e) { return $"threw {e.InnerException?.GetType().Name}"; }
        }

        static IEnumerator EnterBooth()
        {
            var booth = Flow.Find("PhoneBooth");
            Assert.IsNotNull(booth, "no PhoneBooth in the lounge");
            for (int attempt = 0; attempt < 3 && Me()?.GetType() != null && State() != "StageReady" && Flow.ActiveScene == "LoungeScene"; attempt++)
            {
                yield return Flow.InteractObject(booth, 0.8f, "dive booth");
                yield return Flow.WaitUntil(() => State() == "StageReady" || Flow.ActiveScene != "LoungeScene", 4f, "StageReady");
                Flow.Log($"booth attempt {attempt}: state={State()} check={BoothCheck()} focus={Flow.Focus()}");
            }
            yield return Flow.WaitUntil(() => Flow.ActiveScene == "StageScene" && Flow.StageState() == "Playing", 240f, "stage Playing");
        }

        static Component Me() => Flow.MyCharacter();
        static string State() => Flow.Get(Me(), "State")?.ToString() ?? "-";
        static bool Over() => State() == "Dead" || State() == "Escaped" || Flow.ActiveScene != "StageScene" || Flow.StageState() == "Ended";
        static bool Down() => Over() || State() == "Dying";

        static string MeState()
        {
            var me = Me();
            return me == null ? "me=-" : $"hp={Flow.Hp(me)}/{(Flow.Get(me, "HpMax") is float f ? f : 0):F0} stress={(Flow.Get(me, "Stress") is float s ? s : 0):F0} state={State()} pos={me.transform.position}";
        }

        // At 0 HP the diver lies dying (death's door) until it revives or dies; nothing to press meanwhile.
        static IEnumerator WhileDying()
        {
            if (State() != "Dying") yield break;
            Flow.Log($"dying {MeState()}");
            yield return Flow.WaitUntil(() => State() != "Dying" || Over(), 240f, "out of Dying");
        }

        // What the dive does in the stage: ATM 700152, then a Woung (nearest of the chapter's two kinds).
        static IEnumerator DiveTasks(DiveReport report, long coinsToExchange, (int npc, int product, int item, int price)[] shops)
        {
            yield return Settle(2f);
            var me = Me();
            var atm = Flow.FindNpc(700152);
            var woung = shops.Select(s => (s, npc: Flow.FindNpc(s.npc))).Where(x => x.npc != null)
                .OrderBy(x => me ? Flow.Flat(x.npc.transform.position - me.transform.position).magnitude : 0f).FirstOrDefault();
            if (atm == null) { report.AtmNote = "no NPC 700152 in this stage"; Flow.Log($"dive: {report.AtmNote}"); }
            if (woung.npc == null) { report.ShopNote = $"none of {string.Join("/", shops.Select(s => s.npc))} in this stage"; Flow.Log($"dive: {report.ShopNote}"); }
            // The nearer of the two first: the diver is a level-1 character and every metre of the stage costs HP.
            var order = new List<bool>();
            if (atm != null && woung.npc != null)
            {
                float dAtm = Flow.Flat(atm.transform.position - me.transform.position).magnitude, dShop = Flow.Flat(woung.npc.transform.position - me.transform.position).magnitude;
                Flow.Log($"dive: ATM at {atm.transform.position} d={dAtm:F0}, Woung {woung.s.npc} at {woung.npc.transform.position} d={dShop:F0}");
                order.AddRange(dAtm <= dShop ? new[] { true, false } : new[] { false, true });
            }
            else if (atm != null) order.Add(true);
            else if (woung.npc != null) order.Add(false);
            foreach (bool isAtm in order)
            {
                if (Over()) { if (isAtm) report.AtmNote = "the dive ended before the ATM"; else report.ShopNote = "the dive ended before the shop"; continue; }
                if (isAtm) yield return DiveAtm(report, coinsToExchange);
                else yield return DiveShop(report, woung.s);
            }
        }

        static IEnumerator DiveAtm(DiveReport report, long coinsToExchange)
        {
            bool reached = false;
            yield return GoTo(() => Flow.FindNpc(700152), 1.6f, 700f, "ATM 700152", r => reached = r);
            if (!reached) { report.AtmNote = $"not reached: {MeState()}"; yield break; }
            bool opened = false;
            yield return OpenFunction(700152, "CoinExchange", "CoinExchangePopupPresenter", "dive ATM", r => opened = r);
            if (!opened) { report.AtmNote = "CoinExchange did not open"; yield break; }
            yield return Settle(1f);
            yield return Review.Shot("a9b-atm-dive");
            long coin = Coin(), item = ItemCount(10001);
            yield return Exchange(coinsToExchange, toItem: true);
            report.AtmDone = Coin() == coin - coinsToExchange && ItemCount(10001) == item + coinsToExchange;
            report.AtmNote = $"coins {coin} -> {Coin()}, item 10001 {item} -> {ItemCount(10001)}, want -{coinsToExchange}/+{coinsToExchange}";
            yield return Esc("CoinExchangePopupPresenter");
        }

        static IEnumerator DiveShop(DiveReport report, (int npc, int product, int item, int price) shop)
        {
            bool near = false;
            yield return GoTo(() => Flow.FindNpc(shop.npc), 1.6f, 400f, $"Woung {shop.npc}", r => near = r);
            if (!near) { report.ShopNote = $"Woung {shop.npc} not reached: {MeState()}"; yield break; }
            bool shopOpen = false;
            yield return OpenFunction(shop.npc, "Shop", "ShopPopupPresenter", $"Woung {shop.npc}", r => shopOpen = r);
            if (!shopOpen) { report.ShopNote = $"Woung {shop.npc} Shop did not open"; yield break; }
            yield return Settle(1.5f);
            yield return Review.Shot($"a9b-woung-{shop.npc}");
            long coins10001 = ItemCount(10001), got = ItemCount(shop.item);
            yield return BuyProduct(shop.product);
            report.ShopDone = ItemCount(shop.item) == got + 1 && ItemCount(10001) == coins10001 - shop.price;
            report.ShopNote = $"Woung {shop.npc} product {shop.product}: item {shop.item} {got} -> {ItemCount(shop.item)} (want +1), item 10001 {coins10001} -> {ItemCount(10001)} (want -{shop.price})";
            yield return Esc("ShopPopupPresenter");
        }

        // Walk to an NPC (it stands anywhere on the map), fighting what comes at the diver on the way.
        static IEnumerator GoTo(Func<Component> target, float arrive, float budget, string what, Action<bool> reached)
        {
            float start = Time.realtimeSinceStartup, nextLog = start + 15f;
            while (Time.realtimeSinceStartup - start < budget && !Over())
            {
                var t = target();
                var me = Me();
                if (t == null || me == null) break;
                if (State() == "Dying") { yield return WhileDying(); continue; }
                if (Flow.Find("DialogPopupView") != null) { yield return Flow.AdvanceDialogs(() => Flow.Find("DialogPopupView") == null, 60f, "dialog in the dive"); continue; }
                if (Flow.Flat(t.transform.position - me.transform.position).magnitude <= arrive) { reached(true); yield break; }
                // Run past what is not in the way (zombies are slower than a running diver); fight what is on top of it.
                yield return MaybeHeal();
                var m = Enemy(3f);
                if (m != null) { yield return Fight(m); continue; }
                if (Time.realtimeSinceStartup > nextLog) { nextLog += 15f; Flow.Log($"go {what}: d={Flow.Flat(t.transform.position - me.transform.position).magnitude:F0} {MeState()}"); }
                var goal = t.transform.position;
                yield return Flow.Go(goal, arrive, 20f, what, run: true, stop: () => KeepAlive() || Enemy(2.5f) != null || Down());
            }
            reached(false);
        }

        static readonly HashSet<int> s_ignored = new HashSet<int>();
        static float s_lastHeal;
        static int s_heals;

        static float s_lastRefill;

        // The dive's grind cheat (see the header): HP refill under 75%, stress reset from 70. Called every frame of a walk.
        static bool KeepAlive()
        {
            var me = Me();
            if (me == null || State() == "Dying" || Over() || Time.realtimeSinceStartup - s_lastRefill < 0.15f) return false;
            float hpMax = Flow.Get(me, "HpMax") is float f ? f : 1f;
            if (Flow.Hp(me) < 0.75f * hpMax)
            {
                s_lastRefill = Time.realtimeSinceStartup;
                Send("ReqSetCharacterHp", ("Hp", hpMax));
                if (s_hpRefills++ % 10 == 0) Flow.Log($"cheat ReqSetCharacterHp Hp={hpMax} (refill #{s_hpRefills}) {MeState()}");
            }
            if (Convert.ToSingle(Flow.Get(me, "Stress") ?? 0f) >= 70f)
            {
                s_lastRefill = Time.realtimeSinceStartup;
                Send("ReqSetCharacterStress", ("Stress", 0f));
                Flow.Log($"cheat ReqSetCharacterStress Stress=0 (refill #{++s_stressRefills}) {MeState()}");
            }
            return false;
        }

        // Below 45% HP use a potion from the quick slots (keys 1, 2), as a player does.
        static IEnumerator MaybeHeal()
        {
            KeepAlive();
            var me = Me();
            if (me == null || State() == "Dying" || Over()) yield break;
            float hpMax = Flow.Get(me, "HpMax") is float f ? f : 1f;
            if (Flow.Hp(me) >= 0.45f * hpMax || Time.realtimeSinceStartup - s_lastHeal < 3f || s_heals >= 10) yield break;
            s_lastHeal = Time.realtimeSinceStartup;
            int before = Flow.Hp(me);
            var key = (s_heals++ % 2 == 0) ? Key.Digit1 : Key.Digit2;
            yield return Flow.Keys(0.15f, key);
            for (float w = 0f; w < 1.2f; w += Time.unscaledDeltaTime) { KeepAlive(); yield return null; }
            Flow.Log($"heal {key}: hp {before} -> {Flow.Hp(Me())}/{hpMax:F0} (potions used {s_heals})");
        }

        static string MonsterState(Component m) => Flow.Get(m, "UnitModel") is Component model ? Flow.Get(model, "State")?.ToString() : Flow.Get(m.GetComponent("MonsterModel"), "State")?.ToString() ?? "?";

        static Component Enemy(float range)
        {
            var me = Me();
            if (me == null) return null;
            return Flow.Monsters().Where(m => !s_ignored.Contains(m.GetInstanceID()) && MonsterState(m) != "Dead" && MonsterState(m) != "Dying" && Flow.Hp(m) <= 5000)
                .Select(m => (m, d: Flow.Flat(m.transform.position - me.transform.position).magnitude))
                .Where(x => x.d < range).OrderBy(x => x.d).Select(x => x.m).FirstOrDefault();
        }

        static Vector2 Aimed(Component m)
        {
            var cam = Camera.main;
            return cam ? (Vector2)cam.WorldToScreenPoint(m.transform.position + Vector3.up * 0.5f) : new Vector2(Screen.width / 2f, Screen.height / 2f);
        }

        // Fight like a player: close in, aim at the monster, left-click; every fourth swing a skill (right button, Q, E, R).
        static IEnumerator Fight(Component m, float reach = 12f)
        {
            float start = Time.realtimeSinceStartup, lastProgress = start;
            int clicks = 0, skillUses = 0, lastTargetHp = Flow.Hp(m);
            string name = m.name;
            Flow.Log($"fight {name} hp={Flow.Hp(m)} {MeState()}");
            while (Flow.Alive(m) && Time.realtimeSinceStartup - start < 45f && !Down())
            {
                if (Flow.Hp(m) != lastTargetHp) { lastTargetHp = Flow.Hp(m); lastProgress = Time.realtimeSinceStartup; }
                if (Time.realtimeSinceStartup - lastProgress > 10f) { s_ignored.Add(m.GetInstanceID()); Flow.Log($"fight {name}: no hit lands in 10 s, leaving it"); break; }
                if (Flow.Find("DialogPopupView") != null) { yield return Flow.AdvanceDialogs(() => Flow.Find("DialogPopupView") == null, 60f, "dialog in a fight"); continue; }
                if (Flow.TopPopup() != null) { yield return Flow.Keys(0.2f, Key.Escape); }
                KeepAlive();
                yield return MaybeHeal();
                var me = Me();
                float d = Flow.Flat(m.transform.position - me.transform.position).magnitude;
                if (d > reach) break;
                if (d > 1.8f) { yield return Flow.WalkTo(() => Flow.Alive(m) ? m.transform.position : (Vector3?)null, 1.5f, 2f, $"fight {name} approach"); continue; }
                Flow.Hold();
                Flow.Aim(m.transform.position + Vector3.up * 0.5f);
                clicks++;
                if (clicks % 4 == 0)
                {
                    int skill = (skillUses++) % 4;
                    if (skill == 0) yield return Flow.MouseClick(Aimed(m), MouseButton.Right);
                    else yield return Flow.Keys(0.12f, skill == 1 ? Key.Q : skill == 2 ? Key.E : Key.R);
                    yield return new WaitForSecondsRealtime(0.25f);
                }
                else yield return Flow.MouseClick(Aimed(m));
                yield return new WaitForSecondsRealtime(0.2f);
            }
            Flow.Log($"fight {name} end alive={Flow.Alive(m)} clicks={clicks} after {Time.realtimeSinceStartup - start:F1}s {MeState()}");
        }

        static Component NearestExit()
        {
            var me = Me();
            return Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
                .Where(m => m != null && m.isActiveAndEnabled && (m.GetType().Name == "WaveExit" || m.GetType().Name == "SafeExit") && Flow.Get(m, "ExitState")?.ToString() != "Finished")
                .OrderBy(m => me ? Flow.Flat(m.transform.position - me.transform.position).sqrMagnitude : 0f).FirstOrDefault();
        }

        static long ExitCost(int level) => level switch { 1 => 10, 2 => 15, 3 => 25, 4 => 40, 5 => 55, 6 => 65, _ => 0 };

        static string ExitState(Component e) => e == null ? "-" : $"{e.GetType().Name} {Flow.Get(e, "ExitState")} remain={Flow.Get(e, "StateRemainTime")}";

        // Find the terminal, hold F to activate it, hold out, stand in its field (A5.Escape).
        static IEnumerator Escape()
        {
            var exit = NearestExit();
            if (exit == null) { Flow.Log("escape: no exit in the stage"); yield break; }
            Flow.Log($"escape: {exit.name} at {exit.transform.position} {ExitState(exit)}");
            float start = Time.realtimeSinceStartup;
            while (!Over() && Time.realtimeSinceStartup - start < 420f && Flow.Get(exit, "ExitState")?.ToString() == "Deactivated")
            {
                if (State() == "Dying") { yield return WhileDying(); continue; }
                var m = Enemy(2.5f);
                if (m != null) { yield return Fight(m, 8f); continue; }
                yield return Flow.Go(exit.transform.position, 1.2f, 40f, "exit", run: true, stop: () => KeepAlive() || Enemy(2.5f) != null || Down());
                if (Me() == null || Flow.Flat(exit.transform.position - Me().transform.position).magnitude > 2f) continue;
                Flow.Aim(exit.transform.position);
                yield return Flow.WaitUntil(() => Flow.Focused(exit), 3f, "focus exit");
                // WaveExit.OnInteraction takes the same ExitCost item 10001 (TryConsumeUnlockGoods): top up the input item.
                long waveCost = ExitCost(Level());
                if (waveCost > 0 && ItemCount(10001) < waveCost) yield return GiveItem(10001, waveCost - ItemCount(10001));
                Flow.Hold(Key.F);   // ExitActivateInteractionTime 2 s; the refill keeps running while F is held
                float pressed = Time.realtimeSinceStartup;
                while (Time.realtimeSinceStartup - pressed < 2.6f && !Over()) { KeepAlive(); yield return null; }
                Flow.Hold();
                yield return new WaitForSecondsRealtime(0.5f);
                Flow.Log($"escape: after F {ExitState(exit)} {MeState()}");
            }
            if (Over() || Flow.Get(exit, "ExitState")?.ToString() == "Deactivated") { Flow.Log($"escape: terminal not activated {ExitState(exit)} {MeState()}"); yield break; }
            float hold = Time.realtimeSinceStartup, nextLog = hold;
            // A SafeExit (the campaign's own booth, SafeExit.OnInteraction) is already Activated: F on it takes the diver out.
            while (exit && exit.GetType().Name == "SafeExit" && !Over() && Time.realtimeSinceStartup - hold < 150f)
            {
                if (State() == "Dying") { yield return WhileDying(); continue; }
                KeepAlive();
                var m = Enemy(2.5f);
                if (m != null) { yield return Fight(m, 8f); continue; }
                yield return Flow.Go(exit.transform.position, 1.2f, 20f, "safe exit", run: true, stop: () => KeepAlive() || Enemy(2.5f) != null || Down());
                if (Me() == null || Flow.Flat(exit.transform.position - Me().transform.position).magnitude > 2f) continue;
                Flow.Aim(exit.transform.position);
                yield return Flow.WaitUntil(() => Flow.Focused(exit), 3f, "focus safe exit");
                // SafeExit.OnInteraction -> TryConsumeUnlockGoods: the booth costs item 10001 by user level (ExitCost.csv: 1 10, 2 15,
                // 3 25, 4 40, 5 55, 6 65). The Woung and the ATM already took the dive's 10001, so top it up (input item cheat).
                long exitCost = ExitCost(Level());
                if (exitCost > 0 && ItemCount(10001) < exitCost) yield return GiveItem(10001, exitCost - ItemCount(10001));
                // SafeExitInteractionTime 3 s (Const.csv): hold F past it (2.6 s was too short), until the dive is over.
                Flow.Hold(Key.F);
                float heldFrom = Time.realtimeSinceStartup;
                while (Time.realtimeSinceStartup - heldFrom < 6f && !Over()) { KeepAlive(); yield return null; }
                Flow.Hold();
                yield return new WaitForSecondsRealtime(0.5f);
                Flow.Log($"escape: after F on the SafeExit {ExitState(exit)} {MeState()}");
            }
            while (!Over() && Time.realtimeSinceStartup - hold < 180f)
            {
                if (State() == "Dying") { yield return WhileDying(); continue; }
                if (Time.realtimeSinceStartup > nextLog) { nextLog += 10f; Flow.Log($"escape: {ExitState(exit)} {MeState()}"); }
                KeepAlive();
                var m = Enemy(3.5f);
                if (m != null) { yield return Fight(m, 5f); continue; }
                if (exit && Flow.Flat(exit.transform.position - Me().transform.position).magnitude > 2.5f)
                    yield return Flow.WalkTo(() => exit ? exit.transform.position : (Vector3?)null, 1.5f, 3f, "back to the exit");
                for (float w = 0f; w < 0.3f; w += Time.unscaledDeltaTime) { KeepAlive(); yield return null; }
            }
            Flow.Log($"escape end {ExitState(exit)} {MeState()} stage={Flow.StageState()}");
        }

        // The banner (escape or Extraction Failed) and the way back to the lounge.
        static string s_banner = "";

        static IEnumerator AfterDive()
        {
            Flow.Log($"after dive {MeState()} stage={Flow.StageState()} scene={Flow.ActiveScene}");
            yield return Flow.WaitUntil(() => Flow.Find("EscapeFailPopupView") != null || Flow.Find("CampaignFailPopupView") != null
                || Flow.Find("EscapeSuccessPopupView") != null || Flow.ActiveScene != "StageScene", 180f, "dive end banner");
            s_banner = Flow.Find("EscapeSuccessPopupView") != null ? "EscapeSuccessPopupView" : Flow.Find("EscapeFailPopupView") != null ? "EscapeFailPopupView"
                : Flow.Find("CampaignFailPopupView") != null ? "CampaignFailPopupView" : State() == "Escaped" ? "Escaped" : "none:" + Flow.ActiveScene;
            Flow.Log($"dive end: banner={s_banner} top={Flow.TopPopup()} {MeState()}");
        }

        // Dive Results over the lounge: Confirm (or Skip) until it is gone and the lounge plays.
        static IEnumerator Results()
        {
            yield return Flow.WaitUntil(() => Flow.ActiveScene == "LoungeScene", 240f, "back in the lounge");
            yield return Flow.WaitUntil(() => Flow.Find("StageResultPopupView") != null, 120f, "Dive Results");
            var view = Flow.Find("StageResultPopupView");
            if (view == null) { Flow.Log($"no result popup: top={Flow.TopPopup()} {Flow.LoadState()}"); yield break; }
            yield return new WaitForSecondsRealtime(6f);
            for (int i = 0; i < 6 && Flow.Find("StageResultPopupView") != null; i++)
            {
                if (Flow.Ready(Flow.Button(view, "Confirm_Button"))) yield return Flow.Click(view, "Confirm_Button");
                else if (Flow.Ready(Flow.Button(view, "SkipButton"))) yield return Flow.Click(view, "SkipButton");
                yield return new WaitForSecondsRealtime(2f);
            }
            yield return Flow.WaitUntil(() => Flow.Find("StageResultPopupView") == null && Flow.StageState() == "Playing", 60f, "lounge after the dive");
            if (Flow.Find("DialogPopupView") != null)
                yield return Flow.AdvanceDialogs(() => Flow.Find("DialogPopupView") == null, 120f, "lounge after the dive");
            yield return new WaitForSecondsRealtime(3f);
            Flow.RouteInputToGame();
        }
    }
}
