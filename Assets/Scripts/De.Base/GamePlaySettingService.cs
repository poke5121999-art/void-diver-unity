using System;
using System.Globalization;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameCommon.Enum;
using Base.Utility;
using GameCommon.Utility;
using NemoLib.Log;
using Steamworks;
using UniRx;
using UnityEngine;
using VContainer;

public class GamePlaySettingService : NativeBase
{
	[Inject]
	private SeriLogger _logger;

	[Inject]
	private AppManager _appManager;

	[Inject]
	private StorageService _saveStorage;

	[Inject]
	private GraphicSettingService _graphicSetting;

	[Inject]
	private GraphicOptionController _graphicOptionController;

	private readonly CompositeDisposable _disposable = new CompositeDisposable();

	private const int MIN_PAD_SENSITIVITY = 1;

	private GamePlaySettingContext _context;

	private bool _dirty;

	public readonly Subject<ELanguage> OnLanguageApplied = new Subject<ELanguage>();

	private int DefaultPadSensitivity => 40;

	private string FileName => "GamePlaySettingContext";

	private string AccountId => _appManager?.AccountId ?? string.Empty;

	public ELanguage Language => (_context != null) ? _context.RxLanguage.Value : ELanguage.En;

	public bool AimSupportOn => _context == null || _context.RxAimSupportOn.Value;

	public bool AutoAttackOn => _context != null && _context.RxAutoAttackOn.Value;

	public bool LockMouseOn => _context == null || _context.RxLockMouseOn.Value;

	public bool RstickAttackOn => _context == null || _context.RxRstickAttackOn.Value;

	public bool RunToggleOn => _context != null && _context.RxRunToggleOn.Value;

	public bool AimLineOn => _context == null || _context.RxAimLineOn.Value;

	public int PadSensitivity => Math.Max(1, (_context != null) ? _context.RxPadSensitivity.Value : 40);

	public IReadOnlyReactiveProperty<ELanguage> RxLanguage => _context?.RxLanguage;

	public IReadOnlyReactiveProperty<bool> RxAimSupportOn => _context?.RxAimSupportOn;

	public IReadOnlyReactiveProperty<bool> RxAutoAttackOn => _context?.RxAutoAttackOn;

	public IReadOnlyReactiveProperty<bool> RxLockMouseOn => _context?.RxLockMouseOn;

	public IReadOnlyReactiveProperty<bool> RxRstickAttackOn => _context?.RxRstickAttackOn;

	public IReadOnlyReactiveProperty<bool> RxRunToggleOn => _context?.RxRunToggleOn;

	public IReadOnlyReactiveProperty<bool> RxAimLineOn => _context?.RxAimLineOn;

	public IReadOnlyReactiveProperty<int> RxPadSensitivity => _context?.RxPadSensitivity;

