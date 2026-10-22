using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShaderPointRegistry", menuName = "LCArt/Shader Point Registry")]
public class ShaderPointRegistry : ScriptableObject
{
	[Serializable]
	public class CategoryDefinition
	{
		public string name;

		public int maxCount;

		[HideInInspector]
		public string previousName;
	}

	private const int _defaultMaxCount = 8;

	[SerializeField]
	private List<CategoryDefinition> _categoryDefinitions;

	public IReadOnlyList<CategoryDefinition> CategoryDefinitions => null;
}
