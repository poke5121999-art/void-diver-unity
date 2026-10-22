using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "PrefabPalette", menuName = "Map Editor/Prefab Palette")]
public class PrefabPalette : ScriptableObject
{
	[Serializable]
	public class PrefabEntry
	{
		public GameObject prefab;

		public GameObject merge2x2;

		public GameObject merge4x4;

		public GameObject merge8x8;
	}

	[Serializable]
	public class PrefabCategory
	{
		[Tooltip("에디터에 표시되는 카테고리 이름")]
		public string categoryName = "New Category";

		[Tooltip("미리 정해진 레벨 레이어 설정")]
		public string defaultLevel = MapEditorDefines.LayerName_Floor;

		[Tooltip("이 카테고리에 속하는 프리팹 목록")]
		public List<PrefabEntry> prefabEntries = new List<PrefabEntry>();

		[Tooltip("이 카테고리에 대한 선택적 설명")]
		[TextArea(1, 3)]
		public string description;

		[HideInInspector]
		[SerializeField]
		internal List<GameObject> prefabs = new List<GameObject>();
	}

	[Serializable]
	public class SingleTileSet
	{
		[Tooltip("에디터 UI에서 이 싱글 타일 세트의 이름")]
		public string setName = "New Single Tile";

		[Tooltip("이 타일 세트에 대한 선택적 설명")]
		[TextArea(1, 3)]
		public string description;

		[Tooltip("미리 정해진 레벨 레이어 설정")]
		public string defaultLevel = MapEditorDefines.LayerName_Floor;

		[Tooltip("해당 타일의 단위 크기를 설정합니다.")]
		public float tileUnit = 0.5f;

		[Tooltip("배치 시 적용되는 월드 오프셋 (X, Z만 사용, Y는 무시됨)")]
		public Vector3 worldOffset = new Vector3(0.25f, 0f, 0.25f);

		[Tooltip("타일 바리에이션 목록 (1~4개). 그리드 위치 (x+z)%count 로 선택됩니다.")]
		public List<GameObject> variations = new List<GameObject>();

		[Tooltip("동일한 세트/태그의 다른 타일로 완전히 둘러싸인 경우 사용되는 프리팹(북쪽, 남쪽, 동쪽, 서쪽 연결이 존재).")]
		public GameObject fill;

		public GameObject GetVariation(int gridX, int gridZ)
		{
			if (variations == null || variations.Count == 0)
			{
				return null;
			}
			List<GameObject> list = variations.Where((GameObject v) => v != null).ToList();
			if (list.Count == 0)
			{
				return null;
			}
			return list[Math.Abs(gridX + gridZ) % list.Count];
		}

		public bool IsValid()
		{
			if (variations == null)
			{
				return false;
			}
			return variations.Any((GameObject v) => v != null);
		}

		public GameObject GetPreviewPrefab()
		{
			if (variations == null)
			{
				return null;
			}
			return variations.FirstOrDefault((GameObject v) => v != null);
		}

		public GameObject GetFillPrefab(int gridX, int gridZ)
		{
			if (fill != null)
			{
				return fill;
			}
			if (variations == null || variations.Count == 0)
			{
				return null;
			}
			List<GameObject> list = variations.Where((GameObject v) => v != null).ToList();
			if (list.Count == 0)
			{
				return null;
			}
			return list[Math.Abs(gridX + gridZ) % list.Count];
		}

		public bool HasValidFill()
		{
			return fill != null;
		}
	}

	[Serializable]
	public class RuleTileSet
	{
		[Tooltip("에디터 UI에서 이 규칙 타일 세트의 이름")]
		public string ruleSetName = "New Rule Tile";

		[Tooltip("이 규칙 세트에 대한 선택적 설명")]
		[TextArea(1, 3)]
		public string description;

		[Tooltip("미리 정해진 레벨 레이어 설정")]
		public string defaultLevel = MapEditorDefines.LayerName_Floor;

		[Tooltip("동일한 태그를 가진 RuleTileSet은 서로를 이웃으로 인식합니다. 기본 동작(자기 자신과만 연결)을 위해 비워두세요.")]
		public string compatibilityTag = "";

		[Tooltip("해당 타일의 단위 크기를 설정합니다.")]
		public float ruleTileUnit = 1f;

		public GameObject straightBottom;

		public GameObject straightLeft;

		public GameObject straightRight;

		public GameObject straightTop;

		public GameObject concaveBot;

		public GameObject concaveLeft;

		public GameObject concaveRight;

		public GameObject concaveTop;

		public GameObject convexBot;

		public GameObject convexLeft;

		public GameObject convexRight;

		public GameObject convexTop;

		public GameObject fill;

