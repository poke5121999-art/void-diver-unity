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
	}

	private void Start()
	{
	}

	private void OnFpsChanged(float fps)
	{
	}

	private void OnAverageFpsChanged(float fps)
	{
	}

	private void OnMinFpsChanged(float fps)
	{
	}

	private void OnMaxFpsChanged(float fps)
	{
	}

	private void OnLocalRttChanged(ulong rttMs)
	{
	}

	private void OnSendBpsChanged(float bps)
	{
	}

	private void OnReceiveBpsChanged(float bps)
	{
	}

	private void OnSendPpsChanged(int pps)
	{
	}

	private void OnRecvPpsChanged(int pps)
	{
	}

	private static string FormatBps(float bps)
	{
		return null;
	}
}
