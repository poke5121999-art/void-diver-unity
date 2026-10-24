using System;
using GameCommon.Enum;
using GameCommon.Data;
using NemoLib.AssetManagement;
using NemoLib.Log;
using NemoLib.Popup;
using NemoLib.Scene;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

public class RootLifetimeScope : LifetimeScope
{
	[SerializeField]
	private AssetReferenceGameObject _unityNetworkManager;

	[SerializeField]
	private AssetReferenceGameObject _steamNetworkManager;

	[SerializeField]
	private AssetReferenceGameObject _popupManager;

	[SerializeField]
	private AssetReferenceGameObject _toastManager;

	[SerializeField]
	private AssetReferenceGameObject _soundManager;

	[SerializeField]
	private AssetReferenceGameObject _cursorManager;

	[SerializeField]
	private AssetReferenceGameObject _cameraManager;

	[SerializeField]
	private AssetReferenceGameObject _gameFloatingTextManager;

	[SerializeField]
	private AssetReferenceGameObject _graphicOptionController;

	[SerializeField]
	private AssetReferenceGameObject _blockManager;

	[SerializeField]
	private AssetReferenceGameObject _inputIconsManager;

	[SerializeField]
	private AssetReferenceT<EnvironmentSettings> _environmentSettings;

	[SerializeField]
	private AssetReferenceT<MiniMapSettings> _miniMapSettings;

	[SerializeField]
	private AssetReferenceT<LoungeMapSettings> _loungeMapSettings;

	[SerializeField]
	private AssetReferenceT<FontLocalizationSettings> _fontLocalizationSettings;

	[SerializeField]
	private AssetReferenceT<InputActionAsset> _inputActions;

