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
	}

	public AccountContextData ToData()
	{
		return null;
	}
}
