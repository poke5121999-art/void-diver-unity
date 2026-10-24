using System;
using UnityEngine.InputSystem;

namespace UniRx
{
	public static class Extensions
	{
		// ObserveChange<int,object> @0x180c05ca0 (machine code checked): interface slots 4, 6, 7, 8 = ObserveAdd, ObserveRemove, ObserveReplace, ObserveReset, merged pairwise;
		// the reset event maps to default(TKey) (lambda b__0_3)
		public static IObservable<TKey> ObserveChange<TKey, TValue>(this IReadOnlyReactiveDictionary<TKey, TValue> reactiveDictionary)
		{
			return reactiveDictionary.ObserveAdd().Select((DictionaryAddEvent<TKey, TValue> e) => e.Key)
				.Merge(reactiveDictionary.ObserveRemove().Select((DictionaryRemoveEvent<TKey, TValue> e) => e.Key))
				.Merge(reactiveDictionary.ObserveReplace().Select((DictionaryReplaceEvent<TKey, TValue> e) => e.Key))
				.Merge(reactiveDictionary.ObserveReset().Select((Unit _) => default(TKey)));
		}

		// ObserveChange<int> @0x180c06500 (machine code checked): interface slots 2, 5, 7, 6 = ObserveAdd, ObserveRemove, ObserveReset, ObserveReplace (no ObserveMove), merged pairwise
		public static IObservable<Unit> ObserveChange<T>(this IReactiveCollection<T> reactiveCollection)
		{
			return reactiveCollection.ObserveAdd().Select((CollectionAddEvent<T> _) => Unit.Default)
				.Merge(reactiveCollection.ObserveRemove().Select((CollectionRemoveEvent<T> _) => Unit.Default))
				.Merge(reactiveCollection.ObserveReset().Select((Unit _) => Unit.Default))
				.Merge(reactiveCollection.ObserveReplace().Select((CollectionReplaceEvent<T> _) => Unit.Default));
		}

		// ObserveChange<object> @0x180c07540 (machine code checked): same sources and order as the IReactiveCollection overload
		public static IObservable<Unit> ObserveChange<T>(this IReadOnlyReactiveCollection<T> reactiveCollection)
		{
			return reactiveCollection.ObserveAdd().Select((CollectionAddEvent<T> _) => Unit.Default)
				.Merge(reactiveCollection.ObserveRemove().Select((CollectionRemoveEvent<T> _) => Unit.Default))
				.Merge(reactiveCollection.ObserveReset().Select((Unit _) => Unit.Default))
				.Merge(reactiveCollection.ObserveReplace().Select((CollectionReplaceEvent<T> _) => Unit.Default));
		}

		// AddTo<object> @0x180c01660 (machine code checked): a null CompositeDisposable disposes the value instead
		public static void AddTo<T>(this T disposable, CompositeDisposable compositeDisposable) where T : IDisposable
		{
			if (compositeDisposable == null)
			{
				((IDisposable)disposable)?.Dispose();
			}
			else
			{
				compositeDisposable.Add(disposable);
			}
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
