using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameCommon.Enum;
using NemoLib.Log;
using Steamworks;
using VContainer;

public class SteamStorage : NativeBase, IStorage, IDisposable
{
	[Inject]
	private SeriLogger _logger;

	private CallResult<RemoteStorageFileWriteAsyncComplete_t> _steamWriteCallResult;

	private CallResult<RemoteStorageFileReadAsyncComplete_t> _steamReadCallResult;

	private static string ToPath(string fileName)
	{
		return fileName + ".sav";
	}

	ESaveResult IStorage.Save(string fileName, byte[] bytes)
	{
		(ESaveResult, string) tuple = SaveInternal(fileName + ".sav", bytes);
		ESaveResult item = tuple.Item1;
		switch (item)
		{
		case ESaveResult.Success:
			_logger.Debug("Steam save written: " + fileName + ".sav");
			return ESaveResult.Success;
		case ESaveResult.IoFailure:
			_logger.Warning(tuple.Item2);
			return ESaveResult.IoFailure;
		default:
			_logger.Error(tuple.Item2);
			return item;
		}
	}

	async UniTask<ESaveResult> IStorage.SaveAsync(string fileName, byte[] bytes, CancellationToken cancellationToken)
	{
		(ESaveResult, string) tuple = await SaveAsyncInternal(fileName + ".sav", bytes, cancellationToken);
		ESaveResult item = tuple.Item1;
		if (item == ESaveResult.Success)
		{
			_logger.Debug("Steam async save written: " + fileName + ".sav");
			return ESaveResult.Success;
		}
		if (item == ESaveResult.IoFailure)
		{
			_logger.Warning(tuple.Item2);
		}
		else
		{
			_logger.Error(tuple.Item2);
		}
		return item;
	}

	// TODO(decomp): generic method has no pseudo-C in the binary; body inferred from LoadInternal's signature and the call sites
	StorageLoadResult<T> IStorage.Load<T>(string fileName)
	{
		(ELoadResult, T, string) tuple = LoadInternal<T>(fileName + ".sav");
		if (tuple.Item1 != ELoadResult.Success && tuple.Item1 != ELoadResult.NotFound)
		{
			_logger.Error(tuple.Item3);
		}
		return new StorageLoadResult<T>(tuple.Item1, tuple.Item2);
	}

	// TODO(decomp): generic method has no pseudo-C in the binary; body inferred from LoadAsyncInternal's signature and the call sites
	async UniTask<StorageLoadResult<T>> IStorage.LoadAsync<T>(string fileName, CancellationToken cancellationToken)
	{
		(ELoadResult, T, string) tuple = await LoadAsyncInternal<T>(fileName + ".sav", cancellationToken);
		if (tuple.Item1 != ELoadResult.Success && tuple.Item1 != ELoadResult.NotFound)
		{
			_logger.Error(tuple.Item3);
		}
		return new StorageLoadResult<T>(tuple.Item1, tuple.Item2);
	}

	async UniTask<byte[]> IStorage.LoadAsync(string fileName, CancellationToken cancellationToken)
	{
		string filePath = fileName + ".sav";
		try
		{
			if (!SteamRemoteStorage.FileExists(filePath))
			{
				return null;
			}
			int fileSize = SteamRemoteStorage.GetFileSize(filePath);
			if (fileSize < 1)
			{
				return null;
			}
			SteamAPICall_t apiCall = SteamRemoteStorage.FileReadAsync(filePath, 0u, (uint)fileSize);
			byte[] result = await WaitForSteamReadCall(apiCall, (uint)fileSize).AttachExternalCancellation(cancellationToken);
			_logger.Debug("Steam async load bytes success: " + filePath);
			return result;
		}
		catch (Exception ex)
		{
			_logger.Error("Steam async load bytes failed [" + filePath + "]: " + ex.Message);
			return null;
		}
	}

	void IStorage.SaveJson(string fileName, string json)
	{
	}

	void IStorage.Delete(string fileName)
	{
		string text = fileName + ".sav";
		if (SteamRemoteStorage.FileExists(text))
		{
			SteamRemoteStorage.FileDelete(text);
		}
	}

	public void Dispose()
	{
		_steamWriteCallResult?.Dispose();
		_steamReadCallResult?.Dispose();
	}

	private void LogSaveFailure(ESaveResult result, string error)
	{
		if (result == ESaveResult.IoFailure)
		{
			_logger.Warning(error);
		}
		else
		{
			_logger.Error(error);
		}
	}

	private (ESaveResult, string) SaveInternal(string filePath, byte[] bytes)
	{
		try
		{
			SteamRemoteStorage.FileWrite(filePath, bytes, bytes.Length);
			return (ESaveResult.Success, null);
		}
		catch (Exception ex)
		{
			return (ESaveResult.Failed, "Steam save failed [" + filePath + "]: " + ex.Message);
		}
	}

