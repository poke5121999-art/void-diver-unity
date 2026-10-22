using GameCommon.Data;
using NemoLib.Mvp;

public class SlotTextPopupModel : Model
{
	public string Message { get; set; }

	public string AdditionalMessage { get; set; }

	public Goods Goods { get; set; }
}
