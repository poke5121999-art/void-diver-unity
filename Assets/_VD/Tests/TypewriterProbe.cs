using System.Collections;
using Febucci.UI;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.TestTools;

namespace VD.PlayTests
{
    // Dialog lines, radio toasts and unit bubbles stop after their first letter until the player clicks (A4 runs 22-24:
    // tutorial-start shows "W", the prologue "a", unit bubbles stay up with "O"). The same Febucci typewriter on a bare
    // canvas, outside the game, shows whether the typewriter itself reveals the text over time.
    public class TypewriterProbe
    {
        const string Line = "Hello there, this is a typewriter line that should appear letter by letter.";

        [UnityTest]
        public IEnumerator TypewriterRevealsTextOverTime()
        {
            LogAssert.ignoreFailingMessages = true;  // the game's own boot logs errors in this editor (user32.dll)
            var canvasGo = new GameObject("ProbeCanvas", typeof(Canvas));
            canvasGo.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            var go = new GameObject("ProbeText", typeof(RectTransform));
            go.transform.SetParent(canvasGo.transform, false);
            var tmp = go.AddComponent<TextMeshProUGUI>();
            tmp.rectTransform.sizeDelta = new Vector2(900f, 200f);
            var ta = go.AddComponent<TextAnimator_TMP>();
            var tw = go.AddComponent<TypewriterByCharacter>();
            yield return null;

            // What BaseUnitHudPresenter.TrySetWriterText does with a bubble.
            tw.ShowText(Line);
            tw.StartShowingText(restart: true);
            float start = Time.realtimeSinceStartup;
            int visible = 0;
            while (Time.realtimeSinceStartup - start < 6f)
            {
                yield return new WaitForSecondsRealtime(0.25f);
                visible = 0;
                var chars = ta.Characters;
                for (int i = 0; i < ta.CharactersCount; i++) if (chars[i].isVisible) visible++;
                var c5 = ta.CharactersCount > 5 ? chars[5] : default;
                byte alpha5 = 0;
                var ti = tmp.textInfo;
                if (ti != null && ti.characterCount > 5 && ti.characterInfo[5].isVisible)
                {
                    var ci = ti.characterInfo[5];
                    alpha5 = ti.meshInfo[ci.materialReferenceIndex].colors32[ci.vertexIndex].a;
                }
                Debug.LogWarning($"VD_TW t={Time.realtimeSinceStartup - start:F2} showing={tw.isShowingText} visible={visible}/{ta.CharactersCount} allShown={ta.allLettersShown} " +
                          $"taDelta={ta.time.deltaTime:F4} taTime={ta.time.timeSinceStart:F2} c5[vis={c5.isVisible} passed={c5.passedTime:F3} appearMax={c5.info.appearancesMaxDuration:F3} disMax={c5.info.disappearancesMaxDuration:F3}] " +
                          $"alpha5={alpha5} tmpMax={tmp.maxVisibleCharacters} tmpChars={ti?.characterCount}");
                if (!tw.isShowingText && visible == ta.CharactersCount) break;
            }
            Object.Destroy(canvasGo);
            Assert.AreEqual(ta.CharactersCount, visible, "typewriter did not reveal the whole line within 6 s");
        }
    }
}
