using System;
using System.Collections.Generic;
using System.Linq;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.Log;
using NemoLib.Mvp;
using UniRx;
using UnityEngine;
using VContainer;

public class InGameBossStatusPanelPresenter : Presenter
{
	[View]
	private InGameBossStatusPanelView _view;

	[Inject]
	private GameCombatSystem _combatSystem;

	[Inject]
	private SeriLogger _logger;

	[Inject]
	private ToastManager _toastManager;

	private readonly HashSet<int> _encounteredMonsterSet = new HashSet<int>();

	private readonly List<MonsterModel> _bosses = new List<MonsterModel>();

	private void Start()
	{
		OnCurrentBossChanged();
		_combatSystem.OnBossEncountered.Subscribe(OnBossEncountered).AddTo(this);
		_combatSystem.OnBossDeparted.Subscribe(OnBossDeparted).AddTo(this);
	}

	private void OnBossEncountered(MonsterModel boss)
	{
		if (!_bosses.Contains(boss))
		{
			_bosses.Add(boss);
			OnCurrentBossChanged();
		}
	}

	private void OnBossDeparted(MonsterModel boss)
	{
		if (_bosses.Remove(boss))
		{
			OnCurrentBossChanged();
		}
	}

	private void OnCurrentBossChanged()
	{
		_bosses.RemoveAll((MonsterModel _) => _ == null || _.MonsterId == 0);
		if (_bosses.Count > _view.BossSlots.Length)
		{
			_logger.Warning(string.Format("Not enough boss slots. Current boss count: {0}, ", _bosses.Count) + string.Format("Slot count: {0}. ", _view.BossSlots.Length) + "Exceeding bosses will not be displayed.");
		}
		for (int i = 0; i < _view.BossSlots.Length; i++)
		{
			GameObject gameObject = _view.BossSlots[i];
			InGameBossStatusCardModel inGameBossStatusCardModel = new InGameBossStatusCardModel();
			inGameBossStatusCardModel.MonsterModel = ((i < _bosses.Count) ? _bosses[i] : null);
			inGameBossStatusCardModel.OnSlotEnter.Subscribe(OnBuffSlotPointerEnter).AddTo(this);
			inGameBossStatusCardModel.OnSlotExit.Subscribe(OnBuffSlotPointerExit).AddTo(this);
			gameObject.GetComponent<InGameBossStatusCardPresenter>().Inject(inGameBossStatusCardModel);
		}
		_bosses.Select((MonsterModel _) => _.MonsterId).ToArray().ForEach(delegate(int _)
		{
			if (_encounteredMonsterSet.Add(_))
			{
				string bossRadioText = TMonster.Table[_].BossRadioText;
				if (bossRadioText.HasValue())
				{
					_toastManager.Show(new RadioToastModel
					{
						Speaker = ESpeaker.Npc,
						TableId = 700000,
						ImageName = "Default",
						Message = bossRadioText
					});
				}
			}
		});
	}

	private void OnBuffSlotPointerEnter(InGameBuffSlotModel model)
	{
		_view.BuffTooltip.gameObject.SetActive(true);
		_view.BuffTooltip.Inject(new BuffTooltipModel
		{
			BuffId = model.BuffId
		});
	}

	private void OnBuffSlotPointerExit(InGameBuffSlotModel model)
	{
		if (_view.BuffTooltip.gameObject.activeSelf && _view.BuffTooltip.BuffId == model.BuffId)
		{
			_view.BuffTooltip.gameObject.SetActive(false);
		}
	}
}
