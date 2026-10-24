using GameCommon.Table;
using System.Text;
using System;
using UnityEngine;

[Serializable]
public class TextMarkerSpawnData
{
	public string TextKey;

	public Vector3 SpawnPosition;

	// @0x180203BF0: new TextMarkerSpawnData, copies TextKey and SpawnPosition.
	public TextMarkerSpawnData Clone()
	{
		return new TextMarkerSpawnData
		{
			TextKey = TextKey,
			SpawnPosition = SpawnPosition
		};
	}

	public bool Validate(out string errorMessage)
	{
		StringBuilder stringBuilder = new StringBuilder();
		bool result = true;
		if (!TLocalizedText.Table.ContainsKey(TextKey))
		{
			result = false;
			stringBuilder.AppendLine("[TLocalizedText.Id] not contains \"[TextKey] " + TextKey + "\"");
		}
		errorMessage = stringBuilder.ToString();
		return result;
	}
}
