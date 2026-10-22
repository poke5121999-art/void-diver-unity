using System;
using UnityEngine;

[Serializable]
public class CameraPostProcessingMaterialProperty
{
	public string PropertyName;

	public string Description;

	[Space(10f)]
	public float PropertyFactor;

	[NonSerialized]
	public int PropertyID;

	public void InitializePropertyID()
	{
	}
}
