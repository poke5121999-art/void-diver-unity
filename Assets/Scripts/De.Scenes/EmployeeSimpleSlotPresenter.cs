using NemoLib.AssetManagement;
using NemoLib.Mvp;
using VContainer;

public class EmployeeSimpleSlotPresenter : Presenter
{
	[View]
	private EmployeeSimpleSlotView _view;

	[InjectModel]
	private EmployeeSimpleSlotModel _model;

	[Inject]
	private AssetManager _assetManager;

	private void OnInject(EmployeeSimpleSlotModel model)
	{
	}
}
