using NemoLib.Mvp;
using UnityEngine.UI;

public class TycoonTabPresenter : BaseSlotPresenter<TycoonTabModel>
{
	[View]
	private TycoonTabView _view;

	[InjectModel]
	private TycoonTabModel _model;

	protected override TycoonTabModel SlotModel => null;

	protected override Button Button => null;

	private void OnInject(TycoonTabModel model)
	{
	}
}
