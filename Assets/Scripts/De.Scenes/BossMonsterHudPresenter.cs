using DG.Tweening;
using De.InGame.Unit;
using Febucci.UI;
using GameCommon.Enum;
using NemoLib.Log;
using De.InGame;
using NemoLib.Mvp;
using TMPro;
using System;
using UniRx;
using UnityEngine;
using VContainer;

public class BossMonsterHudPresenter : BaseUnitHudPresenter
{
	[SerializeField]
	private MonsterModel _monsterModel;

	[SerializeField]
	private float _activeTimeSec = 0.3f;

	[View]
	private BossMonsterHudView _view;

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

	private MonsterView MonsterView => _monsterModel.GetComponent<MonsterView>();

	private void Start()
	{
		_monsterModel.RxHp.Pairwise().Subscribe(delegate(Pair<int> pair)
		{
			OnHpChanged(pair.Previous, pair.Current);
		}).AddTo(_disposable);
		_monsterModel.RxShield.Subscribe(OnShieldChanged).AddTo(_disposable);
		_monsterModel.RxState.Subscribe(OnStateChanged).AddTo(_disposable);
		UnitController component = _monsterModel.GetComponent<UnitController>();
		component.OnShowBubbleText.Subscribe(ShowBubbleText).AddTo(_disposable);
		component.OnShowMonologueText.Subscribe(ShowMonologueText).AddTo(_disposable);
		_gameChannel.Receive<NtfUnitMonologueText>().Where((NtfUnitMonologueText ntf) => ntf.NetworkObjectId == _monsterModel.NetworkObjectId).Subscribe(delegate(NtfUnitMonologueText ntf)
		{
			ShowMonologueText(LocalizationUtility.GetLocalizedText(ntf.TextKey));
		})
			.AddTo(_disposable);
		_gameChannel.Receive<NtfUnitBubbleText>().Where((NtfUnitBubbleText ntf) => ntf.NetworkObjectId == _monsterModel.NetworkObjectId).Subscribe(delegate(NtfUnitBubbleText ntf)
		{
			ShowBubbleText(LocalizationUtility.GetLocalizedText(ntf.TextKey));
		})
			.AddTo(_disposable);
		_view.HpGauge.SetActive(value: false);
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		_disposable.Dispose();
	}

	private void OnDisable()
	{
		DisableStateIcon();
	}

	private void EnableStateIcon(EActionState state)
	{
		_aggroTween?.Kill();
		_view.QuestionIcon.SetActive(value: false);
		_view.ExclamationIcon.SetActive(value: false);
		GameObject gameObject = ((state == EActionState.Aggro) ? _view.ExclamationIcon : _view.QuestionIcon);
		gameObject.SetActive(value: true);
		gameObject.transform.localScale = Vector3.zero;
		gameObject.transform.localPosition = GetHeadIconPosition();
		_aggroTween = gameObject.transform.DOScale(0.005f, 0.25f).SetEase(Ease.OutExpo);
	}

	private void DisableStateIcon()
	{
		_aggroTween?.Kill();
		_view.QuestionIcon.SetActive(value: false);
		_view.ExclamationIcon.SetActive(value: false);
	}

	private Vector3 GetHeadIconPosition()
	{
		Vector3 boneOffset = _monsterModel.GetComponent<MonsterView>().GetBoneOffset(EUnitBoneType.Head);
		return new Vector3(0f, boneOffset.y + 0.25f, 0f);
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
		// TODO(decomp): the binary only tail-calls UnitModel.get_HpMax here (the result is unused); the real body may be a COMDAT-folded stub. @0x1808b6d80
		float hpMax = _monsterModel.HpMax;
	}

	private void OnShieldChanged(int value)
	{
	}

	private void UpdateHpGauge()
	{
		HpAfterImageTweenGauge component = _view.HpGauge.GetComponent<HpAfterImageTweenGauge>();
		component.SetHpWithGuard(_monsterModel.Hp, (int)_monsterModel.HpMax, _monsterModel.Shield);
	}

	private void OnStateChanged(EActionState state)
	{
		if (state != EActionState.Dead)
		{
			if (state >= EActionState.Alert && state <= EActionState.Aggro)
			{
				EnableStateIcon(state);
				return;
			}
			if (state != EActionState.Captured)
			{
				DisableStateIcon();
				return;
			}
		}
		_view.HpGauge.SetActive(value: false);
		DisableStateIcon();
	}
}
