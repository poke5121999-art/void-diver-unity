using GameCommon.Enum;
using NemoLib.Mvp;
using UniRx;
using VContainer;

public class StressLinePresenter : Presenter
{
	[InjectModel]
	private StressLineModel _model;

	[View]
	private StressLineView _view;

	[Inject]
	private GameObjectManager _objectManager;

	[Inject]
	private GameChannel _gameChannel;

	private CompositeDisposable _disposable;

	private StressLineController _stressLineController;

	protected override void Awake()
	{
	}

	private void Start()
	{
	}

	private void OnDestroy()
	{
	}

	private void OnInject(Model model)
	{
	}

	private void OnUpdateStressState(EStressState state)
	{
	}

	private void OnUpdateStress(float stress)
	{
	}

	private void OnUpdateState(EActionState state)
	{
	}

	private void OnStressDamage(NtfStressDamage message)
	{
	}
}
