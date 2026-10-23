using UnityEngine;
using NemoLib.Popup;
using GameCommon.Table;
using GameCommon.Enum;
using NemoLib.AssetManagement;
using NemoLib.Log;
using NemoLib.Mvp;
using UniRx;
using VContainer;

public class InGameSkillPanelPresenter : Presenter
{
	[View]
	private InGameSkillPanelView _view;

	[Model]
	private InGameSkillPanelModel _model;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private SeriLogger _logger;

	[Inject]
	private PlayerInputController _inputController;

	[Inject]
	private GamePingManager _pingManager;

	[Inject]
	private ToastManager _toastManager;

	[Inject]
	private GameContext _gameContext;

	[Inject]
	private PlayerManager _playerManager;

	private CompositeDisposable _disposable;

	private CharacterModel CharacterModel => _model.CharacterModel;

	private void OnDestroy()
	{
		_disposable?.Dispose();
	}

	private void Start()
	{
		_gameManager.RxViewTargetClientId.Where((ulong id) => id != ulong.MaxValue).Subscribe(OnViewTargetClientIdChanged).AddTo(this);
	}

	private void OnViewTargetClientIdChanged(ulong targetClientId)
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		CharacterModel characterModel = _gameManager.FindCharacterModel(targetClientId);
		if (characterModel == null)
		{
			_logger.Warning(string.Format("targetClientId {0} is not found in PlayerCharacters", targetClientId));
			return;
		}
		_model.CharacterModel = characterModel;
		RefreshSkillSlots();
		_model.CharacterModel.OnActiveSkillsChanged.Subscribe(delegate
		{
			RefreshSkillSlots();
		}).AddTo(_disposable);
	}

	private void RefreshSkillSlots()
	{
		bool flag = CharacterModel.PassiveSkillIds.Count >= 1;
		_view.Slots[0].SetActive(flag);
		if (flag)
		{
			InGameSkillSlotModel inGameSkillSlotModel = new InGameSkillSlotModel();
			inGameSkillSlotModel.SlotIndex = 0;
			inGameSkillSlotModel.UnitModel = CharacterModel;
			inGameSkillSlotModel.SkillId = ((CharacterModel.PassiveSkillIds.Count >= 1) ? CharacterModel.PassiveSkillIds[0] : 0);
			inGameSkillSlotModel.OnEnter.Subscribe(OnSkillSlotPointEnter).AddTo(this);
			inGameSkillSlotModel.OnExit.Subscribe(OnSkillSlotPointExit).AddTo(this);
			inGameSkillSlotModel.OnClick.Subscribe(OnSkillSlotPointDown).AddTo(this);
			_view.Slots[0].GetComponent<InGameSkillSlotPresenter>().Inject(inGameSkillSlotModel);
		}
		int activeSlotCount = _gameContext.Players[CharacterModel.OwnerClientId].ActiveSlotCount;
		int num = _view.Slots.Length;
		for (int i = 1; i < _view.Slots.Length; i++)
		{
			int num2 = ((i < CharacterModel.GetActiveSkillIds().Count) ? CharacterModel.GetActiveSkillIds()[i] : 0);
			if (i == num - 1)
			{
				_view.Slots[i].SetActive(num2 > 0);
				if (num2 <= 0)
				{
					continue;
				}
			}
			InGameSkillSlotModel inGameSkillSlotModel2 = new InGameSkillSlotModel();
			inGameSkillSlotModel2.SlotIndex = i;
			inGameSkillSlotModel2.UnitModel = CharacterModel;
			inGameSkillSlotModel2.SkillId = num2;
			inGameSkillSlotModel2.RxIsLocked.Value = activeSlotCount + 2 <= i;
			inGameSkillSlotModel2.OnEnter.Subscribe(OnSkillSlotPointEnter).AddTo(this);
			inGameSkillSlotModel2.OnExit.Subscribe(OnSkillSlotPointExit).AddTo(this);
			inGameSkillSlotModel2.OnClick.Subscribe(OnSkillSlotPointDown).AddTo(this);
			_view.Slots[i].GetComponent<InGameSkillSlotPresenter>().Inject(inGameSkillSlotModel2);
		}
	}

	private void OnSkillSlotPointEnter(InGameSkillSlotModel model)
	{
		if (model.SkillId >= 1)
		{
			RectTransform component = _view.Slots[model.SlotIndex].GetComponent<RectTransform>();
			RectTransform component2 = _view.InGameSkillTooltip.GetComponent<RectTransform>();
			component2.anchoredPosition = new Vector2(component.anchoredPosition.x - 400f, component2.anchoredPosition.y);
			_view.InGameSkillTooltip.gameObject.SetActive(true);
			_view.InGameSkillTooltip.GetComponent<Presenter>().Inject(new InGameSkillTooltipModel
			{
				SkillId = model.SkillId,
				ShowModifiedValues = true,
				UnitModel = _model.CharacterModel
			});
		}
	}

	private void OnSkillSlotPointExit(InGameSkillSlotModel model)
	{
		_view.InGameSkillTooltip.gameObject.SetActive(false);
	}

	private void OnSkillSlotPointDown(InGameSkillSlotModel model)
	{
		if (!_inputController.IsPingPressed)
		{
			return;
		}
		if (_gameManager.MyCharacterModel.IsHitable)
		{
			EPingResult ePingResult = _pingManager.TrySendPingBySkill(model.SkillId);
			if (ePingResult != EPingResult.Ok)
			{
				_toastManager.Notice(ePingResult.ToLocalText());
			}
		}
		else
		{
			_toastManager.NoticeFail(TLocalizedText.ActionNotAvailableWhileDead.LocalText);
		}
	}
}
