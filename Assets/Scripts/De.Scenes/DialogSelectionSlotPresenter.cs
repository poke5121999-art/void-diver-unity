using GameCommon.Enum;
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

	protected override DialogSelectionSlotModel SlotModel => _model;

	protected override Button Button => _view.Button;

	private void OnInject(DialogSelectionSlotModel model)
	{
		InitSlot(model);
		SelectableMessageData messageData = model.MessageData;
		if (messageData == null)
		{
			_logger.Debug(string.Format("[DialogSelectionSlotPresenter] MessageData is null for index {0}", model.Index));
			return;
		}
		_view.Text.text = messageData.Message;
		_view.NewTag.SetActive(messageData.DialogType == EDialogType.Talk && !messageData.HasRead);
		bool active;
		switch (messageData.DialogType)
		{
		case EDialogType.Quest:
		case EDialogType.Mission:
		case EDialogType.Talk:
			active = messageData.HasRead;
			break;
		default:
			active = false;
			break;
		}
		_view.CheckIcon.gameObject.SetActive(active);
		_view.MainQuestIcon.SetActive(messageData.DialogType == EDialogType.Lounge);
	}
}
