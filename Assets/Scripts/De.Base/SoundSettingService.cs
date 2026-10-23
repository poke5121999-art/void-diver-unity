using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameCommon.Enum;
using NemoLib.Log;
using UniRx;
using VContainer;

public class SoundSettingService : NativeBase
{
	[Inject]
	private SeriLogger _logger;

	[Inject]
	private AppManager _appManager;

	[Inject]
	private StorageService _saveStorage;

	[Inject]
	private SoundManager _soundManager;

	private SoundSettingContext _context;

	private bool _dirty;

	public readonly Subject<float> OnBgmVolumeApplied = new Subject<float>();

	public readonly Subject<float> OnSfxVolumeApplied = new Subject<float>();

	private string FileName => "SoundSettingContext";

	private string AccountId => (_appManager != null && _appManager.AccountId.HasValue()) ? _appManager.AccountId : string.Empty;

	public bool MasterVolumeOn => _context == null || _context.RxMasterVolumeOn.Value;

	public int MasterVolumeLevel => (_context != null) ? _context.RxMasterVolumeLevel.Value : 100;

	public bool BgmVolumeOn => _context == null || _context.RxBgmVolumeOn.Value;

	public int BgmVolumeLevel => (_context != null) ? _context.RxBgmVolumeLevel.Value : 100;

	public bool SfxVolumeOn => _context == null || _context.RxSfxVolumeOn.Value;

	public int SfxVolumeLevel => (_context != null) ? _context.RxSfxVolumeLevel.Value : 100;

	public IReadOnlyReactiveProperty<bool> RxMasterVolumeOn => _context?.RxMasterVolumeOn;

	public IReadOnlyReactiveProperty<int> RxMasterVolumeLevel => _context?.RxMasterVolumeLevel;

	public IReadOnlyReactiveProperty<bool> RxBgmVolumeOn => _context?.RxBgmVolumeOn;

	public IReadOnlyReactiveProperty<int> RxBgmVolumeLevel => _context?.RxBgmVolumeLevel;

	public IReadOnlyReactiveProperty<bool> RxSfxVolumeOn => _context?.RxSfxVolumeOn;

	public IReadOnlyReactiveProperty<int> RxSfxVolumeLevel => _context?.RxSfxVolumeLevel;

	private float MasterVolume => (float)MasterVolumeLevel / 100f;

	private float BgmVolume => (float)BgmVolumeLevel / 100f;

	private float SfxVolume => (float)SfxVolumeLevel / 100f;

	public bool AppliedBgmOn => (_context == null || _context.RxMasterVolumeOn.Value) && BgmVolumeOn;

	public bool AppliedSfxOn => (_context == null || _context.RxMasterVolumeOn.Value) && SfxVolumeOn;

	public float AppliedBgmVolume => AppliedBgmOn ? (MasterVolume * BgmVolume) : 0f;

	public float AppliedSfxVolume => AppliedSfxOn ? (MasterVolume * SfxVolume) : 0f;

	public async UniTask InitAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		if (_context != null)
		{
			return;
		}
		StorageLoadResult<SoundSettingContextData> result = await _saveStorage.LoadFromLocalAsync<SoundSettingContextData>(FileName, cancellationToken);
		if (result.LoadResult != ELoadResult.Success)
		{
			_context = CreateDefault();
			await SaveAsync(cancellationToken);
		}
		else
		{
			_context = new SoundSettingContext(result.Data);
		}
		ApplyBgmVolume();
		ApplySfxVolume();
	}

	private SoundSettingContext CreateDefault()
	{
		SoundSettingContextData data = new SoundSettingContextData
		{
			AccountId = AccountId,
			CreateTimeTicks = DateTime.UtcNow.Ticks,
			SaveTimeTicks = DateTime.UtcNow.Ticks,
			MasterVolumeOn = true,
			MasterVolumeLevel = 100,
			BgmVolumeOn = true,
			BgmVolumeLevel = 100,
			SfxVolumeOn = true,
			SfxVolumeLevel = 100
		};
		return new SoundSettingContext(data);
	}

	public void Reset()
	{
		if (_context != null)
		{
			SoundSettingContextData soundSettingContextData = CreateDefault().ToData();
			_context.RxMasterVolumeOn.Value = soundSettingContextData.MasterVolumeOn;
			_context.RxMasterVolumeLevel.Value = soundSettingContextData.MasterVolumeLevel;
			_context.RxBgmVolumeOn.Value = soundSettingContextData.BgmVolumeOn;
			_context.RxBgmVolumeLevel.Value = soundSettingContextData.BgmVolumeLevel;
			_context.RxSfxVolumeOn.Value = soundSettingContextData.SfxVolumeOn;
			_context.RxSfxVolumeLevel.Value = soundSettingContextData.SfxVolumeLevel;
			_dirty = true;
			ApplyBgmVolume();
			ApplySfxVolume();
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
		await _saveStorage.SaveToLocalAsync(FileName, _context.ToData(), cancellationToken);
	}

	public void SetMasterVolumeOn(bool volumeOn)
	{
		if (_context.RxMasterVolumeOn.Value != volumeOn)
		{
			_context.RxMasterVolumeOn.Value = volumeOn;
			_dirty = true;
			ApplyBgmVolume();
			ApplySfxVolume();
		}
	}

	public void SetMasterVolumeLevel(int volumeLevel)
	{
		if (_context.RxMasterVolumeLevel.Value != volumeLevel)
		{
			_context.RxMasterVolumeLevel.Value = volumeLevel;
			_dirty = true;
			ApplyBgmVolume();
			ApplySfxVolume();
		}
	}

	public void SetBgmVolumeOn(bool volumeOn)
	{
		if (_context.RxBgmVolumeOn.Value != volumeOn)
		{
			_context.RxBgmVolumeOn.Value = volumeOn;
			_dirty = true;
			ApplyBgmVolume();
		}
	}

	public void SetBgmVolumeLevel(int volumeLevel)
	{
		if (_context.RxBgmVolumeLevel.Value != volumeLevel)
		{
			_context.RxBgmVolumeLevel.Value = volumeLevel;
			_dirty = true;
			ApplyBgmVolume();
		}
	}

	public void SetSfxVolumeOn(bool volumeOn)
	{
		if (_context.RxSfxVolumeOn.Value != volumeOn)
		{
			_context.RxSfxVolumeOn.Value = volumeOn;
			_dirty = true;
			ApplySfxVolume();
		}
	}

	public void SetSfxVolumeLevel(int volumeLevel)
	{
		if (_context.RxSfxVolumeLevel.Value != volumeLevel)
		{
			_context.RxSfxVolumeLevel.Value = volumeLevel;
			_dirty = true;
			ApplySfxVolume();
		}
	}

	private void ApplyAll()
	{
		ApplyBgmVolume();
		ApplySfxVolume();
	}

	private void ApplyMasterVolume()
	{
		ApplyBgmVolume();
		ApplySfxVolume();
	}

	private void ApplyBgmVolume()
	{
		_soundManager.SetBgmVolume(AppliedBgmVolume);
		OnBgmVolumeApplied.OnNext(AppliedBgmVolume);
	}

	private void ApplySfxVolume()
	{
		_soundManager.SetBusVolume("SFX_GAME", AppliedSfxVolume);
		_soundManager.SetBusVolume("SFX_SYSTEM", AppliedSfxVolume);
		_soundManager.SetBusVolume("SFX_CUTSCENE", AppliedSfxVolume);
		OnSfxVolumeApplied.OnNext(AppliedSfxVolume);
	}
}
