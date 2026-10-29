# Đóng 8 lỗ hổng của review chuỗi title đến 1202

Ngày 2026-10-29. Báo cáo này trả lời `REPORT.md` (cùng thư mục, `d646d5f4`, 2026-10-26).

Người làm: phiên MAIN. Người verify: phiên review độc lập (chỉ đọc repo, không chạy Unity, không commit). Mọi lần "ĐÓNG" dưới đây là do reviewer tự đọc XML và log rồi chấp nhận, kênh `~/handoff/review-chat.md`.

**KẾT LUẬN: 8/8 lỗ hổng ĐÓNG.** Mỗi gap được reviewer độc lập đóng sau khi tự đọc XML và log. Phán quyết "review sạch" cho toàn bộ: xem dòng cuối của mục "Phán quyết".

## Bảng 1. Tám lỗ hổng

| # | Lỗ hổng (REPORT.md) | Lần chạy chứng minh | Kết quả | Reviewer |
|---|---|---|---|---|
| 1 | a6-full8 3 đỏ | a6-full12 (HEAD c2f546a4) | 8 ca per-boss xanh: Story 1101/1104/1201/1202, Boss 1102/1103/1105, CatchingCatchesNested. Mỗi ca `passed on attempt 1, deaths=0` | ĐÓNG 04:05 10-29 |
| 2 | chuỗi 1102 đến 1202 nối bằng SetClearedCampaign | a4-gl4 (A4 xuất lại a4-saves) + a6-chain14 (game code 53544c4c, test e9f1e996) | 8/8 StoryChain xanh trên MỘT hồ sơ do game ghi, từ bản xuất của A4: 1101 → 1102 → 1103 → 1104 → 1105 → 1201 → 1202 → AfterLast. `SetClearedCampaign` = 0. Mỗi link attempt 1, deaths=0, trừ 1201: chết ở boss 300009 rồi qua ở attempt 2 | ĐÓNG 07:44 10-29 |
| 3 | boss ép HP=1 và bất tử từ đầu | a6-chain14 + a6-full12 | `ReqCheatApplyMonsterHp` = 0. Bất tử chỉ bật sau dòng `proof before invincibility` với ít nhất 1 đòn của boss (300014, 300004, 300005) | ĐÓNG 07:44 10-29 |
| 4 | GameLog vô hiệu ở A1, A4 (Assert.Pass) | a1-gl1 (`f50ba8c3`, bẫy 56, 57) | xanh, GameLog chạy | ĐÓNG |
| 5 | A4 và fixture không bị buộc | a4-gl1 (`b26432ee`, `af02d0ed`); A4 xuất `captures/review/a4-saves` kèm PROVENANCE (`7d7633f3`) | A4 assert LoungeScene + 1100 cleared; chuỗi đọc bản xuất này, có guard "mới hơn mọi file code" | ĐÓNG |
| 6 | sweep A6b cũ | a6b-full2 (adc32720) | 30/30 | ĐÓNG 00:21 10-29 |
| 7 | nội dung sảnh chưa test | A9a (quest phụ) và A9b (hệ thống sảnh), bảng 2 | xanh, trừ một test [Ignore] có lý do | ĐÓNG 06:22 10-29 |
| 8 | sau 1202 không kiểm gì | a6-chain14 `StoryChain_AfterLast` | xanh (39 s), chạy trên hồ sơ sau 1202 của chính chuỗi | ĐÓNG 07:44 10-29 |

## Bảng 2. Gap 7: test sảnh và lần xanh mới nhất

| Test | Lần xanh | Ghi chú |
|---|---|---|
| A9a EdmanQuests | a9a-g1a3 | 2 rift, claim 10001 |
| A9a EvelynQuests, Felix82102, Felix82105 | a9a-g1b2 | |
| A9a Felix82113, Lucas82100, Lucas82205, LucasLandmarks, MuseumProbe | a9a-g2 | |
| A9a Pim82111, Ryujin82206, Scout | a9a-g3 | |
| A9a Lucas82205, Ryujin82203 | a9a-g4 | |
| A9a Pim82107, Pim82201 | a9a-g5 | |
| A9a Pim82204 | a9a-g7 (c2f546a4) | chọn ở trang 2 của danh sách lựa chọn |
| A9a Evelyn82108And82210 | — | [Ignore]: diver test không giết nổi patrol boss Dark Young (43000 HP) / Hatman (48500 HP), chết 4/4 dive ở a9a-run20 |
| A9b InteriorAtLevel4, NpcFunctions, SalesAndWoungChapter1, TycoonAtLevel3, WoungChapter2 | a9b-run10 | |

