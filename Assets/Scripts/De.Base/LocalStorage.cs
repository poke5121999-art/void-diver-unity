using System;
using System.IO;
using System.Text;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameCommon.Enum;
using NemoLib.Log;
using UnityEngine;
using VContainer;

public class LocalStorage : NativeBase, IStorage
{
	[Inject]
	private SeriLogger _logger;

	private readonly string _saveDir;

	public LocalStorage()
	{
		_saveDir = Path.Combine(Directory.GetParent(Application.dataPath).FullName, "SavesDir");
		Directory.CreateDirectory(_saveDir);
	}

	private string ToPath(string fileName)
	{
		return Path.Combine(_saveDir, fileName + ".sav");
	}

	ESaveResult IStorage.Save(string fileName, byte[] bytes)
	{
		try
		{
			WriteFileAtomic(ToPath(fileName), bytes);
			_logger.Debug("Local save written: " + fileName);
			return ESaveResult.Success;
		}
		catch (Exception e)
		{
			ESaveResult eSaveResult = ToSaveResult(e);
			_logger.Error(string.Format("Local save failed [{0}]: {1}", eSaveResult, e));
			return eSaveResult;
		}
	}

	async UniTask<ESaveResult> IStorage.SaveAsync(string fileName, byte[] bytes, CancellationToken cancellationToken)
	{
		try
		{
			await UniTask.RunOnThreadPool(delegate
			{
				WriteFileAtomic(ToPath(fileName), bytes);
			}, configureAwait: true, cancellationToken);
			_logger.Debug("Local save written: " + fileName);
			return ESaveResult.Success;
		}
		catch (Exception e)
		{
			ESaveResult eSaveResult = ToSaveResult(e);
			_logger.Error(string.Format("Local save failed [{0}]: {1}", eSaveResult, e));
			return eSaveResult;
		}
	}

	private static ESaveResult ToSaveResult(Exception e)
	{
		if (e is IOException)
		{
			return ESaveResult.IoFailure;
		}
		return ESaveResult.Failed;
	}

	// LocalStorage.IStorage.Load<object> @0x180c9c7a0 (machine code checked): two catch bodies, "corrupted" (2) and "failed" (3); that the first wraps only Deserialize is inferred
	StorageLoadResult<T> IStorage.Load<T>(string fileName)
	{
		string filePath = ToPath(fileName);
		if (!File.Exists(filePath))
		{
			return new StorageLoadResult<T>(ELoadResult.NotFound);
		}
		try
		{
			byte[] bytes = File.ReadAllBytes(filePath);
			try
			{
				T data = SerializeHelper.Deserialize<T>(bytes);
				_logger.Debug("Local load success: " + filePath);
				return new StorageLoadResult<T>(ELoadResult.Success, data);
			}
			catch (Exception arg)
			{
				_logger.Error(string.Format("Local load corrupted [{0}]: {1}", filePath, arg));
				return new StorageLoadResult<T>(ELoadResult.Corrupted);
			}
		}
		catch (Exception arg2)
		{
			_logger.Error(string.Format("Local load failed [{0}]: {1}", filePath, arg2));
			return new StorageLoadResult<T>(ELoadResult.Failed);
		}
	}

	// <IStorage-LoadAsync>d__8<object>.MoveNext @0x1827c0c50 (machine code checked), same catch layout as the sync variant
	async UniTask<StorageLoadResult<T>> IStorage.LoadAsync<T>(string fileName, CancellationToken cancellationToken)
	{
		string filePath = ToPath(fileName);
		if (!File.Exists(filePath))
		{
			return new StorageLoadResult<T>(ELoadResult.NotFound);
		}
		try
		{
			byte[] bytes = await UniTask.RunOnThreadPool(() => File.ReadAllBytes(filePath), configureAwait: true, cancellationToken);
			try
			{
				T data = await UniTask.RunOnThreadPool(() => SerializeHelper.Deserialize<T>(bytes), configureAwait: true, cancellationToken);
				_logger.Debug("Local load success: " + filePath);
				return new StorageLoadResult<T>(ELoadResult.Success, data);
			}
			catch (Exception arg)
			{
				_logger.Error(string.Format("Local load corrupted [{0}]: {1}", filePath, arg));
				return new StorageLoadResult<T>(ELoadResult.Corrupted);
			}
		}
		catch (Exception arg2)
		{
			_logger.Error(string.Format("Local load failed [{0}]: {1}", filePath, arg2));
			return new StorageLoadResult<T>(ELoadResult.Failed);
		}
	}

	async UniTask<byte[]> IStorage.LoadAsync(string fileName, CancellationToken cancellationToken)
	{
		string filePath = ToPath(fileName);
		if (!File.Exists(filePath))
		{
			return null;
		}
		try
		{
			byte[] result = await UniTask.RunOnThreadPool(() => File.ReadAllBytes(filePath), configureAwait: true, cancellationToken);
			_logger.Debug("Local load bytes success: " + filePath);
			return result;
		}
		catch (Exception arg)
		{
			_logger.Error(string.Format("Local load bytes failed [{0}]: {1}", filePath, arg));
			return null;
		}
	}

	void IStorage.SaveJson(string fileName, string json)
	{
		File.WriteAllText(Path.Combine(_saveDir, fileName + ".json"), json, Encoding.UTF8);
	}

	void IStorage.Delete(string fileName)
	{
		string path = ToPath(fileName);
		if (File.Exists(path))
		{
			File.Delete(path);
		}
	}

	private static void WriteFileAtomic(string filePath, byte[] bytes)
	{
		string text = filePath + ".tmp";
		try
		{
			using (FileStream fileStream = new FileStream(text, FileMode.Create, FileAccess.Write, FileShare.None))
			{
				fileStream.Write(bytes, 0, bytes.Length);
				fileStream.Flush(flushToDisk: true);
			}
			if (File.Exists(filePath))
			{
				File.Delete(filePath);
			}
			File.Move(text, filePath);
		}
		catch
		{
			if (File.Exists(text))
			{
				File.Delete(text);
			}
			throw;
		}
	}
}
