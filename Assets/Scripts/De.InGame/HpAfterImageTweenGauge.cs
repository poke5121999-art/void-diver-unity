using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HpAfterImageTweenGauge : AfterImageTweenGauge
{
	[SerializeField]
	private Image _shieldBar;

	private int _prevHp = int.MinValue;

	private int _prevHpMax = int.MinValue;

	private void OnDisable()
	{
		if ((bool)frontBar)
		{
			DOTween.Kill(frontBar);
		}
		if ((bool)backBar)
		{
			DOTween.Kill(backBar);
		}
		if ((bool)_shieldBar)
		{
			DOTween.Kill(_shieldBar);
		}
	}

	public void SetHpWithGuard(int hp, int hpMax, int guard, bool forceImmediate = false)
	{
		if (hpMax <= 0)
		{
			SetRatio(0f, immediate: true);
			if ((bool)_shieldBar)
			{
				_shieldBar.fillAmount = 0f;
			}
			_prevHp = hp;
		}
		else
		{
			int num = Mathf.Max(hp, 0);
			int num2 = Mathf.Max(guard, 0);
			int num3 = num + num2;
			float num4;
			float num5;
			if (num3 > hpMax)
			{
				num4 = ((num3 > 0) ? ((float)num / (float)num3) : 0f);
				num5 = 1f;
			}
			else
			{
				num4 = Mathf.Clamp01((float)num / (float)hpMax);
				num5 = Mathf.Clamp01((float)num3 / (float)hpMax);
			}
			bool flag = num != _prevHp || hpMax != _prevHpMax;
			bool immediate = forceImmediate || (!flag && num5 >= 1f);
			SetRatio(num4, immediate);
			if (_shieldBar != null)
			{
				_shieldBar.fillAmount = ((num2 > 0) ? num5 : 0f);
			}
			_prevHp = num;
		}
		_prevHpMax = hpMax;
	}
}
