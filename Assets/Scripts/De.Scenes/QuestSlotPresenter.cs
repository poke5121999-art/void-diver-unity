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

	protected override QuestSlotModel SlotModel => _model;

	protected override Button Button => _view.Button.GetComponent<Button>();

	private void OnInject(QuestSlotModel model)
	{
		InitSlot(model);
		_view.QuestTypeText.text = model.QuestType.ToLocalText();
		_view.QuestNameText.text = model.QuestType.GetQuestName(model.TableId);
	}
}
