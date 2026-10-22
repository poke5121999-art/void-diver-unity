# Khung tham chiếu review (VOID DIVER demo)

Khung lấy từ clip YouTube của bản demo gốc, dùng để so cạnh nhau theo `~/.claude/skills/parity-compare/SKILL.md`:
bên này là khung gốc `<key>/NN.jpg`, bên kia là ảnh Unity chụp cùng trạng thái. Mỗi key một trạng thái; `NN` là biến thể.
Nguồn từng khung (URL, giây, ngày clip) nằm trong `SOURCES.tsv`. Kiểm bằng ffmpeg: mọi khung khớp đúng giây ghi trong bảng.

Phiên bản clip: Feb 2026 (20260205), Apr 2026 (20260413), Jul 2026 Demo 2.0 (20260727). Khung số nhỏ nhất của mỗi key là bản mới nhất.
Khác biệt giữa các bản: options Jul có thanh tab icon bên trái, Feb là danh sách phẳng; Apr có nút Operations Manual F1 và 6 ô skill
kiểu cũ, Jul đổi icon skill và thêm boss Hachishakusama (Apr chỉ có Ex-Chief Donovan); cây talent Apr là Tech Development, Jul là Shop/Crafting Trait.

## Key theo vùng (theo docs/PLAN.md)
- A1 tiêu đề: title-menu, title-options
- A2 sảnh: lounge-arrival, lounge-npc-talk, lounge-walk, lounge-phonebooth
- A3 chức năng NPC: npc-campaign-select, npc-character-select, npc-trade, npc-artifact-deal, npc-combat-trait, npc-craft, npc-storage
- A4 tutorial: tutorial-start, tutorial-dialog, tutorial-first-combat
- A5 lặn: dive-loading, dive-explore, dive-combat, dive-skill-vfx, dive-hit-damage-numbers, dive-loot-pickup, dive-inventory, dive-minimap, dive-stress-high, dive-trap, dive-exit, dive-death
- A6 boss: boss-intro, boss-fight
- A7 kết quả, quest: result-screen, quest-panel
- A8 âm thanh: không có khung (không xem được bằng ảnh)

## Bỏ qua khi so
Overlay Discord (góc trên trái, title-menu và title-options), overlay Steam (góc dưới phải, cùng hai key đó), dòng id người stream nhỏ ở góc trên trái dive-minimap/01,
con trỏ chuột trên menu. Webcam: không có trong khung nào.

## Chưa chắc / chưa giải quyết
- npc-craft: chỉ có ở clip Apr, chưa tìm thấy panel chế tạo trong clip Jul; UI Jul có thể khác.
- quest-panel: tab Objectives chưa chọn mục nào nên khung chi tiết bên phải trống.
- dive-loot-pickup/01: máu 0 và HUD tối (đang gần chết), màu bị đỏ; 02 sạch hơn về HUD nhưng là xác để Inspect, không phải vật nhặt.
- boss-*: Unity demo có thể không cùng boss với khung Apr; khung Jul là Hachishakusama.
