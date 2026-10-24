# Shader dựng lại từ DXBC (W3)

AssetRipper Free chỉ xuất shader dạng dummy (`//DummyShaderTextExporter`). Shader riêng của game được dựng lại từ
chính bytecode D3D11 mà game ship, vào `Assets/_VD/Shaders/<tên đã làm sạch>.shader`, giữ nguyên tên shader,
Properties, tag, render state, keyword và bố cục constant buffer gốc. Không viết tay shader nào.

## Chuỗi công cụ (`tools/shaders/run.sh`, chạy lại được)

1. `names.py`: danh sách cần dựng = mọi tên dummy trong bản xuất thô (`~/vd-rip/ar-export`) trừ tên mà package
   donor (URP, Spine, TMP, ugui, shadergraph) hoặc package nhúng trong `Packages/` đã có. Không lấy từ census vì
   census chỉ thấy shader có material; shader nạp bằng code (post-processing, ChocDino UIFX, Outline) cũng là dummy.
2. `extract.py` (UnityPy, `-I`): parsed form + blob D3D11 + bố cục cbuffer của từng subprogram. Bundle shader
   trước, rồi quét mọi bundle và `data.unity3d` qua `~/handoff/heavy.sh`. Một shader có mặt ở nhiều container
   (mỗi bản bị strip theo nội dung của container đó): giữ bản nhiều subprogram nhất.
3. `decompile.py`: mỗi blob qua `patchsig.py` rồi 3Dmigoto `cmd_Decompiler` dưới wine: `-D -V` ra HLSL (lấy chữ ký,
   khai báo texture/sampler/cbuffer; `-V` biên dịch lại bằng d3dcompiler) và `-d` ra disassembly `<n>.asm`.
4. `generate.py`: thân hàm dịch từ `.asm` bằng `asm2hlsl.py` (giữ đúng từng bit, xem bẫy 13), khai báo lấy từ HLSL
   của 3Dmigoto. ShaderLab + một include cho mỗi biến thể đã ship (`<tên>/p<pass>_<vs|ps>_<blob>.hlsl`), `cbN[i]`,
   `tN`, `sN` đổi lại thành tên gốc; cbuffer có `packoffset` gốc; `#if` chọn biến thể có tập keyword lớn nhất là
   tập con của keyword đang bật. GUID tất định `md5("vdshader:" + tên)`. `uvflip.py` thêm nhánh OpenGL cho các dòng
   D3D-only (bẫy 15).
5. `apply.sh`: như `tools/rip/rebuild.sh` (build_project ánh xạ material từ dummy sang `Assets/_VD/Shaders` theo
   tên) nhưng cho check_refs biết `Assets/_VD`; xem bẫy 1.
6. Kiểm trong Unity (lệnh đủ ở đầu `tools/shaders/shots.txt`): `tools/unity-run.sh <log> -executeMethod
   VD.EditorTools.Batch.Run -vdSteps pipeline,census,shadergate,capture,shots ...`. `ShaderGate.CompileAll` biên dịch
   **mọi** biến thể đã ship cho OpenGLCore và D3D (log `VD_SHADERGATE_COMPILE`); `capture` chụp sảnh
   (`captures/sector-9001.png`), `ShaderShots` chụp theo `shots.txt` ra `captures/shaders/`. `shot.sh` bọc lệnh trên và
   trả ProjectSettings về bản trước lúc chạy: chỉ dùng khi không ai khác đang sửa ProjectSettings (nó ghi đè cả thay
   đổi của người khác trong lúc Unity chạy).

## Bẫy

1. `rebuild.sh` chạy check_refs trên stage, mà stage không bao giờ có `Assets/_VD`, nên sau khi material trỏ sang
   shader dựng lại thì check_refs ra 243/2249 và từ chối sync. `apply.sh` thêm `Assets/_VD`; nên sửa `rebuild.sh`
   giống vậy rồi bỏ `apply.sh`. `sync.sh` còn xoá `Assets/Scripts.meta` (stage không có), phải `git checkout` lại.
2. fxc dồn nhiều varying vào một thanh ghi (TEXCOORD0.xy + TEXCOORD8.z...). 3Dmigoto đặt tên trùng (`p1` hai lần)
   và sai swizzle (`xww`): 99/1386 blob không biên dịch lại. `patchsig.py` đổi **mọi** thanh ghi interpolator
   của VS output / PS input thành `VDREG<số thanh ghi>` float4 (cả thanh ghi một phần tử, để VS và PS luôn cùng
   tên vì D3D11 nối theo thanh ghi), tính lại checksum DXBC. Sau đó 0 blob lỗi.
3. Quét mọi container ghi đè thư mục của shader đã tách từ bundle shader bằng bản `data.unity3d` ít biến thể hơn
   (BG_Base 60 → 60 nhưng VFX_Master_typeA 176 → 11). `extract.py` giữ bản lớn hơn.
