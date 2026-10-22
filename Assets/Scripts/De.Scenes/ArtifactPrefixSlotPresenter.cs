using NemoLib.Mvp;

public class ArtifactPrefixSlotPresenter : Presenter
{
	[View]
	private ArtifactPrefixSlotView _view;

	[InjectModel]
	private ArtifactPrefixSlotModel _model;

	private void OnInject(Model model)
	{
	}
}
