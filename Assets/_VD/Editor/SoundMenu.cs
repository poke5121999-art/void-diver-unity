using UnityEditor;
using VD.Runtime;

namespace VD.EditorTools
{
    // VOID DIVER/Sound On: turns playtest sound on or off (see PlaytestMute). Applies at once when in play mode.
    static class SoundMenu
    {
        const string Item = "VOID DIVER/Sound On";

        [MenuItem(Item)]
        static void Toggle() => PlaytestMute.Set(!PlaytestMute.SoundOn(out _));

        [MenuItem(Item, true)]
        static bool Validate()
        {
            Menu.SetChecked(Item, PlaytestMute.SoundOn(out _));
            return true;
        }
    }
}
