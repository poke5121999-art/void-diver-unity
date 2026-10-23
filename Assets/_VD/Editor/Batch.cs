using System;
using UnityEngine;

namespace VD.EditorTools
{
    // Unity takes one -executeMethod per run; this runs several tools in one editor start.
    // -executeMethod VD.EditorTools.Batch.Run -vdSteps catalog,census,capture
    // Step "pipeline" assigns the game's URP asset in memory (ShaderShots.UsePipeline; -vdPipeline overrides the path):
    // ProjectSettings point at a URP asset GUID that no longer exists, and under the built-in pipeline every game shader
    // draws magenta. Put it before capture/shots; nothing is saved (run through tools/shaders/shot.sh to be sure).
    public static class Batch
    {
        public static void Run()
        {
            var args = Environment.GetCommandLineArgs();
            int i = Array.IndexOf(args, "-vdSteps");
            string steps = i >= 0 && i + 1 < args.Length ? args[i + 1] : "census";
            foreach (string step in steps.Split(','))
            {
                try
                {
                    switch (step)
                    {
                        case "catalog": CatalogDump.Run(); break;
                        case "census": Census.Run(); break;
                        case "capture": Capture.Run(); break;
                        case "pipeline": ShaderShots.UsePipeline(ShaderShots.Arg("-vdPipeline", "Assets/MonoBehaviour/UniveralRP_VD.asset")); break;
                        case "shadergate": ShaderGate.CompileAll(); break;
                        case "shots": ShaderShots.Run(); break;
                        case "spriteatlas": SpriteAtlasRebuild.Run(); break;
                        case "addressables": AddressablesSetup.Run(); break;
                        default: throw new ArgumentException("unknown step " + step);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"VD_STEP_FAILED {step}: {e}");
                }
            }
        }
    }
}
