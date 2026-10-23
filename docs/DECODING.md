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
