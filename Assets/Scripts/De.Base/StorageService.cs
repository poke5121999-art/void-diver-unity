using System;
using System.IO;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameCommon.Enum;
using NemoLib.Log;
using UniRx;
using UnityEngine;
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

	// TODO(decomp): generic method has no pseudo-C in the binary; body inferred from the signature and call sites
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
			NotifyIfSaveFailed(ESaveResult.Failed);
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

	// TODO(decomp): generic method has no pseudo-C in the binary; body inferred from the signature and call sites
	public void Save<T>(string fileName, T data) where T : ISaveData
	{
		if (TrySerialize(fileName, data, out var bytes))
		{
			NotifyIfSaveFailed(_storage.Save(fileName, bytes));
		}
	}

	// TODO(decomp): generic method has no pseudo-C in the binary; the snapshot field of the lost display class suggests Clone() before the thread-pool serialize
	public async UniTask SaveAsync<T>(string fileName, T data, CancellationToken cancellationToken = default(CancellationToken)) where T : ISaveData
	{
		T snapshot = (T)data.Clone();
		byte[] bytes;
		try
		{
			bytes = await UniTask.RunOnThreadPool(() => SerializeHelper.Serialize(snapshot), configureAwait: true, cancellationToken);
		}
		catch (Exception arg) when (!(arg is OperationCanceledException))
		{
			_logger.Error(string.Format("Serialize failed [{0}]: {1}", fileName, arg));
			NotifyIfSaveFailed(ESaveResult.Failed);
			return;
		}
		NotifyIfSaveFailed(await _storage.SaveAsync(fileName, bytes, cancellationToken));
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

	// TODO(decomp): generic method has no pseudo-C in the binary; the local directory/extension are assumed to match LocalStorage
	public async UniTask SaveToLocalAsync<T>(string key, T data, CancellationToken cancellationToken = default(CancellationToken)) where T : ISaveData
	{
		T snapshot = (T)data.Clone();
		byte[] bytes;
		try
		{
			bytes = await UniTask.RunOnThreadPool(() => SerializeHelper.Serialize(snapshot), configureAwait: true, cancellationToken);
			string path = ToLocalPath(key);
			await UniTask.RunOnThreadPool(delegate
			{
				File.WriteAllBytes(path, bytes);
			}, configureAwait: true, cancellationToken);
		}
		catch (Exception arg) when (!(arg is OperationCanceledException))
		{
			_logger.Error(string.Format("Local save failed [{0}]: {1}", key, arg));
			NotifyIfSaveFailed(ESaveResult.Failed);
		}
	}

	// TODO(decomp): generic method has no pseudo-C in the binary; the local directory/extension are assumed to match LocalStorage
	public async UniTask<StorageLoadResult<T>> LoadFromLocalAsync<T>(string key, CancellationToken cancellationToken = default(CancellationToken)) where T : class, ISaveData
	{
		string path = ToLocalPath(key);
		if (!File.Exists(path))
		{
			return new StorageLoadResult<T>(ELoadResult.NotFound);
		}
		try
		{
			T data = await UniTask.RunOnThreadPool(() => SerializeHelper.Deserialize<T>(File.ReadAllBytes(path)), configureAwait: true, cancellationToken);
			data.Normalize();
			return new StorageLoadResult<T>(ELoadResult.Success, data);
		}
		catch (Exception arg) when (!(arg is OperationCanceledException))
		{
			_logger.Error(string.Format("Local load failed [{0}]: {1}", key, arg));
			return new StorageLoadResult<T>(ELoadResult.Corrupted);
		}
	}

	private static string ToLocalPath(string key)
	{
		string text = Path.Combine(Directory.GetParent(Application.dataPath).FullName, "SavesDir");
		Directory.CreateDirectory(text);
		return Path.Combine(text, key + ".sav");
	}

	// TODO(decomp): generic method has no pseudo-C in the binary; Normalize and the account check are inferred from ISaveData
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

	// TODO(decomp): generic method has no pseudo-C in the binary; Normalize and the account check are inferred from ISaveData
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

	// TODO(decomp): generic method has no pseudo-C in the binary; body inferred from the signature
	public void Backup<T>(string fileName, T data, long ticks) where T : ISaveData
	{
		if (TrySerialize(fileName, data, out var bytes))
		{
			_backup.Save(fileName, bytes, ticks);
		}
	}

	// TODO(decomp): generic method has no pseudo-C in the binary; body inferred from the signature
	public async UniTask BackupAsync<T>(string fileName, T data, long ticks, CancellationToken cancellationToken = default(CancellationToken)) where T : ISaveData
	{
		T snapshot = (T)data.Clone();
		byte[] bytes = await UniTask.RunOnThreadPool(() => SerializeHelper.Serialize(snapshot), configureAwait: true, cancellationToken);
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

	// TODO(decomp): generic method has no pseudo-C in the binary; body inferred from the signature
	public T LoadBackup<T>(string fileName, long ticks) where T : class, ISaveData
	{
		T val = _backup.Load<T>(fileName, ticks);
		val?.Normalize();
		return val;
	}

	// TODO(decomp): generic method has no pseudo-C in the binary; body inferred from the signature
	public async UniTask<T> LoadBackupAsync<T>(string fileName, long ticks, CancellationToken cancellationToken = default(CancellationToken)) where T : class, ISaveData
	{
		T val = await _backup.LoadAsync<T>(fileName, ticks, cancellationToken);
		val?.Normalize();
		return val;
	}

	// TODO(decomp): generic method has no pseudo-C in the binary; what T is used for is unknown
	public void Delete<T>(string fileName, long ticks) where T : class, ISaveData
	{
		_storage.Delete(fileName);
		_backup.Delete(ticks);
	}

	// TODO(decomp): generic method has no pseudo-C in the binary; what T and backupFileName are used for is unknown
	public async UniTask DeleteAsync<T>(string fileName, long ticks, string backupFileName = null, CancellationToken cancellationToken = default(CancellationToken)) where T : class, ISaveData
	{
		_storage.Delete(fileName);
		_backup.Delete(ticks);
		await UniTask.CompletedTask;
	}
}
