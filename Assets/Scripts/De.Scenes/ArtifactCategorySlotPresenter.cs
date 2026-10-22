using NemoLib.Mvp;

public class ArtifactCategorySlotPresenter : Presenter
{
	[View]
	private ArtifactCategorySlotView _view;

	[InjectModel]
	private ArtifactCategorySlotModel _model;

	private void OnInject(Model model)
	{
		_view.Normal_Text.text = _model.Category.ToLocalText();
	}
}
