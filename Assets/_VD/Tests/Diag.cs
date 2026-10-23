using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace VD.PlayTests
{
    // Title diagnostics for A1: the scene's canvas hierarchy (active state, CanvasGroup alpha, graphics and their shaders)
    // and its animators, logged as VD_UI / VD_ANIM lines.
    public static class Diag
    {
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
