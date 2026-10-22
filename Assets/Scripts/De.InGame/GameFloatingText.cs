using System;
using NemoLib.AssetManagement;
using NemoLib.Log;
using SomeNet.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class GameFloatingText : MonoBase
{
	[SerializeField]
	private TextMeshProUGUI _text;

	[SerializeField]
	private Image _iconFront;

	[SerializeField]
	private Image _iconBack;

	[SerializeField]
	private float _duration = 0.5f;

	[SerializeField]
	[Range(0f, 3f)]
	private float _randomOffsetY;

	[SerializeField]
	[Range(0f, 3f)]
	private float _randomOffsetX;

	[Inject]
	private SeriLogger _logger;

	public void Init(string text, Vector3 position, bool showFrontIcon = false, bool showBackwardIcon = false)
	{
		base.transform.localScale = Vector3.one;
		base.transform.localRotation = Quaternion.identity;
		base.transform.position = position + GetRandomPositionOffset();
		_text.text = text;
		if (_iconFront != null)
		{
			_iconFront.gameObject.SetActive(showFrontIcon);
		}
		if (_iconBack != null)
		{
			_iconBack.gameObject.SetActive(showBackwardIcon);
		}
		GameObject gameObject = base.gameObject;
		float duration = _duration;
		if (gameObject == null)
		{
			return;
		}
		PoolableObject component = gameObject.GetComponent<PoolableObject>();
		if (component != null)
		{
			component.Despawn(duration);
		}
		else
		{
			UnityEngine.Object.Destroy(gameObject, duration);
		}
	}

	private Vector3 GetRandomPositionOffset()
	{
		float num = Mathf.Cos(MathF.PI / 4f) * _randomOffsetX;
		float num2 = Mathf.Sin(MathF.PI / 4f) * _randomOffsetX;
		return new Vector3(RandomUtility.GetRandomFloat(0f - num, num), RandomUtility.GetRandomFloat(0f, _randomOffsetY), RandomUtility.GetRandomFloat(0f - num2, num2));
	}
}
