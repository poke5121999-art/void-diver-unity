using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace VD.PlayTests
{
    // Review captures for docs/review: captures/review/<key>.png (the key matches docs/review/ref/<key>/).
    // Batchmode has no screen: WaitForEndOfFrame never fires and ScreenCapture returns nothing, so cameras are
    // rendered into a texture by hand and overlay canvases are moved onto a camera for the shot.
    public static class Review
    {
        public const int Width = 1280, Height = 720;

        static bool IsOverlay(Camera c) =>
            c.TryGetComponent<UniversalAdditionalCameraData>(out var d) && d.renderType == CameraRenderType.Overlay;

        public static IEnumerator Shot(string key, bool baseOnly = false)
        {
            yield return null;
            var rt = RenderTexture.GetTemporary(Width, Height, 24, RenderTextureFormat.ARGB32);
            var clear = RenderTexture.active;
            RenderTexture.active = rt;
            GL.Clear(true, true, Color.black);
            RenderTexture.active = clear;

            // URP renders overlay cameras only inside their base camera's stack, and a camera without URP data counts as a
            // base camera that clears the target with its background colour. So the game's Screen Space Overlay canvases
            // go onto an overlay camera of ours, appended to the first base camera's stack, and only base cameras render.
            var cameras = Camera.allCameras.Where(c => c.enabled && c.targetTexture == null && !IsOverlay(c)).OrderBy(c => c.depth).ToList();
            var uiCamGo = new GameObject("VD_ReviewUICamera");
            var uiCam = uiCamGo.AddComponent<Camera>();
            uiCam.cullingMask = 1 << LayerMask.NameToLayer("UI");
            uiCam.orthographic = true;
            uiCamGo.AddComponent<UniversalAdditionalCameraData>().renderType = CameraRenderType.Overlay;
            var stack = cameras.Count > 0 ? cameras[0].GetUniversalAdditionalCameraData().cameraStack : null;
            var saved = stack?.ToList();
            if (baseOnly) stack?.Clear(); else stack?.Add(uiCam);
            var moved = new List<Canvas>();
            foreach (var canvas in Object.FindObjectsByType<Canvas>(FindObjectsSortMode.None))
            {
                if (canvas.isRootCanvas && canvas.renderMode == RenderMode.ScreenSpaceOverlay && canvas.enabled)
                {
                    canvas.renderMode = RenderMode.ScreenSpaceCamera;
                    canvas.worldCamera = uiCam;
                    moved.Add(canvas);
                }
            }
            // The engine emits canvas geometry for the cameras it hands to the pipeline. Camera.Render() hands over only the
            // base camera, so the overlays of its stack drew no UI (A1 run 28: the title menu drew through UICamera as a
            // base camera and not as an overlay). Emit it for each overlay as URP starts rendering that camera.
            var overlays = cameras.SelectMany(c => c.GetUniversalAdditionalCameraData().cameraStack ?? new List<Camera>())
                .Where(c => c != null).ToList();
            void Emit(ScriptableRenderContext _, Camera cam)
            {
                if (overlays.Contains(cam)) ScriptableRenderContext.EmitGeometryForCamera(cam);
            }
            RenderPipelineManager.beginCameraRendering += Emit;
            Canvas.ForceUpdateCanvases();
            foreach (var cam in cameras)
            {
                cam.targetTexture = rt;
                cam.Render();
                cam.targetTexture = null;
            }
            RenderPipelineManager.beginCameraRendering -= Emit;
            if (baseOnly) { stack.Clear(); stack.AddRange(saved); } else stack?.Remove(uiCam);
            foreach (var canvas in moved)
            {
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvas.worldCamera = null;
            }
            Object.Destroy(uiCamGo);

            RenderTexture.active = rt;
            var tex = new Texture2D(Width, Height, TextureFormat.RGB24, false);
            tex.ReadPixels(new Rect(0, 0, Width, Height), 0, 0);
            tex.Apply();
            RenderTexture.active = clear;
            RenderTexture.ReleaseTemporary(rt);

            Directory.CreateDirectory(Path.Combine("captures", "review"));
            string path = Path.Combine("captures", "review", key + ".png");
            File.WriteAllBytes(path, tex.EncodeToPNG());
            Object.Destroy(tex);
            Debug.LogWarning($"VD_REVIEW_SHOT {path} cameras={cameras.Count} overlayCanvases={moved.Count}");
        }
    }
}
