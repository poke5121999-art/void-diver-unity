using System.Collections.Generic;
using GameCommon.Enum;
using UniRx;

public class GameContext : NativeBase
{
	public readonly ReactiveProperty<int> RxCampaignId;

	public readonly ReactiveProperty<EDifficulty> RxDifficulty;

	public readonly ReactiveProperty<EAnomalyType> RxAnomalyType;

	public readonly ReactiveProperty<bool> RxIsCutscenePreloaded;

	public int[] MapSize;

	public int[] SectorIds;

	public readonly ReactiveProperty<ERoomDifficulty> RxRoomDifficulty;

	public int CampaignId => 0;

	public EDifficulty Difficulty => EDifficulty.Easy;

	public EAnomalyType AnomalyType => EAnomalyType.None;

	public EThemeType ThemeType => EThemeType.None;

	public int VariantDifficulty => 0;

	public bool IsTutorial { get; set; }

	public ESceneType CurrentScene { get; set; }

	public bool IsInLounge => false;

	public bool IsInStage => false;

	public ERoomDifficulty RoomDifficulty => ERoomDifficulty.Story;

	public ReactiveDictionary<ulong, GamePlayer> RxPlayers { get; }

	public IDictionary<ulong, GamePlayer> Players => null;

	public ReactiveDictionary<ulong, ulong> RxPlayerRtt { get; }

	public IDictionary<ulong, ulong> PlayerRtt => null;

	public BitFlags AreaUnlockedFlags { get; set; }

	public BitFlags AreaDecorationUnlockedFlags { get; set; }

	public Dictionary<int, int> ActiveAreaDecorations { get; set; }

	public Dictionary<int, int> NpcLevels { get; set; }

	public HashSet<EWeaponType> GetPartyWeaponTypes()
	{
		return null;
	}

	public void Clear()
	{
	}
}
