using System;
using System.Runtime.CompilerServices;
using NemoLib.Log;
using Unity.Netcode;
using VContainer;
using VContainer.Unity;

public class NetworkTimer : NativeBase, IDisposable
{
	[Inject]
	private NetworkManager _networkManager;

	[Inject]
	private SeriLogger _logger;

	private NetworkTickSystem _tickSystem;

	private float _tickInterval;

	private bool _isPaused;

	private int _pausedTickAccum;

	private int _pauseStartTick;

	[CompilerGenerated]
	private Action m_OnGameTick;

	public NetworkTimer()
	{
		this.InjectFromRootContainer();
		_networkManager.OnClientStarted += OnClientStarted;
		_networkManager.OnClientStopped += OnClientStopped;
	}

	private bool IsRunning => _tickSystem != null;

	public bool IsPaused => _isPaused;

	public int RawTick
	{
		get
		{
			if (_tickSystem == null)
			{
				return 0;
			}
			return _networkManager.ServerTime.Tick;
		}
	}

	public int GameTick
	{
		get
		{
			if (_tickSystem == null)
			{
				return 0;
			}
			return RawTick - PausedTickOffset;
		}
	}

	public float GameTime
	{
		get
		{
			if (_tickSystem == null)
			{
				return 0f;
			}
			if (!_isPaused)
			{
				return _networkManager.ServerTime.TimeAsFloat - (float)_pausedTickAccum * _tickInterval;
			}
			return (float)GameTick * _tickInterval;
		}
	}

	private int PausedTickOffset
	{
		get
		{
			if (!_isPaused)
			{
				return _pausedTickAccum;
			}
			if (_tickSystem == null)
			{
				return _pausedTickAccum - _pauseStartTick;
			}
			return _pausedTickAccum + (_networkManager.ServerTime.Tick - _pauseStartTick);
		}
	}

	public event Action OnGameTick
	{
		[CompilerGenerated]
		add
		{
			m_OnGameTick = (Action)Delegate.Combine(m_OnGameTick, value);
		}
		[CompilerGenerated]
		remove
		{
			m_OnGameTick = (Action)Delegate.Remove(m_OnGameTick, value);
		}
	}

	private void OnClientStarted()
	{
		if (_tickSystem != null)
		{
			return;
		}
		NetworkTickSystem networkTickSystem = _networkManager.NetworkTickSystem;
		if (networkTickSystem == null)
		{
			_logger.Error("NetworkTimer: NetworkTickSystem is null on start.");
			return;
		}
		_tickSystem = networkTickSystem;
		_tickInterval = ((_tickSystem.TickRate != 0) ? (1f / (float)_tickSystem.TickRate) : 0f);
		_pausedTickAccum = 0;
		_pauseStartTick = 0;
		_isPaused = false;
		_tickSystem.Tick += OnTick;
		_logger.Debug(string.Format("NetworkTimer started. tickRate={0}", _tickSystem.TickRate), UnityEngine.Color.white);
	}

	private void OnClientStopped(bool _)
	{
		if (_tickSystem != null)
		{
			_tickSystem.Tick -= OnTick;
			_tickSystem = null;
			_tickInterval = 0f;
			_isPaused = false;
			_pausedTickAccum = 0;
			_pauseStartTick = 0;
			_logger.Debug("NetworkTimer stopped.", UnityEngine.Color.white);
		}
	}

	private void OnTick()
	{
		if (!_isPaused)
		{
			m_OnGameTick?.Invoke();
		}
	}

	public void Pause()
	{
		if (_tickSystem != null && !_isPaused)
		{
			_pauseStartTick = RawTick;
			_isPaused = true;
			_logger.Debug(string.Format("NetworkTimer paused at tick {0}.", _pauseStartTick), UnityEngine.Color.white);
		}
	}

	public void Resume()
	{
		if (_tickSystem != null && _isPaused)
		{
			int offset = _pausedTickAccum + (RawTick - _pauseStartTick);
			_isPaused = false;
			_pausedTickAccum = offset;
			_logger.Debug(string.Format("NetworkTimer resumed. pausedAccum={0}", _pausedTickAccum), UnityEngine.Color.white);
		}
	}

	public void Dispose()
	{
		if (_networkManager != null)
		{
			_networkManager.OnClientStarted -= OnClientStarted;
			_networkManager.OnClientStopped -= OnClientStopped;
		}
		if (_tickSystem != null)
		{
			_tickSystem.Tick -= OnTick;
		}
		_tickSystem = null;
	}
}
