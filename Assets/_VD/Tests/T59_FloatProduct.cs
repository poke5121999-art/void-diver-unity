using NUnit.Framework;
using UnityEngine;

namespace VD.PlayTests
{
    // Trap 59 (docs/DECODING.md): the shipped IL2CPP build rounds a float product to float32 before an integer cast; the
    // editor's Mono kept a wider intermediate and cut 600 * 0.7f to 419 (artifact deal price, 80a9e3be). 83 casts were
    // narrowed with (float) on that premise (dc570201). This pins the premise: if Mono stops widening, raw turns 420 and the
    // test fails, and the 419 seen in A9b needs another explanation. The operands are static fields, not constants, so the
    // compiler cannot fold the product.
    public class T59_FloatProduct
    {
        static int s_price = 600, s_dmg = 600;
        static float s_multiplier = 0.7f, s_percent = 70f;  // TConst.ArtifactShopCostMultiplier; a FrontGuard-style percent

        [Test]
        public void Trap59_MonoFloatProduct()
        {
            int raw = (int)((float)s_price * s_multiplier);
            int narrowed = (int)(float)((float)s_price * s_multiplier);
            int chainRaw = (int)((1f - s_percent / 100f) * (float)s_dmg);
            int chainNarrowed = (int)(float)((1f - s_percent / 100f) * (float)s_dmg);
            // LogWarning: the game's SeriLogger raises filterLogType, so Debug.Log never reaches the batch log (A1_TitleFlow).
            Debug.LogWarning($"VD_T59 raw={raw} narrowed={narrowed} chainRaw={chainRaw} chainNarrowed={chainNarrowed}");
            Assert.AreEqual(420, narrowed);
            Assert.AreEqual(419, raw, "Mono no longer widens: trap 59 premise changed");
        }
    }
}
