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

	protected override CharacterSettingSlotModel SlotModel => _model;

	private void OnInject(CharacterSettingSlotModel model)
	{
		InitSlot(model);
		_view.Empty.SetActive(_model.CharacterId < 1);
		_view.Profile.SetActive(_model.CharacterId > 0);
		if (model.CharacterId >= 1)
		{
			_view.Profile_ImageFace.sprite = _assetManager.GetIconUnitCharacter(model.CharacterId, "Default");
		}
	}

	private void UpdateEmptyState()
	{
		_view.Empty.SetActive(_model.CharacterId < 1);
		_view.Profile.SetActive(_model.CharacterId > 0);
	}
}
