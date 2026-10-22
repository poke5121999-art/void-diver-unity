using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;

namespace NemoLib.Popup
{
	public static class Extensions
	{
		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CShowConfirmationTextPopupAsync_003Ed__3 : IAsyncStateMachine
		{
			public int _003C_003E1__state;

			public AsyncUniTaskMethodBuilder<bool> _003C_003Et__builder;

			public PopupManager popupManager;

			public string message;

			public string additionalMessage;

			private UniTask<TextPopupPresenter>.Awaiter _003C_003Eu__1;

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
		private struct _003CShowTextPopupAsync_003Ed__1 : IAsyncStateMachine
		{
			public int _003C_003E1__state;

			public AsyncUniTaskMethodBuilder _003C_003Et__builder;

			public PopupManager popupManager;

			public string message;

			public string additionalMessage;

			private UniTask<TextPopupPresenter>.Awaiter _003C_003Eu__1;

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

		public static void ShowTextPopup(this PopupManager popupManager, string message, string additionalMessage = null)
		{
		}

		[AsyncStateMachine(typeof(_003CShowTextPopupAsync_003Ed__1))]
		public static UniTask ShowTextPopupAsync(this PopupManager popupManager, string message, string additionalMessage = null)
		{
			return default;
		}

		public static void ShowConnectionFailPopup(this PopupManager popupManager, int resultCode)
		{
		}

		[AsyncStateMachine(typeof(_003CShowConfirmationTextPopupAsync_003Ed__3))]
		public static UniTask<bool> ShowConfirmationTextPopupAsync(this PopupManager popupManager, string message, string additionalMessage = null)
		{
			return default;
		}

		public static void Notice(this ToastManager toastManager, string text, string sfx = null)
		{
		}

		public static void NoticeFail(this ToastManager toastManager, string text)
		{
		}

		public static void PlayerNotice(this ToastManager toastManager, string text, string sfx = null)
		{
		}
	}
}
