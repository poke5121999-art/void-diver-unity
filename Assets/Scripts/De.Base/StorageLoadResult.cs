using GameCommon.Enum;

public class StorageLoadResult<T> where T : ISaveData
{
	public ELoadResult LoadResult { get; private set; }

	public T Data { get; private set; }

	public StorageLoadResult(ELoadResult loadResult, T data = default(T))
	{
	}
}
