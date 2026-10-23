using System.Collections.Generic;
using NemoLib.Mvp;
using UniRx;
using UnityEngine;
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

	private readonly List<Button> _slotButtons = new List<Button>();

	private readonly List<SkillSlotModel> _slotModels = new List<SkillSlotModel>();

	private GamePlayer Player => _gameContext.Players[_model.ClientId];

	protected override BasePanelModel PanelModel => _model;

	protected override void OnDestroy()
	{
		base.OnDestroy();
		_disposable?.Dispose();
	}

	protected void OnInject(MenuSkillPanelModel model)
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		CharacterModel characterModel = _gameManager.FindCharacterModel(_model.ClientId);
		if (characterModel != null)
		{
			Setup(characterModel, model);
			return;
		}
		_objectManager.OnTargetSpawned.Where((ITarget t) => t is CharacterModel characterModel2 && characterModel2.ClientId == _model.ClientId).Select((ITarget t) => (CharacterModel)t).Take(1)
			.Subscribe(delegate(CharacterModel cm)
			{
				Setup(cm, model);
			})
			.AddTo(_disposable);
	}

	private void Setup(CharacterModel characterModel, MenuSkillPanelModel model)
	{
		_characterModel = characterModel;
		RefreshSkillSlot();
		_characterModel.OnActiveSkillsChanged.Subscribe(delegate
		{
			RefreshSkillSlot();
		}).AddTo(_disposable);
		model.RxIsInputEnabled.Where((bool _) => _).Subscribe(delegate
		{
			OnInputEnabled();
		}).AddTo(_disposable);
		model.RxIsInputEnabled.Where((bool _) => !_).Subscribe(delegate
		{
			OnInputDisabled();
		}).AddTo(_disposable);
	}

	protected override void OnInputEnabled()
	{
		if (_slotButtons.Count > 0)
		{
			SetDefaultNav(_slotButtons[(_model.EntryDirection == ENavDir.Up) ? (_slotButtons.Count - 1) : 0]);
		}
		base.OnInputEnabled();
	}

	private void InitGamePadNav()
	{
		ClearNav();
		if (_slotButtons.Count != 0)
		{
			SetNavHorizontal(_slotButtons);
		}
	}

	private void RefreshSkillSlot()
	{
		_slotButtons.Clear();
		_slotModels.Clear();
		GameObject[] slots = _view.Slots;
		for (int i = 0; i < slots.Length; i++)
		{
			slots[i].SetActive(value: false);
		}
		if (!(_characterModel != null))
		{
			return;
		}
		if (_characterModel.PassiveSkillIds != null && _characterModel.PassiveSkillIds.Count > 0)
		{
			_view.Slots[0].SetActive(value: true);
			int value = ((_characterModel.PassiveSkillIds.Count < 1) ? (-1) : _characterModel.PassiveSkillIds[0]);
			SkillSlotModel skillSlotModel = new SkillSlotModel();
			skillSlotModel.SlotIndex = 0;
			skillSlotModel.RxSkillId.Value = value;
			skillSlotModel.OnEnter.Subscribe(OnSlotEnter);
			skillSlotModel.OnExit.Subscribe(OnSlotExit);
			_view.Slots[0].GetComponent<SkillSlotPresenter>().Inject(skillSlotModel);
			_slotModels.Add(skillSlotModel);
			_slotButtons.Add(_view.Slots[0].GetComponent<Button>());
		}
		for (int j = 1; j < _characterModel.ActiveSkillIds.Count; j++)
		{
			if (j < Player.ActiveSlotCount + 2)
			{
				_view.Slots[j].SetActive(value: true);
				int value2 = _characterModel.GetActiveSkillIds()[j];
				SkillSlotModel skillSlotModel2 = new SkillSlotModel();
				skillSlotModel2.SlotIndex = j;
				skillSlotModel2.RxSkillId.Value = value2;
				skillSlotModel2.OnEnter.Subscribe(OnSlotEnter);
				skillSlotModel2.OnExit.Subscribe(OnSlotExit);
				_view.Slots[j].GetComponent<SkillSlotPresenter>().Inject(skillSlotModel2);
				_slotModels.Add(skillSlotModel2);
				_slotButtons.Add(_view.Slots[j].GetComponent<Button>());
			}
		}
		ClearNav();
		if (_slotButtons.Count != 0)
		{
			SetNavHorizontal(_slotButtons);
		}
	}

	private void OnSlotEnter(SkillSlotModel slotModel)
	{
		_model.OnSlotEnter.OnNext(slotModel);
	}

	private void OnSlotExit(SkillSlotModel slotModel)
	{
		_model.OnSlotExit.OnNext(slotModel);
	}
}
