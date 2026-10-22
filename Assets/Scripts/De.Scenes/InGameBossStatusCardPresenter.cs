using System;
using System.Collections.Generic;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using UniRx;
using UnityEngine;
using VContainer;

public class InGameBossStatusCardPresenter : Presenter
{
	[InjectModel]
	private InGameBossStatusCardModel _model;

	[View]
	private InGameBossStatusCardView _view;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private GameObjectManager _objectManager;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private GameCombatSystem _combatSystem;

	private CompositeDisposable _disposable;

	private IDisposable _immuneTimerDisposable;

	private readonly Dictionary<int, (GameObject Go, InGameBuffSlotModel Model)> _buffSlots;

	private MonsterModel BossMonsterModel => null;

	protected void OnInject(InGameBossStatusCardModel model)
	{
	}

	private void OnDestroy()
	{
	}

	private void UpdateAllBuffSlots()
	{
	}

	private void EnsureBuffSlot(int buffId)
	{
	}

	private void HideBuffSlot(int buffId)
	{
	}

	private void OnHpChanged()
	{
	}

	private void OnShieldChanged()
	{
	}

	private void OnSuppressionGaugeChanged()
	{
	}

	private void UpdateHpUI(bool forceImmediate = false)
	{
	}

	private void UpdateSuppressionUI()
	{
	}

	private void OnBuffSlotEnter(InGameBuffSlotModel model)
	{
	}

	private void OnBuffSlotExit(InGameBuffSlotModel model)
	{
	}
}
