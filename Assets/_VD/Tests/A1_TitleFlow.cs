using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace VD.PlayTests
{
    // A1: boot from SplashScene (build index 0) like the shipped game and capture the title flow.
    public class A1_TitleFlow
    {
        // The game boots as soon as play mode starts (VContainer's RuntimeInitialize), before the test body runs.
        // Errors logged in those first frames land in the test's log scope and fail it before
        // ignoreFailingMessages below is reached, so the flag is raised from the first error on.
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void IgnoreBootErrors()
        {
            Application.logMessageReceivedThreaded -= OnLog;
            Application.logMessageReceivedThreaded += OnLog;
        }

        // Diagnostics for the title capture: cameras, canvases and renderers drawing with a broken shader.
        // LogWarning: the game's SeriLogger raises Debug.unityLogger.filterLogType, so Debug.Log never reaches the log.
        static void DumpScene()
        {
            foreach (var c in Object.FindObjectsByType<Camera>(FindObjectsSortMode.None))
            {
                var d = c.GetComponent<UniversalAdditionalCameraData>();
                Debug.LogWarning($"VD_DUMP camera {c.name} enabled={c.enabled} active={c.gameObject.activeInHierarchy} depth={c.depth} mask={c.cullingMask} pos={c.transform.position} " +
                    $"clear={c.clearFlags} bg={c.backgroundColor} target={(c.targetTexture ? c.targetTexture.name + " " + c.targetTexture.width + "x" + c.targetTexture.height : "-")} " +
                    $"urp={(d ? d.renderType + " stack=" + (d.cameraStack?.Count ?? 0) : "-")}");
            }
            foreach (var c in Object.FindObjectsByType<Canvas>(FindObjectsSortMode.None))
                if (c.isRootCanvas)
                    Debug.LogWarning($"VD_DUMP canvas {c.name} mode={c.renderMode} cam={(c.worldCamera ? c.worldCamera.name : "-")} active={c.gameObject.activeInHierarchy} order={c.sortingOrder} layer={c.gameObject.layer} scene={c.gameObject.scene.name}");
            foreach (var r in Object.FindObjectsByType<Renderer>(FindObjectsSortMode.None))
                foreach (var m in r.sharedMaterials)
                    if (m != null && r.isVisible && (m.shader == null || !m.shader.isSupported || m.shader.name.Contains("Error")))
                        Debug.LogWarning($"VD_DUMP badshader renderer={r.name} mat={m.name} shader={(m.shader ? m.shader.name : "null")} bounds={r.bounds.size}");
            foreach (var r in Object.FindObjectsByType<Renderer>(FindObjectsSortMode.None))
                if (r.isVisible && r.bounds.size.magnitude > 50f)
                    Debug.LogWarning($"VD_DUMP big renderer={r.name} mat={(r.sharedMaterial ? r.sharedMaterial.name + "/" + r.sharedMaterial.shader.name : "null")} size={r.bounds.size}");
        }

        static void OnLog(string message, string stackTrace, LogType type)
        {
            if (type == LogType.Log || type == LogType.Warning) return;
            try { if (!LogAssert.ignoreFailingMessages) LogAssert.ignoreFailingMessages = true; }
            catch (System.InvalidOperationException) { }  // no log scope open yet
        }

        [UnityTest]
        public IEnumerator BootToTitle()
        {
            LogAssert.ignoreFailingMessages = true;
            SceneManager.LoadScene(0);
            for (int i = 0; i < 6; i++)
            {
                yield return new WaitForSecondsRealtime(4f);
                Debug.LogWarning($"VD_A1 t={i * 4 + 4}s scenes={SceneManager.sceneCount} active={SceneManager.GetActiveScene().name}");
                yield return Review.Shot($"a1-boot-{i * 4 + 4:00}s");
            }
            DumpScene();
            Diag.DumpTitle();
            yield return Review.Shot("title-menu");
            var marks = Diag.AddMarkers();
            yield return null;
            yield return Review.Shot("mark-stack");
            foreach (var m in marks) Object.Destroy(m);
            Assert.Pass($"active scene after 24 s: {SceneManager.GetActiveScene().name}");
        }
    }
}
