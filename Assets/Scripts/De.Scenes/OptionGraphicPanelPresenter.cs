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

public class OptionGraphicPanelPresenter : BasePanelPresenter
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003COnGammaButtonClickAsync_003Ed__31 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public OptionGraphicPanelPresenter _003C_003E4__this;

		private UniTask<GammaPopupPresenter>.Awaiter _003C_003Eu__1;

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

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003COnResolutionButtonClickAsync_003Ed__18 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public OptionGraphicPanelPresenter _003C_003E4__this;

		private EResolution[] _003Cresolutions_003E5__2;

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
	private OptionGraphicPanelModel _model;

	[View]
	private OptionGraphicPanelView _view;

	[Inject]
	private GraphicSettingService _graphicSettings;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private GamePlaySettingService _playSettingService;

	private CompositeDisposable _disposable;

	protected override BasePanelModel PanelModel => null;

	protected override void OnDestroy()
	{
	}

	private void OnInject(OptionGraphicPanelModel model)
	{
	}

	private void OnDisable()
	{
	}

	private void OnLanguageChanged(ELanguage language)
	{
	}

	private void InitGamePadNav()
	{
	}

	private void InitScreenMode()
	{
	}

	private void ApplyScreenModeLocalText()
	{
	}

	private void SetScreenMode(EScreenMode screenMode)
	{
	}

	private void InitResolution()
	{
	}

	private void ApplyResolutionLocalText()
	{
	}

	[AsyncStateMachine(typeof(_003COnResolutionButtonClickAsync_003Ed__18))]
	private UniTask OnResolutionButtonClickAsync()
	{
		return default;
	}

	private void InitGraphicsQuality()
	{
	}

	private void ApplyGraphicsQualityLocalText()
	{
	}

	private void SetQualityLevel(EQualityLevel qualityLevel)
	{
	}

	private void InitFrameRate()
	{
	}

	private void ApplyFrameRateLocalText()
	{
	}

	private void SetFrameRate(EFrameRate frameRate)
	{
	}

	private void InitVSync()
	{
	}

	private void SetVSyncOn(bool vsyncOn)
	{
	}

	private void OnVSyncChanged(bool vsyncOn)
	{
	}

	private void InitCameraShaking()
	{
	}

	private void SetCameraShakingOn(bool cameraShakingOn)
	{
	}

	private void InitGamma()
	{
	}

	[AsyncStateMachine(typeof(_003COnGammaButtonClickAsync_003Ed__31))]
	private UniTask OnGammaButtonClickAsync()
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
