using System.Collections.Generic;
using NemoLib.Log;
using NemoLib.Mvp;
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

	private readonly HashSet<int> _encounteredMonsterSet;

	private readonly List<MonsterModel> _bosses;

	private void Start()
	{
	}

	private void OnBossEncountered(MonsterModel boss)
	{
	}

	private void OnBossDeparted(MonsterModel boss)
	{
	}

	private void OnCurrentBossChanged()
	{
	}

	private void OnBuffSlotPointerEnter(InGameBuffSlotModel model)
	{
	}

	private void OnBuffSlotPointerExit(InGameBuffSlotModel model)
	{
	}
}
