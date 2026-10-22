using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Febucci.UI;
using GameCommon.Data;
using GameCommon.Enum;
using GameCommon.Table;
using InputIcons;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using NemoLib.Popup;
using TMPro;
using UniRx;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine;
using VContainer;

public class CharacterHudPresenter : BaseUnitHudPresenter
{
	private const float TOAST_HEIGHT_DEFAULT = 2f;

	private const float TOAST_HEIGHT_FINISH = 2.5f;

	private const float TOAST_DURATION = 1.2f;

	[SerializeField]
	private CharacterModel _characterModel;

	[View]
	private CharacterHudView _view;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private GameContext _gameContext;

	[Inject]
	private PlayerInputController _inputController;

	[Inject]
	private GameChatManager _chatManager;

	[Inject]
	private ToastManager _toastManager;

	private CancellationTokenSource _toastCts;

	private CompositeDisposable _disposable = new CompositeDisposable();

	private int _currentToastPrefabIdx;

	private int _currentToastTextIndex;

	private readonly Queue<string> _toastMessageQueue = new Queue<string>();

	private Tween _consumeGaugeTween;

	private IDisposable _gaugeSkillIdDisposable;

	private CompositeDisposable _viewTargetDisposable;

	private float _targetStaminaRatio;

	private float _staminaGaugeVelocity;

	private readonly Queue<(int id, bool isAdded)> _paradoxQueue = new Queue<(int id, bool isAdded)>();

	private bool _isProcessingParadox;

	private CancellationTokenSource _paradoxCts;

	private CompositeDisposable _skillReadyDisposable;

	private readonly Dictionary<int, bool> _prevSkillUsable = new Dictionary<int, bool>();

	private float _skillReadyCheckTimer;

	private const float SKILL_READY_CHECK_INTERVAL = 0.1f;

	private const string PARADOX_ANIM_IN = "In";

	private const string PARADOX_ANIM_OUT = "Out";

	private const float PARADOX_ANIM_IN_DURATION = 0.9f;

	private const float PARADOX_ANIM_OUT_DURATION = 0.9f;

	private GameObject _currentEmoji;

	protected override GameObject Bubble => _view.Bubble;

	protected override TypewriterByCharacter BubbleTextWriter => _view.Bubble_Text.GetComponent<TypewriterByCharacter>();

	protected override TextMeshProUGUI Monologue => _view.Monologue;

	private StressLineController StressLineController => _view.CharacterStressLine.GetComponent<StressLineController>();

	private bool IsMyCharacter => _characterModel.ClientId == _gameManager.LocalClientId;

