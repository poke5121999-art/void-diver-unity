using DG.Tweening;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ValueTween : MonoBehaviour
{
	private const float TWEEN_DURATION = 0.5f;

	private Tweener _tweener;

	private long _value;

	private TextMeshProUGUI _valueText;

	private void Start()
	{
	}

	private void OnDestroy()
	{
	}

	public long GetValue()
	{
		return 0L;
	}

	public void SetValue(long value, bool applyImmediately = false)
	{
	}
}
