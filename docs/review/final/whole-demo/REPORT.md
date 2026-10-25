# Review độc lập: chuỗi từ màn hình title đến story cuối của demo

Ngày 2026-10-26. Người review không tham gia dựng. Chỉ đọc test, file kết quả NUnit, log; không chạy Unity, không sửa code.

**CÒN LỖ HỔNG: 8 mục**

Câu hỏi: có chuỗi liền, có test chứng minh, từ title đến 1202 không, và mọi nội dung demo có một lần chạy mới, xanh, không lỗi game không?
Trả lời: chưa. Bằng chứng nặng nhất: lần chạy đầy đủ `A6_BossFlow` (`captures/review/a6-full8-results.xml`, 2026-10-25 18:21Z đến 19:02Z) ra **4/7 đạt, 3 đỏ** (1101, 1201, 1202), không phải 7/7. Ngoài ra chuỗi 1102 đến 1202 được nối bằng cheat đánh dấu đã clear, hai trận boss bị ép HP về 1, và recorder lỗi game bị vô hiệu ở A1 và A4.

## Bảng 1. Chuỗi title đến 1202

| Mắt xích | Test chứng minh | Test assert gì | Cheat dùng | Đánh giá |
|---|---|---|---|---|
| Title | `A1_TitleFlow.BootToTitle` | Chỉ chụp ảnh title sau 24 s rồi `Assert.Pass` (A1_TitleFlow.cs:226). Không bấm nút nào. | Không | Đạt nhưng mỏng. GameLog không chạy (xem Lỗ hổng 4). |
| Title, New Game, tutorial 1100, về lounge | `A4_TutorialFlow.NewGameToTutorial` | Từng bước UI (name, difficulty, prologue), `s_missing` rỗng (CallCollision_1..9, TalkToEll), `Assert.Pass` (A4:226). Khối "về lounge" nằm trong `if (ActiveScene == "LoungeScene")` (A4:~195) nên **không assert đã về lounge, không assert 1100 đã clear**. | Không có cheat (grep `cheat` trong A4: 0). | Đạt có điều kiện. Log A4 có `scene=LoungeScene` ở dòng 14444, nhưng test không buộc điều đó. |
| Tutorial sang 1101 | Không có test nối. Mọi test sau chạy từ `tools/fixtures/saves-post-tutorial`, chép tay một lần (commit d71612fa, 2026-10-24). | Fixture chỉ có `ClearedCampaignFlags = 2^38` (bit của 1100 theo bảng Campaign, `BitFlagIndex` 38), level 1, 1500 gold. Có vẻ do game ghi (có `.sav` đúng định dạng), nhưng không có script sinh, không có kiểm tra khớp đầu ra A4 hiện tại. | Không | Khoảng trống nhỏ: không chứng minh được A4 hiện tại cho ra đúng fixture này. |
| 1101 cleared, mở 1102 | `A6_BossFlow.StoryCampaign1101` (Case1101, A6_BossFlow.cs:1007-1086); XML `a6-1101-results.xml` Passed | Fixture chưa có 1101 (A6:1018); nhiệm vụ 11011/11012 đạt (A6:1040); đọc cờ cleared từ `PlayerContext_0.sav` bằng SerializeHelper (A6:1057); slot 1102 trong danh sách của Elara không khoá, slot 1101 có "completed" (A6:1082-1084). So với trạng thái của chính game. | Hồi HP khi dưới 35% (log: 2 lần `ReqSetCharacterHp`), `ReqCheatLevelUp` 1 lần để đủ level 2. Không có SetClearedCampaign, không bất tử. | **Mắt xích tốt nhất.** Nhưng trong lần chạy cả lớp thì đỏ (Lỗ hổng 1). |
| 1102 (boss Hachishakusama) sang 1103 | `BossCampaign1102` | `IsClearedCampaign(1102)` đúng (A6:209); `CheckStateConditions` của 1103 (`UserLevel:3\|CampaignCleared:1102`) bằng trạng thái game (A6:217-227). | **`SetClearedCampaign(1101)`** (A6:272), `ReqCheatLevelUp`, `ReqCheatApplyCharacterOptions Invincibility` sau khi chụp ảnh (A6:702), hồi HP/stress. | Đánh dấu clear = cheat bỏ nội dung (1101 không chơi trong test này). |
| 1103 (Donovan) sang 1104 | `BossCampaign1103` | như trên, 1104 = `UserLevel:4\|CampaignCleared:1103` | `SetClearedCampaign(1101,1102)`, bất tử từ đầu trận, **`ReqCheatApplyMonsterHp Hp=1`** lên Donovan sau 120 s (log a6-full8: `phase 1 still up after 120s: 300004 hp=12977 state=Idle`, dòng `cheat ReqCheatApplyMonsterHp Hp=1` ở t=808.9) | Boss không bị đánh thắng thật (Lỗ hổng 3). |
| 1104 sang 1105 | `StoryCampaign1104` | cleared + unlock 1105 theo game | `SetClearedCampaign(1101..1103)`, level-up | Cheat clear. Không boss. |
| 1105 (Eldris, 2 pha) sang 1201 | `BossCampaign1105` | cleared + unlock 1201 | `SetClearedCampaign(1101..1104)`, bất tử, **HP quái = 1 cả hai pha** (a6-full8 t=1203.5 và t=1327.4) | Boss không bị đánh thắng thật. |
| 1201 sang 1202 | `StoryCampaign1201` | cleared + unlock 1202 | `SetClearedCampaign(1101..1105)`, level-up, hồi HP/stress | Cheat clear. Lần mới nhất đỏ (Lỗ hổng 1). |
| 1202 (story cuối) | `StoryCampaign1202` | cleared (A6:209). Không có "next story" (`StoryOrder` hết). | `SetClearedCampaign(1101..1201)`, level-up | Cheat clear. Lần mới nhất đỏ. |

