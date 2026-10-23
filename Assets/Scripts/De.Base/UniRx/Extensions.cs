using System;
using TMPro;
using UniRx.Triggers;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UniRx
{
	public static class Extensions
	{
		// TODO(decomp): generic method has no pseudo-C in the binary; Do(PlaySfx) via SoundManager.Instance is assumed
		public static IObservable<T> WithSfx<T>(this IObservable<T> source, string sfx = "ButtonClick")
		{
			return source.Do(_ => SoundManager.Instance?.PlaySfx(sfx));
		}

		public static IObservable<Unit> OnClick(this Button button)
		{
			return button.onClick.AsObservable();
		}

		public static IObservable<Unit> OnClickWithSfx(this Button button, string sfx = "ButtonClick")
		{
			return button.onClick.AsObservable().WithSfx(sfx);
		}

		public static IObservable<PointerEventData> OnLeftClickWithSfx(this Button button, string sfx = "ButtonClick")
		{
			return button.OnPointerClickAsObservable().Where(e => e.button == PointerEventData.InputButton.Left).WithSfx(sfx);
		}

		public static IObservable<PointerEventData> OnRightClickWithSfx(this Button button, string sfx = "ButtonClick")
		{
			return button.OnPointerClickAsObservable().Where(e => e.button == PointerEventData.InputButton.Right).WithSfx(sfx);
		}

		public static IObservable<InputAction.CallbackContext> OnClickWithSfx(this InputAction inputAction, string sfx = "ButtonClick")
		{
			return inputAction.OnClickAsObservable().WithSfx(sfx);
		}

		public static IObservable<bool> OnValueChangedWithSfx(this Toggle toggle)
		{
			return toggle.OnValueChangedAsObservable().Skip(1).WithSfx("ButtonClick");
		}

		public static IObservable<bool> OnValueTrueWithSfx(this Toggle toggle)
		{
			return toggle.OnValueChangedAsObservable().Skip(1).Where(isOn => isOn).WithSfx("ButtonClick");
		}

		public static IObservable<float> OnValueChangedWithSfx(this Slider slider)
		{
			return slider.OnValueChangedAsObservable().Skip(1).WithSfx("ButtonClick");
		}

		public static IObservable<BaseEventData> OnSubmit(this TMP_InputField inputField)
		{
			return inputField.OnSubmitAsObservable();
		}

		public static IObservable<BaseEventData> OnSubmitWithSfx(this TMP_InputField inputField, string sfx = "ButtonClick")
		{
			return inputField.OnSubmitAsObservable().WithSfx(sfx);
		}
	}
}
