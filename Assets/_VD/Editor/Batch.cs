using System;
using UnityEngine;

namespace VD.EditorTools
{
    // Unity takes one -executeMethod per run; this runs several tools in one editor start.
    // -executeMethod VD.EditorTools.Batch.Run -vdSteps catalog,census,capture
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
