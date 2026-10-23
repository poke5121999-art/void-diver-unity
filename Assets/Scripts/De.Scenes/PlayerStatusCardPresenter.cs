using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using GameCommon.Enum;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
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

	private StressLineController StressLineController => _view.CharacterStressLine.GetComponent<StressLineController>();

	private void OnDestroy()
	{
		_disposable?.Dispose();
		KillHpGaugeTweens();
	}

	private void OnInject(PlayerStatusCardModel model)
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		_player = _gameContext.Players.Values.First((GamePlayer p) => p.PlayerIndex == model.PlayerIndex);
		_characterModel = _gameManager.FindCharacterModel(_player.ClientId);
		InitProfile(_player);
		KillHpGaugeTweens();
		_hpGaugeInitialized = false;
		_characterModel.RxHp.Subscribe(delegate
		{
			UpdateHpGauge();
		}).AddTo(_disposable);
		_characterModel.RxHpMax.Subscribe(delegate
		{
			UpdateHpGauge();
		}).AddTo(_disposable);
		_characterModel.RxShield.Subscribe(delegate
		{
			UpdateHpGauge();
		}).AddTo(_disposable);
		_characterModel.RxStressState.Subscribe(OnUpdateStressState).AddTo(_disposable);
		_characterModel.RxStress.Subscribe(OnUpdateStress).AddTo(_disposable);
		_gameContext.RxPlayerRtt.ObserveReplace().Where((DictionaryReplaceEvent<ulong, ulong> e) => e.Key == _player.ClientId).Subscribe(delegate(DictionaryReplaceEvent<ulong, ulong> e)
		{
			OnUpdateRtt(e.NewValue);
		})
			.AddTo(_disposable);
		if (_gameContext.PlayerRtt.TryGetValue(_player.ClientId, out var value))
		{
			OnUpdateRtt(value);
		}
	}

	private void InitProfile(GamePlayer player)
	{
		_view.Face_Image.sprite = _assetManager.GetIconUnitCharacter(_characterModel.CharacterId, "Default");
		_view.Name.text = player.Name;
		_view.Steamid.text = player.SteamId.ToString();
		_view.HostIcon.gameObject.SetActive(player.IsHostPlayer);
		_view.ColorBar.color = player.GetPlayerColor();
	}

	private void UpdateHpGauge()
	{
		float hpMax = _characterModel.HpMax;
		if (hpMax <= 0f)
		{
			return;
		}
		int hp = Mathf.Max(0, _characterModel.Hp);
		int shield = Mathf.Max(0, _characterModel.Shield);
		int total = hp + shield;
		float hpRatio;
		float shieldRatio;
		if ((float)total <= hpMax)
		{
			hpRatio = Mathf.Clamp01((float)hp / hpMax);
			shieldRatio = Mathf.Clamp01((float)total / hpMax);
		}
		else
		{
			hpRatio = ((total < 1) ? 0f : ((float)hp / (float)total));
			shieldRatio = 1f;
		}
		SetHpRatio(hpRatio);
		_view.Hp_Gauge_Shield.fillAmount = ((shield == 0) ? 0f : shieldRatio);
	}

	private void SetHpRatio(float ratio)
	{
		Image front = _view.Hp_Gauge_Front;
		Image back = _view.Hp_Gauge_Back;
		KillHpGaugeTweens();
		if (!_hpGaugeInitialized)
		{
			front.fillAmount = ratio;
			back.fillAmount = ratio;
			_prevHpRatio = ratio;
			_hpGaugeInitialized = true;
			return;
		}
		Image tweenBar = front;
		Image snapBar = back;
		if (ratio - _prevHpRatio < 0f)
		{
			tweenBar = back;
			snapBar = front;
		}
		snapBar.fillAmount = ratio;
		float current = tweenBar.fillAmount;
		DOTween.To(() => current, delegate(float value)
		{
			tweenBar.fillAmount = value;
		}, ratio, 0.25f).SetTarget(tweenBar).SetLink(tweenBar.gameObject);
		_prevHpRatio = ratio;
	}

	private void KillHpGaugeTweens()
	{
		if (_view == null)
		{
			return;
		}
		DOTween.Kill(_view.Hp_Gauge_Front);
		DOTween.Kill(_view.Hp_Gauge_Back);
	}

	private void OnUpdateStressState(EStressState state)
	{
		StressLineController.SetStressState(state);
	}

	private void OnUpdateStress(float stress)
	{
		StressLineController.SetStress(stress);
	}

	private void OnUpdateRtt(ulong rtt)
	{
		if (_player == null || _player.IsHostPlayer)
		{
			_view.RttText.gameObject.SetActive(value: false);
			return;
		}
		_view.RttText.gameObject.SetActive(value: true);
		_view.RttText.text = string.Format("{0}ms", rtt);
		int num = ((rtt > 80) ? ((rtt > 140) ? 3 : 2) : ((rtt > 40) ? 1 : 0));
		for (int i = 0; i < _view.RttIcons.Length; i++)
		{
			_view.RttIcons[i].SetActive(i == num);
		}
	}
}
