using NemoLib.AssetManagement;
using NemoLib.Log;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.Mvp;
using UniRx;
using UnityEngine;
using VContainer;

public class InGameBuffSlotPresenter : BaseSlotPresenter<InGameBuffSlotModel>
{
	[InjectModel]
	private InGameBuffSlotModel _model;

	[View]
	private InGameBuffSlotView _view;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private SeriLogger _logger;

	[Inject]
	private GameManager _gameManager;

	private float _startTime;

	private float _endTime;

	private float _duration;

	protected override InGameBuffSlotModel SlotModel => _model;

	private void OnInject(Model model)
	{
		Init();
	}

	private void Init()
	{
		InitSlot(_model);
		if (_model.TargetUnitModel == null)
		{
			base.gameObject.SetActive(false);
			return;
		}
		if (_model.TargetUnitModel.RxBuffs.TryGetValue(_model.BuffId, out var value))
		{
			InitializeVisual(value);
			base.gameObject.SetActive(true);
			_model.TargetUnitModel.RxBuffs.ObserveChange().Where((int id) => id == _model.BuffId).Subscribe(delegate
			{
				OnBuffUpdated();
			})
				.AddTo(Disposable);
		}
		else
		{
			base.gameObject.SetActive(false);
		}
	}

	public void Refresh()
	{
		if (_model.TargetUnitModel != null && _model.TargetUnitModel.RxBuffs.TryGetValue(_model.BuffId, out var value))
		{
			UpdateEndTimeAndStack(value);
		}
		else
		{
			base.gameObject.SetActive(false);
		}
	}

	private void InitializeVisual(UnitBuff unitBuff)
	{
		TBuff tBuff = TBuff.Table[_model.BuffId];
		_duration = tBuff.Duration;
		_endTime = unitBuff.EndTime;
		_startTime = ((_duration > 0f) ? (_endTime - _duration) : _gameManager.TimeNow);
		_view.Icon.sprite = ((tBuff.EffectTag == EStatusEffectTag.None) ? _assetManager.GetIconBuff(_model.BuffId) : _assetManager.GetIconStatusEffectTag(tBuff.EffectTag));
		_view.Gauge.gameObject.SetActive(_duration > 0f && _endTime > 0f);
		_view.Gauge.value = 0f;
		UpdateStackText(unitBuff.StackCount);
	}

	private void UpdateEndTimeAndStack(UnitBuff unitBuff)
	{
		if (unitBuff != null)
		{
			_endTime = unitBuff.EndTime;
			if (_duration > 0f)
			{
				_startTime = _endTime - _duration;
			}
			UpdateStackText(unitBuff.StackCount);
		}
	}

	private void UpdateStackText(int stack)
	{
		if (!(_view.TextStack == null))
		{
			bool flag = stack > 1;
			_view.TextStack.gameObject.SetActive(flag);
			if (flag)
			{
				_view.TextStack.text = stack.ToString();
			}
		}
	}

	private void OnBuffUpdated()
	{
		if (_model.TargetUnitModel.RxBuffs.TryGetValue(_model.BuffId, out var value))
		{
			UpdateEndTimeAndStack(value);
			return;
		}
		_model.TriggerSlotExit();
		base.gameObject.SetActive(false);
	}

	private void FixedUpdate()
	{
		if (!base.gameObject.activeSelf || _duration <= 0f)
		{
			return;
		}
		float timeNow = _gameManager.TimeNow;
		if (_endTime <= 0f)
		{
			return;
		}
		if (_endTime <= timeNow)
		{
			_view.Gauge.value = 1f;
			base.gameObject.SetActive(false);
			return;
		}
		float num = _endTime - _startTime;
		if (num > 0f)
		{
			float num2 = Mathf.Clamp01((_endTime - timeNow) / num);
			_view.Gauge.value = 1f - num2;
		}
		else
		{
			_view.Gauge.value = 1f;
		}
	}
}
