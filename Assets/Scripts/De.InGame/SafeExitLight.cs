public class SafeExitLight : TweenLight
{
	private SafeExit _safeExit;

	private bool _isOn;

	private void Start()
	{
		_safeExit = GetComponentInParent<SafeExit>();
	}

	private void Update()
	{
		bool flag = _safeExit.CanInteract(null);
		if (flag)
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
