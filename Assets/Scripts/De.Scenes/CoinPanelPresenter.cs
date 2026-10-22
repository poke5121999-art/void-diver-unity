using NemoLib.Mvp;
using UniRx;
using UnityEngine;
using VContainer;

public class CoinPanelPresenter : Presenter
{
	[View]
	private CoinPanelView _view;

	[Inject]
	private PlayerManager _playerManager;

	private void Start()
	{
		UpdateGoldUi(applyImmediately: true);
		_playerManager.RxProfile.Skip(1).Subscribe(delegate
		{
			UpdateGoldUi();
		}).AddTo(this);
	}

	private void OnEnable()
	{
		UpdateGoldUi(applyImmediately: true);
	}

	private void UpdateGoldUi(bool applyImmediately = false)
	{
		if (_playerManager == null)
		{
			return;
		}
		_view.Coin_Text.GetOrAddComponent<ValueTween>().SetValue(_playerManager.Coin, applyImmediately);
	}
}
