using NemoLib.Mvp;

public class HelpPopupPageSlotPresenter : Presenter
{
	[View]
	private HelpPopupPageSlotView _view;

	[InjectModel]
	private HelpPopupPageSlotModel _model;

	private void OnInject(Model model)
	{
		_view.Text.text = _model.PageNumber.ToString("00");
	}

	public void SetSelected(bool isActive)
	{
		_view.Text.gameObject.SetActive(isActive);
		_view.Icon.gameObject.SetActive(!isActive);
	}
}
