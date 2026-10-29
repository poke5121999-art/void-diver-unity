using Cysharp.Threading.Tasks;
using GameCommon.Data;
using GameCommon.Enum;
using NemoLib.Log;
#if STEAMWORKS_NET
using Steamworks;
#endif
using UnityEngine;
using VContainer;

public class AppManager : MonoBase
{
	[Inject]
	private EnvironmentSettings _environmentSettings;

	[Inject]
	private SeriLogger _logger;

	public AppVersion Version;

	public string AccountId { get; private set; }

	protected override void Awake()
	{
		base.Awake();
		Object.DontDestroyOnLoad(this);
	}

	private void Start()
	{
		string[] array = Application.version.Split('.');
		AppVersion appVersion = new AppVersion();
		appVersion.Major = int.Parse(array[0]);
		appVersion.Minor = int.Parse(array[1]);
		appVersion.Patch = int.Parse(array[2]);
		appVersion.Revision = int.Parse(array[3]);
		Version = appVersion;
	}

	public async UniTask<bool> AuthenticateAsync(EPlatform platform)
	{
		await UniTask.DelayFrame(1, PlayerLoopTiming.Update);
		switch (platform)
		{
		case EPlatform.Local:
			return AuthenticateLocal();
		case EPlatform.Steam:
			return AuthenticateSteam();
		default:
			return false;
		}
	}

	private bool AuthenticateLocal()
	{
		AccountId = "LocalUser";
		return true;
	}

	private bool AuthenticateSteam()
	{
#if STEAMWORKS_NET
		if (SteamManager.Initialized)
		{
			AccountId = SteamUser.GetSteamID().m_SteamID.ToString();
			return true;
		}
#endif
		return false;
	}

	public bool IsOnline()
	{
		if (_environmentSettings.Platform != EPlatform.Steam)
		{
			return true;
		}
#if STEAMWORKS_NET
		if (!SteamManager.Initialized)
		{
			return false;
		}
		return SteamUser.BLoggedOn();
#else
		return false;
#endif
	}

	public static void Quit()
	{
		Application.Quit();
	}
}
