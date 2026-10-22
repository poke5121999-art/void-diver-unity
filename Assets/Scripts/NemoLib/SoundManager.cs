using DarkTonic.MasterAudio;
using UnityEngine;

public class SoundManager : MonoBase
{
	[SerializeField]
	private Transform _listenerTf;

	[SerializeField]
	private bool _enable;

	private GameObject _followingTarget;

	private string _currentBgmName;

	private bool _isPaused;

	private bool _isReady;

	public static SoundManager Instance { get; private set; }

	protected override void Awake()
	{
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnApplicationFocus(bool hasFocus)
	{
	}

	public bool IsBgmPlaying()
	{
		return false;
	}

	public void PlayBgm(string bgmName)
	{
	}

	public void SetBgmVolume(float volume)
	{
	}

	public void PauseBgm()
	{
	}

	public void UnpauseBgm()
	{
	}

	public void StopBgm()
	{
	}

	public void MuteBus(string busName)
	{
	}

	public void UnmuteBus(string busName)
	{
	}

	public void SetBusVolume(string busName, float volume)
	{
	}

	public PlaySoundResult PlaySfx3D(string sfxName, Transform targetTf)
	{
		return null;
	}

	public PlaySoundResult PlaySfx3D(string sfxName, Vector3 position)
	{
		return null;
	}

	public PlaySoundResult PlaySfx(string sfxName)
	{
		return null;
	}

	public void StopSfxAll(string sfxName)
	{
	}

	public void StopSfxAll()
	{
	}

	public void SetFollowingTarget(GameObject followingTarget)
	{
	}
}
