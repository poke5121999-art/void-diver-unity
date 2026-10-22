using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace VD.PlayTests
{
    // A1: boot from SplashScene (build index 0) like the shipped game and capture the title flow.
    public class A1_TitleFlow
    {
        [UnityTest]
        public IEnumerator BootToTitle()
        {
            LogAssert.ignoreFailingMessages = true;
            SceneManager.LoadScene(0);
            for (int i = 0; i < 6; i++)
            {
                yield return new WaitForSecondsRealtime(4f);
                Debug.Log($"VD_A1 t={i * 4 + 4}s scenes={SceneManager.sceneCount} active={SceneManager.GetActiveScene().name}");
                yield return Review.Shot($"a1-boot-{i * 4 + 4:00}s");
            }
            yield return Review.Shot("title-menu");
            Assert.Pass($"active scene after 24 s: {SceneManager.GetActiveScene().name}");
        }
    }
}
