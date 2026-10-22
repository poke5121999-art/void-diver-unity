# Dịch thân method IL2CPP sang C#

Hợp đồng chung cho mọi agent dịch code game. Đọc hết trước khi sửa file nào.

## Đầu vào cho một kiểu `T` của assembly `A`

- Stub: `Assets/Scripts/<A>/**/<T>.cs`. Kiểu, field, property, chữ ký đều thật; thân method rỗng.
- Pseudo-C của Ghidra: `~/unity-decode-kit/out/vd/code/decomp/<A>/<T>.c` và `<T>+<lồng>.c` (state machine async,
  lambda, display class). Bản `out/vd` (xong 2026-10-22 20:45, 4.968 file, gồm cả MasterAudio, Febucci, DOTweenPro, InputIcons) là bản chuẩn, đã sửa lỗi tail call. Chỉ dùng `out/vd-old-run`
  nhưng đối chiếu lời gọi cuối hàm bằng disassembly bên dưới.
- Disassembly có chú thích (string literal, float, field, MethodRef) để kiểm chéo:
  `cd ~/vd-rip && VD_GAME="$HOME/vd-src/VOID DIVER Escape from the Abyss Demo" venv/bin/python -W ignore ~/survivor-web-hub/games/voiddiver/tools/il2cpp_method.py '<T>.<Method>'`
  (`--find REGEX`, `--fields T`, `--strref REGEX`, `--addr 0x...`). Viết `'T.M'`, không kèm tham số; thân dài thì thêm
  `-n 4000` (mặc định cắt ở 400 lệnh). Assembly không có pseudo-C (NemoMapTools...) thì dịch từ disassembly này.
- Tra cứu chung: `cd ~/unity-decode-kit && UDK_DUMP=$PWD/out/vd bin/udk q '<regex>'`, `bin/udk method '<T>.<M>'`, `bin/udk type <T>`.
- Hành vi đã đo của bản web (để hiểu, không chép số): `~/survivor-web-hub/games/voiddiver/docs/*.md`.

## Giữ nguyên

- Đường dẫn file và `.meta` (prefab trỏ tới script qua GUID của `.meta`). Không xoá rồi tạo lại file có `.meta`.
- Namespace, tên kiểu, lớp cha, interface, tên/kiểu/attribute của field (`[SerializeField]`, `[Inject]`, `[Header]`),
  chữ ký member public/protected/internal. Assembly khác biên dịch dựa trên chúng.

## Viết thân

- State machine async (`<M>d__N` lồng trong kiểu + `[AsyncStateMachine]` trên method) → viết lại method thành
  `async UniTask` / `async UniTask<T>` / `async UniTaskVoid` với `await` đúng thứ tự trong `MoveNext`; xoá kiểu lồng
  và attribute. Iterator (`[IteratorStateMachine]`) → `yield return`.
- Lambda (`<>c`, `<>c__DisplayClassN_M`, `<M>b__N_M`) → lambda tại chỗ; local function (`<M>g__Name|N_M`) → local
  function. Xoá kiểu sinh ra sau khi đã gộp.
- Bỏ nhiễu runtime: `il2cpp_init_metadata`, `il2cpp_write_barrier`, `il2cpp_runtime_class_init`, nhánh null-check chỉ để
  ném `NullReferenceException`.
- COMDAT folding: nhiều method trùng thân dùng chung địa chỉ, nên tên hàm được gọi có thể sai (ví dụ ctor delegate hiện
  thành `PhraseRecognitionSystem.ErrorDelegate..ctor`, `Subscribe<CollectionAddEvent<int>>` thay cho kiểu thật). Suy ra
  đúng hàm từ `MethodInfo` truyền vào và kiểu của đối số.
- `FUN_1801...` chưa có tên: tra bằng `il2cpp_method.py --addr`.
- Hằng float trong pseudo-C có thể ở dạng hex/bit; đổi đúng giá trị. String literal lấy đúng nguyên văn.
- Method generic không có thân riêng trong binary (địa chỉ 0x0 trong `methods.tsv`). Thân dùng chung nằm ở bản
  `__Il2CppFullySharedGenericType` (tìm bằng `il2cpp_method.py --find '<T>.<M>'`); nếu không có thì viết từ chữ ký và
  nơi gọi, kèm `TODO(decomp)`.
- Ghidra cắt ngắn string literal và gán nhầm tên lời gọi khi COMDAT gộp thân. Lấy literal đúng bằng
  `il2cpp_method.py '<T>.<M>' | grep "; '"` hoặc `--strref`.
- Stub bỏ mất constructor và giá trị khởi tạo field (`new Subject<>()`, `new List<>()`): đọc lại từ pseudo-C của `.ctor`
  (và `.cctor` cho static).
- Hàm async bên ngoài chỉ dựng state machine (nhiễu); logic thật nằm trong `…d__N.MoveNext`.
- Không bịa hành vi. Chỗ không đọc ra được: viết phần chắc chắn và để `// TODO(decomp): <điều chưa rõ> @0x<địa chỉ>`.

## Kiểm

- `tools/harness/test.sh <A>` phải in `BUILD <A> errors=0 ok=1` (biên dịch nguyên assembly từ nguồn, các assembly khác
  là bản Unity đã biên dịch). Lệnh mất vài giây; chạy sau mỗi vài kiểu. Mỗi lần chạy có thư mục riêng nên chạy song song được.
- Làm trong git worktree được giao (đường dẫn trong brief), commit vào nhánh của worktree đó khi BUILD sạch; coordinator merge.
- Assembly-CSharp không có asmdef nên thấy mọi assembly trong `ref/asm`. Asmdef thiếu reference hiện ra dưới dạng lỗi
  "type ... is defined in an assembly that is not referenced": thêm reference vào asmdef khi code gốc chứng minh có dùng,
  không tạo vòng giữa các assembly game.
- Logic thuần (tính toán, dữ liệu) có thể thêm test ở `tools/harness/tests/` với giá trị kỳ vọng cụ thể.
- Chạy thật trong Unity là việc của coordinator (`tools/unity-run.sh`, đi qua hàng đợi `~/handoff/heavy.sh`).

## Báo cáo

Số kiểu đã dịch / tổng, số `TODO(decomp)`, kết quả `BUILD`, kiểu nào bỏ dở và vì sao.

## Seen in later batches
- COMDAT folding: identical lambdas and methods share one body, so the pseudo-C may show a body under another type's name. Confirm the address with `il2cpp_method.py` before trusting the file it sits in.
- A nested state machine's `MoveNext` is found as `Type.<Method>d__N.MoveNext`; run `il2cpp_method.py --find` first to get the exact name.
- Stubs drop `using System;` and the initialisers of ordinary fields (`new List<>()`, `new Subject<>()`, `ReactiveProperty`); restore them from the constructor's pseudo-C.
- View property stubs return `null` and stub constants are placeholders (popups really return `IsInputActionPopup => true`, `OpenSfx => "InventoryPopupOpen"`); map each to its field or literal.
- `Close()` and `Close(false)` look the same in pseudo-C; the disassembly shows the default argument.
- The decode-kit sometimes regenerates its dump; while `out/vd/code/decomp` is missing, read `out/vd/code.old/decomp`.