	private void Start()
	{
		OnUpdateMaxHp();
		_characterModel.RxPlayerName.Subscribe(delegate
		{
			UpdateCharacterName();
		}).AddTo(_disposable);
		_characterModel.RxHp.Subscribe(delegate
		{
			UpdateHpUi();
		}).AddTo(_disposable);
		_characterModel.RxHpMax.Subscribe(delegate
		{
			OnUpdateMaxHp();
		}).AddTo(_disposable);
		_characterModel.RxShield.Subscribe(delegate
		{
			UpdateHpUi();
		}).AddTo(_disposable);
		_gameChannel.Receive<NtfUnitMonologueText>().Where((NtfUnitMonologueText _) => _.NetworkObjectId == _characterModel.NetworkObjectId).Subscribe(delegate(NtfUnitMonologueText _)
		{
			ShowMonologueText(LocalizationUtility.GetLocalizedText(_.TextKey));
		}).AddTo(_disposable);
		_gameChannel.Receive<NtfEquipmentBroken>().Where((NtfEquipmentBroken _) => _.NetworkObjectId == _characterModel.NetworkObjectId).Subscribe(OnEquipmentBroken).AddTo(_disposable);
		_gameChannel.Receive<NtfUnitBubbleText>().Where((NtfUnitBubbleText _) => _.NetworkObjectId == _characterModel.NetworkObjectId).Subscribe(delegate(NtfUnitBubbleText _)
		{
			ShowBubbleText(LocalizationUtility.GetLocalizedText(_.TextKey));
		}).AddTo(_disposable);
		_gameChannel.Receive<NtfStressDamage>().Where((NtfStressDamage _) => _.NetworkObjectId == _characterModel.NetworkObjectId).Where((NtfStressDamage _) => _.StressReasonType == EStressReasonType.HpDamaged).Subscribe(delegate
		{
			ShowMonologueText(TLocalizedText.PainMentalDamage.LocalText);
		}).AddTo(_disposable);
		_gameChannel.Receive<NtfStressDamage>().Where((NtfStressDamage _) => _.NetworkObjectId == _characterModel.NetworkObjectId).Where((NtfStressDamage _) => _.StressReasonType == EStressReasonType.AllyDead).Subscribe(delegate
		{
			ShowMonologueText(TLocalizedText.AllyVoiceFades.LocalText);
		}).AddTo(_disposable);
		_gameChannel.Receive<NtfStressDamage>().Where((NtfStressDamage _) => _.NetworkObjectId == _characterModel.NetworkObjectId).Where((NtfStressDamage _) => _.StressReasonType == EStressReasonType.DotRange).Where((NtfStressDamage _) => _characterModel.RxBuffs.Values.Any((UnitBuff _) => _.Id == 2000001)).Subscribe(delegate
		{
			ShowMonologueText(TLocalizedText.DangerousNearby.LocalText);
		}).AddTo(_disposable);
		_gameChannel.Receive<NtfShowConsumeGauge>().Where((NtfShowConsumeGauge _) => _.NetworkObjectId == _characterModel.NetworkObjectId).Where((NtfShowConsumeGauge _) => _.OtherPlayerCanSee || _characterModel.ClientId == _gameManager.RxViewTargetClientId.Value).Subscribe(delegate(NtfShowConsumeGauge _)
		{
			ShowConsumeGauge(_.ItemId, _.Duration);
		}).AddTo(_disposable);
		_characterModel.RxStressState.Where((EStressState _) => _ == EStressState.Fear).Subscribe(delegate
		{
			ShowMonologueText(TLocalizedText.FearApproaches.LocalText);
		}).AddTo(_disposable);
		_characterModel.RxStressState.Where((EStressState _) => _ == EStressState.Despair).Subscribe(delegate
		{
			ShowMonologueText(TLocalizedText.DespairAllEnds.LocalText);
		}).AddTo(_disposable);
		_characterModel.RxState.Where((EActionState _) => _ == EActionState.Dying).Subscribe(delegate
		{
			ShowMonologueText(TLocalizedText.ThisIsTheEnd.LocalText);
		}).AddTo(_disposable);
		_characterModel.RxState.Pairwise().Where((Pair<EActionState> _) => _.Current == EActionState.Revive && _.Previous == EActionState.Dead).Subscribe(delegate
		{
			ShowMonologueText(TLocalizedText.NotOverYet.LocalText);
		}).AddTo(_disposable);
		_characterModel.RxBuffs.ObserveAdd().Where((DictionaryAddEvent<int, UnitBuff> evt) => IsDespairBuff(evt.Key)).Subscribe(delegate(DictionaryAddEvent<int, UnitBuff> evt)
		{
			OnDespairBuffAdded(evt.Key);
		}).AddTo(_disposable);
		_characterModel.RxBuffs.ObserveRemove().Where((DictionaryRemoveEvent<int, UnitBuff> evt) => IsDespairBuff(evt.Key)).Subscribe(delegate(DictionaryRemoveEvent<int, UnitBuff> evt)
		{
			OnDespairBuffRemoved(evt.Key);
		}).AddTo(_disposable);
		_characterModel.RxState.Subscribe(OnUpdateState).AddTo(_disposable);
		_characterModel.RxStressState.Subscribe(OnUpdateStressState).AddTo(_disposable);
		_characterModel.RxStress.Subscribe(OnUpdateStress).AddTo(_disposable);
		_gameChannel.Receive<NtfStressDamage>().Subscribe(OnStressDamage).AddTo(this);
		CharacterController component = _characterModel.GetComponent<CharacterController>();
		component.OnShowBubbleText.Subscribe(ShowBubbleText).AddTo(_disposable);
		component.OnShowMonologueText.Subscribe(ShowMonologueText).AddTo(_disposable);
		component.OnShowEmoji.Subscribe(OnShowEmoji).AddTo(_disposable);
		component.OnShowSelfReviveText.Subscribe(ShowSelfReviveText).AddTo(_disposable);
		foreach (CharacterModel characterModel in _gameManager.CharacterModels)
		{
			if (characterModel.ClientId != _characterModel.ClientId)
			{
				characterModel.RxState.Where((EActionState _) => _ == EActionState.StageReady && _characterModel.RxState.Value != EActionState.StageReady).Subscribe(delegate
				{
					ShowMonologueText(TLocalizedText.SomeoneIsReady.LocalText);
				}).AddTo(_disposable);
			}
		}
		_gameContext.Players[_characterModel.ClientId].RxCharacter.Pairwise().Subscribe(delegate(Pair<Character> _)
		{
			OnUpdateParadox(_.Previous.GetParadoxIds(), _.Current.GetParadoxIds());
		}).AddTo(this);
		_gameManager.OnCharacterSpawned.Subscribe(OnCharacterSpawned).AddTo(this);
		_characterModel.RxState.Subscribe(OnStateChanged).AddTo(_disposable);
		_view.HpGauge.SetActive(value: true);
		_gameManager.RxViewTargetClientId.Subscribe(OnViewTargetChanged).AddTo(this);
	}

