using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using UniRx;
using VContainer;

public class SkillSelectPopupPresenter : GamePopupPresenter
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003COnOpenAsync_003Ed__27 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public SkillSelectPopupPresenter _003C_003E4__this;

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
	private SkillSelectPopupView _view;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private ToastManager _toastManager;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private SoundManager _soundManager;

	[Inject]
	private GameChannel _gameChannel;

	private Character _character;

	private CompositeDisposable _scrollerDisposable;

	private SkillSelectScrollerModel _scrollerModel;

	private int _targetIndex;

	private int _focusedInfoSlotIndex;

	private float _infoSlotScrollPosition;

	private List<SkillSlotModel> _skillSelectSlotModels;

	private GameInputManager GameInputManager => null;

	private GamePlayer MyPlayer => null;

	private int ActiveSkillSlotCount => 0;

	public override bool IsInputActionPopup => false;

	public override string OpenSfx => null;

	private InputActions.UiActions UiActions => default;

	private SkillSlotModel TargetSkillSlot => null;

	[AsyncStateMachine(typeof(_003COnOpenAsync_003Ed__27))]
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

	private void SetScroller()
	{
	}

	private void OnTabSlot(int direction)
	{
	}

	private void OnInputSlotClicked(SkillSlotModel slotModel)
	{
	}

	private void SetKeyGuide()
	{
	}

	private bool IsSlotLocked(SkillSlotModel slot)
	{
		return false;
	}

	private void RefreshPanel()
	{
	}

	private void ShowReadOnlySkillInfo(int skillId)
	{
	}

	private void ShowLocked()
	{
	}

	private void ShowSelectableSlot(SkillSlotModel slot)
	{
	}

	private void OnSkillSelectInfoSlotClick(SkillSelectInfoSlotModel model)
	{
	}

	private void RefreshSlot(SkillSlotModel slotModel)
	{
	}

	private void CloseWithSave()
	{
	}
}
