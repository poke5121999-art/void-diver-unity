using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using UniRx;
using UnityEngine;
using VContainer;

namespace NemoLib.Popup
{
	public class PopupManager : MonoBase
	{
		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CEnsurePopupAsync_003Ed__30<T> : IAsyncStateMachine where T : BasePopupPresenter
		{
			public int _003C_003E1__state;

			public AsyncUniTaskMethodBuilder<T> _003C_003Et__builder;

			public PopupManager _003C_003E4__this;

			private UniTask<T>.Awaiter _003C_003Eu__1;

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
		private struct _003CInstantiatePopupAsync_003Ed__29<T> : IAsyncStateMachine where T : BasePopupPresenter
		{
			public int _003C_003E1__state;

			public AsyncUniTaskMethodBuilder<T> _003C_003Et__builder;

			public PopupManager _003C_003E4__this;

			public Transform rootTf;

			private UniTask<GameObject>.Awaiter _003C_003Eu__1;

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
		private struct _003COpenAsync_003Ed__39<T> : IAsyncStateMachine where T : BasePopupPresenter
		{
			public int _003C_003E1__state;

			public AsyncUniTaskMethodBuilder<T> _003C_003Et__builder;

			public PopupManager _003C_003E4__this;

			public Model model;

			private BlockScope _003CblockScope_003E5__2;

			private TaskAwaiter _003C_003Eu__1;

			private T _003Cpresenter_003E5__3;

			private UniTask<T>.Awaiter _003C_003Eu__2;

			private UniTask.Awaiter _003C_003Eu__3;

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
		private struct _003CPreloadAsync_003Ed__31<T> : IAsyncStateMachine where T : BasePopupPresenter
		{
			public int _003C_003E1__state;

			public AsyncUniTaskMethodBuilder _003C_003Et__builder;

			public PopupManager _003C_003E4__this;

			private Type _003Ctype_003E5__2;

			private UniTask<T>.Awaiter _003C_003Eu__1;

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
		private struct _003CReactivateWhenStillTopAsync_003Ed__41 : IAsyncStateMachine
		{
			public int _003C_003E1__state;

			public AsyncUniTaskMethodBuilder _003C_003Et__builder;

			public BasePopupPresenter popup;

			public PopupManager _003C_003E4__this;

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

		[SerializeField]
		private Canvas _canvas;

		[SerializeField]
		private Canvas _overlayCanvas;

		[SerializeField]
		private Transform _cacheRootTf;

		[SerializeField]
		private string _defaultOpenSfx;

		[SerializeField]
		private string _defaultCloseSfx;

		[Inject]
		private SoundManager _soundManager;

		[Inject]
		private AssetManager _assetManager;

		[Inject]
		private CameraManager _cameraManager;

		private readonly List<BasePopupPresenter> _popups;

		private readonly HashSet<Type> _cacheTypeSet;

		private readonly Dictionary<Type, BasePopupPresenter> _cache;

		private readonly SemaphoreSlim _openSemaphore;

		private bool _skipCloseSfx;

		public readonly Subject<BasePopupPresenter> OnBeforeOpenPopup;

		public readonly Subject<BasePopupPresenter> OnPopupOpened;

		public readonly Subject<BasePopupPresenter> OnPopupClosed;

		public readonly Subject<Unit> OnAllPopupsClosed;

		public readonly Subject<Unit> OnBlurEffectChanged;

		private Transform RootTf => null;

		private Transform OverlayRootTf => null;

		public Canvas OverlayCanvas => null;

		public void HideCanvas()
		{
		}

		public void ShowCanvas()
		{
		}

		public void RefreshBlurEffect()
		{
		}

		protected override void Awake()
		{
		}

		private void Start()
		{
		}

		[AsyncStateMachine(typeof(_003CInstantiatePopupAsync_003Ed__29<>))]
		private UniTask<T> InstantiatePopupAsync<T>(Transform rootTf) where T : BasePopupPresenter
		{
			return default;
		}

		[AsyncStateMachine(typeof(_003CEnsurePopupAsync_003Ed__30<>))]
		private UniTask<T> EnsurePopupAsync<T>() where T : BasePopupPresenter
		{
			return default;
		}

		[AsyncStateMachine(typeof(_003CPreloadAsync_003Ed__31<>))]
		public UniTask PreloadAsync<T>() where T : BasePopupPresenter
		{
			return default;
		}

		public T FindPopup<T>() where T : BasePopupPresenter
		{
			return null;
		}

		public IEnumerable<T> FindAllPopup<T>() where T : BasePopupPresenter
		{
			return null;
		}

		public bool ExistPopup<T>() where T : BasePopupPresenter
		{
			return false;
		}

		public bool ExistPopup()
		{
			return false;
		}

		public bool ExistInputActionPopup()
		{
			return false;
		}

		public bool IsLastPopup(BasePopupPresenter popupPresenter)
		{
			return false;
		}

		public void Open<T>(Model model = null) where T : BasePopupPresenter
		{
		}

		[AsyncStateMachine(typeof(_003COpenAsync_003Ed__39<>))]
		public UniTask<T> OpenAsync<T>(Model model = null) where T : BasePopupPresenter
		{
			return default;
		}

		private void OnClose(BasePopupPresenter target)
		{
		}

		[AsyncStateMachine(typeof(_003CReactivateWhenStillTopAsync_003Ed__41))]
		private UniTask ReactivateWhenStillTopAsync(BasePopupPresenter popup)
		{
			return default;
		}

		public void CloseAll<T>() where T : BasePopupPresenter
		{
		}

		public void CloseAll(params Type[] excludePresenterTypes)
		{
		}

		public void CloseAllWithoutSfx(params Type[] excludePresenterTypes)
		{
		}

		public void CloseTopPopup()
		{
		}

		public void ClearCache<T>() where T : BasePopupPresenter
		{
		}

		public void ClearAllCache()
		{
		}
	}
}
