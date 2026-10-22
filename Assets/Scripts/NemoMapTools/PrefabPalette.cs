using System;
using System.Collections.Generic;
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
		public string categoryName;

		[Tooltip("미리 정해진 레벨 레이어 설정")]
		public string defaultLevel;

		[Tooltip("이 카테고리에 속하는 프리팹 목록")]
		public List<PrefabEntry> prefabEntries;

		[Tooltip("이 카테고리에 대한 선택적 설명")]
		[TextArea(1, 3)]
		public string description;

		[HideInInspector]
		[SerializeField]
		internal List<GameObject> prefabs;
	}

	[Serializable]
	public class SingleTileSet
	{
		[Tooltip("에디터 UI에서 이 싱글 타일 세트의 이름")]
		public string setName;

		[Tooltip("이 타일 세트에 대한 선택적 설명")]
		[TextArea(1, 3)]
		public string description;

		[Tooltip("미리 정해진 레벨 레이어 설정")]
		public string defaultLevel;

		[Tooltip("해당 타일의 단위 크기를 설정합니다.")]
		public float tileUnit;

		[Tooltip("배치 시 적용되는 월드 오프셋 (X, Z만 사용, Y는 무시됨)")]
		public Vector3 worldOffset;

		[Tooltip("타일 바리에이션 목록 (1~4개). 그리드 위치 (x+z)%count 로 선택됩니다.")]
		public List<GameObject> variations;

		[Tooltip("동일한 세트/태그의 다른 타일로 완전히 둘러싸인 경우 사용되는 프리팹(북쪽, 남쪽, 동쪽, 서쪽 연결이 존재).")]
		public GameObject fill;

		public GameObject GetVariation(int gridX, int gridZ)
		{
			return null;
		}

		public bool IsValid()
		{
			return false;
		}

		public GameObject GetPreviewPrefab()
		{
			return null;
		}

		public GameObject GetFillPrefab(int gridX, int gridZ)
		{
			return null;
		}

		public bool HasValidFill()
		{
			return false;
		}
	}

	[Serializable]
	public class RuleTileSet
	{
		[Tooltip("에디터 UI에서 이 규칙 타일 세트의 이름")]
		public string ruleSetName;

		[Tooltip("이 규칙 세트에 대한 선택적 설명")]
		[TextArea(1, 3)]
		public string description;

		[Tooltip("미리 정해진 레벨 레이어 설정")]
		public string defaultLevel;

		[Tooltip("동일한 태그를 가진 RuleTileSet은 서로를 이웃으로 인식합니다. 기본 동작(자기 자신과만 연결)을 위해 비워두세요.")]
		public string compatibilityTag;

		[Tooltip("해당 타일의 단위 크기를 설정합니다.")]
		public float ruleTileUnit;

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
			return false;
		}

		public GameObject GetPreviewPrefab()
		{
			return null;
		}
	}

	[Header("Prefab Categories")]
	[Tooltip("일반 프리팹을 구성하기 위한 카테고리 목록")]
	public List<PrefabCategory> categories;

	[Header("Single Tile Set Settings")]
	[Tooltip("독립적으로 배치되는 싱글 타일 세트 목록")]
	public List<SingleTileSet> singleTileSets;

	[Header("Rule Tile Set Settings")]
	[Tooltip("규칙 기반 배치를 위해 정의된 규칙 타일 세트 목록")]
	public List<RuleTileSet> ruleTileSets;

	private void OnValidate()
	{
	}

	private void ValidateCategories()
	{
	}

	private void ValidateSingleTileSets()
	{
	}

	private void ValidateRuleTileSets()
	{
	}

	public PrefabCategory AddCategory(string name = "New Category")
	{
		return null;
	}

	public bool RemoveCategory(string categoryName)
	{
		return false;
	}

	public PrefabCategory FindCategory(string categoryName)
	{
		return null;
	}

	public bool AddPrefabToCategory(string categoryName, GameObject prefab)
	{
		return false;
	}

	public SingleTileSet AddSingleTileSet(string name = "New Single Tile")
	{
		return null;
	}

	public bool RemoveSingleTileSet(string setName)
	{
		return false;
	}

	public SingleTileSet FindSingleTileSet(string setName)
	{
		return null;
	}

	public RuleTileSet AddRuleTileSet(string name = "New Rule Tile")
	{
		return null;
	}

	public bool RemoveRuleTileSet(string ruleSetName)
	{
		return false;
	}

	public RuleTileSet FindRuleTileSet(string ruleSetName)
	{
		return null;
	}

	public GameObject FindPrefabInCategory(string prefabName)
	{
		return null;
	}

	public bool ContainsPrefab(GameObject prefabAsset)
	{
		return false;
	}
}
