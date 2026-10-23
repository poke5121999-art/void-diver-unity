using GameCommon.Table;
using System.Text;
using System;
using UnityEngine;

[Serializable]
public class TextMarkerSpawnData
{
	public string TextKey;

	public Vector3 SpawnPosition;

	public TextMarkerSpawnData Clone()
	{
		return null;
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
