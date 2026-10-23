using GameCommon.Enum;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using UniRx;
using UnityEngine;
using VContainer;

public class MonsterWeakPointGaugePresenter : Presenter
{
	[InjectModel]
	private MonsterWeakPointGaugeModel _model;

	[View]
	private MonsterWeakPointGaugeView _view;

	[Inject]
	private AssetManager _assetManager;

	private MonsterModel _monsterModel => _model.MonsterModel;

	protected void OnInject(MonsterWeakPointGaugeModel model)
	{
		model.MonsterModel.RxState.Subscribe(OnStateChanged).AddTo(this);
		model.MonsterModel.RxWeakPointType.Subscribe(OnWeakPointTypeChanged).AddTo(this);
		model.MonsterModel.RxWeakPointStackDamage.Subscribe(OnWeakPointStackDamageChanged).AddTo(this);
	}

	private void OnWeakPointTypeChanged(EElementalType type)
	{
		if (type == EElementalType.None)
		{
			_view.Anchor.SetActive(value: false);
			return;
		}
		Sprite iconWeakPoint = _assetManager.GetIconWeakPoint(type);
		_view.FrontImage.sprite = iconWeakPoint;
		_view.BackImage.sprite = iconWeakPoint;
	}

	private void OnStateChanged(EActionState state)
	{
		bool active = state != EActionState.Dying && state != EActionState.Dead && state != EActionState.ReservedDespawn;
		_view.FrontImage.gameObject.SetActive(active);
		_view.BackImage.gameObject.SetActive(active);
	}

	private void OnWeakPointStackDamageChanged(float stackDamage)
	{
		if (_monsterModel.WeakPointElementalType != EElementalType.None)
		{
			_view.Anchor.SetActive(stackDamage > 0f);
			float hpMax = _monsterModel.HpMax;
			_view.FrontImage.fillAmount = stackDamage / (hpMax * _monsterModel.MonsterData.WeakpointGaugeHpPercent / 100f);
		}
	}
}
