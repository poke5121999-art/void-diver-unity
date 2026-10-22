using GameCommon.Table;
using NemoLib.Mvp;
using UniRx;
using VContainer;

public class BgmSlotPresenter : BaseSlotPresenter<BgmSlotModel>
{
	[InjectModel]
	private BgmSlotModel _model;

	[View]
	private BgmSlotView _view;

	[Inject]
	private PlayerManager _playerManager;

	protected override BgmSlotModel SlotModel => _model;

	private void OnInject(BgmSlotModel model)
	{
		InitSlot(model);
		_view.Text.text = TLoungeBgm.Table[model.BgmId].Name;
		_playerManager.RxProfile.Subscribe(OnProfileChanged).AddTo(Disposable);
	}

	private void OnProfileChanged(Profile profile)
	{
		_model.RxIsLocked.Value = !_playerManager.IsLoungeBgmUnlocked(_model.BgmId);
		_model.RxIsSelected.Value = _model.BgmId == profile.BgmId;
	}
}
