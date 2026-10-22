using UnityEngine;
using UnityEngine.Playables;

public class BackgroundTimelineToggleActivator : BackgroundTriggerActivator
{
	[SerializeField]
	private PlayableDirector _playableDirector;

	private bool _isAction;

	protected override void Start()
	{
		if ((bool)_playableDirector)
		{
			_playableDirector.time = 0.0;
			_playableDirector.Stop();
		}
	}

	protected override void UpdateSomething()
	{
		_isAction = TriggerUnits.Count != 0;
		if (!_isAction)
		{
			if (_playableDirector != null)
			{
				_playableDirector.Pause();
			}
		}
		else if (_playableDirector != null)
		{
			_playableDirector.Play();
		}
	}
}