Cheat mài (chấp nhận, liệt kê): `ReqSetCharacterHp` (hồi HP, hàng trăm lần trong A6_BossFlow.log: 129), `ReqSetCharacterStress` (11), `ReqCheatLevelUp` (6 trong lần chạy 6 case). Cheat bỏ nội dung (lỗ hổng): `SetClearedCampaign` (21 trong A6_BossFlow.log), `ReqCheatApplyMonsterHp` (3), `ReqCheatApplyCharacterOptions Invincibility` (3).

Mỗi mắt xích assert "mở khoá của story kế" bằng `PlayerManager.CheckStateConditions` trên `UnlockConditions` đọc từ bảng của game (A6:215-227), không phải giá trị test gán. Phần level của điều kiện đó do `ReqCheatLevelUp` đáp ứng (A6:222). Nhưng vì mỗi test lại khởi động từ fixture rồi `SetClearedCampaign` mọi story trước, không có lần chạy nào đi liền 1101 đến 1202 trên cùng một hồ sơ do chính game ghi. Trong một phiên cả lớp, hồ sơ có tiếp tục giữa các test (log a6-full8: `SetClearedCampaign(1101) -> False` tức đã clear từ test trước), nhưng thứ tự khai báo là 1102, 1103, 1105, 1101, 1104, 1201, 1202 nên nó không phải chuỗi tự nhiên, và chính tính chia sẻ đó làm 1101 đỏ.

## Bảng 2. Phủ id campaign (bảng game: `Assets/StreamingAssets/TableEncrypted/Campaign.bytes`, giải XOR 0xCC, 46 dòng dữ liệu)

