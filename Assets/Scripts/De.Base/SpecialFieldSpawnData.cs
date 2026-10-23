using GameCommon.Table;
using System.Text;
using System;
using UnityEngine;

[Serializable]
public class SpecialFieldSpawnData
{
	public int SpecialFieldId;

	public Vector3 SpawnPosition;

	public Vector3 Scale;

	public SpecialFieldSpawnData Clone(Vector3 positionOffset = default(Vector3))
	{
		return new SpecialFieldSpawnData
		{
			SpecialFieldId = SpecialFieldId,
			SpawnPosition = positionOffset + SpawnPosition,
			Scale = Scale
		};
	}

	public bool Validate(out string errorMessage)
	{
		errorMessage = null;
		StringBuilder stringBuilder = new StringBuilder();
		bool result = true;
		if (!TSpecialField.Table.ContainsKey(SpecialFieldId))
		{
			result = false;
			stringBuilder.AppendLine(string.Format("SpecialFieldId {0} not found in TSpecialField", SpecialFieldId));
		}
		errorMessage = stringBuilder.ToString();
		return result;
	}
}
