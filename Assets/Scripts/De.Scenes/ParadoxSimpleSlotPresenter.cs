using GameCommon.Table;
using NemoLib.Mvp;

public class ParadoxSimpleSlotPresenter : BaseSlotPresenter<ParadoxSimpleSlotModel>
{
	[InjectModel]
	private ParadoxSimpleSlotModel _model;

	[View]
	private ParadoxSimpleSlotView _view;

	protected override ParadoxSimpleSlotModel SlotModel => _model;

	private void OnInject(ParadoxSimpleSlotModel model)
	{
		InitSlot(model);
		if (_model.ParadoxId < 1)
		{
			_view.Text.text = TLocalizedText.Empty.LocalText;
			_view.Text.color = UiUtility.HexToColor("#707070");
			_view.LockIcon.gameObject.SetActive(false);
			return;
		}
		TParadox tParadox = TParadox.Table[model.ParadoxId];
		_view.Text.text = tParadox.GetParadoxNameWithLevel();
		_view.Text.color = UiUtility.HexToColor(tParadox.IsPositive ? "#58754B" : "#954D4D");
		_view.LockIcon.gameObject.SetActive(model.IsParadoxLocked);
	}
}
