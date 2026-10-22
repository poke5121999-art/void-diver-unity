using GameCommon.Enum;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using UniRx;
using VContainer;

public class PlayerStatusCardPresenter : Presenter
{
	[InjectModel]
	private PlayerStatusCardModel _model;

	[View]
	private PlayerStatusCardView _view;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private GameContext _gameContext;

	[Inject]
	private AssetManager _assetManager;

	private CompositeDisposable _disposable;

	private CharacterModel _characterModel;

	private GamePlayer _player;

	private float _prevHpRatio;

	private bool _hpGaugeInitialized;

	private StressLineController StressLineController => null;

	private void OnDestroy()
	{
	}

	private void OnInject(PlayerStatusCardModel model)
	{
	}

	private void InitProfile(GamePlayer player)
	{
	}

	private void UpdateHpGauge()
	{
	}

	private void SetHpRatio(float ratio)
	{
	}

	private void KillHpGaugeTweens()
	{
	}

	private void OnUpdateStressState(EStressState state)
	{
	}

	private void OnUpdateStress(float stress)
	{
	}

	private void OnUpdateRtt(ulong rtt)
	{
	}
}
