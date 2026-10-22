using UnityEngine;
using UnityEngine.Playables;

public class BackgroundTimelinePingpongActivator : BackgroundTriggerActivator
{
	[SerializeField]
	private PlayableDirector _playableDirector;

	[SerializeField]
	private float _playSpeed;

	private bool _isAction;

	private bool _graphReady;

	protected override void Start()
	{
	}

	private void Update()
	{
	}
}
