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

	private List<PlayableBinding> _timelineTrack = new List<PlayableBinding>();

	protected override void Awake()
	{
		base.Awake();
		_playableDirector.stopped += OnDirectorStopped;
	}

	public void PlayTimeline()
	{
		_playableDirector.Play();
	}

	public void StopTimeline()
	{
		_playableDirector.Stop();
	}

	private void OnDirectorStopped(PlayableDirector director)
	{
		_gameCutsceneManager.FinishCutscene(this);
	}
}
