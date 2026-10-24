using System;
using System.IO;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameCommon.Enum;
using NemoLib.Log;
using UniRx;
using UnityEngine;
using Newtonsoft.Json;
using VContainer;

public class StorageService : NativeBase
{
	[Inject]
	private EnvironmentSettings _environmentSettings;

	[Inject]
	private IStorage _storage;

	[Inject]
	private IBackupStorage _backup;

	[Inject]
	private SeriLogger _logger;

	private readonly Subject<ESaveResult> _saveFailedSubject = new Subject<ESaveResult>();

	private bool _saveFailureNotified;

	public IObservable<ESaveResult> OnSaveFailed => _saveFailedSubject;

	// body read from the shared instantiation StorageService.TrySerialize<object> @0x180de9490 (machine code checked)
	private bool TrySerialize<T>(string fileName, T data, out byte[] bytes) where T : ISaveData
	{
		try
		{
			bytes = SerializeHelper.Serialize(data);
			return true;
		}
		catch (Exception arg)
		{
			_logger.Error(string.Format("Serialize failed [{0}]: {1}", fileName, arg));
			bytes = null;
			return false;
		}
	}

	private void NotifyIfSaveFailed(ESaveResult result)
	{
		if (result == ESaveResult.Success)
		{
			_saveFailureNotified = false;
		}
		else if (!_saveFailureNotified)
		{
			_saveFailureNotified = true;
			_saveFailedSubject.OnNext(result);
		}
	}

	// StorageService.Save<object> @0x180de9200 (machine code checked): after the save, the Local platform also writes an indented JSON copy
	public void Save<T>(string fileName, T data) where T : ISaveData
	{
		if (TrySerialize(fileName, data, out var bytes))
		{
			NotifyIfSaveFailed(_storage.Save(fileName, bytes));
			if (_environmentSettings.Platform == EPlatform.Local)
			{
				_storage.SaveJson(fileName, JsonConvert.SerializeObject(data, Formatting.Indented, (JsonSerializerSettings)null));
			}
		}
	}

	// <SaveAsync>d__11<object>.MoveNext @0x1810db860 (machine code checked): no cancellation filter on the catch; the Local platform also writes an indented JSON copy of the snapshot
	public async UniTask SaveAsync<T>(string fileName, T data, CancellationToken cancellationToken = default(CancellationToken)) where T : ISaveData
	{
		T snapshot = (T)data.Clone();
		byte[] bytes;
		try
		{
			bytes = await UniTask.RunOnThreadPool(() => SerializeHelper.Serialize(snapshot), configureAwait: true, cancellationToken);
		}
		catch (Exception arg)
		{
			_logger.Error(string.Format("Serialize failed [{0}]: {1}", fileName, arg));
			return;
		}
		NotifyIfSaveFailed(await _storage.SaveAsync(fileName, bytes, cancellationToken));
		if (_environmentSettings.Platform == EPlatform.Local)
		{
			_storage.SaveJson(fileName, JsonConvert.SerializeObject(snapshot, Formatting.Indented, (JsonSerializerSettings)null));
		}
	}

	public async UniTask SaveAsync(string fileName, byte[] bytes, CancellationToken cancellationToken = default(CancellationToken))
	{
		NotifyIfSaveFailed(await _storage.SaveAsync(fileName, bytes, cancellationToken));
	}

	public UniTask<byte[]> LoadAsync(string fileName, CancellationToken cancellationToken = default(CancellationToken))
	{
		return _storage.LoadAsync(fileName, cancellationToken);
	}

	public void Delete(string fileName)
	{
		_storage.Delete(fileName);
	}

	// <SaveToLocalAsync>d__15<object>.MoveNext @0x1810ddb50 (machine code checked): "local" means PlayerPrefs (Base64 of the serialized bytes), not a file
	public async UniTask SaveToLocalAsync<T>(string key, T data, CancellationToken cancellationToken = default(CancellationToken)) where T : ISaveData
	{
		T snapshot = (T)data.Clone();
		try
		{
			byte[] inArray = await UniTask.RunOnThreadPool(() => SerializeHelper.Serialize(snapshot), configureAwait: true, cancellationToken);
			PlayerPrefs.SetString(key, Convert.ToBase64String(inArray));
			PlayerPrefs.Save();
		}
		catch (Exception arg)
		{
			_logger.Error(string.Format("Serialize failed [{0}]: {1}", key, arg));
		}
	}

	// <LoadFromLocalAsync>d__16<object>.MoveNext @0x1827c9b60 and its <>c__DisplayClass16_0.b__0 @0x180f60010 (machine code checked)
	public async UniTask<StorageLoadResult<T>> LoadFromLocalAsync<T>(string key, CancellationToken cancellationToken = default(CancellationToken)) where T : class, ISaveData
	{
		if (!PlayerPrefs.HasKey(key))
		{
			return new StorageLoadResult<T>(ELoadResult.NotFound);
		}
		string base64 = PlayerPrefs.GetString(key);
		if (string.IsNullOrEmpty(base64))
		{
			return new StorageLoadResult<T>(ELoadResult.NotFound);
		}
		return await UniTask.RunOnThreadPool(delegate
		{
			try
			{
				T val = SerializeHelper.Deserialize<T>(Convert.FromBase64String(base64));
				val.Normalize();
				return new StorageLoadResult<T>(ELoadResult.Success, val);
			}
			catch (Exception)
			{
				return new StorageLoadResult<T>(ELoadResult.Corrupted);
			}
		}, configureAwait: true, cancellationToken);
	}

