using CodeStage.AntiCheat.ObscuredTypes;
using ProtoBuf;
using Tracker;

[ProtoContract]
public class Profile : ITrackableData<Profile>
{
	private ObscuredLong _obscuredGold;

	private ObscuredLong _obscuredExp;

	private ObscuredInt _obscuredLevel;

	private ObscuredLong _obscuredCoin;

	[ProtoMember(1)]
	public EDataOperation Operation { get; set; }

	[ProtoMember(2)]
	public string Name { get; set; }

	[ProtoMember(3)]
	public int SelectedCharacterId { get; set; }

	[ProtoMember(4)]
	public int SelectedPetId { get; set; }

	[ProtoMember(5)]
	public long Gold
	{
		get
		{
			return 0L;
		}
		set
		{
		}
	}

	[ProtoMember(6)]
	public bool IsCurrentlyInStage { get; set; }

	[ProtoMember(7)]
	public long Exp
	{
		get
		{
			return 0L;
		}
		set
		{
		}
	}

	[ProtoMember(8)]
	public int Level
	{
		get
		{
			return 0;
		}
		set
		{
		}
	}

	[ProtoMember(9)]
	public int RestCount { get; set; }

	[ProtoMember(10)]
	public long Coin
	{
		get
		{
			return 0L;
		}
		set
		{
		}
	}

	[ProtoMember(11)]
	public int BgmId { get; set; }

	[ProtoMember(12)]
	public int LostItemRerollCount { get; set; }

	public Profile Clone()
	{
		return null;
	}
}
