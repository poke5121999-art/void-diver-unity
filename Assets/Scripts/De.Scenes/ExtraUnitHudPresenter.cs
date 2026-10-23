using De.InGame;
using DG.Tweening;
using GameCommon.Enum;
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
	private float _activeTimeSec = 0.3f;

	[View]
	private ExtraUnitHudView _view;

	[Inject]
	private SeriLogger _logger;

	[Inject]
	private GameChannel _gameChannel;

	private CompositeDisposable _disposable = new CompositeDisposable();

	private Tween _aggroTween;

	private float _elapsedActiveTime;

	protected override GameObject Bubble => _view.Bubble;

	protected override TypewriterByCharacter BubbleTextWriter => _view.Bubble_Text.GetComponent<TypewriterByCharacter>();

	protected override TextMeshProUGUI Monologue => _view.Monologue;

	private void Start()
	{
		_extraUnitModel.RxHp.Pairwise().Subscribe(delegate(Pair<int> pair)
		{
			if (_extraUnitModel.HpMax > 0f)
			{
				UpdateHpGauge();
				if (pair.Current < pair.Previous)
				{
					_elapsedActiveTime = 0f;
					_view.HpGauge.SetActive(value: true);
				}
			}
		}).AddTo(_disposable);
		_extraUnitModel.RxShield.Subscribe(OnShieldChanged).AddTo(_disposable);
		_extraUnitModel.RxState.Where((EActionState state) => state == EActionState.Dead).Subscribe(delegate
		{
			OnDead();
		}).AddTo(_disposable);
		_gameChannel.Receive<NtfUnitMonologueText>().Where((NtfUnitMonologueText ntf) => ntf.NetworkObjectId == _extraUnitModel.NetworkObjectId).Subscribe(delegate(NtfUnitMonologueText ntf)
		{
			ShowMonologueText(LocalizationUtility.GetLocalizedText(ntf.TextKey));
		})
			.AddTo(_disposable);
		_gameChannel.Receive<NtfUnitBubbleText>().Where((NtfUnitBubbleText ntf) => ntf.NetworkObjectId == _extraUnitModel.NetworkObjectId).Subscribe(delegate(NtfUnitBubbleText ntf)
		{
			ShowBubbleText(LocalizationUtility.GetLocalizedText(ntf.TextKey));
		})
			.AddTo(_disposable);
		_view.HpGauge.SetActive(value: false);
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		_disposable?.Dispose();
	}

	private void Update()
	{
		if (_view.HpGauge.activeSelf)
		{
			_elapsedActiveTime += Time.deltaTime;
			if (_elapsedActiveTime >= _activeTimeSec)
			{
				_view.HpGauge.SetActive(value: false);
			}
		}
	}

	private void OnHpChanged(int oldHp, int newHp)
	{
		if (_extraUnitModel.HpMax > 0f)
		{
			UpdateHpGauge();
			if (newHp < oldHp)
			{
				_elapsedActiveTime = 0f;
				_view.HpGauge.SetActive(value: true);
			}
		}
	}

	private void OnShieldChanged(int value)
	{
		UpdateHpGauge();
	}

	private void UpdateHpGauge()
	{
		HpAfterImageTweenGauge component = _view.HpGauge.GetComponent<HpAfterImageTweenGauge>();
		component.SetHpWithGuard(_extraUnitModel.Hp, (int)_extraUnitModel.HpMax, _extraUnitModel.Shield, false);
	}

	private void OnDead()
	{
		_view.HpGauge.SetActive(value: false);
	}
}
