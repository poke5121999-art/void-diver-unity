using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;

[DefaultExecutionOrder(-10)]
[ExecuteAlways]
public class DecorationVisualManager : MonoBehaviour
{
	[SerializeField]
	private PlayableDirector _directorOut;

	[SerializeField]
	private PlayableDirector _directorIn;

	[Header("Dissolve Settings")]
	[SerializeField]
	private bool _dissolveEnabled = true;

	[SerializeField]
	private Color _edgeColor = Color.white;

	[Header("Debug: 타이밍 테스트용 수동 등록")]
	[SerializeField]
	private List<DecorationObjectDissolveTarget> _debugObjectTargets = new List<DecorationObjectDissolveTarget>();

	[SerializeField]
	private List<DecorationSkinDissolveTarget> _debugSkinTargets = new List<DecorationSkinDissolveTarget>();

	private readonly List<IDissolveTarget> _targets = new List<IDissolveTarget>();

	public static DecorationVisualManager Instance { get; private set; }

	public Color EdgeColor => _edgeColor;

	private void OnEnable()
	{
		Instance = this;
		_targets.Clear();
		if (_debugObjectTargets != null)
		{
			foreach (DecorationObjectDissolveTarget debugObjectTarget in _debugObjectTargets)
			{
				Register(debugObjectTarget);
			}
		}
		if (_debugSkinTargets != null)
		{
			foreach (DecorationSkinDissolveTarget debugSkinTarget in _debugSkinTargets)
			{
				Register(debugSkinTarget);
			}
		}
		HideDirector(_directorOut);
		HideDirector(_directorIn);
	}

	private static void HideDirector(PlayableDirector director)
	{
		if (director != null)
		{
			director.Stop();
			director.gameObject.SetActive(false);
		}
	}

	private void Start()
	{
		if (_targets == null)
		{
			return;
		}
		foreach (IDissolveTarget target in _targets)
		{
			target.SetProgress(0f, _edgeColor);
		}
	}

	private void OnDisable()
	{
		if (Instance == this)
		{
			Instance = null;
		}
	}

	public async UniTask PlayTransitionOut()
	{
		_directorOut.gameObject.SetActive(true);
		_directorOut.Play();
		await UniTask.Delay(2000, false, PlayerLoopTiming.Update);
	}

	public void StopTransitionOut()
	{
		_directorOut.Stop();
		_directorOut.gameObject.SetActive(false);
	}

	public async UniTask PlayTransitionIn()
	{
		_directorIn.gameObject.SetActive(true);
		_directorIn.Play();
		await UniTask.Delay(3000, false, PlayerLoopTiming.Update);
	}

	public void StopTransitionIn()
	{
		_directorIn.Stop();
		_directorIn.gameObject.SetActive(false);
	}

	public void ResetTransitions()
	{
		StopTransitionOut();
		StopTransitionIn();
	}

	public void SetAllProgress(float progress)
	{
		if (!_dissolveEnabled)
		{
			progress = 0f;
		}
		if (_targets == null)
		{
			return;
		}
		foreach (IDissolveTarget target in _targets)
		{
			target.SetProgress(progress, _edgeColor);
		}
	}

	public bool IsRegistered(IDissolveTarget target)
	{
		return _targets.Contains(target);
	}

	public void Register(IDissolveTarget target)
	{
		if (!_targets.Contains(target))
		{
			_targets.Add(target);
		}
	}

	public void Unregister(IDissolveTarget target)
	{
		_targets.Remove(target);
	}

	public void ClearTargets()
	{
		_targets.Clear();
	}
}
