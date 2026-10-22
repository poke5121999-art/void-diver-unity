using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using GameCommon.Enum;
using TMPro;
using UnityEngine;
using VContainer;

[RequireComponent(typeof(TMP_InputField))]
[RequireComponent(typeof(Animator))]
public class InputFieldController : MonoBase
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CShowVirtualKeyboardAsync_003Ed__25 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskVoidMethodBuilder _003C_003Et__builder;

		public InputFieldController _003C_003E4__this;

		private UniTask<(EPadInputResult Result, string Text)>.Awaiter _003C_003Eu__1;

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

	[SerializeField]
	private int _charLimit;

	[SerializeField]
	private string _animatorParameter;

	[Inject]
	private InputManager _inputManager;

	[Inject]
	private ToastManager _toastManager;

	private TMP_InputField _inputField;

	private Animator _animator;

	private string _description;

	private bool _isFocused;

	private bool _isShowingKeyboard;

	private EButtonState _currentState;

	private EButtonState _baseState;

	private bool _isPointerOver;

	protected override void Awake()
	{
	}

	private void OnEnable()
	{
	}

	private void Start()
	{
	}

	public void SetDescription(string description)
	{
	}

	private void OnControlDeviceChanged(EControlDevice device)
	{
	}

	private void OnSelected()
	{
	}

	private void OnDeselected()
	{
	}

	private void OnPointerEnter()
	{
	}

	private void OnPointerExit()
	{
	}

	private void OnPointerUp()
	{
	}

	private void SetBaseState(EButtonState state)
	{
	}

	private void RefreshState()
	{
	}

	private void OnSubmit()
	{
	}

	[AsyncStateMachine(typeof(_003CShowVirtualKeyboardAsync_003Ed__25))]
	private UniTaskVoid ShowVirtualKeyboardAsync()
	{
		return default;
	}
}
