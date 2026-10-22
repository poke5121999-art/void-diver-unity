using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using NemoLib.Log;
using NemoLib.Mvp;
using UniRx;
using UnityEngine;
using VContainer;

namespace NemoLib.Popup
{
	public abstract class BasePopupPresenter : Presenter
	{
		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003COnOpenAsync_003Ed__30 : IAsyncStateMachine
		{
			public int _003C_003E1__state;

			public AsyncUniTaskMethodBuilder _003C_003Et__builder;

			public BasePopupPresenter _003C_003E4__this;

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
		private struct _003COpenAsync_003Ed__29 : IAsyncStateMachine
		{
			public int _003C_003E1__state;

			public AsyncUniTaskMethodBuilder _003C_003Et__builder;

			public BasePopupPresenter _003C_003E4__this;

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

		[Inject]
		protected InputManager InputManager;

		[Inject]
		protected SeriLogger Logger;

		private UniTaskCompletionSource<object> _source;

		private readonly List<GameObject> _initialActiveChildren;

		private List<GameObject> _childrenObjects;

		public bool IsBlurEffect;

		private bool _isPopupOpened;

		private bool _isPopupClosing;

		public Guid Guid { get; }

		public UniTask<object> Task => default;

		public Subject<BasePopupPresenter> OnClose { get; }

		public abstract bool IsInputActionPopup { get; }

		public virtual string OpenSfx => null;

		public virtual string CloseSfx => null;

		protected bool IsInputEnable { get; private set; }

		protected bool IsGamePad => false;

		protected CompositeDisposable OpenDisposable { get; private set; }

		protected CompositeDisposable InputDisposable { get; private set; }

		protected override void Awake()
		{
		}

		internal void ResetForReuse()
		{
		}

		private void OnInject(Model model)
		{
		}

		[AsyncStateMachine(typeof(_003COpenAsync_003Ed__29))]
		public UniTask OpenAsync()
		{
			return default;
		}

		[AsyncStateMachine(typeof(_003COnOpenAsync_003Ed__30))]
		protected virtual UniTask OnOpenAsync()
		{
			return default;
		}

		public virtual void Close(object result = null)
		{
		}

		public void ActivatePopup()
		{
		}

		public void DeactivatePopup()
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
