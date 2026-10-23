using System;
using NemoLib.Mvp;
using UniRx;
using VContainer;

public class FpsPanelPresenter : Presenter
{
	[View]
	private FpsPanelView _view;

	[Inject]
	private DiagnosticService _diagnosticService;

	private CompositeDisposable _disposable;

	private void OnDestroy()
	{
		_disposable?.Dispose();
	}

	private void Start()
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		_diagnosticService.RxFps.Sample(TimeSpan.FromMilliseconds(200.0)).Subscribe(OnFpsChanged).AddTo(_disposable);
		_diagnosticService.RxAverageFps.Subscribe(OnAverageFpsChanged).AddTo(_disposable);
		_diagnosticService.RxMinFps.Subscribe(OnMinFpsChanged).AddTo(_disposable);
		_diagnosticService.RxMaxFps.Subscribe(OnMaxFpsChanged).AddTo(_disposable);
		_diagnosticService.RxLocalRtt.Subscribe(OnLocalRttChanged).AddTo(_disposable);
		_diagnosticService.RxSendBps.Subscribe(OnSendBpsChanged).AddTo(_disposable);
		_diagnosticService.RxReceiveBps.Subscribe(OnReceiveBpsChanged).AddTo(_disposable);
		_diagnosticService.RxSendPacketsPerSec.Subscribe(OnSendPpsChanged).AddTo(_disposable);
		_diagnosticService.RxRecvPacketsPerSec.Subscribe(OnRecvPpsChanged).AddTo(_disposable);
	}

	private void OnFpsChanged(float fps)
	{
		_view.Fps_Current_Value.text = string.Format("{0:F0}", fps);
	}

	private void OnAverageFpsChanged(float fps)
	{
		_view.Fps_Average_Value.text = string.Format("{0:F0}", fps);
	}

	private void OnMinFpsChanged(float fps)
	{
		_view.Fps_Min_Value.text = string.Format("{0:F0}", fps);
	}

	private void OnMaxFpsChanged(float fps)
	{
		_view.Fps_Max_Value.text = string.Format("{0:F0}", fps);
	}

	private void OnLocalRttChanged(ulong rttMs)
	{
		_view.Network_Rtt_Value.text = string.Format("{0} ms", rttMs);
	}

	private void OnSendBpsChanged(float bps)
	{
		_view.Network_SendBps_Value.text = FormatBps(bps);
	}

	private void OnReceiveBpsChanged(float bps)
	{
		_view.Network_ReceiveBps_Value.text = FormatBps(bps);
	}

	private void OnSendPpsChanged(int pps)
	{
		_view.Network_SendPacketsPerSec_Value.text = pps.ToString();
	}

	private void OnRecvPpsChanged(int pps)
	{
		_view.Network_RecvPacketsPerSec_Value.text = pps.ToString();
	}

	private static string FormatBps(float bps)
	{
		if (bps >= 1048576f)
		{
			return string.Format("{0:F2} MB/s", bps / 1048576f);
		}
		if (bps >= 1024f)
		{
			return string.Format("{0:F1} KB/s", bps / 1024f);
		}
		return string.Format("{0:F0} B/s", bps);
	}
}
