using System.Collections.Generic;
using NemoLib.AssetManagement;
using NemoLib.Log;
using NemoLib.Mvp;
using UniRx;
using UnityEngine.EventSystems;
using VContainer;

public class InGameStatusPanelPresenter : BasePanelPresenter
{
	[View]
	private InGameStatusPanelView _view;

	[Model]
	private InGameStatusPanelModel _model;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private SeriLogger _logger;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private PlayerInputController _inputController;

	[Inject]
	private GamePingManager _pingManager;

	[Inject]
	private ToastManager _toastManager;

	private CompositeDisposable _disposable;

	private readonly Dictionary<int, InGameBuffSlotPresenter> _buffSlots;

	protected override BasePanelModel PanelModel => null;

	private CharacterModel CharacterModel => null;

	protected override void OnDestroy()
	{
	}

	private void Start()
	{
	}

	private void OnViewTargetClientIdChanged(ulong targetClientId)
	{
	}

	private void OnContentsPointerDown(PointerEventData eventData)
	{
	}

	private void RefreshUis(CharacterModel characterModel)
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

	private void UpdateHpUi()
	{
	}

	private void OnUpdateMaxHp()
	{
	}

	private void UpdateLightFuelUi()
	{
	}

	private void OnBuffSlotEnter(InGameBuffSlotModel model)
	{
	}

	private void OnBuffSlotExit(InGameBuffSlotModel model)
	{
	}
}
