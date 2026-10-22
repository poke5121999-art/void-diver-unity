using UnityEditor;
using UnityEngine;

namespace VD.EditorTools
{
    // Batchmode captures must not render the cyan "compiling" placeholder: compile shader variants synchronously.
    [InitializeOnLoad]
    public static class ShaderCompileSync
    {
        static ShaderCompileSync()
        {
            if (Application.isBatchMode)
                ShaderUtil.allowAsyncCompilation = false;
        }
    }
}
