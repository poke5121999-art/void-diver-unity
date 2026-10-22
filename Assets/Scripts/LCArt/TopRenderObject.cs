using System.Collections.Generic;
using UnityEngine;

public class TopRenderObject : MonoBehaviour
{
	private MeshRenderer[] _meshRenderers;

	private Dictionary<MeshRenderer, int> _originalLayers;

	[Header("Focus Setting")]
	[SerializeField]
	private LayerMask _focusLayer;

	private bool _initted;

	private bool _enable;

	private void Start()
	{
	}

	private int GetSingleLayerIndex(LayerMask mask)
	{
		return 0;
	}

	private void UpdateIfNeeded()
	{
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	private void OnDestroy()
	{
	}

	public void SetLayerMask(LayerMask mask)
	{
	}
}
