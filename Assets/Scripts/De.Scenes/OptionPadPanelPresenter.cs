using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using NemoLib.Mvp;
using UniRx;
using UnityEngine.UI;
using VContainer;

public class OptionPadPanelPresenter : BasePanelPresenter
{
	private enum EFocusPanel
	{
		Base = 0,
		Scroller = 1
	}

	[InjectModel]
	private OptionPadPanelModel _model;

	[View]
	private OptionPadPanelView _view;

	[Inject]
	private InputService _inputService;

	private KeySettingSlotRowScrollerModel _scrollerModel;

	private KeySettingSlotModel _selectedSlot;

	private CancellationTokenSource _rebindCts;

	private CompositeDisposable _disposable;

	protected override BasePanelModel PanelModel => _model;

	protected override void OnDestroy()
	{
		base.OnDestroy();
		_disposable?.Dispose();
		_rebindCts?.Cancel();
		_rebindCts?.Dispose();
	}

	private void OnInject(OptionPadPanelModel model)
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		InitScroller();
		_scrollerModel.OnSlotClick.Subscribe(OnSlotClick).AddTo(_disposable);
		_view.Scroller.Inject(_scrollerModel);
		InitGamePadNav();
		_view.ResetButton.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnResetClick();
		}).AddTo(_disposable);
		model.RxIsInputEnabled.Where((bool _) => _).Subscribe(delegate
		{
			OnInputEnabled();
		}).AddTo(_disposable);
		model.RxIsInputEnabled.Where((bool _) => !_).Subscribe(delegate
		{
			OnInputDisabled();
		}).AddTo(_disposable);
	}

	private void InitScroller()
	{
		_scrollerModel = new KeySettingSlotRowScrollerModel
		{
			ControlDevice = EControlDevice.GamePad
		};
	}

	private void InitGamePadNav()
	{
		ClearNav();
		List<Selectable> items = new List<Selectable> { _view.ResetButton };
		SetNavHorizontal(items);
		_model.OutOfTop.Subscribe(delegate
		{
			FocusPanel(EFocusPanel.Scroller, ENavDir.Up);
		}).AddTo(_disposable);
		_scrollerModel.OutOfBottom.Subscribe(delegate
		{
			FocusPanel(EFocusPanel.Base, ENavDir.Down);
		}).AddTo(_disposable);
	}

	private void FocusPanel(EFocusPanel panel, ENavDir entryDir)
	{
		if (panel == EFocusPanel.Base)
		{
			_model.EntryDirection = entryDir;
			ResumeNav();
		}
		else
		{
			_scrollerModel.EntryDirection = entryDir;
			SuspendNav();
		}
		_scrollerModel.RxIsInputEnabled.Value = panel == EFocusPanel.Scroller;
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		FocusPanel(EFocusPanel.Scroller, ENavDir.None);
	}

	protected override void OnInputDisabled()
	{
		_scrollerModel.RxIsInputEnabled.Value = false;
		base.OnInputDisabled();
	}

	private void OnSlotClick(KeySettingSlotModel slotModel)
	{
		if (_inputService.CanRebind(slotModel.InputActionId))
		{
			_selectedSlot = slotModel;
			slotModel.RxIsSelected.Value = true;
			RebindAsync(slotModel.InputActionId).Forget();
		}
	}

	private async UniTask RebindAsync(int inputActionId)
	{
		_rebindCts?.Cancel();
		_rebindCts?.Dispose();
		_rebindCts = new CancellationTokenSource();
		await _inputService.RebindAsync(inputActionId, _rebindCts.Token);
		_selectedSlot.RxIsSelected.Value = false;
		_selectedSlot = null;
	}

	private void OnResetClick()
	{
		_inputService.ResetBindings(EControlDevice.GamePad);
	}

	private void OnBack()
	{
		_model.OnBack.OnNext(Unit.Default);
	}
}
