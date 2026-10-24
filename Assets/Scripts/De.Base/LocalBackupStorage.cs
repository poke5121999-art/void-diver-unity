using System;
using System.IO;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using NemoLib.Log;
using UnityEngine;
using VContainer;

public class LocalBackupStorage : NativeBase, IBackupStorage
{
	[Inject]
	private SeriLogger _logger;

	private readonly string _saveDir;

	public LocalBackupStorage()
	{
		_saveDir = Path.Combine(Directory.GetParent(Application.dataPath).FullName, "SavesDir/Backup");
		Directory.CreateDirectory(_saveDir);
	}

	private string ToTicksPath(string fileName, long ticks)
	{
		string path = Path.Combine(_saveDir, ticks.ToString());
		Directory.CreateDirectory(path);
		return Path.Combine(path, fileName + ".bak");
	}

	void IBackupStorage.Save(string fileName, byte[] bytes, long ticks)
	{
		WriteFileAtomic(ToTicksPath(fileName, ticks), bytes);
		_logger.Debug("Backup save written: " + fileName);
	}

	// SaveAsync body @0x180204600 (machine code checked): the "written" line is logged before the thread-pool write is awaited
	async UniTask IBackupStorage.SaveAsync(string fileName, byte[] bytes, long ticks, CancellationToken cancellationToken)
	{
		try
		{
			_logger.Debug("Backup save written: " + fileName);
			await UniTask.RunOnThreadPool(delegate
			{
				WriteFileAtomic(ToTicksPath(fileName, ticks), bytes);
			}, configureAwait: true, cancellationToken);
		}
		catch (Exception arg)
		{
			_logger.Error(string.Format("Backup save failed [{0}]: {1}", fileName, arg));
		}
	}

	long[] IBackupStorage.GetBackupList()
	{
		if (!Directory.Exists(_saveDir))
		{
			return Array.Empty<long>();
		}
		return (from name in Directory.GetDirectories(_saveDir).Select(Path.GetFileName)
			where long.TryParse(name, out var _)
			select long.Parse(name) into ticks
			orderby ticks descending
			select ticks).ToArray();
	}

	// LocalBackupStorage.IBackupStorage.Load<object> @0x180c9c480 (machine code checked)
	T IBackupStorage.Load<T>(string fileName, long ticks)
	{
		string text = ToTicksPath(fileName, ticks);
		if (!File.Exists(text))
		{
			return null;
		}
		try
		{
			T result = SerializeHelper.Deserialize<T>(File.ReadAllBytes(text));
			_logger.Debug("Backup load success: " + text);
			return result;
		}
		catch (Exception e)
		{
			_logger.Error(BuildLoadError(text, e));
			return null;
		}
	}

	// <IBackupStorage-LoadAsync>d__8<object>.MoveNext @0x1827bfe10 (machine code checked): the success line is logged before the thread-pool read is awaited
	async UniTask<T> IBackupStorage.LoadAsync<T>(string fileName, long ticks, CancellationToken cancellationToken)
	{
		string filePath = ToTicksPath(fileName, ticks);
		if (!File.Exists(filePath))
		{
			return null;
		}
		try
		{
			_logger.Debug("Backup load success: " + filePath);
			return await UniTask.RunOnThreadPool(() => SerializeHelper.Deserialize<T>(File.ReadAllBytes(filePath)), configureAwait: true, cancellationToken);
		}
		catch (Exception e)
		{
			_logger.Error(BuildLoadError(filePath, e));
			return null;
		}
	}

	private string BuildLoadError(string filePath, Exception e)
	{
		string text = ArchiveFile(filePath);
		return string.Format("Backup load failed [{0}]: {1}", filePath, e) + ((text == null) ? string.Empty : ("\n" + text));
	}

	void IBackupStorage.Delete(long ticks)
	{
		string path = Path.Combine(_saveDir, ticks.ToString());
		if (Directory.Exists(path))
		{
			Directory.Delete(path, recursive: true);
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

	private string ArchiveFile(string filePath)
	{
		if (!File.Exists(filePath))
		{
			return null;
		}
		try
		{
			string directoryName = Path.GetDirectoryName(filePath);
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
			string extension = Path.GetExtension(filePath);
			string text = Path.Combine(directoryName, fileNameWithoutExtension + "_" + DateTime.UtcNow.ToString("yyyyMMdd_HHmmss") + extension);
			File.Move(filePath, text);
			return text;
		}
		catch (Exception ex)
		{
			_logger.Warning("Failed to archive corrupted backup file [" + filePath + "]: " + ex.Message);
			return null;
		}
	}
}
