using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class CameraPostProcessingInfo
{
	private const int _progressMaxValue = 100;

	[FormerlySerializedAs("Name")]
	public string ProgressName;

	public string Description;

	[Space(10f)]
	[Range(0f, 100f)]
	public int MinProgress;

	[Range(0f, 100f)]
	public int MaxProgress;

	[NonSerialized]
	public int PropertyID;

	public void InitializePropertyID()
	{
	}
}
