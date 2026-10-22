using System.Collections.Generic;
using System.Linq;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using VContainer;

public class HudCampaignQuestSlotPresenter : Presenter
{
	[View]
	private HudCampaignQuestSlotView _view;

	[InjectModel]
	private HudCampaignQuestSlotModel _model;

	[Inject]
	private GameCampaignManager _campaignManager;

	[Inject]
	private GameContext _gameContext;

	private CompositeDisposable _disposable;

	private void OnInject(HudCampaignQuestSlotModel model)
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		UpdateUi();
		_campaignManager.OnCampaignUpdated.Subscribe(delegate
		{
			UpdateUi();
		}).AddTo(_disposable);
	}

	private void OnDestroy()
	{
		_disposable?.Dispose();
	}

	private void UpdateUi()
	{
		TCampaign tCampaign = TCampaign.Table[_model.CampaignId];
		_view.Name.text = tCampaign.Name;
		EDifficulty difficulty = _gameContext.Difficulty;
		_view.Easy.gameObject.SetActive(difficulty == EDifficulty.Easy);
		_view.Normal.gameObject.SetActive(difficulty == EDifficulty.Normal);
		_view.Hard.gameObject.SetActive(difficulty == EDifficulty.Hard);
		_view.Insane.gameObject.SetActive(difficulty == EDifficulty.Insane);
		List<CampaignTask> list = _campaignManager.CampaignTasks.Values.OrderBy((CampaignTask _) => _.Id).ToList();
		for (int i = 0; i < _view.Tasks.Length; i++)
		{
			GameObject gameObject = _view.Tasks[i];
			if (i < list.Count)
			{
				CampaignTask campaignTask = list[i];
				gameObject.SetActive(true);
				TextMeshProUGUI textMeshProUGUI = gameObject.FindChildComponent<TextMeshProUGUI>("Text");
				textMeshProUGUI.text = campaignTask.ToLocalString();
				textMeshProUGUI.fontStyle = (campaignTask.IsPermanentlyCompleted ? FontStyles.Strikethrough : FontStyles.Normal);
				gameObject.FindChildComponent<Image>("Mark").gameObject.SetActive(campaignTask.IsCompleted);
			}
			else
			{
				gameObject.SetActive(false);
			}
		}
	}
}
