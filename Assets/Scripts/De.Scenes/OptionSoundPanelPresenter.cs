using NemoLib.Mvp;
using UniRx;
using UnityEngine.UI;
using VContainer;

public class OptionSoundPanelPresenter : BasePanelPresenter
{
	[InjectModel]
	private OptionSoundPanelModel _model;

	[View]
	private OptionSoundPanelView _view;

	[Inject]
	private SoundSettingService _soundSettings;

	[Inject]
	private ToastManager _toastManager;

	private CompositeDisposable _disposable;

	protected override BasePanelModel PanelModel => null;

	private int VolumeLevelMax => 0;

	protected override void OnDestroy()
	{
	}

	private void OnInject(OptionSoundPanelModel model)
	{
	}

	private void OnDisable()
	{
	}

	protected override void OnInputEnabled()
	{
	}

	private void InitGamePadNav()
	{
	}

	private void InitMasterVolume()
	{
	}

	private void OnMasterVolumeLevelUpdated(int value)
	{
	}

	private void SetMasterVolumeOn(bool value)
	{
	}

	private void SetMasterVolumeLevel(int value)
	{
	}

	private void InitBgmVolume()
	{
	}

	private void OnBgmVolumeLevelUpdated(int value)
	{
	}

	private void SetBgmVolumeOn(bool value)
	{
	}

	private void SetBgmVolumeLevel(int value)
	{
	}

	private void InitSfxVolume()
	{
	}

	private void OnSfxVolumeLevelUpdated(int value)
	{
	}

	private void SetSfxVolumeOn(bool value)
	{
	}

	private void SetSfxVolumeLevel(int value)
	{
	}

	private void OnResetClick()
	{
	}

	private static void SyncToggle(Toggle toggle, bool isOn)
	{
	}

	private void OnMuteTogglePressed()
	{
	}
}
