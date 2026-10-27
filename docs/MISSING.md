# Thiếu so với game gốc

Thứ không phục hồi được từ bản build, hoặc chưa làm. Không tự vẽ, không thay thế; ghi ra đây.

## Không có trong bản build (không phục hồi được)

| Thứ thiếu | Ảnh hưởng | Bằng chứng |
|---|---|---|
| Script của SRDebugger UI (StompyRobot, công cụ debug trả phí) | 23 prefab debug trong `Resources/srdebugger` và `GameObject/*Tab*` mất 98 component. Không phải nội dung game. | `captures/census.json` `prefabsWithMissing` |
| 148 GUID mà asset trỏ tới nhưng không được ship (nguồn PSB của 2D Animation, asset editor) | Không ảnh hưởng runtime | `tools/rip/check_refs.py` trên bản xuất thô: 148 GUID / 223 tham chiếu |
| Key Addressables mà bảng/code của game gốc dựng ra nhưng catalog bản demo không có (6 key do dữ liệu ghi tên): `EventUi/Ping/{Cancel,Item,Skill,Status}.prefab` (4 giá trị `EPingType` không có prefab ping), `Object/DisposableObject/SampleBox.prefab` (bảng DisposableObject chỉ có hàng mẫu), `Vfx/Skill/common/SkillReady_Enemy01.prefab` (Skill 21001003 "문어 보스 소환" viết `common` chữ thường, catalog là `Common`; key Addressables phân biệt hoa thường) | Game gốc cũng không tải được các key này; nếu đi tới, log `Resource prefab(...) not found` là đúng như bản gốc. Không thay bằng asset khác. | `python3 tools/rip/address_audit.py` (2026-10-25), mục `game keys absent` |
| 115 id trong bảng không có asset theo id (`Unit/Monster/{id}` 18, `Object/Npc/{id}` 4, spine `Npc/{id}.asset` 93) | Cận trên: audit thử mọi id của bảng. Phần lớn là hàng không dùng trong demo (quái 2000xx chưa ra, NPC không có spine, pha boss 300015/300016 đổi `TableId` trên cùng object chứ không tạo prefab mới) | như trên |

## Chưa làm (phục hồi được, đang làm)

| Thứ | Tình trạng |
|---|---|
| Shader riêng của game (~100: LCArt, ArtTeam VFX, Shader Graphs của game) | AssetRipper Free chỉ cho shader dummy. 1.519/1.964 material đang dùng dummy (census 2026-10-22). Blob gốc là DXBC (D3D11). |
| Thân method của code game (De.*, NemoLib, LCArt, ...) | Stub rỗng. Đang viết lại theo bản dịch ngược từng method. |
| Thư viện trả phí không có nguồn: MasterAudio, Febucci TextAnimator, DOTweenPro, ParticleImage, InputIcons, EnhancedScroller, ChocDino UIFX, ACTk | Giữ stub để prefab bám được; phần chạy phải tự viết theo hành vi cần dùng. |

## Thư viện trả phí (2026-10-23)
MasterAudio, Febucci Text Animator, InputIcons, ChocDino UIFX, ACTk, EnhancedScroller, ParticleImage, SRF, Graphy là sản
phẩm trả phí của bên thứ ba. Repo công khai này chỉ giữ stub của AssetRipper (bị `.gitignore`). Bản dịch từ game nằm
ở máy dựng (`~/vd-rip/closed-libs`, không đẩy lên đâu). Người khác dựng lại project cần mua các thư viện này, hoặc tự
dịch lại từ bản game của mình.

## Màn chưa có khung clip để so (2026-10-25)
Không clip nào trong `docs/review/ref/SOURCES.tsv` quay cảnh này; chỉ kiểm theo dữ liệu và luồng code, chưa so được với game thật:
- `a7-quest-detail` (bảng quest với một mục đang chọn) và `a7-quest-panel-continued` (bảng quest sau Continue). Đã dò cả 4 clip
  mỗi 3 s: bảng quest chỉ hiện ở W0YYGShicSk 672 s và 918–927 s, khung chi tiết luôn trống (`e151cfeb`).
- A9b (hệ thống sảnh), không có khung clip trong SOURCES.tsv nên chỉ kiểm theo bảng và luồng: `a9b-tycoon-main`/`-hire`/`-started`,
  `a9b-sales-slots`, `a9b-interior-list`/`-bought`, `a9b-sofa-talent`/`-done` (cây trait sofa), `a9b-gatekeeper-room` (phòng multiplayer),
  `a9b-pim-shop` (cửa hàng artifact), `a9b-ryujin-shop`, `a9b-atm-open`/`-done`/`-dive` (đổi coin). Màn Woung (`a9b-woung-*`) có khung `npc-trade/01`.

## Khác bản gốc có chủ ý (2026-10-27)
- **Tắt tiếng mặc định** (`Assets/_VD/Runtime/PlaytestMute.cs`, `bfc639e1`, `aa5cbe74`): theo yêu cầu chủ dự án, mọi lần chạy (editor, test, bản build) khởi động với `AudioListener.volume = 0`; game gốc có tiếng. Bật lại: menu VOID DIVER/Sound On (editor), `VD_SOUND=1`, hoặc cờ `-vd-sound` khi chạy bản build. Muốn bản build giống gốc: xoá thư mục `Assets/_VD/Runtime` (và tham chiếu `VD.Runtime` trong `VD.Editor.asmdef`, file `SoundMenu.cs`); không đoạn code game nào phụ thuộc vào nó.
