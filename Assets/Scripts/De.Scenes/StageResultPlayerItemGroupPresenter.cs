using System.Collections.Generic;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using VContainer;

public class StageResultPlayerItemGroupPresenter : Presenter
{
	[View]
	private StageResultPlayerItemGroupView _view;

	[InjectModel]
	private StageResultPlayerItemGroupModel _model;

	[Inject]
	private AssetManager _assetManager;

	private readonly List<GoodsSlotPresenter> _cachedSlots;

	protected void OnInject(StageResultPlayerItemGroupModel model)
	{
	}
}
