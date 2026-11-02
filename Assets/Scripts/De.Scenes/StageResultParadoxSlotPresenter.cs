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

	// Port fix, not from the machine code (the binary is no longer on this machine): the bulk translation returned null
	// here, so a click on a paradox of the dive results threw a NullReferenceException in OnClick and its tooltip never opened (2026-11-02, reported from the web build). The model's OnClick has subscribers (OnInject below).
	protected override StageResultParadoxSlotModel SlotModel => _model;

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
