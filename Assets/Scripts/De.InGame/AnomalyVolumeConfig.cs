using System;
using GameCommon.Enum;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "AnomalyVolumeConfig", menuName = "Game/Anomaly Volume Config")]
public class AnomalyVolumeConfig : ScriptableObject
{
	[Serializable]
	public struct Entry
	{
		public EAnomalyType Type;

		public VolumeProfile Profile;
	}

	[SerializeField]
	private Entry[] _entries;

	public VolumeProfile GetProfile(EAnomalyType type)
	{
		return null;
	}
}
