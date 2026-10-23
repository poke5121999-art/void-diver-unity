using System;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.AssetManagement;
using UnityEngine.UI;
using NemoLib.Mvp;
using VContainer;

public class RadioToastPresenter : BaseToastPresenter
{
	[InjectModel]
	private RadioToastModel _model;

	[View]
	private RadioToastView _view;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private GameChatManager _chatManager;

	[Inject]
	private PlayerManager _playerManager;

	protected override void OnShow()
	{
		string speakerName = _model.Speaker.GetSpeakerName(_model.TableId, _playerManager.Name, _playerManager.SelectedCharacter.Id);
		_view.Name.text = speakerName;
		_view.Portrait_Image.SetSprite(_assetManager.GetTexturePortraitAsync(_model.Speaker, _model.ImageName));
		_view.Message.text = _model.Message;
		ChatInfo chatInfo = new ChatInfo();
		chatInfo.Type = EChatType.Radio;
		chatInfo.Text = string.Format(TLocalizedText.RadioToastChatFormat.LocalText, speakerName, _model.Message);
		chatInfo.UtcTime = DateTime.UtcNow;
		_chatManager.AddChat(chatInfo);
	}

	private void OnAnimationCompleted()
	{
		Close();
	}
}
