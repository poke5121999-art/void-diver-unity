"""Which exported stub assemblies get replaced by real package source, and from where.

DONOR is ~/vd-rip/donor (see tools/rip/fetch_donors.sh). Each SWAP entry maps a stub assembly
folder under Assets/Scripts to donor directories whose .cs files define the same types.
"""

UNITY_REGISTRY = {
    'com.unity.ugui': '2.0.0',
    'com.unity.render-pipelines.universal': '17.3.0',
    'com.unity.render-pipelines.core': '17.3.0',
    'com.unity.timeline': '1.8.12',
    'com.unity.inputsystem': '1.19.0',
    'com.unity.cinemachine': '2.10.7',
    'com.unity.ai.navigation': '2.0.13',
    'com.unity.2d.animation': '13.0.5',
    'com.unity.2d.spriteshape': '13.0.0',
    'com.unity.addressables': '2.9.1',
    'com.unity.burst': '1.8.29',
    'com.unity.collections': '2.6.6',
    'com.unity.mathematics': '1.3.3',
    'com.unity.netcode.gameobjects': '2.12.0',
    'com.unity.transport': '2.7.3',
    'com.unity.nuget.newtonsoft-json': '3.2.2',
}

# Packages copied into Packages/ (embedded) because they are not on the Unity registry.
EMBEDDED = {
    'com.esotericsoftware.spine.spine-unity': 'spine-runtimes/spine-unity/Assets/Spine',
    'com.esotericsoftware.spine.timeline': 'spine-runtimes/spine-unity/Modules/com.esotericsoftware.spine.timeline',
    'com.esotericsoftware.spine.spine-csharp': 'spine-runtimes/spine-csharp/src',
}

SWAP = {
    'UnityEngine.UI': ['com.unity.ugui'],
    'Unity.TextMeshPro': ['com.unity.ugui'],
    'Unity.InternalAPIEngineBridge.004': ['com.unity.ugui'],
    'Unity.AI.Navigation': ['com.unity.ai.navigation'],
    'Unity.RenderPipelines.Universal.Runtime': ['com.unity.render-pipelines.universal'],
    'Unity.RenderPipelines.Universal.2D.Runtime': ['com.unity.render-pipelines.universal'],
    'Unity.RenderPipeline.Universal.ShaderLibrary': ['com.unity.render-pipelines.universal'],
    'Unity.RenderPipelines.Core.Runtime': ['com.unity.render-pipelines.core'],
    'Unity.RenderPipelines.Core.Runtime.Shared': ['com.unity.render-pipelines.core'],
    'Unity.RenderPipelines.GPUDriven.Runtime': ['com.unity.render-pipelines.core'],
    'Unity.UnifiedRayTracing.Runtime': ['com.unity.render-pipelines.core'],
    'Unity.Timeline': ['com.unity.timeline'],
    'Unity.InputSystem': ['com.unity.inputsystem'],
    'Unity.InputSystem.ForUI': ['com.unity.inputsystem'],
    'Cinemachine': ['com.unity.cinemachine'],
    'Unity.2D.Animation.Runtime': ['com.unity.2d.animation'],
    'Unity.2D.Common.Runtime': ['com.unity.2d.common'],
    'Unity.2D.SpriteShape.Runtime': ['com.unity.2d.spriteshape'],
    'Unity.Addressables': ['com.unity.addressables'],
    'Unity.ResourceManager': ['com.unity.addressables'],
    'Unity.Burst': ['com.unity.burst'],
    'Unity.Burst.Unsafe': ['com.unity.burst'],
    'Unity.Collections': ['com.unity.collections'],
    'Unity.Collections.LowLevel.ILSupport': ['com.unity.collections'],
    'Unity.Mathematics': ['com.unity.mathematics'],
    'Unity.Netcode.Runtime': ['com.unity.netcode.gameobjects'],
    'Unity.Networking.Transport': ['com.unity.transport'],
    'Newtonsoft.Json': ['com.unity.nuget.newtonsoft-json'],
    'spine-csharp': ['spine-runtimes/spine-csharp/src'],
    'spine-unity': ['spine-runtimes/spine-unity/Assets/Spine'],
    'spine-timeline': ['spine-runtimes/spine-unity/Modules/com.esotericsoftware.spine.timeline'],
}

# Stub assemblies that the engine or the .NET profile already provides, or IL2CPP-only artifacts.
DROP = [
    'UnityEngine.AdaptivePerformanceModule',
    'UnityEngine.UnityConsentModule',
    'UnityEngine.RenderAs2DModule',
    'Unity.InternalAPIEngineBridge.001',
    '__Generated',
    'Sirenix.Serialization.AOTGenerated',
]

# Shader name prefixes served by the donor packages above.
SHADER_DONORS = ['com.unity.render-pipelines.universal', 'com.unity.render-pipelines.core', 'com.unity.ugui',
                 'com.unity.shadergraph', 'spine-runtimes/spine-unity/Assets/Spine',
                 'tmp-essentials/Assets/TextMesh Pro/Shaders']

# Stub assemblies replaced by the real NuGet DLL (no asset references them, only code).
# (package id, version) per stub folder; the DLL is copied to Assets/Plugins/NuGet.
NUGET = {
    'Serilog': ('Serilog', '4.2.0'),
    'Serilog.Sinks.File': ('Serilog.Sinks.File', '6.0.0'),
    'protobuf-net': ('protobuf-net', '3.2.30'),
    'protobuf-net.Core': ('protobuf-net.Core', '3.2.30'),
    'System.Collections.Immutable': ('System.Collections.Immutable', '7.0.0'),
    'System.Diagnostics.DiagnosticSource': ('System.Diagnostics.DiagnosticSource', '9.0.1'),
    'System.Threading.Channels': ('System.Threading.Channels', '9.0.1'),
    'System.Runtime.CompilerServices.Unsafe': ('System.Runtime.CompilerServices.Unsafe', '6.0.0'),
}

# Stub sources that AssetRipper decompiles into code that does not compile; (path under Assets/Scripts, old, new).
PATCHES = [
    ('ACTk.Runtime/CodeStage/AntiCheat/Utils/HashUtils.cs', 'using UnityEngine;\n',
     'using UnityEngine;\nusing Quaternion = UnityEngine.Quaternion;\nusing Vector2 = UnityEngine.Vector2;\nusing Vector3 = UnityEngine.Vector3;\n'),
    ('ParticleImage/AssetKits/ParticleImage/ParticleImage.cs', 'using UnityEngine.UI;\n',
     'using UnityEngine.UI;\nusing PlayMode = AssetKits.ParticleImage.Enumerations.PlayMode;\n'),
    ('UniRx/UniRx/SubjectExtensions.cs', 'public AnonymousSubject(IObserver<T> observer, IObservable<T> observable)\n',
     'public AnonymousSubject(IObserver<T> observer, IObservable<T> observable) : base(observer, observable)\n'),
]

# Donor folders copied into the project as-is (with their GUIDs), donor path -> project path.
VENDOR = {
    'tmp-essentials/Assets/TextMesh Pro/Shaders': 'Assets/TextMesh Pro/Shaders',
}
