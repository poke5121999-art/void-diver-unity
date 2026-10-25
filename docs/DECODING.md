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
   `__getTypeName`. Không biên dịch được với Netcode 2.x (2.12 lúc đó, 2.8.1 từ bẫy 40); editor sẽ tự sinh lại. `fix_stubs.py` xoá.
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
23. **Nhãn chuỗi của Ghidra đã bị làm sạch: `StringLiteral__Text` không phải `"Text"`.** Ghidra đặt tên literal bằng
    cách thay ký tự không hợp lệ bằng `_`, nên `"@Text"` và `"_Text"` cùng thành `StringLiteral__Text` (hai gạch), còn
    `"Text"` là `StringLiteral_Text` (một gạch). Bản dịch đọc nhãn như tên và viết `"Text"`: `FindChildComponent` trả null
    vì con trong prefab tên `@Text`, popup chọn nhân vật ném NRE giữa `OnOpenAsync` (A3 lần 2: 6 tag giữ chữ mẫu "근거리",
    không có sprite xem trước, Esc không đóng). Tra giá trị thật trong `out/vd/code/strings.tsv` (nhãn, địa chỉ, chuỗi);
    nhãn trùng thì phân giải bằng địa chỉ trong disassembly hoặc tên trong asset. Kiểm cả repo:
    `python3 -I tools/rip/literal_label_audit.py` (lớp dùng `"X"` trần trong khi nhãn `__X` chỉ có `@X`/`_X`).
24. **`?.` và `!= null &&` của bản gốc bị dịch mất.** Trong pseudo-C, `x?.Dispose()` là `if (p == 0x0) return;` (hoặc
    `if (p != 0x0) Dispose`), còn dereference không kiểm thì nhảy tới `il2cpp_throw_null_reference`. Bản dịch hay bỏ
    nhánh kiểm: 9 `_disposable.Dispose()` trong `OnDestroy`/`OnDisable`/`Clear` (field không có initializer), lambda
    `d.RootTalentIds.Contains(...)` của TalentPopup, `IsSameFilter` inline trong `StashFilterPanelPresenter`,
    `GetDialogInfos` (gốc tạo list rỗng khi thiếu khoá, bản dịch trả null). Hậu quả chung: popup ném giữa `OnOpenAsync`,
    nên `OnInputEnabled` không chạy và **Esc không đóng popup**. Gặp popup không đóng bằng Esc thì tìm exception lúc mở
    trước. Khi dịch, mỗi `== 0x0) { return` trước một lời gọi phải thành guard trong C#.
25. **Vòng lặp đọc field qua offset thô bị bỏ cả khối.** Ghidra không gõ kiểu phần tử của `Object__Array`, nên field
    của phần tử hiện thành `pOVar2[1].klass` (offset 0x10 = field đầu, ví dụ `TNpcFunction.Id`) hay `pOVar2[3].monitor`
    (0x38 = `Percent`). `NpcObject.OnHostSpawn` có vòng như vậy trên các dòng `TNpcFunction` của NPC; bản dịch bỏ vòng và
    đảo điều kiện `TryGetValue`, nên mọi NPC trong sảnh có `HasFunction=false` và F chỉ ra thoại. Đổi offset ra field
    bằng `bin/udk type <T>` (dump ghi offset từng field) rồi dịch lại vòng; một method dịch ra ngắn hơn hẳn pseudo-C là
    dấu hiệu cần so lại.
26. **Shader dựng lại mang sẵn quy ước depth của D3D (reversed Z).** D3D11 ghi depth 1 ở near, 0 ở far; OpenGL ngược lại
    và clip z chạy -1..1. Code game so depth với hằng và blend op đã nướng sẵn: BG_Base lấy `SV_Position.z - customDepth.b`
    để biết tường ở trước nhân vật (cắt lỗ nhìn xuyên), `_CustomDepthBuffer` gộp bằng BlendOp Max/Min, blur và SSAO dựng lại
    vị trí từ depth qua `unity_MatrixInvP`. Trên GL mọi phép so lật: sàn quanh nhân vật bị cắt thành đĩa tối có viền dither,
    tường thành đỏ, blur độ sâu phủ cả màn hình (ảnh mờ như upscale), bóng thành khối chữ nhật. Không phải lỗi của tool chụp
    hay render scale (Review.cs chụp đúng 1280x720, renderScale 1). Cách nhận ra nhanh: chạy cùng test với `-force-vulkan`
    (Vulkan dùng quy ước D3D: reversed Z, UV từ trên xuống) thì ảnh đúng như clip; tắt từng feature/renderer
    (`A4c_StageProbe`, ảnh `captures/review/probe-*`) chỉ ra mesh `SkeletonCircleMaskArea/Msh_MaskArea_Cone` (LightMode
    CustomDepth). Sửa ở bộ sinh: pass nào ghi/đọc custom depth hoặc dựng vị trí từ depth chạy bằng giá trị depth của D3D trên
    GL (docs/SHADERS.md bẫy 16). Bẫy chung: bytecode D3D nướng cả quy ước UV lẫn quy ước depth; sửa UV (bẫy 20) chưa đủ.
27. **Atlas dựng lại phải pack như bản gốc: Rectangle, không xoay.** `SpriteAtlasAsset` mới mặc định tight packing và cho
    xoay. UI `Image` (Simple, `useSpriteMesh` tắt) vẽ cả khung chữ nhật bao sprite trong atlas, không theo đường viền tight
    và không xoay ngược, nên sprite rỗng ruột hay lõm góc hiện luôn sprite khác mà packer nhét vào chỗ trống. Minimap gặp đầu
    tiên sau khi pack atlas (bẫy 21): `MiniMapPanel/Frame/Line` là `circle_256_line_3px` (vòng 258x258) trong `Ui_Common`, và
    khung của nó chứa `circle_128`, chuột, tam giác..., nên trên minimap có một đĩa xám phẳng to cùng các hình trắng
    (`A4d_MinimapProbe` log `mode=Tight texRect=[0,0 258x258]`, `circle_128` ở `[18,58]`). Không phải shader hay RenderTexture:
    tắt Vision/Icon của marker đĩa vẫn còn. Bản gốc thì mọi sprite của cả 29 atlas có `m_RD.settingsRaw` 67 (bit 0 packed,
    bit 1 = 1 Rectangle, bit 2-5 xoay = 0). Bước `spriteatlas` giờ đọc `settingsRaw` của sprite rồi đặt
    `enableTightPacking`/`enableRotation` của importer theo đó. Cách nhận ra nhanh: dump `sprite.packingMode` của Image; ở
    game này Image nào báo `Tight` là sai.
