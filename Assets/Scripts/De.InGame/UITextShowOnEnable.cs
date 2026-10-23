using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class UITextShowOnEnable : MonoBehaviour
{
	private TextMeshProUGUI _text;

	private string _originText;

	private IEnumerator _show_co;

	[Header("UI Text Show Up Setting")]
	[SerializeField]
	private float _typingSpeed = 0.0625f;

	private string RandChars => "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()-=_+[]{}|;:'\",.<>?/`~";

	private string RandCharsKor => "삶과죽음을가르는경계는본디희미하고모호한법.과연누가그것의끝과시작을말할수있으랴?";

	private void Awake()
	{
		_text = GetComponent<TextMeshProUGUI>();
	}

	private void OnEnable()
	{
		if (_show_co != null)
		{
			StopCoroutine(_show_co);
		}
		_show_co = Show_CO();
		StartCoroutine(_show_co);
	}

	private void OnDisable()
	{
		if ((bool)_text)
		{
			_text.text = _originText;
		}
	}

	private char GetRandomChar()
	{
		return RandChars[UnityEngine.Random.Range(0, RandChars.Length)];
	}

	private char GetRandomKorChar()
	{
		return RandCharsKor[UnityEngine.Random.Range(0, RandCharsKor.Length)];
	}

	private IEnumerator Show_CO()
	{
		if (_text == null)
		{
			yield break;
		}
		_originText = _text.text;
		int i = 0;
		float timeRate = 0f;
		string tempText = "";
		WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
		while (i < _originText.Length)
		{
			_text.text = string.Format("{0}{1}", tempText, GetRandomKorChar());
			while (timeRate > _typingSpeed)
			{
				timeRate -= _typingSpeed;
				tempText = string.Format("{0}{1}", tempText, _originText[i]);
				i++;
			}
			yield return waitForEndOfFrame;
			timeRate += Time.deltaTime;
		}
		_text.text = _originText;
	}
}
