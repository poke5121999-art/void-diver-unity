#!/bin/sh
# Compile C# outside the Unity editor with the editor's own Roslyn, for fast checks of game code.
# Usage: tools/harness/csc.sh <out.dll|out.exe> <extra csc args or sources...>
# References: netstandard 2.1, UnityEngine modules, Newtonsoft and NuGet DLLs (donor), and with
# HARNESS_SCRIPT_ASSEMBLIES=<dir> every DLL in that dir. Arguments go through a response file, so paths may
# contain spaces. Final proof still runs in Unity.
set -eu
E=${UNITY_EDITOR_DATA:-"$HOME/Unity/Hub/Editor/6000.3.18f1/Editor/Data"}
OUT=$1; shift
RSP="$OUT.rsp"
case "$OUT" in *.exe) TARGET=exe ;; *) TARGET=library ;; esac
{
  echo "-nologo -noconfig -nostdlib -langversion:9 -target:$TARGET"
  echo "-out:\"$OUT\""
  echo "-nowarn:CS0067,CS0169,CS0414,CS0649,CS1998,CS8632,CS1701"
  echo "-r:\"$E/NetStandard/ref/2.1.0/netstandard.dll\""
  for d in "$E"/Managed/UnityEngine/UnityEngine*.dll; do echo "-r:\"$d\""; done
  echo "-r:\"$HOME/vd-rip/donor/com.unity.nuget.newtonsoft-json/Runtime/Newtonsoft.Json.dll\""
  for d in "$HOME"/vd-rip/donor/nuget/*.dll; do echo "-r:\"$d\""; done
  if [ -n "${HARNESS_SCRIPT_ASSEMBLIES:-}" ]; then
    for d in "$HARNESS_SCRIPT_ASSEMBLIES"/*.dll; do
      case "$(basename "$d")" in UnityEngine.dll|UnityEngine.*Module.dll|Newtonsoft.Json.dll) ;; *) echo "-r:\"$d\"" ;; esac
    done
  fi
  for a in "$@"; do
    case "$a" in -*) echo "$a" ;; *) echo "\"$a\"" ;; esac
  done
} > "$RSP"
exec "$E/NetCoreRuntime/dotnet" "$E/DotNetSdkRoslyn/csc.dll" "@$RSP"
