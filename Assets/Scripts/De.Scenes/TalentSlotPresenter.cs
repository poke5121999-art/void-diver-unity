using NemoLib.AssetManagement;
using NemoLib.Mvp;
using UnityEngine.UI;
using VContainer;

public class TalentSlotPresenter : BaseSlotPresenter<TalentSlotModel>
{
	[InjectModel]
	private TalentSlotModel _model;

	[View]
	private TalentSlotView _view;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private PlayerManager _playerManager;

	protected override TalentSlotModel SlotModel => null;

	protected override Button Button => null;

	private void OnInject(TalentSlotModel model)
	{
	}

	private void OnUpdateUis()
	{
	}
}
