using System.Collections.Generic;

public class MapEditorDefines
{
	public static float GuideAreaWidth = 30f;

	public static float GuideAreaHeight = 30f;

	public static readonly string LayerName_None = "None";

	public static readonly string LayerName_Floor = "Floor";

	public static readonly string LayerName_Wall = "Wall";

	public static readonly string LayerName_Roof = "Roof";

	public static readonly string LayerName_Building = "Building";

	public static readonly string LayerName_Prop = "Prop";

	public static readonly string LayerName_Plant = "Plant";

	public static readonly string LayerName_Light = "Light";

	public static readonly string LayerName_Interactable = "Interactable";

	public static readonly string LayerName_Decal = "Decal";

	public static readonly List<string> LayerNames = new List<string> { LayerName_None, LayerName_Floor, LayerName_Wall, LayerName_Roof, LayerName_Building, LayerName_Prop, LayerName_Plant, LayerName_Light, LayerName_Interactable, LayerName_Decal };

	public static readonly string MapPivotName = "MapPivot";

	public static readonly string HierarchyName_Grid = "Grid";

	public static readonly string HierarchyName_Prop = "Prop";

	public static readonly string[] GridLayerNames = new string[5] { "GridLayer1", "GridLayer2", "GridLayer3", "GridLayer4", "GridLayer5" };

	public static readonly List<LevelInfo> DefaultLevelInfos = new List<LevelInfo>
	{
		new LevelInfo(LayerName_Floor, 0f, 0f, 0f, isVisible: true, isAnActiveLevel: false, isGridActiveLevel: false, isExcludedFromBake: false),
		new LevelInfo(LayerName_Wall, 0f, 0f, 0f, isVisible: true, isAnActiveLevel: true, isGridActiveLevel: false, isExcludedFromBake: false),
		new LevelInfo(LayerName_Roof, 2f, 0f, 0f, isVisible: true, isAnActiveLevel: true, isGridActiveLevel: false, isExcludedFromBake: false),
		new LevelInfo(LayerName_Building, 0f, 0f, 0f, isVisible: true, isAnActiveLevel: true, isGridActiveLevel: false, isExcludedFromBake: false),
		new LevelInfo(LayerName_Prop, 0f, 0f, 0f, isVisible: true, isAnActiveLevel: true, isGridActiveLevel: false, isExcludedFromBake: false),
		new LevelInfo(LayerName_Plant, 0f, 0f, 0f, isVisible: true, isAnActiveLevel: true, isGridActiveLevel: false, isExcludedFromBake: false),
		new LevelInfo(LayerName_Light, 0f, 0f, 0f, isVisible: true, isAnActiveLevel: true, isGridActiveLevel: false, isExcludedFromBake: false),
		new LevelInfo(LayerName_Interactable, 0f, 0f, 0f, isVisible: true, isAnActiveLevel: true, isGridActiveLevel: false, isExcludedFromBake: false),
		new LevelInfo(LayerName_Decal, 0f, 0f, 0f, isVisible: true, isAnActiveLevel: true, isGridActiveLevel: false, isExcludedFromBake: true)
	};

	private static readonly string[] KnownFolderKeywords = new string[17] { "floor", "wall", "roof", "prop", "plant", "decal", "building", "interact", "building", "environment", "light", "dynamic", "tree", "tile", "grid", "depth", "under" };

	public static string GetGridLayerName(PrefabLayer layer)
	{
		if (layer < PrefabLayer.GridLayer1 || (int)layer >= GridLayerNames.Length)
		{
			return GridLayerNames[0];
		}
		return GridLayerNames[(int)layer];
	}

	public static PrefabLayer GetPrefabLayerFromName(string folderName)
	{
		for (int i = 0; i < GridLayerNames.Length; i++)
		{
			if (GridLayerNames[i] == folderName)
			{
				return (PrefabLayer)i;
			}
		}
		return PrefabLayer.GridLayer1;
	}

	public static int GetLevelIndexFromName(string levelName)
	{
		for (int i = 0; i < DefaultLevelInfos.Count; i++)
		{
			if (DefaultLevelInfos[i].name == levelName)
			{
				return i;
			}
		}
		return -1;
	}

	public static string GetLevelNameFromIndex(int index)
	{
		if (index < 0 || index >= DefaultLevelInfos.Count)
		{
			return string.Empty;
		}
		return DefaultLevelInfos[index].name;
	}

	public static bool IsHierarchyFolderFlexible(string name)
	{
		if (string.IsNullOrEmpty(name))
		{
			return false;
		}
		string text = name.ToLowerInvariant();
		if (text.StartsWith("gridlayer"))
		{
			return true;
		}
		for (int i = 0; i < KnownFolderKeywords.Length; i++)
		{
			if (text.Contains(KnownFolderKeywords[i]))
			{
				return true;
			}
		}
		if (text.Length < 2)
		{
			return false;
		}
		if (char.IsDigit(text[0]) && text[1] == 'f')
		{
			return true;
		}
		if (text[0] == 'b' && char.IsDigit(text[1]))
		{
			return true;
		}
		return false;
	}
}
