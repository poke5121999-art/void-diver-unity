using System.Collections.Generic;
using NemoLib.Mvp;
using UniRx;
using UnityEngine.UI;
using VContainer;

public class MenuSkillPanelPresenter : BasePanelPresenter
{
	[InjectModel]
	private MenuSkillPanelModel _model;

	[View]
	private MenuSkillPanelView _view;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private GameObjectManager _objectManager;

	[Inject]
	private GameContext _gameContext;

	private CharacterModel _characterModel;

	private CompositeDisposable _disposable;

	private readonly List<Button> _slotButtons;

	private readonly List<SkillSlotModel> _slotModels;

	private GamePlayer Player => null;

	protected override BasePanelModel PanelModel => null;

	protected override void OnDestroy()
	{
	}

	protected void OnInject(MenuSkillPanelModel model)
	{
	}

	private void Setup(CharacterModel characterModel, MenuSkillPanelModel model)
	{
	}

	protected override void OnInputEnabled()
	{
	}

	private void InitGamePadNav()
	{
	}

	private void RefreshSkillSlot()
	{
	}

	private void OnSlotEnter(SkillSlotModel slotModel)
	{
	}

	private void OnSlotExit(SkillSlotModel slotModel)
	{
	}
}
