using DarkTonic.MasterAudio;
using UnityEngine;
using VContainer.Unity;

public class SoundManager : MonoBase
{
	[SerializeField]
	private Transform _listenerTf;

	[SerializeField]
	private bool _enable = true;

	private GameObject _followingTarget;

	private string _currentBgmName;

	private bool _isPaused;

	private bool _isReady;

	public static SoundManager Instance { get; private set; }

	protected override void Awake()
	{
		this.InjectFromRootContainer();
		Object.DontDestroyOnLoad(this);
		Instance = this;
	}

	private void Start()
	{
		_isReady = true;
	}

	private void Update()
	{
		if (_followingTarget != null)
		{
			_listenerTf.position = _followingTarget.transform.position;
		}
	}

	private void OnApplicationFocus(bool hasFocus)
	{
		if (!_isReady)
		{
			return;
		}
		if (hasFocus)
		{
			if (_isPaused)
			{
				MasterAudio.UnpauseEverything();
				_isPaused = false;
			}
		}
		else if (!_isPaused)
		{
			MasterAudio.PauseEverything();
			_isPaused = true;
		}
	}

	public bool IsBgmPlaying()
	{
		return !string.IsNullOrEmpty(_currentBgmName);
	}

	public void PlayBgm(string bgmName)
	{
		if (_enable && _currentBgmName != bgmName)
		{
			MasterAudio.StartPlaylist(bgmName);
			_currentBgmName = bgmName;
		}
	}

	public void SetBgmVolume(float volume)
	{
		// inlined: MasterAudio.PlaylistMasterVolume setter (store volume, UpdateMasterVolume on every PlaylistController)
		MasterAudio.PlaylistMasterVolume = volume;
	}

	public void PauseBgm()
	{
		MasterAudio.PausePlaylist();
	}

	public void UnpauseBgm()
	{
		MasterAudio.UnpausePlaylist();
	}

	public void StopBgm()
	{
		if (!string.IsNullOrEmpty(_currentBgmName))
		{
			MasterAudio.StopPlaylist();
			_currentBgmName = string.Empty;
		}
	}

	public void MuteBus(string busName)
	{
		MasterAudio.MuteBus(busName);
	}

	public void UnmuteBus(string busName)
	{
		MasterAudio.UnmuteBus(busName);
	}

	public void SetBusVolume(string busName, float volume)
	{
		MasterAudio.SetBusVolumeByName(busName, volume);
	}

	public PlaySoundResult PlaySfx3D(string sfxName, Transform targetTf)
	{
		if (!_enable)
		{
			return null;
		}
		return MasterAudio.PlaySound3DFollowTransform(sfxName, targetTf);
	}

	public PlaySoundResult PlaySfx3D(string sfxName, Vector3 position)
	{
		if (!_enable)
		{
			return null;
		}
		return MasterAudio.PlaySound3DAtVector3(sfxName, position);
	}

	public PlaySoundResult PlaySfx(string sfxName)
	{
		if (_enable && !string.IsNullOrEmpty(sfxName))
		{
			return MasterAudio.PlaySound3DAtTransform(sfxName, _listenerTf);
		}
		return null;
	}

	public void StopSfxAll(string sfxName)
	{
		if (string.IsNullOrEmpty(sfxName))
		{
			return;
		}
		var variations = MasterAudio.GetAllVariationsOfGroup(sfxName);
		if (variations == null)
		{
			return;
		}
		foreach (var audioInfo in variations)
		{
			audioInfo.Variation.Stop();
		}
	}

	public void StopSfxAll()
	{
		var playing = MasterAudio.GetAllPlayingVariations();
		if (playing == null || playing.Count == 0)
		{
			return;
		}
		foreach (SoundGroupVariation item in playing)
		{
			if (item != null)
			{
				item.Stop();
			}
		}
	}

	public void SetFollowingTarget(GameObject followingTarget)
	{
		_followingTarget = followingTarget;
		// The decomp loads Vector3.oneVector (not zero) when there is no target; kept as read.
		_listenerTf.position = (followingTarget != null) ? followingTarget.transform.position : Vector3.one;
	}
}
