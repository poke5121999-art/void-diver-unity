using NemoLib.AssetManagement;
using NemoLib.Mvp;
using VContainer;

public class CharacterSettingSlotPresenter : BaseSlotPresenter<CharacterSettingSlotModel>
{
	[View]
	private CharacterSettingSlotView _view;

	[InjectModel]
	private CharacterSettingSlotModel _model;

	[Inject]
	private AssetManager _assetManager;

	protected override CharacterSettingSlotModel SlotModel => null;

	private void OnInject(CharacterSettingSlotModel model)
	{
	}

	private void UpdateEmptyState()
	{
	}
}
