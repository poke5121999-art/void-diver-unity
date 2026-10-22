using System;
using VContainer;
using VContainer.Unity;

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
			this.InjectFromRootContainer();
			_calling = calling;
			_background = background;
			_blockManager.Block(calling, background);
		}

		public void Dispose()
		{
			_blockManager.Release(_calling, _background);
		}
	}
}
