using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class UIMotionOnEnable : MonoBehaviour
{
	[Header("Motion Setting")]
	[SerializeField]
	private float _motionTime = 0.1f;

	[SerializeField]
	private AnimationCurve _motionCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

	[SerializeField]
	private Vector3 _motionOffsetPosition = Vector3.zero;

	[SerializeField]
	private Quaternion _motionOffsetRotation = Quaternion.Euler(Vector3.zero);

	[SerializeField]
	private Vector3 _motionOffsetScale = Vector3.one;

	[Space(10f)]
	[SerializeField]
	private bool _isUsingStartColor;

	[SerializeField]
	private Color _motionStartColor = Color.white;

	private IEnumerator _motion_co;

	private RectTransform _rectTransform;

	private Vector3 _originLocalPosition;

	private Quaternion _originLocalRotation;

	private Vector3 _originLocalScale;

	private Graphic _graphic;

	private Color _originColor;

	private void Awake()
	{
		_rectTransform = GetComponent<RectTransform>();
		if ((bool)_rectTransform)
		{
			_originLocalPosition = _rectTransform.localPosition;
			_originLocalRotation = _rectTransform.localRotation;
			_originLocalScale = _rectTransform.localScale;
		}
		_graphic = GetComponent<Graphic>();
		if ((bool)_graphic)
		{
			_originColor = _graphic.color;
		}
	}

	private void OnEnable()
	{
		if (_motion_co != null)
		{
			StopCoroutine(_motion_co);
		}
		_motion_co = Motion_CO();
		StartCoroutine(_motion_co);
	}

	private IEnumerator Motion_CO()
	{
		float timeRate = 0f;
		WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
		float filteredMotionTime = Mathf.Max(_motionTime, Mathf.Epsilon);
		Vector3 startLocalPosition = _motionOffsetPosition + _originLocalPosition;
		Quaternion startLocalRotation = _motionOffsetRotation * _originLocalRotation;
		Vector3 startLocalScale = Vector3.Scale(_motionOffsetScale, _originLocalScale);
		Color startColor = _originColor;
		if (_isUsingStartColor)
		{
			startColor = _motionStartColor;
		}
		while (timeRate < 1f)
		{
			timeRate += Time.deltaTime / filteredMotionTime;
			float t = _motionCurve.Evaluate(timeRate);
			_rectTransform.localPosition = Vector3.Lerp(startLocalPosition, _originLocalPosition, t);
			_rectTransform.localRotation = Quaternion.LerpUnclamped(startLocalRotation, _originLocalRotation, t);
			_rectTransform.localScale = Vector3.Lerp(startLocalScale, _originLocalScale, t);
			if (_isUsingStartColor)
			{
				_graphic.color = Color.Lerp(startColor, _motionStartColor, t);
			}
			yield return waitForEndOfFrame;
		}
		_rectTransform.localPosition = _originLocalPosition;
		_rectTransform.localRotation = _originLocalRotation;
		_rectTransform.localScale = _originLocalScale;
		if (_isUsingStartColor)
		{
			_graphic.color = _motionStartColor;
		}
	}
}
