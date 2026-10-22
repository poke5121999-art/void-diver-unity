using System.Threading;
using Cysharp.Threading.Tasks;

public interface IBackupStorage
{
	void Save(string fileName, byte[] bytes, long ticks);

	UniTask SaveAsync(string fileName, byte[] bytes, long ticks, CancellationToken cancellationToken = default(CancellationToken));

	T Load<T>(string fileName, long ticks) where T : class, ISaveData;

	UniTask<T> LoadAsync<T>(string fileName, long ticks, CancellationToken cancellationToken = default(CancellationToken)) where T : class, ISaveData;

	long[] GetBackupList();

	void Delete(long ticks);
}
