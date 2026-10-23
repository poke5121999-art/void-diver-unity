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
		PanicGaugeUtility.Apply(_view.PanicSlider, _view.PanicSlider_Gauge_img_back, _view.PanicSlider_Gauge_img_front, model.Stress, model.IsPanic);
		EmployeeExpUtility.Apply(_view.ExpSlider, model.Level, model.Exp);
		_view.PanicSlider_Face.sprite = _assetManager.GetIconEmployeeCondition(model.Stress, model.IsPanic);
		_view.Face.sprite = _assetManager.GetIconEmployeeFace(model.EmployeeId);
	}
}
