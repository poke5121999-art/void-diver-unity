using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundTriggerManager : MonoBehaviour
{
	public static BackgroundTriggerManager Instance;

	[SerializeField]
	private float _fadeTime = 0.25f;

	private readonly int _globalCoverSeeThroughFactorID = Shader.PropertyToID("_GlobalCoverSeeThroughFactor");

	private readonly Dictionary<Collider, HashSet<BackgroundTriggerUnit>> _unitSets = new Dictionary<Collider, HashSet<BackgroundTriggerUnit>>();

	private bool _isHidden;

	private float _currentFactor = 1f;

	private IEnumerator _fadeCo;

	public float FadeTime
	{
		get
		{
			return _fadeTime;
		}
		set
		{
			_fadeTime = value;
		}
	}

	public int EnterCount => _unitSets.Count;

	private void Awake()
	{
		if (Instance != null)
		{
			Debug.LogWarning("Background Trigger : Warning Multiple BackgroundTriggerManager");
			Destroy(base.gameObject);
			return;
		}
		Instance = this;
		_unitSets.Clear();
		_isHidden = false;
		_currentFactor = 1f;
		Shader.SetGlobalFloat(_globalCoverSeeThroughFactorID, 1f);
	}

	private void OnDestroy()
	{
		if (Instance == this)
		{
			Instance = null;
			_unitSets.Clear();
			Shader.SetGlobalFloat(_globalCoverSeeThroughFactorID, 1f);
		}
	}

	public void NotifyEnter(Collider col, BackgroundTriggerUnit unit)
	{
		if (!_unitSets.TryGetValue(col, out var value))
		{
			value = new HashSet<BackgroundTriggerUnit>();
			_unitSets[col] = value;
		}
		if (value.Add(unit))
		{
			UpdateFade();
		}
	}

	public void NotifyExit(Collider col, BackgroundTriggerUnit unit)
	{
		if (_unitSets.TryGetValue(col, out var value) && value.Remove(unit))
		{
			if (value.Count == 0)
			{
				_unitSets.Remove(col);
			}
			UpdateFade();
		}
	}

	private void UpdateFade()
	{
		bool flag = _unitSets.Count > 0;
		if (flag == _isHidden)
		{
			return;
		}
		_isHidden = flag;
		if (_fadeCo != null)
		{
			StopCoroutine(_fadeCo);
		}
		_fadeCo = Fade(_isHidden ? 0f : 1f);
		StartCoroutine(_fadeCo);
	}

	private IEnumerator Fade(float endFactor)
	{
		float startFactor = _currentFactor;
		float t = 0f;
		while (t < 1f)
		{
			t += Time.deltaTime / Mathf.Max(_fadeTime, Mathf.Epsilon);
			_currentFactor = Mathf.Lerp(startFactor, endFactor, t);
			Shader.SetGlobalFloat(_globalCoverSeeThroughFactorID, _currentFactor);
			yield return null;
		}
		_currentFactor = endFactor;
		Shader.SetGlobalFloat(_globalCoverSeeThroughFactorID, endFactor);
		_fadeCo = null;
	}
}
