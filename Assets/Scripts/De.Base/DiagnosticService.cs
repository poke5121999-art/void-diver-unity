using UnityEngine;
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

	private readonly ReactiveProperty<float> _rxFps = new ReactiveProperty<float>();

	private readonly ReactiveProperty<float> _rxAverageFps = new ReactiveProperty<float>();

	private readonly ReactiveProperty<float> _rxMinFps = new ReactiveProperty<float>();

	private readonly ReactiveProperty<float> _rxMaxFps = new ReactiveProperty<float>();

	private readonly ReactiveProperty<ulong> _rxLocalRtt = new ReactiveProperty<ulong>();

	private readonly ReactiveProperty<float> _rxSendBps = new ReactiveProperty<float>();

	private readonly ReactiveProperty<float> _rxReceiveBps = new ReactiveProperty<float>();

	private readonly ReactiveProperty<int> _rxSendPacketsPerSec = new ReactiveProperty<int>();

	private readonly ReactiveProperty<int> _rxRecvPacketsPerSec = new ReactiveProperty<int>();

	private readonly float[] _fpsSamples = new float[120];

	private int _fpsSampleIndex;

	private int _fpsSampleFilled;

	private float _flushTimer;

	private int _prevSendCountTotal;

	private int _prevRecvCountTotal;

	public IReadOnlyReactiveProperty<float> RxFps => _rxFps;

	public IReadOnlyReactiveProperty<float> RxAverageFps => _rxAverageFps;

	public IReadOnlyReactiveProperty<float> RxMinFps => _rxMinFps;

	public IReadOnlyReactiveProperty<float> RxMaxFps => _rxMaxFps;

	public IReadOnlyReactiveProperty<ulong> RxLocalRtt => _rxLocalRtt;

	public IReadOnlyReactiveProperty<float> RxSendBps => _rxSendBps;

	public IReadOnlyReactiveProperty<float> RxReceiveBps => _rxReceiveBps;

	public IReadOnlyReactiveProperty<int> RxSendPacketsPerSec => _rxSendPacketsPerSec;

	public IReadOnlyReactiveProperty<int> RxRecvPacketsPerSec => _rxRecvPacketsPerSec;

	protected override void Awake()
	{
		base.Awake();
		UnityEngine.Object.DontDestroyOnLoad(this); // tail call 0x183f187a0 is Object.DontDestroyOnLoad (machine code checked @0x1801c6400)
	}

	private void Update()
	{
		SampleFps();
		TickFlush();
	}

	private void SampleFps()
	{
		float unscaledDeltaTime = Time.unscaledDeltaTime;
		if (!(unscaledDeltaTime <= 0f))
		{
			_rxFps.Value = 1f / unscaledDeltaTime;
			_fpsSamples[_fpsSampleIndex] = 1f / unscaledDeltaTime;
			_fpsSampleIndex = (_fpsSampleIndex + 1) % 120;
			if (_fpsSampleFilled < 120)
			{
				_fpsSampleFilled++;
			}
		}
	}

	private void FlushFpsStats()
	{
		if (_fpsSampleFilled >= 1)
		{
			float min = float.MaxValue;
			float sum = 0f;
			float max = 0f;
			for (int i = 0; i < _fpsSampleFilled; i++)
			{
				float v = _fpsSamples[i];
				sum += v;
				if (v < min)
				{
					min = v;
				}
				if (max < v)
				{
					max = v;
				}
			}
			_rxAverageFps.Value = sum / (float)_fpsSampleFilled;
			_rxMinFps.Value = min;
			_rxMaxFps.Value = max;
		}
	}

	private void TickFlush()
	{
		_flushTimer += Time.unscaledDeltaTime;
		if (!(_flushTimer < 1f))
		{
			FlushFpsStats();
			UpdateLocalRtt();
			_flushTimer = 0f;
		}
	}

	private void FlushNetworkStats(float interval)
	{
		// the body is a tail jump to UpdateLocalRtt (`jmp 0x1801c6780`, machine code checked @0x1801c6560); nothing in the binary calls this method
		UpdateLocalRtt();
	}

	private void UpdateLocalRtt()
	{
		ulong rtt = 0uL;
		if (!(_networkManager == null) && _networkManager.IsListening)
		{
			ulong localClientId = _networkManager.LocalClientId;
			if (_gameContext.PlayerRtt != null && _gameContext.PlayerRtt.TryGetValue(localClientId, out var value))
			{
				rtt = value;
			}
		}
		_rxLocalRtt.Value = rtt;
	}
}
