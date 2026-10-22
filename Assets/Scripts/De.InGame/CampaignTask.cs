public class CampaignTask
{
	private const string COLOR_DONE = "#eeeeee";

	private const string COLOR_NOT_DONE = "#b5b5b5";

	public int Id { get; set; }

	public long PermanentProgress { get; set; }

	public long TemporaryProgress { get; set; }

	public long Goal { get; set; }

	public long Progress => 0L;

	public bool IsCompleted => false;

	public bool IsPermanentlyCompleted => false;

	public string ToLocalString()
	{
		return null;
	}
}
