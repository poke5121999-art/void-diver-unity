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
	private List<CameraPostProcessingMaterialProperty> _stressPostProcessingProperties = new List<CameraPostProcessingMaterialProperty>();

	[FormerlySerializedAs("_sanityPostProcessingUIProperties")]
	[SerializeField]
	private List<CameraPostProcessingMaterialProperty> _stressPostProcessingUIProperties = new List<CameraPostProcessingMaterialProperty>();

	[FormerlySerializedAs("_sanityPostProcessingEffects")]
	[Header("Camera Post Processing Setting: Progress")]
	[FormerlySerializedAs("SanityPostProcessingEffects")]
	[SerializeField]
	private List<CameraPostProcessingInfo> _stressPostProcessingEffects = new List<CameraPostProcessingInfo>();

	[FormerlySerializedAs("_sanityPostProcessingUIEffects")]
	[FormerlySerializedAs("SanityPostProcessingEffects")]
	[SerializeField]
	private List<CameraPostProcessingInfo> _stressPostProcessingUIEffects = new List<CameraPostProcessingInfo>();

	private bool _propertyIDsInitialized;

	private void InitializePropertyIDs()
	{
		if (_propertyIDsInitialized)
		{
			return;
		}
		foreach (CameraPostProcessingMaterialProperty stressPostProcessingProperty in _stressPostProcessingProperties)
		{
			stressPostProcessingProperty.InitializePropertyID();
		}
		foreach (CameraPostProcessingMaterialProperty stressPostProcessingUIProperty in _stressPostProcessingUIProperties)
		{
			stressPostProcessingUIProperty.InitializePropertyID();
		}
		foreach (CameraPostProcessingInfo stressPostProcessingEffect in _stressPostProcessingEffects)
		{
			stressPostProcessingEffect.InitializePropertyID();
		}
		foreach (CameraPostProcessingInfo stressPostProcessingUIEffect in _stressPostProcessingUIEffects)
		{
			stressPostProcessingUIEffect.InitializePropertyID();
		}
		_propertyIDsInitialized = true;
	}

	private void UpdateProperties()
	{
		InitializePropertyIDs();
		foreach (CameraPostProcessingMaterialProperty stressPostProcessingProperty in _stressPostProcessingProperties)
		{
			if (stressPostProcessingProperty.PropertyID != -1)
			{
				Shader.SetGlobalFloat(stressPostProcessingProperty.PropertyID, stressPostProcessingProperty.PropertyFactor);
			}
		}
		if (!_stressScreenUIMaterial)
		{
			return;
		}
		foreach (CameraPostProcessingMaterialProperty stressPostProcessingUIProperty in _stressPostProcessingUIProperties)
		{
			if (stressPostProcessingUIProperty.PropertyID != -1)
			{
				_stressScreenUIMaterial.SetFloat(stressPostProcessingUIProperty.PropertyID, stressPostProcessingUIProperty.PropertyFactor);
			}
		}
	}

	private float ConvertProgressForMaterial(in int progress, in int minProgress, in int maxProgress)
	{
		int num = Mathf.Max(progress - minProgress, 0);
		int num2 = Mathf.Max(maxProgress - minProgress, 1);
		return 1f - Mathf.Clamp01((float)num / (float)num2);
	}

	public void UpdateProgress(float intensity)
	{
		InitializePropertyIDs();
		// Trap 59 (docs/DECODING.md): float32 product before the integer conversion, as IL2CPP; Mono needs the (float).
		int progress = (int)(float)((1f - intensity) * 100f);
		foreach (CameraPostProcessingInfo stressPostProcessingEffect in _stressPostProcessingEffects)
		{
			if (stressPostProcessingEffect.PropertyID != -1)
			{
				Shader.SetGlobalFloat(stressPostProcessingEffect.PropertyID, ConvertProgressForMaterial(in progress, in stressPostProcessingEffect.MinProgress, in stressPostProcessingEffect.MaxProgress));
			}
		}
		if (!_stressScreenUIMaterial)
		{
			return;
		}
		foreach (CameraPostProcessingInfo stressPostProcessingUIEffect in _stressPostProcessingUIEffects)
		{
			if (stressPostProcessingUIEffect.PropertyID != -1)
			{
				_stressScreenUIMaterial.SetFloat(stressPostProcessingUIEffect.PropertyID, ConvertProgressForMaterial(in progress, in stressPostProcessingUIEffect.MinProgress, in stressPostProcessingUIEffect.MaxProgress));
			}
		}
	}

	public void ResetProgress()
	{
		InitializePropertyIDs();
		foreach (CameraPostProcessingInfo stressPostProcessingEffect in _stressPostProcessingEffects)
		{
			if (stressPostProcessingEffect.PropertyID != -1)
			{
				Shader.SetGlobalFloat(stressPostProcessingEffect.PropertyID, 0f);
			}
		}
		if (!_stressScreenUIMaterial)
		{
			return;
		}
		foreach (CameraPostProcessingInfo stressPostProcessingUIEffect in _stressPostProcessingUIEffects)
		{
			if (stressPostProcessingUIEffect.PropertyID != -1)
			{
				_stressScreenUIMaterial.SetFloat(stressPostProcessingUIEffect.PropertyID, 0f);
			}
		}
	}

	private void OnEnable()
	{
		_propertyIDsInitialized = false;
		InitializePropertyIDs();
		UpdateProperties();
	}
}
