using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

[Serializable]
[VolumeComponentMenu("Project DE/Dialog")]
[SupportedOnRenderPipeline(typeof(UniversalRenderPipelineAsset))]
public class DialogVisualVolumeComponent : VolumeComponent, IPostProcessComponent
{
	[Header("Dialog Visual Properties")]
	[FormerlySerializedAs("_dialogColor0")]
	[SerializeField]
	private ColorParameter _baseColor;

	[FormerlySerializedAs("_dialogColor1")]
	[SerializeField]
	private ColorParameter _rimLightColor;

	[SerializeField]
	private ClampedFloatParameter _rimLightAngle;

	[SerializeField]
	private ClampedFloatParameter _rimLightOffset;

	public ColorParameter BaseColor => null;

	public ColorParameter RimLightColor => null;

	public ClampedFloatParameter RimLightAngle => null;

	public ClampedFloatParameter RimLightOffset => null;

	public bool IsActive()
	{
		return false;
	}

	public bool IsTileCompatible()
	{
		return false;
	}
}
