using GameCommon.Table;
using InputIcons;
using NemoLib.Mvp;
using VContainer;

public class KeySettingSlotPresenter : BaseSlotPresenter<KeySettingSlotModel>
{
	[InjectModel]
	private KeySettingSlotModel _model;

	[View]
	private KeySettingSlotView _view;

	[Inject]
	private GameInputManager _inputManager;

	[Inject]
	private GamePlaySettingService _playSettingService;

	protected override KeySettingSlotModel SlotModel => null;

	private void OnInject(KeySettingSlotModel model)
	{
	}

	private void RefreshText(TInputAction data)
	{
	}

	private void SetPromptAction(II_ImagePrompt prompt, string actionName, int bindingIndex)
	{
	}
}
