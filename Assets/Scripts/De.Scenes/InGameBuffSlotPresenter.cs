using NemoLib.AssetManagement;
using NemoLib.Log;
using NemoLib.Mvp;
using VContainer;

public class InGameBuffSlotPresenter : BaseSlotPresenter<InGameBuffSlotModel>
{
	[InjectModel]
	private InGameBuffSlotModel _model;

	[View]
	private InGameBuffSlotView _view;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private SeriLogger _logger;

	[Inject]
	private GameManager _gameManager;

	private float _startTime;

	private float _endTime;

	private float _duration;

	protected override InGameBuffSlotModel SlotModel => null;

	private void OnInject(Model model)
	{
	}

	private void Init()
	{
	}

	public void Refresh()
	{
	}

	private void InitializeVisual(UnitBuff unitBuff)
	{
	}

	private void UpdateEndTimeAndStack(UnitBuff unitBuff)
	{
	}

	private void UpdateStackText(int stack)
	{
	}

	private void OnBuffUpdated()
	{
	}

	private void FixedUpdate()
	{
	}
}
