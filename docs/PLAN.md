# Kế hoạch dựng lại VOID DIVER trong Unity

Nguồn: bản Steam demo `~/vd-src/VOID DIVER Escape from the Abyss Demo/` (Unity 6000.3.5f2, IL2CPP, URP).
Đích: project Unity 6000.3.18f1 ở repo này, chơi được, mọi asset lấy từ bản gốc.

## Định nghĩa xong (kiểm được)

Mỗi mốc có một phép kiểm chạy lại được. Chưa qua phép kiểm thì mốc chưa xong.

| Mốc | Phép kiểm |
|---|---|
| M1 Asset project | Unity batchmode mở repo, 0 lỗi biên dịch; `tools/unity/census` báo 0 missing script trên 3 scene và mọi prefab |
| M2 Hình đúng | Ảnh chụp từ Unity của sảnh, một sector, một nhân vật Spine đặt cạnh `~/vd-src/vd-ref/steamshots`; không còn material hồng/xám dummy |
| M3 Sảnh chơi được | Play mode: Splash → Lounge, nhân vật đi bằng phím, camera gốc, BGM; kịch bản input giả lập + ảnh chụp |
| M4 Lặn chơi được | StageScene: map sector, quái, skill, sát thương, loot, thoát; kịch bản input giả lập + ảnh chụp |
| M5 Đủ game demo | Tutorial Lua, quest, kho đồ, NPC chức năng, talent, lưu game |

## Lộ trình

1. **Rip.** `tools/rip/export.sh` chạy AssetRipper 2.0.0 headless ra `~/vd-rip/ar-export` (ngoài git, sinh lại được).
2. **Thay stub thư viện bằng package thật.** AssetRipper Free xuất mọi assembly IL2CPP thành stub thân rỗng.
   Package có nguồn (UGUI/TMP, URP, Core, Timeline, Input System, Cinemachine 2, AI Navigation, 2D Animation,
   Spine 4.2, ...) được thay bằng bản thật; GUID script trong mọi asset được ánh xạ lại bằng công cụ.
   Thư viện trả phí không có nguồn (MasterAudio, Febucci TextAnimator, DOTweenPro, ...) giữ stub và tự viết phần chạy.
3. **Shader.** Bản Free chỉ xuất shader dummy. Shader của package lấy lại từ package thật theo tên.
   Shader riêng của game (LCArt, ArtTeam VFX Master, ...) viết lại theo property + ảnh tham chiếu.
4. **Code game.** De.Base / De.InGame / De.Scenes / NemoLib... giữ nguyên tên type và field để prefab bám được,
   viết thân method theo bản dịch ngược từng method (`il2cpp_method.py`, tool của phiên decode-kit) và bản web
   `~/survivor-web-hub/games/voiddiver` làm tham chiếu hành vi. Bảng và Lua đọc nguyên văn như game gốc.
5. **Từng khu vực:** Splash/Title → Lounge → Tutorial → Dive → kho đồ/quest → âm thanh.

## Luật

- Không tự vẽ, không sinh art. Thiếu asset thì ghi vào `docs/MISSING.md`.
- Mọi bước biến đổi export là script trong `tools/`, chạy lại được.
- Một Unity batchmode một lúc, luôn có `timeout`.
- Nhật ký quyết định: `.audit/decisions.tsv`.