	protected override void Configure(IContainerBuilder builder)
	{
		EnvironmentSettings environmentSettings = LoadAsset(_environmentSettings);
		switch (environmentSettings.Platform)
		{
		case EPlatform.Local:
			builder.RegisterComponent(UnityEngine.Object.Instantiate(LoadPrefab<NetworkManager>(_unityNetworkManager)));
			builder.RegisterComponentOnNewGameObject<LocalRoomMatchmaker>(Lifetime.Singleton).As<IRoomMatchmaker>();
			builder.RegisterComponentOnNewGameObject<LocalAchievementService>(Lifetime.Singleton).As<IAchievementService>();
			builder.Register<LocalStorage>(Lifetime.Singleton).As<IStorage>();
			break;
		case EPlatform.Steam:
			builder.RegisterComponent(UnityEngine.Object.Instantiate(LoadPrefab<NetworkManager>(_steamNetworkManager)));
			builder.RegisterComponentOnNewGameObject<SteamRoomMatchmaker>(Lifetime.Singleton).As<IRoomMatchmaker>();
			builder.RegisterComponentOnNewGameObject<SteamAchievementService>(Lifetime.Singleton).As<IAchievementService>();
			builder.Register<SteamStorage>(Lifetime.Singleton).As<IStorage>();
			break;
		default:
			throw new ArgumentOutOfRangeException(string.Format("Unsupported platform type: {0}", environmentSettings.Platform));
		}
		builder.RegisterComponentInNewPrefab(LoadPrefab<PopupManager>(_popupManager), Lifetime.Singleton);
		builder.RegisterComponentInNewPrefab(LoadPrefab<ToastManager>(_toastManager), Lifetime.Singleton);
		builder.RegisterComponentInNewPrefab(LoadPrefab<BlockManager>(_blockManager), Lifetime.Singleton);
		builder.RegisterComponentInNewPrefab(LoadPrefab<SoundManager>(_soundManager), Lifetime.Singleton);
		builder.RegisterComponentInNewPrefab(LoadPrefab<CursorManager>(_cursorManager), Lifetime.Singleton);
		builder.RegisterComponentInNewPrefab(LoadPrefab<InputIconsManager>(_inputIconsManager), Lifetime.Singleton);
		builder.RegisterInstance(environmentSettings).AsSelf();
		builder.RegisterInstance(LoadAsset(_fontLocalizationSettings)).AsSelf();
		builder.RegisterInstance(LoadAsset(_inputActions)).AsSelf();
		builder.Register<AssetManager>(Lifetime.Singleton);
		builder.Register<GameEventSystem>(Lifetime.Singleton);
		builder.Register<GraphicSettingService>(Lifetime.Singleton);
		builder.Register<SoundSettingService>(Lifetime.Singleton);
		builder.Register<GamePlaySettingService>(Lifetime.Singleton);
		builder.Register<FontLocalizationService>(Lifetime.Singleton);
		builder.Register<FontPrewarmService>(Lifetime.Singleton);
		builder.Register<SceneManager>(Lifetime.Singleton);
		builder.Register<AccountRecordManager>(Lifetime.Singleton);
		builder.Register<AchievementManager>(Lifetime.Singleton);
		builder.Register<NewTagManager>(Lifetime.Singleton);
		builder.Register<StorageService>(Lifetime.Singleton);
		builder.Register<LocalBackupStorage>(Lifetime.Singleton).As<IBackupStorage>();
		builder.RegisterComponentOnNewGameObject<GameUiEventSystem>(Lifetime.Singleton);
		builder.RegisterComponentOnNewGameObject<LuaRunner>(Lifetime.Singleton).As<ILuaRunner>();
		builder.RegisterComponentOnNewGameObject<GameInputManager>(Lifetime.Singleton, "GameInputManager").AsSelf().As<InputManager>();
		builder.Register<InputService>(Lifetime.Singleton);
		builder.RegisterComponentOnNewGameObject<AnalyticsController>(Lifetime.Singleton);
		builder.RegisterComponentOnNewGameObject<AppManager>(Lifetime.Singleton);
		builder.RegisterComponentOnNewGameObject<DiagnosticService>(Lifetime.Singleton);
		SeriLogger seriLogger = new GameObject("SeriLogger").AddComponent<SeriLogger>();
		seriLogger.Init(environmentSettings.UnityLogLevel, environmentSettings.FileLogLevel, environmentSettings.UseFileLogger);
		builder.RegisterInstance(seriLogger);
		builder.Register<GameHost>(Lifetime.Singleton).AsSelf().As<INetworkHost>();
		builder.Register<GameChannel>(Lifetime.Singleton).AsSelf().As<INetworkChannel>();
		builder.Register<NetworkTimer>(Lifetime.Singleton);
		builder.Register<RoomContext>(Lifetime.Singleton);
		builder.Register<GameContext>(Lifetime.Singleton);
		builder.Register<DialogReplayService>(Lifetime.Singleton);
		builder.RegisterComponentOnNewGameObject<GameManager>(Lifetime.Singleton);
		builder.RegisterComponentOnNewGameObject<PlayerInputController>(Lifetime.Singleton);
		builder.RegisterComponentOnNewGameObject<GameCameraController>(Lifetime.Singleton);
		builder.RegisterComponentOnNewGameObject<GameMapManager>(Lifetime.Singleton);
		builder.RegisterComponentOnNewGameObject<GameLuaController>(Lifetime.Singleton);
		builder.RegisterComponentOnNewGameObject<GameObjectManager>(Lifetime.Singleton);
		builder.RegisterComponentOnNewGameObject<GameSoundManager>(Lifetime.Singleton);
		builder.RegisterComponentOnNewGameObject<GameVfxManager>(Lifetime.Singleton);
		builder.RegisterComponentOnNewGameObject<GameCampaignManager>(Lifetime.Singleton);
		builder.RegisterComponentOnNewGameObject<GameShopManager>(Lifetime.Singleton);
		builder.RegisterComponentOnNewGameObject<GameMissionManager>(Lifetime.Singleton);
		builder.RegisterComponentOnNewGameObject<GameLoungeQuestManager>(Lifetime.Singleton);
		builder.RegisterComponentOnNewGameObject<GameLoungeManager>(Lifetime.Singleton);
		builder.RegisterComponentOnNewGameObject<GameWaveManager>(Lifetime.Singleton);
		builder.RegisterComponentOnNewGameObject<GameCombatSystem>(Lifetime.Singleton);
		builder.RegisterComponentOnNewGameObject<GameCutsceneManager>(Lifetime.Singleton);
		builder.RegisterComponentOnNewGameObject<GamePingManager>(Lifetime.Singleton);
		builder.RegisterComponentOnNewGameObject<PlayerManager>(Lifetime.Singleton);
		builder.RegisterComponentOnNewGameObject<GameStageManager>(Lifetime.Singleton);
		builder.RegisterComponentOnNewGameObject<GameChatManager>(Lifetime.Singleton);
		builder.RegisterComponentOnNewGameObject<ArtifactDealManager>(Lifetime.Singleton);
		builder.RegisterComponentOnNewGameObject<TycoonManager>(Lifetime.Singleton);
		builder.RegisterComponentOnNewGameObject<GameNpcTalkManager>(Lifetime.Singleton);
		builder.RegisterComponentOnNewGameObject<GameNpcDialogManager>(Lifetime.Singleton);
		builder.RegisterComponentInNewPrefab(LoadPrefab<CameraManager>(_cameraManager), Lifetime.Singleton).AsSelf();
		builder.RegisterComponentInNewPrefab(LoadPrefab<GameFloatingTextManager>(_gameFloatingTextManager), Lifetime.Singleton).AsSelf();
		builder.RegisterComponentInNewPrefab(LoadPrefab<GraphicOptionController>(_graphicOptionController), Lifetime.Singleton).AsSelf();
		builder.RegisterInstance(LoadAsset(_miniMapSettings)).AsSelf();
		builder.RegisterInstance(LoadAsset(_loungeMapSettings)).AsSelf();
		builder.RegisterEntryPoint<AppStartUp>(Lifetime.Singleton);
	}

	private T LoadAsset<T>(AssetReferenceT<T> reference) where T : UnityEngine.Object
	{
		// shared body @0x180d8cdd0 (machine code checked): synchronous load, UnityEngine.Object null check, error log with the runtime key
		T val = reference.LoadAssetAsync().WaitForCompletion();
		if (val == null)
		{
			UnityEngine.Debug.LogError(string.Format("[RootLifetimeScope] Failed to load addressable asset (key: {0}).", reference.RuntimeKey));
		}
		return val;
	}

	private TComponent LoadPrefab<TComponent>(AssetReferenceGameObject reference) where TComponent : Component
	{
		// shared body @0x180d8cf10 (machine code checked): synchronous load; a null prefab logs the error and returns null, otherwise GetComponent<TComponent>()
		GameObject val = reference.LoadAssetAsync().WaitForCompletion();
		if (val == null)
		{
			UnityEngine.Debug.LogError(string.Format("[RootLifetimeScope] Failed to load addressable prefab (key: {0}).", reference.RuntimeKey));
			return null;
		}
		return val.GetComponent<TComponent>();
	}
}
