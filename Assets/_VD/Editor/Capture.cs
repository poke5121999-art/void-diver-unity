using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace VD.EditorTools
{
    // Batchmode (needs a graphics device, so no -nographics):
    //   -executeMethod VD.EditorTools.Capture.Run -vdScene <scene path> -vdTarget x,y,z -vdOut captures/x.png
    // Renders the scene through the game's camera rig: FOV 10, offset (25, 21, -25) from the target (ASSETS.md §3).
    public static class Capture
    {
        public static void Run()
        {
            string scene = Arg("-vdScene", "Assets/Scenes/InGame/LoungeScene.unity");
            string outPath = Arg("-vdOut", "captures/capture.png");
            Vector3 target = ParseVec(Arg("-vdTarget", "0,0,0"));
            int w = int.Parse(Arg("-vdWidth", "1920")), h = int.Parse(Arg("-vdHeight", "1080"));

            EditorSceneManager.OpenScene(scene, OpenSceneMode.Single);
            // Sectors and units are spawned by game code at runtime; -vdPrefab places them for a still capture.
            foreach (string spec in Arg("-vdPrefab", "").Split(';', StringSplitOptions.RemoveEmptyEntries))
            {
                var parts = spec.Split('@');
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(parts[0]);
                if (prefab == null) throw new FileNotFoundException("prefab not found", parts[0]);
                var inst = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                if (parts.Length > 1) inst.transform.position = ParseVec(parts[1]);
            }
            var go = new GameObject("VD_CaptureCamera");
            var cam = go.AddComponent<Camera>();
            cam.fieldOfView = float.Parse(Arg("-vdFov", "10"));
            cam.nearClipPlane = 0.1f;
            cam.farClipPlane = 500f;
            cam.transform.position = target + ParseVec(Arg("-vdOffset", "25,21,-25"));
            cam.transform.LookAt(target);

            var rt = new RenderTexture(w, h, 24, RenderTextureFormat.ARGB32);
            cam.targetTexture = rt;
            cam.Render();
            RenderTexture.active = rt;
            var tex = new Texture2D(w, h, TextureFormat.RGB24, false);
            tex.ReadPixels(new Rect(0, 0, w, h), 0, 0);
            tex.Apply();
            Directory.CreateDirectory(Path.GetDirectoryName(outPath));
            File.WriteAllBytes(outPath, tex.EncodeToPNG());
            Debug.Log($"VD_CAPTURE wrote {outPath} {w}x{h} scene={scene} target={target}");
        }

        static string Arg(string name, string fallback)
        {
            var args = Environment.GetCommandLineArgs();
            int i = Array.IndexOf(args, name);
            return i >= 0 && i + 1 < args.Length ? args[i + 1] : fallback;
        }

        static Vector3 ParseVec(string s)
        {
            var p = s.Split(',');
            return new Vector3(float.Parse(p[0]), float.Parse(p[1]), float.Parse(p[2]));
        }
    }
}
