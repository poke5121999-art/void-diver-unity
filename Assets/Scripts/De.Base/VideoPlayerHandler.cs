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

#if UNITY_WEBGL && !UNITY_EDITOR
	private bool _ended;
#endif

	private void Start()
	{
		_rawImage.enabled = false;
#if UNITY_WEBGL && !UNITY_EDITOR
		// The web player cannot play a VideoClip from a bundle; WebGLBuild copies the .webm files next to the build
		// (StreamingAssets/video/<clip>.webm) and the clip plays from there. Browsers only autoplay a muted video, so
		// the video's own sound is off. A video that fails, or does not end, still lets the scene go on.
		if (_videoPlayer.source == VideoSource.VideoClip && _videoPlayer.clip != null)
		{
			_videoPlayer.url = Application.streamingAssetsPath + "/video/" + _videoPlayer.clip.name + ".webm";
			_videoPlayer.source = VideoSource.Url;
		}
		_videoPlayer.audioOutputMode = VideoAudioOutputMode.None;
		_videoPlayer.errorReceived += delegate(VideoPlayer source, string message)
		{
			Debug.LogWarning("[VideoPlayerHandler] " + source.url + ": " + message);
			OnVideoEnded(source);
		};
		if (!_videoPlayer.isLooping)
		{
			Observable.Timer(System.TimeSpan.FromSeconds(30)).Subscribe(delegate
			{
				OnVideoEnded(_videoPlayer);
			}).AddTo(this);
		}
#endif
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
#if UNITY_WEBGL && !UNITY_EDITOR
		if (_ended && !videoPlayer.isLooping)
		{
			return;
		}
		_ended = true;
#endif
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
