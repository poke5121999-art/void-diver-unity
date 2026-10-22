using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class MonsterInfoPanelPresenter : BasePanelPresenter
{
	[View]
	private MonsterInfoPanelView _view;

	[InjectModel]
	private MonsterInfoPanelModel _model;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private GameManager _gameManager;

	private CompositeDisposable _disposable;

	protected override BasePanelModel PanelModel => _model;

	private void Start()
	{
		_view.BackButton.OnClickWithSfx().Subscribe(delegate
		{
			OnBackButtonClick();
		}).AddTo(this);
	}

	protected override void OnDestroy()
	{
		_disposable?.Dispose();
		base.OnDestroy();
	}

	private void OnInject(MonsterInfoPanelModel model)
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		TMonster monsterData = TMonster.Table[_model.MonsterId];
		EMonsterRecordFlags recordFlags = _playerManager.GetMonsterRecordFlags(model.MonsterId);
		bool isBoss = monsterData.MonsterType.IsBoss();
		_view.BossTag.SetActive(monsterData.MonsterType == EMonsterType.Boss);
		_view.MiniBossTag.SetActive(monsterData.MonsterType == EMonsterType.MiniBoss);
		UpdateBaseInfo();
		UpdateWeakness();
		UpdateDescription();
		InitGamePadNav();
		model.RxIsInputEnabled.Where((bool _) => _).Subscribe(delegate
		{
			OnInputEnabled();
		}).AddTo(_disposable);
		model.RxIsInputEnabled.Where((bool _) => !_).Subscribe(delegate
		{
			OnInputDisabled();
		}).AddTo(_disposable);
		void UpdateBaseInfo()
		{
			bool hasFlag = (recordFlags & EMonsterRecordFlags.BaseInfo) != 0;
			UniTask.DelayFrame(1).ContinueWith(delegate
			{
				string text = (hasFlag ? monsterData.Name : "?");
				_view.Name.text = text;
				return text;
			}).Forget();
			_view.CategoryText.text = (hasFlag ? GetCategoryText() : (isBoss ? "?" : ""));
			_view.Stat_Sight_Value.text = (hasFlag ? (monsterData.SightTag.ToLocalText() ?? "") : "?");
			_view.Stat_Speed_Value.text = (hasFlag ? (monsterData.SpeedTag.ToLocalText() ?? "") : "?");
		}
		void UpdateDescription()
		{
			_view.Description_Text.text = (((recordFlags & EMonsterRecordFlags.Description) != 0) ? monsterData.Desc : "???");
		}
		void UpdateWeakness()
		{
			bool flag = (recordFlags & EMonsterRecordFlags.Weakness) != 0;
			Color color = GameCommon.Utility.ColorUtility.ParseOrWhite(flag ? "#EF7767" : "#707070");
			int num = (int)Math.Round(monsterData.FireResistancePercent);
			_view.Elemental_Fire_Text.text = (flag ? GetResistancePercentText(num) : "?");
			_view.Elemental_Fire_Text.color = color;
			_view.Elemental_Fire_WeakTag.SetActive(flag && num < 0);
			_view.Elemental_Fire_NormalTag.SetActive(flag && num == 0);
			_view.Elemental_Fire_StrongTag.SetActive(flag && num > 0);
			int num2 = (int)Math.Round(monsterData.WaterResistancePercent);
			_view.Elemental_Water_Text.text = (flag ? GetResistancePercentText(num2) : "?");
			_view.Elemental_Water_Text.color = color;
			_view.Elemental_Water_WeakTag.SetActive(flag && num2 < 0);
			_view.Elemental_Water_NormalTag.SetActive(flag && num2 == 0);
			_view.Elemental_Water_StrongTag.SetActive(flag && num2 > 0);
			int num3 = (int)Math.Round(monsterData.WindResistancePercent);
			_view.Elemental_Wind_Text.text = (flag ? GetResistancePercentText(num3) : "?");
			_view.Elemental_Wind_Text.color = color;
			_view.Elemental_Wind_WeakTag.SetActive(flag && num3 < 0);
			_view.Elemental_Wind_NormalTag.SetActive(flag && num3 == 0);
			_view.Elemental_Wind_StrongTag.SetActive(flag && num3 > 0);
		}
	}

	private void OnBackButtonClick()
	{
		_model.OnBack.OnNext(Unit.Default);
	}

	private void InitGamePadNav()
	{
		ClearNav();
		SetNavHorizontal(new List<Selectable> { _view.BackButton });
	}

	private string GetCategoryText()
	{
		TMonster tMonster = TMonster.Table[_model.MonsterId];
		if (tMonster.MonsterCategories == null)
		{
			return default(EMonsterCategory).ToLocalText();
		}
		return string.Join(", ", tMonster.MonsterCategories.Select((EMonsterCategory _) => _.ToLocalText()).ToList());
	}

	private string GetResistancePercentText(int percent)
	{
		if (percent > 0)
		{
			return string.Format("<color=#598016>+{0}%</color>", percent);
		}
		if (percent < 0)
		{
			return string.Format("<color=#EF7767>{0}%</color>", percent);
		}
		return string.Format("<color=#707070>{0}%</color>", percent);
	}
}