28. **Thân hàm gộp (identical code folding) bị dịch thành rỗng.** IL2CPP link với ICF: các method có mã máy giống hệt nhau
    dùng chung một địa chỉ. Decomp vẫn in một header `// <chữ ký>  @0x<addr>` cho từng method, nhưng thân in bên dưới
    mang nhãn của method mà Ghidra đặt tên cho địa chỉ đó, ví dụ dưới `// Void OnAnimationEnd()  @0x180065E50` là
    `/* Void <OnInputEnabled>b__11_0(InputAction+CallbackContext) */` của `SamplePopupPresenter`. Bản dịch đọc nhãn như
    "method khác" và để thân rỗng. 0x180065E50 là `this.Close(null)` qua vtable, dùng chung cho lambda Esc, `OnCancel`,
    `OnAnimationEnd` của gần trăm popup. Ba chỗ mất: `CampaignStartPopupPresenter.OnAnimationEnd` (event cuối animation
    không đóng thẻ "campaign bắt đầu"), lambda `OnCampaignStarted` của `CampaignPopupPresenter` và lambda
    `OnCampaignVoteCancelled` của `CampaignVotePopupPresenter`. Hậu quả A5 lần 1: chọn campaign 1101 xong hai popup
    (`IsInputActionPopup`) ở lại, input nhân vật bị chặn, nhân vật đứng yên, F vào Elara và buồng lặn không phản hồi.
    Kiểm cả repo: `python3 -I tools/codegen/folded_body_audit.py` (method/lambda rỗng trong khi thân gộp có lời gọi;
    bỏ qua thân gộp vào `UIToolkitTextSource.ModifyMesh`, vốn rỗng). Khi dịch: nhãn dưới header khác tên method nghĩa là
    thân gộp, vẫn phải dịch thân đó cho method này.
29. **`ExposedReference` xuất ra dạng `id: <hash>`, editor không đọc được.** Bản build giữ `PropertyName` bằng hash int, nên
    AssetRipper ghi `exposedName:` kèm dòng con `id: 1162937878`; editor lưu `PropertyName` là chuỗi trơn
    (`exposedName: def55ea2...`) và đọc dạng `id:` thành tên rỗng. ControlTrack của timeline vì thế không tìm được
    `sourceGameObject` và không bật gì: nền chi tiết của Story Campaign (`TL_CampaignDetailBackgroundTimeline_master`, track
    `BG_Sub_Theme_0x`) để mọi diorama theme ở trạng thái tắt, panel chỉ còn sương và vòng tròn xám (`A3b_CampaignBgProbe`
    log track theme không mute nhưng `Prefab_CampaignDetailBackground_sub_Theme_0x on=False`). Nền danh sách chỉ dùng
    AnimationTrack, binding qua `m_SceneBindings`, nên vẫn vẽ. Phía `PlayableDirector` (`m_ExposedReferences.m_References:
    - 1162937878: {fileID: ...}`) đã là khóa chuỗi, nên `tools/rip/exposed_names.py` (rebuild.sh chạy) ghi chính số đó làm
    tên và hai bên khớp lại. Chuỗi gốc đã mất (chỉ hash được ship), chuỗi nào cũng được miễn hai bên giống nhau.