	// StorageService.Load<object> @0x180de89c0 (machine code checked)
	public StorageLoadResult<T> Load<T>(string fileName, string accountId) where T : class, ISaveData
	{
		StorageLoadResult<T> storageLoadResult = _storage.Load<T>(fileName);
		if (storageLoadResult.LoadResult != ELoadResult.Success)
		{
			return storageLoadResult;
		}
		T data = storageLoadResult.Data;
		if (data.AccountId != accountId)
		{
			return new StorageLoadResult<T>(ELoadResult.UserMismatch);
		}
		data.Normalize();
		return storageLoadResult;
	}

	// <LoadAsync>d__18<object>.MoveNext @0x1827c8860 (machine code checked)
	public async UniTask<StorageLoadResult<T>> LoadAsync<T>(string fileName, string accountId, CancellationToken cancellationToken = default(CancellationToken)) where T : class, ISaveData
	{
		StorageLoadResult<T> storageLoadResult = await _storage.LoadAsync<T>(fileName, cancellationToken);
		if (storageLoadResult.LoadResult != ELoadResult.Success)
		{
			return storageLoadResult;
		}
		T data = storageLoadResult.Data;
		if (data.AccountId != accountId)
		{
			return new StorageLoadResult<T>(ELoadResult.UserMismatch);
		}
		data.Normalize();
		return storageLoadResult;
	}

	// StorageService.Backup<object> @0x180de8160 (machine code checked)
	public void Backup<T>(string fileName, T data, long ticks) where T : ISaveData
	{
		if (TrySerialize(fileName, data, out var bytes))
		{
			_backup.Save(fileName, bytes, ticks);
		}
	}

	// <BackupAsync>d__20<object>.MoveNext @0x18252d0e0 (machine code checked): the serialize failure is logged and the backup is skipped
	public async UniTask BackupAsync<T>(string fileName, T data, long ticks, CancellationToken cancellationToken = default(CancellationToken)) where T : ISaveData
	{
		T snapshot = (T)data.Clone();
		byte[] bytes;
		try
		{
			bytes = await UniTask.RunOnThreadPool(() => SerializeHelper.Serialize(snapshot), configureAwait: true, cancellationToken);
		}
		catch (Exception arg)
		{
			_logger.Error(string.Format("Serialize failed [{0}]: {1}", fileName, arg));
			return;
		}
		await _backup.SaveAsync(fileName, bytes, ticks, cancellationToken);
	}

	public long[] GetBackupList()
	{
		return _backup.GetBackupList();
	}

	public void DeleteBackup(long ticks)
	{
		_backup.Delete(ticks);
	}

	// StorageService.LoadBackup<object> @0x180de8780 (machine code checked)
	public T LoadBackup<T>(string fileName, long ticks) where T : class, ISaveData
	{
		T val = _backup.Load<T>(fileName, ticks);
		val?.Normalize();
		return val;
	}

	// <LoadBackupAsync>d__24<object>.MoveNext @0x1827c8e60 (machine code checked)
	public async UniTask<T> LoadBackupAsync<T>(string fileName, long ticks, CancellationToken cancellationToken = default(CancellationToken)) where T : class, ISaveData
	{
		T val = await _backup.LoadAsync<T>(fileName, ticks, cancellationToken);
		val?.Normalize();
		return val;
	}

	// StorageService.Delete<object> @0x180de8380 (machine code checked): backs the loaded file up under `ticks` before deleting it; the backup itself is kept
	public void Delete<T>(string fileName, long ticks) where T : class, ISaveData
	{
		StorageLoadResult<T> storageLoadResult = _storage.Load<T>(fileName);
		if (storageLoadResult.LoadResult == ELoadResult.Success && TrySerialize(fileName, storageLoadResult.Data, out var bytes))
		{
			_backup.Save(fileName, bytes, ticks);
		}
		_storage.Delete(fileName);
	}

	// <DeleteAsync>d__26<object>.MoveNext @0x182637500 (machine code checked): same as Delete<T> but through BackupAsync, under backupFileName when given
	public async UniTask DeleteAsync<T>(string fileName, long ticks, string backupFileName = null, CancellationToken cancellationToken = default(CancellationToken)) where T : class, ISaveData
	{
		StorageLoadResult<T> storageLoadResult = await _storage.LoadAsync<T>(fileName, cancellationToken);
		if (storageLoadResult.LoadResult == ELoadResult.Success)
		{
			await BackupAsync(backupFileName ?? fileName, storageLoadResult.Data, ticks, cancellationToken);
		}
		_storage.Delete(fileName);
	}
}
