using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using GameCommon.Enum;
using NemoLib.Mvp;
using VContainer;

public class RoomDifficultyPopupPresenter : GamePopupPresenter
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003COnOpenAsync_003Ed__15 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public RoomDifficultyPopupPresenter _003C_003E4__this;

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
	private RoomDifficultyPopupView _view;

	[InjectModel]
	private RoomDifficultyPopupModel _model;

	[Inject]
	private GameContext _gameContext;

	[Inject]
	private SoundManager _soundManager;

	[Inject]
	private ToastManager _toastManager;

	private ERoomDifficulty _selectedDifficulty;

	private readonly List<RoomDifficultySlotModel> _slotModels;

	private GameInputManager GameInputManager => null;

	public override string OpenSfx => null;

	public override bool IsInputActionPopup => false;

	private InputActions.UiActions UiActions => default;

	[AsyncStateMachine(typeof(_003COnOpenAsync_003Ed__15))]
	protected override UniTask OnOpenAsync()
	{
		return default;
	}

	private void InitGamePadNav()
	{
	}

	private void InitBottomGamePadNav()
	{
	}

	protected override void OnInputEnabled()
	{
	}

	private void OnSlotClick(RoomDifficultySlotModel slotModel)
	{
	}

	private void OnConfirmButtonClick()
	{
	}
}
