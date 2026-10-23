using System.Linq;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using VContainer;

public class HudLoungeQuestSlotPresenter : Presenter
{
	[View]
	private HudLoungeQuestSlotView _view;

	[InjectModel]
	private HudLoungeQuestSlotModel _model;

	[Inject]
	private GameLoungeQuestManager _loungeQuestManager;

	[Inject]
	private PlayerManager _playerManager;

	private CompositeDisposable _disposable;

	private void OnInject(HudLoungeQuestSlotModel model)
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		UpdateUI();
		_loungeQuestManager.OnQuestUpdated.Where(((LoungeQuest oldValue, LoungeQuest newValue) data) => data.newValue.Id == _model.LoungeQuestId).Subscribe(delegate
		{
			UpdateUI();
		}).AddTo(_disposable);
	}

	private void OnDestroy()
	{
		_disposable?.Dispose();
	}

	private void UpdateUI()
	{
		TLoungeQuest tLoungeQuest = TLoungeQuest.Table[_model.LoungeQuestId];
		_view.Name.text = tLoungeQuest.Name;
		_view.MainIcon.SetActive(tLoungeQuest.Category == ELoungeQuestCategory.Main);
		_view.SubIcon.SetActive(tLoungeQuest.Category == ELoungeQuestCategory.Sub);
		if (_loungeQuestManager.TryGetQuest(tLoungeQuest.Id, out var loungeQuest) && loungeQuest.IsStarted)
		{
			base.gameObject.SetActive(true);
			int num = ((loungeQuest.Tasks != null) ? loungeQuest.Tasks.Length : 0);
			for (int i = 0; i < _view.Tasks.Length; i++)
			{
				GameObject gameObject = _view.Tasks[i];
				if (i < num)
				{
					LoungeQuestTask loungeQuestTask = loungeQuest.Tasks[i];
					gameObject.SetActive(true);
					TextMeshProUGUI textMeshProUGUI = gameObject.FindChildComponent<TextMeshProUGUI>("Text");
					textMeshProUGUI.text = loungeQuestTask.ToLocalString();
					textMeshProUGUI.fontStyle = (loungeQuestTask.IsCompleted ? FontStyles.Strikethrough : FontStyles.Normal);
					gameObject.FindChildComponent<Image>("Mark").gameObject.SetActive(loungeQuestTask.IsCompleted);
				}
				else
				{
					gameObject.SetActive(false);
				}
			}
		}
		else
		{
			base.gameObject.SetActive(false);
		}
	}
}
