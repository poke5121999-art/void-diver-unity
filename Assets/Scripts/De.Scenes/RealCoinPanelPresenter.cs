using GameCommon.Data;
using NemoLib.Mvp;
using UniRx;
using UnityEngine;
using VContainer;

public class RealCoinPanelPresenter : Presenter
{
	[View]
	private RealCoinPanelView _view;

	[Inject]
	private PlayerManager _playerManager;

	private void Start()
	{
		UpdateRealCoinUi(applyImmediately: true);
		_playerManager.OnOwnedGoodsChanged.Subscribe(delegate
		{
			UpdateRealCoinUi();
		}).AddTo(this);
		_playerManager.OnStashChanged.Subscribe(delegate
		{
			UpdateRealCoinUi();
		}).AddTo(this);
	}

	private void OnEnable()
	{
		UpdateRealCoinUi(applyImmediately: true);
	}

	private void UpdateRealCoinUi(bool applyImmediately = false)
	{
		if (_playerManager == null)
		{
			return;
		}
		Goods goods = GoodsCreator.MakeItemGoods(10001);
		long totalGoodsCount = _playerManager.GetTotalGoodsCount(goods);
		_view.RealCoin_Value.GetOrAddComponent<ValueTween>().SetValue(totalGoodsCount, applyImmediately);
	}
}
