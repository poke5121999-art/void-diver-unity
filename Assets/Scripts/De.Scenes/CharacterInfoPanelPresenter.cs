using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GameCommon.Data;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.AssetManagement;
using NemoLib.Log;
using NemoLib.Mvp;
using UniRx;
using UnityEngine;
using VContainer;

public class CharacterInfoPanelPresenter : BasePanelPresenter
{
	[InjectModel]
	private CharacterInfoPanelModel _model;

	[View]
	private CharacterInfoPanelView _view;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private SeriLogger _logger;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private GameInputManager _inputManager;

	private readonly string _toggleKey = "CharacterInfoPanelPresenter_Toggle";

	private CompositeDisposable _disposable;

	private CompositeDisposable _inputDisposable;

	protected override BasePanelModel PanelModel => _model;

	private InputActions.UiActions UiActions => _inputManager.UiActions;

	private CharacterModel CharacterModel => _gameManager.MyCharacterModel;

	private TCharacter CharacterData => TCharacter.Table[_gameManager.MyCharacterModel.CharacterId];

	private int AccessorySlotCount => TConst.CharacterAccessorySlotCount.IntValue;

	private int ArtifactSlotCount => TConst.CharacterArtifactSlotCount.IntValue;

	private bool ShowAllStats => _view.Toggle.isOn;

