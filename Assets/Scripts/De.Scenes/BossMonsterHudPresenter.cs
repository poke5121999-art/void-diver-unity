using DG.Tweening;
using De.InGame.Unit;
using Febucci.UI;
using GameCommon.Enum;
using NemoLib.Log;
using NemoLib.Mvp;
using TMPro;
using UniRx;
using UnityEngine;
using VContainer;

public class BossMonsterHudPresenter : BaseUnitHudPresenter
{
	[SerializeField]
	private MonsterModel _monsterModel;

	[SerializeField]
	private float _activeTimeSec;

	[View]
	private BossMonsterHudView _view;

	[Inject]
	private SeriLogger _logger;

	[Inject]
	private GameChannel _gameChannel;

	private CompositeDisposable _disposable;

	private Tween _aggroTween;

	private float _elapsedActiveTime;

	protected override GameObject Bubble => null;

	protected override TypewriterByCharacter BubbleTextWriter => null;

	protected override TextMeshProUGUI Monologue => null;

	private MonsterView MonsterView => null;

	private void Start()
	{
	}

	protected override void OnDestroy()
	{
	}

	private void OnDisable()
	{
	}

	private void EnableStateIcon(EActionState state)
	{
	}

	private void DisableStateIcon()
	{
	}

	private Vector3 GetHeadIconPosition()
	{
		return default;
	}

	private void Update()
	{
	}

	private void OnHpChanged(int oldHp, int newHp)
	{
	}

	private void OnShieldChanged(int value)
	{
	}

	private void UpdateHpGauge()
	{
	}

	private void OnStateChanged(EActionState state)
	{
	}
}