30. **Hằng hex đổi sang thập phân bằng tay bị sai.** Ghidra in hằng ở dạng hex (`AddBuff(..., 0x2dc6c1, ...)`). Bản dịch đổi
    tay và sai một chữ số: `UnitReviveState.OnExit` thêm buff 2934465 (0x2cc6c1) thay vì 3000001 (0x2dc6c1, bất tử 2 s sau
    hồi sinh), `MonsterController` dùng 1987617-9 (0x1e5421..) thay vì buff điểm yếu 2004001-3 (0x1e9421..),
    `InGamePassiveSkillSlotPresenter.Set100003` đọc skill 10000000 thay vì 10012000 (0x98c560). Id không có trong bảng thì
    `TBuff.Table[id]` ném KeyNotFoundException; ở `OnExit` của trạng thái Revive, máy trạng thái không đổi được trạng thái
    nên thợ lặn đứng mãi ở Revive, không đánh, không chết, màn không kết thúc (A5 lần 3). Kiểm cả repo:
    `python3 -I tools/codegen/literal_value_audit.py` (mỗi số thập phân >= 256 trong C# phải có trong decomp của lớp đó,
    dạng hex hoặc thập phân; còn lại là hằng chỉ có ở C#: initializer mảng, `Const`, cờ Win32).
31. **`goto` vào khúc đuôi chung là một `return` sớm, bản dịch làm mất.** Ghidra gộp các nhánh cùng kết thúc bằng một
    lời gọi (thường `SetActive(x, b); return;`) thành một nhãn ở cuối hàm và cho nhánh sớm nhảy vào đó với `b` đã gán sẵn
    (`bVar9 = false; goto LAB_1808aec44;`). Trong `InventoryGoodsSlotPresenter.UpdateLayout`, ô trống ẩn `Normal` rồi
    **thoát**; bản dịch chỉ chép phần `SetActive(false)` rồi chạy tiếp xuống `SetNormalLayout()` và
    `Normal.SetActive(layout < Unrevealed)`, nên mọi ô trống (túi đồ, kho, ô nhanh, Automaton) vẽ lớp `Normal` xám đặc
    (0.153, luma 39) thay vì để lộ nền tối (luma 18), và lớp đó che luôn hình nhẫn/đèn của ô trang bị trống
    (`AccessoryIcon`/`ArtifactIcon` nằm dưới `Normal` trong prefab). Khi dịch một nhánh có `goto LAB_...`, đọc khúc
    đích tới hết: nếu nó kết thúc bằng `return`, nhánh đó cũng phải `return`.
32. **Hình theo thiết bị (InputIcons) chỉ hiện sau khi có một input thật.** `II_DeviceBasedObjectController` bật
    `objectToShow` khi `InputIconsManagerSO.CurrentInputDeviceIsKeyboard()`; `InputIconsManager.asset` để
    `defaultStartDeviceType: 0` (Auto), nên `ShowDeviceIconsBasedOnSettings` không đặt thiết bị nào và thiết bị hiện tại là
    null cho đến khi một InputAction performed (`HandleDeviceChange` -> `RequestDeviceDisplayChange`, trễ 0.2 s). Bản gốc
    cũng vậy; người chơi thật rê chuột ở title nên không thấy. Test batchmode bấm nút bằng `onClick.Invoke()` và
    `ResetAndDisableNonBackgroundDevices` đã tắt chuột/bàn phím, nên mọi thứ gắn controller kiểu KeyboardAndMouse đều ẩn:
    `KeyGuidePanel!` của HUD ("Control Guide O"), nhóm `StaticDecoration/TutorialGuides/Keyboard` của sector 10009 (sàn
    "Move + Look", "Basic Attack"/"Skill"), phím dưới quick slot, chữ "F" cạnh "Next" của dialog (review A4: thiếu ở
    phòng đầu, có lại sau khi test gửi phím W). Không phải lỗi dịch: test phải `Flow.RouteInputToGame()` rồi rê chuột
    (`A4_TutorialFlow.MoveMouseLikeAPlayer`) trước khi chụp, và log `GetCurrentInputDevice()` để biết thiết bị đã có.
    Khi một hình HUD/sàn "lúc có lúc không", xem trước objectToShow của controller này trong prefab.
33. **Ghidra xếp lại các khối của state machine `async`; thứ tự `await` phải đọc theo `__1__state`.** Trong `MoveNext`
    Ghidra đặt nhánh `state == 0` (chỗ tiếp tục sau `await` đầu) lên trước, nên mã ở nhánh `else` (chạy lần đầu, state -1)
    trông như nằm sau. `LoungeScene.OnPlaying` bị dịch ngược: mở Dive Results rồi mới tắt màn đen `LoungeLoadingPanel`;
    bản gốc làm tycoon, tắt màn đen (trừ lúc prologue tutorial, `_isTutorialStarting`), rồi mới `await` popup kết quả. Hậu
    quả: Dive Results hiện trên nền đen thay vì sảnh mờ phía sau (A5 lần 5). Cách đọc: tìm `this->__1__state = N; ...
    AwaitUnsafeOnCompleted; return;` để biết mỗi `await` dừng ở đâu, rồi nối khối tiếp tục `if (state == N)` vào sau nó.
34. **Danh sách "đang dịch" cũ làm bản kiểm initializer bỏ sót.** `tools/codegen/ctor_fields.py` bỏ qua các file trong
    batch W4 còn đang dịch (`PENDING`); batch xong từ lâu mà danh sách vẫn còn, nên ~1.300 file không được kiểm và
    `RewardBoxSpawnData.SlotCount = -1` (cùng `GuestSpawn*.Id = -1`, `SpecialFieldSpawn*.Scale = Vector3.one`,
    `WavePhase.WaveSpawnDatas = new List`, `MonsterSpawnComponent.UseMonsterSpawnTable = true`) bị mất. Thiếu -1 thì rương
    tạo lần đầu có `SlotCount` 0, `Math.Max(0, 1)` = 1: panel ghi "Search Results 3/1" (A5 lần 4). Bộ lọc tạm của một
    công cụ kiểm phải hết hạn cùng việc nó che; chạy lại công cụ sau khi bỏ bộ lọc.
35. **Một ảnh so với clip không đo được độ sáng của cảnh có animation ánh sáng.** Hố xanh ở title "sáng x1.22" so với clip
    (A1 lần 59) mà không có lỗi render: `Anim_LoungeBG` (vòng 20 s, đồng hồ riêng, không theo timeline 39.15 s của camera)
    nháy Point Light_7/_8 nằm trong hố (cường độ 10-18 / 2-20), và khói là particle ngẫu nhiên. Giữ đúng pose của A1, gieo
    seed cho particle rồi quét cả vòng (`A1b_PitProbe.PitOverTime`, pit-run10): hố 106,5-126,7, clip 102-125; ảnh lần 59
    rơi đúng đỉnh nháy (2,06 s). Cùng khung đóng băng, GL 126,6 và Vulkan 123,5 (pit-run8/9), nên cũng không phải quy ước
    D3D. Trước khi sửa shader vì một vùng lệch sáng: tìm mọi curve `m_Intensity`/`m_Color` của đèn và particle trong vùng,
    quét pha của chúng ở pose cố định, rồi so khoảng với các khung clip; ảnh review chụp ở pha đã biết (A1 bỏ qua đỉnh
    1,5-4 s và ghi `lights=` cạnh ảnh).
36. **`DontDestroyOnLoad(this)` ở cuối `Awake` bị bỏ, manager gốc chết khi đổi scene.** Các manager của `RootLifetimeScope`
    đăng ký bằng `RegisterComponentOnNewGameObject` (không `.DontDestroyOnLoad()`), nên GameObject mới nằm trong scene đang
    mở; mỗi manager tự gọi `Object.DontDestroyOnLoad(this)` trong `Awake`. `GameWaveManager.Awake` và
    `GraphicOptionController.Awake` mất lời gọi đó: manager bị huỷ ở lần đổi scene đầu, VContainer vẫn tiêm bản đã chết
    (C# còn, Unity đã huỷ, subscription `AddTo(this)` đã dispose). Hậu quả A5: bật terminal thoát, wave chạy (host gọi thẳng
    vào object chết) nhưng `RxCurrentWave` không bao giờ đổi, HUD "Dive Booth has been summoned!" + đếm ngược không hiện.
    Cách nhận ra: `FindObjectsByType` không thấy manager trong khi presenter còn giữ tham chiếu (instance id âm). Kiểm cả
    repo: mọi lớp có `UnityEngine__Object__DontDestroyOnLoad` trong decomp phải có `DontDestroyOnLoad` trong C#.
37. **Test đi bằng WASD kẹt ở góc trong của NavMesh, không phải lỗi game.** Bàn phím chỉ cho 8 hướng; góc đường đi
    (`NavMesh.CalculatePath`) nằm ngay trên mép vùng đi được (bake đã trừ bán kính agent), nên ở góc trong hướng gần nhất
    có thể vuông góc với mép và `NavMeshAgent.Move` (`UnitView.Move`) không còn gì để trượt theo: A4 lần 26 đứng yên ở
    (26.07, 43.30), góc hành lang đông và phòng phía nam, phím S+A (nam) đâm thẳng vào mép, trigger 100095 không bao giờ
    chạm. Chuột sống không liên quan: nó chỉ đổi hướng nhìn (`Forward`) và tốc độ lùi, lần 25 cũng có chuột và cũng kẹt
    ở chỗ khác. `A4_TutorialFlow.KeysToward` thử 8 hướng theo thứ tự gần nhất và bỏ hướng mà `NavMesh.Raycast` chặn
    trong 0.3 m. Đích nằm trong vật cản (Elara, 100097) tính là tới khi đứng ở điểm đi được gần nhất, trừ khi có cửa
    đóng sát đích (cổng 2005: Go mở cửa). Test cũ vẫn "pass" khi bỏ sót bước: giờ mỗi bước là một `TutorialEvent` mà
    `Campaign/1100.lua` gửi (`SendTutorialEvent("CallCollision_N")`, `TalkToEll_1/2`), thiếu bước nào test đỏ. Radio
    không phải tín hiệu tin được: radio 100098 phát lúc tới cổng rồi tắt trong lúc popup chìa khoá, nên lần 25/26 tưởng
    là trượt.
38. **Shader UI tự glitch theo `_Time.y`; ảnh review chụp trúng khe glitch, không phải lỗi dịch.** Logo title
    (`TitleImage!`, `Mtl_DE_UI_TITLE_Glitch`) có bốn cổng nháy trong shader: `frac(sin(floor(_Time.y*rate)*12.9898+seed)*43758.5)`
    vượt ngưỡng 0,95 thì mở (slice kèm tách RGB, rung đỉnh, warp, block; mỗi cổng mở ~1/20 khe), không có animation hay
    curve nào điều khiển. A1 lần 60 và 61 đều có viền đỏ/xanh lơ 1-2 px (30% điểm sáng của logo) vì batchmode bước thời
    gian đều 1/3 s, nên giờ chụp trùng nhau giữa các lần chạy. URP đặt `_Time.y = Time.time`, nên tính cổng bằng float
    trên CPU khớp GPU: quét 182 khung (`A1c_LogoProbe.LogoOverTime`, logo-run1) thì mọi khung có viền đều có cổng slice
    mở, cổng đóng thì không khung nào có viền (cổng mở mà noise nhỏ thì có thể không viền). Trước khi coi một hiệu ứng UI
    là sai cường độ: tìm cổng thời gian trong shader, quét theo thời gian, rồi chụp review ở lúc mọi cổng đóng (A1 ghi
    `logo t= slice= ...` cạnh ảnh và chờ pose sau nếu cổng mở).
39. **Khung ref trích bằng ffmpeg mặc định ra `.jpg` lệch màu theo độ bão hoà (BT.709 giải như BT.601).** Clip YouTube là
    `yuv420p` gắn nhãn `bt709`; `ffmpeg -ss t -i clip -frames:v 1 x.jpg` chỉ đổi dải tv→pc, giữ nguyên YCbCr, mà JPEG
    được giải bằng ma trận BT.601. Điểm trung tính không lệch, điểm bão hoà lệch G +5..8, R +4..7, B -2 (title-menu,
    pixel bão hoà >75), nên mọi ref màu có B/G thấp hơn thật 1-6% (hố xanh lơ 0,96 thay vì 1,01). Đo màu so với clip
    thì giải lại đúng: `-vf scale=in_color_matrix=bt709:in_range=tv:out_range=pc,format=rgb24` ra `.png`. Cách nhận ra:
    lệch tỉ lệ theo độ bão hoà và không có ở khung xám (npc-character-select, dive-loading).
40. **Phiên bản package khác bản gốc đổi thứ tự callback: Netcode 2.12 reset `LocalClient` trước `OnClientStopped`.** Bản
    build dùng NGO **2.8.1**: so tên kiểu của ảnh `Unity.Netcode.Runtime` trong `out/vd/inspector/types.cs` (TypeDefIndex
    13905-14320) với source từng bản trên registry, 2.8.x khớp nhất (có `CommandLineOptions`, `AnimatorParameterEntry` của
    2.8.0, 2.6/2.7 thiếu; 2.9 thêm một kiểu mà bản build không có); có `NetworkObject.IsSpawnAuthority` (2.8.1, #3850) và
    `NetworkList` còn override `OnSpawned` (2.8.2 đổi thành `InternalOnSpawned`). Đừng đoán phiên bản qua một hai tên:
    lần đầu tra `udk q` không ra `AuthorityMode` nên kết luận nhầm 2.7. `ShutdownInternal` của 2.8.1 gọi
    `OnClientStopped` / `OnServerStopped` rồi mới `ConnectionManager.LocalClient = new NetworkClient()` (disassembly
    `il2cpp_method.py 'NetworkManager.ShutdownInternal'`: hai lời gọi ảo ở +0xc0/+0xb8 trước khi ghi +0x58); repo từng dùng
    2.12.0, nơi #3908 (2.11.0) đảo thứ tự, nên trong mọi handler của `GameChannel.OnStopped` `IsHost` đã là false. Hậu
    quả A7 lần 2-3: Return to Title -> `GameManager.OnGameLeft` -> `CloseScene` -> `GameShopManager.Reset` bỏ qua
    `_trackableProducts.ClearWithDirty()` (chỉ host) nhưng vẫn xoá `_existingShopGroupIds`; Continue, Lucas spawn,
    `OnNpcSpawned` thêm lại sản phẩm 1 và ném "same key has already been added", `NpcObject.OnNetworkSpawn` dừng giữa
    chừng. Code game dịch đúng; gốc lỗi là phiên bản package. Giờ manifest, `packages.py` và `fetch_donors.sh` (tải từ
    registry, editor không kèm bản này) ghim 2.8.1; GUID script của hai bản trùng nhau nên không phải regen. Kiểm API:
    biên dịch `Runtime/` của bản cần thử bằng `tools/harness/csc.sh`, thay `Unity.Netcode.Runtime.dll` trong một bản sao
    `~/vd-rip/harness/ref` rồi chạy harness với `HARNESS_REF=` (SomeNet.Unity, De.Base, De.InGame, De.Scenes: 0 lỗi).
    Khi một lỗi chỉ xảy ra quanh callback của thư viện, so thứ tự trong disassembly của thư viện trong bản build với source
    package trước khi sửa code game; mọi package khác đã được rà theo cùng cách, bảng và cách chứng minh ở docs/PACKAGES.md.
41. **Lời gọi tới thân gộp mang tên một handler khác bị bỏ khi kiểu đối số không khớp.** `MonsterController.OnMonsterPhaseUpEvent`
    trong pseudo-C gọi `UnitController__OnStateChangeEvent_StateChangeEvent_(this, (StateChangeEvent*)unitEvent)` sau
    `SetNextPhase`. `MonsterPhaseUpEvent` không phải `StateChangeEvent`, nên bản dịch bỏ lời gọi; thật ra 0x18065C430 là thân
    gộp (bẫy 28) của `UnitController.TriggerEvent(IUnitEvent)` cùng mười mấy handler `On*Event` (`il2cpp_method.py --addr`
    liệt kê đủ). Mất `TriggerEvent` thì máy trạng thái không thấy sự kiện: boss đổi `TableId` sang pha sau nhưng ở mãi
    trạng thái Dead, thanh boss tắt, không đánh được (A6 lần 3: Hachishakusama 300015 `state=Dead hp=3600` suốt 150 s). Sửa
    xong, lần 4 qua đủ ba pha 300014 -> 300015 -> 300016 (`state=PhaseUp` rồi Idle). Khi pseudo-C ép kiểu đối số sang kiểu
    không liên quan cho một lời gọi, tra địa chỉ hàm: đó gần như chắc là thân gộp, và lời gọi vẫn phải dịch. Đã soát mọi
    nơi gọi 0x18065C430 trong `CharacterController`/`MonsterController`/`UnitController`: chỉ chỗ này thiếu.
42. **Một hàm dùng chung có cùng tên tham số không có nghĩa là cùng đơn vị: `RefreshScroller(1f)` không phải `ReloadData(1f)`.**
    `ChatScrollerPresenter.OnInject` gốc tự dựng lại cell rồi gọi `EnhancedScroller.ReloadData(1.0)`: đối số là
    `scrollPositionFactor` (0 đầu, 1 cuối danh sách). Bản dịch gói lại thành `RefreshScroller(1f)` của lớp cha, mà ở đó
    số này đi vào `SetScrollPositionImmediately`, tức vị trí tính bằng pixel. Chat log trong lặn vì vậy đứng ở mấy dòng
    cũ nhất, dòng mới nằm dưới mặt nạ (A6 lần 3-4: năm dòng đầu không đổi suốt trận boss, thiếu "Wait, something's
    coming!" và "Exploration Record ... Wandering Hachishakusama" mà ref boss-intro/01 có; lần 5 có đủ). Khi thay một
    chuỗi lời gọi trong pseudo-C bằng helper có sẵn, kiểm đơn vị của từng đối số trong helper.
43. **Gọi nhầm helper anh em dựng key Addressables khác tiền tố: asset có đủ, key thì không.** `HitBox.SpawnVfx` gốc gọi
    `Extensions.SpawnSkillVfx` (`"Vfx/Skill/" + vfx + ".prefab"`); bản dịch gọi `SpawnVfx` (`"Vfx/" + vfx`). Tên trong bảng
    HitBox (`1001/1001_01_SwordAttack_Cast_1st`, `Common/Trap_Fire_FireThrower`, ...) vì vậy thành `Vfx/1001/...`, key
    không có trong catalog gốc lẫn của ta, trong khi prefab đã xuất và đăng ký đủ dưới `Vfx/Skill/...` với nhãn Login. Cả
    200 vfx của HitBox không vẽ: nhát chém kiếm, vòng thanh tẩy, lửa bẫy (A5 run1: 59 dòng `Resource prefab(...) not
    found`, 12 key). Không phải lỗi rip hay address map, đừng thêm alias vào catalog. `tools/rip/address_audit.py` so
    từng lớp: helper `Extensions` mà decomp gọi với helper mà C# gọi (lệch thì in ra), rồi dựng lại mọi key từ bảng và
    literal theo cách của bản gốc và của ta, đối chiếu với AssetGroups và catalog gốc. Trước sửa: 1 lớp lệch, 200 key không
    tải được; sau: 0 và 0. Lệch do IL2CPP inline (`GameWave.StartWave` gọi thẳng `InstantiateSpecialField` vì
    `GameObjectManager.SpawnSpecialField` bị inline) ghi vào `INLINED` kèm lý do. A5 vfx-run2 sau sửa: 0 dòng `Resource
    prefab(...) not found`, nhát chém kiếm hiện trong dive-combat.
44. **Batchmode không có cuối frame: `WaitForEndOfFrame` không bao giờ chạy tiếp, MasterAudio không phát BGM.**
    MasterAudio chờ clip Addressable bằng `yield return MasterAudio.EndOfFrameDelay` (một `WaitForEndOfFrame`, đúng như
    decomp của `AudioAddressableOptimizer`). Editor batchmode không có Game view nên không có cuối frame (A8 đo: 0 lần trong
    4.184 frame). Bài nào chưa tải xong ở lần kiểm đầu thì coroutine treo: `PlaylistController` đứng `Stopped` với
    `_addressableCoroutine` còn đó, map `AddressableTasksByAddressableId` rỗng trong khi chính test tải cùng key thì
    `Succeeded` ngay; title, sảnh, màn lặn đều câm (A8 lần 1-3). SFX thì lần phát đầu của mỗi group câm (variation đứng
    `Loading`), lần sau mới nghe vì clip đã nằm trong cache Addressables: nút Option ở title không có ButtonClick, popup
    không có PopUpOpen. Không phải lỗi dịch và bản gốc chạy trong player có cuối frame. A8 đặt `EndOfFrameDelay = null`
    ngay ở SubsystemRegistration: đổi sau khi MasterAudio đã phát âm warming là muộn, Mono đã JIT loader và đọc
    `static readonly` như hằng (lần 6: playlist chạy, SFX đầu vẫn treo). Ghi âm: tap `OnAudioFilterRead` trên AudioListener
    nghe cả nguồn đi qua AudioMixer; `AudioRenderer` khi không đặt `Time.captureFramerate` render ~0 mẫu mỗi frame và đồng hồ
    DSP đứng theo (`dspTime` 53,14 -> 53,18 trong 8 s), đừng dùng.
45. **Batchmode không vẽ gì nên Spine không cập nhật: không có tiếng bước chân.** `SkeletonRenderer.Awake` đặt
    `updateMode = updateWhenInvisible`, prefab nhân vật để `Nothing`, chỉ `OnBecameVisible` trả về `FullUpdate`. Batchmode
    không render giữa các ảnh review (Review.cs), nên skeleton không áp animation, không bắn event `sfx_walk_*`/`sfx_run_*`,
    và `UnitView.OnSpineEvent` không đếm (`_currentFootstepCount` 0 sau 7,3 m, A8 lần 3). Ảnh review chỉ làm skeleton
    "thấy được" trong một frame. A8 render các camera base vào RT 320x180 mỗi frame quanh khoảnh khắc trong màn: 13 event,
    Walk_L/R_Grass phát. Đừng render mỗi frame suốt test: frame chậm làm F giữ ở buồng lặn thiếu 1,5 s game time (lần 4).
    Mọi thứ khác treo vào event Spine (âm thanh, VFX theo event) cũng chỉ kiểm được khi có frame được vẽ.
46. **`}` của Ghidra đóng `if` sớm hơn bản dịch tưởng: lời gọi sau khối chạy cho mọi nhánh.** Nhánh `StageEndEvent` của
    `GameStageManager.<OnEventOccured>g__ApplyExpGain|12_0` (@0x180760720) chỉ bọc phần thưởng thoát
    (`if (IsEscaped) { if (HasEscapeExpReward) ... }`); `StageResultLocalInfo`, `SendToHost(ReqStageResultPlayerLocalInfo)`,
    reset bucket và `FlushAccumulatedExp` nằm sau dấu `}` đó nên chạy cả khi chết. Bản dịch kéo chúng vào trong
    `if (IsEscaped)`: chết thì host giữ `LocalInfos` toàn 0, Dive Results ghi Monsters Defeated/Searched Chests +0
    (review A5 run1: kills=1 boxes=1) và EXP tích luỹ của lượt lặn không được cộng. Sửa xong (res-run3, chết): +250/+15.
    Khi dịch một khối lồng, đếm ngoặc trong pseudo-C tới đúng dòng đóng; lời gọi đặt ngay sau `}` thường là phần chung.
    Hai triệu chứng cùng màn hình không phải lỗi dịch, kiểm trước khi sửa: (a) Lost Items trống khi chết có thể là xúc xắc,
    không phải lỗi: `ApplyStageDead` bỏ từng ô với `GoodsRemovalPercent<Độ khó phòng>` (Junior 50, Story 0; trang bị chỉ từ
    Classic), 3 ô giữ cả là 12,5% (run1); res-run2 mất 4 chồng và mục hiện đủ. Đọc `GameContext.RoomDifficulty` và bảng
    Const trước. (b) Màn hình batchmode là 640x480: ở 4:3 canvas cao hơn nên nội dung (1037) lọt viewport (1156),
    `StartAutoScrollAsync` coi như không tràn và kết thúc ngay ở đầu; `Review.Shot` vẽ lại ở 1280x720 (viewport 436) nên
    ảnh trông như "đang hiện dở". Ở 16:9 (`A5c_ResultProbe`, `PlayModeWindow.SetCustomRenderingResolution(1280, 720)`)
    viewport 796, popup tự cuộn 0,5/s (2 s) xuống đáy rồi mới bật Confirm, khớp clip. Popup nào cuộn/căn theo kích thước
    màn hình phải chụp sau khi đặt độ phân giải của ảnh.
47. **Nhánh `switch` dịch thành rơi xuống nhánh khác: thân hàm cục bộ chỉ còn nửa đầu.** `GameSoundManager.OnStageStateChanged`
    được dịch với nhánh Closed gọi `OnStageClosed()` rồi `break` xuống chung khúc `UnmuteBus` của Playing, và
    `OnStageClosed` (local function `<OnStageStateChanged>g__OnStageClosed|27_0` @0x18072D850, cũng được inline nguyên
    văn trong @0x18072C380) chỉ còn ba lệnh dispose. Bản gốc còn xoá `_overrideBgm`, dừng vòng DangerAlert, đặt lại
    `_clientId`, cờ pause, ambience anomaly và `StopBgm`. Hậu quả: BGM mà Lua đặt (`LuaApi:PlayBgm` -> `PlayOverrideBgm`,
    prologue 1100 đặt "Lounge") sống qua lần đổi màn, và view target cùng client id ở màn sau bỏ qua cập nhật BGM. Sửa ở
    7820d0d8; A8b (đường A4 có ghi âm) nghe City_Ambience ở màn hướng dẫn, BossDarkYoung_Battle ở cutscene, Lounge khi về
    sảnh. Khi một local function có cả bản riêng lẫn bản inline trong decomp, đối chiếu cả hai với bản dịch.
48. **Batchmode chỉ lên được 1280x720 qua `PlayModeWindow.SetCustomRenderingResolution`, và giá là chậm ~10 lần.** Màn hình
    của editor batchmode là 640x480; `-screen-width/-screen-height` và `Screen.SetResolution` đều để nguyên 640x480 (probe
    scr-probe1), chỉ kích thước tuỳ chỉnh của cửa sổ play mode đổi được `Screen` (có hiệu lực ở frame sau). Bật nó cho cả
    lượt test thì mỗi frame chậm hẳn: A1 a1-run64 sau 24 s vẫn ở SplashScene (bình thường cảnh tiêu đề lên ở ~8-10 s),
    a8b-run3 mở menu tiêu đề mất 112 s thay vì ~10 s, kill-run1 kẹt ở sảnh. Đừng đặt nó toàn cục. Chỉ bọc quanh ảnh nào
    bố cục/cuộn theo kích thước màn hình bằng `Flow.AtShotScreen` (đặt 1280x720, chờ `Screen` đổi, xong trả về 640x480):
    A5 a5-run23 bọc Dive Results từ lúc về sảnh tới ảnh `result-screen`, popup mất 55 s mới hiện (ở 640x480: 0,9 s) nhưng
    phần còn lại của lượt chạy giữ tốc độ thường (tiêu đề 10,3 s).
    Trong cửa sổ đó âm thanh cũng câm: clip Addressable tải không kịp (A8 đo: coroutine của variation được 2 frame trong 3 s,
    `LoadAssetAsync` của chính test mới 50%), nên Dive Results không có EscapeResult và nút Confirm không có ButtonClick,
    và variation vẫn kẹt `Loading` sau khi trả về 640x480 (a8-a5-run1..5). Với `VD_NO_SHOT_SCREEN=1` (`AtShotScreen` chạy
    thẳng `body`), a8-a5-run6 nghe EscapeResult lúc popup mở (1,1 s) và ButtonClick lúc Confirm: lỗi của test, không phải
    của game. Kiểm âm thanh thì tắt cửa sổ 1280x720.
49. **Danh sách trạng thái dịch tay rơi mất một phần tử: `IsHitable` thiếu Dying, mỗi nhát sau khi chết lại báo một lần giết.**
    `UnitModel.get_IsHitable` (@0x1806948b0) so lần lượt `cmp byte [state], 4/5/6/0x70/0xd/0xa/8/0x72/0xc` rồi `!= 0xb`:
    Dying, Dead, Revive, PhaseUp, Cutscene, ReservedDespawn, Escaped, Captured, StageReady, khác Spawn. Bản dịch bỏ Dying (4),
    nên quái ở 0 HP đang Dying vẫn qua `HitBox.CheckHitable`; mỗi nhát trúng thêm đi qua `OnHpDamageEvent` (`Hp <= 0` →
    `DieEvent`) → `MonsterController.OnDieEvent` → `NtfMonsterKill` → `MonsterKillEvent`, nên EXP giết bị cộng nhiều lần
    (res-run3: ~7 con chết, Monsters Defeated +250 thay vì ≤70). Sửa xong (kill-run4): 8 con chết, 8 sự kiện giết, +80.
    Còn một lần đôi hợp lệ: passive kích trong `TriggerEvent` của chính nhát đó giết quái trước, rồi kiểm `Hp <= 0` sau
    `TriggerEvent` báo thêm một `DieEvent`; decomp cùng thứ tự nên đó là hành vi gốc. Khi dịch một chuỗi so sánh enum,
    đếm số lệnh `cmp` trong disassembly và đối với số vế trong C# (Ghidra in hằng byte lớn như `0` trong pseudo-C).
    Truy vết: log `new StackTrace().GetFrames().Skip(16)` trong `OnDieEvent` mới thấy được nơi gọi; 8 khung đầu toàn là UniRx.
50. **Nhánh `IsBoss` của `MonsterDeadState.GetNextStateEvent` bị đảo: boss chết biến mất cùng chiến lợi phẩm.** Trong
    pseudo-C, `IsBoss(monsterType)` đúng thì nhảy thẳng tới `LAB_1806b55a9` (trả `None`, ở lại trạng thái Dead), còn quái
    thường mới vào khối kiểm túi đồ rồi `ReservedDespawn` (10). Bản dịch viết `if (IsBoss) return ReservedDespawn;`. Hậu
    quả A6 (1103 "Formless Horizon"): xác Donovan biến mất ngay, mà bức tranh 300011 (nhóm drop 127000/127001) nằm trong
    túi của xác đó, nên điều kiện `TeamItemCountInInventory:300011` không bao giờ đạt, Lua không thả buồng 1007 và màn
    không kết thúc (lần 14-15: không có DropGoods, RewardBox hay xác nào). Sửa xong, lần 16 mở xác, lấy 401103 và 300011,
    buồng 1007 hiện sau 3,5 s và màn kết thúc. Khi một nhánh trong pseudo-C rẽ bằng `if (!cond) {...}` rồi rơi xuống nhãn
    chung, đọc lại xem nhánh `cond` đúng thật sự trả về gì; đừng suy ra từ chỗ mã của nhánh kia đứng gần.
51. **Bảng "theo cấp" tra bằng cấp kế tiếp: `TLevel.Table[level + 1]`, bản dịch tra cấp hiện tại.** Hàng của `TLevel` là cấp
    đích (hàng 2: NeedExp 1500, Coin:0:3). Cả `OnLevelUpAsync` của InGameScene (thoại nâng cấp ở chủ tiệm) lẫn
    `CharacterController.OnLevelUp` phía host đọc `Profile.Level + 1` (`id = iVar + 1` ngay trước `Table.get_Item`), bản dịch
    đọc `Table[level]`: thoại báo "about 0 Otherworldly Coins", điều kiện exp/vàng/coin là của cấp đang có, và host trừ giá
    của hàng hiện tại (cấp 1: Gold 0), nên lên cấp miễn phí. A8 a8-run13 sau sửa: thoại báo 3 coin, LevelUp phát. Hai chỗ
    khác (`InGameLevelPanelPresenter`, `DialogPopupPresenter`) đã đọc `level + 1`; gặp tra bảng theo cấp thì tìm `+ 1` trong
    pseudo-C trước lời gọi `get_Item`.

52. **API riêng của Windows (`user32.dll`, `kernel32.dll`) chạy trên Linux là `DllNotFoundException`.** Bản gốc chỉ ra Windows,
    nên `AppStartUp.MoveWindowToPrimaryMonitor` (gọi ở cuối `Start`), `GraphicSettingService.ApplyWindowBorder` và
    `AppStartUp.ScheduleForceQuit` (`TerminateProcess`) gọi thẳng DLL hệ thống. Trên editor Linux mỗi lần `AppStartUp.Start`
    chạy là một exception: phần còn lại của `Start` không chạy (`MoveWindowToPrimaryMonitor` là lệnh cuối nên ở đây không mất
    gì), nhưng test PlayMode đỏ (reviewer a8-review4-a6: BossCampaign1102/1103/1105 đỏ vì đúng lỗi này) và các test phải bật
    `LogAssert.ignoreFailingMessages`, che luôn lỗi thật. Sửa: chặn theo `Application.platform` (Windows player/editor mới gọi DLL;
    nơi khác bỏ qua chỉnh cửa sổ, còn force-quit dùng `Process.Kill`). Đây là chỗ duy nhất code lệch decomp có chủ ý; mỗi chỗ ghi
    "Port guard" để grep.

53. **Quái boss "trắng phẳng, mờ" ngoài chùm đèn pin không phải lỗi shader: đó là hiệu ứng nháy trúng đòn của bản gốc.** Capture
    `a6b-108-patrol` (Dark Young) và `a6b-204-patrol` hiện thân quái màu xám trắng phẳng, rời rạc kiểu dither. Tra shader
    `shader_DE_Character_Normal` (ps blob 36): `discard` khi `_SightEffectEnable*(v2.w-1) + _Effect + 1 < Bayer4x4`, với `v2.w` là
    độ nhìn thấy từ `_PlaneSightRenderTarget`/`_PlanePointBuffer`. Quái ngoài vùng nhìn thấy bị dither mất hẳn; `_Effect` (nháy
    trúng đòn, `_EffectColor` WhiteAlpha = (1,1,1,0.4)) cộng vào ngưỡng nên làm nó hiện lại, pha 40% trắng lên thân tối: phẳng,
    xám nhạt, mờ. Bản gốc y như vậy: `docs/review/ref/dive-monster-dark/02.jpg` (Wandering Hachishakusama đang nháy, t=2920 của
    W0YYGShicSk). Quái nằm trong vùng nhìn thấy nhưng ngoài chùm đèn vẫn có đủ chi tiết, tối và đục (`01.jpg`, t=2932), và
    capture 108 lần khác của chính ta (boss ở y vị trí, đã aggro) cũng ra Dark Young xanh đen đủ chi tiết. Đừng sửa shader vì cái
    nháy này. Đo trạng thái vật liệu ở runtime bằng `Debug.Log` trong assembly game không ra dòng nào trong log Unity batchmode
    (chỉ log của assembly test hiện); ghi file (`File.AppendAllText`) thì được.

54. **Bản gốc cũng ném "Collection was modified" trong `UnitSkillState.OnUpdate`: đừng "sửa" cho lệch decomp.** Các kỹ năng
    dịch chuyển (21003003/022/051/064 Soul Collector, 21000055, 21000355, 22000033/042 Hatman, 90000004) có node gốc chỉ một
    `ForceUpdateTargetActionEvent` (startTime 0) và node con `ForceSkillTargetUpdateSkillTrigger` (Immediate). `OnUpdate` duyệt
    `_reservedActionEvents` bằng foreach; sự kiện đó → `HandleUnitEvent` → `MessageBroker.Publish` (đồng bộ) →
    `OnForceSkillTargetUpdateEvent` (thân gập ICF vào `TriggerEvent`) → `ProceedNextSkillAction` → `ExecuteSkillAction` →
    `InitSkillAction`, xoá rồi nạp lại chính danh sách đang duyệt (decomp @0x1806CB910; `Clear` inline có `_version++`), nên
    `MoveNext` ném. Cả chuỗi khớp decomp: game bán ra cũng ném mỗi lần Eldris dịch chuyển (bản release không hiện console). Dịch
    chuyển vẫn chạy; frame đó chỉ bỏ qua `RemoveAll` và kiểm tra hết giờ. Đổi sang duyệt bản sao thì khác bản gốc: sau
    `ExecuteSkillAction` thời gian về 0, `RemoveAll` sẽ xoá sự kiện t=0 của node mới trước khi chạy. Giữ nguyên; A6_BossFlow đếm
    riêng exception này (stack có `UnitSkillState.OnUpdate`) thay vì đánh đỏ. Cách tìm ra node gây lỗi: cờ tạm trong
    `InitSkillAction` ghi `Environment.StackTrace` khi đang ở trong vòng foreach (run a6-diag6).

55. **Test đổi platform sang Local "trong bộ nhớ" phải giữ suốt phiên; trả lại Steam ở `[TearDown]` làm hỏng test sau.**
    `Flow.UseLocalPlatform` đã tự trả Steam khi thoát play mode. Lớp test nào gọi thêm `Flow.RestorePlatform()` trong
    `[TearDown]` thì test thứ hai cùng phiên boot lại splash (`ContinueToLounge` → `LoadScene(0)`) bằng Steam:
    `SplashScene.AuthAsync` ghi "[Auth] Authentication failed." (Steam client không chạy), save đi đường Steam, và test nào bắt lỗi
    log sẽ đỏ (A6 a6-story4: 1201/1202 đỏ vì đúng dòng này, còn 1104 chạy trước thì xanh). Lớp nhiều `[UnityTest]` mà mỗi test
    boot lại title: đừng restore platform theo từng test.

56. **`Assert.Pass` trong thân test bỏ qua bộ ghi lỗi; Steam không chạy thì `SteamManager` ghi lỗi như bản gốc.**
    `Flow.GameLog.Run(label, body)` chỉ gọi `AssertClean` sau khi `body` chạy hết. `Assert.Pass` ném `SuccessException` từ
    trong `body`, nên test xanh mà không ai đếm lỗi game (A1, A4 tới `d646d5f4`). Thân test kết thúc bằng log, không bằng
    `Assert.Pass`. Khi Steam client không chạy (máy này), `SteamManager.Awake` ghi "[Steamworks.NET] SteamAPI_Init() failed"
    bằng `Debug.LogError`, đúng như decomp @0x1801FE350 (dòng `LogError(StringLiteral__Steamworks_NET__SteamAPI_Init__)`):
    đó là hàng `KnownShipped` trap 56, không phải lỗi dịch.
