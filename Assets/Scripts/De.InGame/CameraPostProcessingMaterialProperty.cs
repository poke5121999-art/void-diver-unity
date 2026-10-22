using System;
using UnityEngine;

[Serializable]
public class CameraPostProcessingMaterialProperty
{
	public string PropertyName = "";

	public string Description = "";

	[Space(10f)]
	public float PropertyFactor;

	[NonSerialized]
	public int PropertyID = -1;

	public void InitializePropertyID()
	{
		if (!string.IsNullOrEmpty(PropertyName))
		{
			PropertyID = Shader.PropertyToID(PropertyName);
		}
	}
}
