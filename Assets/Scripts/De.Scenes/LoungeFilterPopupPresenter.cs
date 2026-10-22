using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using GameCommon.Enum;
using NemoLib.Mvp;

public class LoungeFilterPopupPresenter : GamePopupPresenter
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003COnOpenAsync_003Ed__8 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public LoungeFilterPopupPresenter _003C_003E4__this;

		private UniTask.Awaiter _003C_003Eu__1;

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

	[View]
	private LoungeFilterPopupView _view;

	[InjectModel]
	private LoungeFilterPopupModel _model;

	private GameInputManager GameInputManager => null;

	public override bool IsInputActionPopup => false;

	private InputActions.UiActions UiActions => default;

	[AsyncStateMachine(typeof(_003COnOpenAsync_003Ed__8))]
	protected override UniTask OnOpenAsync()
	{
		return default;
	}

	private void InitGamePadNav()
	{
	}

	protected override void OnInputEnabled()
	{
	}

	private void InitUI()
	{
	}

	private void RefreshUI()
	{
	}

	private void BindUIEvents()
	{
	}

	private void BindButtonEvents()
	{
	}

	private void OnBlockOtherAreaToggleChanged(bool isOn)
	{
	}

	private void OnDifficultyToggleChanged(ERoomDifficulty difficulty, bool isOn)
	{
	}

	private void OnMinHostLevelChanged(float value)
	{
	}

	private void OnPingToggleChanged(int index, bool isOn)
	{
	}

	private void OnResetButtonClick()
	{
	}

	private void OnConfirm()
	{
	}

	private void OnCancel()
	{
	}

	private void OnMinHostLevelUpClick()
	{
	}

	private void OnMinHostLevelDownClick()
	{
	}
}
