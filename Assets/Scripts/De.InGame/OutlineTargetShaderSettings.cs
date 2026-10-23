using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Outline Target Shader Settings", menuName = "NEMO Art/Outline Target Shader Settings")]
public class OutlineTargetShaderSettings : ScriptableObject
{
	[SerializeField]
	private List<Shader> _outlineTargetShaders = new List<Shader>();

	public List<Shader> OutlineTargetShaders => _outlineTargetShaders;

	public bool IsEnableTargetShader(Shader shader)
	{
		if (_outlineTargetShaders == null)
		{
			return true;
		}
		if (shader == null)
		{
			return false;
		}
		return _outlineTargetShaders.Any((Shader s) => s != null && s.name == shader.name);
	}
}
