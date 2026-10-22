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

	public Subject<VideoPlayer> RxVideoEnded { get; }

	private void Start()
	{
	}

	private void OnDestroy()
	{
	}

	private void OnVideoPrepared(VideoPlayer videoPlayer)
	{
	}

	private void OnVideoEnded(VideoPlayer videoPlayer)
	{
	}

	private void UpdateVolume()
	{
	}
}
