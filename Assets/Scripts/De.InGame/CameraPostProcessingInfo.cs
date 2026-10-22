using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class CameraPostProcessingInfo
{
	private const int _progressMaxValue = 100;

	[FormerlySerializedAs("Name")]
	public string ProgressName = "";

	public string Description = "";

	[Space(10f)]
	[Range(0f, 100f)]
	public int MinProgress;

	[Range(0f, 100f)]
	public int MaxProgress = 100;

	[NonSerialized]
	public int PropertyID = -1;

	public void InitializePropertyID()
	{
		if (!string.IsNullOrEmpty(ProgressName))
		{
			PropertyID = Shader.PropertyToID(ProgressName);
		}
	}
}
