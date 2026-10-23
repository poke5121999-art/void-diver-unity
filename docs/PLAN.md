# Kế hoạch dựng lại VOID DIVER trong Unity

Nguồn: bản Steam demo `~/vd-src/VOID DIVER Escape from the Abyss Demo/` (Unity 6000.3.5f2, IL2CPP, URP).
Đích: project Unity 6000.3.18f1 ở repo này, chơi trọn bản demo từ đầu tới cuối, mọi asset lấy từ bản gốc.

## Định nghĩa xong

Toàn bộ demo chơi được từ màn tiêu đề tới hết nội dung, bằng asset gốc, và một vòng review mới cho mọi khu vực
không còn sai khác nào, rồi reviewer độc lập (model khác) duyệt lần cuối từng khu vực.

## Vòng review của một khu vực

1. Chơi khu vực trong Unity bằng kịch bản input giả lập (`Assets/_VD/Editor` + play mode), chụp ảnh ở các mốc cố định.
2. Đặt cạnh ảnh thật: `~/vd-src/vd-ref/steamshots`, mô tả đo được trong `~/survivor-web-hub/games/voiddiver/docs`,
   và ảnh chụp bản gốc chạy dưới Proton khi có (`~/vd-rip/game-run.sh`; hiện game tự tắt ở StartScene khi không có Steam).
3. Liệt kê mọi sai khác vào `docs/review/<khu>.md`, sửa, chụp lại, lặp tới khi một vòng mới không còn gì.
4. Reviewer độc lập (model khác) duyệt khu vực đó. Sạch thì push `main` và cập nhật STATUS.

## Khu vực

| # | Khu vực | Nội dung |
|---|---|---|
| A1 | Splash, tiêu đề | SplashScene, StartScene (video, menu, cài đặt, chọn save) |
| A2 | Sảnh Balusha | sector 9001–9003, di chuyển, camera, NPC, hội thoại Lua, NpcTalk |
| A3 | Chức năng NPC | chọn campaign, nhân vật, bán, chế, talent, lên cấp, sửa đồ, kho, giao dịch artifact |
| A4 | Tutorial | campaign 1100, script Lua tutorial |
| A5 | Lặn | map sector ngẫu nhiên, quái + AI, skill, buff, sát thương, đèn, stress, corruption, loot, bẫy, cửa, thoát |
| A6 | Boss + story | 1101, 1102 (Slender), các boss demo |
| A7 | Kết quả, tiến trình | màn kết quả, EXP, quest, account record, lưu game |
| A8 | Âm thanh | BGM, SFX qua MasterAudio |

## Dòng việc nền (làm song song, không chung file)

| Dòng | Việc | Kiểm |
|---|---|---|
| W1 Asset project | xong (M1) | census, check_refs |
| W2 Package thật | thay stub thư viện mã nguồn mở (UniTask, VContainer, UniRx, MoonSharp, DOTween, SoftMask, Graphy) | 0 lỗi biên dịch, census không đổi |
| W3 Shader | dựng lại ~100 shader riêng từ DXBC gốc: tách blob + bố cục constant buffer (UnityPy), dịch ngược DXBC → HLSL, bọc ShaderLab với property/state gốc | ảnh chụp material so với ảnh gốc |
| W4 Code | dịch thân method từ pseudo-C Ghidra (`~/unity-decode-kit/out/vd/code/decomp`) sang C#, giữ nguyên tên/chữ ký; cả thư viện trả phí không có nguồn (MasterAudio, Febucci, DOTweenPro, InputIcons, ParticleImage, EnhancedScroller) | `tools/harness/test.sh` biên dịch + test hành vi; play mode |
| W5 Dữ liệu | bảng (198 lớp), Lua (MoonSharp, script gốc), address catalog | so với `~/vd-src/vd-ref/json` |

## Luật

- Không tự vẽ, không sinh art. Thiếu asset thì ghi vào `docs/MISSING.md`.
- Mọi bước biến đổi export là script trong `tools/`, chạy lại được. Asset của game không vào git.
- Một Unity batchmode một lúc, luôn có `timeout`.
- Nhật ký quyết định: `.audit/decisions.tsv`.
- Hạn mức ~92% thì bàn giao theo `~/.claude/brain/knowledge/handoff-at-92pct.md`.

## W4b: thư viện đóng (thêm 2026-10-23)
AssetRipper Free để stub cả các thư viện trả phí mà game dùng. Đo bằng số thân hàm rỗng: MasterAudio 634/850,
Febucci TextAnimator 212/315, InputIcons 406/805, ChocDino UIFX 344/532, ACTk 466/1.693, EnhancedScroller 47/133,
ParticleImage 64/124, SRF và Graphy (debug).
- Có trong dump decompile: dịch theo batch như W4 (`~/vd-rip/w4/lib-*.txt`).
- Mã nguồn mở (Odin Serializer, Steamworks.NET): thay bằng source thật như W2.
- ACTk.Runtime: chưa có trong dump, đã nhờ decode-kit thêm.
