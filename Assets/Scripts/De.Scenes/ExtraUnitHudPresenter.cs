using DG.Tweening;
using Febucci.UI;
using NemoLib.Log;
using NemoLib.Mvp;
using TMPro;
using UniRx;
using UnityEngine;
using VContainer;

public class ExtraUnitHudPresenter : BaseUnitHudPresenter
{
	[SerializeField]
	private ExtraUnitModel _extraUnitModel;

	[SerializeField]
	private float _activeTimeSec;

	[View]
	private ExtraUnitHudView _view;

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

	private void Start()
	{
	}

	protected override void OnDestroy()
	{
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

	private void OnDead()
	{
	}
}
