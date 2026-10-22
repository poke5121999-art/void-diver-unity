using NemoLib.Mvp;
using UnityEngine.UI;

public class TycoonTabPresenter : BaseSlotPresenter<TycoonTabModel>
{
	[View]
	private TycoonTabView _view;

	[InjectModel]
	private TycoonTabModel _model;

	protected override TycoonTabModel SlotModel => _model;

	protected override Button Button => _view?.Button;

	private void OnInject(TycoonTabModel model)
	{
		InitSlot(model);
	}
}
