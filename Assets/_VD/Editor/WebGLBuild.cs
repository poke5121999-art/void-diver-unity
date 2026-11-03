using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
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
        const string BootScenePath = "Assets/_VD/WebBoot.unity";
        const string StreamingListName = "vd-streaming-files.txt"; // StreamingAssetsFiles.ListName
        const string StreamingPackName = "vd-streaming-pack.txt"; // StreamingAssetsFiles.PackName; .txt: see WebBoot.CompressibleSuffix
        const string VideoSourceDir = "Assets/AssetBundles/dependencies_assets_video";
        const string FontDir = "Assets/AssetBundles/dependencies_assets_fonts";

        public static void BuildCLI()
        {
            string outDir = Arg("-vdOut", Path.Combine(Environment.GetEnvironmentVariable("HOME") ?? ".", "vd-webgl-build"));
            int code = 1;
            int platform = SetPlatform(LocalPlatform);
            // -vdLogLevel <n>: a diagnostic build lets the game's logger through from level n (Serilog: 1 Debug, 2
            // Information); the shipped value (3, Warning) keeps the browser console quiet.
            string logLevel = Arg("-vdLogLevel", null);
            int unityLogLevel = logLevel == null ? -1 : SetEnvironment("UnityLogLevel", int.Parse(logLevel));
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
                if (unityLogLevel >= 0)
                    SetEnvironment("UnityLogLevel", unityLogLevel);
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
            CutWebDeadWeight(settings);
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
            string boot = CreateBootScene();
            BuildReport report;
            try
            {
                report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
                {
                    scenes = new[] { boot }.Concat(scenes).ToArray(),
                    locationPathName = outDir,
                    target = BuildTarget.WebGL,
                    options = BuildOptions.None,
                });
            }
            finally
            {
                File.Delete(list);
                File.Delete(list + ".meta");
                AssetDatabase.DeleteAsset(boot);
            }
            var s = report.summary;
            if (s.result == BuildResult.Succeeded)
            {
                AddLoadingScreen(outDir);
                AddLogCollector(outDir);
                PackStreamingFiles(outDir);
                NameBundlesAsText(outDir);
                AddCacheControl(outDir);
            }
            // The web player cannot play a VideoClip from a bundle; VideoPlayerHandler plays StreamingAssets/video/<clip>.webm.
            if (s.result == BuildResult.Succeeded)
            {
                string videoDir = Path.Combine(outDir, "StreamingAssets", "video");
                Directory.CreateDirectory(videoDir);
                foreach (var f in Directory.GetFiles(VideoSourceDir, "*.webm"))
                    File.Copy(f, Path.Combine(videoDir, Path.GetFileName(f)), true);
                Debug.Log($"VD_WEBGL {Directory.GetFiles(videoDir).Length} videos copied to {videoDir}");
            }
            Debug.Log($"VD_WEBGL result={s.result} errors={s.totalErrors} warnings={s.totalWarnings} size={s.totalSize / 1048576}MB time={s.totalTime.TotalMinutes:F0}min out={outDir}");
            foreach (var f in Directory.GetFiles(outDir, "*", SearchOption.AllDirectories).OrderByDescending(f => new FileInfo(f).Length).Take(20))
                Debug.Log($"VD_WEBGL file {new FileInfo(f).Length / 1048576.0:F1}MB {f.Substring(outDir.Length)}");
            var over = Directory.GetFiles(outDir, "*", SearchOption.AllDirectories).Where(f => new FileInfo(f).Length > FileLimitBytes).ToArray();
            foreach (var f in over)
                Debug.LogError($"VD_WEBGL file over the {FileLimitBytes / 1000000} MB host limit: {f.Substring(outDir.Length)} {new FileInfo(f).Length / 1048576.0:F1}MB");
            return s.result == BuildResult.Succeeded && over.Length == 0 ? 0 : 1;
        }

        // A static host such as GitHub Pages refuses files over 100 MB. With the shipped grouping three bundles were
        // big (fonts 143 MB, prefab 106 MB, bgm 81 MB, LZ4). The web player cannot read LZMA bundles ("Decompressing
        // this format (1) is not supported on this platform"), so every group stays LZ4: the fonts and the bgm tracks
        // get a bundle each, and the 2987 prefabs are cut into PrefabParts groups of about the same source size, in
        // address order so a folder stays together. Build() then fails on any file still over the limit.
        const string PrefabGroup = "dependencies_assets_prefab";
        const int PrefabParts = 6;

        static void FitBundlesUnderFileLimit(AddressableAssetSettings settings)
        {
            foreach (var group in settings.groups.ToList())
            {
                var schema = group == null ? null : group.GetSchema<UnityEditor.AddressableAssets.Settings.GroupSchemas.BundledAssetGroupSchema>();
                if (schema == null) continue;
                // A static host answers some requests with a 503 when hundreds come at once (GitHub Pages did, 2026-10-30),
                // and a download that stops receiving would wait forever: retry both (Timeout counts seconds without data).
                schema.RetryCount = 3;
                schema.Timeout = 60;
                if (schema.Compression == UnityEditor.AddressableAssets.Settings.GroupSchemas.BundledAssetGroupSchema.BundleCompressionMode.LZMA)
                {
                    schema.Compression = UnityEditor.AddressableAssets.Settings.GroupSchemas.BundledAssetGroupSchema.BundleCompressionMode.LZ4;
                    Debug.Log($"VD_WEBGL group {group.Name}: LZMA -> LZ4");
                }
                if (group.Name == "dependencies_assets_fonts" || group.Name == "remote_sound_assets_bgm")
                {
                    schema.BundleMode = UnityEditor.AddressableAssets.Settings.GroupSchemas.BundledAssetGroupSchema.BundlePackingMode.PackSeparately;
                    Debug.Log($"VD_WEBGL group {group.Name}: one bundle per entry");
                }
            }
            SplitGroup(settings, PrefabGroup, PrefabParts);
            AssetDatabase.SaveAssets();
        }

        // Two kinds of content the web player downloads before the title and never uses (2026-10-30, 427 MB came first):
        //   - the font files behind the dynamic TMP fonts (about 80 MB): the web player cannot add glyphs to the ripped
        //     atlases anyway (the Vietnamese accents came out garbled), and it starts in English, which the static
        //     atlases hold. The TMP fonts become Static (TMP then drops m_SourceFontFile) and the font files leave the
        //     fonts group; Pretendard stays, a legacy Text uses it.
        //   - the VideoClips (24 MB): the web player cannot play a clip from a bundle and plays the .webm copied next to
        //     the build instead; the VideoPlayers that name a clip get its URL, and the clips leave the video group.
        static void CutWebDeadWeight(AddressableAssetSettings settings)
        {
            int fonts = 0;
            foreach (string guid in AssetDatabase.FindAssets("t:TMP_FontAsset"))
            {
                var so = new SerializedObject(AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GUIDToAssetPath(guid)));
                var mode = so.FindProperty("m_AtlasPopulationMode");
                if (mode == null || mode.intValue == 0) continue;
                mode.intValue = 0;
                so.FindProperty("m_SourceFontFile").objectReferenceValue = null;
                so.ApplyModifiedPropertiesWithoutUndo();
                fonts++;
            }
            int fontFiles = 0;
            foreach (string path in Directory.GetFiles(FontDir).Where(p => p.EndsWith(".ttf") || p.EndsWith(".otf")))
            {
                if (Path.GetFileName(path).StartsWith("Pretendard")) continue;
                if (settings.RemoveAssetEntry(AssetDatabase.AssetPathToGUID(path.Replace('\\', '/')), false)) fontFiles++;
            }
            var clips = Directory.GetFiles(VideoSourceDir, "*.webm").ToDictionary(p => AssetDatabase.AssetPathToGUID(p.Replace('\\', '/')), p => Path.GetFileName(p));
            int players = 0;
            foreach (string file in Directory.GetFiles("Assets", "*.prefab", SearchOption.AllDirectories).Concat(Directory.GetFiles("Assets", "*.unity", SearchOption.AllDirectories)))
            {
                string text = File.ReadAllText(file);
                if (!text.Contains("--- !u!328 ") || !clips.Keys.Any(text.Contains)) continue;
                string patched = Regex.Replace(text, @"(--- !u!328 [^\n]*\n)((?:(?!--- )[^\n]*\n)*)", m =>
                {
                    string body = m.Groups[2].Value;
                    var clip = Regex.Match(body, @"m_VideoClip: \{fileID: \d+, guid: ([0-9a-f]{32}), type: 3\}");
                    if (!clip.Success || !clips.TryGetValue(clip.Groups[1].Value, out string name)) return m.Value;
                    players++;
                    body = body.Replace(clip.Value, "m_VideoClip: {fileID: 0}");
                    body = Regex.Replace(body, @"m_DataSource: \d", "m_DataSource: 1");
                    body = Regex.Replace(body, @"m_Url:[^\n]*", "m_Url: StreamingAssets/video/" + name);
                    body = Regex.Replace(body, @"m_AudioOutputMode: \d", "m_AudioOutputMode: 0");
                    return m.Groups[1].Value + body;
                });
                File.WriteAllText(file, patched);
            }
            int clipEntries = clips.Keys.Count(g => settings.RemoveAssetEntry(g, false));
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"VD_WEBGL web cuts: {fonts} TMP fonts made static, {fontFiles} font files and {clipEntries} video clips out of Addressables, {players} VideoPlayers play by URL");
        }

        // Moves the entries of <name> and of its parts <name>_1.._{parts-1} into the parts, in address order, cutting
        // where the running source size passes each 1/parts of the total. Running it again keeps the same split.
        static void SplitGroup(AddressableAssetSettings settings, string name, int parts)
        {
            var first = settings.FindGroup(name);
            if (first == null) throw new InvalidOperationException($"VD_WEBGL group {name} not found");
            var groups = new AddressableAssetGroup[parts];
            groups[0] = first;
            for (int i = 1; i < parts; i++)
                groups[i] = settings.FindGroup($"{name}_{i}") ?? settings.CreateGroup($"{name}_{i}", false, false, false, first.Schemas);
            var entries = groups.SelectMany(g => g.entries).OrderBy(e => e.address, StringComparer.Ordinal).ToList();
            long Size(AddressableAssetEntry e) => File.Exists(e.AssetPath) ? new FileInfo(e.AssetPath).Length : 0;
            long total = entries.Sum(Size), running = 0;
            var buckets = Enumerable.Range(0, parts).Select(_ => new System.Collections.Generic.List<AddressableAssetEntry>()).ToArray();
            foreach (var e in entries)
            {
                int part = (int)Math.Min(parts - 1, running * parts / Math.Max(1, total));
                buckets[part].Add(e);
                running += Size(e);
            }
            for (int i = 0; i < parts; i++)
            {
                settings.MoveEntries(buckets[i].Where(e => e.parentGroup != groups[i]).ToList(), groups[i], false, false);
                Debug.Log($"VD_WEBGL group {groups[i].Name}: {groups[i].entries.Count} entries, from {buckets[i].FirstOrDefault()?.address}");
            }
        }

        // The page is black from the engine start to the title (the shared bundles, about 70% of the build, come first).
        // A loading screen covers the canvas until the title calls WebLoadingScreen.Done (VdLoading.jslib): it shows the
        // megabytes the browser has received (resource timing, so the number is measured) against that 70% estimate.
        // The log collector (vd-log-collector.js.txt) goes first in <head>, before the Unity loader, so it sees every
        // console line from the engine start, and it keeps the log in localStorage through a hang and a reload.
        const string LogCollectorPath = "Assets/_VD/Editor/vd-log-collector.js.txt";

        static void AddLogCollector(string outDir)
        {
            string index = Path.Combine(outDir, "index.html");
            string html = File.ReadAllText(index);
            int head = html.IndexOf("<head>", StringComparison.Ordinal);
            if (head < 0)
                throw new InvalidDataException("VD_WEBGL index.html has no <head> to put the log collector in");
            if (!File.Exists(LogCollectorPath))
                throw new FileNotFoundException("VD_WEBGL log collector script not found", LogCollectorPath);
            string script = "\n<script>\n" + File.ReadAllText(LogCollectorPath) + "</script>";
            html = html.Insert(head + "<head>".Length, script);
            File.WriteAllText(index, html);
            Debug.Log("VD_WEBGL log collector added to index.html (F8 or the LOG tab saves voiddiver-log-<time>.txt)");
        }

        // Return visits (2026-11-03, measured with tools/harness/cache-check.mjs): the template's cacheControl answered
        // "no-store" for the .wasm, .framework.js, the Addressables catalog and the StreamingAssets pack, and
        // "must-revalidate" for the .data and the bundles. On GitHub Pages a revalidation is not reliable: its replicas
        // hold the same file with Last-Modified one second apart, and a stored copy from the other replica comes again in
        // full (If-Modified-Since 05:12:18 -> 200, 05:12:19 -> 304). The live webgl-37 downloaded 30 MB of bundles it
        // already had, plus the 15 MB .wasm, on the second visit. vd-cache-control.js.txt asks for every file by a URL
        // that names its content and keeps it in the loader's IndexedDB cache as "immutable"; this fills in the ids.
        // Runs after PackStreamingFiles and NameBundlesAsText, on the files as they ship.
        const string CacheControlPath = "Assets/_VD/Editor/vd-cache-control.js.txt";

        static void AddCacheControl(string outDir)
        {
            string index = Path.Combine(outDir, "index.html");
            string html = File.ReadAllText(index);
            if (!File.Exists(CacheControlPath))
                throw new FileNotFoundException("VD_WEBGL cache control script not found", CacheControlPath);
            // The engine files: the id goes into their URLs in the loader config.
            int engineFiles = 0;
            foreach (string file in Directory.GetFiles(Path.Combine(outDir, "Build")))
            {
                string name = Path.GetFileName(file);
                if (!System.Text.RegularExpressions.Regex.IsMatch(name, @"\.(data|wasm|framework\.js)(\.unityweb|\.br|\.gz)?$"))
                    continue;
                string quoted = "\"/" + name + "\"";
                if (!html.Contains(quoted))
                    throw new InvalidDataException("VD_WEBGL index.html does not load " + name + " (no " + quoted + " in the loader config)");
                html = html.Replace(quoted, "\"/" + name + "?v=" + ContentId(file) + "\"");
                engineFiles++;
            }
            if (engineFiles != 3)
                throw new InvalidDataException($"VD_WEBGL found {engineFiles} engine files (.data, .wasm, .framework.js) in {outDir}/Build, not 3");
            // StreamingAssets files with fixed names (the catalog, its settings, the pack and its list): the id goes into
            // vdCacheIds, keyed by the path under the web root. Bundles are named by their content already; the videos
            // play from a <video> element, not through UnityWebRequest.
            string streaming = Path.Combine(outDir, "StreamingAssets");
            var ids = new SortedDictionary<string, string>(StringComparer.Ordinal);
            foreach (string file in Directory.GetFiles(streaming, "*", SearchOption.AllDirectories))
            {
                string rel = "StreamingAssets/" + file.Substring(streaming.Length + 1).Replace('\\', '/');
                if (System.Text.RegularExpressions.Regex.IsMatch(rel, @"_[0-9a-f]{32}\.bundle(\.txt)?$") || rel.StartsWith("StreamingAssets/video/"))
                    continue;
                if (Uri.EscapeUriString(rel) != rel)
                    throw new InvalidDataException("VD_WEBGL " + rel + " has characters a URL escapes; the page could not match it to its cache id");
                ids[rel] = ContentId(file);
            }
            string json = "{" + string.Join(",", ids.Select(kv => "\n          \"" + kv.Key + "\": \"" + kv.Value + "\"")) + "\n        }";
            string script = File.ReadAllText(CacheControlPath);
            if (!script.Contains("/*VD_CACHE_IDS*/{}"))
                throw new InvalidDataException("VD_WEBGL " + CacheControlPath + " has no /*VD_CACHE_IDS*/{} for the ids");
            script = script.Replace("/*VD_CACHE_IDS*/{}", json);
            var line = System.Text.RegularExpressions.Regex.Match(html, @"\n[ \t]*productVersion: ""[^""]*"",\n");
            if (!line.Success)
                throw new InvalidDataException("VD_WEBGL index.html has no 'productVersion: \"...\",' line in the loader config to add cacheControl after");
            html = html.Insert(line.Index + line.Length, script);
            File.WriteAllText(index, html);
            Debug.Log($"VD_WEBGL cacheControl added to the loader config: {engineFiles} engine files and {ids.Count} StreamingAssets files by content id, bundles by name");
        }

        // The first 16 hex digits of the file's SHA-256.
        static string ContentId(string file)
        {
            using (var sha = System.Security.Cryptography.SHA256.Create())
            using (var stream = File.OpenRead(file))
                return BitConverter.ToString(sha.ComputeHash(stream)).Replace("-", "").Substring(0, 16).ToLowerInvariant();
        }

        // Batchmode: -executeMethod VD.EditorTools.WebGLBuild.CacheControlCLI -vdOut <web root>
        // Adds the return-visit cache to a web build made before AddCacheControl existed (its index.html must not have it).
        public static void CacheControlCLI()
        {
            string outDir = Arg("-vdOut", null);
            int code = 1;
            try
            {
                if (outDir == null || !File.Exists(Path.Combine(outDir, "index.html")))
                    throw new FileNotFoundException("VD_WEBGL -vdOut names no web build (no index.html)", outDir);
                if (File.ReadAllText(Path.Combine(outDir, "index.html")).Contains("vdCacheIds"))
                    throw new InvalidDataException("VD_WEBGL " + outDir + "/index.html already has the cache control");
                AddCacheControl(outDir);
                code = 0;
            }
            catch (Exception e)
            {
                Debug.LogError("VD_WEBGL cache control failed: " + e);
            }
            EditorApplication.Exit(code);
        }

        static void AddLoadingScreen(string outDir)
        {
            string index = Path.Combine(outDir, "index.html");
            long total = Directory.GetFiles(outDir, "*", SearchOption.AllDirectories).Sum(f => new FileInfo(f).Length);
            long expected = total * 7 / 10;
            const string style = @"<style>
#vd-loading{position:fixed;z-index:10;display:flex;flex-direction:column;align-items:center;justify-content:center;
background:#07090d;color:#cfe8e6;font:14px/1.5 system-ui,sans-serif;pointer-events:none;transition:opacity .6s}
#vd-loading h1{font:700 34px/1 system-ui,sans-serif;letter-spacing:.12em;margin:0 0 6px;color:#e8f6f4}
#vd-loading .sub{opacity:.6;margin-bottom:28px;letter-spacing:.08em}
#vd-loading .bar{width:min(420px,70%);height:4px;background:#1b2a2c;border-radius:2px;overflow:hidden}
#vd-loading .fill{height:100%;width:0;background:#46a4a0;transition:width .4s}
#vd-loading .status{margin-top:12px;opacity:.75;font-variant-numeric:tabular-nums}
html,body{margin:0;height:100%;overflow:hidden;background:#000}
#unity-container.unity-desktop{position:fixed;left:0;top:0;width:100%;height:100%;transform:none;display:flex;align-items:center;justify-content:center}
#unity-canvas{background:#000}
#unity-footer{display:none}
#vd-fullscreen{position:fixed;right:10px;bottom:10px;z-index:5;width:38px;height:38px;cursor:pointer;opacity:.35;
background:url('TemplateData/fullscreen-button.png') no-repeat center}
#vd-fullscreen:hover{opacity:.9}
</style>";
            string body = @"<div id=""vd-loading""><h1>VOID DIVER</h1><div class=""sub"">Escape from the Abyss</div>
<div class=""bar""><div class=""fill""></div></div><div class=""status"">Starting...</div></div>
<script>
(function () {
  var el = document.getElementById('vd-loading'), fill = el.querySelector('.fill'), status = el.querySelector('.status');
  var expected = " + expected + @", bytes = 0, done = false, last = 0, lastChange = Date.now();
  try {
    new PerformanceObserver(function (list) {
      list.getEntries().forEach(function (e) { bytes += e.encodedBodySize || e.transferSize || 0; });
    }).observe({ type: 'resource', buffered: true });
  } catch (e) {}
  function place() {
    var c = document.getElementById('unity-canvas');
    if (c) { var r = c.getBoundingClientRect(); el.style.left = r.left + 'px'; el.style.top = r.top + 'px';
      el.style.width = r.width + 'px'; el.style.height = r.height + 'px'; }
  }
  function tick() {
    if (done) return;
    place();
    if (bytes !== last) { last = bytes; lastChange = Date.now(); }
    var mb = (bytes / 1048576).toFixed(0), idle = Date.now() - lastChange > 2500;
    fill.style.width = Math.min(97, 100 * bytes / expected).toFixed(1) + '%';
    status.textContent = idle && bytes > 0 ? 'Preparing the game... (' + mb + ' MB loaded)' : 'Downloading game data... ' + mb + ' MB';
    requestAnimationFrame(tick);
  }
  window.vdLoadingDone = function () {
    if (done) return;
    done = true; fill.style.width = '100%'; el.style.opacity = '0';
    console.log('[vd-loading] title after ' + (performance.now() / 1000).toFixed(1) + ' s, ' + (bytes / 1048576).toFixed(0) + ' MB loaded');
    setTimeout(function () { el.remove(); }, 700);
    var c = document.getElementById('unity-canvas'); if (c) c.focus();
  };
  tick();
})();
// The game fills the window at 16:9, as on a monitor, instead of the template's fixed 960x600 box: leaving
// fullscreen (Esc) used to drop it into that small box. In fullscreen, Esc is locked to the page where the browser
// allows it (Chrome), so a press reaches the game's menu and holding it leaves fullscreen.
(function () {
  var canvas = document.getElementById('unity-canvas');
  function fit() {
    var w = window.innerWidth, h = window.innerHeight;
    if (w * 9 > h * 16) w = Math.round(h * 16 / 9); else h = Math.round(w * 9 / 16);
    canvas.style.width = w + 'px'; canvas.style.height = h + 'px';
  }
  fit();
  window.addEventListener('resize', fit);
  document.addEventListener('fullscreenchange', function () {
    fit();
    if (document.fullscreenElement && navigator.keyboard && navigator.keyboard.lock)
      navigator.keyboard.lock(['Escape']).catch(function () {});
  });
  var button = document.createElement('div');
  button.id = 'vd-fullscreen'; button.title = 'Fullscreen';
  button.onclick = function () { (document.getElementById('unity-container') || canvas).requestFullscreen().catch(function () {}); };
  document.body.appendChild(button);
})();
</script>
";
            string html = File.ReadAllText(index);
            if (!html.Contains("</head>") || !html.Contains("</body>"))
                throw new InvalidDataException("VD_WEBGL index.html has no </head> or </body> to add the loading screen to");
            // Saves live in persistentDataPath (LocalStorage, LocalBackupStorage); without this the browser keeps them only
            // until the page closes.
            const string autoSync = "// config.autoSyncPersistentDataPath = true;";
            if (!html.Contains(autoSync))
                throw new InvalidDataException("VD_WEBGL index.html has no '" + autoSync + "' line to turn on");
            html = html.Replace(autoSync, "config.autoSyncPersistentDataPath = true;");
            html = html.Replace("</head>", style + "\n</head>").Replace("</body>", body + "</body>");
            File.WriteAllText(index, html);
            Debug.Log($"VD_WEBGL loading screen added to index.html (expects {expected / 1048576} MB before the title)");
        }

        // Scene 0 of the web build: WebBoot loads asynchronously what the game would load synchronously, then opens
        // the game's first scene (scene 1).
        static string CreateBootScene()
        {
            var type = Type.GetType("WebBoot, Global") ?? throw new InvalidOperationException("VD_WEBGL type WebBoot not found in assembly Global");
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            new GameObject("WebBoot").AddComponent(type);
            var camera = new GameObject("Camera").AddComponent<Camera>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = Color.black;
            if (!EditorSceneManager.SaveScene(scene, BootScenePath))
                throw new IOException("VD_WEBGL boot scene was not saved to " + BootScenePath);
            return BootScenePath;
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

        // The 280 listed files (tables and Lua, 23 MB) took 150 s to come from GitHub Pages one by one, and a 503 on any of
        // them stopped the game before the title (2026-10-30). They go into one pack: the list gives each file's path and
        // length, in pack order, and the loose copies leave the build.
        // WebBoot.ServeBundlesAsText asks for every bundle as "<name>.bundle.txt" (see WebBoot.CompressibleSuffix).
        const string CompressibleSuffix = ".txt";

        static void NameBundlesAsText(string outDir)
        {
            string dir = Path.Combine(outDir, "StreamingAssets", "aa", "WebGL");
            string[] bundles = Directory.GetFiles(dir, "*.bundle");
            if (bundles.Length == 0)
                throw new FileNotFoundException("VD_WEBGL no .bundle files to rename in " + dir);
            foreach (string bundle in bundles)
                File.Move(bundle, bundle + CompressibleSuffix);
            Debug.Log($"VD_WEBGL {bundles.Length} bundles renamed to *.bundle{CompressibleSuffix}");
        }

        static void PackStreamingFiles(string outDir)
        {
            string root = Path.Combine(outDir, "StreamingAssets");
            string listPath = Path.Combine(root, StreamingListName);
            string[] files = File.ReadAllLines(listPath).Where(l => l.Length > 0).ToArray();
            var lines = new List<string>();
            using (var pack = File.Create(Path.Combine(root, StreamingPackName)))
            {
                foreach (string f in files)
                {
                    byte[] bytes = File.ReadAllBytes(Path.Combine(root, f));
                    pack.Write(bytes, 0, bytes.Length);
                    lines.Add(f + "\t" + bytes.Length);
                    File.Delete(Path.Combine(root, f));
                }
            }
            File.WriteAllText(listPath, string.Join("\n", lines) + "\n");
            foreach (string dir in Directory.GetDirectories(root, "*", SearchOption.AllDirectories).OrderByDescending(d => d.Length))
            {
                if (!Directory.EnumerateFileSystemEntries(dir).Any())
                    Directory.Delete(dir);
            }
            Debug.Log($"VD_WEBGL {files.Length} StreamingAssets files packed into {StreamingPackName}");
        }

        // Returns the platform the asset had, so the caller can put it back.
        static int SetPlatform(int platform) => SetEnvironment("Platform", platform);

        static int SetEnvironment(string field, int value)
        {
            var asset = AssetDatabase.LoadMainAssetAtPath(EnvironmentSettingsPath);
            if (asset == null)
                throw new FileNotFoundException("VD_WEBGL EnvironmentSettings asset not found", EnvironmentSettingsPath);
            var so = new SerializedObject(asset);
            var prop = so.FindProperty(field);
            int was = prop.intValue;
            prop.intValue = value;
            so.ApplyModifiedPropertiesWithoutUndo();
            AssetDatabase.SaveAssets();
            Debug.Log($"VD_WEBGL EnvironmentSettings.{field} {was} -> {value}");
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
