using NemoLib.Mvp;
using UnityEngine.UI;

public class SelectionSlotPresenter : BaseSlotPresenter<SelectionSlotModel>
{
	[View]
	private SelectionSlotView _view;

	[InjectModel]
	private SelectionSlotModel _model;

	protected override SelectionSlotModel SlotModel => _model;

	protected override Button Button => _view.Button;

	private void OnInject(SelectionSlotModel model)
	{
		InitSlot(model);
		_view.Text.text = model.SelectionData.InteractionName;
		_view.LoungeQuestMark.SetActive(model.SelectionData.ShowQuestMark);
	}
}