	private void OnCharacterSpawned(CharacterController characterController)
	{
		if (characterController.ClientId != _characterModel.ClientId)
		{
			characterController.GetComponent<CharacterModel>().RxState.Where((EActionState _) => _ == EActionState.StageReady && _characterModel.RxState.Value != EActionState.StageReady).Subscribe(delegate
			{
				ShowMonologueText(TLocalizedText.SomeoneIsReady.LocalText);
			}).AddTo(_disposable);
		}
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		_toastCts?.Cancel();
		_disposable.Dispose();
		_skillReadyDisposable?.Dispose();
		_consumeGaugeTween?.Kill();
		_paradoxCts?.Cancel();
		_paradoxCts?.Dispose();
	}

	private void Update()
	{
		if (_view.StaminaGauge.activeSelf)
		{
			_view.StaminaGauge_Fill.fillAmount = Mathf.SmoothDamp(_view.StaminaGauge_Fill.fillAmount, _targetStaminaRatio, ref _staminaGaugeVelocity, 0.1f, float.PositiveInfinity, Time.deltaTime);
		}
		_skillReadyCheckTimer += Time.deltaTime;
		if (_skillReadyCheckTimer >= 0.1f)
		{
			_skillReadyCheckTimer = 0f;
			UpdateSkillReadyNotification();
		}
	}

	private void UpdateSkillReadyNotification()
	{
		if (_characterModel.ClientId != _gameManager.LocalClientId)
		{
			return;
		}
		foreach (int activeSkillId in _characterModel.GetActiveSkillIds())
		{
			if (activeSkillId >= 1 && TSkill.Table[activeSkillId].NotifySkillReady)
			{
				bool flag = _characterModel.CheckSkill(activeSkillId);
				bool value;
				bool flag2 = _prevSkillUsable.TryGetValue(activeSkillId, out value);
				_prevSkillUsable[activeSkillId] = flag;
				if (flag2 && flag && !value)
				{
					ShowSkillReadyEffectAsync(activeSkillId).Forget();
				}
			}
		}
	}

	private void UpdateCharacterName()
	{
		_view.NameText.text = _characterModel.RxPlayerName.Value;
		_view.NameText_Image.gameObject.SetActive(_characterModel.ClientId == 0);
	}

