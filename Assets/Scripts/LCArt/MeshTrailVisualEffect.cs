using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshTrailVisualEffect : MonoBehaviour
{
	[Header("Trail Setting")]
	[SerializeField]
	private int _trailCount = 10;

	private const int _trailMaxCount = 20;

	[SerializeField]
	private float _trailPeriod = 0.125f;

	[SerializeField]
	private Color _trailStartColor = Color.white;

	[SerializeField]
	private Color _trailEndColor = new Color(0f, 0f, 0f, 0f);

	private int _trailIndex;

	[SerializeField]
	private List<MeshFilter> _meshFilters = new List<MeshFilter>();

	[SerializeField]
	private List<MeshRenderer> _meshRenderers = new List<MeshRenderer>();

	private List<MeshTrailClone> _trailClones;

	private GameObject _trailCloneHolder;

	private IEnumerator _trailing;

	private bool _stillTrailing;

	[Header("Shader Start Setting")]
	[SerializeField]
	private Shader _shader;

	[SerializeField]
	private Material _material;

	private void Start()
	{
		InitializeMeshInformations();
		InitializeClones();
	}

	private void OnValidate()
	{
		_trailCount = Mathf.Clamp(_trailCount, 1, 20);
	}

	public void SetTrailCount(int count)
	{
		_trailCount = Mathf.Clamp(count, 1, 20);
	}

	public void SetTrailPeriod(float period)
	{
		_trailPeriod = period;
	}

	public void SetTrailColor(Color startColor, Color endColor)
	{
		_trailStartColor = startColor;
		_trailEndColor = endColor;
	}

	private bool IsValidFilters()
	{
		if (_meshFilters == null)
		{
			return false;
		}
		return _meshFilters.Count > 0;
	}

	private bool IsValidRenderers()
	{
		if (_meshRenderers == null)
		{
			return false;
		}
		return _meshRenderers.Count > 0;
	}

	private bool IsValidIndexClone()
	{
		if (_trailClones == null || _trailIndex >= _trailClones.Count)
		{
			return false;
		}
		MeshTrailClone clone = _trailClones[_trailIndex];
		if (clone == null)
		{
			return false;
		}
		return clone.gameObject != null;
	}

	private void InitializeMeshInformations()
	{
		if (_meshFilters == null || _meshFilters.Count == 0)
		{
			_meshFilters = GetComponentsInChildren<MeshFilter>(true).ToList();
		}
		if (_meshRenderers == null || _meshRenderers.Count == 0)
		{
			_meshRenderers = GetComponentsInChildren<MeshRenderer>(true).ToList();
		}
		if ((bool)_shader)
		{
			_material = new Material(_shader);
			if (_meshRenderers != null && _meshRenderers.Count > 0)
			{
				Material source = _meshRenderers[0].sharedMaterial;
				if (source != null)
				{
					_material.mainTexture = source.mainTexture;
				}
			}
		}
	}

	private void InitializeClones()
	{
		int layer = base.gameObject.layer;
		_trailCloneHolder = new GameObject(base.gameObject.name + " Mesh Trail Clone Holder");
		_trailCloneHolder.transform.SetParent(base.transform);
		_trailCloneHolder.layer = layer;
		_trailClones = new List<MeshTrailClone>(20);
		for (int i = 0; i < 20; i++)
		{
			GameObject cloneObject = new GameObject(string.Format("{0} Mesh Trail Clone {1}", base.gameObject.name, i));
			cloneObject.transform.SetParent(_trailCloneHolder.transform);
			cloneObject.layer = layer;
			MeshTrailClone clone = cloneObject.AddComponent<MeshTrailClone>();
			MeshRenderer cloneRenderer = clone.GetComponent<MeshRenderer>();
			if ((bool)cloneRenderer)
			{
				cloneRenderer.material = _material;
			}
			_trailClones.Add(clone);
		}
	}

	public void SetActiveTrail(bool active)
	{
		_stillTrailing = active;
		if (_trailing != null)
		{
			StopCoroutine(_trailing);
			_trailing = null;
		}
		if (active && base.isActiveAndEnabled)
		{
			_trailing = Trailing_CO();
			StartCoroutine(_trailing);
		}
	}

	private void OnEnable()
	{
		if (_stillTrailing && _trailing == null)
		{
			_trailing = Trailing_CO();
			StartCoroutine(_trailing);
		}
	}

	private void OnDisable()
	{
		_trailing = null;
	}

	private void ResetTrail()
	{
		_stillTrailing = false;
		if (_trailing != null)
		{
			StopCoroutine(_trailing);
			_trailing = null;
		}
		_trailClones.ForEach(delegate(MeshTrailClone clone)
		{
			clone.gameObject.SetActive(false);
		});
	}

	private IEnumerator Trailing_CO()
	{
		while (true)
		{
			ActiveTrailClone();
			yield return new WaitForSeconds(_trailPeriod);
		}
	}

	private void ActiveTrailClone()
	{
		if (_trailClones != null && _trailIndex < _trailClones.Count && (bool)_trailClones[_trailIndex])
		{
			MeshTrailClone clone = _trailClones[_trailIndex];
			clone.SetCloneMesh(GetActiveMesh());
			clone.ActiveClone((float)_trailCount * _trailPeriod, _trailStartColor, _trailEndColor, base.gameObject.transform.position, base.gameObject.transform.rotation, base.gameObject.transform.localScale);
		}
		_trailIndex = (_trailIndex + 1) % 20;
	}

	private Mesh GetActiveMesh()
	{
		MeshFilter filter = _meshFilters.FirstOrDefault((MeshFilter f) => f != null && f.gameObject.activeSelf);
		if (!(filter != null))
		{
			return null;
		}
		return filter.sharedMesh;
	}

	[ContextMenu("ActiveTrue")]
	public void SetActiveTrue()
	{
		_stillTrailing = true;
		if (_trailing != null)
		{
			StopCoroutine(_trailing);
			_trailing = null;
		}
		if (base.isActiveAndEnabled)
		{
			_trailing = Trailing_CO();
			StartCoroutine(_trailing);
		}
	}

	[ContextMenu("ActiveFalse")]
	public void SetActiveFalse()
	{
		_stillTrailing = false;
		if (_trailing != null)
		{
			StopCoroutine(_trailing);
			_trailing = null;
		}
	}

	[ContextMenu("Assign Renderer Components")]
	private void GetRendererComponents()
	{
		_meshFilters = GetComponentsInChildren<MeshFilter>(true).ToList();
		_meshRenderers.Clear();
		_meshRenderers = GetComponentsInChildren<MeshRenderer>(true).ToList();
	}
}
