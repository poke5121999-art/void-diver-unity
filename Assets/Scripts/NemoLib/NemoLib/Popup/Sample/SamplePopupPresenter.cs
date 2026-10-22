using Cysharp.Threading.Tasks;
using NemoLib.Mvp;

namespace NemoLib.Popup.Sample
{
	public class SamplePopupPresenter : BasePopupPresenter
	{
		[InjectModel]
		private SamplePopupModel model;

		[View]
		private SamplePopupView view;

		public override bool IsInputActionPopup => true;

		protected override async UniTask OnOpenAsync()
		{
			await base.OnOpenAsync();
		}

		public override void Close(object result = null)
		{
		}
	}
}
