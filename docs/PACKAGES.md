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
