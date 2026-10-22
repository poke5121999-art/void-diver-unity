using NemoLib.Log;
using NemoLib.Mvp;
using UnityEngine.UI;
using VContainer;

public class DialogSelectionSlotPresenter : BaseSlotPresenter<DialogSelectionSlotModel>
{
	[View]
	private DialogSelectionSlotView _view;

	[InjectModel]
	private DialogSelectionSlotModel _model;

	[Inject]
	private SeriLogger _logger;

	protected override DialogSelectionSlotModel SlotModel => null;

	protected override Button Button => null;

	private void OnInject(DialogSelectionSlotModel model)
	{
	}
}
