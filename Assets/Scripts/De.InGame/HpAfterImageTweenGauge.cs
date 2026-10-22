using UnityEngine;
using UnityEngine.UI;

public class HpAfterImageTweenGauge : AfterImageTweenGauge
{
	[SerializeField]
	private Image _shieldBar;

	private int _prevHp;

	private int _prevHpMax;

	private void OnDisable()
	{
	}

	public void SetHpWithGuard(int hp, int hpMax, int guard, bool forceImmediate = false)
	{
	}
}
