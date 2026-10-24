# Unity package versions: ours vs the shipped build

Every package should run at the version the shipped build used. A newer package can change the order of its
callbacks under the same game code (DECODING.md trap 40: Netcode 2.12 vs 2.8.1). This page records what the build's own
files prove about each package and what the project pins.

## How a version is proven

1. **Version stamped in the build.** Some packages leave their version in the player: `Unity.InputSystem` carries its
   `kAssemblyVersion` as the assembly version (`// Image 7: ... Version=1.17.0.0` in `out/vd/inspector/types.cs`), and
   Addressables writes `m_AddressablesVersion` into `StreamingAssets/aa/settings.json`.
2. **Type and member diff against registry releases.** `tools/rip/pkg_diff.py` lists, for each pair of consecutive
   releases, the members and string literals (12+ chars) one adds or removes, and which of them the build has
   (`types.cs` for the assembly's type range, `out/vd/code/strings.tsv` for literals). If the build has what release N+1
   added, it is at least N+1. If it has what N+1 removed, it is at most N. IL2CPP strips unused code, so a missing
   member proves nothing. When no token separates two releases, the version is a range.
3. **API check before pinning.** `tools/rip/asm_build.py` compiles the release's runtime asmdef with
   `tools/harness/csc.sh`. Those DLLs replace the snapshot's in a copy of `~/vd-rip/harness/ref` (`HARNESS_REF=`), and
   every game assembly is built against them. Each must give the same result as against the current packages.

Release tarballs: `https://download.packages.unity.com/<name>/-/<name>-<version>.tgz`; metadata:
`https://packages.unity.com/<name>`. Keep the cache on the home disk (`~/vd-rip/pkgaudit`), not in /tmp: a few
releases of Burst or Input System fill the /tmp quota.

## Table

| Package | Ours (before) | Shipped | Evidence | Action |
|---|---|---|---|---|
| com.unity.netcode.gameobjects | 2.12.0 | **2.8.1** | Type set matches 2.8.x; `NetworkObject.IsSpawnAuthority` (2.8.1); `NetworkList.OnSpawned` override (renamed in 2.8.2) | Pinned 2.8.1 (trap 40) |
| com.unity.inputsystem | 1.19.0 | **1.17.0** | Assembly version 1.17.0.0 = `kAssemblyVersion`; diff: has 1.17.0's `TypeTable.m_Manager` | Pinned 1.17.0 |
| com.unity.addressables | 2.9.1 | **2.8.1** | `StreamingAssets/aa/settings.json`: `"m_AddressablesVersion":"2.8.1"` | Pinned 2.8.1 |
| com.unity.timeline | 1.8.12 | **1.8.10** | Has 1.8.10's `ObjectId` members (`EntityId`, `CompareTo`...); has `ObjectId.DefaultId`, which 1.8.11 removed | Pinned 1.8.10 |
| com.unity.transport | 2.7.3 | **2.6.0** | Has 41 of 2.6.0's additions (`NetworkSimulatorParameter.RandomSeed`...); has 62 members 2.7.0 removed (`BottomLayer.m_ConnectionLists`...) | Pinned 2.6.0 |
| com.unity.collections | 2.6.6 | 2.5.7 to 2.6.2 | Has 2.5.7's `UnsafeQueueData.m_BlockSize`; has `HashMapHelper.kMinimumCapacity`, removed in 2.6.3; nothing separates 2.5.7 from 2.6.2 | Pinned 2.6.2 (newest of the range; assumption) |
| com.unity.2d.animation | 13.0.5 | 13.0.1 to 13.0.4 | Has 13.0.1's `BoneTransformsChangeDetectionJob`; has `Profiling.DeactivateDeformableBuffers`, removed in 13.0.5; 13.0.1 to 13.0.4 identical by tokens | Pinned 13.0.4 (newest of the range; assumption) |
| com.unity.2d.common | 12.0.3 (dependency) | 12.0.1 or later | Has 12.0.1's `Int3Compare` methods; 12.0.1 to 12.0.4 identical by tokens | Kept (inside the range) |
| com.unity.2d.spriteshape | 13.0.0 | 12.0.2 or 13.0.0 | No token separates them; 13.0.0 is the release for 6000.3 | Kept |
| com.unity.cinemachine | 2.10.7 | 2.10.2 or later | Has 2.10.2's `CinemachineConfiner2D.m_Padding`; 2.10.2 to 2.10.7 identical by tokens | Kept (inside the range; not provable) |
| com.unity.ai.navigation | 2.0.13 | 2.0.9 or later | Has 2.0.9's `NavMeshModifier.s_NavMeshModifiersSet`; 2.0.9 to 2.0.14 identical, 2.0.15+ additions absent (may be stripped) | Kept (inside the range; not provable) |
| com.unity.mathematics | 1.3.3 | 1.3.1 to 1.3.3 | No token separates them | Kept |
| com.unity.burst | 1.8.29 | not provable | Player's `Unity.Burst` API is the same across 1.8.x; `lib_burst_generated.dll` has no version string | Kept (editor-bundled) |
| com.unity.nuget.newtonsoft-json | 3.2.2 | not provable | Every 3.2.x ships Newtonsoft.Json 13.0.x with assembly version 13.0.0.0 | Kept |
| com.unity.scriptablebuildpipeline | 2.6.1 (dependency) | editor only | Not in the player | Follows Addressables' dependency |
| com.unity.render-pipelines.universal / core | 17.3.0 | 17.3.0 | Editor-bound: versioned with the editor, contents follow the editor build | Editor-bound |
| com.unity.ugui (uGUI + TextMeshPro) | 2.0.0 | 2.0.0 | Built-in package, contents follow the editor build | Editor-bound |
| Unity editor | 6000.3.18f1 | 6000.3.5f2 | `data.unity3d` / docs/DECODING.md | Editor-bound (built-in packages and engine differ by patch; not changed here) |

Script GUIDs of every pinned release match the donor's (`.cs.meta` compared), so no regen is needed after a pin.

## Embedded third-party packages

Same method (`tools/rip/pkg_diff.py` over git tags/commits; `asm_build.py` builds each candidate with 0 errors).

| Package | Ours (donor) | Shipped | Evidence | Action |
|---|---|---|---|---|
| DOTween (DLL) | 1.3.045 | **1.2.790** | `DOTween.Version` literal `1.2.790` in the build; `DOTween_1_2_790.zip` still on demigiant.com | Pinned 1.2.790 (DLL and modules) |
| DOTweenPro | build's own | 1.0.386 | Translated from the build (`Version` literal 1.0.386) | Same by construction |
| VContainer | HEAD (after 1.19.0) | **1.16.5** | Has 1.16.5's `PrefabDirtyScope`; has `BuilderCallbackDisposable.callback` and the "Type was not dound" literal, removed in 1.16.6 | Pinned 1.16.5 |
| UniTask | HEAD (after 2.5.11) | 2.5.0 to 2.5.1 | Has 2.5.0's `_Timer.cancellationToken`...; has `AsyncOperationHandleConfiguredSource.continuationAction`, removed in 2.5.2 | Pinned 2.5.1 (newest of the range; assumption) |
| SoftMaskForUGUI | 3.6.5 | 3.3.0 to 3.3.4 | Has 3.3.0's `GraphicDuplicator.CopyMesh`...; has 15 members removed in 3.4.0 (`UISoftMaskProjectSettings.m_ShaderVariantRegistry`...) | Pinned 3.3.4 (newest of the range; assumption); the TMP sample in Assets/Samples comes from it too |
| Spine 4.2 runtimes | e7dc143 (spine-unity 4.2.120) | bf8f1f69b to 659e043f1 (spine-unity 4.2.87 to 4.2.106) | Has `ClipInfos.clipResolvedWeights` (bf8f1f69b); lacks the serialized field `BoneFollower.followAttachmentZSpacing` (added 57844ebe1, 4.2.108; fields of kept types are not stripped) | Pinned 659e043f1 (newest of the range; assumption) |
| UniRx | master (2024) | 7.0.0 to master | No token separates them | Kept |
| Steamworks.NET | 2024.8.0 | 2024.8.0 | `Version.cs` (fetch_donors.sh) | Kept |
| Odin Serializer | ba19025 | before the 2026-10-14 batch | fetch_donors.sh note | Kept |
| Serilog | 4.2.0 | 4.2.x | Assembly version 4.2.0.0 (Serilog stamps major.minor) | Kept |
| Serilog.Sinks.File | 6.0.0 | 6.0.x | Assembly version 6.0.0.0 | Kept |
| System.Collections.Immutable | 7.0.0 | 7.0.x | Assembly version 7.0.0.0 | Kept |
| System.Diagnostics.DiagnosticSource / Threading.Channels | 9.0.1 | 9.0.x | Assembly version 9.0.0.0; patch not provable | Kept |
| protobuf-net / .Core | 3.2.30 | 3.x | Assembly version 3.0.0.0 for every 3.x | Kept (not provable) |
| MoonSharp | NuGet 2.0.0.0 | built from source (assembly `MoonSharp`, version 0.0.0.0) | No version literal left | Kept (not provable) |

How the pins were applied: `fetch_donors.sh` checks out the tags (`SPINE_REV`, the DOTween 1.2.790 zip); inside the heavy
lock the live project got the same folders `build_project.py` copies (`Packages/<embedded>`, the SoftMask TMP sample,
`Assets/Plugins/DOTween/DOTween.dll` with its .meta kept, the DOTween modules in `Assets/Scripts/DOTweenAssembly`). Every
GUID that the pinned trees share with the old ones is equal; files only in the old trees (8 scripts) are referenced by no
asset. Harness: with the pinned libraries every game assembly builds as before; DOTweenAssembly needs the netfx
`System.Core` facade in the harness (DOTween 1.2.790 targets .NET 3.5 and uses `Action<,,,>`; Unity has the facade).
