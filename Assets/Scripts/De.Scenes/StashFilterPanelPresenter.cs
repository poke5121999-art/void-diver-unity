using System;
using System.Collections.Generic;
using System.Linq;
using GameCommon.Enum;
using NemoLib.Mvp;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class StashFilterPanelPresenter : BasePanelPresenter
{
	[InjectModel]
	private StashFilterPanelModel _model;

	[View]
	private StashFilterPanelView _view;

	private CompositeDisposable _disposable;

	private readonly List<Selectable> _toggleSelectables = new List<Selectable>();

	private Toggle[] _toggles;

	private EGoodsType[][] _filterTypes;

	protected override BasePanelModel PanelModel => _model;

	protected override void OnDestroy()
	{
		base.OnDestroy();
		_disposable?.Dispose();
	}

	private void OnInject(StashFilterPanelModel model)
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		InitFilterToggles();
		InitGamePadNav();
		model.RxIsInputEnabled.Where((bool x) => x).Subscribe(delegate
		{
			OnInputEnabled();
		}).AddTo(_disposable);
		model.RxIsInputEnabled.Where((bool x) => !x).Subscribe(delegate
		{
			OnInputDisabled();
		}).AddTo(_disposable);
	}

	private void InitFilterToggles()
	{
		EGoodsType[] second = new EGoodsType[5]
		{
			EGoodsType.None,
			EGoodsType.Weapon,
			EGoodsType.Accessory,
			EGoodsType.Artifact,
			EGoodsType.Consumable
		};
		EGoodsType[] array = Enum.GetValues(typeof(EGoodsType)).Cast<EGoodsType>().Except(second).ToArray();
		_filterTypes = new EGoodsType[6][]
		{
			null,
			new EGoodsType[1] { EGoodsType.Weapon },
			new EGoodsType[1] { EGoodsType.Accessory },
			new EGoodsType[1] { EGoodsType.Artifact },
			new EGoodsType[1] { EGoodsType.Consumable },
			array
		};
		_toggles = new Toggle[_view.Toggles.Length];
		for (int i = 0; i < _view.Toggles.Length; i++)
		{
			_toggles[i] = _view.Toggles[i].GetComponent<Toggle>();
			EGoodsType[] allowedTypes = _filterTypes[i];
			_toggles[i].OnValueTrueWithSfx().Subscribe(delegate
			{
				_model.RxAllowedSubTypes.Value = allowedTypes;
				_model.OnUserToggled.OnNext(allowedTypes);
			}).AddTo(_disposable);
		}
		_model.RxAllowedSubTypes.Subscribe(ApplyFilterToToggles).AddTo(_disposable);
	}

	private void ApplyFilterToToggles(EGoodsType[] allowed)
	{
		for (int i = 0; i < _toggles.Length; i++)
		{
			Toggle toggle = _toggles[i];
			EGoodsType[] array = _filterTypes[i];
			// ApplyFilterToToggles @0x18091fa60 (machine code checked) inlines IsSameFilter: a null "allowed" (no filter) only
			// matches the toggle whose filter is null, it is never handed to SequenceEqual
			bool isOn = IsSameFilter(array, allowed);
			toggle.SetIsOnWithoutNotify(isOn);
		}
	}

	private bool IsSameFilter(EGoodsType[] a, EGoodsType[] b)
	{
		if (a == null)
		{
			return b == null;
		}
		if (b == null)
		{
			return false;
		}
		return a.SequenceEqual(b);
	}

	private void InitGamePadNav()
	{
		ClearNav();
		_toggleSelectables.Clear();
		GameObject[] toggles = _view.Toggles;
		foreach (GameObject gameObject in toggles)
		{
			_toggleSelectables.Add(gameObject.GetComponent<Toggle>());
		}
		SetNavHorizontal(_toggleSelectables);
	}
}
