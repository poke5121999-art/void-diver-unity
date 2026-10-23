using System.Collections.Generic;
using Cinemachine;
using GameCommon.Enum;
using UniRx;
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

	private List<PlayableBinding> _timelineTrack = new List<PlayableBinding>();

	private GameChannel GameChannel => base.NetworkChannel as GameChannel;

	public override void OnNetworkSpawn()
	{
		base.OnNetworkSpawn();
		InitCinemachineBrain();
		_gameManager.RxStageState.Where((EStageState _) => _ == EStageState.Intro).Subscribe(delegate
		{
			PlayTimeline();
		}).AddTo(this);
	}

	private void InitCinemachineBrain()
	{
		foreach (PlayableBinding output in _playableDirector.playableAsset.outputs)
		{
			if (output.streamName == "Cinemachine Track" || output.outputTargetType == typeof(CinemachineBrain))
			{
				_timelineTrack.Add(output);
				_playableDirector.SetGenericBinding(output.sourceObject, _cameraManager.MainCamera.GetComponent<CinemachineBrain>());
			}
		}
	}

	public void OnIntroFinished()
	{
		_vCam.gameObject.SetActive(value: false);
		GameChannel.SendToHost(new ReqClientIntroFinished
		{
			ClientId = _gameManager.LocalClientId
		});
	}

	private void PlayTimeline()
	{
		_playableDirector.Play();
	}

	public void OnTimelineFinished()
	{
	}
}
