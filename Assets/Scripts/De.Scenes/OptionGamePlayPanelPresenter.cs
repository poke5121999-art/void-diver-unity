using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using GameCommon.Enum;
using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using UnityEngine;
using VContainer;

public class OptionGamePlayPanelPresenter : BasePanelPresenter
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003COnLanguageButtonClickAsync_003Ed__29 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public OptionGamePlayPanelPresenter _003C_003E4__this;

		private ELanguage[] _003Clanguages_003E5__2;

		private UniTask<OptionListPopupPresenter>.Awaiter _003C_003Eu__1;

		private UniTask<object>.Awaiter _003C_003Eu__2;

		private void MoveNext()
		{
		}

		void IAsyncStateMachine.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			this.MoveNext();
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
		}

		void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
			this.SetStateMachine(stateMachine);
		}
	}

	[InjectModel]
	private OptionGamePlayPanelModel _model;

	[View]
	private OptionGamePlayPanelView _view;

	[Inject]
	private GamePlaySettingService _gamePlaySettings;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private ToastManager _toastManager;

	private CompositeDisposable _disposable;

	protected override BasePanelModel PanelModel => null;

	protected override void OnDestroy()
	{
	}

	private void OnInject(OptionGamePlayPanelModel model)
	{
	}

	private void OnDisable()
	{
	}

	private void InitGamePadNav()
	{
	}

	private void InitAimSupport()
	{
	}

	private void SetAimSupportOn(bool aimSupportOn)
	{
	}

	private void InitPadSensitivity()
	{
	}

	private void OnPadSensitivityChanged(float value)
	{
	}

	private void UpdatePadSensitivityText(int sensitivity)
	{
	}

	private void InitAutoAttack()
	{
	}

	private void SetAutoAttackOn(bool autoAttackOn)
	{
	}

	private void InitLockMouse()
	{
	}

	private void SetLockMouseOn(bool lockMouseOn)
	{
	}

	private void InitRstickAttack()
	{
	}

	private void SetRstickAttackOn(bool rstickAttackOn)
	{
	}

	private void InitRun()
	{
	}

	private void SetRunToggleOn(bool runToggleOn)
	{
	}

	private void InitAimLine()
	{
	}

	private void SetAimLineOn(bool aimLineOn)
	{
	}

	private void InitLanguage()
	{
	}

	[AsyncStateMachine(typeof(_003COnLanguageButtonClickAsync_003Ed__29))]
	private UniTask OnLanguageButtonClickAsync()
	{
		return default;
	}

	private void OnResetClick()
	{
	}

	private static void SyncToggle(GameObject[] toggles, int selectedIndex)
	{
	}
}
