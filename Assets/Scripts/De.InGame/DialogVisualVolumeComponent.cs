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
	private ColorParameter _baseColor = new ColorParameter(new Color(0.6f, 0.6f, 0.75f, 1f), true);

	[FormerlySerializedAs("_dialogColor1")]
	[SerializeField]
	private ColorParameter _rimLightColor = new ColorParameter(new Color(1f, 1f, 1f, 1f), true);

	[SerializeField]
	private ClampedFloatParameter _rimLightAngle = new ClampedFloatParameter(40f, 0f, 360f, true);

	[SerializeField]
	private ClampedFloatParameter _rimLightOffset = new ClampedFloatParameter(0.01f, 0f, 1f, true);

	public ColorParameter BaseColor => _baseColor;

	public ColorParameter RimLightColor => _rimLightColor;

	public ClampedFloatParameter RimLightAngle => _rimLightAngle;

	public ClampedFloatParameter RimLightOffset => _rimLightOffset;

	public bool IsActive()
	{
		return _baseColor.overrideState || _rimLightColor.overrideState;
	}

	public bool IsTileCompatible()
	{
		return true;
	}
}
