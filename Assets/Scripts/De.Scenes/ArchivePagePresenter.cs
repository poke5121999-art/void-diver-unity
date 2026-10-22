using System;
using System.Linq;
using GameCommon.Enum;
using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using UnityEngine;
using VContainer;

public class ArchivePagePresenter : BasePanelPresenter
{
	[InjectModel]
	private ArchivePageModel _model;

	[View]
	private ArchivePageView _view;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private GameInputManager _inputManager;

	private ArchiveScrollerModel _archiveScrollerModel;

	private TextSlotScrollerModel _textSlotScrollerModel;

	private CompositeDisposable _disposable;

	private EQuestType[] _questTypes;

	private EThemeType[] _themeTypes;

	private EGoodsType[] _itemSubTypes;

	private EWeaponType[] _weaponTypes;

	private EGoodsGradeType[] _accessoryGradeTypes;

	private EGoodsGradeType[] _artifactGradeTypes;

	private EArchiveRecordCategory[] _archiveRecordCategories;

	private EGlossaryCategory[] _glossaryCategories;

	private ArchiveSlotModel _selectedArchiveSlot;

	private ArchiveSlotModel _focusedArchiveSlot;

	protected override BasePanelModel PanelModel => _model;

	private InputActions.UiActions UiActions => _inputManager.UiActions;

	private bool HasSelectedSlot => _selectedArchiveSlot != null;

	protected override void Awake()
	{
		_questTypes = GetEnumValuesExceptNone<EQuestType>();
		_themeTypes = GetEnumValuesExceptNone<EThemeType>().Where((EThemeType _) => _ != EThemeType.Lounge).ToArray();
		_archiveRecordCategories = GetEnumValuesExceptNone<EArchiveRecordCategory>();
		_glossaryCategories = GetEnumValuesExceptNone<EGlossaryCategory>();
		_weaponTypes = GetEnumValuesExceptNone<EWeaponType>();
		_accessoryGradeTypes = GetEnumValuesExceptNone<EGoodsGradeType>();
		_artifactGradeTypes = GetEnumValuesExceptNone<EGoodsGradeType>().Where((EGoodsGradeType _) => _ != EGoodsGradeType.Rare).ToArray();
		_itemSubTypes = new EGoodsType[4]
		{
			EGoodsType.Consumable,
			EGoodsType.Blueprint,
			EGoodsType.MusicDisc,
			EGoodsType.Misc
		};
		base.Awake();
		// TODO(decomp): exact filter of the generic local function GetEnumValuesExceptNone (shared generic body, address 0x0)
		static T[] GetEnumValuesExceptNone<T>() where T : struct, Enum
		{
			return Enum.GetValues(typeof(T)).Cast<T>().Where((T v) => Convert.ToInt32(v) != 0).ToArray();
		}
	}

