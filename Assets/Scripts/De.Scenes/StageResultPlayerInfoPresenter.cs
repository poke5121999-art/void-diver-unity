using System;
using GameCommon.Enum;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using TMPro;
using VContainer;

public class StageResultPlayerInfoPresenter : BasePanelPresenter
{
	[InjectModel]
	private StageResultPlayerInfoModel _model;

	[View]
	private StageResultPlayerInfoView _view;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private GameContext _gameContext;

	private const string COLOR_DONE = "#DCDCDC";

	private const string COLOR_NOT_DONE = "#A45646";

	protected override BasePanelModel PanelModel => _model;

	protected void OnInject(StageResultPlayerInfoModel model)
	{
		StageResultPlayerInfo playerInfo = model.PlayerInfo;
		int characterId = playerInfo.CharacterId;
		string userName = playerInfo.UserName;
		if (characterId == 0 || string.IsNullOrEmpty(userName))
		{
			return;
		}
		_view.ImageFace.sprite = _assetManager.GetIconUnitCharacter(characterId);
		bool flag = _gameContext.Players.ContainsKey(model.PlayerInfo.ClientId);
		bool flag2 = model.PlayerInfo.State == EActionState.Dead || model.PlayerInfo.State == EActionState.Dying;
		if (!flag)
		{
			flag2 = true;
		}
		_view.NameText.text = userName.WithColor(flag2 ? COLOR_NOT_DONE : COLOR_DONE);
		_view.NameText.fontStyle = (flag2 ? FontStyles.Strikethrough : FontStyles.Normal);
		_view.DeadDimmer.gameObject.SetActive(flag2);
		_view.EscapeFail.gameObject.SetActive(flag2);
		_view.EscapeSuccess.gameObject.SetActive(!flag2);
	}
}
