# Giải mã VOID DIVER thành Unity project

Ghi lại cách làm và các bẫy đã dính, để lần sau (hoặc game Unity IL2CPP khác) không dính lại.

## Chuỗi công cụ

```
~/vd-src/<game>  --tools/rip/export.sh-->  ~/vd-rip/ar-export      (AssetRipper 2.0.0 headless, ~2,5 phút)
                 --tools/rip/fetch_donors.sh--> ~/vd-rip/donor     (package thật: tarball của editor, Spine git, NuGet)
                 --tools/rip/build_project.py--> ~/vd-rip/stage    (thay stub, ánh xạ GUID, GUID tất định)
                 --tools/rip/fix_stubs.py-->   (codemod cho stub code game)
                 --tools/rip/sync.sh-->        repo
```

`tools/rip/regen.sh` chạy cả chuỗi. Asset sinh ra không vào git (`.gitignore`); repo chỉ giữ công cụ,
code game viết lại, `Assets/_VD`, ProjectSettings và manifest.

Phép kiểm sau mỗi lần build:
- `python3 tools/rip/check_refs.py ~/vd-rip/stage ~/vd-rip/donor/com.unity.*`: GUID gãy phải bằng mốc của
  bản xuất thô (148 GUID, 223 tham chiếu, toàn asset không được ship: SRDebugger, nguồn PSB của 2D Animation).
  Số lớn hơn nghĩa là công cụ làm gãy tham chiếu.
- `tools/rip/script_census.py`: mọi script được asset tham chiếu phải ánh xạ được sang package thật.

## Sự thật đo được

- Unity 6000.3.5f2, IL2CPP, metadata v39, Addressables 2.8.1 catalog nhị phân. 23.307 asset.
- AssetRipper 2.0.0 Free đọc được v39. Thân method IL2CPP chỉ là stub rỗng (Level 2; Level 3 là bản trả phí).
  Shader chỉ xuất được dạng dummy (decompile là bản trả phí).
- Bundle Addressables đặt key container bằng GUID gốc, và tên AssetBundle bên trong là hash. Tên đọc được
  nằm ở tên file bundle; `tools/rip/bundle_names.py` sinh `bundle_names.json`.

## Bẫy

1. **GUID asset ngẫu nhiên.** AssetRipper gọi `UnityGuid.NewGuid()` cho mọi asset không phải script
   (`Source/AssetRipper.Export.UnityProjects/AssetExportCollection.cs:78`). Hai lần export cho hai bộ GUID khác nhau,
   nên file nào trong git trỏ tới asset bằng GUID sẽ gãy sau khi regen. `build_project.py` thay bằng
   `md5("vd:" + đường dẫn trong project)`. GUID script thì ổn định (`ScriptHashing`).
2. **Tên chỉ khác hoa/thường.** Trên Linux AssetRipper ghi cả `Default.asset` lẫn `default.asset`; Unity bỏ qua một
   trong hai và cảnh báo "Case sensitive file systems are not fully supported". `case_unique()` đổi tên cái sau thành `_caseN`.
3. **Netcode ILPP.** Game đã qua post-processor của Netcode, stub có `__initializeVariables`, `__initializeRpcs`,
   `__getTypeName`. Không biên dịch được với Netcode 2.12; editor sẽ tự sinh lại. `fix_stubs.py` xoá.
4. **NetworkList<T>** ở Netcode 2.x đòi `T : unmanaged`; game viết `struct`. `fix_stubs.py` đổi.
5. **Burst** sinh `__JobReflectionRegistrationOutput__*.cs` vào assembly game; xoá, editor tự sinh.
6. **Spine 4.2** tách `spine-csharp` thành package riêng (`spine-csharp/src`); `spine-unity` khai báo phụ thuộc vào nó.
7. **Shader TMP** không nằm trong package `com.unity.ugui` mà trong `Package Resources/TMP Essential Resources.unitypackage`.
   `fetch_donors.sh` bung nó ra (giữ GUID).