4. Keyword: mỗi keyword một dòng `multi_compile` làm không gian biến thể thành 2^n (ChocDino Blend-Fill-Gradient
   18 keyword). Keyword cùng tiền tố không bao giờ đi cùng nhau trong dữ liệu ship được gộp về một dòng
   (`_ BLEND_ALPHABLEND BLEND_DARKEN ...`), cộng nhóm URP và `KeywordEnum` của property.
5. ShaderLab: `ColorMask` đặt render target **sau** mask (`ColorMask 0 1`), ngược với `Blend 1 ...`. SubShader
   của pipeline khác bị build strip còn mỗi tag, mà SubShader rỗng là lỗi parse: bỏ, để lại comment.
6. Unity in lại lỗi biên dịch cũ của include đã đổi (`redefinition of 'p1'` ở dòng không còn tồn tại) khi khởi
   động: tin `VD_SHADERGATE_COMPILE`, không tin dòng `Shader error` đầu log.
7. Bản ProjectSettings đã commit còn trỏ GUID `a82167dd...` (URP asset của bản xuất GUID ngẫu nhiên cũ); URP asset
   của game là `Assets/MonoBehaviour/UniveralRP_VD.asset` (`30a0fafef162b71e9d708abbfb0f3109`). Không có pipeline thì
   mọi shader URP của game vẽ magenta. Bước Batch `pipeline` (đặt trước `capture`/`shots`) gán pipeline trong bộ nhớ.
   Sửa hẳn nằm ở ProjectSettings (working tree đã trỏ đúng, chờ phiên ProjectSettings commit), không phải W3.
8. Color space: game là **Linear**; ProjectSettings đã là Linear (commit `d4037ec1`). Ảnh chụp trước đó (Gamma) bỏ.
   Màu property (kể cả `[HDR]`) do Unity đổi theo color space của project, shader không tự đổi.
9. Editor Linux render bằng OpenGLCore. Hằng số D3D đã nướng vào bytecode (`UNITY_REVERSED_Z`,
   `UNITY_UV_STARTS_AT_TOP`) vẫn còn trong HLSL dịch ngược. Các mẫu lật UV và z near-clip đã có nhánh GL (bẫy 15);
   mẫu nào chưa nhận ra (vd. so sánh depth theo reversed-Z) vẫn có thể sai trên GL; bản build Windows (D3D11) đúng như gốc.
10. Biến thể instancing/stereo không được dựng (`DROP_KEYWORDS`); material bật GPU instancing sẽ dùng biến thể
    thường.

11. URP không dựng được trong editor nếu có một `VolumeComponent` nào có field `*Parameter` null: editor quét mọi
    kiểu VolumeComponent bằng reflection, `OnEnable` NRE, constructor của `UniversalRenderPipeline` ném lỗi, ảnh
    đen tuyền. Stub `WeatherVisualVolumeComponent` mất initializer gây ra lỗi này (sửa ở `36054c3a`). Ảnh đen 100%
    nghĩa là pipeline không dựng được, không phải lỗi shader: tìm `UniversalRenderPipeline..ctor` trong log.
12. Máy chạy locale vi_VN: `float.Parse("0.15")` ra 15. Công cụ editor phải parse bằng `InvariantCulture`.
13. **Thân hàm của 3Dmigoto sai với code số nguyên.** Nó giữ mọi thanh ghi tạm dạng float4 và đổi qua lại float/int
    ở từng lệnh: `and` mặt nạ bit thành `? :`, `ushr` thành dịch có dấu, mặt nạ 32 bit cất trong float mất 8 bit thấp.
    Vòng lặp đèn Forward+ của URP (trong `shader_DE_Character_Normal`) không bao giờ xoá bit nên quay mãi: GPU bỏ
    khung hình, render target giữ nguyên bộ nhớ cũ (nhiễu, mảnh atlas UI lặp lại), llvmpipe thì treo. Trông như lỗi
    của `CustomRenderObjectsFeature` (tắt feature là hết, vì chỉ feature này vẽ pass `CustomForward`), nhưng shader
    thử màu phẳng cùng LightMode, cùng stencil qua feature vẫn vẽ đúng. `asm2hlsl.py` dịch thẳng disassembly: thanh ghi
    tạm là uint4 chứa bit thô, mỗi lệnh đọc nguồn theo kiểu của nó (`asfloat`/`asint`) và cất `asuint`. Lệnh nào nó
    chưa biết thì giữ thân 3Dmigoto và báo `body3dm` trong `generate_report.json` (hiện 0).
14. Ảnh chụp qua renderer Main (`-vdRenderer 1`, có hậu kỳ) từng bị lật dọc trên GL, giống ảnh title của A1: mỗi blit
    qua shader dựng lại lật một lần, renderer Main có ba pass như vậy (DECODING bẫy 20). Đã sửa ở bẫy 15.
