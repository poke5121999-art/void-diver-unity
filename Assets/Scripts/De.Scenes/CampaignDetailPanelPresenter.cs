using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GameCommon.Data;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using NemoLib.Popup;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UniRx;
using UnityEngine;
using VContainer;

public class CampaignDetailPanelPresenter : BasePanelPresenter
{
	[View]
	private CampaignDetailPanelView _view;

	[InjectModel]
	private CampaignDetailPanelModel _model;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private GameCampaignManager _campaignManager;

	[Inject]
	private ToastManager _toastManager;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private GameContext _gameContext;

	[Inject]
	private RoomContext _roomContext;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private GameInputManager _gameInputManager;

	[SerializeField]
	private CampaignDetailBackgroundController _backgroundController;

	private CompositeDisposable _disposable;

	private int _firstRewardCount;

	private int _rewardCount;

	protected override BasePanelModel PanelModel => _model;

	private InputActions.UiPadActions UiPadActions => _gameInputManager.UiPadActions;

	private EDifficulty Difficulty
	{
		get
		{
			return _gameContext.RxDifficulty.Value;
		}
		set
		{
			_gameContext.RxDifficulty.Value = value;
		}
	}

	private void OnInject(CampaignDetailPanelModel model)
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		TCampaign tCampaign = TCampaign.Table[model.CampaignId];
		ECampaignCategory category = tCampaign.Category;
		bool isCleared = _playerManager.IsClearedCampaign(model.CampaignId);
		EAnomalyType campaignAnomalyType = _campaignManager.GetCampaignAnomalyType(model.CampaignId);
		_view.Info_Anomaly_Icon.sprite = _assetManager.GetIconAnomaly(campaignAnomalyType);
		_view.Info_Anomaly_Name.gameObject.SetActive(campaignAnomalyType != EAnomalyType.None);
		_view.Info_Anomaly_Name.text = campaignAnomalyType.ToLocalText();
		_view.Info_Anomaly_Description.text = campaignAnomalyType.ToLocalText("Desc");
		_view.Info_Layout_Chapter.text = tCampaign.GetChapterString();
		_view.Info_Layout_Name.text = tCampaign.Name;
		_view.Info_Layout_Boss.SetActive(tCampaign.Category == ECampaignCategory.Normal && tCampaign.IsBossCampaign);
		_view.Info_Completed.SetActive(category == ECampaignCategory.Story && isCleared);
		_view.Info_NewTag.gameObject.SetActive(category == ECampaignCategory.Story && !isCleared);
		UpdateDifficultyUI();
		_view.Info_Level_Value.text = tCampaign.RecommendedLevel.ToString();
		_view.Info_Region_TextRegion.text = tCampaign.ThemeType.ToLocalText();
		_view.Dialog_Desc.text = tCampaign.Desc;
		ECampaignCategory category2 = tCampaign.Category;
		GoodsData[] firstRewards = tCampaign.FirstRewards;
		GoodsData[] repeatRewards = tCampaign.RepeatRewards;
		BuildFirstRewardGoods(firstRewards?.Select((GoodsData data) => data.ToGoodsAsBase()).ToArray(), category2 == ECampaignCategory.Normal, isCleared);
		BuildRewardGoods(repeatRewards?.Select((GoodsData data) => data.ToGoodsAsBase()).ToArray());
		_view.Info_StartCampaignButton.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			StartCampaign().Forget();
		}).AddTo(_disposable);
		_view.Info_Difficulty_PreButton.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			DecreaseDifficulty();
		}).AddTo(_disposable);
		_view.Info_Difficulty_NextButton.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			IncreaseDifficulty();
		}).AddTo(_disposable);
		_model.OnConfirm.Subscribe(delegate
		{
			StartCampaign().Forget();
		}).AddTo(_disposable);
		_firstRewardCount = ((category2 == ECampaignCategory.Normal || firstRewards == null) ? 0 : firstRewards.Length);
		_rewardCount = ((repeatRewards != null) ? repeatRewards.Length : 0);
		_backgroundController.ChangeAndPlayThemeAnimation(tCampaign.ThemeType);
		InitGamePadNav();
		model.RxIsInputEnabled.Where((bool enabled) => enabled).Subscribe(delegate
		{
			OnInputEnabled();
		}).AddTo(_disposable);
		model.RxIsInputEnabled.Where((bool enabled) => !enabled).Subscribe(delegate
		{
			OnInputDisabled();
		}).AddTo(_disposable);
	}

	protected override void OnDestroy()
	{
		_disposable?.Dispose();
		base.OnDestroy();
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		UiPadActions.OptionLeft.OnClickWithSfx("ButtonClick").Subscribe(OnOptionLeftClick).AddTo(InputDisposable);
		UiPadActions.OptionRight.OnClickWithSfx("ButtonClick").Subscribe(OnOptionRightClick).AddTo(InputDisposable);
	}

	private void InitGamePadNav()
	{
		ClearNav();
		List<List<Selectable>> list = new List<List<Selectable>>();
		list.Add(new List<Selectable> { _view.Info_Difficulty_PadButton });
		if (_firstRewardCount > 0)
		{
			list.Add(_view.Info_FirstReward_Slots.Take(_firstRewardCount).Select((GameObject go) => go.GetComponent<CampaignFirstRewardSlotPresenter>().Selectable).ToList());
		}
		if (_rewardCount > 0)
		{
			list.Add(_view.Info_Reward_Slots.Take(_rewardCount).Select((GameObject go) => (Selectable)go.GetComponent<GoodsSlotView>().Slot).ToList());
		}
		list.Add(new List<Selectable> { _view.Info_StartCampaignButton });
		list.ForEach(delegate(List<Selectable> row)
		{
			SetNavHorizontal(row);
		});
		for (int i = 0; i < list.Count - 1; i++)
		{
			List<Selectable> list2 = list[i];
			List<Selectable> list3 = list[i + 1];
			int num = list2.Count - list3.Count;
			for (int j = -num; j + num < list2.Count; j++)
			{
				Selectable from = list2[j + num];
				Selectable to = list3[(j < 0) ? 0 : Math.Min(j, list3.Count - 1)];
				ConnectNav(from, ENavDir.Down, to);
			}
			for (int k = num; k + -num < list3.Count; k++)
			{
				Selectable from2 = list3[k + -num];
				Selectable to2 = list2[(k < 0) ? 0 : Math.Min(k, list2.Count - 1)];
				ConnectNav(from2, ENavDir.Up, to2);
			}
		}
		Selectable infoStartCampaignButton = _view.Info_StartCampaignButton;
		SetDefaultNav(infoStartCampaignButton);
	}

	private void OnOptionLeftClick(InputAction.CallbackContext _)
	{
		if (_gameInputManager.GetSelectedObject() == _view.Info_Difficulty_PadButton.gameObject)
		{
			_view.Info_Difficulty_PreButton.Submit();
		}
	}

	private void OnOptionRightClick(InputAction.CallbackContext _)
	{
		if (_gameInputManager.GetSelectedObject() == _view.Info_Difficulty_PadButton.gameObject)
		{
			_view.Info_Difficulty_NextButton.Submit();
		}
	}

	private void UpdateDifficultyUI()
	{
		EDifficulty difficulty = Difficulty;
		_view.Info_Difficulty_Text.text = difficulty.ToLocalText();
		_view.Info_Difficulty_BgEasy.gameObject.SetActive(Difficulty == EDifficulty.Easy);
		_view.Info_Difficulty_BgNormal.gameObject.SetActive(Difficulty == EDifficulty.Normal);
		_view.Info_Difficulty_BgHard.gameObject.SetActive(Difficulty == EDifficulty.Hard);
		_view.Info_Difficulty_BgInsane.gameObject.SetActive(Difficulty == EDifficulty.Insane);
		_view.Info_Difficulty_Layout_TextHard.gameObject.SetActive(Difficulty < EDifficulty.Insane);
		_view.Info_Difficulty_Layout_TextInsane.gameObject.SetActive(Difficulty >= EDifficulty.Insane);
		_view.Info_Difficulty_PreButton.gameObject.SetActive(Difficulty > EDifficulty.Easy);
		_view.Info_Difficulty_NextButton.gameObject.SetActive(Difficulty < EDifficulty.Insane);
		_view.Info_Difficulty_PreButton_Easy.SetActive(Difficulty == EDifficulty.Easy);
		_view.Info_Difficulty_PreButton_Normal.SetActive(Difficulty == EDifficulty.Normal);
		_view.Info_Difficulty_PreButton_Hard.SetActive(Difficulty == EDifficulty.Hard);
		_view.Info_Difficulty_NextButton_Normal.SetActive(Difficulty == EDifficulty.Normal);
		_view.Info_Difficulty_NextButton_Hard.SetActive(Difficulty == EDifficulty.Hard);
		_view.Info_Difficulty_NextButton_Insane.SetActive(Difficulty == EDifficulty.Insane);
	}

	private void BuildFirstRewardGoods(Goods[] rewards, bool isNormalCampaign, bool isCleared)
	{
		_view.Info_FirstReward_TextDesc.gameObject.SetActive(isNormalCampaign);
		for (int i = 0; i < _view.Info_FirstReward_Slots.Length; i++)
		{
			GameObject gameObject = _view.Info_FirstReward_Slots[i];
			if (isNormalCampaign || rewards == null || rewards.Length <= i)
			{
				gameObject.SetActive(value: false);
				continue;
			}
			gameObject.SetActive(value: true);
			Presenter component = gameObject.GetComponent<CampaignFirstRewardSlotPresenter>();
			component.Inject(new CampaignFirstRewardSlotModel
			{
				Goods = rewards[i],
				IsReceipted = isCleared
			});
		}
	}

	private void BuildRewardGoods(Goods[] rewards)
	{
		for (int i = 0; i < _view.Info_Reward_Slots.Length; i++)
		{
			GameObject gameObject = _view.Info_Reward_Slots[i];
			if (rewards == null || rewards.Length <= i)
			{
				gameObject.SetActive(value: false);
				continue;
			}
			gameObject.SetActive(value: true);
			Goods goods = rewards[i];
			GoodsSlotModel goodsSlotModel = new GoodsSlotModel(goods);
			goodsSlotModel.ShowItemCount = true;
			goodsSlotModel.OnClick.Subscribe(delegate
			{
				_popupManager.Open<GoodsTooltipPopupPresenter>(new GoodsTooltipPopupModel
				{
					Goods = goods
				});
			}).AddTo(_disposable);
			gameObject.GetComponent<GoodsSlotPresenter>().Inject(goodsSlotModel);
		}
	}

	private async UniTask StartCampaign()
	{
		EAnomalyType anomalyType = _campaignManager.GetCampaignAnomalyType(_model.CampaignId);
		if (!_roomContext.IsSingleMode && _gameManager.PlayerCount < 3 && !(await _popupManager.ShowConfirmationTextPopupAsync(TLocalizedText.ConfirmHostStartCampaign.LocalText)))
		{
			return;
		}
		if (_gameManager.PlayerCount != 1 && !_roomContext.IsSingleMode)
		{
			_gameManager.StartVoteCampaign(_model.CampaignId, _gameContext.RxDifficulty.Value, anomalyType);
		}
		else
		{
			_gameManager.StartCampaign(_model.CampaignId, _gameContext.RxDifficulty.Value, anomalyType);
		}
	}

	private void DecreaseDifficulty()
	{
		if (Difficulty == EDifficulty.Easy)
		{
			_toastManager.NoticeFail(TLocalizedText.LowestDifficulty.LocalText);
		}
		else
		{
			Difficulty = Difficulty - 1;
		}
		UpdateDifficultyUI();
	}

	private void IncreaseDifficulty()
	{
		if (Difficulty == EDifficulty.Insane)
		{
			_toastManager.NoticeFail(TLocalizedText.HighestDifficulty.LocalText);
		}
		else
		{
			Difficulty = Difficulty + 1;
		}
		UpdateDifficultyUI();
	}
}
