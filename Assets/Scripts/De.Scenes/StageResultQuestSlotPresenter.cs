using GameCommon.Data;
using GameCommon.Table;
using NemoLib.Mvp;
using UnityEngine;
using VContainer;

public class StageResultQuestSlotPresenter : Presenter
{
	[View]
	private StageResultQuestSlotView _view;

	[InjectModel]
	private StageResultQuestSlotModel _model;

	[Inject]
	private GameManager _gameManager;

	private void OnInject(StageResultQuestSlotModel model)
	{
		if (_model.CampaignTaskId == 0)
		{
			base.gameObject.SetActive(value: false);
			return;
		}
		TCampaignTask tCampaignTask = TCampaignTask.Table[_model.CampaignTaskId];
		var (num, num2) = _gameManager.StageResultInfo.CampaignSubTasks[_model.CampaignTaskId];
		bool flag = num2 <= num;
		_view.TaskTitle.text = tCampaignTask.EventCondition.ToDescription();
		base.gameObject.SetActive(value: true);
		_view.Complete.gameObject.SetActive(flag);
		_view.Count.gameObject.SetActive(!flag);
		_view.Count.text = string.Format("{0}/{1}", num, num2);
	}
}
