using System;
using System.Collections.Generic;

[Serializable]
public class WavePhase
{
	public List<WaveSpawnData> WaveSpawnDatas = new List<WaveSpawnData>();

	public float SpawnMinRange;

	public float SpawnMaxRange;

	public float StartDelay;
}
