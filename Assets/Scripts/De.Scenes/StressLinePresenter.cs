using GameCommon.Enum;
using GameCommon.Table;
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
		_stressLineController = GetComponent<StressLineController>();
		base.Awake();
	}

	private void Start()
	{
		_gameChannel.Receive<NtfStressDamage>().Subscribe(OnStressDamage).AddTo(this);
	}

	private void OnDestroy()
	{
		_disposable?.Dispose();
	}

	private void OnInject(Model model)
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		_model.CharacterModel.RxState.Subscribe(OnUpdateState).AddTo(_disposable);
		_model.CharacterModel.RxStressState.Subscribe(OnUpdateStressState).AddTo(_disposable);
		_model.CharacterModel.RxStress.Subscribe(OnUpdateStress).AddTo(_disposable);
	}

	private void OnUpdateStressState(EStressState state)
	{
		_stressLineController.SetStressState(state);
	}

	private void OnUpdateStress(float stress)
	{
		int intValue = TConst.StressMax.IntValue;
		_view.Stress_Value.text = stress.ToString("F0");
		_view.Stress_Max.text = string.Format("/ {0}", intValue);
		_stressLineController.SetStress(stress);
	}

	private void OnUpdateState(EActionState state)
	{
		if (state == EActionState.Dying || state == EActionState.Dead)
		{
			_stressLineController.SetDeathState(death: true);
		}
		else if (state == EActionState.Revive)
		{
			_stressLineController.SetDeathState(death: false);
		}
	}

	private void OnStressDamage(NtfStressDamage message)
	{
		if (_model.CharacterModel != null && message != null && _model.CharacterModel.NetworkObjectId == message.NetworkObjectId && message.Amount > 0f)
		{
			_stressLineController.PlayHitEffect();
		}
	}
}
