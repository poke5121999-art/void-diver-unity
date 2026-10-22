using System.Threading;
using Cysharp.Threading.Tasks;
using GameCommon.Enum;

public interface IStorage
{
	ESaveResult Save(string fileName, byte[] bytes);

	UniTask<ESaveResult> SaveAsync(string fileName, byte[] bytes, CancellationToken cancellationToken = default(CancellationToken));

	void SaveJson(string fileName, string json);

	StorageLoadResult<T> Load<T>(string fileName) where T : class, ISaveData;

	UniTask<StorageLoadResult<T>> LoadAsync<T>(string fileName, CancellationToken cancellationToken = default(CancellationToken)) where T : class, ISaveData;

	UniTask<byte[]> LoadAsync(string fileName, CancellationToken cancellationToken = default(CancellationToken));

	void Delete(string fileName);
}
