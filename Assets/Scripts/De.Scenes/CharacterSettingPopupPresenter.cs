using System.Collections.Generic;
using System.Linq;
using System;
using Cysharp.Threading.Tasks;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using NemoLib.Popup;
using TMPro;
using UniRx;
using UnityEngine.UI;
using UnityEngine;
using VContainer;

public class CharacterSettingPopupPresenter : GamePopupPresenter
{
	[View]
	private CharacterSettingPopupView _view;

	[InjectModel]
	private CharacterSettingPopupModel _model;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private ToastManager _toastManager;

	[Inject]
	private SoundManager _soundManager;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private PlayerManager _playerManager;

	private CharacterSettingScrollerModel _characterSettingScrollerModel;

	private Dictionary<int, Character> _characters;

	private int _selectedCharacterId;

	private int[] _characterSkinIds;

	private int _selectedSkinId;

	private int _currentSkinPresetId;

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	public override bool IsInputActionPopup => true;

	private InputActions.UiActions UiActions => GameInputManager.UiActions;

	private InputActions.UiPadActions UiPadActions => GameInputManager.UiPadActions;

	private GamePlayer MyPlayer => _gameManager.MyPlayer;

	private Character SelectedCharacter => _characters[_selectedCharacterId];

	private int ActiveSkillSlotCount => _gameManager.MyPlayer.ActiveSlotCount;

