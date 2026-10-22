using NemoLib.Mvp;

public class HelpPopupPageSlotPresenter : Presenter
{
	[View]
	private HelpPopupPageSlotView _view;

	[InjectModel]
	private HelpPopupPageSlotModel _model;

	private void OnInject(Model model)
	{
	}

	public void SetSelected(bool isActive)
	{
	}
}
