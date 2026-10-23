using System.Collections.Generic;
using GameCommon.Enum;
using GameCommon.Table;
using UniRx;

public class GameContext : NativeBase
{
	public readonly ReactiveProperty<int> RxCampaignId = new ReactiveProperty<int>();

	public readonly ReactiveProperty<EDifficulty> RxDifficulty = new ReactiveProperty<EDifficulty>(EDifficulty.Normal);

	public readonly ReactiveProperty<EAnomalyType> RxAnomalyType = new ReactiveProperty<EAnomalyType>();

	public readonly ReactiveProperty<bool> RxIsCutscenePreloaded = new ReactiveProperty<bool>(initialValue: true);

	public int[] MapSize;

	public int[] SectorIds;

	public readonly ReactiveProperty<ERoomDifficulty> RxRoomDifficulty = new ReactiveProperty<ERoomDifficulty>();

	public int CampaignId => RxCampaignId.Value;

	public EDifficulty Difficulty => RxDifficulty.Value;

	public EAnomalyType AnomalyType => RxAnomalyType.Value;

	public EThemeType ThemeType => TCampaign.Table[RxCampaignId.Value].ThemeType;

	public int VariantDifficulty
	{
		get
		{
			if (RxCampaignId.Value < 1)
			{
				return 0;
			}
			return TCampaign.Table[RxCampaignId.Value].VariantDifficulty;
		}
	}

	public bool IsTutorial { get; set; }

	public ESceneType CurrentScene { get; set; }

	public bool IsInLounge => CurrentScene == ESceneType.Lounge;

	public bool IsInStage => CurrentScene == ESceneType.Stage;

	public ERoomDifficulty RoomDifficulty => RxRoomDifficulty.Value;

	public ReactiveDictionary<ulong, GamePlayer> RxPlayers { get; } = new ReactiveDictionary<ulong, GamePlayer>();

	public IDictionary<ulong, GamePlayer> Players => RxPlayers;

	public ReactiveDictionary<ulong, ulong> RxPlayerRtt { get; } = new ReactiveDictionary<ulong, ulong>();

	public IDictionary<ulong, ulong> PlayerRtt => RxPlayerRtt;

	public BitFlags AreaUnlockedFlags { get; set; } = new BitFlags();

	public BitFlags AreaDecorationUnlockedFlags { get; set; } = new BitFlags();

	public Dictionary<int, int> ActiveAreaDecorations { get; set; } = new Dictionary<int, int>();

	public Dictionary<int, int> NpcLevels { get; set; } = new Dictionary<int, int>();

	public HashSet<EWeaponType> GetPartyWeaponTypes()
	{
		HashSet<EWeaponType> hashSet = new HashSet<EWeaponType>();
		foreach (GamePlayer value in RxPlayers.Values)
		{
			Character character = value.Character;
			if (character != null && character.Id > 0 && TCharacter.Table.TryGetValue(character.Id, out var row))
			{
				hashSet.Add(row.WeaponType);
			}
		}
		return hashSet;
	}

	public void Clear()
	{
		RxCampaignId.Value = 0;
		RxAnomalyType.Value = EAnomalyType.None;
		RxIsCutscenePreloaded.Value = false;
		IsTutorial = false;
		CurrentScene = ESceneType.None;
		RxPlayers.Clear();
		RxPlayerRtt.Clear();
		MapSize = null;
	}
}