	public override string OpenSfx => "PopUpOpen";

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		_characters = _playerManager.Characters.Values.ToDictionary((Character c) => c.Id, (Character c) => c.Clone());
		_selectedCharacterId = _playerManager.SelectedCharacter.Id;
		_characterSkinIds = _playerManager.GetAvailableSkinIds(_selectedCharacterId);
		_selectedSkinId = TCharacterSkinPreset.Table[_characters[_selectedCharacterId].SkinPresetId].CharacterSkinId;
		_view.Skin_PrevButton.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnMoveToPrevSkin();
		}).AddTo(this);
		_view.Skin_NextButton.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnMoveToNextSkin();
		}).AddTo(this);
		_view.SkinPreset_PrevButton.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnMoveToPrevSkinPreset();
		}).AddTo(this);
		_view.SkinPreset_NextButton.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnMoveToNextSkinPreset();
		}).AddTo(this);
		Character[] characters = _characters.Values.OrderBy((Character c) => c.Id).Select((Character c) => c.Clone()).ToArray();
		CharacterSettingScrollerPresenter component = _view.CharacterSettingScroller.GetComponent<CharacterSettingScrollerPresenter>();
		_characterSettingScrollerModel = new CharacterSettingScrollerModel
		{
			Characters = characters
		};
		_characterSettingScrollerModel.OnSlotClick.Subscribe(OnCharacterSlotClick).AddTo(this);
		_characterSettingScrollerModel.RxSelectedCharacterId.Value = _selectedCharacterId;
		component.Inject(_characterSettingScrollerModel);
		_view.Skill_SettingButton.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnSkillSettingButtonClick();
		}).AddTo(this);
		_gameManager.MyPlayer.RxCharacter.Subscribe(OnCharacterUpdated).AddTo(this);
		UpdateSkinUi();
		Selectable[] items = new Selectable[3] { _view.Skin_Button, _view.SkinPreset_Button, _view.Skill_SettingButton };
		SetNavVertical(items);
		SetDefaultNav(_view.Skill_SettingButton);
	}

	private void InitGamePadNav()
	{
		Selectable[] items = new Selectable[3] { _view.Skin_Button, _view.SkinPreset_Button, _view.Skill_SettingButton };
		SetNavVertical(items);
		SetDefaultNav(_view.Skill_SettingButton);
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		GameInputManager gameInputManager = GameInputManager;
		if (gameInputManager != null)
		{
			UiActions.Escape.OnClickWithSfx("ButtonClick").Subscribe(delegate
			{
				Close();
			}).AddTo(InputDisposable);
			UiPadActions.TabLeft.OnClickWithSfx("ButtonClick").Subscribe(delegate
			{
				OnTabLeftClick();
			}).AddTo(InputDisposable);
			UiPadActions.TabRight.OnClickWithSfx("ButtonClick").Subscribe(delegate
			{
				OnTabRightClick();
			}).AddTo(InputDisposable);
			UiPadActions.OptionLeft.OnClickWithSfx("ButtonClick").Subscribe(delegate
			{
				OnOptionLeftClick();
			}).AddTo(InputDisposable);
			UiPadActions.OptionRight.OnClickWithSfx("ButtonClick").Subscribe(delegate
			{
				OnOptionRightClick();
			}).AddTo(InputDisposable);
		}
	}

	private void OnOptionLeftClick()
	{
		GameObject selectedObject = InputManager.GetSelectedObject();
		if (selectedObject == _view.Skin_Button.gameObject)
		{
			OnMoveToPrevSkin();
			return;
		}
		selectedObject = InputManager.GetSelectedObject();
		if (selectedObject == _view.SkinPreset_Button.gameObject)
		{
			OnMoveToPrevSkinPreset();
		}
	}

	private void OnOptionRightClick()
	{
		GameObject selectedObject = InputManager.GetSelectedObject();
		if (selectedObject == _view.Skin_Button.gameObject)
		{
			OnMoveToNextSkin();
			return;
		}
		selectedObject = InputManager.GetSelectedObject();
		if (selectedObject == _view.SkinPreset_Button.gameObject)
		{
			OnMoveToNextSkinPreset();
		}
	}

	private void OnTabLeftClick()
	{
		List<int> list = _characters.Keys.OrderBy((int id) => id).ToList();
		int num = list.IndexOf(_selectedCharacterId);
		if (num > 0)
		{
			_soundManager.PlaySfx("select2");
			SetCharacter(list[num - 1]);
		}
	}

	private void OnTabRightClick()
	{
		List<int> list = _characters.Keys.OrderBy((int id) => id).ToList();
		int num = list.IndexOf(_selectedCharacterId);
		if (num < list.Count - 1)
		{
			_soundManager.PlaySfx("select2");
			SetCharacter(list[num + 1]);
		}
	}

	private void SetCharacter(int id)
	{
		if (_selectedCharacterId != id)
		{
			_selectedCharacterId = id;
			_characterSkinIds = _playerManager.GetAvailableSkinIds(_selectedCharacterId);
			_selectedSkinId = TCharacterSkinPreset.Table[_characters[_selectedCharacterId].SkinPresetId].CharacterSkinId;
			UpdateCharacterUis();
			UpdateSkillInfo();
			UpdateSkinUi();
			UpdateSkinAsync().Forget();
			_gameChannel.SendToHost(new ReqChangeCharacter
			{
				Character = _characters[_selectedCharacterId]
			});
			_characterSettingScrollerModel.RxSelectedCharacterId.Value = id;
		}
	}

	private void SetSkin(int skinId)
	{
		if (_selectedSkinId != skinId)
		{
			_selectedSkinId = skinId;
			int id = TCharacterSkinPreset.GroupTable[skinId].First().Id;
			_characters[_selectedCharacterId].SkinPresetId = id;
			UpdateSkinAsync().Forget();
			UpdateSkinUi();
			_gameChannel.SendToHost(new ReqChangeCharacterSkin
			{
				SkinPresetId = id
			});
		}
	}

	private void SetSkinPreset(int skinPresetId)
	{
		if (_characters[_selectedCharacterId].SkinPresetId != skinPresetId)
		{
			_characters[_selectedCharacterId].SkinPresetId = skinPresetId;
			UpdateSkinAsync().Forget();
			UpdateSkinUi();
			_gameChannel.SendToHost(new ReqChangeCharacterSkin
			{
				SkinPresetId = skinPresetId
			});
		}
	}

	private void UpdateSkinUi()
	{
		TCharacterSkin tCharacterSkin = TCharacterSkin.Table[_selectedSkinId];
		_view.Skin_Text.text = tCharacterSkin.Name;
		int num = Array.IndexOf(_characterSkinIds, _selectedSkinId);
		_view.Skin_PrevButton.gameObject.SetActive(num > 0);
		_view.Skin_NextButton.gameObject.SetActive(num < _characterSkinIds.Length - 1);
		IList<TCharacterSkinPreset> presets = TCharacterSkinPreset.GroupTable[_selectedSkinId];
		int num2 = presets.IndexOf(TCharacterSkinPreset.Table[_characters[_selectedCharacterId].SkinPresetId]);
		_view.SkinPreset_PrevButton.gameObject.SetActive(num2 > 0);
		_view.SkinPreset_NextButton.gameObject.SetActive(num2 < presets.Count - 1);
		UpdatePagenationCells(presets.Count, num2);
	}

	private void UpdatePagenationCells(int presetCount, int currentIndex)
	{
		for (int i = 0; i < _view.PagenationCells.Length; i++)
		{
			GameObject gameObject = _view.PagenationCells[i];
			if (i < presetCount)
			{
				gameObject.SetActive(value: true);
				Image image = gameObject.FindChildComponent<Image>("Normal");
				Image image2 = gameObject.FindChildComponent<Image>("Select");
				image.gameObject.SetActive(i != currentIndex);
				image2.gameObject.SetActive(i == currentIndex);
			}
			else
			{
				gameObject.SetActive(value: false);
			}
		}
	}

	private void UpdateCharacterUis()
	{
		TCharacter tCharacter = TCharacter.Table[_selectedCharacterId];
		_view.ChaInfo_ChaName.text = tCharacter.Name;
		_view.ChaInfo_ChaSubTitle.text = tCharacter.Title;
		_view.Desc_Text.text = tCharacter.Desc;
		for (int i = 0; i < _view.ChaInfo_TagGroup.Length; i++)
		{
			GameObject gameObject = _view.ChaInfo_TagGroup[i];
			if (i < tCharacter.Tags.Length)
			{
				gameObject.FindChildComponent<TextMeshProUGUI>("Text").text = tCharacter.Tags[i];
				gameObject.SetActive(value: true);
			}
			else
			{
				gameObject.SetActive(value: false);
			}
		}
		_view.Portrait.SetSprite(_assetManager.GetTexturePortraitAsync(ESpeaker.Character, _selectedCharacterId.ToString()));
	}

	private void UpdateSkillInfo()
	{
		for (int i = 0; i < _view.Skill_Group.Length; i++)
		{
			GameObject gameObject = _view.Skill_Group[i];
			if (i < _gameManager.MyPlayer.ActiveSlotCount)
			{
				SkillSlotPresenter component = gameObject.GetComponent<SkillSlotPresenter>();
				int[] selectedActiveSkillIds = SelectedCharacter.SelectedActiveSkillIds;
				int value = ((i < selectedActiveSkillIds.Length) ? SelectedCharacter.SelectedActiveSkillIds[i] : (-1));
				SkillSlotModel skillSlotModel = new SkillSlotModel();
				skillSlotModel.RxSkillId.Value = value;
				component.Inject(skillSlotModel);
				gameObject.SetActive(value: true);
			}
			else
			{
				gameObject.SetActive(value: false);
			}
		}
	}

	private async UniTask UpdateSkinAsync()
	{
		int newSkinPresetId = _characters[_selectedCharacterId].SkinPresetId;
		if (_currentSkinPresetId == newSkinPresetId)
		{
			return;
		}
		int weaponId = _characters[_selectedCharacterId].WeaponId;
		int num = ((_currentSkinPresetId > 0) ? TCharacterSkinPreset.Table[_currentSkinPresetId].CharacterId : 0);
		int characterId = TCharacterSkinPreset.Table[newSkinPresetId].CharacterId;
		_currentSkinPresetId = newSkinPresetId;
		if (num != characterId)
		{
			UnitSkeletonData unitSkeletonData = await _assetManager.GetCharacterSkeletonDataAsync(_selectedCharacterId);
			if (_view == null)
			{
				return;
			}
			_view.SkinPreset_Skeleton.skeletonDataAsset = unitSkeletonData.Sw;
			_view.SkinPreset_Skeleton.Initialize(overwrite: true);
		}
		_view.SkinPreset_Skeleton.Skeleton.UpdateCharacterSkin(newSkinPresetId, weaponId);
	}

	private void OnCharacterUpdated(Character character)
	{
		_characters[character.Id] = character.Clone();
		UpdateCharacterUis();
		UpdateSkillInfo();
		UpdateSkinAsync().Forget();
	}

	private void OnCharacterSlotClick(CharacterSettingSlotModel model)
	{
		if (model.CharacterId < 1)
		{
			_toastManager.Notice(TLocalizedText.FeatureComingSoon.LocalText);
			return;
		}
		_soundManager.PlaySfx("select2");
		SetCharacter(model.CharacterId);
	}

	private void OnSkillSettingButtonClick()
	{
		_popupManager.Open<SkillSelectPopupPresenter>();
	}

	private void OnMoveToPrevSkin()
	{
		int num = Array.IndexOf(_characterSkinIds, _selectedSkinId);
		if (num >= 1)
		{
			SetSkin(_characterSkinIds[num - 1]);
		}
	}

	private void OnMoveToNextSkin()
	{
		int num = Array.IndexOf(_characterSkinIds, _selectedSkinId);
		if (num < _characterSkinIds.Length - 1)
		{
			SetSkin(_characterSkinIds[num + 1]);
		}
	}

	private void OnMoveToPrevSkinPreset()
	{
		IList<TCharacterSkinPreset> list = TCharacterSkinPreset.GroupTable[_selectedSkinId];
		int num = list.IndexOf(TCharacterSkinPreset.Table[_characters[_selectedCharacterId].SkinPresetId]);
		if (num >= 1)
		{
			SetSkinPreset(list[num - 1].Id);
		}
	}

	private void OnMoveToNextSkinPreset()
	{
		IList<TCharacterSkinPreset> list = TCharacterSkinPreset.GroupTable[_selectedSkinId];
		int num = list.IndexOf(TCharacterSkinPreset.Table[_characters[_selectedCharacterId].SkinPresetId]);
		if (num < list.Count - 1)
		{
			SetSkinPreset(list[num + 1].Id);
		}
	}
}
