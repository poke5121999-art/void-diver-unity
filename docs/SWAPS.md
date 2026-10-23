# Thay stub bằng mã nguồn mở thật (W4b)

Làm theo cách W2 (UniTask/VContainer/UniRx): `tools/rip/fetch_donors.sh` lấy donor về `~/vd-rip/donor`,
`tools/rip/packages.py` khai báo SWAP/EMBEDDED/OVERLAY, `build_project.py` đổi GUID script theo namespace+lớp.

## Steamworks.NET (`com.rlabrecque.steamworks.net`)
- Bản game dùng: **2024.8.0** (SDK 1.60, steam_api 08.97.99.70). Bằng chứng: hằng `Steamworks.Version` trong
  stub và `types.cs`, chuỗi `08.97.99.70` trong `global-metadata.dat`. Git tag `2024.8.0` của repo gốc (MIT).
- Donor: thư mục UPM `com.rlabrecque.steamworks.net` của tag đó, nhúng vào `Packages/` (EMBEDDED) và SWAP
  vào stub cùng tên. Không có asset nào của game tham chiếu GUID script Steamworks.
- API công khai: giống stub, chỉ khác 24 dòng là delegate nội bộ `ISteamMatchmaking*Response.Internal*`
  (bản dump IL2CPP có thêm tham số `IntPtr this`). Game không đụng vào.
- Lưu ý khi biên dịch ngoài Unity: cần define `UNITY_STANDALONE_WIN`, `UNITY_STANDALONE`, `UNITY_2017_1_OR_NEWER`.

## Odin Serializer (`Sirenix.Serialization`)
- Repo `TeamSirenix/odin-serializer` không có tag nào sau 1.1.1.0 (2018). Game không có
  `DictionaryComparerUtility`, `FloatingPointFormatting`, `TypeExtensions.SubstituteGenericParameters`... tức là
  trước loạt commit 2026-10-14. Ghim commit **ba19025** (2026-07-21, trước loạt đó).
- Namespace của repo là `OdinSerializer`, của game là `Sirenix.Serialization`, nên `fetch_donors.sh` sinh package
  `odin-sirenix` (đổi namespace, đổi tên assembly trong chuỗi `"OdinSerializer.AOTGenerated"`, v.v.).
  6 lớp `Serialized*` đặt vào namespace `Sirenix.OdinInspector` như game. Bỏ các file đã nằm trong stub Config
  (logger, DataFormat, policy), file chỉ chạy trong editor (AOTSupportScanner...) và AssemblyInfo.
  Mỗi file có `.meta` với GUID cố định (md5 của đường dẫn) để GUID của 3 stub có meta được ánh xạ.
- Không có `serializationData` trong YAML nào dưới `Assets/` (đã grep cả export lẫn repo) và game không dùng
  `[OdinSerialize]`/`SerializedMonoBehaviour`. Chỉ `Resources/sirenix/GlobalSerializationConfig.asset` tham chiếu
  một script Sirenix (lớp stub `GlobalSerializationConfig`, còn nguyên GUID).
- So API công khai (reflection, bản stub biên dịch vs bản mới): không mất thành viên nào, thêm 341 dòng do bản mở
  để `public` các tiện ích mà bản đóng để `internal` (Sirenix.Serialization.Utilities).

## Sirenix.Utilities và Sirenix.Serialization.Config (một phần)
- `Sirenix.Utilities` là Odin Inspector, đóng. Repo mở có bản sao công khai của 21 file (EmitUtilities, ImmutableList,
  Flags, MemberAlias*, UnsafeUtilities, LinqExtensions, StringExtensions...) trong namespace `OdinSerializer.Utilities`.
  Chúng đè lên file stub cùng tên (OVERLAY), file stub `IImmutableList`, `ValueGetter`, `ValueSetter`, `WeakValueGetter`,
  `WeakValueSetter` bị xóa vì lớp đó nằm trong file đã đè.
- Còn stub (thân rỗng): `GlobalConfig<T>`, `GlobalConfigUtility`, `ProjectPathFinder`, `GUILayoutOptions`, `ColorExtensions`,
  `ArrayUtilities`, `MathUtilities`, `ListExtensions`, `StringUtilities`, `DeepReflection`... (không có trong bản mở, game không gọi).
- `LinqExtensions` bản mở thiếu 31 hàm của bản đóng. `fetch_donors.sh` thêm lại bằng ngữ nghĩa đã biết của Odin
  (đây là phần dựng lại, không phải mã gốc): `IsNullOrEmpty`, `AddRange`, `Populate`, `Sort`, `ToImmutableList`,
  `Examine`, `Convert`, `FilterCast`, `AppendWith/If`, `PrependWith/If`. Game gọi `ForEach` (stub trả `null`, nên mọi
  `ForEach` trong gameplay đang là no-op) và `IsNullOrEmpty`; cả hai chạy thật sau khi thay.
- Còn thiếu 18 thành viên, game không gọi: `PathUtilities` (6: CanMakeRelative, TryMakeRelative, MakeRelative,
  FindParentDirectoryWithName, Combine, GetDirectoryName), `StringExtensions` (3: Contains(StringComparison),
  CalculateLevenshteinDistance, SplitPascalCase), `TypeExtensions` (8: HasDefaultConstructor, InstantiateDefault,
  FindIdealConstructor, IsCSharpKeyword, ...), `MemberInfoExtensions.SignaturesAreEqual`.
  `TypeExtensions.TypeNameAlternatives` đổi kiểu `Dictionary<Type,string>` -> `Dictionary<string,string>`.
- `Sirenix.Serialization.Config`: chỉ đè 6 file thường (CustomLogger, DataFormat, DefaultLoggers, ErrorHandlingPolicy,
  ILogger, LoggingPolicy). `GlobalSerializationConfig` giữ stub vì là ScriptableObject dựa trên `GlobalConfig<T>` đóng
  (asset đang tham chiếu GUID của nó). Hệ quả: `GlobalSerializationConfig.Instance` và `HasInstanceLoaded` vẫn là stub;
  chỉ ảnh hưởng nếu game có dữ liệu Odin (không có).

## Kiểm
Xem báo cáo trong phiên: `check_refs` giữ 148 dangling / 223 reference; biên dịch Sirenix.*, Steamworks và các assembly
phụ thuộc (De.*, NemoLib, InputIcons, Global) bằng `tools/harness/test.sh` với `HARNESS_REF` trỏ vào bản DLL mới.
Chưa chạy Unity. Serialize/deserialize thử bằng DLL mới (object lồng, mảng, float; Binary và JSON) khớp; `List<T>`/
`Dictionary` lỗi "Was not in array" cả với mã upstream chưa đổi tên khi chạy trên .NET 6 của harness, nên đó là giới hạn
của môi trường thử, cần thử lại trong Unity (Mono).
