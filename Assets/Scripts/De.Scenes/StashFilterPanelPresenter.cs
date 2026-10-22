using System.Collections.Generic;
using GameCommon.Enum;
using NemoLib.Mvp;
using UniRx;
using UnityEngine.UI;

public class StashFilterPanelPresenter : BasePanelPresenter
{
	[InjectModel]
	private StashFilterPanelModel _model;

	[View]
	private StashFilterPanelView _view;

	private CompositeDisposable _disposable;

	private readonly List<Selectable> _toggleSelectables;

	private Toggle[] _toggles;

	private EGoodsType[][] _filterTypes;

	protected override BasePanelModel PanelModel => null;

	protected override void OnDestroy()
	{
	}

	private void OnInject(StashFilterPanelModel model)
	{
	}

	private void InitFilterToggles()
	{
	}

	private void ApplyFilterToToggles(EGoodsType[] allowed)
	{
	}

	private bool IsSameFilter(EGoodsType[] a, EGoodsType[] b)
	{
		return false;
	}

	private void InitGamePadNav()
	{
	}
}
