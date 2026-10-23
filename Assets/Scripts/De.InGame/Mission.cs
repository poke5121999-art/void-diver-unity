using GameCommon.Enum;
using GameCommon.Table;
using System;

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

	public bool IsCompleted => Progress >= Goal;

	public bool IsStarted
	{
		get
		{
			if (State != EMissionState.None)
			{
				return State != EMissionState.NotStarted;
			}
			return false;
		}
	}

	public string ToLocalString()
	{
		string name = TMission.Table[Id].Name;
		if (Progress >= Goal)
		{
			return string.Format("{0}> ({1}/{2})", name, Progress, Goal).WithColor("#eeeeee");
		}
		return (name ?? string.Empty).WithColor("#b5b5b5") + string.Format(" ({0}", Progress).WithColor("#eeeeee") + string.Format("/{0})", Goal).WithColor("#b5b5b5");
	}
}
