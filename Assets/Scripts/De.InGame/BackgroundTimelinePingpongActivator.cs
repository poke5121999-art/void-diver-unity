using UnityEngine;
using UnityEngine.Playables;

public class BackgroundTimelinePingpongActivator : BackgroundTriggerActivator
{
	[SerializeField]
	private PlayableDirector _playableDirector;

	[SerializeField]
	private float _playSpeed = 1f;

	private bool _isAction;

	private bool _graphReady;

	protected override void Start()
	{
		if (!(_playableDirector == null))
		{
			_playableDirector.timeUpdateMode = DirectorUpdateMode.Manual;
			_playableDirector.extrapolationMode = DirectorWrapMode.Hold;
			_playableDirector.time = 0.0;
			_playableDirector.Evaluate();
			_graphReady = _playableDirector.playableGraph.IsValid();
		}
	}

	private void Update()
	{
		if (_playableDirector == null || !_graphReady)
		{
			return;
		}
		_isAction = TriggerUnits.Count != 0;
		double duration = _playableDirector.duration;
		if (!(duration <= 0.0))
		{
			float num = (_isAction ? 1f : (-1f));
			double num2 = _playableDirector.time + (double)(_playSpeed * num * Time.deltaTime);
			if (num2 <= 0.0)
			{
				num2 = 0.0;
			}
			else if (duration <= num2)
			{
				num2 = duration;
			}
			_playableDirector.time = num2;
			_playableDirector.Evaluate();
		}
	}
}