	private async UniTask<(ESaveResult, string)> SaveAsyncInternal(string filePath, byte[] bytes, CancellationToken cancellationToken = default(CancellationToken))
	{
		try
		{
			SteamAPICall_t apiCall = SteamRemoteStorage.FileWriteAsync(filePath, bytes, (uint)bytes.Length);
			EResult eResult = await WaitForSteamWriteCall(apiCall).AttachExternalCancellation(cancellationToken);
			if (eResult == EResult.k_EResultOK)
			{
				return (ESaveResult.Success, null);
			}
			return ((eResult != (EResult)37) ? ESaveResult.Failed : ESaveResult.IoFailure, string.Format("Steam async save failed [{0}]: Steam FileWriteAsync failed: {1}", filePath, eResult));
		}
		catch (Exception ex)
		{
			// TODO(decomp): catch block not readable; result code and message prefix assumed @0x180207720
			return (ESaveResult.Failed, "Steam async save failed [" + filePath + "]: " + ex.Message);
		}
	}

	// TODO(decomp): generic method has no pseudo-C in the binary; body inferred from the signature and Steam remote storage API
	private (ELoadResult, T, string) LoadInternal<T>(string filePath) where T : class, ISaveData
	{
		try
		{
			if (!SteamRemoteStorage.FileExists(filePath))
			{
				return (ELoadResult.NotFound, null, null);
			}
			int fileSize = SteamRemoteStorage.GetFileSize(filePath);
			if (fileSize < 1)
			{
				return (ELoadResult.NotFound, null, null);
			}
			byte[] array = new byte[fileSize];
			int num = SteamRemoteStorage.FileRead(filePath, array, fileSize);
			if (num != fileSize)
			{
				return (ELoadResult.Failed, null, "Steam load failed [" + filePath + "]: read " + num + "/" + fileSize);
			}
			return (ELoadResult.Success, SerializeHelper.Deserialize<T>(array), null);
		}
		catch (Exception ex)
		{
			return (ELoadResult.Corrupted, null, "Steam load failed [" + filePath + "]: " + ex.Message);
		}
	}

	// TODO(decomp): generic method has no pseudo-C in the binary; body inferred from the signature and the sync variant
	private async UniTask<(ELoadResult, T, string)> LoadAsyncInternal<T>(string filePath, CancellationToken cancellationToken = default(CancellationToken)) where T : class, ISaveData
	{
		try
		{
			if (!SteamRemoteStorage.FileExists(filePath))
			{
				return (ELoadResult.NotFound, null, null);
			}
			int fileSize = SteamRemoteStorage.GetFileSize(filePath);
			if (fileSize < 1)
			{
				return (ELoadResult.NotFound, null, null);
			}
			SteamAPICall_t apiCall = SteamRemoteStorage.FileReadAsync(filePath, 0u, (uint)fileSize);
			byte[] array = await WaitForSteamReadCall(apiCall, (uint)fileSize).AttachExternalCancellation(cancellationToken);
			if (array == null)
			{
				return (ELoadResult.Failed, null, "Steam async load failed [" + filePath + "]");
			}
			return (ELoadResult.Success, SerializeHelper.Deserialize<T>(array), null);
		}
		catch (Exception ex)
		{
			return (ELoadResult.Corrupted, null, "Steam async load failed [" + filePath + "]: " + ex.Message);
		}
	}

	private UniTask<EResult> WaitForSteamWriteCall(SteamAPICall_t apiCall)
	{
		UniTaskCompletionSource<EResult> tcs = new UniTaskCompletionSource<EResult>();
		_steamWriteCallResult = CallResult<RemoteStorageFileWriteAsyncComplete_t>.Create(delegate(RemoteStorageFileWriteAsyncComplete_t result, bool failure)
		{
			tcs.TrySetResult(failure ? ((EResult)37) : result.m_eResult);
		});
		_steamWriteCallResult.Set(apiCall);
		return tcs.Task;
	}

	private UniTask<byte[]> WaitForSteamReadCall(SteamAPICall_t apiCall, uint fileSize)
	{
		UniTaskCompletionSource<byte[]> tcs = new UniTaskCompletionSource<byte[]>();
		byte[] buffer = new byte[fileSize];
		_steamReadCallResult = CallResult<RemoteStorageFileReadAsyncComplete_t>.Create(delegate(RemoteStorageFileReadAsyncComplete_t result, bool failure)
		{
			if (failure || result.m_eResult != EResult.k_EResultOK)
			{
				tcs.TrySetResult(null);
			}
			else
			{
				SteamRemoteStorage.FileReadAsyncComplete(result.m_hFileReadAsync, buffer, result.m_cubRead);
				tcs.TrySetResult(buffer);
			}
		});
		_steamReadCallResult.Set(apiCall);
		return tcs.Task;
	}
}
