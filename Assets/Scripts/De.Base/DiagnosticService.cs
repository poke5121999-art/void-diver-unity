using UniRx;
using Unity.Netcode;
using VContainer;

public class DiagnosticService : MonoBase
{
	private const float _flushIntervalSeconds = 1f;

	private const int _fpsSampleCount = 120;

	[Inject]
	private GameContext _gameContext;

	[Inject]
	private NetworkManager _networkManager;

	private readonly ReactiveProperty<float> _rxFps;

	private readonly ReactiveProperty<float> _rxAverageFps;

	private readonly ReactiveProperty<float> _rxMinFps;

	private readonly ReactiveProperty<float> _rxMaxFps;

	private readonly ReactiveProperty<ulong> _rxLocalRtt;

	private readonly ReactiveProperty<float> _rxSendBps;

	private readonly ReactiveProperty<float> _rxReceiveBps;

	private readonly ReactiveProperty<int> _rxSendPacketsPerSec;

	private readonly ReactiveProperty<int> _rxRecvPacketsPerSec;

	private readonly float[] _fpsSamples;

	private int _fpsSampleIndex;

	private int _fpsSampleFilled;

	private float _flushTimer;

	private int _prevSendCountTotal;

	private int _prevRecvCountTotal;

	public IReadOnlyReactiveProperty<float> RxFps => null;

	public IReadOnlyReactiveProperty<float> RxAverageFps => null;

	public IReadOnlyReactiveProperty<float> RxMinFps => null;

	public IReadOnlyReactiveProperty<float> RxMaxFps => null;

	public IReadOnlyReactiveProperty<ulong> RxLocalRtt => null;

	public IReadOnlyReactiveProperty<float> RxSendBps => null;

	public IReadOnlyReactiveProperty<float> RxReceiveBps => null;

	public IReadOnlyReactiveProperty<int> RxSendPacketsPerSec => null;

	public IReadOnlyReactiveProperty<int> RxRecvPacketsPerSec => null;

	protected override void Awake()
	{
	}

	private void Update()
	{
	}

	private void SampleFps()
	{
	}

	private void FlushFpsStats()
	{
	}

	private void TickFlush()
	{
	}

	private void FlushNetworkStats(float interval)
	{
	}

	private void UpdateLocalRtt()
	{
	}
}
