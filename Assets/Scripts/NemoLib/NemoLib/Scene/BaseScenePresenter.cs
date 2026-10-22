using NemoLib.Mvp;
using NemoLib.Popup;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace NemoLib.Scene
{
	public abstract class BaseScenePresenter : Presenter
	{
		[SerializeField]
		private Canvas _canvas;

		[Inject]
		protected CameraManager CameraManager;

		[Inject]
		protected PopupManager PopupManager;

		[Inject]
		protected SceneManager SceneManager;

		[Inject]
		protected InputManager InputManager;

		protected abstract Selectable SelectedObject { get; }

		private bool IsGamePad => InputManager.RxControlDevice.Value == EControlDevice.GamePad;

		public bool IsInputEnable { get; private set; }

		protected CompositeDisposable InputDisposable { get; private set; }

		protected override void Awake()
		{
			base.Awake();
			_canvas.worldCamera = CameraManager.UiCamera;
			InputManager.RxControlDevice.Subscribe(OnControlDeviceChanged).AddTo(this);
			PopupManager.OnAllPopupsClosed.Subscribe(OnAllPopupsClosed).AddTo(this);
			PopupManager.OnPopupOpened.Subscribe(delegate
			{
				OnPopupOpened();
			}).AddTo(this);
		}

		protected virtual void Start()
		{
			SetInputEnable(!PopupManager.ExistPopup());
			SceneManager.OnSceneStarted(this);
		}

		protected virtual void OnDestroy()
		{
			if (IsInputEnable)
			{
				IsInputEnable = false;
				OnInputDisabled();
			}
		}

		private void RefreshSceneSelection()
		{
			if (PopupManager.ExistPopup())
			{
				return;
			}
			InputManager.SelectObject(IsGamePad && SelectedObject != null ? SelectedObject.gameObject : null);
		}

		private void SetInputEnable(bool enable)
		{
			if (IsInputEnable != enable)
			{
				IsInputEnable = enable;
				if (enable)
				{
					OnInputEnabled();
				}
				else
				{
					OnInputDisabled();
				}
			}
		}

		private void OnControlDeviceChanged(EControlDevice controlDevice)
		{
			if (PopupManager.ExistPopup())
			{
				return;
			}
			InputManager.SelectObject(IsGamePad && SelectedObject != null ? SelectedObject.gameObject : null);
		}

		private void OnAllPopupsClosed(Unit unit)
		{
			if (!PopupManager.ExistInputActionPopup())
			{
				if (!IsInputEnable)
				{
					IsInputEnable = true;
					OnInputEnabled();
				}
				RefreshSceneSelection();
			}
		}

		private void OnPopupOpened()
		{
			if (PopupManager.ExistInputActionPopup() && IsInputEnable)
			{
				IsInputEnable = false;
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
		}
	}
}
