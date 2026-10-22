using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;

namespace UnityEngine.UI
{
	public static class Extensions
	{
		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CSetSpriteAsync_003Ed__2 : IAsyncStateMachine
		{
			public int _003C_003E1__state;

			public AsyncUniTaskMethodBuilder _003C_003Et__builder;

			public Image image;

			public UniTask<Sprite> iconTask;

			private bool _003CpreviousEnabled_003E5__2;

			private UniTask<Sprite>.Awaiter _003C_003Eu__1;

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

		public static void SetSprite(this Image image, Sprite sprite)
		{
		}

		public static void SetSprite(this Image image, UniTask<Sprite> spriteTask)
		{
		}

		[AsyncStateMachine(typeof(_003CSetSpriteAsync_003Ed__2))]
		private static UniTask SetSpriteAsync(this Image image, UniTask<Sprite> iconTask)
		{
			return default;
		}
	}
}
