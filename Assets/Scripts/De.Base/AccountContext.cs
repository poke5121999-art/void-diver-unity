using System.Linq;

public class AccountContext
{
	public string AccountId { get; set; }

	public long SaveTimeTicks { get; set; }

	public long CreateTimeTicks { get; set; }

	public long PlayTimeTicks { get; set; }

	public int PlayerSlotIndex { get; set; }

	public PlayerSlot[] PlayerSlots { get; set; }

	public AccountContext(AccountContextData data)
	{
		AccountId = data.AccountId;
		SaveTimeTicks = data.SaveTimeTicks;
		CreateTimeTicks = data.CreateTimeTicks;
		PlayTimeTicks = data.PlayTimeTicks;
		PlayerSlotIndex = data.LastPlayerSlotIndex;
		PlayerSlots = data.PlayerSlots.Select((PlayerSlot s) => s.Clone()).ToArray();
	}

	public AccountContextData ToData()
	{
		return new AccountContextData
		{
			AccountId = AccountId,
			SaveTimeTicks = SaveTimeTicks,
			CreateTimeTicks = CreateTimeTicks,
			PlayTimeTicks = PlayTimeTicks,
			LastPlayerSlotIndex = PlayerSlotIndex,
			PlayerSlots = PlayerSlots.Select((PlayerSlot s) => s.Clone()).ToArray()
		};
	}
}
