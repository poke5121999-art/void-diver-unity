"""Codemods that make decompiled game stubs compile against the real packages.

1. Remove Netcode ILPP-generated overrides.

The shipped game was post-processed by Netcode for GameObjects, which adds __initializeVariables,
__initializeRpcs and __getTypeName to every NetworkBehaviour. Decompiled back to source they fail to
compile (the base members are internal), and the editor's ILPP adds them again on build anyway.
2. Delete Burst's generated __JobReflectionRegistrationOutput__*.cs; the editor regenerates it.
3. NetworkList<T> in Netcode 2.x needs T : unmanaged where the game's 1.x accepted struct.
Usage: python fix_stubs.py <Assets/Scripts dir>...   Idempotent.
"""
import os, re, sys

GENERATED = re.compile(
    r'\n[ \t]*(?:protected|public|internal)[\w \t]*\s(?:__initializeVariables|__initializeRpcs|__getTypeName)\(\)\s*\{[^{}]*\}[ \t]*\n',
)


def main():
    changed = 0
    for root in sys.argv[1:]:
        for dp, _, fs in os.walk(root):
            for f in fs:
                if not f.endswith('.cs'):
                    continue
                p = os.path.join(dp, f)
                if f.startswith('__JobReflectionRegistrationOutput__'):
                    os.remove(p)
                    if os.path.exists(p + '.meta'):
                        os.remove(p + '.meta')
                    changed += 1
                    continue
                text = open(p, encoding='utf-8').read()
                new = GENERATED.sub('\n', text).replace('where T : struct, IEquatable<T>', 'where T : unmanaged, IEquatable<T>')
                if new != text:
                    open(p, 'w', encoding='utf-8').write(new)
                    changed += 1
    print(f'fix_stubs: {changed} files changed')


main()
