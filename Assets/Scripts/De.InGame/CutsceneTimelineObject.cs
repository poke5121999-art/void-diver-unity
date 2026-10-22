using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Playables;
using VContainer;

public class CutsceneTimelineObject : MonoBase
{
	[SerializeField]
	private PlayableDirector _playableDirector;

	[SerializeField]
	private CinemachineVirtualCamera _vCam;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private CameraManager _cameraManager;

	[Inject]
	private GameCutsceneManager _gameCutsceneManager;

	private List<PlayableBinding> _timelineTrack;

	protected override void Awake()
	{
	}

	public void PlayTimeline()
	{
	}

	public void StopTimeline()
	{
	}

	private void OnDirectorStopped(PlayableDirector director)
	{
	}
}
