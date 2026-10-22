using System;
using UnityEngine.InputSystem;

namespace UniRx
{
	public static class Extensions
	{
		public static IObservable<TKey> ObserveChange<TKey, TValue>(this IReadOnlyReactiveDictionary<TKey, TValue> reactiveDictionary)
		{
			return null;
		}

		public static IObservable<Unit> ObserveChange<T>(this IReactiveCollection<T> reactiveCollection)
		{
			return null;
		}

		public static IObservable<Unit> ObserveChange<T>(this IReadOnlyReactiveCollection<T> reactiveCollection)
		{
			return null;
		}

		public static void AddTo<T>(this T disposable, CompositeDisposable compositeDisposable) where T : IDisposable
		{
		}

		public static IObservable<InputAction.CallbackContext> OnClickAsObservable(this InputAction inputAction)
		{
			return null;
		}

		public static IObservable<InputAction.CallbackContext> OnPressStartAsObservable(this InputAction inputAction)
		{
			return null;
		}

		public static IObservable<InputAction.CallbackContext> OnPressEndAsObservable(this InputAction inputAction)
		{
			return null;
		}
	}
}