15. **Lật UV của D3D (`tools/shaders/uvflip.py`).** Mỗi blob khớp mẫu được dịch hai lần (asm gốc và asm đã sửa theo
    nhánh GL của chính mã nguồn URP/Shader Graph), `merge` đặt các dòng khác nhau dưới
    `#if UNITY_UV_STARTS_AT_TOP ... #else ... #endif`; PRELUDE định nghĩa macro này cho mọi API trừ GLCore/GLES3 như
    `ShaderLibrary/API/*.hlsl` của core (chương trình không include gì của URP). Phía D3D trùng từng dòng với bản dịch cũ
    (bỏ nhánh `#else` thì 7.510/7.510 include giống hệt). Mẫu (khảo sát trên mọi blob, số = biến thể trong report `uvflip`):
    - `vs-uv` 26: VS nhận `SV_VertexID` dùng `GetFullScreenTriangleTexCoord` (Blit.hlsl `Vert` của URP trong FinalGamma,
      ScreenBlur*, CampaignTilt/EdgeOutline, SSAO, UnifiedShadow; DitheringScreen_Optimized). `uvflip` chạy thử VS với
      vertex id 0..3: lệnh `add d, -s, 1.0` mà `s` = `0.5 * clip.y + 0.5` ở mọi đỉnh là phép lật; GL: `mov d, s`.
    - `vs-uv(movc)` 20: VS fullscreen riêng của game (UnifiedScreenEffect, DynamicVignette, Sanity, Snapshot,
      LateCustomScreenUI, DitheringScreen): `uv.y = _FlipY < 0.5 ? 1 - v : v`. GL đổi chỗ hai nhánh (giữ nghĩa
      `_FlipY`; code game không đặt nó nên luôn 0).
    - `vs-nearz` 46: cùng các VS đó ghi `clip.z = 1.0` (UNITY_NEAR_CLIP_VALUE của D3D reversed-Z); GL `-1.0`.
    - `ps-scalebiasrt` 198: `TransformNormalizedScreenUV` (`1 - (v * _ScaleBiasRt.x + _ScaleBiasRt.y)`, sau
      `GetNormalizedScreenSpaceUV`: tile Forward+, SSAO, texture màn hình) ở BG_Base, Character_Normal, Shader Graph...;
      GL không biến đổi.
    - `ps-pixelpos` 197: PixelPosition của Shader Graph (`_ProjectionParams.x < 0 ? _ScreenParams.y - y : y`); GL dùng
      `> 0` như template.
    `_ProjectionParams.x` dùng ở chỗ khác (`ComputeScreenPos`) là uniform lúc chạy, không phải hằng, nên không sửa. Mẫu
    nào có hình dạng lạ thì report ghi `...: <lý do>` thay vì đoán (hiện 0).

## Phủ

- 191 tên shader dummy trong bản xuất thô: 56 do package có sẵn, **135 dựng lại từ DXBC, 0 hỏng** ở bước sinh
  (`~/vd-rip/shaders-work/generate_report.json`, 7.510 blob, `body3dm` 0). Unity biên dịch 135 shader, **15.020 biến
  thể (OpenGLCore + D3D), 0 lỗi** (`~/vd-rip/logs/w3-gate7.log`, 2026-10-24; sau bẫy 15:
  `~/vd-rip/logs/w3-uvflip-gate1.log`, 15.020 biến thể, 0 lỗi).
- Census cùng lần chạy: 1.964 material, **0 trên shader dummy** (trước W3: 1.519), 39 không có shader (của bản xuất,
  không phải W3).
- Chưa dựng: biến thể instancing/stereo (bẫy 10). Không shader nào bị bỏ hay viết tay.

## Ảnh kiểm (w3-gate7, Linear, OpenGLCore, editor)

- Sảnh `captures/sector-9001.png` (renderer Default): sàn ca rô, thảm, đèn bàn, tượng hươu, rắn hổ mang, tủ kính, cây,
  lan can kính khớp ss05 và `docs/review/ref/lounge-*`.
- Unit `captures/shaders/unit-100001.png`, `unit-100001-lounge.png`: nhân vật Spine vẽ sạch qua
  `CustomRenderObjectsFeature` (lỗi khung nhiễu cũ là bẫy 13, đã hết).
- VFX `captures/shaders/vfx-*_t*.png`: vòng kiếm xanh trắng, đĩa đỏ dưới đất, cầu lửa cam.

## Còn mở

- Ảnh tĩnh sáng và phẳng hơn ss05: thiếu thứ do code runtime dựng (camera rig, global shader như
  `_GlobalCharacterShadowColor`, `_PlaneSightRenderTarget`, `_GlobalIsRuntime`, sương mù, đèn pin/vùng tối, vignette,
  pixel hoá, HUD, NPC). So đúng phải làm trong play mode (vòng review A2).
- `captures/shaders/sector-9001-main.png` (renderer Main): ảnh cũ lật dọc (bẫy 14, đã sửa ở bẫy 15, chưa chụp lại) và có
  vệt tối loang (SSAO/blur hậu kỳ chạy không có dữ liệu runtime). Chưa xét lại trong play mode.
- Bản build Windows (D3D11) chưa chạy thử; bẫy 9 chỉ kiểm được ở đó.
