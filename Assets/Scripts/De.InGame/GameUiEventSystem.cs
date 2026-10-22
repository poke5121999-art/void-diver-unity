using UniRx;
using VContainer;

public class GameUiEventSystem : MonoBase
{
	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private GameManager _gameManager;

	public readonly Subject<(int level, long exp)> OnUpdateLevelAndExp;

	public int Level { get; private set; }

	public long Exp { get; private set; }

	protected override void Awake()
	{
	}

	private void Start()
	{
	}

	public void FlushLevelAndExp()
	{
	}

	private void OnPlayerInitialized(Unit unit)
	{
	}
}
