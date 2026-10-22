using GameCommon.Enum;

public class Mission
{
	private const string COLOR_DONE = "#eeeeee";

	private const string COLOR_NOT_DONE = "#b5b5b5";

	public int Id { get; set; }

	public long Progress { get; set; }

	public long Goal { get; set; }

	public int Step { get; set; }

	public EMissionState State { get; set; }

	public long CurrentDialogReplayId { get; set; }

	public bool IsCompleted => false;

	public bool IsStarted => false;

	public string ToLocalString()
	{
		return null;
	}
}
