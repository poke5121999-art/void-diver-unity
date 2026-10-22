using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using UnityEngine;
using VContainer;

public class StageResultPlayerParadoxGroupPresenter : Presenter
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CArrangeSlotsAsync_003Ed__8 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public StageResultPlayerParadoxGroupPresenter _003C_003E4__this;

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

	[View]
	private StageResultPlayerParadoxGroupView _view;

	[InjectModel]
	private StageResultPlayerParadoxGroupModel _model;

	[Inject]
	private AssetManager _assetManager;

	private const float BODY_WIDTH = 680f;

	private const float SLOT_SPACE = 10f;

	private readonly List<StageResultParadoxSlotPresenter> _cachedSlots;

	protected void OnInject(StageResultPlayerParadoxGroupModel model)
	{
	}

	private StageResultParadoxSlotPresenter GetOrCreateSlot(int index)
	{
		return null;
	}

	[AsyncStateMachine(typeof(_003CArrangeSlotsAsync_003Ed__8))]
	private UniTask ArrangeSlotsAsync()
	{
		return default;
	}

	private bool HasActiveChild(Transform parent)
	{
		return false;
	}

	private void DeactivateAllSlots()
	{
	}
}
