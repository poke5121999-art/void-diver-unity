using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class UIMotionOnEnable : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CMotion_CO_003Ed__16 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public UIMotionOnEnable _003C_003E4__this;

		private float _003CtimeRate_003E5__2;

		private WaitForEndOfFrame _003CwaitForEndOfFrame_003E5__3;

		private float _003CfilteredMotionTime_003E5__4;

		private Vector3 _003CstartLocalPosition_003E5__5;

		private Quaternion _003CstartLocalRotation_003E5__6;

		private Vector3 _003CstartLocalScale_003E5__7;

		private Color _003CstartColor_003E5__8;

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return null;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return null;
			}
		}

		[DebuggerHidden]
		public _003CMotion_CO_003Ed__16(int _003C_003E1__state)
		{
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
		}
	}

	[Header("Motion Setting")]
	[SerializeField]
	private float _motionTime;

	[SerializeField]
	private AnimationCurve _motionCurve;

	[SerializeField]
	private Vector3 _motionOffsetPosition;

	[SerializeField]
	private Quaternion _motionOffsetRotation;

	[SerializeField]
	private Vector3 _motionOffsetScale;

	[Space(10f)]
	[SerializeField]
	private bool _isUsingStartColor;

	[SerializeField]
	private Color _motionStartColor;

	private IEnumerator _motion_co;

	private RectTransform _rectTransform;

	private Vector3 _originLocalPosition;

	private Quaternion _originLocalRotation;

	private Vector3 _originLocalScale;

	private Graphic _graphic;

	private Color _originColor;

	private void Awake()
	{
	}

	private void OnEnable()
	{
	}

	[IteratorStateMachine(typeof(_003CMotion_CO_003Ed__16))]
	private IEnumerator Motion_CO()
	{
		return null;
	}
}
