using System;
using System.Runtime.CompilerServices;
using NemoLib.Log;
using Unity.Netcode;
using VContainer;

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

	private bool IsRunning => false;

	public bool IsPaused => false;

	public int RawTick => 0;

	public int GameTick => 0;

	public float GameTime => 0f;

	private int PausedTickOffset => 0;

	public event Action OnGameTick
	{
		[CompilerGenerated]
		add
		{
		}
		[CompilerGenerated]
		remove
		{
		}
	}

	private void OnClientStarted()
	{
	}

	private void OnClientStopped(bool _)
	{
	}

	private void OnTick()
	{
	}

	public void Pause()
	{
	}

	public void Resume()
	{
	}

	public void Dispose()
	{
	}
}
