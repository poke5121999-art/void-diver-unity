#!/usr/bin/env python3
# tools/webgl/patch-plugins.py <project>: make the ripped third-party plugins compile for the WebGL player on 6000.0.
# The plugins are not in git (tools/rip/regen.sh writes them), so tools/webgl-prep.sh runs this on the build copy.
# Idempotent: a block that is already guarded is left alone.
#   - ParticleImage: the static Vector2.Normalize(v) exists from 6000.1 on only.
#   - InputIcons: InputSystem compiles the Switch Pro / DualShock / DualSense HID classes for the editor and desktop
#     players only; on the web those pads are plain gamepads and the name checks below the type checks pick the icons.
#   - InputIcons' Steamworks extension calls SteamAPI.Init(); the web player has no Steam.
import re
import sys

DESKTOP = "#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_STANDALONE_LINUX || UNITY_WSA\n"
STEAM = "#if STEAMWORKS_NET\n"
END = "#endif\n"
HID = "SwitchProControllerHID|DualShock3GamepadHID|DualShock4GamepadHID|DualSenseGamepadHID"

root = sys.argv[1].rstrip("/") + "/Assets/Scripts/"


def guard(s, pattern, cond):
    """Wraps every match of pattern in cond ... #endif unless the line before it is already cond."""
    out, pos, n = [], 0, 0
    for m in re.finditer(pattern, s):
        before = s[:m.start()]
        if before.endswith(cond):
            continue
        out.append(s[pos:m.start()] + cond + m.group(0) + END)
        pos = m.end()
        n += 1
    out.append(s[pos:])
    return "".join(out), n


def patch(rel, fn):
    p = root + rel
    s = open(p, encoding="utf-8").read()
    s2, n = fn(s)
    if s2 != s:
        open(p, "w", encoding="utf-8").write(s2)
    print(f"patch-plugins: {rel}: {n} change(s)")


def normalize(s):
    return re.subn(r"Vector2\.Normalize\(([A-Za-z_][A-Za-z0-9_.]*)\)", r"\1.normalized", s)


def input_icons(s):
    total = 0
    s, n = guard(s, r"using UnityEngine\.InputSystem\.Switch;\n", DESKTOP); total += n
    # if (device is X) { <one line> }
    s, n = guard(s, r"(\t+)if \(device is (" + HID + r")\)\n\1\{\n\1\t[^\n]*\n\1\}\n", DESKTOP); total += n
    # if (!(device is DualShock4GamepadHID)) { ... }: on the web the block always runs, as for any non-DS4-HID pad.
    s, n = guard(s, r"\t+if \(!\(device is DualShock4GamepadHID\)\)\n", DESKTOP); total += n
    return s, total


def steam_extension(s):
    if s.startswith(STEAM):
        return s, 0
    return STEAM + s.rstrip("\n") + "\n" + END, 1


def icons_manager(s):
    return guard(s, r"(\t+)if \(Application\.isPlaying\)\n\1\{\n\1\tScriptableObject\.CreateInstance<InputIconsSteamworksExtensionSO>\(\);\n\1\}\n", STEAM)


patch("ParticleImage/AssetKits/ParticleImage/ParticleImage.cs", normalize)
for f in ["InputIconsDeviceUtility.cs", "II_CustomDeviceGraphicBehaviour.cs", "InputIconSetConfiguratorSO.cs"]:
    patch("InputIcons/InputIcons/" + f, input_icons)
patch("InputIcons/InputIcons/InputIconsSteamworksExtensionSO.cs", steam_extension)
patch("InputIcons/InputIcons/InputIconsManagerSO.cs", icons_manager)
