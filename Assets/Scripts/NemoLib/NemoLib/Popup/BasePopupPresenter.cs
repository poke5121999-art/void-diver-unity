using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using NemoLib.Log;
using NemoLib.Mvp;
using UniRx;
using UnityEngine;
using VContainer;

namespace NemoLib.Popup
{
	public abstract class BasePopupPresenter : Presenter
	{
		[Inject]
		protected InputManager InputManager;

		[Inject]
		protected SeriLogger Logger;

		private UniTaskCompletionSource<object> _source = new UniTaskCompletionSource<object>();

		private readonly List<GameObject> _initialActiveChildren = new List<GameObject>();

		private List<GameObject> _childrenObjects = new List<GameObject>();

		public bool IsBlurEffect = true;

		private bool _isPopupOpened;

		private bool _isPopupClosing;

		public Guid Guid { get; } = Guid.NewGuid();

		public UniTask<object> Task => _source.Task;

		public Subject<BasePopupPresenter> OnClose { get; } = new Subject<BasePopupPresenter>();

		public abstract bool IsInputActionPopup { get; }

		public virtual string OpenSfx => null;

		public virtual string CloseSfx => null;

		protected bool IsInputEnable { get; private set; }

		protected bool IsGamePad => InputManager.RxControlDevice.Value == EControlDevice.GamePad;

		protected CompositeDisposable OpenDisposable { get; private set; }

		protected CompositeDisposable InputDisposable { get; private set; }

		protected override void Awake()
		{
			base.Awake();
			foreach (Transform item in base.gameObject.transform)
			{
				if (item.gameObject.activeSelf)
				{
					item.gameObject.SetActive(value: false);
					_childrenObjects.Add(item.gameObject);
					_initialActiveChildren.Add(item.gameObject);
				}
			}
		}

		internal void ResetForReuse()
		{
			_isPopupOpened = false;
			_isPopupClosing = false;
			_source = new UniTaskCompletionSource<object>();
			OpenDisposable?.Dispose();
			OpenDisposable = new CompositeDisposable();
			_childrenObjects = new List<GameObject>(_initialActiveChildren);
			foreach (GameObject childrenObject in _childrenObjects)
			{
				if (childrenObject)
				{
					childrenObject.SetActive(value: false);
				}
			}
		}

		private void OnInject(Model model)
		{
		}

		public async UniTask OpenAsync()
		{
			await OnOpenAsync();
		}

		protected virtual async UniTask OnOpenAsync()
		{
			await UniTask.DelayFrame(1);
			_childrenObjects.ForEach(delegate(GameObject go)
			{
				if (go)
				{
					go.SetActive(value: true);
				}
			});
			_childrenObjects = null;
			_isPopupOpened = true;
		}

		public virtual void Close(object result = null)
		{
			if (_isPopupClosing)
			{
				return;
			}
			_isPopupClosing = true;
			CloseAsync().Forget();

			async UniTask CloseAsync()
			{
				await UniTask.WaitUntil(() => _isPopupOpened);
				_source.TrySetResult(result);
				OnClose.OnNext(this);
				OpenDisposable.Dispose();
				OpenDisposable = null;
			}
		}

		public void ActivatePopup()
		{
			IsInputEnable = true;
			if (!(InputManager == null))
			{
				InputManager.ClearSelectedPosition();
				OnInputEnabled();
			}
		}

		public void DeactivatePopup()
		{
			IsInputEnable = false;
			if (!(InputManager == null))
			{
				InputManager.ClearSelectedPosition();
				OnInputDisabled();
			}
		}

		protected virtual void OnInputEnabled()
		{
			InputDisposable?.Dispose();
			InputDisposable = new CompositeDisposable();
		}

		protected virtual void OnInputDisabled()
		{
			InputDisposable?.Dispose();
			InputDisposable = null;
		}
	}
}
