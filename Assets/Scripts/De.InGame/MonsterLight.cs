using GameCommon.Enum;

public class MonsterLight : TweenLight
{
	private MonsterModel _monsterModel;

	private bool _isOn;

	public void Start()
	{
		_monsterModel = GetComponentInParent<MonsterModel>();
	}

	private void Update()
	{
		EActionState syncState = _monsterModel.SyncState;
		if (syncState == EActionState.Dying || syncState == EActionState.Dead || syncState == EActionState.ReservedDespawn)
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