8. **Settings của AssetRipper qua REST** bị bỏ một phần: `ScriptExportMode` luôn thành `Hybrid` với IL2CPP. Xem dòng
   `General :` trong log để biết setting thật.
9. **Đừng `pkill -f <chuỗi có tên repo>` từ shell của agent**: pattern khớp luôn dòng lệnh của chính shell đó và giết nó.
   Dùng `tools/kill-match.sh <regex>`: nó bỏ qua chính nó và các tiến trình cha.
10. Trên bảng (CSV) và Lua: `TableEncrypted/*.bytes` XOR 0xCC, `LuaEncrypted/**.bytes` XOR 0xF4 (đo trên Windows, phiên decode-kit chấm 83/83 bảng, 196/196 Lua khớp).
11. **Asmdef của AssetRipper thiếu reference.** Danh sách `references` không đủ những assembly mà code gốc thật sự gọi
    (NemoLib gọi `UniTask.Addressables` và `Serilog.Sinks.Unity3D`, cả hai không có trong asmdef). Stub thân rỗng nên
    không lộ; tới khi dịch code thì Unity báo lỗi. `tools/rip/asmdef_refs.py` thêm mọi assembly thư viện vào asmdef
    của code game (thư viện không bao giờ trỏ ngược vào game nên không sinh vòng).
12. **Harness phải giống Unity:** chỉ thấy assembly mà asmdef khai báo cộng DLL precompiled, và cần facade `mscorlib`
    (DOTween.dll nhắm .NET 3.5). Harness cũ tham chiếu mọi DLL nên đã cho qua lỗi mà Unity bắt.
13. **`__GEN/NetworkVariableSerializationHelper` trong stub.** Netcode ILPP của editor sinh lại kiểu này, assembly có
    hai kiểu trùng tên và Burst treo khi băm IL ("An item with the same key has already been added", editor quay 120% CPU,
    log im). `fix_stubs.py` xoá mọi thư mục `__GEN`. `tools/unity-run.sh` giết Unity khi log im quá 10 phút.
14. **AssetRipper không xuất PlayerSettings.** Unity tự sinh `ProjectSettings.asset` mặc định: color space Gamma (gốc là
    Linear), Input Manager cũ (gốc là Input System), và không preload `VContainerSettings`. Thiếu cái cuối thì VContainer
    không dựng `RootLifetimeScope`, mọi field `[Inject]` null (NRE ở `SceneLoader`, `GlobalShaderManager`), màn hình đen.
    `tools/rip/player_settings.py` chép mọi field scalar và `preloadedAssets` từ `data.unity3d` của game.
15. **`EditorBuildSettings.asset` mang con trỏ tới settings Addressables** (`m_configObjects: com.unity.addressableassets`).
    Bước `addressables` ghi nó vào. Khôi phục file này bằng `git checkout` là Addressables mất settings: play mode không build
    fast mode, runtime đi tìm `Library/com.unity.addressables/aa/<platform>/settings.json` và mọi lần tải asset đều hỏng.
16. **GUID của asset phải là GUID gốc.** Field `AssetReference` (như `RootLifetimeScope._environmentSettings`) lưu GUID
    asset của project gốc, và catalog cũng lấy GUID đó làm key. AssetRipper đặt GUID ngẫu nhiên, ta từng đổi sang
    `md5(path)`, nên `LoadAssetAsync` báo `No Location found for Key=<guid>`. Giờ `address_map.py` ghi GUID gốc cho mỗi
    entry và `build_project.py` dùng nó (16.890 asset); `repo_guids.py` sửa theo ở các file repo tự quản.
    `check_refs` phải được xem cả `Assets/_VD` của repo, nếu không shader dựng lại bị tính là tham chiếu treo.
