using System;
using System.Collections.Generic;
using System.Linq;
using GameCommon.Data;
using NemoLib.Mvp;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class UnlockMaterialPanelPresenter : BasePanelPresenter
{
	[InjectModel]
	private UnlockMaterialPanelModel _model;

	[View]
	private UnlockMaterialPanelView _view;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private GameManager _gameManager;

	private CompositeDisposable _disposable;

	protected override BasePanelModel PanelModel => _model;

	protected void OnInject(UnlockMaterialPanelModel model)
	{
		_disposable?.Clear();
		_disposable = new CompositeDisposable();
		_playerManager.RxProfile.Subscribe(delegate
		{
			OnUpdateUis();
		}).AddTo(_disposable);
		_playerManager.OnOwnedGoodsChanged.Subscribe(delegate
		{
			OnUpdateUis();
		}).AddTo(_disposable);
		model.RxIsInputEnabled.Where((bool x) => x).Subscribe(delegate
		{
			OnInputEnabled();
		}).AddTo(_disposable);
		model.RxIsInputEnabled.Where((bool x) => !x).Subscribe(delegate
		{
			OnInputDisabled();
		}).AddTo(_disposable);
	}

	private void OnUpdateUis()
	{
		if (_model.UnlockConditions != null)
		{
			_view.Condition.SetActive(value: true);
			int num = _model.UnlockConditions.Length;
			for (int i = 0; i < _view.Condition_Texts.Length; i++)
			{
				GameObject gameObject = _view.Condition_Texts[i];
				gameObject.SetActive(i < num);
				if (i < num)
				{
					gameObject.GetComponent<TextMeshProUGUI>().text = _model.UnlockConditions[i].ToDescription();
				}
			}
		}
		else
		{
			_view.Condition.SetActive(value: false);
		}
		Goods[] array = ((_model.Materials != null) ? _model.Materials.Select((Goods x) => x.Clone()).ToArray() : Array.Empty<Goods>());
		_view.Material_Caption.gameObject.SetActive(array.Any());
		_view.Material_Slots.First().transform.parent.gameObject.SetActive(array.Any());
		if (array.Any())
		{
			for (int j = 0; j < _view.Material_Slots.Length; j++)
			{
				GameObject gameObject2 = _view.Material_Slots[j];
				gameObject2.SetActive(j < array.Length);
				if (j < array.Length)
				{
					gameObject2.GetComponent<MaterialSlotPresenter>().Inject(new MaterialSlotModel
					{
						Goods = array[j]
					});
				}
			}
		}
		_view.NeedCurrency.SetActive(_model.NeedGold > 0);
		if (_model.NeedGold > 0)
		{
			string text = _model.NeedGold.ToString("N0");
			if (!_playerManager.HasEnoughGold(_model.NeedGold))
			{
				text = text.WithColor("#a45646");
			}
			_view.GoldText.text = text;
		}
		_view.NeedCoin.SetActive(_model.NeedCoin > 0);
		if (_model.NeedCoin > 0)
		{
			string text2 = _model.NeedCoin.ToString("N0");
			if (!_playerManager.HasEnoughCoin(_model.NeedCoin))
			{
				text2 = text2.WithColor("#a45646");
			}
			_view.CoinText.text = text2;
		}
		_view.NeedRealCoin.SetActive(_model.NeedRealCoin > 0);
		if (_model.NeedRealCoin > 0)
		{
			long totalGoodsCount = _playerManager.GetTotalGoodsCount(GoodsCreator.MakeItemGoods(10001));
			string text3 = _model.NeedRealCoin.ToString("N0");
			if (totalGoodsCount < _model.NeedRealCoin)
			{
				text3 = text3.WithColor("#a45646");
			}
			_view.RealCoinText.text = text3;
		}
		InitGamePadNav();
	}

	private void InitGamePadNav()
	{
		ClearNav();
		if (_model.Materials != null && _model.Materials.Length != 0)
		{
			SetNavHorizontal(_view.Material_Slots.Take(_model.Materials.Length).Select(delegate(GameObject go)
			{
				if (go == null)
				{
					return null;
				}
				MaterialSlotView component = go.GetComponent<MaterialSlotView>();
				if (component == null || component.GoodsSlot == null)
				{
					return null;
				}
				GoodsSlotView component2 = component.GoodsSlot.GetComponent<GoodsSlotView>();
				return (component2 == null) ? null : component2.Slot;
			}).Cast<Selectable>().ToList());
		}
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		_disposable?.Dispose();
	}
}
