using UniRx;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using VContainer;

public class VideoPlayerHandler : MonoBase
{
	[SerializeField]
	private VideoPlayer _videoPlayer;

	[SerializeField]
	private RawImage _rawImage;

	[Inject]
	private SoundSettingService _soundSettings;

	public Subject<VideoPlayer> RxVideoEnded { get; } = new Subject<VideoPlayer>();

	private void Start()
	{
		_rawImage.enabled = false;
		_videoPlayer.Prepare();
		_videoPlayer.prepareCompleted += OnVideoPrepared;
		_videoPlayer.loopPointReached += OnVideoEnded;
		UpdateVolume();
		_soundSettings.OnBgmVolumeApplied.Subscribe(delegate
		{
			UpdateVolume();
		}).AddTo(this);
	}

	private void OnDestroy()
	{
		_videoPlayer.prepareCompleted -= OnVideoPrepared;
		_videoPlayer.loopPointReached -= OnVideoEnded;
	}

	private void OnVideoPrepared(VideoPlayer videoPlayer)
	{
		_rawImage.enabled = true;
		_videoPlayer.Play();
	}

	private void OnVideoEnded(VideoPlayer videoPlayer)
	{
		RxVideoEnded.OnNext(videoPlayer);
	}

	private void UpdateVolume()
	{
		float appliedBgmVolume = _soundSettings.AppliedBgmVolume;
		for (ushort num = 0; num < _videoPlayer.audioTrackCount; num++)
		{
			_videoPlayer.SetDirectAudioVolume(num, appliedBgmVolume);
		}
	}
}
