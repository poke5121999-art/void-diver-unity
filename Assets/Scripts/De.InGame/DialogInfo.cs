using System;
using GameCommon.Enum;

public class DialogInfo
{
	public string Key { get; set; }

	public EDialogType Type { get; set; }

	public string Title { get; set; }

	public int Priority { get; set; }

	public EDialogPurpose Purpose { get; set; }

	public Action OnClicked { get; set; }
}
