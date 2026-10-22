using NemoLib.AssetManagement;
using NemoLib.Mvp;
using VContainer;

public class SkillSlotPresenter : BaseSlotPresenter<SkillSlotModel>
{
	[View]
	private SkillSlotView _view;

	[InjectModel]
	private SkillSlotModel _model;

	[Inject]
	private AssetManager _assetManager;

	protected override SkillSlotModel SlotModel => null;

	protected override void Start()
	{
	}

	protected override void OnSelected(bool isSelected)
	{
	}

	protected override void OnLocked(bool isLocked)
	{
	}

	private void OnInject(SkillSlotModel model)
	{
	}

	private void OnSkillChanged(int skillId)
	{
	}
}