		public bool IsValid()
		{
			return new GameObject[13] { straightTop, straightBottom, straightLeft, straightRight, concaveLeft, concaveTop, concaveBot, concaveRight, convexBot, convexRight, convexLeft, convexTop, fill }.Any((GameObject tile) => tile != null);
		}

		public GameObject GetPreviewPrefab()
		{
			if (straightLeft != null)
			{
				return straightLeft;
			}
			if (fill != null)
			{
				return fill;
			}
			return new GameObject[11] { straightTop, straightBottom, straightRight, concaveLeft, concaveTop, concaveBot, concaveRight, convexBot, convexRight, convexLeft, convexTop }.FirstOrDefault((GameObject tile) => tile != null);
		}
	}

	[Header("Prefab Categories")]
	[Tooltip("일반 프리팹을 구성하기 위한 카테고리 목록")]
	public List<PrefabCategory> categories = new List<PrefabCategory>();

	[Header("Single Tile Set Settings")]
	[Tooltip("독립적으로 배치되는 싱글 타일 세트 목록")]
	public List<SingleTileSet> singleTileSets = new List<SingleTileSet>();

	[Header("Rule Tile Set Settings")]
	[Tooltip("규칙 기반 배치를 위해 정의된 규칙 타일 세트 목록")]
	public List<RuleTileSet> ruleTileSets = new List<RuleTileSet>();

	private void OnValidate()
	{
		ValidateCategories();
		ValidateSingleTileSets();
		ValidateRuleTileSets();
	}

	private void ValidateCategories()
	{
		HashSet<string> hashSet = new HashSet<string>();
		if (categories == null)
		{
			categories = new List<PrefabCategory>();
		}
		for (int num = categories.Count - 1; num >= 0; num--)
		{
			if (categories[num] == null)
			{
				categories.RemoveAt(num);
				continue;
			}
			string text = (string.IsNullOrWhiteSpace(categories[num].categoryName) ? "Unnamed Category" : categories[num].categoryName);
			string text2 = text;
			int num2 = 1;
			while (hashSet.Contains(text2))
			{
				text2 = string.Format("{0} ({1})", text, num2);
				num2++;
			}
			categories[num].categoryName = text2;
			hashSet.Add(text2);
			if (categories[num].prefabs != null && categories[num].prefabs.Count > 0)
			{
				if (categories[num].prefabEntries == null)
				{
					categories[num].prefabEntries = new List<PrefabEntry>();
				}
				foreach (GameObject prefab in categories[num].prefabs)
				{
					GameObject go = prefab;
					if (go != null && !categories[num].prefabEntries.Any((PrefabEntry e) => e.prefab == go))
					{
						categories[num].prefabEntries.Add(new PrefabEntry { prefab = go });
					}
				}
				categories[num].prefabs.Clear();
			}
			if (categories[num].prefabEntries == null)
			{
				categories[num].prefabEntries = new List<PrefabEntry>();
			}
		}
	}

	private void ValidateSingleTileSets()
	{
		HashSet<string> hashSet = new HashSet<string>();
		if (singleTileSets == null)
		{
			singleTileSets = new List<SingleTileSet>();
		}
		for (int num = singleTileSets.Count - 1; num >= 0; num--)
		{
			if (singleTileSets[num] == null)
			{
				singleTileSets.RemoveAt(num);
				continue;
			}
			string text = (string.IsNullOrWhiteSpace(singleTileSets[num].setName) ? "Unnamed SingleSet" : singleTileSets[num].setName);
			string text2 = text;
			int num2 = 1;
			while (hashSet.Contains(text2))
			{
				text2 = string.Format("{0} ({1})", text, num2);
				num2++;
			}
			singleTileSets[num].setName = text2;
			hashSet.Add(text2);
			if (singleTileSets[num].variations == null)
			{
				singleTileSets[num].variations = new List<GameObject>();
			}
		}
	}

	private void ValidateRuleTileSets()
	{
		HashSet<string> hashSet = new HashSet<string>();
		if (ruleTileSets == null)
		{
			ruleTileSets = new List<RuleTileSet>();
		}
		for (int num = ruleTileSets.Count - 1; num >= 0; num--)
		{
			if (ruleTileSets[num] == null)
			{
				ruleTileSets.RemoveAt(num);
				continue;
			}
			string text = (string.IsNullOrWhiteSpace(ruleTileSets[num].ruleSetName) ? "Unnamed RuleSet" : ruleTileSets[num].ruleSetName);
			string text2 = text;
			int num2 = 1;
			while (hashSet.Contains(text2))
			{
				text2 = string.Format("{0} ({1})", text, num2);
				num2++;
			}
			ruleTileSets[num].ruleSetName = text2;
			hashSet.Add(text2);
		}
	}

