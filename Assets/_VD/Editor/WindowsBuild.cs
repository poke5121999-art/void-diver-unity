using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace VD.EditorTools
{
    // Batchmode, on the copy tools/win-prep.sh writes:
    //   -buildTarget Win64 -executeMethod VD.EditorTools.WindowsBuild.BuildCLI [-vdOut <dir>]
    // Builds the offline Windows 64-bit player (Mono, no Steam) into <dir> (default ~/vd-win-build) as VoidDiver.exe:
    // the Addressables content first, then the enabled scenes of the build settings. Quality is the desktop game's own;
    // nothing of the web player's (low tier, boot scene, streaming pack) goes in. EnvironmentSettings is switched to
    // the Local platform for the build and switched back afterwards, whatever the result.
    public static class WindowsBuild
    {
        const int LocalPlatform = 1;
        public const string ExeName = "VoidDiver.exe";

        public static void BuildCLI()
        {
            string outDir = WebGLBuild.Arg("-vdOut", Path.Combine(Environment.GetEnvironmentVariable("HOME") ?? ".", "vd-win-build"));
            int code = 1;
            int platform = WebGLBuild.SetPlatform(LocalPlatform);
            try
            {
                code = Build(outDir);
            }
            catch (Exception e)
            {
                Debug.LogError("VD_WIN exception: " + e);
            }
            finally
            {
                WebGLBuild.SetPlatform(platform);
            }
            EditorApplication.Exit(code);
        }

        static int Build(string outDir)
        {
            if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.StandaloneWindows64)
            {
                Debug.LogError($"VD_WIN active build target is {EditorUserBuildSettings.activeBuildTarget}, not StandaloneWindows64; start Unity with -buildTarget Win64");
                return 1;
            }
            var standalone = UnityEditor.Build.NamedBuildTarget.Standalone;
            if (PlayerSettings.GetScriptingDefineSymbols(standalone).Contains("STEAMWORKS_NET"))
            {
                Debug.LogError("VD_WIN STEAMWORKS_NET is in the Standalone defines; build from the copy tools/win-prep.sh writes");
                return 1;
            }
            string[] scenes = EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray();
            if (scenes.Length == 0)
            {
                Debug.LogError("VD_WIN no enabled scene in the build settings");
                return 1;
            }
            PlayerSettings.SetScriptingBackend(standalone, ScriptingImplementation.Mono2x);
            PlayerSettings.SplashScreen.show = false;

            var settings = AddressableAssetSettingsDefaultObject.Settings;
            if (settings == null)
            {
                Debug.LogError("VD_WIN no Addressables settings in the project");
                return 1;
            }
            settings.BuildAddressablesWithPlayerBuild = AddressableAssetSettings.PlayerBuildOption.DoNotBuildWithPlayer;
            AddressableAssetSettings.CleanPlayerContent();
            AddressableAssetSettings.BuildPlayerContent(out var aa);
            if (!string.IsNullOrEmpty(aa.Error))
            {
                Debug.LogError("VD_WIN Addressables build failed: " + aa.Error);
                return 1;
            }
            Debug.Log($"VD_WIN addressables built in {aa.Duration:F0}s");

            // Clear only a folder that holds nothing or a previous Windows build.
            if (Directory.Exists(outDir) && Directory.EnumerateFileSystemEntries(outDir).Any())
            {
                if (!File.Exists(Path.Combine(outDir, ExeName)))
                {
                    Debug.LogError($"VD_WIN {outDir} holds files but no {ExeName}; not clearing a folder that is not a Windows build");
                    return 1;
                }
                Directory.Delete(outDir, true);
            }
            Directory.CreateDirectory(outDir);
            BuildReport report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
            {
                scenes = scenes,
                locationPathName = Path.Combine(outDir, ExeName),
                target = BuildTarget.StandaloneWindows64,
                options = BuildOptions.None,
            });
            var s = report.summary;
            Debug.Log($"VD_WIN result={s.result} errors={s.totalErrors} warnings={s.totalWarnings} size={s.totalSize / 1048576}MB time={s.totalTime.TotalMinutes:F0}min backend={PlayerSettings.GetScriptingBackend(standalone)} out={outDir}");
            foreach (var f in Directory.GetFiles(outDir, "*", SearchOption.AllDirectories).OrderByDescending(f => new FileInfo(f).Length).Take(15))
                Debug.Log($"VD_WIN file {new FileInfo(f).Length / 1048576.0:F1}MB {f.Substring(outDir.Length)}");
            // The player writes no crash dumps next to the exe worth shipping; drop the burst debug folder if any.
            foreach (var d in Directory.GetDirectories(outDir, "*_BurstDebugInformation_DoNotShip"))
                Directory.Delete(d, true);
            return s.result == BuildResult.Succeeded ? 0 : 1;
        }
    }
}
