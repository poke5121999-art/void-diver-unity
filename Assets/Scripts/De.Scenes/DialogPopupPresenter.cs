using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Febucci.UI;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using VContainer;

public class DialogPopupPresenter : GamePopupPresenter
{
	private enum EState
	{
		None = 0,
		Started = 1,
		Ended = 2
	}

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CClearAllCustomImagesAsync_003Ed__60 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public DialogPopupPresenter _003C_003E4__this;

		private Image[] _003CallImages_003E5__2;

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

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CClearCustomImageAsync_003Ed__59 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public bool isFront;

		public DialogPopupPresenter _003C_003E4__this;

		public int index;

		private Image _003Cimg_003E5__2;

		private TaskAwaiter _003C_003Eu__1;

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
	private struct _003CDelayDialogAsync_003Ed__66 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public int milliseconds;

		public DialogPopupPresenter _003C_003E4__this;

		private CancellationToken _003Ctoken_003E5__2;

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

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003COnOpenAsync_003Ed__38 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public DialogPopupPresenter _003C_003E4__this;

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

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CRefreshSelectionNavAsync_003Ed__55 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskVoidMethodBuilder _003C_003Et__builder;

		public DialogPopupPresenter _003C_003E4__this;

		private Cysharp.Threading.Tasks.YieldAwaitable.Awaiter _003C_003Eu__1;

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
	private struct _003CSetCustomImageAsync_003Ed__58 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public bool isFront;

		public DialogPopupPresenter _003C_003E4__this;

		public int index;

		public string imageName;

		private float _003Cduration_003E5__2;

		private Image _003CtargetImg_003E5__3;

		private UniTask<Sprite>.Awaiter _003C_003Eu__1;

		private TaskAwaiter _003C_003Eu__2;

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
	private DialogPopupModel _model;

	[View]
	private DialogPopupView _view;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private SoundManager _soundManager;

	[Inject]
	private GameManager _gameManager;

	private TypewriterByCharacter _typeWriter;

	private bool _isWritingText;

	private readonly Queue<DialogEntry> _dialogQueue;

	private DialogEntry _currentEntry;

	private bool _canClose;

	private bool _needSelect;

	private const float _defaultBgFadeDuration = 0.5f;

	private EState _state;

	private CancellationTokenSource _delayCts;

	private bool _isDelayActive;

	private int _selectionPageIndex;

	private bool _focusLastAfterRefresh;

	private CompositeDisposable _selectionDisposable;

	private GameInputManager GameInputManager => null;

	public override bool IsInputActionPopup => false;

	private InputActions.UiActions UiActions => default;

	private InputActions.UiKeyboardActions UiKeyboardActions => default;

	private bool HasSelection => false;

	private Animator Animator => null;

	private Animator FrameAnimator => null;

	private int SelectionPageCount => 0;

	[AsyncStateMachine(typeof(_003COnOpenAsync_003Ed__38))]
	protected override UniTask OnOpenAsync()
	{
		return default;
	}

	protected override void OnInputEnabled()
	{
	}

	private void OnProfileChanged(Profile profile)
	{
	}

	private void OnNext(bool playSfx)
	{
	}

	private void OnSelectButtonClick(int actualIndex)
	{
	}

	private void OnScrollInput(InputAction.CallbackContext ctx)
	{
	}

	private void OnSelectionPageChange(int direction)
	{
	}

	protected override void OnNavOutOfBounds(ENavDir dir)
	{
	}

	private void OnTextWriterEnd()
	{
	}

	public void AppendDialogText(DialogEntry entry)
	{
	}

	private void MoveToNext(bool playSfx)
	{
	}

	public void SetClosable()
	{
	}

	private void ShowNextDialog()
	{
	}

	public void SetDialogImmediate(DialogEntry entry)
	{
	}

	private void SetPortraitActive(Image image, DialogSpeakerEntry entry)
	{
	}

	public void RefreshSelections(SelectableMessageData[] selectableMessageDatas)
	{
	}

	private void UpdateSelectionDisplay()
	{
	}

	[AsyncStateMachine(typeof(_003CRefreshSelectionNavAsync_003Ed__55))]
	private UniTaskVoid RefreshSelectionNavAsync()
	{
		return default;
	}

	private void PlaySpeakerAnimation(int index)
	{
	}

	public void SetBackgroundAlpha(float alpha)
	{
	}

	[AsyncStateMachine(typeof(_003CSetCustomImageAsync_003Ed__58))]
	public UniTask SetCustomImageAsync(string imageName, bool isFront, int index)
	{
		return default;
	}

	[AsyncStateMachine(typeof(_003CClearCustomImageAsync_003Ed__59))]
	public UniTask ClearCustomImageAsync(bool isFront, int index)
	{
		return default;
	}

	[AsyncStateMachine(typeof(_003CClearAllCustomImagesAsync_003Ed__60))]
	public UniTask ClearAllCustomImagesAsync()
	{
		return default;
	}

	private void OnAnimationEndCompleted()
	{
	}

	private void UpdateDimActive()
	{
	}

	private bool HasAnyCustomImageActive()
	{
		return false;
	}

	public void FadeOutFrame()
	{
	}

	public void FadeInFrame()
	{
	}

	[AsyncStateMachine(typeof(_003CDelayDialogAsync_003Ed__66))]
	public UniTask DelayDialogAsync(int milliseconds)
	{
		return default;
	}
}
