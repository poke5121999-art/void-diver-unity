using GameCommon.Table;
using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using VContainer;

public class StageResultParadoxSlotPresenter : BaseSlotPresenter<StageResultParadoxSlotModel>
{
	[View]
	private StageResultParadoxSlotView _view;

	[InjectModel]
	private StageResultParadoxSlotModel _model;

	[Inject]
	private PopupManager _popupManager;

	protected override StageResultParadoxSlotModel SlotModel => null;

	private void OnInject(StageResultParadoxSlotModel model)
	{
		InitSlot(model);
		TParadox tParadox = TParadox.Table[model.ParadoxId];
		string paradoxNameWithLevel = tParadox.GetParadoxNameWithLevel();
		_view.ParadoxNamePositive.text = paradoxNameWithLevel;
		_view.ParadoxNameNegetive.text = paradoxNameWithLevel;
		_view.Clear.gameObject.SetActive(model.IsRemoved);
		_view.ParadoxNamePositive.gameObject.SetActive(tParadox.IsPositive);
		_view.ParadoxNameNegetive.gameObject.SetActive(!tParadox.IsPositive);
		model.OnClick.Subscribe(delegate
		{
			OnSlotClick();
		}).AddTo(Disposable);
	}

	private void OnSlotClick()
	{
		_popupManager.Open<ParadoxTooltipPopupPresenter>(new ParadoxTooltipPopupModel
		{
			ParadoxId = _model.ParadoxId
		});
	}
}
