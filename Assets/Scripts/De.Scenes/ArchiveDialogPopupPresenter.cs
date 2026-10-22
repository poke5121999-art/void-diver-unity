using Cysharp.Threading.Tasks;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.Mvp;
using SomeNet.Collections;
using TMPro;
using UniRx;
using UnityEngine;
using VContainer;

public class ArchiveDialogPopupPresenter : GamePopupPresenter
{
	[InjectModel]
	private ArchiveDialogPopupModel _model;

	[View]
	private ArchiveDialogPopupView _view;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private DialogReplayService _dialogReplayService;

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	public override bool IsInputActionPopup => true;

	private InputActions.UiActions UiActions => GameInputManager.UiActions;

	public override string OpenSfx => "PopUpOpen";

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		_view.Top_QuestType.text = _model.QuestType.ToLocalText();
		_view.Top_Title.text = _model.QuestType.GetQuestName(_model.TableId);
		long[] visibleDialogReplayIds = _dialogReplayService.GetVisibleDialogReplayIds(_model.QuestType, _model.TableId, _model.DisplayFullDialog);
		if (visibleDialogReplayIds != null)
		{
			visibleDialogReplayIds.ForEach(delegate(long id)
			{
				GameObject gameObject = Object.Instantiate(_view.PlayerMessageSlotTemplate, _view.MessageRoot.transform, false);
				gameObject.SetActive(true);
				TDialogReplay tDialogReplay = TDialogReplay.Table[id];
				TextMeshProUGUI textMeshProUGUI = gameObject.FindChildComponent<TextMeshProUGUI>("Player");
				TextMeshProUGUI textMeshProUGUI2 = gameObject.FindChildComponent<TextMeshProUGUI>("Speaker");
				TextMeshProUGUI textMeshProUGUI3 = gameObject.FindChildComponent<TextMeshProUGUI>("Message");
				if (tDialogReplay.Speaker == ESpeaker.Player)
				{
					textMeshProUGUI.gameObject.SetActive(true);
					textMeshProUGUI2.gameObject.SetActive(false);
					textMeshProUGUI.text = _playerManager.Name;
				}
				else
				{
					textMeshProUGUI.gameObject.SetActive(false);
					textMeshProUGUI2.gameObject.SetActive(true);
					textMeshProUGUI2.text = tDialogReplay.GetSpeakerName(_playerManager.Name, _playerManager.SelectedCharacter.Id);
				}
				textMeshProUGUI3.text = tDialogReplay.Message;
			});
		}
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		UiActions.Escape.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			Close();
		}).AddTo(InputDisposable);
	}
}
