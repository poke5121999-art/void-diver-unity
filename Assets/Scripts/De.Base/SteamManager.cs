using System;
using System.Text;
using AOT;
using Steamworks;
using UnityEngine;

[DisallowMultipleComponent]
public class SteamManager : MonoBehaviour
{
	private static bool _everInitialized;

	private static SteamManager _instance;

	private bool _initialized;

	protected SteamAPIWarningMessageHook_t m_SteamAPIWarningMessageHook;

	public static bool Initialized => _instance != null && _instance._initialized;

	public static void Initialize()
	{
		if (!Initialized)
		{
			_instance = new GameObject("SteamManager").AddComponent<SteamManager>();
		}
	}

	[MonoPInvokeCallback(typeof(SteamAPIWarningMessageHook_t))]
	protected static void SteamAPIDebugTextHook(int nSeverity, StringBuilder pchDebugText)
	{
		Debug.LogWarning(pchDebugText);
	}

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	private static void InitOnPlayMode()
	{
		_everInitialized = false;
		_instance = null;
	}

	protected virtual void Awake()
	{
		if (_instance != null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		_instance = this;
		if (_everInitialized)
		{
			throw new Exception("Tried to Initialize the SteamAPI twice in one session!");
		}
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		if (!Packsize.Test())
		{
			Debug.LogError("[Steamworks.NET] Packsize Test returned false, the wrong version of Steamworks.NET is being run in this platform.", this);
		}
		if (!DllCheck.Test())
		{
			Debug.LogError("[Steamworks.NET] DllCheck Test returned false, One or more of the Steamworks binaries seems to be the wrong version.", this);
		}
		try
		{
			if (SteamAPI.RestartAppIfNecessary((AppId_t)3438920u))
			{
				Application.Quit();
				return;
			}
		}
		catch (DllNotFoundException ex)
		{
			Debug.LogError("[Steamworks.NET] Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location. Refer to the README for more details.\n" + ex, this);
			Application.Quit();
			return;
		}
		_initialized = SteamAPI.Init();
		if (!_initialized)
		{
			Debug.LogError("[Steamworks.NET] SteamAPI_Init() failed. Refer to Valve's documentation or the comment above this line for more information.", this);
		}
		else
		{
			_everInitialized = true;
		}
	}

	protected virtual void OnEnable()
	{
		if (_instance == null)
		{
			_instance = this;
		}
		if (_initialized && m_SteamAPIWarningMessageHook == null)
		{
			m_SteamAPIWarningMessageHook = SteamAPIDebugTextHook;
			SteamClient.SetWarningMessageHook(m_SteamAPIWarningMessageHook);
		}
	}

	public static void ShutdownSteamApi()
	{
		if (!(_instance == null) && _instance._initialized)
		{
			_instance._initialized = false;
			if (CallbackDispatcher.IsInitialized)
			{
				SteamAPI.RunCallbacks();
			}
			SteamAPI.Shutdown();
		}
	}

	protected virtual void OnDestroy()
	{
		if (!(_instance != this))
		{
			if (!(_instance == null) && _instance._initialized)
			{
				_instance._initialized = false;
				if (CallbackDispatcher.IsInitialized)
				{
					SteamAPI.RunCallbacks();
				}
				SteamAPI.Shutdown();
			}
			_instance = null;
		}
	}

	protected virtual void Update()
	{
		RunCallbacks();
	}

	private void RunCallbacks()
	{
		if (CallbackDispatcher.IsInitialized)
		{
			SteamAPI.RunCallbacks();
		}
	}
}
