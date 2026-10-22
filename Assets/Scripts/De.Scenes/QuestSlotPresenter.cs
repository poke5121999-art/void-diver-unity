using NemoLib.Mvp;
using UnityEngine.UI;
using VContainer;

public class QuestSlotPresenter : BaseSlotPresenter<QuestSlotModel>
{
	[InjectModel]
	private QuestSlotModel _model;

	[View]
	private QuestSlotView _view;

	[Inject]
	private PlayerManager _playerManager;

	protected override QuestSlotModel SlotModel => null;

	protected override Button Button => null;

	private void OnInject(QuestSlotModel model)
	{
	}
}
