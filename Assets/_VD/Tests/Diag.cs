using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace VD.PlayTests
{
    // TEMPORARY diagnostics for the title capture (orientation markers, title canvas hierarchy).
    public static class Diag
    {
        public static GameObject[] AddMarkers()
        {
            var main = Camera.allCameras.FirstOrDefault(c => c.name == "MainCamera");
            var uiCam = Camera.allCameras.FirstOrDefault(c => c.name == "UICamera");
            var list = new System.Collections.Generic.List<GameObject>();
            if (main != null)
            {
                var q = GameObject.CreatePrimitive(PrimitiveType.Quad);
                q.name = "VD_MarkWorldRed";
                Object.Destroy(q.GetComponent<Collider>());
                var sh = Shader.Find("Universal Render Pipeline/Unlit");
                var m = new Material(sh);
                m.SetColor("_BaseColor", Color.red);
                m.color = Color.red;
                q.GetComponent<Renderer>().sharedMaterial = m;
                float d = main.nearClipPlane + 0.5f;
                q.transform.position = main.ViewportToWorldPoint(new Vector3(0.1f, 0.85f, d));
                q.transform.rotation = main.transform.rotation;
                float h = main.orthographic ? main.orthographicSize * 2 : 2f * d * Mathf.Tan(main.fieldOfView * 0.5f * Mathf.Deg2Rad);
                q.transform.localScale = Vector3.one * h * 0.12f;
                q.layer = 0;
                list.Add(q);
                Debug.LogWarning($"VD_MARK world shader={(sh ? sh.name : "null")} pos={q.transform.position}");
            }
            list.Add(MakeCanvas("VD_MarkOverlayGreen", RenderMode.ScreenSpaceOverlay, null, Color.green, new Vector2(0, 1), new Vector2(80, -80)));
            if (uiCam != null)
                list.Add(MakeCanvas("VD_MarkUICamBlue", RenderMode.ScreenSpaceCamera, uiCam, Color.blue, new Vector2(1, 1), new Vector2(-80, -80)));
            return list.ToArray();
        }

        static GameObject MakeCanvas(string name, RenderMode mode, Camera cam, Color color, Vector2 anchor, Vector2 pos)
        {
            var go = new GameObject(name);
            go.layer = LayerMask.NameToLayer("UI");
            var canvas = go.AddComponent<Canvas>();
            canvas.renderMode = mode;
            canvas.worldCamera = cam;
            canvas.sortingOrder = 1000;
            if (cam != null) canvas.planeDistance = 1f;
            var img = new GameObject("img");
            img.layer = go.layer;
            img.transform.SetParent(go.transform, false);
            var i = img.AddComponent<Image>();
            i.color = color;
            var rt = i.rectTransform;
            rt.anchorMin = rt.anchorMax = anchor;
            rt.sizeDelta = new Vector2(120, 120);
            rt.anchoredPosition = pos;
            return go;
        }

        static ScriptableRendererData[] RendererData()
        {
            var asset = (UniversalRenderPipelineAsset)GraphicsSettings.currentRenderPipeline;
            var f = typeof(UniversalRenderPipelineAsset).GetField("m_RendererDataList", BindingFlags.NonPublic | BindingFlags.Instance);
            return (ScriptableRendererData[])f.GetValue(asset);
        }

        static void SetFeatures(int rendererIndex, System.Func<ScriptableRendererFeature, bool> active)
        {
            var data = RendererData()[rendererIndex];
            foreach (var feat in data.rendererFeatures)
                if (feat != null) feat.SetActive(active(feat));
        }

        // Renders the title with parts of the pipeline switched off, one capture each.
        public static IEnumerator Variants()
        {
            var main = Camera.allCameras.First(c => c.name == "MainCamera");
            var md = main.GetUniversalAdditionalCameraData();
            var datas = RendererData();
            for (int i = 0; i < datas.Length; i++)
                Debug.LogWarning($"VD_RD {i} {datas[i].name} features={string.Join(",", datas[i].rendererFeatures.Select(x => x == null ? "null" : x.name + ":" + x.isActive))}");
            var uiCam = Camera.allCameras.First(c => c.name == "UICamera");
            var ud = uiCam.GetUniversalAdditionalCameraData();

            // 1. the game's UI camera rendered alone as a base camera
            main.enabled = false;
            ud.renderType = CameraRenderType.Base;
            var clear = uiCam.clearFlags; var bg = uiCam.backgroundColor;
            uiCam.clearFlags = CameraClearFlags.SolidColor; uiCam.backgroundColor = new Color(0.3f, 0.3f, 0.3f, 1);
            yield return Review.Shot("u-uicam-base", baseOnly: true);
            uiCam.clearFlags = clear; uiCam.backgroundColor = bg;
            ud.renderType = CameraRenderType.Overlay;
            main.enabled = true;

            // 2. main camera reduced to an empty renderer, stack kept
            int mask = main.cullingMask;
            main.cullingMask = 0;
            md.SetRenderer(8);
            md.renderPostProcessing = false;
            yield return Review.Shot("u-empty");
            md.SetRenderer(1);
            md.renderPostProcessing = true;
            main.cullingMask = mask;

            // 3. our own base camera with the game's UI camera stacked
            var ownGo = new GameObject("VD_OwnCamera");
            var own = ownGo.AddComponent<Camera>();
            own.clearFlags = CameraClearFlags.SolidColor; own.backgroundColor = new Color(0.3f, 0.3f, 0.3f, 1);
            own.cullingMask = 0;
            var od = ownGo.AddComponent<UniversalAdditionalCameraData>();
            od.SetRenderer(0);
            od.renderPostProcessing = false;
            od.cameraStack.Add(uiCam);
            main.enabled = false;
            yield return Review.Shot("u-own-stack");
            main.enabled = true;
            Object.Destroy(ownGo);
        }

        public static void DumpTitle()
        {
            var sb = new StringBuilder();
            foreach (var c in Object.FindObjectsByType<Canvas>(FindObjectsSortMode.None))
            {
                if (!c.isRootCanvas || c.gameObject.scene.name == "DontDestroyOnLoad") continue;
                Walk(c.transform, "", sb, 0);
            }
            foreach (var a in Object.FindObjectsByType<Animator>(FindObjectsSortMode.None))
            {
                if (a.gameObject.scene.name == "DontDestroyOnLoad") continue;
                var clips = a.isActiveAndEnabled && a.runtimeAnimatorController != null ? string.Join(",", a.GetCurrentAnimatorClipInfo(0).Select(ci => ci.clip.name + "@" + ci.weight)) : "-";
                var st = a.isActiveAndEnabled && a.runtimeAnimatorController != null ? a.GetCurrentAnimatorStateInfo(0) : default;
                sb.AppendLine($"VD_ANIM {Path(a.transform)} enabled={a.enabled} active={a.gameObject.activeInHierarchy} ctrl={(a.runtimeAnimatorController ? a.runtimeAnimatorController.name : "null")} state={st.shortNameHash} t={st.normalizedTime:F2} clips={clips}");
            }
            Debug.LogWarning(sb.ToString());
        }

        static string Path(Transform t) => t.parent == null ? t.name : Path(t.parent) + "/" + t.name;

        static void Walk(Transform t, string indent, StringBuilder sb, int depth)
        {
            var parts = new StringBuilder();
            parts.Append($"VD_UI {indent}{t.name} self={(t.gameObject.activeSelf ? 1 : 0)} hier={(t.gameObject.activeInHierarchy ? 1 : 0)} layer={t.gameObject.layer}");
            if (t is RectTransform r) parts.Append($" rect={r.rect.width:F0}x{r.rect.height:F0}@{r.position.x:F1},{r.position.y:F1},{r.position.z:F1} scale={r.lossyScale.x:F4}");
            foreach (var cg in t.GetComponents<CanvasGroup>()) parts.Append($" CG(a={cg.alpha:F2},en={cg.enabled})");
            foreach (var g in t.GetComponents<Graphic>())
                parts.Append($" {g.GetType().Name}(en={g.enabled},a={g.color.a:F2},mat={(g.materialForRendering ? g.materialForRendering.shader.name : "null")},sup={(g.materialForRendering && g.materialForRendering.shader.isSupported ? 1 : 0)},tex={(g.mainTexture ? g.mainTexture.name : "null")})");
            var cv = t.GetComponent<Canvas>();
            if (cv != null) parts.Append($" Canvas(en={cv.enabled},mode={cv.renderMode},order={cv.sortingOrder},override={cv.overrideSorting})");
            foreach (var mb in t.GetComponents<MonoBehaviour>())
                if (mb != null && !(mb is Graphic) && !(mb is CanvasScaler) && !(mb is GraphicRaycaster)) parts.Append($" [{mb.GetType().Name}{(mb.enabled ? "" : " OFF")}]");
                else if (mb == null) parts.Append(" [MISSING SCRIPT]");
            sb.AppendLine(parts.ToString());
            if (depth > 12) return;
            foreach (Transform ch in t) Walk(ch, indent + "  ", sb, depth + 1);
        }
    }
}