## Cheat đã dùng, và giới hạn của bằng chứng

Luật: chỉ dùng cheat của chính game, và chỉ cho phần mài: level, tiền, item, hồi HP/stress, và tăng counter chỉ sau khi đã làm thật một lần. Không dùng cheat nhận quest, hoàn thành quest, hay clear campaign trong chuỗi.

### Phạm vi của từng loại bằng chứng
1. **Ca per-boss (gap 1) và sweep A6b dùng `SetClearedCampaign` làm điều kiện tiên quyết.** Chúng chứng minh từng map tải được, đánh được, thoát được. Chúng KHÔNG chứng minh tiến trình cốt truyện. Phần đó là việc của `StoryChain` (gap 2), nơi `SetClearedCampaign` = 0.
2. Sweep A6b chứng minh "mỗi map tải được và rời được", không chứng minh tiến trình. 1202 được rời bằng `ReqForceEscape` (đường bị chặn). Không ca nào chụp được patrol boss.
3. **Sweep A6b (a6b-full2, adc32720) chạy TRƯỚC game fix `53544c4c`** (hộp khoá). Fix đó chỉ đổi hai handler popup chìa khoá trong `InGameScene.cs`. Reviewer chấp nhận không chạy lại.
4. Các test A9a và A9b của gap 7 xanh trên các commit trước `53544c4c` (bảng 2), trừ Pim82204 (a9a-g7, `c2f546a4`). Sau các lượt đó, code test chỉ đổi ở phần lật trang (mục 9) và phần kiểm quest khác của cùng NPC.

### Cheat trong chuỗi và ca per-boss
5. Bất tử (`ReqCheatApplyCharacterOptions Invincibility`) chỉ bật sau khi có bằng chứng boss đã đánh trúng diver (dòng `proof before invincibility`). Ở đợt quái 1201, bất tử chỉ bật sau một đòn mất >= 25% HP. Ở a6-full12 không có đòn như thế nên không bật; ở a6-chain14 attempt 2 có một đòn như thế (hp 461/630) nên có bật, và tắt lại khi đợt quái xong.
6. Ca per-boss lặn lại khi chết, giống chuỗi: tối đa 3 lần, chết hai lần cùng một chỗ là đỏ. Ở a6-full12 không ca nào phải lặn lại. Ở a6-chain14, link 1201 chết một lần ở boss 300009 rồi qua ở attempt 2. Ngoài lần đó, đường lặn lại chỉ được chứng minh bằng a6-catch1.
7. Điểm yếu của test, thấy ở lần chết 1201 (không chặn):
   - (a) Ở t=3174-3186, diver giữ F 12 s ở booth trong khi `focused=False` và đang bị quái 210020/200022 đánh. Refill HP không theo kịp. Cách sửa: chưa focus thì không giữ F, mà đánh hoặc lùi lại trước.
   - (b) Nhãn DeathPlace được lấy sau khi diver đã về lounge (`LoungeScene cell 0,3 boss 300009`), nên ghi sai chỗ chết. Quy tắc "chết hai lần cùng chỗ" vì thế có thể đỏ oan. Lỗi này fail-closed, không gây pass giả.

### Lỗi đã sửa trong lượt này
8. **Lỗi game thật, sửa từ pseudo-C (`53544c4c`):** khi đóng popup chìa khoá mà không có chìa, bản port chỉ gửi `ReqInputCancelInteract`. Bản gốc gửi cancel rồi LUÔN gửi `ReqFinishInteractionWaiting`. Thiếu lệnh thứ hai thì `CharacterModel.IsWaitingInteractionFinish` không được hạ, và lần tương tác sau kẹt ở state Interact (a6-full11, hộp 11306). Đã đối chiếu bằng pseudo-C Ghidra, vì `il2cpp_method.py` không chạy được (thiếu `pefile`). Bằng chứng runtime, test `A6c_LockedBox` (ngoài 8 lỗ hổng):
   - a6c-1 xanh (hộp 11306) nhưng KHÔNG phân biệt được code cũ với code đã sửa. Lúc popup mở, state đã là Idle, nên assert "state khác Interact" đúng một cách hiển nhiên. Lần này không được tính là bằng chứng.
   - `79215a75`, `e9f1e996` (chỉ đổi test) cho test kiểm thẳng cái chốt: cờ phải lên khi popup mở và xuống trong 3 s sau Esc. Sau đó diver phải đi khỏi hộp, và lần F thứ hai lên cùng hộp phải mở lại popup và lại nhả. Cờ không lên thì kết quả là Inconclusive.
   - a6c-2 (code có `53544c4c`) Passed: hộp 11309 (keys=[9015]). Cả 2 vòng đều `key popup open, state=Idle waiting=True`, cờ hạ ngay sau Esc, diver đi được 4.9/5.5 m, có dòng `let go ... twice, waiting=False`.
   - a6c-mut1 (xoá tạm dòng `ReqFinishInteractionWaiting` trong `OnKeyInteractionRewardBoxAsync`) Failed đúng chỗ: `IsWaitingInteractionFinish is still set 3 s after the key popup of box 10300 closed in round 1 (state=Idle)`. Sau lượt đó, file đã được trả lại cùng mtime, `git status` sạch.
   - Kết luận: fix được chứng minh ở runtime bằng cặp a6c-2 xanh + mutant đỏ. Nhánh cửa khoá (`OnKeyInteractionEntranceAsync`) chỉ được soát tĩnh.
