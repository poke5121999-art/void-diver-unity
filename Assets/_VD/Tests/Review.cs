using System.Collections;
using System.IO;
using UnityEngine;

namespace VD.PlayTests
{
    // Review captures for docs/review/<area>.md. Files: captures/review/<area>/<NN>-<label>.png
    public static class Review
    {
        static int counter;

        public static IEnumerator Shot(string area, string label)
        {
            yield return new WaitForEndOfFrame();
            string dir = Path.Combine("captures", "review", area);
            Directory.CreateDirectory(dir);
            var tex = ScreenCapture.CaptureScreenshotAsTexture();
            string path = Path.Combine(dir, $"{++counter:00}-{label}.png");
            File.WriteAllBytes(path, tex.EncodeToPNG());
            Debug.Log($"VD_REVIEW_SHOT {path} {tex.width}x{tex.height}");
            Object.Destroy(tex);
        }
    }
}
