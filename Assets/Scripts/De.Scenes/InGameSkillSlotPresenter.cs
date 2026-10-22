using GameCommon.Table;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using UnityEngine.UI;
using VContainer;

public class InGameSkillSlotPresenter : BaseSlotPresenter<InGameSkillSlotModel>
{
	[View]
	protected InGameSkillSlotView _view;

	[InjectModel]
	protected InGameSkillSlotModel _model;

	[Inject]
	protected AssetManager AssetManager;

	[Inject]
	protected GameManager GameManager;

	private const string STACK_TEXT_COLOR = "#B5B5B5";

	private const string STACK_TEXT_NOT_RECOVERY_COLOR = "#A45646";

	private const float CLEAR_EFFECT_DURATION = 0.5f;

	private bool _isSkillUsable;

	private float _changeSkillGaugeMax;

	protected override InGameSkillSlotModel SlotModel => null;

	protected override Button Button => null;

	private TSkill SkillData => null;

	protected virtual void OnInject(InGameSkillSlotModel model)
	{
	}

	protected virtual void FixedUpdate()
	{
	}

	private void OnHpUpdated(int hp)
	{
	}

	private void OnStressUpdated(float stress)
	{
	}

	private void OnStaminaUpdated(float stamina)
	{
	}

	protected void UpdateCoolTime()
	{
	}

	protected void UpdateChargeGauge()
	{
	}

	protected void UpdateStack()
	{
	}

	protected void UpdateSkillUsable()
	{
	}

	protected void PlayClearEffect()
	{
	}

	private void UpdateChangeSkillGauge()
	{
	}
}
