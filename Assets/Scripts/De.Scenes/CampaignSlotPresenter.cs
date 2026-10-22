using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using UnityEngine.UI;
using VContainer;

public class CampaignSlotPresenter : BaseSlotPresenter<CampaignSlotModel>
{
	[InjectModel]
	private CampaignSlotModel _model;

	[View]
	private CampaignSlotView _view;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private GameCampaignManager _gameCampaignManager;

	[Inject]
	private AssetManager _assetManager;

	protected override CampaignSlotModel SlotModel => _model;

	protected override Button Button => _view.Button;

	private void OnInject(CampaignSlotModel model)
	{
		InitSlot(model);
		TCampaign tCampaign = TCampaign.Table[model.CampaignId];
		_view.Text.text = tCampaign.Name;
		_view.Chapter.gameObject.SetActive(tCampaign.Category == ECampaignCategory.Story);
		if (tCampaign.Category == ECampaignCategory.Story)
		{
			_view.Chapter.text = tCampaign.GetChapterString();
		}
		_view.Boss.gameObject.SetActive(tCampaign.Category == ECampaignCategory.Normal && tCampaign.IsBossCampaign);
		_view.Sector.text = ((System.Enum)tCampaign.ThemeType).ToLocalText();
		bool active = tCampaign.Category == ECampaignCategory.Story && _playerManager.IsClearedCampaign(model.CampaignId);
		_view.CompletedMark.gameObject.SetActive(active);
		bool flag = _playerManager.CheckStateConditions(tCampaign.UnlockConditions);
		_view.LockIcon.gameObject.SetActive(!flag);
		_view.NewTag.gameObject.SetActive(tCampaign.Category == ECampaignCategory.Story && !_playerManager.IsClearedCampaign(model.CampaignId));
		if (!flag)
		{
			_view.Button.GetComponent<ButtonController>().SetLocked(true);
		}
		EAnomalyType campaignAnomalyType = _gameCampaignManager.GetCampaignAnomalyType(model.CampaignId);
		_view.IconAnomaly.sprite = _assetManager.GetIconAnomaly(campaignAnomalyType);
		_view.LevelLayout_Text.text = tCampaign.RecommendedLevel.ToString();
	}
}