9. Lỗi lật trang danh sách lựa chọn trong test (`464280c5`): danh sách hiện 3 dòng mỗi trang, nên idx 4 bị kẹp về 2 và test nhận nhầm 82107. Các test xanh trước đó đều chọn idx < 3 (reviewer grep `-> [3-9]` ở g1..g5: 0 kết quả), nên không bị ảnh hưởng. Từ `c2f546a4`, nếu nhận một quest làm đổi trạng thái quest khác của cùng NPC thì test đỏ.
10. Lỗi của chính MAIN khi xếp lượt chạy: gộp A4 với A6 vào cùng một lượt Unity (a6-chain13). `PrepareSaves` của A6 seed save trước, nên A4 thấy màn "Continue" và đỏ. Đã tách thành a4-gl4 rồi a6-chain14.

### Giới hạn và follow-up
11. Fixture drift: các test sảnh khởi động từ `tools/fixtures/saves-post-tutorial`. Chuỗi log những chỗ fixture đó khác bản xuất của A4 (dòng `fixture drift`), nhưng không assert.
12. Evelyn82108And82210 bị [Ignore]. Lý do: diver test không giết nổi patrol boss Dark Young/Hatman. Đây là giới hạn của test, không phải lỗi game.
13. Woung (A9b) được phủ nhờ may: shop Woung chỉ xuất hiện theo lượt.
14. Pim82204: bảng campaign được rút lại sau mỗi lần campaign clear/fail. Đường thử lại khi bảng không có Town (`d41161aa`) chưa chạy lần nào ở runtime, vì g6 và g7 đều có Town ngay lần đầu. Logic chỉ được soát tĩnh. LevelTo/Pim82107 có rủi ro tương tự (trọng số bảng).
15. Panel rift đóng sớm, hoặc click trúng lúc diver đang choáng (stress 100): thấy ở a9a-g1a3, lần mở thứ hai cứu được. Không chặn.
16. Hiệu năng (không chặn): ở a9a-g7, dive 2 của rift 1 mất khoảng 1 giờ, vì diver kẹt ở (169.70, 81.60) trên đường tới circle 1035 và side step không gỡ được.
17. Lượt Tour80200 chạy trước khi có chain save là thừa (nit).
18. Các lượt `safe-unity-*` chạy ngoài `heavy.sh`.
19. F17 cho A5/A9b nằm ngoài 8 lỗ hổng.
20. GameLog có lỗi `Collection was modified` ở `UnitSkillState.OnUpdate`: 27 lần ở a6-full12 (boss 300005 của 1105), 5 lần ở a9a-g7 (các dive rift). Đây là bẫy 54 (`docs/DECODING.md`). Bản gốc cũng có cùng vòng foreach (decomp `OnUpdate`), nên đây không phải lỗi của bản port.

## Phán quyết

- Review độc lập (09:05 10-29): sạch, 8/8 lỗ hổng ĐÓNG.
- Reviewer (phiên `vd-review`, chỉ đọc) đọc bản đã commit `400ce622` và working tree sạch, rồi ghi phán quyết này ở `~/handoff/review-chat.md` lúc 09:05 10-29.

## Tra cứu

- XML: `captures/review/<tên>-results.xml`. Log: `~/vd-rip/logs/<tên>.log` (máy dựng, không có trên repo).
- Kênh review: `~/handoff/review-chat.md`.