	public async UniTask InitAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		if (_context == null)
		{
			StorageLoadResult<GamePlaySettingContextData> result = await _saveStorage.LoadAsync<GamePlaySettingContextData>("GamePlaySettingContext", AccountId, cancellationToken);
			if (result.LoadResult != ELoadResult.Success)
			{
				_context = CreateDefault();
				await SaveAsync(cancellationToken);
			}
			else
			{
				_context = new GamePlaySettingContext(result.Data);
			}
		}
		_context.RxLanguage.Subscribe(OnUpdateLanguage).AddTo(_disposable);
		_context.RxLockMouseOn.Subscribe(OnUpdateLockMouse).AddTo(_disposable);
		_context.RxAimLineOn.Subscribe(OnUpdateAimLineOn).AddTo(_disposable);
		_graphicSetting.RxScreenMode.Skip(1).Subscribe(delegate
		{
			OnUpdateLockMouse(_context?.RxLockMouseOn.Value ?? true);
		}).AddTo(_disposable);
	}

	private GamePlaySettingContext CreateDefault()
	{
		GamePlaySettingContextData data = new GamePlaySettingContextData
		{
			AccountId = AccountId,
			CreateTimeTicks = DateTime.UtcNow.Ticks,
			SaveTimeTicks = DateTime.UtcNow.Ticks
		};
		if (!TryGetSteamLanguage(out var language))
		{
			language = GetCultureLanguage();
		}
		data.Language = language;
		data.AimSupportOn = true;
		data.AutoAttackOn = true;
		data.LockMouseOn = false;
		data.RstickAttackOn = false;
		data.RunToggleOn = true;
		data.AimLineOn = false;
		data.PadSensitivity = 40;
		return new GamePlaySettingContext(data);
	}

	private static ELanguage GetDefaultLanguage()
	{
		if (TryGetSteamLanguage(out var language))
		{
			return language;
		}
		return GetCultureLanguage();
	}

	private static bool TryGetSteamLanguage(out ELanguage language)
	{
		language = ELanguage.None;
		if (!SteamManager.Initialized)
		{
			language = ELanguage.En;
			return false;
		}
		string currentGameLanguage = SteamApps.GetCurrentGameLanguage();
		if (string.IsNullOrEmpty(currentGameLanguage))
		{
			language = ELanguage.En;
			return false;
		}
		switch (currentGameLanguage)
		{
		case "russian":
			language = ELanguage.Ru;
			return true;
		case "brazilian":
			language = ELanguage.Br;
			return true;
		case "french":
			language = ELanguage.Fr;
			return true;
		case "latam":
		case "spanish":
			language = ELanguage.Es;
			return true;
		case "portuguese":
			language = ELanguage.Pt;
			return true;
		case "japanese":
			language = ELanguage.Jp;
			return true;
		case "italian":
			language = ELanguage.It;
			return true;
		case "schinese":
			language = ELanguage.Cn;
			return true;
		case "polish":
			language = ELanguage.Pl;
			return true;
		case "koreana":
			language = ELanguage.Ko;
			return true;
		case "tchinese":
			language = ELanguage.Tw;
			return true;
		case "german":
			language = ELanguage.De;
			return true;
		case "vietnamese":
			language = ELanguage.Vi;
			return true;
		case "turkish":
			language = ELanguage.Tr;
			return true;
		default:
			language = ELanguage.En;
			return true;
		}
	}

	private static ELanguage GetCultureLanguage()
	{
		CultureInfo currentCulture = CultureInfo.CurrentCulture;
		switch (currentCulture.TwoLetterISOLanguageName)
		{
		case "vi":
			return ELanguage.Vi;
		case "ko":
			return ELanguage.Ko;
		case "pl":
			return ELanguage.Pl;
		case "zh":
		{
			string name = currentCulture.Name;
			if (name == "zh-TW" || name == "zh-HK" || name == "zh-MO")
			{
				return ELanguage.Tw;
			}
			return ELanguage.Cn;
		}
		case "es":
			return ELanguage.Es;
		case "it":
			return ELanguage.It;
		case "tr":
			return ELanguage.Tr;
		case "ru":
			return ELanguage.Ru;
		case "fr":
			return ELanguage.Fr;
		case "de":
			return ELanguage.De;
		case "pt":
			return (currentCulture.Name == "pt-BR") ? ELanguage.Br : ELanguage.Pt;
		case "ja":
			return ELanguage.Jp;
		default:
			return ELanguage.En;
		}
	}

	public void Reset()
	{
		if (_context != null)
		{
			GamePlaySettingContextData gamePlaySettingContextData = CreateDefault().ToData();
			_context.RxAimSupportOn.Value = gamePlaySettingContextData.AimSupportOn;
			_context.RxAutoAttackOn.Value = gamePlaySettingContextData.AutoAttackOn;
			_context.RxLockMouseOn.Value = gamePlaySettingContextData.LockMouseOn;
			_context.RxRstickAttackOn.Value = gamePlaySettingContextData.RstickAttackOn;
			_context.RxRunToggleOn.Value = gamePlaySettingContextData.RunToggleOn;
			_context.RxAimLineOn.Value = gamePlaySettingContextData.AimLineOn;
			_context.RxPadSensitivity.Value = gamePlaySettingContextData.PadSensitivity;
			_dirty = true;
		}
	}

	public void Flush()
	{
		if (_context != null && _dirty)
		{
			_dirty = false;
			SaveAsync().Forget();
		}
	}

	private async UniTask SaveAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		_context.AccountId = AccountId;
		_context.SaveTimeTicks = DateTime.UtcNow.Ticks;
		await _saveStorage.SaveAsync(FileName, _context.ToData(), cancellationToken);
	}

	public void SetLanguage(ELanguage language)
	{
		if (_context.RxLanguage.Value != language)
		{
			_context.RxLanguage.Value = language;
			_dirty = true;
		}
	}

	public void SetAimSupportOn(bool aimSupportOn)
	{
		if (_context.RxAimSupportOn.Value != aimSupportOn)
		{
			_context.RxAimSupportOn.Value = aimSupportOn;
			_dirty = true;
		}
	}

	public void SetAutoAttackOn(bool autoAttackOn)
	{
		if (_context.RxAutoAttackOn.Value != autoAttackOn)
		{
			_context.RxAutoAttackOn.Value = autoAttackOn;
			_dirty = true;
		}
	}

	public void SetLockMouseOn(bool lockMouseOn)
	{
		if (_context.RxLockMouseOn.Value != lockMouseOn)
		{
			_context.RxLockMouseOn.Value = lockMouseOn;
			_dirty = true;
		}
	}

	public void SetRstickAttackOn(bool rstickAttackOn)
	{
		if (_context.RxRstickAttackOn.Value != rstickAttackOn)
		{
			_context.RxRstickAttackOn.Value = rstickAttackOn;
			_dirty = true;
		}
	}

	public void SetRunToggleOn(bool runToggleOn)
	{
		if (_context.RxRunToggleOn.Value != runToggleOn)
		{
			_context.RxRunToggleOn.Value = runToggleOn;
			_dirty = true;
		}
	}

	public void SetAimLineOn(bool aimLineOn)
	{
		if (_context.RxAimLineOn.Value != aimLineOn)
		{
			_context.RxAimLineOn.Value = aimLineOn;
			_dirty = true;
		}
	}

	public void SetPadSensitivity(int padSensitivity)
	{
		if (_context.RxPadSensitivity.Value != padSensitivity)
		{
			_context.RxPadSensitivity.Value = padSensitivity;
			_dirty = true;
		}
	}

	private void OnUpdateLanguage(ELanguage language)
	{
		LocalizationUtility.SetLanguage((_context != null) ? _context.RxLanguage.Value : ELanguage.En);
		TableHelper.LoadLocalizedTables();
		TableUtility.Localize();
		OnLanguageApplied.OnNext(language);
	}

	private void OnUpdateLockMouse(bool lockMouseOn)
	{
		if (_graphicSetting.ScreenMode == EScreenMode.Fullscreen)
		{
			lockMouseOn = true;
		}
		Cursor.lockState = lockMouseOn ? CursorLockMode.Confined : CursorLockMode.None;
	}

	private void OnUpdateAimLineOn(bool aimLineOn)
	{
		_graphicOptionController.SetCursorLineEnable(aimLineOn);
	}
}