| Nhóm id | Số | Phủ bởi | Kết quả | Ghi chú |
|---|---|---|---|---|
| 1100 | 1 | A4 | xanh 2026-10-25 | assert yếu (xem Lỗ hổng 5) |
| 1101 | 1 | A5.DiveCampaign1101 (xanh); A6.StoryCampaign1101 (xanh riêng lẻ, **đỏ trong lần chạy cả lớp**) | | |
| 1102, 1103, 1105 | 3 | A6.BossCampaign* | xanh trong `A6_BossFlow.xml` (6 case) và a6-full8 | boss bị ép HP/bất tử |
| 1104, 1201, 1202 | 3 | A6.StoryCampaign* + A6b | `A6_BossFlow.xml` xanh; a6-full8 **1201, 1202 đỏ**; A6b run5/run6 xanh (cũ) | |
| 101-108, 111-118, 201-211 | 27 | A6b_CampaignSweep.Campaign(id) | a6b-run2..6 xanh (04:35Z đến 08:17Z); chỉ 101, 108, 204, 211 chạy lại sau khi có GameLog (`a6b-review1.xml`, 13:55Z) | Lỗ hổng 6 |
| 301-310, 999 | 11 | không | `docs/review/a6/campaigns.tsv` ghi "not reachable" | Chấp nhận được: `UnlockConditions UserLevel:11`, còn `TConst.LevelMax = 7` (Const.bytes). 999 là "Test Map". |

Đối chiếu: 46 id trong bảng, 46 dòng trong `campaigns.tsv`, không thiếu không thừa. `docs/MISSING.md` không liệt kê campaign nào (chỉ liệt kê thư viện/shader/key thiếu), nên 301-310/999 được giải thích chỉ trong tsv. Cột "covered" của tsv trích `a6-full7` cho 1102..1202; đó là lần chạy cũ, không phải a6-full8.

## Bảng 3. File kết quả XML

| File | result | Số case | Đạt | Skip/Inconclusive | Ghi chú |
|---|---|---|---|---|---|
| A1_TitleFlow.xml | Passed | 1 | 1 | 0 | 2026-10-25 14:18Z |
| A3_NpcFlow.xml | Passed | 1 | 1 | 0 | |
| A4_TutorialFlow.xml | Passed | 1 | 1 | 0 | |
| A5_DiveFlow.xml | Passed | 1 | 1 | 0 | |
| A7_LoungeProgressFlow.xml | Passed | 1 | 1 | 0 | |
| A8_AudioFlow.xml | Passed | 1 | 1 | 0 | |
| A8b_TutorialAudio.xml | Passed | 1 | 1 | 0 | |
| A6_BossFlow.xml (15:06Z) | Passed | 6 | 6 | 0 | **Không có 1101** (chưa tồn tại lúc đó). |
| a6-1101-results.xml (18:13Z 10-25 = 01:13 +07 10-26) | Passed | 1 | 1 | 0 | |
| **a6-full8-results.xml** (18:21Z đến 19:02Z) | **Failed(Child)** | 7 | **4** | 0 | 1101, 1201, 1202 đỏ |

Không test nào bị bỏ qua. Lần a6-full8 hoàn tất trong lúc tôi review (không phải chờ 90 phút).

## Bảng 4. Flow.GameLog

| Test | Bọc bằng GameLog.Run | Hiệu lực |
|---|---|---|
| A1 (A1:139), A4 (A4:25) | Có | **Vô hiệu** khi test đạt: cả hai kết thúc bằng `Assert.Pass` (A1:226, A4:226). `SuccessException` bay qua `Run`, dòng `AssertClean` (Flow.cs, sau khối try/finally) không bao giờ chạy. |
| A3 (A3:82), A5 (A5:71), A7 (A7:71), A8 (A8:84), A8b (A8b:65) | Có | Hiệu lực |
| A6 (A6:152, 158) | Có | Hiệu lực (log có dòng `known campaign 1105: ...`, chứng tỏ AssertClean chạy) |
| A6b (A6b:104-122) | Dùng Start/Stop/AssertClean trực tiếp | Hiệu lực |

Bảng `KnownShipped` (Flow.cs, struct Known) chỉ có **một hàng**: "Collection was modified" với stack `UnitSkillState.OnUpdate`, bẫy 54. `docs/DECODING.md:399-409` chứng minh từ decomp (`@0x1806CB910`, `Clear` inline có `_version++`, foreach trong `OnUpdate`) rằng game gốc cũng ném. Đạt yêu cầu "chỉ vấn đề đã chứng minh trong decomp".
Hạn chế nhỏ: `Capture` bỏ qua mọi thông điệp chứa chuỗi `VD_` (Flow.cs, đầu hàm Capture); lỗi game có chữ `VD_` sẽ lọt. Cảnh báo chỉ tính khi khớp `LoadFailureHints`.

