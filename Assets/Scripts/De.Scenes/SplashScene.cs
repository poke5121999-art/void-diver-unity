using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using NemoLib.AssetManagement;
using NemoLib.Log;
using NemoLib.Mvp;
using NemoLib.Scene;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using VContainer;
using Base.Utility;

public class SplashScene : BaseScenePresenter
{
	[View]
	private SplashSceneView _view;

	[Inject]
	private SeriLogger _logger;

	[Inject]
	private GraphicSettingService _graphicSettings;

	[Inject]
	private SoundSettingService _soundSettings;

	[Inject]
	private GamePlaySettingService _gamePlaySettings;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private EnvironmentSettings _environmentSettings;

	[Inject]
	private AppManager _appManager;

	[Inject]
	private GameEventSystem _eventSystem;

	private bool _loadingEnded;

	protected override Selectable SelectedObject => null;

	protected override void Start()
	{
		base.Start();
		_view.SplashVideo.GetComponent<VideoPlayerHandler>().RxVideoEnded.Subscribe(OnSplashVideoEnd).AddTo(this);
		StartAsync().Forget();
	}

	private async UniTask StartAsync()
	{
		UniTask preloadTask = _assetManager.EnsureAsync("Login");
		await StreamingAssetsFiles.PrepareAsync();
		WebStep("streaming assets");
		await TableHelper.LoadEncryptedTablesAsync();
		WebStep("tables");
		await AuthAsync();
		WebStep("auth");
		await _graphicSettings.InitAsync();
		WebStep("graphic settings");
		await _soundSettings.InitAsync();
		WebStep("sound settings");
		await _gamePlaySettings.InitAsync();
		WebStep("gameplay settings");
#if UNITY_WEBGL && !UNITY_EDITOR
		int frameRate = Application.targetFrameRate;
		Application.targetFrameRate = WebLoadingScreen.PreloadFrameRate;
		await preloadTask;
		Application.targetFrameRate = frameRate;
#else
		await preloadTask;
#endif
		WebStep("Login assets");
		_loadingEnded = true;
	}

	private async UniTask AuthAsync()
	{
		if (!await _appManager.AuthenticateAsync(_environmentSettings.Platform))
		{
			_logger.Error("[Auth] Authentication failed.");
			return;
		}
		_eventSystem.Trigger(new LoginStepEvent(ELoginStep.SESSION_IN, ELoginDtl.SUCCESS, ELoginRsn.AUTO, ELoginType.LC, 0L));
		_eventSystem.Trigger(new LoginStepEvent(ELoginStep.SPLASH, ELoginDtl.SUCCESS, ELoginRsn.AUTO, ELoginType.LC, 0L));
	}

	// The web player logs each loading step, so a step that never ends can be named from the browser console. A
	// warning, because the game's logger drops plain logs there.
	private static void WebStep(string step)
	{
#if UNITY_WEBGL && !UNITY_EDITOR
		UnityEngine.Debug.LogWarning("[SplashScene] " + step + " done at " + Time.realtimeSinceStartup.ToString("F1") + "s");
#endif
	}

	private void OnSplashVideoEnd(VideoPlayer videoPlayer)
	{
		WebStep("splash video (loading ended: " + _loadingEnded + ")");
		_view.SplashVideo.SetActive(false);
		if (!_loadingEnded)
		{
			WaitLoadingAndStart().Forget();
		}
		else
		{
			SceneManager.LoadSceneAsync("Scene/StartScene.unity").Forget();
		}
		async UniTask WaitLoadingAndStart()
		{
			GameLoadingPopupPresenter popup = await PopupManager.OpenAsync<GameLoadingPopupPresenter>(new GameLoadingPopupModel());
			await UniTask.WaitUntil(() => _loadingEnded, PlayerLoopTiming.Update);
			await SceneManager.LoadSceneAsync("Scene/StartScene.unity");
			popup.Close();
		}
	}
}
