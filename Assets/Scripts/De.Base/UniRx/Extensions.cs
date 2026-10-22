using System;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UniRx
{
	public static class Extensions
	{
		public static IObservable<T> WithSfx<T>(this IObservable<T> source, string sfx = "ButtonClick")
		{
			return null;
		}

		public static IObservable<Unit> OnClick(this Button button)
		{
			return null;
		}

		public static IObservable<Unit> OnClickWithSfx(this Button button, string sfx = "ButtonClick")
		{
			return null;
		}

		public static IObservable<PointerEventData> OnLeftClickWithSfx(this Button button, string sfx = "ButtonClick")
		{
			return null;
		}

		public static IObservable<PointerEventData> OnRightClickWithSfx(this Button button, string sfx = "ButtonClick")
		{
			return null;
		}

		public static IObservable<InputAction.CallbackContext> OnClickWithSfx(this InputAction inputAction, string sfx = "ButtonClick")
		{
			return null;
		}

		public static IObservable<bool> OnValueChangedWithSfx(this Toggle toggle)
		{
			return null;
		}

		public static IObservable<bool> OnValueTrueWithSfx(this Toggle toggle)
		{
			return null;
		}

		public static IObservable<float> OnValueChangedWithSfx(this Slider slider)
		{
			return null;
		}

		public static IObservable<BaseEventData> OnSubmit(this TMP_InputField inputField)
		{
			return null;
		}

		public static IObservable<BaseEventData> OnSubmitWithSfx(this TMP_InputField inputField, string sfx = "ButtonClick")
		{
			return null;
		}
	}
}