	private void OnInject(CharacterInfoPanelModel model)
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		_view.Toggle.isOn = !PlayerPrefs.HasKey(_toggleKey) || PlayerPrefs.GetInt(_toggleKey) == 1;
		_view.CharacterName.text = CharacterData.Name;
		SetInfo();
		UpdateStats();
		UpdateBuffStats();
		UpdateSkinsAsync().Forget();
		SetEquipmentAsync().Forget();
		CharacterModel.RxStats.ObserveChange().Subscribe(delegate
		{
			UpdateStats();
		}).AddTo(_disposable);
		CharacterModel.RxBuffs.ObserveChange().Subscribe(delegate
		{
			UpdateBuffStats();
		}).AddTo(_disposable);
		_view.Toggle.OnValueChangedWithSfx().Subscribe(delegate
		{
			UpdateStats();
		}).AddTo(_disposable);
		model.RxIsInputEnabled.Where((bool _) => _).Subscribe(delegate
		{
			OnInputEnabled();
		}).AddTo(_disposable);
		model.RxIsInputEnabled.Where((bool _) => !_).Subscribe(delegate
		{
			OnInputDisabled();
		}).AddTo(_disposable);
	}

	private void OnDisable()
	{
		_disposable?.Dispose();
		_disposable = null;
	}

	protected override void OnDestroy()
	{
		PlayerPrefs.SetInt(_toggleKey, _view.Toggle.isOn ? 1 : 0);
		base.OnDestroy();
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		_inputDisposable = new CompositeDisposable();
		_inputManager.UiActions.Toggle.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnCharacterInfoPanelToggleClick();
		}).AddTo(_inputDisposable);
	}

	protected override void OnInputDisabled()
	{
		_inputDisposable.Dispose();
	}

	private void OnCharacterInfoPanelToggleClick()
	{
		_view.Toggle.isOn = !_view.Toggle.isOn;
	}

	private void SetInfo()
	{
		TCharacter tCharacter = TCharacter.Table[_playerManager.SelectedCharacter.Id];
		_view.ChaInfo_ChaName.text = tCharacter.Name;
		_view.ChaInfo_ChaSubTitle.text = tCharacter.Title;
	}

	private void UpdateStats()
	{
		bool showAllStats = _view.Toggle.isOn;
		_view.CriticalDamageBoost.SetActive(showAllStats);
		_view.AttackSpeed.SetActive(showAllStats);
		_view.DefensePenetration.SetActive(showAllStats);
		_view.CooldownReduction.SetActive(showAllStats);
		_view.NaturalRecoveryHp.SetActive(showAllStats);
		_view.DamageReduction.SetActive(showAllStats);
		_view.BackAttackDamageBoost.SetActive(showAllStats);
		_view.WeaknessDamageBoost.SetActive(showAllStats);
		_view.FireResistance.SetActive(showAllStats);
		_view.WaterResistance.SetActive(showAllStats);
		_view.WindResistance.SetActive(showAllStats);
		_view.NaturalRecoverStamina.SetActive(showAllStats);
		CharacterModel model = _gameManager.MyCharacterModel;
		_view.Atk_Value.text = string.Format("{0:F0}", model.Atk);
		_view.Def_Value.text = string.Format("{0:F0}", model.Def);
		_view.Hp_Value.text = string.Format("{0}/{1}", model.Hp, model.HpMax);
		_view.Stamina_Value.text = string.Format("{0:F0}", model.StaminaMax);
		_view.MoveSpeed_Value.text = string.Format("{0:F1}", model.MoveSpeed);
		_view.CriticalChance_Value.text = string.Format("{0:F0}%", model.CriticalChancePercent);
		_view.CriticalDamageBoost_Value.text = string.Format("{0:F0}%", model.CriticalDamagePercent);
		_view.AttackSpeed_Value.text = string.Format("{0:F0}%", (model.AtkSpeed - model.BaseStats[EStatType.AtkSpeed]) * 100f);
		string sign = ((model.PenetrationPercent > 0f) ? "+" : string.Empty);
		_view.DefensePenetration_Value.text = string.Format("{0:F0} {1} {2:F0}%", model.Penetration, sign, model.PenetrationPercent);
		_view.CooldownReduction_Value.text = string.Format("{0:F0}%", model.CooldownReductionPercent);
		_view.NaturalRecoveryHp_Value.text = string.Format("{0:F0}", model.HpRegen);
		_view.NaturalRecoverStamina_Value.text = string.Format("{0:F1}", model.StaminaRegen);
		_view.DamageReduction_Value.text = string.Format("{0:F0}%", model.ReductionDamagePercent);
		_view.BackAttackDamageBoost_Value.text = string.Format("{0:F0}%", model.BackAttackDamagePercent);
		_view.WeaknessDamageBoost_Value.text = string.Format("{0:F0}%", model.WeaknessDamagePercent);
		_view.FireResistance_Value.text = string.Format("{0:F0}%", model.FireResistancePercent);
		_view.WaterResistance_Value.text = string.Format("{0:F0}%", model.WaterResistancePercent);
		_view.WindResistance_Value.text = string.Format("{0:F0}%", model.WindResistancePercent);
	}

	private void UpdateBuffStats()
	{
		IList<BuffEffectInfo<StressDamageTakenAmplifierEffect>> buffEffects = _gameManager.MyCharacterModel.GetBuffEffects<StressDamageTakenAmplifierEffect>();
		float num = buffEffects?.Sum((BuffEffectInfo<StressDamageTakenAmplifierEffect> b) => (float)b.Stack * b.BuffEffect.Percent) ?? 0f;
		_view.StressDamageResistance_Value.text = string.Format("{0:F0}%", num);
	}

	private async UniTask UpdateSkinsAsync()
	{
		Character character = _playerManager.SelectedCharacter;
		int weaponId = character.WeaponId;
		UnitSkeletonData unitSkeletonData = await _assetManager.GetCharacterSkeletonDataAsync(character.Id);
		if (!(this == null))
		{
			_view.Spine.skeletonDataAsset = unitSkeletonData.Sw;
			_view.Spine.Initialize(overwrite: true);
			_view.Spine.Skeleton.UpdateCharacterSkin(character.SkinPresetId, weaponId);
		}
	}

	private async UniTask SetEquipmentAsync()
	{
		await UniTask.Yield(PlayerLoopTiming.PreLateUpdate);
		if (this == null)
		{
			return;
		}
		Character character = _playerManager.SelectedCharacter;
		InventoryGoodsSlotModel model = CreateSlotModel(EGoodsSlotCategory.WeaponSlot, character.ActiveWeaponSlot.Goods);
		_view.WeaponSlot.GetComponent<InventoryGoodsSlotPresenter>().Inject(model);
		for (int i = 0; i < AccessorySlotCount; i++)
		{
			InventoryGoodsSlotModel model2 = CreateSlotModel(EGoodsSlotCategory.AccessorySlot, character.AccessorySlots[i].Goods);
			_view.AccessorySlots[i].GetComponent<InventoryGoodsSlotPresenter>().Inject(model2);
		}
		for (int j = 0; j < ArtifactSlotCount; j++)
		{
			InventoryGoodsSlotModel model3 = CreateSlotModel(EGoodsSlotCategory.ArtifactSlot, character.ArtifactSlots[j].Goods);
			_view.ArtifactSlots[j].GetComponent<InventoryGoodsSlotPresenter>().Inject(model3);
		}

		InventoryGoodsSlotModel CreateSlotModel(EGoodsSlotCategory slotCategory, Goods goods)
		{
			InventoryGoodsSlotModel slotModel = new InventoryGoodsSlotModel(slotCategory, goods, InventoryGoodsSlotPresenter.ELayout.Normal);
			slotModel.UseTooltip = false;
			slotModel.ShowItemCount = false;
			slotModel.OnLeftClick.Subscribe(delegate
			{
				_model.OnGoodsSlotLeftClick.OnNext(slotModel);
			}).AddTo(this);
			slotModel.OnRightClick.Subscribe(delegate
			{
				_model.OnGoodsSlotRightClick.OnNext(slotModel);
			}).AddTo(this);
			slotModel.OnEnter.Subscribe(delegate
			{
				_model.OnGoodsSlotEntered.OnNext(slotModel);
			}).AddTo(this);
			slotModel.OnExit.Subscribe(delegate
			{
				_model.OnGoodsSlotExit.OnNext(slotModel);
			}).AddTo(this);
			return slotModel;
		}
	}
}
