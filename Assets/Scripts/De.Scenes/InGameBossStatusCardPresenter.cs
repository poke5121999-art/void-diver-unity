using System;
using System.Collections.Generic;
using System.Linq;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using NemoLib.AssetManagement;
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

	private readonly Dictionary<int, (GameObject Go, InGameBuffSlotModel Model)> _buffSlots = new Dictionary<int, (GameObject, InGameBuffSlotModel)>();

	private MonsterModel BossMonsterModel => _model.MonsterModel;

	protected void OnInject(InGameBossStatusCardModel model)
	{
		_disposable?.Dispose();
		if (_model.MonsterModel == null)
		{
			foreach (KeyValuePair<int, (GameObject Go, InGameBuffSlotModel Model)> buffSlot in _buffSlots)
			{
				if (buffSlot.Value.Go != null && buffSlot.Value.Go.activeSelf)
				{
					_model.OnSlotExit.OnNext(buffSlot.Value.Model);
				}
			}
			base.gameObject.SetActive(false);
			return;
		}
		base.gameObject.SetActive(true);
		_disposable = new CompositeDisposable();
		TMonster tMonster = TMonster.Table[_model.MonsterModel.MonsterId];
		_view.TextBossName.text = tMonster.Name;
		_view.TextBossName_BossTag.SetActive(tMonster.MonsterType == EMonsterType.Boss);
		_view.TextBossName_MiniBossTag.SetActive(tMonster.MonsterType == EMonsterType.MiniBoss);
		_model.MonsterModel.RxHp.Subscribe(delegate
		{
			OnHpChanged();
		}).AddTo(_disposable);
		_model.MonsterModel.RxShield.Subscribe(delegate
		{
			OnShieldChanged();
		}).AddTo(_disposable);
		_model.MonsterModel.RxSuppressionGauge.Subscribe(delegate
		{
			OnSuppressionGaugeChanged();
		}).AddTo(_disposable);
		_model.MonsterModel.RxSuppressionImmunityEndTime.Subscribe(delegate
		{
			OnSuppressionGaugeChanged();
		}).AddTo(_disposable);
		_model.MonsterModel.RxBuffs.ObserveChange().Subscribe(delegate
		{
			UpdateAllBuffSlots();
		}).AddTo(_disposable);
		UpdateAllBuffSlots();
		UpdateHpUI(forceImmediate: true);
		_view.WeakPointGauge.GetComponent<Presenter>().Inject(new MonsterWeakPointGaugeModel
		{
			MonsterModel = _model.MonsterModel
		});
	}

	private void OnDestroy()
	{
		_disposable?.Dispose();
		_immuneTimerDisposable?.Dispose();
	}

	private void UpdateAllBuffSlots()
	{
		if (_model.MonsterModel == null)
		{
			return;
		}
		HashSet<int> hashSet = new HashSet<int>();
		foreach (KeyValuePair<int, UnitBuff> rxBuff in _model.MonsterModel.RxBuffs)
		{
			UnitBuff value = rxBuff.Value;
			if (TBuff.Table.TryGetValue(value.Id, out var value2) && value2.ShowBuffIcon && value.IsActive && value.StackCount > 0)
			{
				hashSet.Add(value.Id);
				EnsureBuffSlot(value.Id);
			}
		}
		foreach (KeyValuePair<int, (GameObject Go, InGameBuffSlotModel Model)> item in new List<KeyValuePair<int, (GameObject, InGameBuffSlotModel)>>(_buffSlots))
		{
			if (hashSet.Contains(item.Key))
			{
				continue;
			}
			// TODO(decomp): the branch taken when the buff is still present and shown is not visible in the pseudo-C (assumed to keep the slot) @0x180800450
			if (_model.MonsterModel.Buffs.ContainsKey(item.Key))
			{
				UnitBuff unitBuff = _model.MonsterModel.Buffs[item.Key];
				if (TBuff.Table.TryGetValue(unitBuff.Id, out var value3) && value3.ShowBuffIcon && unitBuff.IsActive && unitBuff.StackCount > 0)
				{
					continue;
				}
			}
			HideBuffSlot(item.Key);
		}
	}

	private void EnsureBuffSlot(int buffId)
	{
		if (_buffSlots.TryGetValue(buffId, out var value))
		{
			if (!value.Go.activeSelf)
			{
				value.Go.SetActive(true);
			}
			value.Go.GetComponent<InGameBuffSlotPresenter>().Refresh();
			return;
		}
		GameObject gameObject = _assetManager.InstantiateBuffSlot();
		gameObject.transform.parent = _view.HpSlider_StatusEffectTags.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		gameObject.name = string.Format("BossBuff_{0}", buffId);
		InGameBuffSlotModel inGameBuffSlotModel = new InGameBuffSlotModel
		{
			BuffId = buffId,
			TargetUnitModel = _model.MonsterModel
		};
		inGameBuffSlotModel.OnEnter.Subscribe(OnBuffSlotEnter).AddTo(this);
		inGameBuffSlotModel.OnExit.Subscribe(OnBuffSlotExit).AddTo(this);
		_buffSlots.Add(buffId, (gameObject, inGameBuffSlotModel));
		gameObject.GetComponent<InGameBuffSlotPresenter>().Inject(inGameBuffSlotModel);
	}

	private void HideBuffSlot(int buffId)
	{
		if (_buffSlots.TryGetValue(buffId, out var value) && value.Go.activeSelf)
		{
			_model.OnSlotExit.OnNext(value.Model);
			value.Go.SetActive(false);
		}
	}

	private void OnHpChanged()
	{
		UpdateHpUI();
	}

	private void OnShieldChanged()
	{
		UpdateHpUI();
	}

	private void OnSuppressionGaugeChanged()
	{
		UpdateSuppressionUI();
	}

	private void UpdateHpUI(bool forceImmediate = false)
	{
		if (!(_model.MonsterModel.HpMax <= 0f))
		{
			_view.HpSlider.GetComponent<HpAfterImageTweenGauge>().SetHpWithGuard((int)_model.MonsterModel.Hp, (int)_model.MonsterModel.HpMax, (int)_model.MonsterModel.Shield, forceImmediate);
		}
	}

	private void UpdateSuppressionUI()
	{
		_immuneTimerDisposable?.Dispose();
		_immuneTimerDisposable = null;
		bool isSuppressionImmune = _model.MonsterModel.IsSuppressionImmune;
		_view.GroggySlider.gameObject.SetActive(!isSuppressionImmune);
		if (!isSuppressionImmune)
		{
			_view.GroggySlider.GetComponent<ImageTweenGauge>().SetRatio(_model.MonsterModel.SuppressionGaugeValue / _model.MonsterModel.EffectiveMaxSuppressionGauge, immediate: true);
			return;
		}
		float num = _model.MonsterModel.SuppressionImmunityEndTime - _gameManager.TimeNow;
		if (!(num <= 0f))
		{
			_immuneTimerDisposable = Observable.Timer(TimeSpan.FromSeconds(num)).Subscribe(delegate
			{
				OnSuppressionGaugeChanged();
			});
			_immuneTimerDisposable.AddTo(_disposable);
		}
	}

	private void OnBuffSlotEnter(InGameBuffSlotModel model)
	{
		_model.OnSlotEnter.OnNext(model);
	}

	private void OnBuffSlotExit(InGameBuffSlotModel model)
	{
		_model.OnSlotExit.OnNext(model);
	}
}
