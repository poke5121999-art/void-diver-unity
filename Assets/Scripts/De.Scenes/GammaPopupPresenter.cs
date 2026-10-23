using Cysharp.Threading.Tasks;
using NemoLib.Mvp;
using UniRx;
using UnityEngine.UI;

public class GammaPopupPresenter : GamePopupPresenter
{
	[InjectModel]
	private GammaPopupModel _model;

	[View]
	private GammaPopupView _view;

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	private InputActions.UiActions UiActions => GameInputManager.UiActions;

	public override bool IsInputActionPopup => true;

	public override string OpenSfx => "PopUpOpen";

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		_view.Gamma_Slider.wholeNumbers = true;
		_view.Gamma_Slider.minValue = -50f;
		_view.Gamma_Slider.maxValue = 50f;
		_view.Gamma_Slider.value = _model.GammaPercent;
		_view.Gamma_Slider.OnValueChangedAsObservable().Subscribe(OnGammaValueChanged).AddTo(this);
		_view.CloseButton.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			Close();
		}).AddTo(this);
		SetNavVertical(new Selectable[2] { _view.Gamma_Slider, _view.CloseButton });
		SetDefaultNav(_view.Gamma_Slider);
	}

	private void OnGammaValueChanged(float value)
	{
		_view.Gamma_Slider_Handle_Text.text = ((int)value).ToString();
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		UiActions.Escape.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			Close();
		}).AddTo(InputDisposable);
	}

	private void InitGamePadNav()
	{
		SetNavVertical(new Selectable[2] { _view.Gamma_Slider, _view.CloseButton });
		SetDefaultNav(_view.Gamma_Slider);
	}

	public override void Close(object result = null)
	{
		base.Close((int)_view.Gamma_Slider.value);
	}
}
