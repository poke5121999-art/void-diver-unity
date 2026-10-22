using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Camera Post Processing Scriptable Data", menuName = "NEMO Art/Camera Post Processing Scriptable Data")]
public class CameraPostProcessingScriptableObject : ScriptableObject
{
	private const int _progressMaxValue = 100;

	[FormerlySerializedAs("_sanityScreenMaterial")]
	[Header("Camera Post Processing Setting: Materials")]
	[SerializeField]
	private Material _stressScreenMaterial;

	[FormerlySerializedAs("_sanityScreenUIMaterial")]
	[SerializeField]
	private Material _stressScreenUIMaterial;

	[FormerlySerializedAs("_sanityPostProcessingProperties")]
	[Header("Camera Post Processing Setting: Properties")]
	[SerializeField]
	private List<CameraPostProcessingMaterialProperty> _stressPostProcessingProperties;

	[FormerlySerializedAs("_sanityPostProcessingUIProperties")]
	[SerializeField]
	private List<CameraPostProcessingMaterialProperty> _stressPostProcessingUIProperties;

	[FormerlySerializedAs("_sanityPostProcessingEffects")]
	[Header("Camera Post Processing Setting: Progress")]
	[FormerlySerializedAs("SanityPostProcessingEffects")]
	[SerializeField]
	private List<CameraPostProcessingInfo> _stressPostProcessingEffects;

	[FormerlySerializedAs("_sanityPostProcessingUIEffects")]
	[FormerlySerializedAs("SanityPostProcessingEffects")]
	[SerializeField]
	private List<CameraPostProcessingInfo> _stressPostProcessingUIEffects;

	private bool _propertyIDsInitialized;

	private void InitializePropertyIDs()
	{
	}

	private void UpdateProperties()
	{
	}

	private float ConvertProgressForMaterial(in int progress, in int minProgress, in int maxProgress)
	{
		return 0f;
	}

	public void UpdateProgress(float intensity)
	{
	}

	public void ResetProgress()
	{
	}

	private void OnEnable()
	{
	}
}
