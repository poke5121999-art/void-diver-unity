using NemoLib.AssetManagement;
using NemoLib.Mvp;
using UnityEngine.UI;
using VContainer;

public class SkillSelectInfoSlotPresenter : BaseSlotPresenter<SkillSelectInfoSlotModel>
{
	[View]
	private SkillSelectInfoSlotView _view;

	[InjectModel]
	private SkillSelectInfoSlotModel _model;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private GameManager _gameManager;

	protected override SkillSelectInfoSlotModel SlotModel => null;

	protected override Button Button => null;

	private void OnInject(SkillSelectInfoSlotModel model)
	{
	}

	private void Refresh()
	{
	}
}