	private void UpdateHpUi()
	{
		if (!(_characterModel.HpMax <= 0f))
		{
			_view.HpGauge.GetComponent<HpAfterImageTweenGauge>().SetHpWithGuard(_characterModel.Hp, (int)_characterModel.HpMax, _characterModel.Shield);
		}
	}

	private void ShowConsumeGauge(int itemId, float duration)
	{
		_consumeGaugeTween?.Kill(complete: true);
		_gaugeSkillIdDisposable?.Dispose();
		IList<BuffEffectInfo<ItemUseSpeedAmplifierEffect>> buffEffects = _characterModel.GetBuffEffects<ItemUseSpeedAmplifierEffect>();
		if (buffEffects.Any())
		{
			float value = buffEffects.Sum((BuffEffectInfo<ItemUseSpeedAmplifierEffect> b) => (float)b.Stack * b.BuffEffect.Percent) / 100f + 1f;
			value = Mathf.Clamp(value, TConst.BuffEffectAmplifierMin.FloatValue, TConst.BuffEffectAmplifierMax.FloatValue);
			duration /= value;
		}
		_view.ItemUseGauge.SetActive(value: true);
		_view.ItemUseGauge_GaugeFill.fillAmount = 0f;
		_view.ItemUseGauge_Image.sprite = _assetManager.GetIconItem(itemId);
		int itemSkillId = TItem.Table[itemId].SkillId;
		_consumeGaugeTween = _view.ItemUseGauge_GaugeFill.DOFillAmount(1f, duration).SetLink(_view.ItemUseGauge_GaugeFill.gameObject).OnComplete(delegate
		{
			_view.ItemUseGauge.SetActive(value: false);
			_gaugeSkillIdDisposable?.Dispose();
			_gaugeSkillIdDisposable = null;
		});
		_gaugeSkillIdDisposable = _characterModel.RxUsingSkillId.Skip(1).Where((int id) => id != itemSkillId).Take(1).Subscribe(delegate
		{
			_view.ItemUseGauge.SetActive(value: false);
			_consumeGaugeTween?.Kill();
		});
		_gaugeSkillIdDisposable.AddTo(_disposable);
	}

	private void OnUpdateMaxHp()
	{
		if (!(_characterModel.HpMax <= 0f))
		{
			float num = TConst.HpBarMaxLimit.IntValue;
			float num2 = TConst.HpBarMinLimit.IntValue;
			float num3 = Mathf.Clamp01((_characterModel.HpMax - (float)TConst.HpBarMinLimit.IntValue) / (num - num2));
			RectTransform rectTransform = _view.HpGauge.transform as RectTransform;
			rectTransform.sizeDelta = new Vector2(num3 * 30f + 62f, rectTransform.sizeDelta.y);
			UpdateHpUi();
		}
	}

	private async UniTask TryShowToastMessageAsync()
	{
		if (_toastCts != null)
		{
			return;
		}
		string text = _toastMessageQueue.Dequeue();
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		_toastCts = new CancellationTokenSource();
		float interval = TConst.AutoToastMessageInterval.FloatValue;
		_currentToastTextIndex = (_currentToastTextIndex + 1) % _view.ToastTexts.Length;
		TextMeshProUGUI toastText = _view.ToastTexts[_currentToastPrefabIdx].GetComponent<TextMeshProUGUI>();
		Vector2 anchoredPosition = toastText.rectTransform.anchoredPosition;
		toastText.text = text;
		toastText.alpha = 0f;
		toastText.rectTransform.anchoredPosition = new Vector2(anchoredPosition.x, 2f);
		toastText.DOFade(1f, interval / 3f).OnComplete(delegate
		{
			toastText.DOFade(0f, interval / 3f);
		});
		toastText.rectTransform.DOAnchorPosY(2.5f, interval);
		await UniTask.Delay((int)(interval * 1000f), ignoreTimeScale: false, PlayerLoopTiming.Update, _toastCts.Token);
		_toastCts = null;
		if (_toastMessageQueue.Any())
		{
			TryShowToastMessageAsync().Forget();
		}
	}

