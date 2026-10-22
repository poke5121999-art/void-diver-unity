using System;
using UnityEngine.InputSystem;

namespace UniRx
{
	public static class Extensions
	{
		public static IObservable<TKey> ObserveChange<TKey, TValue>(this IReadOnlyReactiveDictionary<TKey, TValue> reactiveDictionary)
		{
			// TODO(decomp): generic method has no pseudo-C; merging add/remove/replace keys is assumed
			return Observable.Merge(
				reactiveDictionary.ObserveAdd().Select((DictionaryAddEvent<TKey, TValue> e) => e.Key),
				reactiveDictionary.ObserveRemove().Select((DictionaryRemoveEvent<TKey, TValue> e) => e.Key),
				reactiveDictionary.ObserveReplace().Select((DictionaryReplaceEvent<TKey, TValue> e) => e.Key));
		}

		public static IObservable<Unit> ObserveChange<T>(this IReactiveCollection<T> reactiveCollection)
		{
			// TODO(decomp): generic method has no pseudo-C; the set of merged events is assumed
			return Observable.Merge(
				reactiveCollection.ObserveAdd().AsUnitObservable(),
				reactiveCollection.ObserveRemove().AsUnitObservable(),
				reactiveCollection.ObserveReplace().AsUnitObservable(),
				reactiveCollection.ObserveMove().AsUnitObservable(),
				reactiveCollection.ObserveReset());
		}

		public static IObservable<Unit> ObserveChange<T>(this IReadOnlyReactiveCollection<T> reactiveCollection)
		{
			// TODO(decomp): generic method has no pseudo-C; the set of merged events is assumed
			return Observable.Merge(
				reactiveCollection.ObserveAdd().AsUnitObservable(),
				reactiveCollection.ObserveRemove().AsUnitObservable(),
				reactiveCollection.ObserveReplace().AsUnitObservable(),
				reactiveCollection.ObserveMove().AsUnitObservable(),
				reactiveCollection.ObserveReset());
		}

		public static void AddTo<T>(this T disposable, CompositeDisposable compositeDisposable) where T : IDisposable
		{
			// TODO(decomp): generic method has no pseudo-C; CompositeDisposable.Add assumed
			compositeDisposable.Add(disposable);
		}

		public static IObservable<InputAction.CallbackContext> OnClickAsObservable(this InputAction inputAction)
		{
			return Observable.FromEvent<Action<InputAction.CallbackContext>, InputAction.CallbackContext>((Action<InputAction.CallbackContext> h) => h, delegate(Action<InputAction.CallbackContext> h)
			{
				inputAction.performed += h;
			}, delegate(Action<InputAction.CallbackContext> h)
			{
				inputAction.performed -= h;
			});
		}

		public static IObservable<InputAction.CallbackContext> OnPressStartAsObservable(this InputAction inputAction)
		{
			return Observable.FromEvent<Action<InputAction.CallbackContext>, InputAction.CallbackContext>((Action<InputAction.CallbackContext> h) => h, delegate(Action<InputAction.CallbackContext> h)
			{
				inputAction.started += h;
			}, delegate(Action<InputAction.CallbackContext> h)
			{
				inputAction.started -= h;
			});
		}

		public static IObservable<InputAction.CallbackContext> OnPressEndAsObservable(this InputAction inputAction)
		{
			return Observable.FromEvent<Action<InputAction.CallbackContext>, InputAction.CallbackContext>((Action<InputAction.CallbackContext> h) => h, delegate(Action<InputAction.CallbackContext> h)
			{
				inputAction.canceled += h;
			}, delegate(Action<InputAction.CallbackContext> h)
			{
				inputAction.canceled -= h;
			});
		}
	}
}
