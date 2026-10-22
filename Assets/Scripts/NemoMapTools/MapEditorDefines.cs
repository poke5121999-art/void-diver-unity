using System.Collections.Generic;

public class MapEditorDefines
{
	public static float GuideAreaWidth;

	public static float GuideAreaHeight;

	public static readonly string LayerName_None;

	public static readonly string LayerName_Floor;

	public static readonly string LayerName_Wall;

	public static readonly string LayerName_Roof;

	public static readonly string LayerName_Building;

	public static readonly string LayerName_Prop;

	public static readonly string LayerName_Plant;

	public static readonly string LayerName_Light;

	public static readonly string LayerName_Interactable;

	public static readonly string LayerName_Decal;

	public static readonly List<string> LayerNames;

	public static readonly string MapPivotName;

	public static readonly string HierarchyName_Grid;

	public static readonly string HierarchyName_Prop;

	public static readonly string[] GridLayerNames;

	public static readonly List<LevelInfo> DefaultLevelInfos;

	private static readonly string[] KnownFolderKeywords;

	public static string GetGridLayerName(PrefabLayer layer)
	{
		return null;
	}

	public static PrefabLayer GetPrefabLayerFromName(string folderName)
	{
		return PrefabLayer.GridLayer1;
	}

	public static int GetLevelIndexFromName(string levelName)
	{
		return 0;
	}

	public static string GetLevelNameFromIndex(int index)
	{
		return null;
	}

	public static bool IsHierarchyFolderFlexible(string name)
	{
		return false;
	}
}