17. **Không còn desktop thì không có display.** `unity-run.sh` mượn Xwayland của phiên desktop; khi người dùng đăng xuất,
    chỉ còn Xwayland của màn hình đăng nhập (user gdm, file auth không đọc được) và Unity thoát ngay với
    `cannot open display` (`peakRSS=8MB`). Giờ nó tự dùng Xvfb riêng (`tools/xvfb.sh`, giải nén gói Ubuntu, không cần root).
18. **Harness xanh chưa phải Unity xanh với package nhúng.** Harness build từng assembly trong `Assets/Scripts`, nên lỗi
    trong `Packages/<embedded>` và lỗi tham chiếu giữa các asmdef của package (Odin: `using Utilities;`, thiếu
    `Sirenix.Utilities`) chỉ lộ khi Unity compile. Đổi package nào thì chạy một lần Unity compile
    (`tools/unity-run.sh <log> -executeMethod VD.EditorTools.Batch.Run -vdSteps census`, xem `errorsCS=0`) trước khi commit.
19. **SpriteAtlas không được xuất; vài Sprite nằm trong texture kiểu Default.** AssetRipper Free bỏ 29 SpriteAtlas.
    `tools/rip/sprite_atlases_extract.py` đọc chúng từ bundle (`tools/rip/sprite_atlases.json`: GUID gốc, sprite đã đóng gói),
    bước Batch `spriteatlas` dựng lại vào `Assets/_VD/SpriteAtlas/` (cả 29 là `.spriteatlasv2`, xem bẫy 21;
    `SpriteAtlasAsset.Save` chỉ ghi file, phải `ImportAsset` mới có `.meta`). 13 sprite gốc là sub-asset của texture (chung GUID), bản xuất để texture
    ở kiểu Default nên không có Sprite: bước này đổi chúng sang Sprite Single (log `texture imported as Sprite`). Sau
    `regen.sh` phải chạy lại `-vdSteps spriteatlas,addressables` (log `VD_SPRITEATLAS_DONE atlases=29/29 verified=29`).
20. **Shader dựng lại từ DXBC mang sẵn phép lật UV của D3D.** `GetFullScreenTriangleTexCoord` của URP lật `uv.y` khi
    `UNITY_UV_STARTS_AT_TOP`; bytecode D3D đã biên dịch phép đó thành hằng (`r0.z = 1 - r0.y`), nên trên OpenGL mỗi lần
    blit qua shader dựng lại (FinalGamma, UnifiedScreenEffect, ScreenBlurUnified...) lật ảnh một lần. Renderer Main có ba
    pass như vậy nên ảnh A1 lật dọc; tắt bất kỳ một trong ba (CustomScreenFeature, ScreenPostProcessingRenderer,
    ScreenBlurUnifiedRendererFeature) là ảnh đứng lại (A1 lần 27). Lỗi ở shader, không ở tool chụp: màn hình GL cũng lật.
    Đã sửa ở bộ sinh (`tools/shaders/uvflip.py`, docs/SHADERS.md bẫy 15): mỗi blob có mẫu D3D-only được dịch hai lần,
    bản gốc và bản GL (nhánh `#else` của chính mã nguồn URP/Shader Graph), dòng khác nhau nằm dưới
    `#if UNITY_UV_STARTS_AT_TOP ... #else ... #endif`; phía D3D giữ nguyên từng dòng của bản dịch cũ (7.510/7.510 include).
    Không chỉ fullscreen VS: `_ScaleBiasRt` (TransformNormalizedScreenUV) và PixelPosition của Shader Graph
    (`_ProjectionParams.x < 0`) trong pixel shader cũng bị nướng như vậy, 51 shader / 397 include. Kiểm: ShaderGate
    15.020 biến thể 0 lỗi (`w3-uvflip-gate1`); A1 lần 46, đủ ba pass, `title-menu.png` đứng như ref (vòng sáng ở trên,
    nhóm nhân vật góc dưới phải).