	public PrefabCategory AddCategory(string name = "New Category")
	{
		if (categories == null)
		{
			categories = new List<PrefabCategory>();
		}
		PrefabCategory category = new PrefabCategory
		{
			categoryName = name,
			prefabEntries = new List<PrefabEntry>()
		};
		categories.Add(category);
		ValidateCategories();
		ValidateSingleTileSets();
		ValidateRuleTileSets();
		return categories.Find((PrefabCategory c) => c == category);
	}

	public bool RemoveCategory(string categoryName)
	{
		if (categories == null)
		{
			return false;
		}
		int num = categories.FindIndex((PrefabCategory c) => c.categoryName == categoryName);
		if (num == -1)
		{
			return false;
		}
		categories.RemoveAt(num);
		return true;
	}

	public PrefabCategory FindCategory(string categoryName)
	{
		if (categories == null)
		{
			return null;
		}
		return categories.Find((PrefabCategory c) => c.categoryName == categoryName);
	}

	public bool AddPrefabToCategory(string categoryName, GameObject prefab)
	{
		PrefabCategory prefabCategory = FindCategory(categoryName);
		if (prefabCategory == null)
		{
			return false;
		}
		if (prefabCategory.prefabEntries == null)
		{
			prefabCategory.prefabEntries = new List<PrefabEntry>();
		}
		if (prefabCategory.prefabEntries.Any((PrefabEntry e) => e.prefab == prefab))
		{
			return false;
		}
		prefabCategory.prefabEntries.Add(new PrefabEntry { prefab = prefab });
		return true;
	}

	public SingleTileSet AddSingleTileSet(string name = "New Single Tile")
	{
		if (singleTileSets == null)
		{
			singleTileSets = new List<SingleTileSet>();
		}
		SingleTileSet set = new SingleTileSet
		{
			setName = name,
			variations = new List<GameObject>()
		};
		singleTileSets.Add(set);
		ValidateCategories();
		ValidateSingleTileSets();
		ValidateRuleTileSets();
		return singleTileSets.Find((SingleTileSet s) => s == set);
	}

	public bool RemoveSingleTileSet(string setName)
	{
		if (singleTileSets == null)
		{
			return false;
		}
		int num = singleTileSets.FindIndex((SingleTileSet s) => s.setName == setName);
		if (num == -1)
		{
			return false;
		}
		singleTileSets.RemoveAt(num);
		return true;
	}

	public SingleTileSet FindSingleTileSet(string setName)
	{
		if (singleTileSets == null)
		{
			return null;
		}
		return singleTileSets.Find((SingleTileSet s) => s.setName == setName);
	}

	public RuleTileSet AddRuleTileSet(string name = "New Rule Tile")
	{
		if (ruleTileSets == null)
		{
			ruleTileSets = new List<RuleTileSet>();
		}
		RuleTileSet ruleSet = new RuleTileSet { ruleSetName = name };
		ruleTileSets.Add(ruleSet);
		ValidateCategories();
		ValidateSingleTileSets();
		ValidateRuleTileSets();
		return ruleTileSets.Find((RuleTileSet rs) => rs == ruleSet);
	}

	public bool RemoveRuleTileSet(string ruleSetName)
	{
		if (ruleTileSets == null)
		{
			return false;
		}
		int num = ruleTileSets.FindIndex((RuleTileSet rs) => rs.ruleSetName == ruleSetName);
		if (num == -1)
		{
			return false;
		}
		ruleTileSets.RemoveAt(num);
		return true;
	}

	public RuleTileSet FindRuleTileSet(string ruleSetName)
	{
		if (ruleTileSets == null)
		{
			return null;
		}
		return ruleTileSets.Find((RuleTileSet rs) => rs.ruleSetName == ruleSetName);
	}

	public GameObject FindPrefabInCategory(string prefabName)
	{
		if (categories == null)
		{
			return null;
		}
		foreach (PrefabCategory category in categories)
		{
			if (category == null || category.prefabEntries == null)
			{
				continue;
			}
			foreach (PrefabEntry prefabEntry in category.prefabEntries)
			{
				if (prefabEntry?.prefab != null && prefabEntry.prefab.name == prefabName)
				{
					return prefabEntry.prefab;
				}
			}
		}
		return null;
	}

	public bool ContainsPrefab(GameObject prefabAsset)
	{
		if (prefabAsset == null)
		{
			return false;
		}
		if (categories == null)
		{
			return false;
		}
		foreach (PrefabCategory category in categories)
		{
			if (category != null && category.prefabEntries != null && category.prefabEntries.Any((PrefabEntry e) => e.prefab == prefabAsset))
			{
				return true;
			}
		}
		return false;
	}
}
