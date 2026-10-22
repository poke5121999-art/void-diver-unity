using System.Linq;
using Cysharp.Threading.Tasks;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using UnityEngine;
using VContainer;

public class ArchiveMonsterPopupPresenter : GamePopupPresenter
{
	[InjectModel]
	private ArchiveMonsterPopupModel _model;

	[View]
	private ArchiveMonsterPopupView _view;

	[Inject]
	private ToastManager _toastManager;

	[Inject]
	private PlayerManager _playerManager;

	private MonsterSlotRowScrollerModel _scrollerModel;

	private MonsterInfoPanelModel _monsterPanelModel;

	private CompositeDisposable _panelDisposable;

	private MonsterSlotModel _selectedSlot;

	private MonsterSlotModel _focusedSlot;

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	public override bool IsInputActionPopup => true;

	private InputActions.UiActions UiActions => GameInputManager.UiActions;

	public override string OpenSfx => "PopUpOpen";

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		_view.Title.text = EArchiveCategory.Monster.ToLocalText();
		_view.TitleSub.text = _model.FilterThemeType.ToLocalText();
		_view.Empty.SetActive(true);
		_view.MonsterInfoPanel.gameObject.SetActive(false);
		int[] monsterIds = BuildMonsterIds();
		_scrollerModel = new MonsterSlotRowScrollerModel
		{
			MonsterIds = monsterIds
		};
		_scrollerModel.OnSlotClick.Subscribe(OnSlotClick).AddTo(this);
		_scrollerModel.OnSlotEnter.Subscribe(OnSlotEnter).AddTo(this);
		_scrollerModel.OnSlotExit.Subscribe(OnSlotExit).AddTo(this);
		_view.Scroller.GetComponent<MonsterSlotRowScrollerPresenter>().Inject(_scrollerModel);
	}

	private int[] BuildMonsterIds()
	{
		return (from x in TMonster.Table.Values.Where((TMonster x) => x.ThemeTypes != null && x.ThemeTypes.Contains(_model.FilterThemeType)).GroupBy((TMonster x) => x.GroupId)
				select x.Key into x
				orderby x
				select x).ToArray().Select((int groupId) => TMonster.GroupTable[groupId].First().Id).ToArray();
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		GameInputManager gameInputManager = GameInputManager;
		if (gameInputManager != null)
		{
			UiActions.Escape.OnClickWithSfx("ButtonClick").Subscribe(delegate
			{
				OnBack();
			}).AddTo(InputDisposable);
			_scrollerModel.RxIsInputEnabled.Value = _selectedSlot == null;
			if (_selectedSlot != null && _monsterPanelModel != null)
			{
				_monsterPanelModel.RxIsInputEnabled.Value = true;
			}
		}
	}

	protected override void OnInputDisabled()
	{
		base.OnInputDisabled();
		_scrollerModel.RxIsInputEnabled.Value = false;
		if (_monsterPanelModel != null)
		{
			_monsterPanelModel.RxIsInputEnabled.Value = false;
		}
	}

	private void OnBack()
	{
		if (_selectedSlot == null)
		{
			Close();
			return;
		}
		UpdateSelectedObject(null);
		UpdateInfo(_focusedSlot);
	}

	private void OnSlotClick(MonsterSlotModel slotModel)
	{
		if (_playerManager.IsUnknownMonster(slotModel.MonsterId))
		{
			_toastManager.NoticeFail(TLocalizedText.UnknownMonster.LocalText);
			return;
		}
		UpdateSelectedObject(slotModel);
		UpdateInfo(slotModel);
	}

	private void OnSlotEnter(MonsterSlotModel slotModel)
	{
		_focusedSlot = slotModel;
		if (_selectedSlot == null)
		{
			UpdateInfo(_focusedSlot);
		}
	}

	private void OnSlotExit(MonsterSlotModel slotModel)
	{
		_focusedSlot = null;
		if (_selectedSlot == null)
		{
			UpdateInfo(null);
		}
	}

	private void UpdateInfo(MonsterSlotModel slotModel)
	{
		_panelDisposable?.Dispose();
		_panelDisposable = new CompositeDisposable();
		if (slotModel == null)
		{
			_view.Empty.SetActive(true);
			_view.MonsterInfoPanel.gameObject.SetActive(false);
			return;
		}
		_view.Empty.SetActive(false);
		_view.MonsterInfoPanel.gameObject.SetActive(true);
		_monsterPanelModel = new MonsterInfoPanelModel
		{
			MonsterId = slotModel.MonsterId
		};
		if (IsInputEnable)
		{
			_monsterPanelModel.RxIsInputEnabled.Value = _selectedSlot != null;
		}
		_monsterPanelModel.OnBack.Subscribe(delegate
		{
			OnBack();
		}).AddTo(_panelDisposable);
		_view.MonsterInfoPanel.Inject(_monsterPanelModel);
	}

	private void UpdateSelectedObject(MonsterSlotModel slotModel)
	{
		if (_selectedSlot != null)
		{
			_selectedSlot.RxIsSelected.Value = false;
		}
		if (slotModel != null)
		{
			slotModel.RxIsSelected.Value = true;
		}
		_selectedSlot = slotModel;
		if (IsInputEnable)
		{
			_scrollerModel.RxIsInputEnabled.Value = _selectedSlot == null;
			if (_monsterPanelModel != null)
			{
				_monsterPanelModel.RxIsInputEnabled.Value = _selectedSlot != null;
			}
		}
	}
}
