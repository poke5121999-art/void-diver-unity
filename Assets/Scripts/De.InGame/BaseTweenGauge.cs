using UnityEngine;

public abstract class BaseTweenGauge : MonoBehaviour
{
	public abstract void SetRatio(float ratio, bool immediate = false);
}
