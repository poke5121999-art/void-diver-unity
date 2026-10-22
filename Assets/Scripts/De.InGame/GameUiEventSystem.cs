using UniRx;
using UnityEngine;
using VContainer;

public class GameUiEventSystem : MonoBase
{
	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private GameManager _gameManager;

	public readonly Subject<(int level, long exp)> OnUpdateLevelAndExp = new Subject<(int level, long exp)>();

	public int Level { get; private set; }

	public long Exp { get; private set; }

	protected override void Awake()
	{
		Object.DontDestroyOnLoad(this);
	}

	private void Start()
	{
		_playerManager.OnPlayerInitialized.Subscribe(OnPlayerInitialized).AddTo(this);
	}

	public void FlushLevelAndExp()
	{
		Level = _playerManager.Level;
		Exp = _playerManager.Exp;
		OnUpdateLevelAndExp.OnNext((Level, Exp));
	}

	private void OnPlayerInitialized(Unit unit)
	{
		Level = _playerManager.Level;
		Exp = _playerManager.Exp;
	}
}