	private void AddToastMessage(string message)
	{
		if (!string.IsNullOrEmpty(message))
		{
			_toastMessageQueue.Enqueue(message);
			if (_toastMessageQueue.Any())
			{
				TryShowToastMessageAsync().Forget();
			}
		}
	}

	private void OnStateChanged(EActionState state)
	{
		bool flag = ((state == EActionState.Dead || state == EActionState.Revive || state == EActionState.Escaped) ? false : (state != EActionState.StageReady));
		_view.HpGauge.SetActive(flag);
		_view.CharacterStressLine.SetActive(flag);
		_view.NameText.gameObject.SetActive(_characterModel.ClientId != _gameManager.LocalClientId && flag);
	}

	private void OnViewTargetChanged(ulong clientId)
	{
		_viewTargetDisposable?.Dispose();
		if (_characterModel.ClientId == clientId)
		{
			_viewTargetDisposable = new CompositeDisposable();
			_characterModel.RxStamina.Skip(1).Do(delegate(float stamina)
			{
				_targetStaminaRatio = Mathf.Clamp01(stamina / _characterModel.StaminaMax);
				_view.StaminaGauge.SetActive(value: true);
			}).Throttle(TimeSpan.FromSeconds(TConst.StaminaGaugeHideDelay.FloatValue)).Subscribe(delegate
			{
				_view.StaminaGauge.SetActive(value: false);
			})
				.AddTo(_viewTargetDisposable);
		}
		else
		{
			_view.StaminaGauge.SetActive(value: false);
		}
	}

	private void OnShowEmoji(int emojiId)
	{
		if (_currentEmoji != null)
		{
			_currentEmoji.DespawnOrDestroy();
		}
		_currentEmoji = _assetManager.SpawnEmoji(emojiId, _view.EmojiRoot.transform);
		_currentEmoji.transform.localPosition = Vector3.zero;
		_currentEmoji.DespawnOrDestroy(1.5f);
	}

	private void OnStressDamage(NtfStressDamage message)
	{
		if (_characterModel.NetworkObjectId == message.NetworkObjectId && message.Amount > 0f)
		{
			StressLineController.PlayHitEffect();
		}
	}

	private void OnUpdateStressState(EStressState state)
	{
		StressLineController.SetStressState(state);
	}

	private void OnUpdateStress(float stress)
	{
		StressLineController.SetStress(stress);
	}

	private void OnUpdateState(EActionState state)
	{
		switch (state)
		{
		case EActionState.Dying:
		case EActionState.Dead:
			StressLineController.SetDeathState(death: true);
			break;
		case EActionState.Revive:
			StressLineController.SetDeathState(death: false);
			break;
		}
	}

	private void ShowSelfReviveText(string text)
	{
		_view.OneCoin_Text.text = text;
		_view.OneCoin.SetActive(value: true);
		Observable.Timer(TimeSpan.FromSeconds(2.5)).Subscribe(delegate
		{
			_view.OneCoin.SetActive(value: false);
		}).AddTo(_disposable);
	}

	private void OnUpdateParadox(IList<int> previousIds, IList<int> currentIds)
	{
		if (previousIds == null)
		{
			previousIds = new List<int>();
		}
		if (currentIds == null)
		{
			currentIds = new List<int>();
		}
		IEnumerable<int> enumerable = currentIds.Except(previousIds);
		IEnumerable<int> enumerable2 = previousIds.Except(currentIds);
		foreach (int item in enumerable)
		{
			EnqueueParadox(item, added: true);
		}
		foreach (int item2 in enumerable2)
		{
			EnqueueParadox(item2, added: false);
		}
	}

	private void EnqueueParadox(int id, bool added)
	{
		_paradoxQueue.Enqueue((id, added));
		ProcessParadoxQueueAsync().Forget();
	}

