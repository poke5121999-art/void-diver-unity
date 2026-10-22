using System;
using System.Collections.Generic;

[Serializable]
public class WavePhase
{
	public List<WaveSpawnData> WaveSpawnDatas;

	public float SpawnMinRange;

	public float SpawnMaxRange;

	public float StartDelay;
}
