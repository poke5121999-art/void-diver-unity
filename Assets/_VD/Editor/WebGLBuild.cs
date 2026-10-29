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
    // Batchmode: -buildTarget WebGL -executeMethod VD.EditorTools.WebGLBuild.BuildCLI [-vdOut <dir>]
    // Builds the web player into <dir> (default ~/vd-webgl-build): the enabled scenes of the build settings, the
    // Addressables content first, Brotli with the decompression fallback, no splash. The web player has no Steam
    // (STEAMWORKS_NET is a Standalone define), so EnvironmentSettings is switched to the Local platform for the
    // build and switched back afterwards, whatever the result.
    public static class WebGLBuild
    {
        const string EnvironmentSettingsPath = "Assets/AssetBundles/dependencies_assets_scriptable/EnvironmentSettings.asset";
        const int LocalPlatform = 1;
        const long FileLimitBytes = 100_000_000;
        const string StreamingListName = "vd-streaming-files.txt"; // StreamingAssetsFiles.ListName

        public static void BuildCLI()
        {
            string outDir = Arg("-vdOut", Path.Combine(Environment.GetEnvironmentVariable("HOME") ?? ".", "vd-webgl-build"));
            int code = 1;
            int platform = SetPlatform(LocalPlatform);
            try
            {
                code = Build(outDir);
            }
            catch (Exception e)
            {
                Debug.LogError("VD_WEBGL exception: " + e);
            }
            finally
            {
                SetPlatform(platform);
            }
            EditorApplication.Exit(code);
        }

        static int Build(string outDir)
        {
            if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.WebGL)
            {
                Debug.LogError($"VD_WEBGL active build target is {EditorUserBuildSettings.activeBuildTarget}, not WebGL; start Unity with -buildTarget WebGL");
                return 1;
            }
            string[] scenes = EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray();
            if (scenes.Length == 0)
            {
                Debug.LogError("VD_WEBGL no enabled scene in the build settings");
                return 1;
            }

            PlayerSettings.SplashScreen.show = false;
            PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Brotli;
            PlayerSettings.WebGL.decompressionFallback = true;
            PlayerSettings.WebGL.nameFilesAsHashes = false;
            PlayerSettings.SetManagedStrippingLevel(UnityEditor.Build.NamedBuildTarget.WebGL, ManagedStrippingLevel.Minimal);
            SetWebGL("maximumMemorySize", 4096);

            var settings = AddressableAssetSettingsDefaultObject.Settings;
            if (settings == null)
            {
                Debug.LogError("VD_WEBGL no Addressables settings in the project");
                return 1;
            }
            settings.BuildAddressablesWithPlayerBuild = AddressableAssetSettings.PlayerBuildOption.DoNotBuildWithPlayer;
            FitBundlesUnderFileLimit(settings);
            AddressableAssetSettings.CleanPlayerContent();
            AddressableAssetSettings.BuildPlayerContent(out var aa);
            if (!string.IsNullOrEmpty(aa.Error))
            {
                Debug.LogError("VD_WEBGL Addressables build failed: " + aa.Error);
                return 1;
            }
            Debug.Log($"VD_WEBGL addressables built in {aa.Duration:F0}s");

            // Bundle names carry a content hash, so a previous build's bundles would stay next to the new ones.
            // Clear only a folder that holds nothing or a previous web build.
            if (Directory.Exists(outDir) && Directory.EnumerateFileSystemEntries(outDir).Any())
            {
                if (!File.Exists(Path.Combine(outDir, "index.html")))
                {
                    Debug.LogError($"VD_WEBGL {outDir} holds files but no index.html; not clearing a folder that is not a web build");
                    return 1;
                }
                Directory.Delete(outDir, true);
            }
            Directory.CreateDirectory(outDir);
            string list = WriteStreamingList();
            BuildReport report;
            try
            {
                report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
                {
                    scenes = scenes,
                    locationPathName = outDir,
                    target = BuildTarget.WebGL,
                    options = BuildOptions.None,
                });
            }
            finally
            {
                File.Delete(list);
                File.Delete(list + ".meta");
            }
            var s = report.summary;
            Debug.Log($"VD_WEBGL result={s.result} errors={s.totalErrors} warnings={s.totalWarnings} size={s.totalSize / 1048576}MB time={s.totalTime.TotalMinutes:F0}min out={outDir}");
            foreach (var f in Directory.GetFiles(outDir, "*", SearchOption.AllDirectories).OrderByDescending(f => new FileInfo(f).Length).Take(20))
                Debug.Log($"VD_WEBGL file {new FileInfo(f).Length / 1048576.0:F1}MB {f.Substring(outDir.Length)}");
            var over = Directory.GetFiles(outDir, "*", SearchOption.AllDirectories).Where(f => new FileInfo(f).Length > FileLimitBytes).ToArray();
            foreach (var f in over)
                Debug.LogError($"VD_WEBGL file over the {FileLimitBytes / 1000000} MB host limit: {f.Substring(outDir.Length)} {new FileInfo(f).Length / 1048576.0:F1}MB");
            return s.result == BuildResult.Succeeded && over.Length == 0 ? 0 : 1;
        }

        // A static host such as GitHub Pages refuses files over 100 MB. With the shipped grouping two bundles were over
        // (fonts 143 MB, prefab 106 MB, LZ4): the font assets share little, so they get a bundle each; the 2987 prefabs
        // share a lot, so they stay together and use LZMA. Build() then fails on any file still over the limit.
        static void FitBundlesUnderFileLimit(AddressableAssetSettings settings)
        {
            foreach (var group in settings.groups)
            {
                var schema = group == null ? null : group.GetSchema<UnityEditor.AddressableAssets.Settings.GroupSchemas.BundledAssetGroupSchema>();
                if (schema == null) continue;
                if (group.Name == "dependencies_assets_fonts")
                {
                    schema.BundleMode = UnityEditor.AddressableAssets.Settings.GroupSchemas.BundledAssetGroupSchema.BundlePackingMode.PackSeparately;
                    Debug.Log($"VD_WEBGL group {group.Name}: one bundle per entry");
                }
                else if (group.Name == "dependencies_assets_prefab")
                {
                    schema.Compression = UnityEditor.AddressableAssets.Settings.GroupSchemas.BundledAssetGroupSchema.BundleCompressionMode.LZMA;
                    Debug.Log($"VD_WEBGL group {group.Name}: LZMA");
                }
            }
        }

        // The web player cannot list or read StreamingAssets; StreamingAssetsFiles downloads the files this list names.
        static string WriteStreamingList()
        {
            string root = Application.streamingAssetsPath;
            var files = Directory.GetFiles(root, "*", SearchOption.AllDirectories)
                .Select(f => f.Substring(root.Length + 1).Replace('\\', '/'))
                .Where(f => !f.EndsWith(".meta") && !f.StartsWith("aa/") && f != StreamingListName)
                .OrderBy(f => f, StringComparer.Ordinal)
                .ToArray();
            string path = Path.Combine(root, StreamingListName);
            File.WriteAllText(path, string.Join("\n", files) + "\n");
            Debug.Log($"VD_WEBGL {files.Length} StreamingAssets files listed in {StreamingListName}");
            return path;
        }

        // Returns the platform the asset had, so the caller can put it back.
        static int SetPlatform(int platform)
        {
            var asset = AssetDatabase.LoadMainAssetAtPath(EnvironmentSettingsPath);
            if (asset == null)
                throw new FileNotFoundException("VD_WEBGL EnvironmentSettings asset not found", EnvironmentSettingsPath);
            var so = new SerializedObject(asset);
            var prop = so.FindProperty("Platform");
            int was = prop.intValue;
            prop.intValue = platform;
            so.ApplyModifiedPropertiesWithoutUndo();
            AssetDatabase.SaveAssets();
            Debug.Log($"VD_WEBGL EnvironmentSettings.Platform {was} -> {platform}");
            return was;
        }

        // The memory settings moved between Unity versions; set them by name so this compiles on each.
        static void SetWebGL(string property, int value)
        {
            var p = typeof(PlayerSettings.WebGL).GetProperty(property);
            if (p == null)
            {
                Debug.LogWarning($"VD_WEBGL PlayerSettings.WebGL.{property} does not exist in this Unity");
                return;
            }
            p.SetValue(null, value);
        }

        static string Arg(string name, string fallback)
        {
            var args = Environment.GetCommandLineArgs();
            int i = Array.IndexOf(args, name);
            return i >= 0 && i + 1 < args.Length ? args[i + 1] : fallback;
        }
    }
}