## Các lỗ hổng

### 1. Lần chạy đầy đủ A6 không đạt 7/7 (a6-full8: 4 đạt, 3 đỏ)
Bằng chứng (`captures/review/a6-full8-results.xml`):
- `StoryCampaign1101` đỏ sau 8,7 s: "the fixture save already has 1101 cleared ... Expected: False But was: True" (A6_BossFlow.cs:1018). Nguyên nhân: test 1102 chạy trước trong cùng phiên đã `SetClearedCampaign(1101)` rồi lưu, còn `Flow.SeedSaves` chỉ chạy một lần ở `SubsystemRegistration` (A6:~66), nên hồ sơ bị dùng chung. Test 1101 xanh khi chạy riêng (a6-1101-results.xml) nhưng đỏ trong lớp: nó phụ thuộc thứ tự.
- `StoryCampaign1201` đỏ: "diver Dead after equipment 3" (A6_BossFlow.cs:843). Test chạy mà không bất tử, diver chết. Lần `A6_BossFlow.xml` (15:06Z) xanh cùng test, vậy đây là test không ổn định.
- `StoryCampaign1202` đỏ là hệ quả: "campaign 1202 did not start ... Expected 1202 But was 1201" (A6:179). 1201 chưa đóng nên slot campaign vẫn là 1201.
Đề xuất: (a) `[SetUp]`/đầu mỗi case nạp lại fixture (copy lại `saves-post-tutorial` và vào lại title Continue) hoặc đặt 1101 đầu lớp và bỏ `SetClearedCampaign(1101)` ở các case sau để chuỗi dùng hồ sơ do chính game ghi; (b) làm 1201 ổn định (hồi HP theo Guardian đang có, thêm đường tránh quân lính, hoặc kiểm tra vì sao chết ở "equipment 3"); (c) đóng campaign dở ở cuối mỗi case (như A6b `CloseOpenCampaign`) để lỗi một case không kéo case sau; (d) chạy lại cả lớp, yêu cầu 7/7.

### 2. Chuỗi 1102 đến 1202 nối bằng cheat đánh dấu clear, không có lần chạy liền
`CheatPastEarlierCampaigns` (A6_BossFlow.cs:267-280) gọi `PlayerManager.SetClearedCampaign` cho mọi story trước, rồi `ReqCheatLevelUp`. Log: A6_BossFlow.log có 21 dòng `cheat SetClearedCampaign`. Mỗi test khởi từ fixture (1100 clear), nên không test nào chứng minh 1101 đến 1102 đến 1103 ... đi liền trên một hồ sơ. Từng mắt xích có assert mở khoá bằng trạng thái game (tốt), nhưng đầu vào của mắt xích sau là giá trị test gán.
Đề xuất: một test chuỗi duy nhất (hoặc `[Order]` + giữ hồ sơ): 1101 (Case1101, không cheat clear) rồi 1102 ... 1202, mỗi bước bắt đầu từ hồ sơ test trước ghi ra; chỉ cho phép hồi HP và level-up. Có thể tách thành hai phiên nếu quá dài (tổng thời gian thực khoảng 45 phút).

### 3. Hai boss không bị đánh thắng thật: ép HP về 1 và bất tử
`ReqCheatApplyMonsterHp Hp=1` (A6:708) khi pha còn sống sau `PhaseBudget` 120 s. Log a6-full8: 1103 Donovan (hp=12977, `state=Idle`, d=0.0 ở t=808.9), 1105 Eldris pha 1 (hp=13837, t=1203.5) và pha 2 (hp=20146, t=1327.4). Cũng có trong A6_BossFlow.log (3 lần). Cùng lúc diver bất tử (`ReqCheatApplyCharacterOptions`, A6:702; 1103 và 1105 bật từ đầu trận). 1102 cũng bất tử sau khi chụp ảnh. Boss "Idle" ở d=0 với HP 12977 nghĩa là diver cấp 1 không gây đủ sát thương trong 120 s, và test thắng bằng cheat. Đây là "đặt boss gần chết không qua giao tranh".
Đề xuất: để diver có sức chiến đấu như người chơi (grind cấp/trang bị bằng cheat cấp phát đồ là chấp nhận được) và bỏ `ApplyMonsterHp` và `Invincibility`; hoặc ghi rõ trong MISSING.md rằng ba pha này chưa chứng minh được thắng bằng đánh thật.

