using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Playables;
using VContainer;

public class IntroTimelineObject : NetworkBase
{
	[SerializeField]
	private PlayableDirector _playableDirector;

	[SerializeField]
	private CinemachineVirtualCamera _vCam;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private CameraManager _cameraManager;

	private List<PlayableBinding> _timelineTrack;

	private GameChannel GameChannel => null;

	public override void OnNetworkSpawn()
	{
	}

	private void InitCinemachineBrain()
	{
	}

	public void OnIntroFinished()
	{
	}

	private void PlayTimeline()
	{
	}

	public void OnTimelineFinished()
	{
	}



}
