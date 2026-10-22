using NemoLib.Mvp;
using UnityEngine.UI;
using VContainer;

public class ChatSlotPresenter : BaseSlotPresenter<ChatSlotModel>
{
	[InjectModel]
	private ChatSlotModel _model;

	[View]
	private ChatSlotView _view;

	[Inject]
	private GameManager _gameManager;

	protected override ChatSlotModel SlotModel => null;

	protected override Button Button => null;

	protected void OnInject(ChatSlotModel model)
	{
	}
}
