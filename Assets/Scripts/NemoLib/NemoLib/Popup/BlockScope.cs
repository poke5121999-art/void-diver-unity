using System;
using VContainer;

namespace NemoLib.Popup
{
	public class BlockScope : NativeBase, IDisposable
	{
		[Inject]
		private BlockManager _blockManager;

		private readonly bool _calling;

		private readonly bool _background;

		public BlockScope(bool calling = true, bool background = false)
		{
		}

		public void Dispose()
		{
		}
	}
}
