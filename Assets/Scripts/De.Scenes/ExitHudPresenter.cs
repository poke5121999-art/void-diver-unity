using GameCommon.Data;
using GameCommon.Enum;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using UniRx;
using UnityEngine;
using VContainer;

public class ExitHudPresenter : Presenter
{
	[SerializeField]
	private ExitObject _exitObject;

	[View]
	private ExitHudView _view;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private GameContext _gameContext;

	private bool _isInitialized;

	private bool _isExitActivated;

	private CompositeDisposable _buffDisposable;

	private void Start()
	{
	}

	private void OnDestroy()
	{
	}

	private void OnStageStateChanged(EStageState state)
	{
	}

	private Goods GetUnlockGoods()
	{
		return null;
	}

	private void SetupUnlockGoodsLayout()
	{
	}

	private void UpdateCountText()
	{
	}

	private void OnExitStateChanged(EExitState exitState)
	{
	}

	private void OnRemainTimeChanged(float remainTime)
	{
	}

	private void OnViewTargetChanged(ulong targetClientId)
	{
	}

	private void UpdateEscapeBuffGauge(CharacterModel targetModel)
	{
	}
}