21. **Atlas chưa pack thì `SpriteAtlas.GetSprite` trả null.** AssetRipper không xuất `EditorSettings`, project mới có Sprite
    Packer mode Disabled: atlas dựng lại không bao giờ được pack, `AssetManager.GetSprite` log `Resource load failed. Atlas :
    Icon_Skill` và mọi icon kỹ năng/vật phẩm vẽ ra ô trắng (Image không có sprite). Bước kiểm cũ chỉ đếm `packables:` trong
    YAML nên vẫn báo 29/29. Giờ `ProjectSettings/EditorSettings.asset` để mode 5 (SpriteAtlasV2), bước `spriteatlas` tự đặt mode
    đó, pack hết và kiểm `spriteCount` + `GetSprite` theo tên. Đo trên 6000.3: mode V2 pack được cả file `.spriteatlas` cũ,
    nhưng atlas cũ ghi dữ liệu pack vào chính file đang track ở mỗi lần pack, nên cả 29 atlas giờ là `.spriteatlasv2`.
22. **Shader trong `Samples~` hay trong package nhúng không phải donor.** `names.py` coi mọi shader dưới `Packages/` là package
    đã có, kể cả `Samples~` mà Unity không bao giờ import; `build_project.py` chỉ thay dummy bằng shader của `SHADER_DONORS`,
    không gồm package nhúng. Kết quả: 4 dummy SoftMask ở lại `Assets/Shader`, dummy `Hidden/UI/Default (SoftMaskable)` thắng
    bản thật của package khi `Shader.Find`, còn `Hidden/TextMeshPro/Distance Field (SoftMaskable)` chỉ có bản dummy, nên chữ
    dưới SoftMask (chat log của HUD) vẽ đen. Keyword trong `data.unity3d` (`SOFTMASKABLE`, `SOFTMASK_EDITOR`) cho biết game
    import sample "TextMeshPro Support (Unity 6)" của SoftMask v3: `VENDOR` chép sample vào `Assets/Samples/UI Soft Mask/`,
    `SHADER_DONORS` thêm `Shaders/` của package và sample đó. Kiểm dummy còn sót: `grep -l DummyShaderTextExporter Assets/Shader`.
21. **Shader dựng lại mang sẵn quy ước depth của D3D (reversed Z).** D3D11 ghi depth 1 ở near, 0 ở far; OpenGL ngược lại
    và clip z chạy -1..1. Code game so depth với hằng và blend op đã nướng sẵn: BG_Base lấy `SV_Position.z - customDepth.b`
    để biết tường ở trước nhân vật (cắt lỗ nhìn xuyên), `_CustomDepthBuffer` gộp bằng BlendOp Max/Min, blur và SSAO dựng lại
    vị trí từ depth qua `unity_MatrixInvP`. Trên GL mọi phép so lật: sàn quanh nhân vật bị cắt thành đĩa tối có viền dither,
    tường thành đỏ, blur độ sâu phủ cả màn hình (ảnh mờ như upscale), bóng thành khối chữ nhật. Không phải lỗi của tool chụp
    hay render scale (Review.cs chụp đúng 1280x720, renderScale 1). Cách nhận ra nhanh: chạy cùng test với `-force-vulkan`
    (Vulkan dùng quy ước D3D: reversed Z, UV từ trên xuống) thì ảnh đúng như clip; tắt từng feature/renderer
    (`A4c_StageProbe`, ảnh `captures/review/probe-*`) chỉ ra mesh `SkeletonCircleMaskArea/Msh_MaskArea_Cone` (LightMode
    CustomDepth). Sửa ở bộ sinh: pass nào ghi/đọc custom depth hoặc dựng vị trí từ depth chạy bằng giá trị depth của D3D trên
    GL (docs/SHADERS.md bẫy 16). Bẫy chung: bytecode D3D nướng cả quy ước UV lẫn quy ước depth; sửa UV (bẫy 20) chưa đủ.
