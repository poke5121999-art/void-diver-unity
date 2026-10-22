using System.Collections.Generic;
using NemoLib.Mvp;
using UniRx;
using UnityEngine.UI;

public class ParadoxPanelPresenter : BasePanelPresenter
{
	[InjectModel]
	private ParadoxPanelModel _model;

	[View]
	private ParadoxPanelView _view;

	private CompositeDisposable _disposable;

	private List<Button> _navButtons;

	protected override BasePanelModel PanelModel => null;

	private void OnInject(ParadoxPanelModel model)
	{
	}

	protected override void OnDestroy()
	{
	}

	private void InitGamePadNav()
	{
	}

	private void OnCharacterChanged(Character character)
	{
	}

	private void OnParadoxSlotEnter(ParadoxSimpleSlotModel slotModel)
	{
	}

	private void OnParadoxSlotExit(ParadoxSimpleSlotModel slotModel)
	{
	}
}
