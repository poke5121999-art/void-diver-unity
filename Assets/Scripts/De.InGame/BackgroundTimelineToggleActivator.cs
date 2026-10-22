using UnityEngine;
using UnityEngine.Playables;

public class BackgroundTimelineToggleActivator : BackgroundTriggerActivator
{
	[SerializeField]
	private PlayableDirector _playableDirector;

	private bool _isAction;

	protected override void Start()
	{
	}

	protected override void UpdateSomething()
	{
	}
}
