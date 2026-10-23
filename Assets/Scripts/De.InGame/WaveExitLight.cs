using GameCommon.Enum;
public class WaveExitLight : TweenLight
{
	private WaveExit _waveExit;

	private bool _isOn;

	private void Start()
	{
		_waveExit = GetComponentInParent<WaveExit>();
	}

	private void Update()
	{
		if (_waveExit.ExitState == EExitState.Finished)
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
