using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.Popup;
using UnityEngine;
using UnityEngine.InputSystem;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using UniRx;
using VContainer;

public class SkillSelectPopupPresenter : GamePopupPresenter
{
	[View]
	private SkillSelectPopupView _view;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private ToastManager _toastManager;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private SoundManager _soundManager;

	[Inject]
	private GameChannel _gameChannel;

	private Character _character;

	private CompositeDisposable _scrollerDisposable;

	private SkillSelectScrollerModel _scrollerModel;

	private int _targetIndex = -1;

	private int _focusedInfoSlotIndex;

	private float _infoSlotScrollPosition;

	private List<SkillSlotModel> _skillSelectSlotModels = new List<SkillSlotModel>();

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	private GamePlayer MyPlayer => _gameManager.MyPlayer;

	private int ActiveSkillSlotCount => _gameManager.MyPlayer.GetTalentEffectValue(ETalentEffectType.SkillSlotCount) + 2;

	public override bool IsInputActionPopup => true;

	public override string OpenSfx => "TalentOpen";

	private InputActions.UiActions UiActions => GameInputManager.UiActions;

	private SkillSlotModel TargetSkillSlot => _skillSelectSlotModels[_targetIndex];

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		_character = _gameManager.MyPlayer.Character.Clone();
		_view.CharacterImage.sprite = _assetManager.GetIconUnitCharacter(_character.Id, "Default");
		_targetIndex = 0;
		int i;
		for (i = 0; i < 4; i++)
		{
			GameObject slotObject = _view.SkillGroup[i];
			int skillId = ((i < _character.SelectedActiveSkillIds.Length) ? _character.SelectedActiveSkillIds[i] : (-1));
			int activeSlotCount = _gameManager.MyPlayer.ActiveSlotCount;
			SkillSlotModel slotModel = new SkillSlotModel();
			slotModel.SlotIndex = i;
			slotModel.SlotType = ESkillSlotType.Active;
			slotModel.RxSkillId.Value = skillId;
			slotModel.ShowLockIcon = i >= activeSlotCount;
			slotModel.OnClick.Subscribe(OnInputSlotClicked).AddTo(this);
			_skillSelectSlotModels.Add(slotModel);
			slotObject.GetComponent<SkillSlotPresenter>().Inject(slotModel);
		}
		GameObject equipmentSlotObject = _view.SkillGroup[i];
		if (_character.EquipmentActiveSkillId < 1)
		{
			equipmentSlotObject.SetActive(value: false);
		}
		else
		{
			SkillSlotModel equipmentSlotModel = new SkillSlotModel();
			equipmentSlotModel.SlotIndex = i;
			equipmentSlotModel.SlotType = ESkillSlotType.Equipment;
			equipmentSlotModel.RxSkillId.Value = _character.EquipmentActiveSkillId;
			equipmentSlotModel.OnClick.Subscribe(OnInputSlotClicked).AddTo(this);
			_skillSelectSlotModels.Add(equipmentSlotModel);
			equipmentSlotObject.GetComponent<SkillSlotPresenter>().Inject(equipmentSlotModel);
			equipmentSlotObject.SetActive(value: true);
		}
		_skillSelectSlotModels.First().RxIsSelected.Value = true;
		_view.SkillTitle_TextTitle.text = TLocalizedText.SkillOwnerFormat.Format(TCharacter.Table[_character.Id].Name);
		SetKeyGuide();
		RefreshPanel();
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		UiActions.Escape.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			CloseWithSave();
		}).AddTo(InputDisposable);
		if (_scrollerModel != null)
		{
			_scrollerModel.RxIsInputEnabled.Value = true;
		}
	}

	protected override void OnInputDisabled()
	{
		if (_scrollerModel != null)
		{
			_scrollerModel.RxIsInputEnabled.Value = false;
		}
		base.OnInputDisabled();
	}

	private void SetScroller()
	{
		if (_scrollerModel != null)
		{
			_scrollerModel.RxIsInputEnabled.Value = false;
			_focusedInfoSlotIndex = _scrollerModel.FocusedIndex;
			_infoSlotScrollPosition = _scrollerModel.ScrollPosition;
		}
		_scrollerDisposable?.Dispose();
		_scrollerDisposable = new CompositeDisposable();
		_scrollerModel = new SkillSelectScrollerModel
		{
			TrialIndex = _targetIndex,
			EquippedSkillIds = _character.SelectedActiveSkillIds,
			FocusedIndex = _focusedInfoSlotIndex,
			ScrollPosition = _infoSlotScrollPosition
		};
		_scrollerModel.OnSlotClick.Subscribe(OnSkillSelectInfoSlotClick).AddTo(_scrollerDisposable);
		_scrollerModel.OutOfLeft.Subscribe(delegate
		{
			OnTabSlot(-1);
		}).AddTo(_scrollerDisposable);
		_scrollerModel.OutOfRight.Subscribe(delegate
		{
			OnTabSlot(1);
		}).AddTo(_scrollerDisposable);
		if (IsInputEnable)
		{
			_scrollerModel.RxIsInputEnabled.Value = true;
		}
		_view.ScrollView.gameObject.SetActive(value: true);
		_view.ScrollView.GetComponent<SkillSelectScrollerPresenter>().Inject(_scrollerModel);
	}

	private void OnTabSlot(int direction)
	{
		int index = _targetIndex + direction;
		if (index >= 0 && index < _skillSelectSlotModels.Count)
		{
			SkillSlotModel slotModel = _skillSelectSlotModels[index];
			_soundManager.PlaySfx("select");
			_targetIndex = slotModel.SlotIndex;
			_skillSelectSlotModels.ForEach(delegate(SkillSlotModel m)
			{
				m.RxIsSelected.Value = m.SlotIndex == _targetIndex;
			});
			SetKeyGuide();
			RefreshPanel();
		}
	}

	private void OnInputSlotClicked(SkillSlotModel slotModel)
	{
		_soundManager.PlaySfx("select");
		_targetIndex = slotModel.SlotIndex;
		_skillSelectSlotModels.ForEach(delegate(SkillSlotModel m)
		{
			m.RxIsSelected.Value = m.SlotIndex == _targetIndex;
		});
		SetKeyGuide();
		RefreshPanel();
	}

	private void SetKeyGuide()
	{
		for (int i = 0; i < _view.KeyPrompt.Length; i++)
		{
			_view.KeyPrompt[i].SetActive(i == _targetIndex);
		}
	}

	private bool IsSlotLocked(SkillSlotModel slot)
	{
		if (slot.SlotType != ESkillSlotType.Active)
		{
			return false;
		}
		return slot.SlotIndex >= _gameManager.MyPlayer.ActiveSlotCount;
	}

	private void RefreshPanel()
	{
		SkillSlotModel targetSkillSlot = TargetSkillSlot;
		if (targetSkillSlot.SlotType == ESkillSlotType.Equipment)
		{
			ShowReadOnlySkillInfo(targetSkillSlot.SkillId);
		}
		else if (IsSlotLocked(targetSkillSlot))
		{
			ShowLocked();
		}
		else
		{
			ShowSelectableSlot(targetSkillSlot);
		}
	}

	private void ShowReadOnlySkillInfo(int skillId)
	{
		_view.ScrollView.gameObject.SetActive(value: false);
		_view.SlotEmpty.gameObject.SetActive(value: false);
		_view.SlotLocked.gameObject.SetActive(value: false);
		_view.SlotEmptyEquipment.gameObject.SetActive(value: false);
		_view.EmptySkillGuide.SetActive(value: false);
		_view.SkillInfoDisplaySlot.gameObject.SetActive(value: true);
		_view.EquipmentSkillGuide.SetActive(value: true);
		_view.SkillInfoDisplaySlot.Inject(new SkillInfoDisplaySlotModel
		{
			SkillId = skillId,
			ShowArrow = false
		});
	}

	private void ShowLocked()
	{
		_view.ScrollView.gameObject.SetActive(value: false);
		_view.SlotEmpty.gameObject.SetActive(value: false);
		_view.SlotEmptyEquipment.gameObject.SetActive(value: false);
		_view.EmptySkillGuide.SetActive(value: false);
		_view.EquipmentSkillGuide.SetActive(value: false);
		_view.SlotLocked.gameObject.SetActive(value: true);
		_view.SkillInfoDisplaySlot.gameObject.SetActive(value: false);
	}

	private void ShowSelectableSlot(SkillSlotModel slot)
	{
		int skillId = slot.SkillId;
		_view.SlotLocked.gameObject.SetActive(value: false);
		_view.SlotEmptyEquipment.gameObject.SetActive(value: false);
		_view.EmptySkillGuide.SetActive(value: false);
		_view.EquipmentSkillGuide.SetActive(value: false);
		_view.SkillInfoDisplaySlot.gameObject.SetActive(skillId > 0);
		_view.SlotEmpty.gameObject.SetActive(skillId <= 0);
		if (skillId > 0)
		{
			_view.SkillInfoDisplaySlot.Inject(new SkillInfoDisplaySlotModel
			{
				SkillId = slot.SkillId,
				ShowArrow = true
			});
		}
		SetScroller();
	}

	private void OnSkillSelectInfoSlotClick(SkillSelectInfoSlotModel model)
	{
		if (_targetIndex < 0)
		{
			return;
		}
		_soundManager.PlaySfx("select2");
		foreach (SkillSlotModel slotModel in _skillSelectSlotModels)
		{
			if (slotModel.SlotIndex != _targetIndex && slotModel.SkillId > 0 && _gameManager.MyPlayer.GetBaseSkillId(slotModel.SkillId) == _gameManager.MyPlayer.GetBaseSkillId(model.SkillId))
			{
				_toastManager.Notice(TLocalizedText.SkillUnequippedFormat.Format(TSkill.Table[slotModel.SkillId].Name));
				slotModel.RxSkillId.Value = -1;
				RefreshSlot(slotModel);
			}
		}
		_skillSelectSlotModels[_targetIndex].RxSkillId.Value = model.SkillId;
		RefreshSlot(_skillSelectSlotModels[_targetIndex]);
		_character.SelectedActiveSkillIds = _skillSelectSlotModels.Select((SkillSlotModel m) => m.SkillId).ToArray();
		RefreshPanel();
	}

	private void RefreshSlot(SkillSlotModel slotModel)
	{
		if (slotModel.SlotIndex >= 0 && slotModel.SlotIndex < _view.SkillGroup.Length)
		{
			_skillSelectSlotModels[slotModel.SlotIndex].RxSkillId.Value = slotModel.SkillId;
		}
	}

	private void CloseWithSave()
	{
		int[] activeSkillIds = _skillSelectSlotModels.Select((SkillSlotModel m) => m.SkillId).ToArray();
		if (_character != null)
		{
			_gameChannel.SendToHost(new ReqChangeCharacterSkills
			{
				ActiveSkillIds = activeSkillIds
			});
		}
		Close();
	}
}
