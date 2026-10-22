using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Outline Target Shader Settings", menuName = "NEMO Art/Outline Target Shader Settings")]
public class OutlineTargetShaderSettings : ScriptableObject
{
	[SerializeField]
	private List<Shader> _outlineTargetShaders;

	public List<Shader> OutlineTargetShaders => null;

	public bool IsEnableTargetShader(Shader shader)
	{
		return false;
	}
}
