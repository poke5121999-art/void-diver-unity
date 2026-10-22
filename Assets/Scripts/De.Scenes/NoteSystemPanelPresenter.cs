using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Febucci.UI;
using NemoLib.Mvp;
using TMPro;

public class NoteSystemPanelPresenter : BasePanelPresenter
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CPlayMessageAsync_003Ed__11 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public NoteSystemPanelPresenter _003C_003E4__this;

		public CancellationToken ct;

		public string text;

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
	private NoteSystemPanelModel _model;

	[View]
	private NoteSystemPanelView _view;

	private TypewriterByCharacter _typewriter;

	private bool _isWriting;

	protected override BasePanelModel PanelModel => null;

	public TextMeshProUGUI Message => null;

	public bool IsWriting => false;

	private void OnInject(NoteSystemPanelModel model)
	{
	}

	[AsyncStateMachine(typeof(_003CPlayMessageAsync_003Ed__11))]
	public UniTask PlayMessageAsync(CancellationToken ct, string text)
	{
		return default;
	}

	public void SkipTypewriter()
	{
	}
}
