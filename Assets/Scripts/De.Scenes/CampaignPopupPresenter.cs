using System.Linq;
using Cysharp.Threading.Tasks;
using GameCommon.Table;
using GameCommon.Enum;
using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using UnityEngine;
using VContainer;

public class CampaignPopupPresenter : GamePopupPresenter
{
	private enum ELayout
	{
		None = 0,
		List = 1,
		Detail = 2
	}

	[View]
	private CampaignPopupView _view;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private GameCampaignManager _campaignManager;

	[Inject]
	private GameContext _gameContext;

	[Inject]
	private ToastManager _toastManager;

	[Inject]
	private RoomContext _roomContext;

	[Inject]
	private PlayerManager _playerManager;

	private ELayout _layout;

	private ECampaignCategory _category;

	private int _campaignId;

	private CampaignListPanelModel _listPanelModel;

	private CampaignDetailPanelModel _detailPanelModel;

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	public override bool IsInputActionPopup => true;

	private InputActions.UiActions UiActions => GameInputManager.UiActions;

	private InputActions.UiPadActions UiPadActions => GameInputManager.UiPadActions;

	public override string OpenSfx => "NPC_AI_End";

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		bool flag = TCampaign.GroupTable[ECampaignCategory.Normal].All((TCampaign _) => _playerManager.CheckStateConditions(_.UnlockConditions));
		if (_gameContext.IsTutorial)
		{
			_category = ECampaignCategory.Tutorial;
		}
		else if (flag || _campaignManager.HasNewStoryCampaign())
		{
			_category = ECampaignCategory.Story;
		}
		else
		{
			_category = ECampaignCategory.Normal;
		}
		_campaignManager.OnCampaignStarted.Subscribe(delegate((int, EDifficulty) _)
		{
		}).AddTo(OpenDisposable);
		_view.CategoryCommon.SetActive(_category != ECampaignCategory.Tutorial);
		_view.CategoryTutorial.SetActive(_category == ECampaignCategory.Tutorial);
		_view.CategoryCommon_Toggle_Normal.OnValueTrueWithSfx().Subscribe(delegate
		{
			OnCategoryToggleValueChange(ECampaignCategory.Normal);
		}).AddTo(OpenDisposable);
		_view.CategoryCommon_Toggle_Story.OnValueTrueWithSfx().Subscribe(delegate
		{
			OnCategoryToggleValueChange(ECampaignCategory.Story);
		}).AddTo(OpenDisposable);
		_view.CategoryCommon_Toggle_Story_NewTag.gameObject.SetActive(_campaignManager.HasNewStoryCampaign());
		if (_category != ECampaignCategory.Story)
		{
			_view.CategoryCommon_Toggle_Normal.GetComponent<ToggleController>().SetIsOnWithoutNotify(true);
			_view.CategoryCommon_Toggle_Story.GetComponent<ToggleController>().SetIsOnWithoutNotify(false);
		}
		else
		{
			_view.CategoryCommon_Toggle_Story.GetComponent<ToggleController>().SetIsOnWithoutNotify(true);
			_view.CategoryCommon_Toggle_Normal.GetComponent<ToggleController>().SetIsOnWithoutNotify(false);
		}
		SetLayout(ELayout.List);
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		UiActions.Escape.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnBack();
		}).AddTo(InputDisposable);
		if (_category != ECampaignCategory.Tutorial)
		{
			UiPadActions.TabLeft.OnClickWithSfx("ButtonClick").Subscribe(delegate
			{
				OnTabLeft();
			}).AddTo(InputDisposable);
			UiPadActions.TabRight.OnClickWithSfx("ButtonClick").Subscribe(delegate
			{
				OnTabRight();
			}).AddTo(InputDisposable);
		}
		SetLayoutInputEnabled(value: true);
	}

	protected override void OnInputDisabled()
	{
		base.OnInputDisabled();
		SetLayoutInputEnabled(value: false);
	}

	private void SetLayoutInputEnabled(bool value)
	{
		if (_listPanelModel != null)
		{
			_listPanelModel.RxIsInputEnabled.Value = _layout == ELayout.List && value;
		}
		if (_detailPanelModel != null)
		{
			_detailPanelModel.RxIsInputEnabled.Value = _layout == ELayout.Detail && value;
		}
	}

	private void SetLayout(ELayout layout)
	{
		_layout = layout;
		switch (layout)
		{
		case ELayout.List:
			_view.CampaignListPanel.gameObject.SetActive(value: true);
			_view.CampaignDetailPanel.gameObject.SetActive(value: false);
			_listPanelModel = new CampaignListPanelModel
			{
				Category = _category
			};
			_listPanelModel.OnSlotClick.Subscribe(OnCampaignSlotClick).AddTo(OpenDisposable);
			_view.CampaignListPanel.Inject(_listPanelModel);
			break;
		case ELayout.Detail:
			_view.CampaignListPanel.gameObject.SetActive(value: false);
			_view.CampaignDetailPanel.gameObject.SetActive(value: true);
			_detailPanelModel = new CampaignDetailPanelModel
			{
				CampaignId = _campaignId
			};
			_view.CampaignDetailPanel.Inject(_detailPanelModel);
			break;
		}
		SetLayoutInputEnabled(IsInputEnable);
	}

	private void OnBack()
	{
		if (_layout == ELayout.List)
		{
			Close();
		}
		else if (_layout == ELayout.Detail)
		{
			SetLayout(ELayout.List);
		}
	}

	private void OnCampaignSlotClick(int campaignId)
	{
		_campaignId = campaignId;
		SetLayout(ELayout.Detail);
	}

	private void OnTabLeft()
	{
		if (_category != ECampaignCategory.Normal)
		{
			_view.CategoryCommon_Toggle_Normal.isOn = true;
		}
	}

	private void OnTabRight()
	{
		if (_category != ECampaignCategory.Story)
		{
			_view.CategoryCommon_Toggle_Story.isOn = true;
		}
	}

	private void OnCategoryToggleValueChange(ECampaignCategory category)
	{
		if (_category != category)
		{
			InputManager.ClearSelectedPosition();
			_category = category;
			SetLayout(ELayout.List);
		}
	}
}