	private async UniTask ProcessParadoxQueueAsync()
	{
		if (_isProcessingParadox)
		{
			return;
		}
		_isProcessingParadox = true;
		if (_paradoxCts == null)
		{
			_paradoxCts = new CancellationTokenSource();
		}
		Animator animator = _view.GainParadox_Normal.GetComponent<Animator>();
		_view.GainParadox.SetActive(value: true);
		while (true)
		{
			if (_paradoxQueue.Count < 1)
			{
				_isProcessingParadox = false;
				_view.GainParadox.SetActive(value: false);
				return;
			}
			(int id, bool isAdded) tuple = _paradoxQueue.Dequeue();
			_view.GainParadox_Text.text = TParadox.Table[tuple.id].Name;
			if (!tuple.isAdded)
			{
				_view.GainParadox_Line.gameObject.SetActive(value: true);
				if (animator != null && animator.isActiveAndEnabled)
				{
					animator.Play("Out", 0, 0f);
				}
				await UniTask.Delay(TimeSpan.FromSeconds(0.9f), ignoreTimeScale: false, PlayerLoopTiming.Update, _paradoxCts.Token);
			}
			else
			{
				_view.GainParadox_Line.gameObject.SetActive(value: false);
				if (animator.isActiveAndEnabled)
				{
					animator.Play("In", 0, 0f);
				}
				await UniTask.Delay(TimeSpan.FromSeconds(0.9f), ignoreTimeScale: false, PlayerLoopTiming.Update, _paradoxCts.Token);
			}
		}
	}

	private async UniTask ShowSkillReadyEffectAsync(int skillId)
	{
		_skillReadyDisposable?.Dispose();
		_skillReadyDisposable = new CompositeDisposable();
		_view.HudSkillReset.SetActive(value: false);
		await UniTask.Yield(PlayerLoopTiming.PreLateUpdate);
		if (this == null)
		{
			return;
		}
		_view.HudSkillReset.SetActive(value: true);
		_view.HudSkillReset_Icon.sprite = _assetManager.GetIconSkill(skillId);
		II_ImagePrompt component = _view.HudSkillReset_KeyPrompt.GetComponent<II_ImagePrompt>();
		InputAction skillInputAction = _inputController.GetSkillInputAction(skillId);
		if (skillInputAction != null)
		{
			if (component.spritePromptDatas.Count > 0)
			{
				component.spritePromptDatas[0].actionReference = InputActionReference.Create(skillInputAction);
				component.UpdateDisplayedImages();
			}
			Observable.Timer(TimeSpan.FromSeconds(2.5)).Subscribe(delegate
			{
				_view.HudSkillReset.SetActive(value: false);
			}).AddTo(_skillReadyDisposable);
		}
	}

	private bool IsDespairBuff(int buffId)
	{
		return TBuff.Table[buffId].IsDespairEffect;
	}

	private void OnDespairBuffAdded(int buffId)
	{
		_view.DespairBuffAdded.SetActive(value: true);
		_view.DespairBuffAdded_Text.text = TBuff.Table[buffId].Name;
		Observable.Timer(TimeSpan.FromSeconds(2.0)).Subscribe(delegate
		{
			_view.DespairBuffAdded.SetActive(value: false);
		}).AddTo(_disposable);
	}

	private void OnDespairBuffRemoved(int buffId)
	{
		_view.DespairBuffRemoved.SetActive(value: true);
		_view.DespairBuffRemoved_Text.text = TBuff.Table[buffId].Name;
		Observable.Timer(TimeSpan.FromSeconds(2.0)).Subscribe(delegate
		{
			_view.DespairBuffRemoved.SetActive(value: false);
		}).AddTo(_disposable);
	}

	private void OnEquipmentBroken(NtfEquipmentBroken message)
	{
		string name = TEquipment.Table[message.EquipmentId].Name;
		string text = TLocalizedText.MonologueEquipmentBrokenFormat.Format(name);
		ShowMonologueText(text);
		if (_characterModel.ClientId == _gameManager.LocalClientId)
		{
			_toastManager.Notice(text);
			_chatManager.AddChat(new ChatInfo
			{
				Type = EChatType.System,
				Text = TLocalizedText.ChatEquipmentBrokenFormat.Format(name),
				UtcTime = DateTime.UtcNow
			});
		}
	}
}
