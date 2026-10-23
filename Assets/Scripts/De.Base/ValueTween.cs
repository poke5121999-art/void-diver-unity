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
		_valueText = GetComponent<TextMeshProUGUI>();
		_valueText.text = string.Format("{0:N0}", _value);
	}

	private void OnDestroy()
	{
		_tweener?.Kill();
	}

	public long GetValue()
	{
		return _value;
	}

	public void SetValue(long value, bool applyImmediately = false)
	{
		if (_valueText == null)
		{
			_value = value;
			return;
		}
		_tweener?.Kill();
		if (applyImmediately)
		{
			_value = value;
			_valueText.text = string.Format("{0:N0}", _value);
			return;
		}
		_tweener = DOTween.To(() => _value, x =>
		{
			_value = x;
			_valueText.text = string.Format("{0:N0}", _value);
		}, value, 0.5f);
	}
}