	private void OnInject(ArchivePageModel model)
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		if (_selectedArchiveSlot != null)
		{
			_selectedArchiveSlot.RxIsSelected.Value = false;
		}
		_selectedArchiveSlot = null;
		_focusedArchiveSlot = null;
		_archiveScrollerModel = new ArchiveScrollerModel();
		_archiveScrollerModel.OnSlotClick.Subscribe(OnArchiveSlotClick).AddTo(_disposable);
		_archiveScrollerModel.OnSlotEnter.Subscribe(OnArchiveSlotEnter).AddTo(_disposable);
		_archiveScrollerModel.OnSlotExit.Subscribe(OnArchiveSlotExit).AddTo(_disposable);
		_view.Category_Scroller.GetComponent<ArchiveScrollerPresenter>().Inject(_archiveScrollerModel);
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
		base.OnInputEnabled();
		if (_inputManager != null)
		{
			UiActions.Escape.OnClickWithSfx("ButtonClick").Subscribe(delegate
			{
				OnBack();
			}).AddTo(InputDisposable);
			if (_selectedArchiveSlot == null)
			{
				_archiveScrollerModel.RxIsInputEnabled.Value = _selectedArchiveSlot == null;
			}
			else
			{
				_textSlotScrollerModel.RxIsInputEnabled.Value = _selectedArchiveSlot != null;
			}
		}
	}

	protected override void OnInputDisabled()
	{
		_archiveScrollerModel.RxIsInputEnabled.Value = false;
		_textSlotScrollerModel.RxIsInputEnabled.Value = false;
		base.OnInputDisabled();
	}

	private void OnDisable()
	{
		_selectedArchiveSlot = null;
		UpdateSubLists(EArchiveCategory.None);
		_disposable?.Dispose();
	}

	private void Close()
	{
		_model.OnEscape.OnNext(Unit.Default);
	}

	private void OnBack()
	{
		if (_selectedArchiveSlot == null)
		{
			_model.OnEscape.OnNext(Unit.Default);
			return;
		}
		_selectedArchiveSlot.RxIsSelected.Value = false;
		_selectedArchiveSlot = null;
		if (IsInputEnabled)
		{
			_textSlotScrollerModel.RxIsInputEnabled.Value = false;
			_archiveScrollerModel.RxIsInputEnabled.Value = true;
		}
		UpdateSubLists((_focusedArchiveSlot != null) ? _focusedArchiveSlot.Category : EArchiveCategory.None);
	}

	private void OnArchiveSlotClick(ArchiveSlotModel slotModel)
	{
		if (_selectedArchiveSlot != null)
		{
			_selectedArchiveSlot.RxIsSelected.Value = false;
		}
		_selectedArchiveSlot = slotModel;
		slotModel.RxIsSelected.Value = true;
		UpdateSubLists(slotModel.Category);
		if (IsInputEnabled)
		{
			_archiveScrollerModel.RxIsInputEnabled.Value = false;
			_textSlotScrollerModel.RxIsInputEnabled.Value = true;
		}
	}

	private void OnArchiveSlotEnter(ArchiveSlotModel slotModel)
	{
		_focusedArchiveSlot = slotModel;
		if (_selectedArchiveSlot == null)
		{
			UpdateSubLists((slotModel != null) ? slotModel.Category : EArchiveCategory.None);
		}
	}

	private void OnArchiveSlotExit(ArchiveSlotModel slotModel)
	{
		_focusedArchiveSlot = null;
		if (_selectedArchiveSlot == null)
		{
			UpdateSubLists(EArchiveCategory.None);
		}
	}

	private void UpdateSubLists(EArchiveCategory category)
	{
		if (category == EArchiveCategory.None)
		{
			_view.List.SetActive(false);
			_view.Empty.SetActive(true);
			return;
		}
		string[] array;
		switch (category)
		{
		case EArchiveCategory.Quest:
			array = _questTypes.Select((EQuestType _) => _.ToLocalText()).ToArray();
			break;
		case EArchiveCategory.Monster:
			array = _themeTypes.Select((EThemeType _) => _.ToLocalText()).ToArray();
			break;
		case EArchiveCategory.Item:
			array = _itemSubTypes.Select((EGoodsType _) => _.ToLocalText()).ToArray();
			break;
		case EArchiveCategory.Weapon:
			array = _weaponTypes.Select((EWeaponType _) => _.ToLocalText()).ToArray();
			break;
		case EArchiveCategory.Accessory:
			array = _accessoryGradeTypes.Select((EGoodsGradeType _) => _.ToLocalText()).ToArray();
			break;
		case EArchiveCategory.Artifact:
			array = _artifactGradeTypes.Select((EGoodsGradeType _) => _.ToLocalText()).ToArray();
			break;
		case EArchiveCategory.Record:
			array = _archiveRecordCategories.Select((EArchiveRecordCategory _) => _.ToLocalText()).ToArray();
			break;
		case EArchiveCategory.Glossary:
			array = _glossaryCategories.Select((EGlossaryCategory _) => _.ToLocalText()).ToArray();
			break;
		default:
			return;
		}
		int num = array.Length;
		_view.List.SetActive(num != 0);
		_view.Empty.SetActive(num == 0);
		_textSlotScrollerModel = new TextSlotScrollerModel
		{
			Texts = array
		};
		_textSlotScrollerModel.OnSlotClick.Subscribe(OnTextSlotClick).AddTo(_disposable);
		_view.List_Scroller.GetComponent<TextSlotScrollerPresenter>().Inject(_textSlotScrollerModel);
	}

	private void OnTextSlotClick(TextSlotModel slotModel)
	{
		if (slotModel == null || _selectedArchiveSlot == null)
		{
			return;
		}
		switch (_selectedArchiveSlot.Category)
		{
		case EArchiveCategory.Quest:
			OnSelectQuestType(_questTypes[slotModel.Index]);
			break;
		case EArchiveCategory.Monster:
			OnSelectThemeType(_themeTypes[slotModel.Index]);
			break;
		case EArchiveCategory.Item:
			OnSelectItemType(_itemSubTypes[slotModel.Index]);
			break;
		case EArchiveCategory.Weapon:
			OnSelectWeaponType(_weaponTypes[slotModel.Index]);
			break;
		case EArchiveCategory.Accessory:
			OnSelectAccessoryType(_accessoryGradeTypes[slotModel.Index]);
			break;
		case EArchiveCategory.Artifact:
			OnSelectArtifactType(_artifactGradeTypes[slotModel.Index]);
			break;
		case EArchiveCategory.Record:
			OnSelectRecordCategory(_archiveRecordCategories[slotModel.Index]);
			break;
		case EArchiveCategory.Glossary:
			OnSelectGlossaryCategory(_glossaryCategories[slotModel.Index]);
			break;
		}
		void OnSelectQuestType(EQuestType questType)
		{
			QuestScrollerPresenter.EFilterType filterType;
			switch (questType)
			{
			case EQuestType.Campaign:
				filterType = QuestScrollerPresenter.EFilterType.ClearedCampaign;
				break;
			case EQuestType.Mission:
				filterType = QuestScrollerPresenter.EFilterType.ClearedMission;
				break;
			case EQuestType.Lounge:
				filterType = QuestScrollerPresenter.EFilterType.ClearedLounge;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			_popupManager.Open<ArchiveQuestPopupPresenter>(new ArchiveQuestPopupModel
			{
				FilterType = filterType
			});
		}
		void OnSelectThemeType(EThemeType themeType)
		{
			_popupManager.Open<ArchiveMonsterPopupPresenter>(new ArchiveMonsterPopupModel
			{
				FilterThemeType = themeType
			});
		}
		void OnSelectItemType(EGoodsType goodsType)
		{
			_popupManager.Open<ArchiveGoodsPopupPresenter>(new ArchiveGoodsPopupModel
			{
				FilterGoodsType = goodsType
			});
		}
		void OnSelectWeaponType(EWeaponType weaponType)
		{
			_popupManager.Open<ArchiveGoodsPopupPresenter>(new ArchiveGoodsPopupModel
			{
				FilterGoodsType = EGoodsType.Weapon,
				FilterWeaponType = weaponType
			});
		}
		void OnSelectAccessoryType(EGoodsGradeType gradeType)
		{
			_popupManager.Open<ArchiveGoodsPopupPresenter>(new ArchiveGoodsPopupModel
			{
				FilterGoodsType = EGoodsType.Accessory,
				FilterGradeType = gradeType
			});
		}
		void OnSelectArtifactType(EGoodsGradeType gradeType)
		{
			_popupManager.Open<ArchiveGoodsPopupPresenter>(new ArchiveGoodsPopupModel
			{
				FilterGoodsType = EGoodsType.Artifact,
				FilterGradeType = gradeType
			});
		}
		void OnSelectRecordCategory(EArchiveRecordCategory recordCategory)
		{
			if (recordCategory == EArchiveRecordCategory.Note)
			{
				_popupManager.Open<ArchiveGoodsPopupPresenter>(new ArchiveGoodsPopupModel
				{
					FilterGoodsType = EGoodsType.Note
				});
			}
		}
		void OnSelectGlossaryCategory(EGlossaryCategory glossaryCategory)
		{
			_popupManager.Open<ArchiveGlossaryPopupPresenter>(new ArchiveGlossaryPopupModel
			{
				FilterCategory = glossaryCategory
			});
		}
	}
}