### 4. GameLog vô hiệu ở A1 và A4 (Assert.Pass)
`Assert.Pass` ở A1_TitleFlow.cs:226 và A4_TutorialFlow.cs:226 ném `SuccessException` ngay trong `BootToTitleBody`/`NewGameToTutorialBody`; `Flow.GameLog.Run` (Flow.cs) chỉ gọi `AssertClean` sau khi body kết thúc bình thường. Bằng chứng thực nghiệm: log A1 (`~/vd-rip/logs/A1_TitleFlow.log:4441`, `SteamManager:Awake (SteamManager.cs:79)` gọi `Debug.LogError`, "[Steamworks.NET] SteamAPI_Init() failed") xảy ra **trong** thân test, sau `SceneManager.LoadScene(0)` và trước dòng `VD_A1 t=4s` (4468), đúng lúc recorder đang bật, mà A1 vẫn đạt. Nếu `AssertClean` chạy thì test đã đỏ (đó là `LogType.Error`, không nằm trong `KnownShipped`). (Lỗi Steam này là do môi trường không có Steam; điều cần sửa là test không phát hiện, không phải lỗi này.) Các lỗi game thật trong A1/A4 vì vậy không được bắt.
Đề xuất: bỏ `Assert.Pass` (để kết thúc tự nhiên) hoặc gọi `Flow.GameLog.AssertClean` ngay trước `Assert.Pass`; sau đó chạy lại A1/A4 và xử lý các lỗi mới lộ ra (A1 dùng nền tảng Steam: dùng `Flow.UseLocalPlatform` như các test khác).

### 5. A4 và fixture: mắt xích tutorial sang lounge không bị buộc
(a) A4 không assert đã về lounge và 1100 đã clear: khối ở A4:~195 chỉ chạy `if (ActiveScene == "LoungeScene")`, rồi `Assert.Pass`. Nếu tutorial không dẫn về lounge, test vẫn xanh miễn `s_missing` rỗng. (b) `tools/fixtures/saves-post-tutorial` được chép một lần (2026-10-24) từ SavesDir; không có script hay hash chứng minh nó bằng đầu ra A4 hiện tại, và không test nào đi A4 rồi 1101 liền.
Đề xuất: thêm assert `Flow.ActiveScene == "LoungeScene"` và `IsClearedCampaign(1100)` (đọc lại từ `.sav`) cuối A4; thêm bước trong script chạy A4 tự chép `SavesDir` vào fixture (hoặc so sánh cờ), để fixture luôn là sản phẩm của lần chạy A4 mới nhất.

### 6. Sweep A6b cũ so với code hiện tại
23 trong 27 id của sweep (102-107, 111-118, 201-203, 205-210, 1104, 1201, 1202...) chỉ có lần xanh trong a6b-run2..6 (04:35Z đến 08:17Z 10-25). Sau đó có 4 commit đổi code game: `8cc7298e` (CharacterController, InGameScene: level-up), `ba2dd365` (AppStartUp), `f0e2394e` (ValidationExtensions), `3fdf3d89` (Stat.TryParse). Lần xanh sau GameLog chỉ có `a6b-review1.xml` với 101, 108, 204, 211 (4 case). `a6b-run1.xml` đỏ (2/3), `a6b-run6` chạy lại 103, 106, 114, 115, 117 vì run2/3 đã xanh. Trong sweep, 1202 thoát bằng ReqForceEscape ("objectives not played", tsv), chấp nhận vì A6 chơi 1202 thật.
Đề xuất: chạy lại cả A6b (khoảng 35 phút/27 case chia 3-4 lượt) trên code hiện tại và thay các XML cũ.

