using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class UITextShowOnEnable : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CShow_CO_003Ed__13 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public UITextShowOnEnable _003C_003E4__this;

		private int _003Ci_003E5__2;

		private float _003CtimeRate_003E5__3;

		private string _003CtempText_003E5__4;

		private WaitForEndOfFrame _003CwaitForEndOfFrame_003E5__5;

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
		public _003CShow_CO_003Ed__13(int _003C_003E1__state)
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

	private TextMeshProUGUI _text;

	private string _originText;

	private IEnumerator _show_co;

	[Header("UI Text Show Up Setting")]
	[SerializeField]
	private float _typingSpeed;

	private string RandChars => null;

	private string RandCharsKor => null;

	private void Awake()
	{
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	private char GetRandomChar()
	{
		return '\0';
	}

	private char GetRandomKorChar()
	{
		return '\0';
	}

	[IteratorStateMachine(typeof(_003CShow_CO_003Ed__13))]
	private IEnumerator Show_CO()
	{
		return null;
	}
}
