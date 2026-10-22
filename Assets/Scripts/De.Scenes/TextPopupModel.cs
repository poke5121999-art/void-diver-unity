using NemoLib.Mvp;

public class TextPopupModel : Model
{
	public CommonPopupType CommonPopupType { get; set; }

	public string Message { get; set; }

	public string AdditionalMessage { get; set; }
}
