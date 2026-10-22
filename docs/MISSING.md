# Thiếu so với game gốc

Thứ không phục hồi được từ bản build, hoặc chưa làm. Không tự vẽ, không thay thế; ghi ra đây.

## Không có trong bản build (không phục hồi được)

| Thứ thiếu | Ảnh hưởng | Bằng chứng |
|---|---|---|
| Script của SRDebugger UI (StompyRobot, công cụ debug trả phí) | 23 prefab debug trong `Resources/srdebugger` và `GameObject/*Tab*` mất 98 component. Không phải nội dung game. | `captures/census.json` `prefabsWithMissing` |
| 148 GUID mà asset trỏ tới nhưng không được ship (nguồn PSB của 2D Animation, asset editor) | Không ảnh hưởng runtime | `tools/rip/check_refs.py` trên bản xuất thô: 148 GUID / 223 tham chiếu |

## Chưa làm (phục hồi được, đang làm)

| Thứ | Tình trạng |
|---|---|
| Shader riêng của game (~100: LCArt, ArtTeam VFX, Shader Graphs của game) | AssetRipper Free chỉ cho shader dummy. 1.519/1.964 material đang dùng dummy (census 2026-10-22). Blob gốc là DXBC (D3D11). |
| Thân method của code game (De.*, NemoLib, LCArt, ...) | Stub rỗng. Đang viết lại theo bản dịch ngược từng method. |
| Thư viện trả phí không có nguồn: MasterAudio, Febucci TextAnimator, DOTweenPro, ParticleImage, InputIcons, EnhancedScroller, ChocDino UIFX, ACTk | Giữ stub để prefab bám được; phần chạy phải tự viết theo hành vi cần dùng. |
