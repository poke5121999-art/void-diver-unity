using System;
using System.Collections.Generic;
using UnityEngine;

public class TopRenderObject : MonoBehaviour
{
	private MeshRenderer[] _meshRenderers;

	private Dictionary<MeshRenderer, int> _originalLayers = new Dictionary<MeshRenderer, int>();

	[Header("Focus Setting")]
	[SerializeField]
	private LayerMask _focusLayer = 67108864;

	private bool _initted;

	private bool _enable = true;

	private void Start()
	{
		_meshRenderers = GetComponentsInChildren<MeshRenderer>();
		MeshRenderer[] meshRenderers = _meshRenderers;
		foreach (MeshRenderer meshRenderer in meshRenderers)
		{
			if (meshRenderer == null)
			{
				continue;
			}
			for (int i = 0; i < meshRenderer.sharedMaterials.Length; i++)
			{
				if (!meshRenderer.GetComponent<TopRenderExceptionObject>())
				{
					_originalLayers[meshRenderer] = meshRenderer.gameObject.layer;
				}
			}
		}
		_initted = true;
		UpdateIfNeeded();
	}

	private int GetSingleLayerIndex(LayerMask mask)
	{
		int value = mask.value;
		if (value == 0 || (value & (value - 1)) != 0)
		{
			Debug.LogWarning("TopRenderObject: 단일 레이어만 선택해야 합니다.");
		}
		return Mathf.RoundToInt((float)Math.Log((float)value, 2.0));
	}

	private void UpdateIfNeeded()
	{
		foreach (MeshRenderer key in _originalLayers.Keys)
		{
			if (_enable)
			{
				key.gameObject.layer = GetSingleLayerIndex(_focusLayer);
			}
			else
			{
				key.gameObject.layer = _originalLayers[key];
			}
		}
	}

	private void OnEnable()
	{
		if (_initted)
		{
			_enable = true;
			UpdateIfNeeded();
		}
	}

	private void OnDisable()
	{
		_enable = false;
		UpdateIfNeeded();
	}

	private void OnDestroy()
	{
		_enable = false;
		UpdateIfNeeded();
	}

	public void SetLayerMask(LayerMask mask)
	{
		_focusLayer = mask;
		UpdateIfNeeded();
	}
}
