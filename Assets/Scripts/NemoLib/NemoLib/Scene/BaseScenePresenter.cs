using NemoLib.Mvp;
using NemoLib.Popup;
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

		private bool IsGamePad => false;

		public bool IsInputEnable { get; private set; }

		protected CompositeDisposable InputDisposable { get; private set; }

		protected override void Awake()
		{
		}

		protected virtual void Start()
		{
		}

		protected virtual void OnDestroy()
		{
		}

		private void RefreshSceneSelection()
		{
		}

		private void SetInputEnable(bool enable)
		{
		}

		private void OnControlDeviceChanged(EControlDevice controlDevice)
		{
		}

		private void OnAllPopupsClosed(Unit unit)
		{
		}

		private void OnPopupOpened()
		{
		}

		protected virtual void OnInputEnabled()
		{
		}

		protected virtual void OnInputDisabled()
		{
		}
	}
}