### 7. Nội dung chạm tới được mà không test nào chạm
Lấy từ bảng `LoungeQuest` (78 dòng), `Npc`/`NpcFunction` (giải XOR từ TableEncrypted):
- Lounge quest: chỉ 80200 được chơi (A7, A3, A6) và 82100 được nhắc trong comment A3. Không test: các quest Main level 80002-80007 (Elara/Owner), 80100 (`CampaignCleared:1101`, NPC 700005), 81101-81105 (`81102..81105` đúng nhịp 1101..1104), các Sub 82101-82113, 82190, 82201-82206, 82210, 82290, 83101-83115/83201-83212 (Ryujin), 84101-84502 (level 5/6). 82207-82209 cần `UserLevel:9` nên không tới được (LevelMax 7).
- NPC có function mà grep không thấy trong A1, A3, A4, A5, A6, A7: 700003 Evelyn (ParadoxPurify, ParadoxLock), 700004 sofa Shoggoth (Rest), 700012 cổng multiplayer, 700015 Pim (ArtifactShop), 700016 Ryujin (Shop), 700214/700215/700230/700231 Woung (Shop, Scrap), 700009/700010/700101/700102 (Dialog), 700150/700152 ATM (CoinExchange), customer 600002-600013, và function Tycoon (Elara, `UserLevel:3`), Interior (Elara, `UserLevel:4`), TycoonSalesSlot 700106-108. A8_AudioFlow chạm 700005 (LevelUp), 700011 và 700151 để nghe âm thanh, không kiểm hành vi. A3 chạm: 700000, 700001, 700002, 700006, 700008, 700013, 700014 và customer 600001.
Cần xác định NPC nào thật sự có trong lounge của demo trước khi quyết (bảng Npc có 128 dòng).
Đề xuất: một test A3b chạy qua mọi NPC có function trong lounge ở level 7 sau 1202, mở và đóng từng function; một test duyệt toàn bộ lounge quest đang mở và hoàn thành quest khả dụng theo thứ tự `UnlockConditions`.

### 8. Sau story cuối (1202): không có gì được kiểm
A6 dừng ở "1202 cleared" và hộp thoại Elara (log a6-full8/A6_BossFlow.log: `dialog Elara 1202 complete: 'Being manipulated by someone...'`). Không test nào kiểm cái người chơi thấy sau đó: lounge quest 82205 (`CampaignCleared:1202`, NPC 700001), danh sách campaign không còn story nào mở, màn kết/thông báo hết demo (nút Wishlist ở title, `StartScene.cs:162-182`, cũng không được bấm trong A1). Không tìm thấy màn "demo end" riêng trong `Assets/Scripts` (grep `ThanksForPlaying`/`DemoEnd`: không có), nên có thể game gốc không có, nhưng chưa có test xác nhận điều đó. Nút Options/Quit trên title cũng không có test hành vi.
Đề xuất: thêm bước sau 1202: về lounge, mở danh sách campaign (xác nhận không story nào mở, chỉ Normal), nhận quest 82205, thoát về title, bấm từng nút title (Options, Wishlist, Quit) và gom log.

## Bảng 5. Tóm tắt kết luận theo 5 câu hỏi

| # | Câu hỏi | Kết luận |
|---|---|---|
| 1 | Chuỗi title đến 1202 liền, test chứng minh, không cheat bỏ nội dung | Không: lỗ hổng 1, 2, 3, 5 |
| 2 | Mọi id campaign được phủ hoặc có lý do | Có về mặt tài liệu (46/46 trong tsv; 301-310 và 999 vì LevelMax 7). Độ tươi của 23 id sweep: lỗ hổng 6 |
| 3 | XML Passed, đủ case, không skip | Các XML A1..A8b, A6 (6 case), a6-1101 đều Passed, không skip. Nhưng a6-full8 Failed 4/7 (lỗ hổng 1) |
| 4 | GameLog bọc thân test, KnownShipped chỉ có vấn đề chứng minh bằng decomp | KnownShipped đúng (1 hàng, bẫy 54). Bọc có, nhưng vô hiệu ở A1, A4 (lỗ hổng 4) |
| 5 | Thứ người chơi tới được mà test không chạm | Lỗ hổng 7 và 8 |
