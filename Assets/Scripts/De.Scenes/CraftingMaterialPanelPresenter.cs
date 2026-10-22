using System.Collections.Generic;
using NemoLib.Mvp;
using UniRx;
using UnityEngine.UI;
using VContainer;

public class CraftingMaterialPanelPresenter : BasePanelPresenter
{
	[InjectModel]
	private CraftingMaterialPanelModel _model;

	[View]
	private CraftingMaterialPanelView _view;

	[Inject]
	private PlayerManager _playerManager;

	private const int _numberOfSlotsInRow = 6;

	private CompositeDisposable _disposable;

	protected override BasePanelModel PanelModel => null;

	public int NavigableColumnCount => 0;

	protected void OnInject(CraftingMaterialPanelModel model)
	{
	}

	private void InitGamePadNav()
	{
	}

	public IReadOnlyList<Selectable> GetNavigableSlots()
	{
		return null;
	}

	private void OnUpdateUis()
	{
	}

	protected override void OnDestroy()
	{
	}
}
