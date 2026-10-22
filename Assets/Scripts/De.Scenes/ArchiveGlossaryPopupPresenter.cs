using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using GameCommon.Table;
using NemoLib.Mvp;
using UnityEngine;
using VContainer;

public class ArchiveGlossaryPopupPresenter : GamePopupPresenter
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003COnOpenAsync_003Ed__18 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public ArchiveGlossaryPopupPresenter _003C_003E4__this;

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

	[InjectModel]
	private ArchiveGlossaryPopupModel _model;

	[View]
	private ArchiveGlossaryPopupView _view;

	[Inject]
	private PlayerManager _playerManager;

	private TGlossary[] _glossaryDatas;

	private TextSlotScrollerModel _scrollerModel;

	private TextSlotModel _selectedSlot;

	private TextSlotModel _focusedSlot;

	private GameInputManager GameInputManager => null;

	public override bool IsInputActionPopup => false;

	private InputActions.UiActions UiActions => default;

	private InputActions.UiPadActions UiPadActions => default;

	public override string OpenSfx => null;

	private void Start()
	{
	}

	[AsyncStateMachine(typeof(_003COnOpenAsync_003Ed__18))]
	protected override UniTask OnOpenAsync()
	{
		return default;
	}

	protected override void OnInputEnabled()
	{
	}

	protected override void OnInputDisabled()
	{
	}

	private void OnUpdateScroll(Vector2 vector2)
	{
	}

	private void OnBack()
	{
	}

	private void OnSlotClick(TextSlotModel slotModel)
	{
	}

	private void OnSlotEnter(TextSlotModel slotModel)
	{
	}

	private void OnSlotExit(TextSlotModel slotModel)
	{
	}

	private void ShowGlossary(TGlossary data)
	{
	}
}
