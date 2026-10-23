using UnityEngine;
using UnityEngine.Rendering;

[ExecuteAlways]
public class LensFlareScaleController : MonoBehaviour
{
	[Header("Lens Flare Settings")]
	[SerializeField]
	private LensFlareComponentSRP _lensFlare;

	[Header("Scale Control")]
	[SerializeField]
	private float _baseFOV = 10f;

	[SerializeField]
	private float _baseScale = 1f;

	[SerializeField]
	private bool _enableFOVCompensation = true;

	[Header("Debug")]
	[SerializeField]
	private bool _showDebugInfo;

	private float _originalScale;

	private void OnEnable()
	{
		if (_lensFlare == null)
		{
			_lensFlare = GetComponent<LensFlareComponentSRP>();
		}
		if (_lensFlare != null)
		{
			_originalScale = _lensFlare.scale;
		}
		if (_showDebugInfo)
		{
			Debug.Log("[LensFlareScaleController] Enabled on " + base.gameObject.name);
		}
	}

	private void OnDisable()
	{
		if (_lensFlare != null)
		{
			_lensFlare.scale = _originalScale;
		}
		if (_showDebugInfo)
		{
			Debug.Log("[LensFlareScaleController] Disabled on " + base.gameObject.name);
		}
	}

	private void LateUpdate()
	{
		if (!_enableFOVCompensation || _lensFlare == null)
		{
			return;
		}
		Camera main = Camera.main;
		if (!(main == null))
		{
			float num = CalculateFOVRatio(main.fieldOfView);
			_lensFlare.scale = num * _baseScale;
			if (_showDebugInfo && Application.isPlaying)
			{
				Debug.Log(string.Format("[LensFlare] FOV: {0:F1}°, Ratio: {1:F2}, Scale: {2:F2}", main.fieldOfView, num, _lensFlare.scale));
			}
		}
	}

	private float CalculateFOVRatio(float currentFOV)
	{
		return _baseFOV / currentFOV;
	}

	private void OnValidate()
	{
		if (_lensFlare == null)
		{
			_lensFlare = GetComponent<LensFlareComponentSRP>();
		}
		_baseFOV = Mathf.Clamp(_baseFOV, 1f, 179f);
		_baseScale = Mathf.Max(0.01f, _baseScale);
	}
}
