using System.Collections.Generic;

public struct TagDiff
{
	public IReadOnlyList<int> AddedPrefer;

	public IReadOnlyList<int> RemovedPrefer;

	public IReadOnlyList<int> AddedDislike;

	public IReadOnlyList<int> RemovedDislike;

	public bool HasAny => AddedPrefer.Count + RemovedPrefer.Count + AddedDislike.Count + RemovedDislike.Count > 0;
}
