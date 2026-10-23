public class SkillExecutorLight : TweenLight
{
	private SkillExecutor _skillExecutor;

	private bool _isOn;

	private void Start()
	{
		_skillExecutor = GetComponentInParent<SkillExecutor>();
	}

	private void Update()
	{
		if (_skillExecutor.RemainCount < 1)
		{
			if (_isOn)
			{
				TurnOff();
				_isOn = false;
			}
		}
		else if (!_isOn)
		{
			TurnOn();
			_isOn = true;
		}
	}
}
